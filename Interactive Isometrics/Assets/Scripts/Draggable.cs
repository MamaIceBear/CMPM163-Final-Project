using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public int grabRange = 5;
    public int throwStrength = 10;
    Rigidbody rb;
    Vector3 mousePosition;
    Vector3 objPosition;
    bool holding = false;
    bool grabbing = true;

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMouseDown() 
    {   
        if(!holding)
        {
            holding = true;
            rb.isKinematic = true;
            grabbing = true;
        }
        else
        {
            holding = false;
            rb.isKinematic = false;
        }
    }

    private void Update() 
    {
        if(holding)
        {
            mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, grabRange);
            objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            if(grabbing)
            {
                transform.position = Vector3.Lerp(transform.position, objPosition, 10 * Time.deltaTime);
                if(Vector3.Distance(transform.position, objPosition) < 1f)
                {
                    grabbing = false;
                }
            }
            else
            {
                transform.position = objPosition;
            }
            if(Input.GetMouseButtonDown(1))
            {
                rb.isKinematic = false;
                rb.AddForce(Camera.main.transform.parent.forward * throwStrength, ForceMode.Impulse);
                holding = false;
            }
        }
    }
}
