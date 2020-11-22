using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIVan : MonoBehaviour
{
    NavMeshAgent m_Agent;

    EconomyManager economy;

    [Header("Van Velocity")]
    public float speed = 0.0f;

    [Header("Van Load")]
    [SerializeField]
    uint bikeLoad = 0;
    public uint bikesToLoad = 5;    //Should be given in the Player script
    public float vanLoadCooldown = 1.0f;
    float vanStartedLoadingAt = 0.0f;

    [Header("Van Destination")]
    public BikeStation origin;
    public BikeStation destination;

    // AI Status
    public enum VAN_STATUS {
        NONE = -1,
        LOADING,
        TRAVELLING,
        UNLOADING
    };
    [SerializeField]
    VAN_STATUS vanStatus = VAN_STATUS.NONE;

    // Start is called before the first frame update
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.isStopped = true;
        vanStatus = VAN_STATUS.LOADING;
        vanStartedLoadingAt = Time.time;

        economy = FindObjectOfType<EconomyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(vanStatus)
        {
            // Van loads bikes that was asked to load
            case VAN_STATUS.LOADING:    // TODO-UI: Show state of loading van + number of bikes loaded
                if (Time.time - vanStartedLoadingAt > vanLoadCooldown)
                {
                    if (origin.bikeStock > 0 && bikesToLoad > 0)
                    {
                        --origin.bikeStock;
                        --bikesToLoad;
                        ++bikeLoad;
                        vanStartedLoadingAt = Time.time;
                    }

                    if (origin.bikeStock <= 0 || bikesToLoad <= 0)
                    {
                        m_Agent.isStopped = false;
                        m_Agent.destination = destination.transform.position;
                        vanStatus = VAN_STATUS.TRAVELLING;
                    }
                }
                break;

            // Agent Walks from Apartment to Bike Station A
            case VAN_STATUS.TRAVELLING:    // TODO-UI: Show number of bikes loaded
                if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
                {
                    m_Agent.isStopped = true;
                    vanStartedLoadingAt = Time.time;
                    vanStatus = VAN_STATUS.UNLOADING;
                }
                break;

            // Agent waits around the bike station
            case VAN_STATUS.UNLOADING:    // TODO-UI: Show state of unloading van + number of bikes loaded
                if (Time.time - vanStartedLoadingAt > vanLoadCooldown)
                {
                    if (bikeLoad > 0)
                    {
                        --bikeLoad;
                        if (destination.bikeStock < destination.maxBikes)
                            ++destination.bikeStock;
                        //else
                            //TODO-UI: Show icon of bike lost in the overall bike economy
                        vanStartedLoadingAt = Time.time;
                    }

                    if (bikeLoad <= 0)
                    {
                        economy.AddVan();
                        Destroy(gameObject);
                    }
                }
                break;

            case VAN_STATUS.NONE:
                Debug.LogError("Van has STATUS::NONE!");
                break;

            default:
                break;
        }
    }

    // -- EXAMPLE CODE TO CREATE A VAN
    //if (Input.GetKeyDown(KeyCode.R))
    //{
    //    AIVan newVan = Instantiate(vanGO).GetComponent<AIVan>();

    //    newVan.origin = stationStart;
    //    newVan.destination = stationEnd;

    //    newVan.bikesToLoad = 5;

    //    newVan.transform.position = newVan.origin.transform.position;
    //}
}