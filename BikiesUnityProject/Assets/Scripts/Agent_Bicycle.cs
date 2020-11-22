using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UNITTYPE
{
    Pedestrian = 0,
    Bicycle
}

public class Agent_Bicycle : MonoBehaviour
{
    [Header("Necssary Sprites")]
    public Sprite pedestrian_sprite;
    public Sprite bicycle_sprite;

    public UNITTYPE unit_type = UNITTYPE.Pedestrian;
    private SpriteRenderer renderized_sprite;
    private float last_pos_x = 0.0f;
    public UNITTYPE previous_unit_type = UNITTYPE.Pedestrian;
    // Start is called before the first frame update
    void Start()
    {
        //Get our own sprite renderer
        renderized_sprite = transform.GetComponent<SpriteRenderer>();
        previous_unit_type = unit_type;

        if (previous_unit_type == UNITTYPE.Bicycle)
            renderized_sprite.sprite = bicycle_sprite;
        else if (previous_unit_type == UNITTYPE.Pedestrian)
            renderized_sprite.sprite = pedestrian_sprite;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        transform.Rotate(90.0f, 0.0f, 0.0f);

        if (unit_type == UNITTYPE.Bicycle)
        {
            if(previous_unit_type == UNITTYPE.Pedestrian)
            {
                renderized_sprite.sprite = bicycle_sprite;
            }
            if (transform.position.x > last_pos_x)//Going right
            {
                renderized_sprite.flipX = true;
            }
            else if (transform.position.x < last_pos_x)//Going left
            {
                renderized_sprite.flipX = false;
            }
        }
        else if (unit_type == UNITTYPE.Pedestrian)
        {
            if (previous_unit_type == UNITTYPE.Bicycle)
            {
                renderized_sprite.sprite = pedestrian_sprite;
            }
        }

        last_pos_x = transform.position.x;
        previous_unit_type = unit_type;
    }
}
