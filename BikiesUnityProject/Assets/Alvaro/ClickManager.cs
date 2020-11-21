using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{

    public LayerMask detectionMask;
    public GameObject lastSelected;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit hit;
        
       if(Physics.Raycast(mousePos, Vector3.forward, out hit, Mathf.Infinity, detectionMask))
        { 
            Debug.Log(hit.collider.gameObject.name);
            lastSelected = hit.collider.gameObject.transform.GetChild(0).gameObject;
            ActivateFloatingMenu(lastSelected);

            if (Input.GetMouseButtonDown(0))
            {
                // Aqui se comprara una bici
                Debug.Log("Hey");
                hit.collider.GetComponent<currentBikes>().AddBike();
        
            }
        }
        else if (hit.collider == null && lastSelected != null)
        {
            DeactivateFloatingMenu(lastSelected);
        }


       
    }

    void ActivateFloatingMenu(GameObject floatingMenu)
    {
        if (!floatingMenu.activeSelf)
            floatingMenu.SetActive(true);
    }

    void DeactivateFloatingMenu(GameObject floatingMenu)
    {
        if (floatingMenu.activeSelf)
            floatingMenu.SetActive(false);

        lastSelected = null;

    }
}

