#pragma strict
var levelToLoad : String;

function Start() {
}
function OnTriggerEnter(col : Collider) {
    if(col.gameObject.tag == "Player") { 
        Application.LoadLevel(levelToLoad);
    }
}
   
