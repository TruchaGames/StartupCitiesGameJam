using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    [Header("Camera Speeds")]
    public float mov_speed = 3.0f;
    public float zoom_speed = 1.0f;
    public float PanSpeed = 2.0f;

    [Header("Camera Zoom")]
    public int max_zoom_in = 6;
    public int max_zoom_out = 20;
    
    [Header("Camera Boundaries")]
    public float[] BoundariesZ = new float[] { -125f, 22f };
    public float[] BoundariesX = new float[] { -50f, 69f };

    public bool AllowMovement = false;


    Vector3 lastPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!AllowMovement)
            return;

        // Camera Panning
        if (Input.GetMouseButtonDown(2))
            lastPosition = Input.mousePosition;
        else if (Input.GetMouseButton(2))
        {
            Vector3 offset = GetComponent<Camera>().ScreenToViewportPoint(lastPosition - Input.mousePosition);
            Vector3 Move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);
            transform.Translate(Move, Space.World);
        }

        lastPosition = Input.mousePosition;

        //GET WASD to move the camera in each axis
        float mSpeed = mov_speed;
        if (Input.GetKey(KeyCode.LeftShift))
            mSpeed = mov_speed * 2.0f;

        if (Input.GetKey(KeyCode.A))
            transform.position = new Vector3(transform.position.x - mSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        if (Input.GetKey(KeyCode.D))
            transform.position = new Vector3(transform.position.x + mSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        if (Input.GetKey(KeyCode.W))
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + mSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - mSpeed * Time.deltaTime);

        //Use mousewheel to zoom in/out
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)   //Mouse_Wheel Forward
        {
            float final_pos = transform.position.y - zoom_speed * Time.deltaTime;
            if (final_pos <= max_zoom_in)
                transform.position = new Vector3(transform.position.x, max_zoom_in, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x, transform.position.y - zoom_speed*Time.deltaTime, transform.position.z);            
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)   //Mouse_Wheel backward
        {
            float final_pos = transform.position.y + zoom_speed * Time.deltaTime;
            if (final_pos >= max_zoom_out)
                transform.position = new Vector3(transform.position.x, max_zoom_out, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x, transform.position.y + zoom_speed * Time.deltaTime, transform.position.z);
        }

        //GetComponent<Camera>().fieldOfView = Mathf.Clamp(GetComponent<Camera>().fieldOfView - (Input.GetAxis("Mouse ScrollWheel") * zoom_speed), max_zoom_in, max_zoom_out);

        // Camera Boundaries Clamp
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundariesX[0], BoundariesX[1]);
        pos.z = Mathf.Clamp(transform.position.z, BoundariesZ[0], BoundariesZ[1]);
        transform.position = pos;
    }
}
