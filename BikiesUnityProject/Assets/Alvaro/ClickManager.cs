using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClickManager : MonoBehaviour
{

    public LayerMask detectionMask;
    public GameObject lastSelected;
    public GameObject lastSelectedBuyPanel;

    public EconomyManager economyManager;
    public TextMeshProUGUI coinText;
    public int coins;

    // Start is called before the first frame update
    void Awake()
    {
        coins = economyManager.getWallet();
        coinText.SetText(coins.ToString());
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
            lastSelectedBuyPanel = hit.collider.gameObject.transform.GetChild(1).gameObject;
            ActivateFloatingMenu(lastSelected);

            if (Input.GetMouseButtonDown(0))
            {
                // Aqui se comprara una bici
                if (!lastSelectedBuyPanel.activeSelf)
                {
                    ActivateFloatingMenu(lastSelectedBuyPanel);
                }
                else if (lastSelectedBuyPanel.activeSelf)
                {
                    DeactivateFloatingMenu(lastSelectedBuyPanel);
                }


            }
        }
        else if (hit.collider == null && lastSelected != null)
        {
            DeactivateFloatingMenu(lastSelected);
        }


        coins = economyManager.getWallet();
        coinText.SetText(coins.ToString());


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

