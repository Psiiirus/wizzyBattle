using UnityEngine;
using System.Collections;

public class WizActionHandlerNetwork : MonoBehaviour 
{	
	public NetworkProjectile fireMainPrefab;
	
	public float reloadTimeMain = 0.5F;
	private float lastShotMain = -10.0F;
	
	public Transform fireStartVector;
	public int health = 100;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetButton("Fire1"))
		{
			if(Network.isClient || Network.isServer)
			{
				if(gameObject.networkView.isMine)
				{
					Debug.Log(gameObject+" doFireMain");
					doFireMainNetwork();	
				}	
			}else
			{
				Debug.Log(gameObject+" doFireMain");
				doFireMain();	
			}
		}
	}

	void doFireMainNetwork()
	{
		if (Time.time > reloadTimeMain + lastShotMain) 
		{
		    lastShotMain = Time.time;
			Vector3 tPos = fireStartVector.position;
			
			NetworkProjectile tProjectile = (NetworkProjectile) Network.Instantiate(fireMainPrefab,
																					tPos,
																					gameObject.transform.rotation,
																					0);
			tProjectile.shoot();
		}
	}

	void doFireMain()
	{
		if (Time.time > reloadTimeMain + lastShotMain) 
		{
		    lastShotMain = Time.time;
			Vector3 tPos = fireStartVector.position;
			
			NetworkProjectile tProjectile = (NetworkProjectile) GameObject.Instantiate(fireMainPrefab,
																					tPos,
																					gameObject.transform.rotation);
			tProjectile.shoot();
		}
	}
	
	private void popupHit(int DMG)
	{
		var tPopupPos= gameObject.transform.position;
		
		var tText 	= DMG.ToString();
		var tPosY 	= 6.0F;
		
		gameObject.GetComponent<IngameTextPopup>().popupText(tText,tPopupPos,tPosY);
	}
	
	public void hit(int aDMG)
	{
		Debug.Log(gameObject+" got damaged! | isKinematic "+rigidbody.isKinematic);
		health -= aDMG;
		popupHit(aDMG);
		
		//rigidbody.isKinematic = false;
		rigidbody.AddForceAtPosition(new Vector3(0,0,aDMG), transform.position);
		
		if(health<=0)
		{
			kill();
		}
	}
	
	void kill()
	{
		Debug.Log(gameObject+" got killed");
	}
}