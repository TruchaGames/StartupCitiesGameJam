using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Bicycle : MonoBehaviour
{

    private SpriteRenderer renderized_sprite;
    private float last_pos_x = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        //Get our own sprite renderer
        renderized_sprite = transform.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        transform.Rotate(90.0f, 0.0f, 0.0f);
        if (transform.position.x > last_pos_x)//Going right
        {
            renderized_sprite.flipX = true;
        }
        else if (transform.position.x < last_pos_x)//Going left
        {
            renderized_sprite.flipX = false;
        }

        last_pos_x = transform.position.x;
    }
}
