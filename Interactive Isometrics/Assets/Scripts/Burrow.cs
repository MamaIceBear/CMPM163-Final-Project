using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burrow : MonoBehaviour
{
    public int burrowDepth = 1;
    [HideInInspector]
    public bool IsGrounded;

    bool hoverable = true;
    bool turnLeft = false;
    bool turnRight = false;
    bool grabbed = false;

    Vector3 burrowPosition;
    Vector3 originalPosition;
    float distToGround;

    Rigidbody rb;

    private void Start() 
    {
        distToGround = GetComponent<Collider>().bounds.extents.y;
        rb = GetComponent<Rigidbody>();
    }

    private void OnMouseOver() 
    {
        if(hoverable && IsGrounded)
        {
            originalPosition = burrowPosition = transform.position;
            rb.isKinematic = true;
            burrowPosition.y -= burrowDepth; 
            hoverable = false;
            turnLeft = true;
            StartCoroutine(Hide());
        }
    }

    private void OnMouseDown() 
    {
        turnLeft = false;
        turnRight = false;
        StopAllCoroutines();
        if(!grabbed)
        {
            grabbed = true;
            hoverable = false;
        }
        else
        {
            grabbed = false;
            hoverable = true;
        }
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(4);
        turnLeft = false;
        transform.rotation = Quaternion.identity;
        StartCoroutine(Appear());

    }

    IEnumerator Appear()
    {
        yield return new WaitForSeconds(Random.Range(2, 5));
        turnRight = true;
        StartCoroutine(Idle());
    }

    IEnumerator Idle()
    {   
        yield return new WaitForSeconds(4);
        rb.isKinematic = false;
        turnRight = false;
        hoverable = true;
        transform.rotation = Quaternion.identity;
        transform.position = originalPosition;
    }

    private void Update() 
    {
        if(turnLeft && !grabbed)
        {
            transform.Rotate(0, 450 * Time.deltaTime, 0);
            transform.position = Vector3.Lerp(transform.position, burrowPosition, 1f * Time.deltaTime);
        }
        if(turnRight && !grabbed)
        {
            transform.Rotate(0, -450 * Time.deltaTime, 0);
            transform.position = Vector3.Lerp(transform.position, originalPosition, 1f * Time.deltaTime);
        }
    }
}

