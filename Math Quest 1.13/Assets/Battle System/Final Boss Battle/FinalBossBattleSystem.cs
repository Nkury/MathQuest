// Programmer:  Nizar Kury
// Date:        11/15/2015
// Description: Because the Final Boss does not adhere to the current battle system, a separate script is made
//              for his unique battle style. The Final Boss has five phases with multiple health ranges from
//              0 - 18. In between most of his phases, the final boss also performs a web shot, which is a
//              high stakes, powerful move that the player must stop by clicking the two numbers that
//              add up to the power of the charge shot before time runs out. Upon defeat, an animation is played
//              and the player is led to the credits screen.   

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FinalBossBattleSystem : MonoBehaviour
{  
    public GameObject webShotObject; // Game object responsible for displaying web shot

    public GameObject explosion; // explosion particle effect

    public GameObject boss;     // the boss model object
    public GameObject playerModel; // the player model object
    public GameObject player;      // the player controller object
    public GameObject playerRef;   // reference point of the player in terms of health
    public GameObject enemyRef;    // enemy reference point in terms of health

    public GameObject playerHealthBar; // player health bar
    public GameObject enemyHealthBar; // enemy health bar
    public GameObject A1_Button; // action 1 button
    public GameObject A2_Button; // action 2 button
    public GameObject A3_Button; // action 3 button
    public GameObject Attack_Button; // attack button

    public GameObject postBattle; // post-battle screen that appears showing all actions done
    private List<int> actions = new List<int>(); // a list of actions performed by the player for the post-battle screen

    // holds the numbers of the buttons selected by the player. If none are selected, it remains at -1.
    public int enemyHealth;	// the amount of health the enemy has
    private int randHealth; // this variable stores the passed in random number (depending on what enemy the player runs into)
    public int playerHealth; // this variable stores the number of hearts the player has

    // variables used for flickering of player sprite
    private bool damageTaken = false;
    private bool playerAttack = false;
    private bool bossWait = true;
    private bool spacePress = false;
    private int interval = 0;
    private int bossPhases = 1;
    private bool chargeShot = false;
    private AudioSource[] audios;
    public GameObject[] spiderExplosions;
    public GameObject gameOverCube;

    // Use this for initialization
    void Start()
    {
        playerHealth = 3;
        player = GameObject.Find("Cha_Knight");
        playerHealthBar.GetComponent<HealthBarManager>().InitHealthBar(playerRef, playerHealth);
        enemyHealth = Random.Range(7, 11);
        enemyHealthBar.GetComponent<HealthBarManager>().InitHealthBar(enemyRef, enemyHealth);
        GameObject battleGUI = GameObject.Find("BattleGUI");
        battleGUI.GetComponent<ButtonManager>().AttackButtonClickedEvent += setButtons;
        assignButtons();
        GameObject mainCamera = GameObject.Find("Main Camera");
        audios = mainCamera.GetComponents<AudioSource>();
        audios[2].Play();
    }

    // Update is called once per frame
    void Update()
    {
        interval++;

        // if damage is taken, this handles the "blinking" effect of getting hit
        if (damageTaken)
        {
            if (interval % 5 == 0)
                playerModel.GetComponent<SkinnedMeshRenderer>().enabled = !playerModel.GetComponent<SkinnedMeshRenderer>().enabled;
        }
        else if (!playerAttack && !damageTaken)
        {
            playerModel.GetComponent<SkinnedMeshRenderer>().enabled = true;
            player.GetComponent<Animation>().Play("Wait");
        }

        // if boss is waiting for the player to select actions, then the animation that should be playing idle;
        // otherwise, play the stationary run animation
        if (bossWait)
            boss.GetComponent<Animation>().Play("Idle");
        else if (chargeShot)
            boss.GetComponent<Animation>().Play("Run");

        // this section deals with the length of time it takes for the charge shot to execute. Depending on what phase the boss is in,
        // the time it will take for the player to counter the spider's charge attack gets reduced over each phase. This handles the 
        // number that pops up on screen and slowly dissolves.
        if (webShotObject.transform.localScale.x > 0 && webShotObject.GetComponent<Renderer>().enabled)
        {
            if(bossPhases == 2)
            webShotObject.transform.localScale = new Vector3(webShotObject.transform.localScale.x - .025f, webShotObject.transform.localScale.y - .025f,
                                           webShotObject.transform.localScale.z - .025f);
            else if (bossPhases == 4)
            {
                webShotObject.transform.localScale = new Vector3(webShotObject.transform.localScale.x - .035f, webShotObject.transform.localScale.y - .035f,
                                           webShotObject.transform.localScale.z - .035f);
            }
            else if (bossPhases == 6)
            {
                webShotObject.transform.localScale = new Vector3(webShotObject.transform.localScale.x - .045f, webShotObject.transform.localScale.y - .045f,
                                           webShotObject.transform.localScale.z - .045f);
            }
            else if (bossPhases == 8)
            {
                webShotObject.transform.localScale = new Vector3(webShotObject.transform.localScale.x - .06f, webShotObject.transform.localScale.y - .06f,
                                           webShotObject.transform.localScale.z - .06f);
            } 
        }
        // else, we check if we need to charge shot and start the procedure here
        else if (webShotObject.transform.localScale.x < 0  && chargeShot)
        {
            webShotObject.transform.localScale = new Vector3(10, 10, 10);
            A1_Button.SetActive(false);
            A2_Button.SetActive(false);
            A3_Button.SetActive(false);
            Attack_Button.SetActive(false);
            webShotObject.GetComponent<Renderer>().enabled = false;
            if(enemyHealth != 0)
                StartCoroutine(GetWebShotted());
            bossWait = true;
        }

        // victory conditions. Since I haven't gotten the buttons to work out, just press P to win and test it out.
        if (Input.GetKeyDown(KeyCode.P) || (Input.GetKeyDown(KeyCode.Space) && spacePress))
        {
            GameObject zone1storage = GameObject.Find("PersistentZone1");
            zone1storage.GetComponent<Zone1Start>().destroyList.Add(PlayerPrefs.GetString("monster_name"));
            Application.LoadLevel("final Zone #1");
        }

        if (playerHealth <= 0)
            StartCoroutine(gameOverScreen());
    }

    public void assignButtons()
    {
        // first attack is a random number between 1 and enemy health - 1 (since max is exclusive)
        int attack1;
        if (enemyHealth <= 9)
            attack1 = Random.Range(1, enemyHealth);
        else
            attack1 = Random.Range(enemyHealth % 10 + 1, 10);

        // second attack is the difference of the enemy health and attack1
        int attack2 = enemyHealth - attack1;

        // third attack is a random number between 1 and enemyHealth + 1 and makes sure it's not the same as the other
        // numbers (I set it to attack1 so that way we can start the following while loop)
        int attack3 = attack1;

        // keep finding a number for attack3 that is not the same as either attack1 or 2 and its sum with either 
        // attack1 or 2 will not lead to the enemy having 1 health.
        // int count = 0;
        while ((attack3 == attack1 || attack3 == attack2) || ((attack1 + attack3 == (enemyHealth - 1)) ||
                (attack2 + attack3 == (enemyHealth - 1))))
        {
            if (enemyHealth <= 9)
                attack3 = Random.Range(1, enemyHealth + 1);
            else
            {
                attack3 = Random.Range(enemyHealth % 10-2, 10);
                if (attack3 <= 0)
                {
                    attack3 = attack1;
                }
            }       
        }
       
        int rand = Random.Range(1, 7);

        if (rand < 3)
        {

        }
        else if (rand >= 3 && rand < 5)
        {
            int temp = attack1;
            attack1 = attack2;
            attack2 = attack3;
            attack3 = temp;
        }
        else
        {
            int temp = attack2;
            attack2 = attack1;
            attack1 = attack3;
            attack3 = temp;
        }
        int[] attacks = { attack1, attack2, attack3 };
        GameObject battleGUI = GameObject.Find("BattleGUI");
        battleGUI.GetComponent<ButtonManager>().InitActionButtons(attacks);
    }

    // set the buttons with the values set by AssignButtons
    public void setButtons(int[] values)
    {
        // attack misses and player suffers one heart damage
        if (values[0] + values[1] > enemyHealth)
        {
            if (!chargeShot)
                StartCoroutine(AttackAndGotHit(true, 0, 0));
            else
                StartCoroutine(GetWebShotted());
            assignButtons();
        }
        else
        {
            enemyHealth -= values[0] + values[1];

            if (enemyHealth > 0)
            {
                assignButtons();
                if (!chargeShot)
                    StartCoroutine(AttackAndGotHit(false, values[0], values[1]));
                else
                    StartCoroutine(GetWebShotted());
            }
            else
            {
                enemyHealth = 0;
                StartCoroutine(AttackAndKill(values[0], values[1]));    
            }
        }
    }

    IEnumerator gameOverScreen()
    {
        gameOverCube.SendMessage("startFadeOut");
        playerModel.GetComponent<Renderer>().enabled = false;
        damageTaken = false;
        yield return new WaitForSeconds(3);
        Application.LoadLevel("final boss");
    }

    IEnumerator SpiderChargeShot()
    {
        chargeShot = true;
        webShotObject.transform.localScale = new Vector3(10, 10, 10);

        yield return new WaitForSeconds(2);
        enemyHealth = Random.Range(1, 10);
        webShotObject.GetComponent<TextMesh>().text = enemyHealth.ToString();        
        assignButtons();
        webShotObject.GetComponent<Renderer>().enabled = true;
        A1_Button.SetActive(true);
        A2_Button.SetActive(true);
        A3_Button.SetActive(true);
        Attack_Button.SetActive(true);
    }

    IEnumerator GetWebShotted()
    {
        A1_Button.SetActive(false);
        A2_Button.SetActive(false);
        A3_Button.SetActive(false);
        Attack_Button.SetActive(false);

        if (chargeShot)
        {
            webShotObject.transform.localScale = new Vector3(-.003f, 0, 0);
        }

        chargeShot = false;

        yield return new WaitForSeconds(.5f);
        audios[2].volume = .97f;
        audios[5].Play();
        yield return new WaitForSeconds(1);

        bossWait = false;

        yield return new WaitForSeconds(.5f);
        explosion.SetActive(true);
        player.GetComponent<Animation>().Play("Damage");

        audios[0].Play(); // OUCH sound effect
        audios[1].Play(); // damage sound effect
        audios[2].volume = .17f;
        if (playerHealth == 3)
        {
            playerHealth -= 2;
            playerHealthBar.GetComponent<HealthBarManager>().OverrideSpriteFor(2);
        }
        else
        {
            playerHealth = 0;
            playerHealthBar.GetComponent<HealthBarManager>().OverrideSpriteFor(1);
        }

        yield return new WaitForSeconds(.5f);
        audios[2].volume = .95f;
        if (playerHealth == 1)
            audios[2].pitch = 1.3f;
        damageTaken = true;
        yield return new WaitForSeconds(3);
        damageTaken = false;
        A1_Button.SetActive(true);
        A2_Button.SetActive(true);
        A3_Button.SetActive(true);
        Attack_Button.SetActive(true);
        bossWait = true;
        explosion.SetActive(false);
        setPhases();
    }

    IEnumerator AttackAndGotHit(bool missed, int attack1, int attack2)
    {
        A1_Button.SetActive(false);
        A2_Button.SetActive(false);
        A3_Button.SetActive(false);
        Attack_Button.SetActive(false);

        playerAttack = true;
        if (missed)
        {
            audios[7].Play();
        }
        yield return new WaitForSeconds(.5f);
        player.GetComponent<Animation>().Play("Attack");
        yield return new WaitForSeconds(.5f);
        playerAttack = false;

        if (!missed)
        {
            audios[1].Play();
            audios[2].volume = .15f;
            enemyHealthBar.GetComponent<HealthBarManager>().OverrideSpriteFor(attack1 + attack2);
            actions.Add(attack1);
            actions.Add(attack2);
        }

        yield return new WaitForSeconds(.5f);
        audios[2].volume = .97f;
        yield return new WaitForSeconds(1);

        bossWait = false;
        chargeShot = false;

        boss.GetComponent<Animation>().Play("Attack");

        yield return new WaitForSeconds(.5f);
        player.GetComponent<Animation>().Play("Damage");
        audios[4].Play();

        audios[0].Play(); // OUCH sound effect
        audios[1].Play(); // damage sound effect
        audios[2].volume = .17f;
        if (playerHealth == 3)
        {
            playerHealth -= 2;
            playerHealthBar.GetComponent<HealthBarManager>().OverrideSpriteFor(2);
        }
        else
        {
            playerHealth = 0;
            playerHealthBar.GetComponent<HealthBarManager>().OverrideSpriteFor(1);
        }

        //yield return new WaitForSeconds(.5f);

        yield return new WaitForSeconds(.5f);
        audios[2].volume = .95f;
        if (playerHealth == 1)
            audios[2].pitch = 1.3f;
        damageTaken = true;
        yield return new WaitForSeconds(3);
        damageTaken = false;
        A1_Button.SetActive(true);
        A2_Button.SetActive(true);
        A3_Button.SetActive(true);
        Attack_Button.SetActive(true);
        bossWait = true;
    }

    IEnumerator AttackAndKill(int attack1, int attack2)
    {
        A1_Button.SetActive(false);
        A2_Button.SetActive(false);
        A3_Button.SetActive(false);
        Attack_Button.SetActive(false);
        playerAttack = true;
        player.GetComponent<Animation>().Play("Attack");
        yield return new WaitForSeconds(.5f);
        playerAttack = false;
        audios[1].Play();

        if (chargeShot)
        {
            webShotObject.transform.localScale = new Vector3(-.003f, 0, 0);
        }
        actions.Add(attack1);
        actions.Add(attack2);

        yield return new WaitForSeconds(.3f);
        chargeShot = false;
        bossWait = true;

        enemyHealthBar.GetComponent<HealthBarManager>().InitHealthBar(enemyRef, 0);
        setPhases();


        if (bossPhases == 10)
        {
            bossWait = false;
            audios[2].volume = 0;
            audios[3].Play();
            audios[5].Play();
            boss.GetComponent<Animation>().Play("Death");
            spiderExplosions[0].SetActive(true);
            yield return new WaitForSeconds(.25f);
            spiderExplosions[1].SetActive(true);
            yield return new WaitForSeconds(.25f);
            spiderExplosions[2].SetActive(true);
            yield return new WaitForSeconds(.25f);
            spiderExplosions[3].SetActive(true);
            yield return new WaitForSeconds(2);
            boss.SetActive(false);

            int[] playerActions = new int[actions.Count];
            for (int i = 0; i < actions.Count; i++)
                playerActions[i] = actions[i];

            if (playerHealth == 3)
                postBattle.GetComponent<PostBattleMenu>().InitPostBattleMenuWith(3, playerActions);
            else if (playerHealth == 2)
                postBattle.GetComponent<PostBattleMenu>().InitPostBattleMenuWith(2, playerActions);
            else if (playerHealth == 1)
                postBattle.GetComponent<PostBattleMenu>().InitPostBattleMenuWith(1, playerActions);

        }

    }

    void setPhases()
    {
        
        bossPhases++;
        Debug.Log("Boss phases: " + bossPhases);
        if (bossPhases % 2 == 0 && bossPhases != 10)
        {
            bossWait = false;
           
            enemyHealthBar.GetComponent<HealthBarManager>().OverrideSpriteFor(enemyHealth);
            StartCoroutine(SpiderChargeShot());

        }
        if (bossPhases == 3)
        {
            enemyHealth = Random.Range(11, 16);
            enemyHealthBar.GetComponent<HealthBarManager>().InitHealthBar(enemyRef, enemyHealth);
            assignButtons();
            A1_Button.SetActive(true);
            A2_Button.SetActive(true);
            A3_Button.SetActive(true);
            Attack_Button.SetActive(true);
        }
        if (bossPhases == 5 || bossPhases == 7)
        {
            enemyHealth = Random.Range(16, 18);
            enemyHealthBar.GetComponent<HealthBarManager>().InitHealthBar(enemyRef, enemyHealth);
            assignButtons();
            A1_Button.SetActive(true);
            A2_Button.SetActive(true);
            A3_Button.SetActive(true);
            Attack_Button.SetActive(true);
        }
        if (bossPhases == 9)
        {
            enemyHealth = 18;
            enemyHealthBar.GetComponent<HealthBarManager>().InitHealthBar(enemyRef, enemyHealth);
            assignButtons();
            A1_Button.SetActive(true);
            A2_Button.SetActive(true);
            A3_Button.SetActive(true);
            Attack_Button.SetActive(true);
        }
    }

    public void finishGame()
    {
        Application.LoadLevel("congratulations");
    }
}
