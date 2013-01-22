var runSpeedScale = 1.0;
var walkSpeedScale = 1.0;
var torso : Transform;

function Awake ()
{
	// By default loop all animations
	animation.wrapMode = WrapMode.Loop;

	// We are in full control here - don't let any other animations play when we start
	animation.Stop();
	animation.Play("idle");
}

function Update ()
{
	var moveController = GetComponent("moveOnMouseClick");
	var currentSpeed = moveController.GetSpeed();
	var walkSpeed = 0; //moveController.walkSpeed;

	// Fade in run
	if (currentSpeed > walkSpeed)
	{
		animation.CrossFade("run");
		// We fade out jumpland quick otherwise we get sliding feet
		SendMessage("SyncAnimation", "run");
	}
	// Fade in walk
	else if (currentSpeed > 0.1)
	{
		animation.CrossFade("walk");
		// We fade out jumpland realy quick otherwise we get sliding feet
		SendMessage("SyncAnimation", "walk");
	}
	// Fade out walk and run
	else
	{
		animation.CrossFade("idle");
		SendMessage("SyncAnimation", "idle");
	}
	
	animation["run"].normalizedSpeed = runSpeedScale;
	animation["walk"].normalizedSpeed = walkSpeedScale;
	
}
/*

function DidPunch () {
	animation.CrossFadeQueued("punch", 0.3, QueueMode.PlayNow);
}

function DidButtStomp () {
	animation.CrossFade("buttstomp", 0.1);
	SendMessage("SyncAnimation", "buttstomp");
	animation.CrossFadeQueued("jumpland", 0.2);
}

function ApplyDamage () {
	animation.CrossFade("gothit", 0.1);
	SendMessage("SyncAnimation", "gothit");
}
*/