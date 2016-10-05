#pragma strict

var wall1Hint : UI.Text;

function OnTriggerEnter(col : Collider) {
    if(col.gameObject.tag == "Player") {
        wall1Hint.text = "Don't be a coward!\nGo rescue your mother!";
    }
}

    function OnTriggerExit(col : Collider) {
        if(col.gameObject.tag == "Player") {
            wall1Hint.text = "";
        }
    }



/*var guiShow : boolean = false;
var text : UI.Text;;

function OnTriggerEnter (Col : Collider) {          //on trigger stay?
    if(Col.gameObject.tag == "Player")
    {
        guiShow = true;
    }
}

    function OnTriggerExit (Col : Collider) {
        if(Col.gameObject.tag == "Player")
        {
            guiShow = false;
        }
    }

        function onGUI()
        {
            if(guiShow == true) {
                GUI.DrawTexture(Rect(Screen.width / 4.5, Screen.height / 4, 512, 512), text);
                }
        }
        */