var playerPrefab : Transform;

function OnNetworkLoadedLevel ()
{
	var randTransfrom = transform.position;
	
	var newPosition : Vector2 = Random.insideUnitCircle * 10;
	randTransfrom.x += newPosition.x;
	
	
	var PlayerTransform = Network.Instantiate(playerPrefab, randTransfrom, transform.rotation, 0);
	PlayerTransform.parent = transform.parent;
}

function OnPlayerDisconnected (player : NetworkPlayer)
{
	Debug.Log("Server destroying player");
	Network.RemoveRPCs(player, 0);
	Network.DestroyPlayerObjects(player);
}
