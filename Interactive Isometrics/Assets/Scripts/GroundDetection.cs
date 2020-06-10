using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GroundDetection : MonoBehaviour
{
    public Burrow script;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.isTrigger == false)
        {
            script.IsGrounded = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.isTrigger == false)
        {
            script.IsGrounded = false;
        }
    }
    private void Start() 
    {
        if(script == null)
        {
            script = GetComponentInParent<Burrow>();
        }
    }
}
