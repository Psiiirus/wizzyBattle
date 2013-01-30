using UnityEngine;
using System.Collections;

public class WizWeightObject : MonoBehaviour {

	private Vector3 forceToAdd;
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(forceToAdd != Vector3.zero)
		{
			if(transform.tag=="Player")
				Debug.Log(" force player "+rigidbody+" | "+forceToAdd);
			//rigidbody.isKinematic = true;
			//rigidbody.AddForce(forceToAdd,ForceMode.Impulse);
			 float radius = 0.0F;
			rigidbody.AddExplosionForce(20F,transform.position,radius,3.0F);
			
			//rigidbody.isKinematic = false;
			forceToAdd = Vector3.zero;
		}
	}
	
	public void applyForce(Vector3 aForce)
	{
		forceToAdd = aForce;
	}
	
}
