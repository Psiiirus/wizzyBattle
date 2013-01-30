
var forceDirection:Vector3;

// Update is called once per frame
function FixedUpdate () 
{
	if(forceDirection != Vector3.zero)
	{
		knockback(forceDirection);
		
		forceDirection = Vector3.zero;
	}
}

public function applyDirection(aForceDirection: Vector3)
{
	forceDirection = aForceDirection;
	Debug.Log("applyDirecton "+aForceDirection);
}

function knockback(aDirection:Vector3)
{
	Debug.Log("Knockback "+aDirection);	
	var startKnockback = Time.time;
	while (Time.time <= (startKnockback +.2))
	{
		var nVector:Vector3 = aDirection;
		var tFx : float = nVector.x * Time.deltaTime;
		var tFy : float = nVector.y * Time.deltaTime;
		var tFz : float = nVector.z * Time.deltaTime;				

		//Debug.Log("Knockback Translate"+tFx+","+tFy+","+tFz+" | "+Time.deltaTime+" | ");
		
		transform.Translate(Vector3(tFx,tFy,tFz),Space.World);
		yield;
	}
}

