using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    GameObject NextDestination;
    public GameObject FinalDestination;
    NavMeshAgent m_Agent;

    [Header("Agent Patience")]
    public float waitTimeLimit = 0.0f;
    float startedWaitingAt = 0.0f;

    // AI Status
    public enum AGENT_STATUS { NONE = -1, APT_WAIT, WALKING, BIKE_WAIT, TRAVELLING, ARRIVING };
    AGENT_STATUS AgentStatus = AGENT_STATUS.NONE;

    // Start is called before the first frame update
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        AgentStatus = AGENT_STATUS.APT_WAIT;
    }

    // Update is called once per frame
    void Update()
    {
        switch(AgentStatus)
        {
            // Agent waits around the apartement
            case AGENT_STATUS.APT_WAIT:
                break;

            // Agent Walks from Apartment to Bike Station A
            case AGENT_STATUS.WALKING:
                if (m_Agent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    NextDestination.GetComponent<BikeStation>().waitingCyclists.Enqueue(this);

                    AgentStatus = AGENT_STATUS.BIKE_WAIT;
                }
                break;

            // Agent waits around the bike station
            case AGENT_STATUS.BIKE_WAIT:
                if (Time.time - startedWaitingAt > waitTimeLimit)
                {
                    NextDestination.GetComponent<BikeStation>().waitingCyclists.Dequeue();
                    //TODO-Lucho: Augmentar polució, eliminate agent, etc.
                }
                break;

            // Agent is travelling from Bike Station A to Bike Station B
            case AGENT_STATUS.TRAVELLING:
                if (m_Agent.pathStatus == NavMeshPathStatus.PathComplete)
                    ChangeDestination(FinalDestination, AgentStatus = AGENT_STATUS.ARRIVING);
                break;

            // Agent walks from Bike Station B to Destination
            case AGENT_STATUS.ARRIVING:
                if (m_Agent.pathStatus == NavMeshPathStatus.PathComplete)
                    Destroy(gameObject);
                break;

            case AGENT_STATUS.NONE:
                Debug.LogError("Agent has STATUS::NONE!");
                break;

            default:
                break;
        }
    }

    // Called Upon Agent Spawn or when Arrives at bike station
    public void ChangeDestination(GameObject destination, AGENT_STATUS agent_next_status)
    {
        AgentStatus = agent_next_status;

        if (destination != null)
        {
            NextDestination = destination;
            SphereCollider sphere = NextDestination.GetComponent<SphereCollider>();

            if (sphere != null)
            {
                Vector2 random_unit_circle = Random.insideUnitCircle;
                Vector3 dest = new Vector3(random_unit_circle.x, 0.0f, random_unit_circle.y) * sphere.radius;

                if(m_Agent == null)
                    m_Agent = GetComponent<NavMeshAgent>();

                m_Agent.destination = NextDestination.transform.position + dest;
            }
            else
            {
                Debug.LogError("DESTINATION PASSED HAS NOT A SPHERE COLLIDER OR AGENT HAS NOT AN AGENT COMPONENT!");
                AgentStatus = AGENT_STATUS.NONE;
                return;
            }
        }
    }
}