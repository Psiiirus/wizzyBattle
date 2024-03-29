var walkSpeed = 8.0;
var walkBackwardSpeed = 4.0;
var sidestepSpeed = 8.0;
var gravity = 10.0;
var maxVelocityChange = 10.0;
var inAirControl = 0.1;
var canJump = true;
var jumpHeight = 2.0;

// added for run (set to Fire2. Change in Project Settings / Input if different desired.)
var canRun = true;
var runSpeed = 14.0; // negative values here makes game unhappy
var runBackwardSpeed = 6.0; // negative values here makes game unhappy
// var runSpeedChange = 4.0; // negative values here makes game unhappy

var canRunSidestep = false;
var runSidestepSpeed = 12.0;

// added for keyboard rotation ('Horizontal' rotates instead of translates / strafe)
/*
var canRotate = true; 
var rotateSpeed = 1.0;
var rotateInAir = false;
*/
private var grounded = false;
private var groundVelocity : Vector3;
private var capsule : CapsuleCollider;

public  var rotateSpeed:float = 4.0F; 

private var currentMovingSpeed:float = 0.0F;

@script RequireComponent(Rigidbody, CapsuleCollider)
 
 
function GetSpeed()
{
	return currentMovingSpeed;
}
 
function Awake ()
{
   rigidbody.freezeRotation = true;
   //rigidbody.useGravity = false;
   capsule = GetComponent(CapsuleCollider);
}

function Update()
{
        
	if(Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical")!=0)
		currentMovingSpeed = walkSpeed;
	else
		currentMovingSpeed = 0.0F;

}

function FixedUpdate ()
{

    // Generate a plane that intersects the transform's position with an upwards normal.
    var playerPlane = new Plane(Vector3.up, transform.position);
 
    // Generate a ray from the cursor position
    var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
 
    // Determine the point where the cursor ray intersects the plane.
    // This will be the point that the object must look towards to be looking at the mouse.
    // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
    //   then find the point along that ray that meets that distance.  This will be the point
    //   to look at.
    var hitdist = 0.0;
    // If the ray is parallel to the plane, Raycast will return false.
    if (playerPlane.Raycast (ray, hitdist)) {
        // Get the point along the ray that hits the calculated distance.
        var targetPoint = ray.GetPoint(hitdist);
 
        // Determine the target rotation.  This is the rotation if the transform looks at the target point.
        var targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
 
        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }


    if (grounded)
    {
        // Calculate how fast we should rotate
        //var rotation = Input.GetAxis("Horizontal") * rotateSpeed;
        
        // Set 'Horizontal' input based on rotate variable
        
        //var targetVelocity = (canRotate) ? new Vector3(0, rotation, Input.GetAxis("Vertical")) : new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
      

        
        // Calculate how fast we should be moving
       	var horizontalInput = Input.GetAxis("Horizontal");
       	var verticalModifier = 1;
       	if( transform.rotation.eulerAngles.x > 90 && transform.rotation.eulerAngles.x < 270 )
       		verticalModifier = -1;
       	
        var verticalInput = Input.GetAxis("Vertical") * verticalModifier;
        
        targetVelocity = new Vector3(horizontalInput, 0, verticalInput);
         
        //targetVelocity = transform.TransformDirection(targetVelocity);	
        
        if (Input.GetAxis("Vertical") > 0)
        {
            targetVelocity.x *= (canRun && Input.GetButton("Fire2")) ? runSpeed : walkSpeed;
        }
        else 
        { 
            targetVelocity.x *= (canRun && Input.GetButton("Fire2")) ? runBackwardSpeed : walkBackwardSpeed;
        }

        targetVelocity.z *= (canRunSidestep && Input.GetButton("Fire2")) ? runSidestepSpeed : sidestepSpeed;
        
        
        // Rotate if rotate is enabled
        /*if (canRotate)
        {
            transform.Rotate(0, rotation, 0);
        }
        */
        
        // Apply a force that attempts to reach our target velocity
        var velocity = rigidbody.velocity;
        var velocityChange = (targetVelocity - velocity) + groundVelocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
       
        // Jump
        if (canJump && Input.GetButton("Jump"))
        {
            rigidbody.velocity = Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
        }
       
        grounded = false;
    }
    
    else
    {
        // Add in air
        
        // Calculate how fast we should rotate
        /*rotation = Input.GetAxis("Horizontal") * rotateSpeed;
        
        // Set 'Horizontal' input behavior based on whether we can rotate in air
        targetVelocity = (rotateInAir) ? new Vector3(0, rotation, Input.GetAxis("Vertical"))
        : new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        */    
        targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        targetVelocity = transform.TransformDirection(targetVelocity) * inAirControl;
        
        // Rotate if rotate in air is enabled.
        /*if (rotateInAir)
        {
            transform.Rotate(0, rotation, 0);
        }
        */
        rigidbody.AddForce(targetVelocity, ForceMode.VelocityChange);
    }
       
    // We apply gravity manually for more tuning control
    rigidbody.AddForce(Vector3 (0, -gravity * rigidbody.mass, 0));
}

function TrackGrounded (col : Collision) 
{
   var minimumHeight = capsule.bounds.min.y + capsule.radius;
   for (var c : ContactPoint in col.contacts) 
   {
      if (c.point.y < minimumHeight) 
      {
         //we track velocity for rigidbodies we're standing on
         if (col.rigidbody) groundVelocity = col.rigidbody.velocity;
         //and become children of non-rigidbody colliders
         else transform.parent = col.transform;
         grounded = true;
      }
   }   
}
/*
//unparent if we are no longer standing on our parent
function OnCollisionExit (col : Collision) 
{
   if (col.transform == transform.parent) transform.parent = null;
}
*/

function OnCollisionStay (col : Collision)
{
	if(col.gameObject.tag == "Baseplate")
   		TrackGrounded (col);
}

function OnCollisionEnter (col : Collision)
{
   if(col.gameObject.tag == "Baseplate")
   		TrackGrounded (col);
}
 
function CalculateJumpVerticalSpeed ()
{
   // From the jump height and gravity we deduce the upwards speed
   // for the character to reach at the apex.
   return Mathf.Sqrt(2 * jumpHeight * gravity);
} 