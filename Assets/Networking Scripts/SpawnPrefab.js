var playerPrefab : Transform;

function OnNetworkLoadedLevel ()
{
	var PlayerTransform = Network.Instantiate(playerPrefab, transform.position, transform.rotation, 0);
	PlayerTransform.parent = transform.parent;
}

function OnPlayerDisconnected (player : NetworkPlayer)
{
	Debug.Log("Server destroying player");
	Network.RemoveRPCs(player, 0);
	Network.DestroyPlayerObjects(player);
}
