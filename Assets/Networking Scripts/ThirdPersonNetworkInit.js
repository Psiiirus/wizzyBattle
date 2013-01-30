function OnNetworkInstantiate (msg : NetworkMessageInfo) {
	// This is our own player
	if (networkView.isMine)
	{
		Camera.main.GetComponent("FollowTrackingCamera").SetTarget(transform);
		GetComponent("NetworkInterpolatedTransform").enabled = false;
	}
	// This is just some remote controlled player
	else
	{
		name += "Remote";
		//GetComponent(WizCharacterControls).enabled = false;
		GetComponent(WizSimpleAnimation).enabled = false;
		GetComponent("NetworkInterpolatedTransform").enabled = true;
		GetComponent("WizMoveOnMouseClick").enabled = false;
	}
}
