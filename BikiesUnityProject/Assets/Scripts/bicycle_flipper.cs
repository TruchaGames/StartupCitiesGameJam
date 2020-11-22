using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum UNITTYPE
//{
//    Pedestrian = 0,
//    Bicycle
//}

public class bicycle_flipper : MonoBehaviour
{
    public UNITTYPE unit_type = UNITTYPE.Pedestrian;
    public Sprite leftSprite;
    public Sprite rightSprite;
    public Sprite pedestrianSprite;
    public float rotation_time = 1.0f;
   

    public bool go_left = true;
    private bool last_direction = true;
    private bool changed_direction = false;
    private bool rotating = false;
    private bool run_timer = false;

    private float last_pos_x = 0.0f;
    private float time_rot = 0.0f;
    private float degrees_rot = 0.0f;

    private SpriteRenderer renderized_sprite;
    // Start is called before the first frame update
    private void Awake()
    {
        renderized_sprite = transform.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (go_left)
        //    renderized_sprite.sprite = leftSprite;
        //else
        //    renderized_sprite.sprite = rightSprite;

        if (unit_type == UNITTYPE.Bicycle)
        {
            
            renderized_sprite.sprite = leftSprite;
            if (transform.position.x > last_pos_x)//Going right
            {
                renderized_sprite.flipX = true;
            }
            else if (transform.position.x < last_pos_x)//Going left
            {
                renderized_sprite.flipX = false;
            }
        }
        else if(unit_type == UNITTYPE.Pedestrian)
        {
            renderized_sprite.sprite = pedestrianSprite;
        }

        //if (rotating == true)
        //{
        //    time_rot += Time.deltaTime;

        //    //Here we lerp the value to rotate
        //    float lerp_val = time_rot / rotation_time;
        //    if (lerp_val > 1.0f)
        //        lerp_val = 1.0f;

        //    //Lerp and rotate
        //    transform.Rotate(180.0f * Time.deltaTime,0,0);

        //    //We finished rotating
        //    if (time_rot >= rotation_time)  
        //    {
        //        rotating = false;
        //        time_rot = 0.0f;
        //        degrees_rot = 0.0f;
        //    }
        //}

        //if (last_direction != go_left)
        //{
        //    rotating = true;
        //}

        last_pos_x = transform.position.x;
        last_direction = go_left;
    }
}
