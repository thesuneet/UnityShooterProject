using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovements : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    // Start is called before the first frame update
    void Start()
    {
        // Locking the cursor to the middle of the screen and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Getting Mouse Inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Rotation around X axis(look up and down)
        xRotation -= mouseY;

        //Clamp rotation
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        //Rotation around X axis(look left and right)
        yRotation += mouseX;

        //Apply rotations to our trransform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
