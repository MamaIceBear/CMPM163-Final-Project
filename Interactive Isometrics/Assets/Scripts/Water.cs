using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Water : MonoBehaviour
{
	public float Pressure;
	public float WaterDrag;
	private float waterheight;

	private Collider Col;
	// Start is called before the first frame update
	void Start()
	{
		Col = GetComponent<Collider>();
		waterheight = transform.position.y + Col.bounds.size.y/2;
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		float speed;
		Vector3 impactangle;
		Rigidbody rb = null;
		if (other.GetComponent<Rigidbody>())
		{
			rb = other.GetComponent<Rigidbody>();
			speed = rb.velocity.magnitude;
			impactangle = rb.velocity.normalized;

			GameObject watersplash = VFXManager.instance.PlayEffectReturn("Water Splash", new Vector3(rb.gameObject.transform.position.x, waterheight, rb.gameObject.transform.position.z), 0, "");
			Renderer r = rb.gameObject.GetComponent<Renderer>();
			if (r == null)
			{
				r = rb.gameObject.GetComponentInChildren<Renderer>();
			}
			if (r)
			{
				float scale = r.bounds.size.magnitude * speed/10;
				watersplash.GetComponent<WaterSplash>().SetDrops((int)Random.Range(1, r.bounds.size.magnitude * speed / 10));
				watersplash.GetComponent<WaterSplash>().SetWaves((int)Random.Range(3, r.bounds.size.magnitude * speed / 10));
				watersplash.GetComponent<VFXScript>().ScaleVFX(watersplash.transform, scale);
			}
			
		}

	}

	private void OnTriggerStay(Collider other)
	{
		if (other.GetComponent<Rigidbody>())
		{
			Rigidbody rb = other.GetComponent<Rigidbody>();
			rb.drag = WaterDrag;
			rb.AddForce((1 + Pressure - rb.mass) * Mathf.Clamp((waterheight - other.transform.position.y)/4, 0.05f, 5) * transform.up, ForceMode.Impulse);
			rb.angularDrag = 2;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<Rigidbody>())
		{
			Rigidbody rb = other.GetComponent<Rigidbody>();
			rb.drag = 0;
			rb.angularDrag = 0.05f; // Default
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		//Gizmos.DrawSphere(transform.position + new Vector3(0, GetComponent<Collider>().bounds.size.y/2, 0), 1);
	}
}
