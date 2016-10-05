#pragma strict

var dynamiteText : UI.Text;
public static var hasDynamite : boolean = false;

function OnTriggerEnter(col : Collider) {
    if(col.gameObject.tag == "Player") {
       
        if(!GameObject.Find("banditBoss").GetComponent.<banditBoss>().enableDynamite){
            dynamiteText.text = "You must talk to the bandit boss first.";
        }
        else {
            dynamiteText.text = "Oh no! Watch out!";
            hasDynamite = true;
        }
    }
}

    function OnTriggerExit(col : Collider) {
        if(col.gameObject.tag == "Player") {
            dynamiteText.text = "";
        }
    }


        //!GameObject.Find("banditBoss").GetComponent<banditBoss>().enableDynamite