using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    //How fast the fish rotates
    public float rotationSpeed = 4.0f;

    //How much distance required for each fish and their neighbors to form a flock
    public float neighbourDistance = 2f;

    //Different speeds each fish will have
    public float minSpeed = 2f;
    public float maxSpeed = 4f;

    FishTank tank;

    //Each fish's currentSpeed
    float currentSpeed;

    Vector3 managerPos;
 
    void Start()
    {
        currentSpeed = Random.Range(minSpeed, maxSpeed);
        tank = GetComponentInParent<FishTank>();
    }

    void Update()
    {
        managerPos = tank.objectPos;

        float currentXPOS = Mathf.Abs(transform.position.x - managerPos.x);
        float currentYPOS = Mathf.Abs(transform.position.y - managerPos.y);
        float currentZPOS = Mathf.Abs(transform.position.z - managerPos.z);
        //If fish is going out of the boundary
        if (currentXPOS >= tank.tankSizeX || currentYPOS >= tank.tankSizeY || currentZPOS >= tank.tankSizeZ)
        {
            Vector3 direction = managerPos - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            currentSpeed = Random.Range(minSpeed, maxSpeed);
        }
        else
        {
            //How often fishes will form flocks
            if (Random.Range(0, 5) < 1)
            {
                ApplyRules();
            }
        }
        //Consistent movement
        transform.Translate(0, 0, Time.deltaTime * currentSpeed);
    }

    //Basics of Boid
    void ApplyRules()
    {
        //Get all existing fish in the scene
        GameObject[] existingFish;
        existingFish = tank.allFish;

        //Use to determine fish's thinking when in a group
        Vector3 vcenter = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float groupSpeed = 0f;

        //The group will have a similar goal
        Vector3 goalPos = tank.goalPos;
        float dist;
        
        //Flock size
        int groupSize = 0;
        foreach(GameObject fish in existingFish)
        {
            //If the fish is other than itself
            if (fish != this.gameObject)
            {
                dist = Vector3.Distance(fish.transform.position, this.transform.position);

                //If the fishes are close enough
                if(dist <= neighbourDistance)
                {
                    //Get the average point for the group's position
                    vcenter += fish.transform.position;

                    //Increase flock's numbers
                    groupSize++;

                    //If the fish are too close to each other
                    if(dist < 3.0f)
                    {
                        vavoid += (this.transform.position - fish.transform.position);
                    }

                    Flock anotherFlock = fish.GetComponent<Flock>();

                    //Add the fish's speed to the total group's speed
                    groupSpeed += anotherFlock.currentSpeed;
                }
            }
        }

        //If flock is made
        if(groupSize > 0)
        {
            vcenter = vcenter / groupSize + (goalPos - this.transform.position);
            
            //Get the average speed of the group
            currentSpeed = groupSpeed / groupSize;
            
            Vector3 direction = (vcenter + vavoid) - transform.position;

            //Moves together in a direction
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }
    }
}
