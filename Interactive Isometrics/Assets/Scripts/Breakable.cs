using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Breakable : MonoBehaviour
{
	public GameObject BrokenAsset;
	public float hardness;
	private bool destroyed;
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void LateUpdate()
	{
		if (GetComponent<Collider>() && destroyed == false)
		{
			Rigidbody rb = GetComponent<Rigidbody>();
			float speed = rb.velocity.magnitude;
			if(speed > 10 * hardness)
			{
				GetComponent<Collider>().isTrigger = true;
			}
			else
			{
				GetComponent<Collider>().isTrigger = false;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		float speed;
		Vector3 impactangle;
		Rigidbody rb = null;
		if (GetComponent<Rigidbody>() && other.isTrigger == false && BrokenAsset)
		{
			rb = GetComponent<Rigidbody>();
			speed = rb.velocity.magnitude;
			impactangle = rb.velocity.normalized;
			bool broken = speed > 10 * hardness;
			Debug.Log("Speed: " + speed + " - " + (10 * hardness) + "  Broken:" + broken);
			if (broken)
			{
				GameObject emit = Instantiate(BrokenAsset, transform.position, Quaternion.identity);
				emit.transform.rotation = transform.rotation;
				emit.transform.localScale = transform.localScale;
				emit.transform.parent = null;
				if (GetComponent<Collider>())
				{
					GetComponent<Collider>().enabled = false;
				}
				Break(emit, speed, impactangle);
				destroyed = true;
				Destroy(this.gameObject);
			}
		}

	}

	public void Break(GameObject emit, float speed, Vector3 angle)
	{
		if (emit.GetComponent<Collider>())
		{
			emit.GetComponent<Collider>().enabled = false;
		}

		int children = emit.transform.childCount;
		for (int i = children - 1; i >= 0; --i)
		{
			GameObject child = emit.transform.GetChild(i).gameObject;
			if (!child.GetComponent<BoxCollider>())
			{
				child.AddComponent<BoxCollider>();
				child.GetComponent<BoxCollider>().size *= .95f;
			}
			if (!child.GetComponent<Rigidbody>())
			{
				child.AddComponent<Rigidbody>();
				child.GetComponent<Rigidbody>().mass = GetComponent<Rigidbody>().mass;
			}
			if (!child.GetComponent<Draggable>())
			{
				child.AddComponent<Draggable>();
			}
			child.GetComponent<Rigidbody>().AddForce((speed * angle), ForceMode.Impulse);
			child.GetComponent<Rigidbody>().AddExplosionForce(speed, emit.transform.position, emit.GetComponent<Collider>().bounds.size.magnitude);
			child.transform.parent = null;
		}
	}
}
