using UnityEngine;
using System.Collections;

public class WizBullet : MonoBehaviour 
{
	public float force = 10.0F;
	
	public float speed = 500.0F;
	public float randomAngle = 0.0F;
	public float lifeTime = 3.0F;
	public float bulletGravitiy = 9.8F;
	public GameObject decolHitWall;
	public float floatInFrontOfWall = 0.00001F;
	
	private Vector3 moveDirection;
	private float shortestSoFar;
	private Vector3 instanciatePoint;
	private Quaternion instanciateRotation;
	private bool foundHit = false;
	private Transform parentToAdd;
	
	
	void Awake() 
	{
		transform.Rotate( (Random.value * 2 * randomAngle) - randomAngle,0, Random.value * 360);
		moveDirection = transform.forward * speed;
		shortestSoFar = Mathf.Infinity;
		foundHit = false;
	}
	
	void FixedUpdate()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.rotation = Quaternion.LookRotation(moveDirection);
		RaycastHit[] hits;
		hits = Physics.RaycastAll(transform.position,transform.forward,speed * Time.deltaTime);
		Debug.Log("hits "+hits.Length);
		RaycastHit nearestHit;
		
		for(int i=0;i<hits.Length;i++)
		{	
			RaycastHit hit = hits[i];	
			
			if(hit.transform.tag == "Level Parts" || hit.transform.tag == "WizWeightObject" || hit.transform.tag == "Player")
			{
				if(Vector3.Distance(transform.position, hit.point) < shortestSoFar)
				{
					if(hit.transform.tag=="Player")
						Debug.Log("found player nearby");
					
					Debug.DrawLine(transform.position, hit.point);
					
					instanciatePoint = hit.point + (hit.normal * (floatInFrontOfWall + Random.Range(0,0.01F)) );
					instanciateRotation = Quaternion.LookRotation(hit.normal);
					parentToAdd = hit.transform;
					shortestSoFar = Vector3.Distance(transform.position, hit.point);
					foundHit = true;
					nearestHit = hit;
				}
			}
		}
	
		if(foundHit)
		{
			/*
			GameObject spawnedBulletHole = (GameObject) Instantiate(decolHitWall, instanciatePoint,instanciateRotation);
			if(parentToAdd)
				spawnedBulletHole.transform.position = parentToAdd.position;
			*/
			
			Renderer renderer = nearestHit.collider.renderer;
            if (renderer) 
			{
                renderer.material.shader = Shader.Find("Transparent/Diffuse");
				//float tColor = 0.3F;
				Color nColor = new Color(0,0,0,0.3F);
                renderer.material.color = nColor;
            }
			
			if(nearestHit.transform.tag == "WizWeightObject" || nearestHit.transform.tag == "Player")
			{				
				WizWeightObjectJS tWizWeightObject = nearestHit.transform.GetComponent("WizWeightObjectJS") as WizWeightObjectJS;
				Debug.Log("tWizWeightObject "+tWizWeightObject+" | "+transform.forward+" | "+transform.right);
				//tWizWeightObject.forceToAdd = transform.forward * force;
				//tWizWeightObject.applyForce(transform.forward * force);
				tWizWeightObject.applyDirection(transform.forward);
			}
				 
			DestroyObject(gameObject);
	
		
		
		}
	
		transform.position 	+= moveDirection * Time.deltaTime;
		moveDirection.y 	-= bulletGravitiy*Time.deltaTime;
		
		lifeTime -= Time.deltaTime;
		if(lifeTime < 0)
			DestroyObject(gameObject);
			
	}
}
