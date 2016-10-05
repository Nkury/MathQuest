#pragma strict

var banditBossText : UI.Text;
public static var enableDynamite : boolean = false;
var banditBossScene : boolean = false;


function OnTriggerEnter(col : Collider) {
    if(col.gameObject.tag == "Player") {
        if(banditBossScene == false){   
            banditBossScene = true;
            enableDynamite = true;
            
        }
        else if (!GameObject.Find("findDynamite").GetComponent.<dynamite>().hasDynamite) {
        }
        else {
        }
    }
}

    function OnTriggerExit(col : Collider) {
        if(col.gameObject.tag == "Player") {
            banditBossText.text = "";
        }
    }