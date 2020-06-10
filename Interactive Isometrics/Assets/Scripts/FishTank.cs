using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTank : MonoBehaviour
{
    //Holds the fish model
    public GameObject fishPrefab;
    public bool seeBoundaries = false;

    //The width and height of the tank is tankSize x 2
    public float tankSizeX = 15f;
    public float tankSizeY = 2f;
    public float tankSizeZ = 15f;

    //How many fish to spawn within this cluster
    public int numFish = 20;
    public int[] initialRotation;

    [HideInInspector]
    public GameObject[] allFish;
    [HideInInspector]
    public Vector3 goalPos = Vector3.zero;
    [HideInInspector]
    public Vector3 objectPos;

    //Randomizes each fish's scaling
    public float randomScaleMin = 0.8f;
    public float randomScaleMax = 1.2f;
    float randomSize;
    Vector3 fishSize;
    Quaternion initDirection;

    void Start()
    {
        //Creating all existing fish
        allFish = new GameObject[numFish];

        //Randomly spawns all fish within the tank
        for (int i = 0; i < numFish; i++)
        {
            //Setting each fish's random position
            Vector3 pos = new Vector3(Random.Range(-tankSizeX, tankSizeX),
                                      Random.Range(-tankSizeY, tankSizeY),
                                      Random.Range(-tankSizeZ, tankSizeZ));
            
            initDirection = Quaternion.Euler(0, initialRotation[Random.Range(0, initialRotation.Length)], 0);

            //Setting the scale of each fish before instantiation
            randomSize = Random.Range(randomScaleMin, randomScaleMax);
            fishSize = new Vector3(randomSize, randomSize, randomSize);
            fishPrefab.transform.localScale = fishSize;
            allFish[i] = (GameObject) Instantiate(fishPrefab, transform.localPosition + pos, initDirection);
            allFish[i].transform.parent = gameObject.transform;
        }
        if(seeBoundaries)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, new Vector3(tankSizeX, tankSizeY, tankSizeZ));
        }
    }

    void Update()
    {
        objectPos = transform.position;
        //How often the fish's new designation would change
        if (Random.Range(0, 10000) < 50)
        {
            //Setting the goal point randomly within the tank
            goalPos = transform.position + new Vector3(Random.Range(-tankSizeX, tankSizeX),
                                                       Random.Range(-tankSizeY, tankSizeY),
                                                       Random.Range(-tankSizeZ, tankSizeZ));
        }
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, new Vector3(tankSizeX * 2, tankSizeY * 2, tankSizeZ * 2));
    }
}
