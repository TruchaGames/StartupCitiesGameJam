using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIVan : MonoBehaviour
{
    NavMeshAgent m_Agent;

    [Header("Van Velocity")]
    public float speed = 0.0f;

    [Header("Van Load")]
    public uint bikesToLoad = 0;    //Should be given in the Player script
    uint bikeLoad = 0;
    public float vanLoadCooldown = 0.0f;
    float vanStartedLoadingAt = 0.0f;

    [Header("Van Destination")]
    BikeStation origin;
    BikeStation destination;

    // AI Status
    public enum VAN_STATUS {
        NONE = -1,
        LOADING,
        TRAVELLING,
        UNLOADING
    };

    VAN_STATUS vanStatus = VAN_STATUS.NONE;

    // Start is called before the first frame update
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        vanStatus = VAN_STATUS.LOADING;
        vanStartedLoadingAt = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        switch(vanStatus)
        {
            // Van loads bikes that was asked to load
            case VAN_STATUS.LOADING:
                if (Time.time - vanStartedLoadingAt > vanLoadCooldown)
                {
                    if (origin.bikeStock > 0 && bikesToLoad > 0)
                    {
                        --origin.bikeStock;
                        --bikesToLoad;
                        ++bikeLoad;
                    }

                    if (origin.bikeStock <= 0 || bikesToLoad <= 0)
                    {
                        vanStatus = VAN_STATUS.TRAVELLING;
                        //TODO-Lucho: Que la van faci pathfinding a destination.
                    }
                }
                break;

            // Agent Walks from Apartment to Bike Station A
            case VAN_STATUS.TRAVELLING:
                if (m_Agent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    vanStartedLoadingAt = Time.time;
                    vanStatus = VAN_STATUS.UNLOADING;
                }
                break;

            // Agent waits around the bike station
            case VAN_STATUS.UNLOADING:
                if (Time.time - vanStartedLoadingAt > vanLoadCooldown)
                {
                    if (bikeLoad > 0)
                    {
                        --bikeLoad;
                        ++origin.bikeStock;
                    }

                    if (bikeLoad <= 0)
                    {
                        //TODO-Carles: ++PlayerVans
                        Destroy(this);
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
}