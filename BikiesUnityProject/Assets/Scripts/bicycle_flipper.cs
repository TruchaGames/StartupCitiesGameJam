using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bicycle_flipper : MonoBehaviour
{
    public Sprite leftSprite;
    public Sprite rightSprite;

    public bool go_left = true;

    private float last_pos_x = 0.0f;

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
        if (go_left)
            renderized_sprite.sprite = leftSprite;
        else
            renderized_sprite.sprite = rightSprite;


        if (transform.position.x > last_pos_x)//Going right
        {
            go_left = false;
        }
        else if (transform.position.x < last_pos_x)//Going left
        {
            go_left = true;
        }


        last_pos_x = transform.position.x;
    }
}
