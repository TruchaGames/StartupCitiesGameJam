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
        economyManager = FindObjectOfType<EconomyManager>();
        coins = economyManager.getWallet();
        coinText.SetText(coins.ToString());
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, detectionMask))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.transform.childCount > 0)
            {
                if (hit.collider.gameObject.transform.GetChild(0).gameObject.tag == "UI")
                {

                    lastSelected = hit.collider.gameObject.transform.GetChild(0).gameObject;
                }

                if (hit.collider.gameObject.transform.GetChild(1).gameObject.tag == "UI")
                {
                    lastSelectedBuyPanel = hit.collider.gameObject.transform.GetChild(1).gameObject;

                }
            }
               
            if (lastSelected != null)
            {
                ActivateFloatingMenu(lastSelected);
            }

            if (Input.GetMouseButtonDown(0) && lastSelected !=null)
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
        else if (hit.collider == null)
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
        if (lastSelected != null && floatingMenu.activeSelf)
            floatingMenu.SetActive(false);

        lastSelected = null;

    }
}

