using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    public int mov_speed = 3;
    public int zoom_speed = 2000000;

    public int max_zoom_in = 6;
    public int max_zoom_out = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //GET WASD to move the camera in each axis
        if(Input.GetKey(KeyCode.A))
            transform.position = new Vector3(transform.position.x - mov_speed * Time.deltaTime, transform.position.y, transform.position.z);
        if (Input.GetKey(KeyCode.D))
            transform.position = new Vector3(transform.position.x + mov_speed * Time.deltaTime, transform.position.y, transform.position.z);
        if (Input.GetKey(KeyCode.W))
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + mov_speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - mov_speed * Time.deltaTime);

        //Use mousewheel to zoom in/out
        if ( Input.GetAxis("Mouse ScrollWheel") > 0f)   //Mouse_Wheel Forward
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
    }
}
