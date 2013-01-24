using UnityEngine;
using System.Collections;

public class NetworkProjectile : MonoBehaviour 
{
	public float initialSpeed = 90.0F;
	public float TTL;
	// Use this for initialization
	public int DMG = 10;
	private float pushPower = 0.5F;
	private LayerMask pushLayers  = -1;
	
	void Start () 
	{
		if (TTL == 0)
            TTL = 5;
		 
        Invoke("projectileTimeout", TTL);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void shoot()
	{
		//rigidbody.velocity = transform.TransformDirection( new Vector3 (0.0F, 0.0F, initialSpeed) );
		rigidbody.AddRelativeForce(new Vector3 (0.0F, 0.0F, initialSpeed));
	}
	
	void OnNetworkInstantiate (NetworkMessageInfo msg) 
	{
		// This is our own player
		if (networkView.isMine)
		{
			GetComponent<NetworkInterpolatedTransform>().enabled = false;
		}
		// This is just some remote controlled player
		else
		{
			name += "Remote";
			GetComponent<NetworkInterpolatedTransform>().enabled = true;
		}
	}
	
	void projectileTimeout()
    {
        DestroyObject(gameObject);
    }
	
	void OnTriggerEnter(Collider cObject)
	{
		Debug.Log("OnTriggerEnter - "+cObject.tag);
		if(cObject.tag == "Player" && cObject.networkView.isMine==false)
		{
						
			Debug.Log("OnTriggerEnter -- "+cObject);
			cObject.GetComponent<WizActionHandlerNetwork>().hit(DMG);

			//pushCollider(cObject);
			
			DestroyObject(gameObject);

		}
		 
	}
	
	void pushCollider(Collider cObject)
	{
		float pushPower = 0.5F;
		Rigidbody body = cObject.rigidbody;
		Debug.Log("pushCollider "+body);
		Debug.Log("pushCollider "+body.velocity+"__"+cObject.gameObject.transform.position );
		GameObject cGameObject = cObject.gameObject;
		/*
		// no rigidbody
		if (body == null || body.isKinematic)
			return;
		// Ignore pushing those rigidbodies
		int bodyLayerMask = 1 << body.gameObject.layer;
		if ((bodyLayerMask & pushLayers.value) == 0)
			return;
			
		// We dont want to push objects below us
		if (hit.moveDirection.y < -0.3) 
			return;
		*/
		// Calculate push direction from move direction, we only push objects to the sides
		// never up and down
		
		ThirdPersonController controller = cGameObject.GetComponent<ThirdPersonController>();
		
		cObject.transform.position  =  cObject.transform.position 
							* pushPower 
							* Mathf.Min(controller.GetSpeed(), controller.walkSpeed);	
		// push with move speed but never more than walkspeed
		body.velocity =  cObject.transform.position 
							* pushPower 
							* Mathf.Min(controller.GetSpeed(), controller.walkSpeed);		
	
		
	}
	 
}
