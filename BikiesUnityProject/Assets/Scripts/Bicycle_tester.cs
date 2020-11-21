using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bicycle_tester : MonoBehaviour
{

    public int mov_speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.J))
            transform.position = new Vector3(transform.position.x - mov_speed * Time.deltaTime, transform.position.y, transform.position.z);
        if (Input.GetKey(KeyCode.L))
            transform.position = new Vector3(transform.position.x + mov_speed * Time.deltaTime, transform.position.y, transform.position.z);
        if (Input.GetKey(KeyCode.I))
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + mov_speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.K))
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - mov_speed * Time.deltaTime);
    }
}
