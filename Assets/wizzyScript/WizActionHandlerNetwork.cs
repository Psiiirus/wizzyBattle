using UnityEngine;
using System.Collections;

public class WizActionHandlerNetwork : MonoBehaviour 
{	
	public NetworkProjectile Spell_1;
	public NetworkProjectile Spell_2;
	public NetworkProjectile Spell_3;
	public NetworkProjectile Spell_4;
	
	public WizBullet Spell_5;
	
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
		NetworkProjectile tNetworkProjectile = null;
		WizBullet tWizBullet = null;
		
		if(Input.GetButton("Spell_1"))
		{
			tNetworkProjectile = Spell_1;
		}
		else if(Input.GetButton("Spell_2"))
		{
			tNetworkProjectile = Spell_2;
		}
		else if(Input.GetButton("Spell_3"))
		{
			tNetworkProjectile = Spell_3;
		}
		else if(Input.GetButton("Spell_4"))
		{
			tWizBullet = Spell_5;
		}

		
		if(tWizBullet)
		{
			if(Network.isClient || Network.isServer)
			{
				if(gameObject.networkView.isMine)
				{
					Debug.Log(gameObject+" doCastSpell NETWORK");
					doCastSpell(tWizBullet,false);	
				}	
			}else
			{
				Debug.Log(gameObject+" doCastSpell");
				doCastSpell(tWizBullet,true);	
			}
		}				
		
		if(tNetworkProjectile)
		{
			if(Network.isClient || Network.isServer)
			{
				if(gameObject.networkView.isMine)
				{
					Debug.Log(gameObject+" doFireMain NETWORK");
					doFireMainNetwork(tNetworkProjectile);	
				}	
			}else
			{
				Debug.Log(gameObject+" doFireMain");
				doFireMain(tNetworkProjectile);	
			}
		}		
	}
	
	void doFireMainNetwork(NetworkProjectile aNetworkProjectile)
	{
		if (Time.time > reloadTimeMain + lastShotMain) 
		{
		    lastShotMain = Time.time;
			Vector3 tPos = fireStartVector.position;
			
			NetworkProjectile tProjectile = (NetworkProjectile) Network.Instantiate(aNetworkProjectile,
																					tPos,
																					gameObject.transform.rotation,
																					0);
			//NetworkProjectile.owner = gameObject;
			tProjectile.shoot();
		}
	}

	void doFireMain(NetworkProjectile aNetworkProjectile)
	{
		if (Time.time > reloadTimeMain + lastShotMain) 
		{
		    lastShotMain = Time.time;
			Vector3 tPos = fireStartVector.position;
			
			NetworkProjectile tProjectile = (NetworkProjectile) GameObject.Instantiate(aNetworkProjectile,
																					tPos,
																					gameObject.transform.rotation);
			tProjectile.shoot();
		}
	}

	void doCastSpell(WizBullet aWizProjectile,bool isNetwork)
	{
		if (Time.time > reloadTimeMain + lastShotMain) 
		{
		    lastShotMain = Time.time;
			Vector3 tPos = fireStartVector.position;
			WizBullet tProjectile = null;
			
			if(isNetwork)
				tProjectile = (WizBullet) Network.Instantiate(aWizProjectile,
					tPos,
					gameObject.transform.rotation,
					0);
			else
				tProjectile = (WizBullet) GameObject.Instantiate(aWizProjectile,
					tPos,
					gameObject.transform.rotation);
			
			//tProjectile.shoot();
		}
	}	
	
	private void popupHit(int DMG)
	{
		var tPopupPos= gameObject.transform.position;
		
		var tText 	= DMG.ToString();
		var tPosY 	= 6.0F;
		
		gameObject.GetComponent<IngameTextPopup>().popupText(tText,tPopupPos,tPosY);
	}
	
	public void hit(int aDMG, Transform projectileTransform)
	{
		Debug.Log(gameObject+" got damaged! | MASS "+rigidbody.mass+" | DMG "+aDMG);
		health -= aDMG;
		popupHit(aDMG);

		//rigidbody.isKinematic = true;
		//rigidbody.AddRelativeForce(new Vector3(0,0,aDMG*2),ForceMode.Impulse);
		//rigidbody.AddExplosionForce(power, transform.position, radius, 3.0F);
		//rigidbody.MovePosition(new Vector3(0,0,aDMG*10));
		//rigidbody.isKinematic = false;
		//rigidbody.AddForceAtPosition(new Vector3(0,0,aDMG), transform.position);
		
		Vector3 direction = transform.position - projectileTransform.position;
		rigidbody.AddForceAtPosition(direction.normalized*aDMG, projectileTransform.position,ForceMode.Impulse);
		Debug.Log(direction.normalized+" direction.normalized "+direction.normalized*aDMG);
		
		
		if(rigidbody.mass>0.1F)
		{
			float newMass = aDMG;
			newMass = newMass / 100.0F;
			newMass = rigidbody.mass-newMass;

			if(newMass<0.1F)
				newMass = 0.1F;

			rigidbody.mass = newMass;	
		}
		
		
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