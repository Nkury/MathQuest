// Programmer:  Nizar Kury
// Date:        10/10/2015
// Description: The Battle System calculates random values for three actions presented on screen where two of the
//              action's sum equals the health of the enemy. The Battle System is presented in the battle scene
//              where interacting with an object in the scene beforehand leads to an instance battle with set
//              conditions. For example, colliding with a slime leads to a slime battle where the slime typically
//              has less than five health for easy arithmetic battles. Upon victory, the instance is saved, deleting
//              the enemy in the previous scene and giving the player a medal. A post-battle screen will also pop up
//              giving the player a summation of their actions during the battle. Animations are also present depending
//              on what enemy was collided with beforehand.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{

    public GameObject enemyHealthObject; // Game object responsible for displaying enemy health

    public GameObject slimeModel;   // Game object representing the slime prefab
    public GameObject banditModel;  // Game object representing the bandit prefab
    public GameObject trollModel;   // Game object representing the troll prefab
    public GameObject playerModel;
    public GameObject kittenModel;
    public GameObject player;
    public GameObject playerRef;
    public GameObject enemyRef;

    public GameObject playerHealthBar;
    public GameObject enemyHealthBar;
    public GameObject A1_Button;
    public GameObject A2_Button;
    public GameObject A3_Button;
    public GameObject Attack_Button;
    public GameObject postBattle;
    public GameObject tutorial;
    public GameObject gameOverCube;

    // holds the numbers of the buttons selected by the player. If none are selected, it remains at -1.

    public int enemyHealth;	// the amount of health the enemy has
    private int randHealth; // this variable stores the passed in random number (depending on what enemy the player runs into)
    public int playerHealth; // this variable stores the number of hearts the player has
    private int EnemyType;  // type of enemy (1 = slime, 2 = bandit, 3 = troll)
    private int trollCount = 3; // 3 times to beat troll

    // variables used for flickering of player sprite
    private bool damageTaken = false;
    private bool playerAttack = false;
    private bool spacePress = false;
    private int interval = 0;
    private bool firstTutorial = true;
    private List<int> actions = new List<int>();


    // Use this for initialization
    void Start()
    {
        playerHealth = 3;
        player = GameObject.Find("Cha_Knight");
        playerHealthBar.GetComponent<HealthBarManager>().InitHealthBar(playerRef, playerHealth);
        enemyHealth = PlayerPrefs.GetInt("enemy_health");
        int monsterType = PlayerPrefs.GetInt("monster_type");
        setBattle(enemyHealth, monsterType);
        GameObject battleGUI = GameObject.Find("BattleGUI");
        battleGUI.GetComponent<ButtonManager>().AttackButtonClickedEvent += setButtons;
        assignButtons();
        GameObject mainCamera = GameObject.Find("Main Camera");
        AudioSource[] audios = mainCamera.GetComponents<AudioSource>();
        if (EnemyType != 3)
        {
            audios[2].Play();
        }
        else
        {
            audios[6].Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        interval++;

        enemyHealthObject.GetComponent<TextMesh>().text = enemyHealth.ToString();
    
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

        // victory conditions. Since I haven't gotten the buttons to work out, just press P to win and test it out.
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject zone1storage = GameObject.Find("PersistentZone1");
            zone1storage.GetComponent<Zone1Start>().destroyList.Add(PlayerPrefs.GetString("monster_name"));
            Application.LoadLevel("final Zone #1.2");
        }

        if (playerHealth <= 0)
            StartCoroutine(gameOverScreen());

        if (EnemyType == 4 && interval % 50 == 0 && trollCount == 3 && firstTutorial)
        {
            tutorial.GetComponent<BattleTutorial>().startTutorial();
            firstTutorial = false;
        }
        else if (EnemyType == 4 && interval % 500 == 0 && trollCount == 3)
        {
            tutorial.GetComponent<BattleTutorial>().startTutorial();
        }

        if (trollCount < 3)
            tutorial.SetActive(false);
    }

    public void assignButtons()
    {
        if (EnemyType == 4 && trollCount == 3)
        {
            int attack1 = 1;
            int attack2 = 1;
            int attack3 = 2;
            int[] attacks = { attack1, attack2, attack3 };
            GameObject battleGUI = GameObject.Find("BattleGUI");
            battleGUI.GetComponent<ButtonManager>().InitActionButtons(attacks);
        }
        else
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
                    attack3 = Random.Range(enemyHealth % 10-1, 10);
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
    }

    public void setButtons(int[] values)
    {
        tutorial.SetActive(false);
        // attack misses and player suffers one heart damage
        if (values[0] + values[1] > enemyHealth)
        {
            StartCoroutine(AttackAndGotHit(true, 0, 0));
            assignButtons();
        }
        else
        {
            enemyHealth -= values[0] + values[1];
            actions.Add(values[0]);
            actions.Add(values[1]);
            if (enemyHealth > 0)
            {
                assignButtons();
                StartCoroutine(AttackAndGotHit(false, values[0], values[1]));
            }
            else
            {
                enemyHealth = 0;
                StartCoroutine(AttackAndKill());
            }
        }
    }

    // method takes in enemy health and type of enemy. For example, if enemy is a slime, then 
    // the enemyType parameter would be a 1. Refer to the content of the method to see the various
    // enemy models
    public void setBattle(int initialHealth, int enemyType)
    {
        enemyHealth = initialHealth;
        EnemyType = enemyType;

        // SLIME
        if (enemyType == 1)
        {
            Instantiate(slimeModel, new Vector3(111.51f, -.2f, 29.43f), new Quaternion());
        }
        // BANDIT
        else if (enemyType == 2)
        {
            Instantiate(banditModel, new Vector3(109.51f, -.2f, 26.36f), banditModel.transform.rotation);
        }
        // TROLL
        else if (enemyType == 3)
        {
            GameObject mainCamera = GameObject.Find("Main Camera");
            AudioSource[] audios = mainCamera.GetComponents<AudioSource>();
            Instantiate(trollModel, new Vector3(110.1894f, 0, 27.26139f), trollModel.transform.rotation);
            audios[4].Play();
            audios[5].Play();
        }
        else if (enemyType == 4)
        {
            Instantiate(kittenModel, new Vector3(109.2989f, -4.768372e-07f, 26.37297f), kittenModel.transform.rotation);
          //  tutorial.GetComponent<BattleTutorial>().startTutorial();
        }

        enemyHealthBar.GetComponent<HealthBarManager>().InitHealthBar(enemyRef, enemyHealth);
    }

    IEnumerator gameOverScreen()
    {

        if (EnemyType != 4)
        {
            // enable game over screen in future
            GameObject zone1storage = GameObject.Find("PersistentZone1");
            // have all the enemies respawn
            if (zone1storage.GetComponent<Zone1Start>().destroyList.Count > 0)
                zone1storage.GetComponent<Zone1Start>().destroyList.Clear();
            Destroy(zone1storage);
            // start the player back at the starting position
            PlayerPrefs.SetFloat("player_x", 15.3f);
            PlayerPrefs.SetFloat("player_y", 10.35779f);
            PlayerPrefs.SetFloat("player_z", 32.2f);
            gameOverCube.SendMessage("startFadeOut");
            playerModel.GetComponent<Renderer>().enabled = false;
            damageTaken = false;
            yield return new WaitForSeconds(3);
            Application.LoadLevel("final Zone #1.2");
        }
        else
        {
            gameOverCube.SendMessage("startFadeOut");
            playerModel.GetComponent<Renderer>().enabled = false;
            damageTaken = false;
            yield return new WaitForSeconds(3);
            Application.LoadLevel("TutorialTown");
        }
    }

    IEnumerator AttackAndGotHit(bool missed, int attack1, int attack2)
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        AudioSource[] audios = mainCamera.GetComponents<AudioSource>();
        GameObject enemy;
        A1_Button.SetActive(false);
        A2_Button.SetActive(false);
        A3_Button.SetActive(false);
        Attack_Button.SetActive(false);
        playerAttack = true;
        if (missed)
        {
            audios[8].Play();
        }
        yield return new WaitForSeconds(.5f);
        player.GetComponent<Animation>().Play("Attack");
      
        yield return new WaitForSeconds(.5f);
        playerAttack = false;
        if (EnemyType == 1 && !missed)
        {
            enemy = GameObject.Find("Cha_Slime battle (Clone)");
            enemy.GetComponent<Animation>().Play("Damage");
        }
     

        if (!missed)
        {
            audios[1].Play();
            audios[2].volume = .15f;
            enemyHealthBar.GetComponent<HealthBarManager>().OverrideSpriteFor(attack1 + attack2);
        }

        yield return new WaitForSeconds(.5f);
        audios[2].volume = .97f;
        yield return new WaitForSeconds(1);

        if (EnemyType == 1)
        {
            enemy = GameObject.Find("Cha_Slime battle (Clone)");
            enemy.GetComponent<Animation>().Play("Attack");
        }
        else if (EnemyType == 2)
        {
            enemy = GameObject.Find("Battle jack(Clone)");
            enemy.GetComponent<Animation>().Play("Lumbering");
        }
        else if (EnemyType == 3)
        {
            // play animation for troll if we make one
            playerHealth = 1; // will later be deduced by 1 to equal 0
            playerHealthBar.GetComponent<HealthBarManager>().OverrideSpriteFor(2);
            audios[4].Play();
        }
        else if (EnemyType == 4)
        {
            enemy = GameObject.Find("KittenBattle(Clone)");
            kittenModel.GetComponent<Animation>().Play("Jump");
        }
        yield return new WaitForSeconds(.5f);
        player.GetComponent<Animation>().Play("Damage");

        audios[0].Play(); // OUCH sound effect
        audios[1].Play(); // damage sound effect
        audios[2].volume = .17f;
        playerHealth--;
        playerHealthBar.GetComponent<HealthBarManager>().OverrideSpriteFor(1);
        //yield return new WaitForSeconds(.5f);
        if (EnemyType == 1)
        {
            enemy = GameObject.Find("Cha_Slime battle (Clone)");
            enemy.GetComponent<Animation>().Play("Wait");
        }
        else if (EnemyType == 2)
        {
            enemy = GameObject.Find("Battle jack(Clone)");
            enemy.GetComponent<Animation>().Play("Idle");
        }
        else if (EnemyType == 3)
        {
            // play animation for troll if we make one
        }
        yield return new WaitForSeconds(.5f);
        audios[2].volume = .95f;
        if (playerHealth == 1)
            audios[2].pitch = 1.5f;
        damageTaken = true;
        yield return new WaitForSeconds(3);
        damageTaken = false;
        A1_Button.SetActive(true);
        A2_Button.SetActive(true);
        A3_Button.SetActive(true);
        Attack_Button.SetActive(true);
        tutorial.SetActive(true);
    }

    // this method is called when the attack kills the target
    IEnumerator AttackAndKill()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        AudioSource[] audios = mainCamera.GetComponents<AudioSource>();
        GameObject enemy;
        A1_Button.SetActive(false);
        A2_Button.SetActive(false);
        A3_Button.SetActive(false);
        Attack_Button.SetActive(false);
        playerAttack = true;
        player.GetComponent<Animation>().Play("Attack");
        yield return new WaitForSeconds(.5f);
        playerAttack = false;
        if (EnemyType == 1)
        {
            enemy = GameObject.Find("Cha_Slime battle (Clone)");
            enemy.GetComponent<Animation>().Play("Dead");
        } 
        else if (EnemyType == 3)
        {
            enemy = GameObject.Find("Mean Looking Troll(Clone)");
            audios[4].Play();
            trollCount--;
            enemyHealthBar.GetComponent<HealthBarManager>().InitHealthBar(enemyRef, 0);
            yield return new WaitForSeconds(.5f);
            if (trollCount == 2)
                enemyHealth = Random.Range(7, 11);
            if (trollCount == 1)
                enemyHealth = 14;
            enemyHealthBar.GetComponent<HealthBarManager>().InitHealthBar(enemyRef, enemyHealth);
            if (trollCount != 0)
            {
                enemy.transform.localScale += new Vector3(.2f, .2f, .2f);
                assignButtons();
                A1_Button.SetActive(true);
                A2_Button.SetActive(true);
                A3_Button.SetActive(true);
                Attack_Button.SetActive(true);
            }
        }
        else if (EnemyType == 4)
        {
            enemy = GameObject.Find("KittenBattle(Clone)");
            // play meow sound
            trollCount--;
            enemyHealthBar.GetComponent<HealthBarManager>().InitHealthBar(enemyRef, 0);
            yield return new WaitForSeconds(.5f);
            if (trollCount == 2)
            {
                enemyHealth = 3;
            }
            else if (trollCount == 1)
            {
                enemyHealth = 4;
            }
            enemyHealthBar.GetComponent<HealthBarManager>().InitHealthBar(enemyRef, enemyHealth);
            if (trollCount != 0)
            {
                assignButtons();
                A1_Button.SetActive(true);
                A2_Button.SetActive(true);
                A3_Button.SetActive(true);
                Attack_Button.SetActive(true);
            }
        }
        tutorial.SetActive(true);
        audios[1].Play();
        if ((EnemyType != 3 && EnemyType != 4) || trollCount == 0)
        {
            audios[2].volume = 0;
            audios[6].volume = 0;
            audios[3].Play();
            enemyHealthBar.GetComponent<HealthBarManager>().InitHealthBar(enemyRef, 0);
            yield return new WaitForSeconds(2);
            if (EnemyType == 1)
            {
                enemy = GameObject.Find("Cha_Slime battle (Clone)");
                Destroy(enemy);
            }
            else if (EnemyType == 2)
            {
                enemy = GameObject.Find("Battle jack(Clone)");
                Destroy(enemy);
            }
            else if (EnemyType == 3)
            {
                enemy = GameObject.Find("Mean Looking Troll(Clone)");
                Destroy(enemy);
                audios[5].Play();
            }
            else if (EnemyType == 4)
            {
                enemy = GameObject.Find("KittenBattle(Clone)");
                Destroy(enemy);
                QuestDelegate.startQuestForZone(1);
            }

            yield return new WaitForSeconds(2);
            int[] playerActions = new int[actions.Count];
            for (int i = 0; i < actions.Count; i++)
                playerActions[i] = actions[i];

            if (playerHealth == 3)
            {
                postBattle.GetComponent<PostBattleMenu>().InitPostBattleMenuWith(3, playerActions);
                QuestLogManager.masteryQuestStatusUpdated(1);
            }
            else if (playerHealth == 2)
                postBattle.GetComponent<PostBattleMenu>().InitPostBattleMenuWith(2, playerActions);
            else if (playerHealth == 1)
                postBattle.GetComponent<PostBattleMenu>().InitPostBattleMenuWith(1, playerActions);
       
        }
    }

    // upon victory, we save who dies through a persistent game object called zone1storage and store
    // the name of the monster to destroy at the start of the scene
    public void victory()
    {
        if (EnemyType != 4)
        {
            GameObject zone1storage = GameObject.Find("PersistentZone1");
            zone1storage.GetComponent<Zone1Start>().destroyList.Add(PlayerPrefs.GetString("monster_name"));
            Application.LoadLevel("final Zone #1.2");
        }
        else
        {
            cat.beaten = true;
            Application.LoadLevel("TutorialTown");
        }
    }
}
