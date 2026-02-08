using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target; // The target object to orbit around
    public float rotSpeed = 1.5f; // Rotation speed
    private float rotY; // Current rotation around the Y-axis
    private Vector3 offset; // Initial offset from the target
    void Start()
    {
        rotY = transform.eulerAngles.y; // Initialize the Y rotation based on the current rotation
        offset = target.position - transform.position; // Calculate the initial offset from the target
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float horInput = Input.GetAxis("Horizontal"); // Get horizontal input (A/D or Left/Right arrow keys)
        if (!Mathf.Approximately(horInput, 0f)) // Check if there is horizontal input
        {
            rotY += horInput * rotSpeed; // Update the Y rotation based on input and rotation speed
        }
        else
        {
            rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;
        }
        Quaternion rotation = Quaternion.Euler(0, rotY, 0); // Create a rotation based on the Y rotation
        transform.position = target.position - (rotation * offset); // Update the camera's position based on the target and rotation
        transform.LookAt(target); // Make the camera look at the target
    }
}
