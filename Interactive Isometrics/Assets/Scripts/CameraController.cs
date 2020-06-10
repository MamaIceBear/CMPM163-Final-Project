using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;

    float cameraMoveSpeed;
    public float lerpSpeed = 5f;

    //Movement Speed
    public float normalSpeed = 0.5f;
    public float fastSpeed = 1f;

    //Rotation Speed
    public float rotationSpeed = 1f;

    //Zoom Speed
    public Vector3 zoomAmount;

    //Boundaries
    public float moveLimit = 100f;
    public float zoomInLimit = 0f;
    public float zoomOutLimit = 100f;
    public float rotationMinSpeed = 0.1f;
    
    //New properties that will be lerped to
    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
        Camera.main.depth = 0;
    }

    void Update()
    {
        KeyboardInput();
        MouseInput();
    }

    void KeyboardInput()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            cameraMoveSpeed = fastSpeed;
        }
        else
        {
            cameraMoveSpeed = normalSpeed;
        }
        //Camera panning with the WASD & arrow keys
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * cameraMoveSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (-transform.right * cameraMoveSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (-transform.forward * cameraMoveSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * cameraMoveSpeed);
        }
        //Camera rotation with the Q & E keys
        rotationSpeed = 1 - (cameraTransform.localPosition.y / zoomOutLimit);
        rotationSpeed = Mathf.Clamp(rotationSpeed, rotationMinSpeed, float.MaxValue);
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationSpeed);
        }
        newPosition.x = Mathf.Clamp(newPosition.x, -moveLimit, moveLimit);
        newPosition.z = Mathf.Clamp(newPosition.z, -moveLimit, moveLimit);
        transform.position = Vector3.Lerp(transform.position, newPosition, lerpSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, lerpSpeed * Time.deltaTime);
    }

    void MouseInput()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }
        newZoom.y = Mathf.Clamp(newZoom.y, zoomInLimit, zoomOutLimit);
        newZoom.z = Mathf.Clamp(newZoom.z, -(zoomOutLimit), -(zoomInLimit));
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, lerpSpeed * Time.deltaTime);
    }
}
