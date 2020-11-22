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
    public TextMeshProUGUI vanText;
    public uint vans;

    public bool vanMovement;
    public bool originSelected;
    public bool destinationSelected;
    public BikeStation originPoint;
    public BikeStation destinationPoint;
    public AIVan vanPrefab;


    public TextMeshProUGUI bikesToMoveText;
    public uint bikesToMove = 1;
    public uint maxBikes;





    // Start is called before the first frame update
    void Awake()
    {
        economyManager = FindObjectOfType<EconomyManager>();
        coins = economyManager.getWallet();
        coinText.SetText(coins.ToString());
        vans = economyManager.getVans();
        vanText.SetText(vans.ToString());
        bikesToMoveText.SetText(bikesToMove.ToString());

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivateVanMode(false);
        }

        if (vanMovement)
        {
            Debug.Log("aaa");
            Ray vanRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit vanHit;

            if (Physics.Raycast(vanRay, out vanHit, Mathf.Infinity, detectionMask))
            {
                if (vanHit.collider.gameObject.GetComponent<BikeStation>() != null)
                {
                    if (Input.GetMouseButtonDown(0) && !originSelected)
                    {
                        originPoint = vanHit.collider.gameObject.GetComponent<BikeStation>();
                        originSelected = true;
                    }
                    else if (Input.GetMouseButtonDown(0) && originSelected)
                    {
                        destinationPoint = vanHit.collider.gameObject.GetComponent<BikeStation>();
                        destinationSelected = true;
                    }


                }
            }

            if (originSelected && destinationSelected && originPoint != destinationPoint)
            {
                AIVan van = Instantiate(vanPrefab, originPoint.transform.position, Quaternion.identity);
                van.origin = originPoint;
                van.destination = destinationPoint;
                van.bikesToLoad = bikesToMove;
                economyManager.RemoveVan();
                ActivateVanMode(false);
            }
            if (originSelected && destinationSelected && originPoint == destinationPoint)
            {

                ActivateVanMode(false);
            }


        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, detectionMask))
        {
            if (hit.collider.tag == "UnderConstruction")
                return;

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

            if (Input.GetMouseButtonDown(0) && lastSelected != null && !vanMovement)
            {
                // Aqui se comprara una bici
                if (lastSelectedBuyPanel != null && !lastSelectedBuyPanel.activeSelf)
                {
                    ActivateFloatingMenu(lastSelectedBuyPanel);
                }
                else if (lastSelectedBuyPanel != null && lastSelectedBuyPanel.activeSelf)
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
        vans = economyManager.getVans();
        vanText.SetText(vans.ToString());
        bikesToMoveText.SetText(bikesToMove.ToString());



    }

    public void ActivateVanMode(bool state)
    {
        vanMovement = state;

        if (state == false)
        {
            originSelected = false;
            destinationSelected = false;
            originPoint = null;
            destinationPoint = null;
        }
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

    public void AddBikesToLoad()
    {
        if (bikesToMove < maxBikes)
        {
            bikesToMove++;

        }
    }

    public void DecreaseBikesToLoad()
    {
        if (bikesToMove > 1)
        {
            bikesToMove--;

        }
    }
}

