using UnityEngine;
using System.Collections;

public class ThirdPersonPushBodies : MonoBehaviour {
	
	private LayerMask pushLayers = -1;
	private ThirdPersonController controller;
	// Use this for initialization
	void Start () 
	{
		controller = GetComponent<ThirdPersonController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Debug.Log("OnControllerColliderHit");
		Rigidbody body = gameObject.rigidbody;
		
		var bodyLayerMask = 1 << body.gameObject.layer;
		var colliderLayerMask = hit.collider.gameObject.layer;
		Debug.Log("OnControllerColliderHit layer:"+gameObject+"=="+hit.collider.gameObject.layer);

		/*
		// no rigidbody
		if (body == null || body.isKinematic)
			return;
		// Ignore pushing those rigidbodies
		var bodyLayerMask = 1 << body.gameObject.layer;
		if ((bodyLayerMask & pushLayers.value) == 0)
			return;
			
		// We dont want to push objects below us
		if (hit.moveDirection.y < -0.3) 
			return;
		*/
		if(bodyLayerMask == colliderLayerMask)
		{
			NetworkProjectile networkProjectile = hit.collider.gameObject.GetComponent<NetworkProjectile>() as NetworkProjectile;
			Debug.Log("OnControllerColliderHit - "+hit.collider.gameObject);
			float pushPower  = networkProjectile.DMG;
			
			// Calculate push direction from move direction, we only push objects to the sides
			// never up and down
			Vector3 pushDir = new Vector3 (hit.moveDirection.x, 0, hit.moveDirection.z);
			
			// push with move speed but never more than walkspeed
			body.velocity = pushDir * pushPower * Mathf.Min(controller.GetSpeed(), controller.walkSpeed);
		}
	}
}

