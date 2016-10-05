 #pragma strict
 
 public var theCamera : Transform;
 
 private var movementSpeed : float = 5.0;
 private var rotationSpeed : float = 5.0;
 private var currentRotation : float = 0.0;
 
 private var myTransform : Transform;
 private var cameraHolder : Transform;
 private var anim : Animation;
 private var shiftHeld = false;
 
 function Start() 
 {
     myTransform = this.transform;
     cameraHolder = transform.Find( "CameraHolder" );
     anim = GetComponent.<Animation>();
 }
 
 function Update() 
 {
     // get inputs
     var inputX : float = Input.GetAxis( "Horizontal" );
     var inputY : float = Input.GetAxis( "Vertical" );
     var inputR : float = Mathf.Clamp( Input.GetAxis( "Mouse X" ), -1.0, 1.0 );
     // press 'Q' to turn 180 degrees
     if ( Input.GetKeyDown(KeyCode.Q) )
     {
         currentRotation += 180.0;
     }
     	if(Input.GetButton("Run"))
     		shiftHeld = true;
     	else
     		shiftHeld = false;
     		

     	if(Input.GetAxis("Horizontal") || Input.GetAxis("Vertical")){
     		 GetComponent.<Animation>().Play("Walk");
     		 if(shiftHeld){
     		 	movementSpeed = 10.0f;
     		 	anim["Walk"].speed = 2.0f;	
     		 } else{  
     		 	movementSpeed = 5.0f;
	         	anim["Walk"].speed = 1.0f;	     		 
     		 }
	   	} else
     		GetComponent.<Animation>().Play("Wait");
     		/*
     if((Input.GetAxis("Horizontal") || Input.GetAxis("Vertical")) && Input.GetKeyDown(KeyCode.LeftShift)){
         GetComponent.<Animation>().Play("Walk");
         movementSpeed = 8.0f;
         anim["Walk"].speed = 2.0f;
     } else if (Input.GetAxis("Horizontal") || Input.GetAxis("Vertical")){
     	 GetComponent.<Animation>().Play("Walk");
         movementSpeed = 5.0f;
         anim["Walk"].speed = 1.0f;
     } else
     	GetComponent.<Animation>().Play("Wait"); */
     	
     // get current position and rotation, then do calculations
     // position
     var moveVectorX : Vector3 = myTransform.forward * inputY;
     var moveVectorY : Vector3 = myTransform.right * inputX;
     var moveVector : Vector3 = ( moveVectorX + moveVectorY ).normalized * movementSpeed * Time.deltaTime;
     
    
     // rotation
     currentRotation = ClampAngle( currentRotation + ( inputR * rotationSpeed ) );
     var rotationAngle : Quaternion = Quaternion.Euler( 0.0, currentRotation, 0.0 );
     
     // update Character position and rotation
     myTransform.position = myTransform.position + moveVector;
     myTransform.rotation = rotationAngle;
     
     // update Camera position and rotation
     theCamera.position = cameraHolder.position;
     theCamera.rotation = cameraHolder.rotation;
 }
 
 
 function ClampAngle( theAngle : float ) : float 
 {
     if ( theAngle < -360.0 )
     {
         theAngle += 360.0;
     }
     else if ( theAngle > 360.0 )
     {
         theAngle -= 360.0;
     }
     
     return theAngle;
 }