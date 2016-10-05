#pragma strict

var rockText : UI.Text;

function OnTriggerEnter(col : Collider) {
    if(col.gameObject.tag == "Player") {    
        if(!GameObject.Find("findDynamite").GetComponent.<dynamite>().hasDynamite){
            rockText.text = "You need dynamite to clear the rock before you can continue.";
        }
        else {
            rockText.text = "You plant the dynamite. The rock explodes! Continue to the next level";
            Destroy(gameObject);
        }
    }
}

    function OnTriggerExit(col : Collider) {
        if(col.gameObject.tag == "Player") {
            rockText.text = "";
        }
    }