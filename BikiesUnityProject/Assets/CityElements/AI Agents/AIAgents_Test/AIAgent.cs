﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    GameObject NextDestination;
    public InterestPoint finalDestination;

    NavMeshAgent m_Agent;

    [Header("Agent Patience")]
    public float waitTimeLimit = 10.0f;
    public float startedWaitingAt = 0.0f;

    // AI Status
    public enum AGENT_STATUS {
        NONE = -1,
        APT_WAIT,
        WALKING,
        BIKE_WAIT,
        TRAVELLING,
        ARRIVING
    };
    [SerializeField]
    AGENT_STATUS AgentStatus = AGENT_STATUS.NONE;

    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        startedWaitingAt = Time.time;
        AgentStatus = AGENT_STATUS.WALKING;
    }

    private void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        startedWaitingAt = Time.time;
        AgentStatus = AGENT_STATUS.WALKING;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_Agent == null)
            m_Agent = GetComponent<NavMeshAgent>();

        switch (AgentStatus)
        {
            // Agent waits around the apartement
            case AGENT_STATUS.APT_WAIT:
                if (Time.time - startedWaitingAt > waitTimeLimit)
                {
                    Destroy(gameObject);
                    //TODO-Lucho: Augmentar polució, eliminate agent, etc.
                }
                break;

            // Agent Walks from Apartment to Bike Station A
            case AGENT_STATUS.WALKING:
                if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
                {
                    NextDestination.GetComponent<BikeStation>().waitingCyclists.Enqueue(this);
                    startedWaitingAt = Time.time;
                    AgentStatus = AGENT_STATUS.BIKE_WAIT;
                }
                break;

            // Agent waits around the bike station
            case AGENT_STATUS.BIKE_WAIT:
                if (Time.time - startedWaitingAt > waitTimeLimit)
                {
                    NextDestination.GetComponent<BikeStation>().waitingCyclists.Dequeue();
                    Destroy(gameObject);
                    //TODO-Lucho: Augmentar polució, eliminate agent, etc.
                }
                break;

            // Agent is travelling from Bike Station A to Bike Station B
            case AGENT_STATUS.TRAVELLING:
                if (m_Agent.remainingDistance <= m_Agent.stoppingDistance)
                    ChangeDestination(finalDestination.gameObject, AGENT_STATUS.ARRIVING, finalDestination.ArriveRadius);
                break;

            // Agent walks from Bike Station B to Destination
            case AGENT_STATUS.ARRIVING:
                //if (m_Agent.pathStatus == NavMeshPathStatus.PathComplete)
                    //Destroy(gameObject);
                break;

            case AGENT_STATUS.NONE:
                Debug.LogError("Agent has STATUS::NONE!");
                break;

            default:
                break;
        }
    }

    // Called Upon Agent Spawn or when Arrives at bike station
    public void ChangeDestination(GameObject destination, AGENT_STATUS agent_next_status, float arrive_radius)
    {
        AgentStatus = agent_next_status;

        if (m_Agent == null)
            m_Agent = GetComponent<NavMeshAgent>();

        if (destination != null)
        {
            NextDestination = destination;

            Vector2 random_unit_circle = Random.insideUnitCircle;
            Vector3 dest = new Vector3(random_unit_circle.x, 0.0f, random_unit_circle.y) * arrive_radius;
            m_Agent.destination = NextDestination.transform.position + dest;
        }
        else
        {
            Debug.LogError("Destination was NULL!");
            AgentStatus = AGENT_STATUS.NONE;
        }
    }

    public void SetFinalDestination(InterestPoint destination)
    {
        if(destination != null)
            finalDestination = destination;
    }
}