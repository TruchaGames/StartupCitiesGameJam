using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public GameObject destPoint;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SphereCollider sphere = destPoint.GetComponent<SphereCollider>();

        agent.destination = destPoint.transform.position;

        if (sphere != null && agent != null)
        {
            Vector2 random_unit_circle = Random.insideUnitCircle;
            Vector3 pos = destPoint.transform.position;
            Vector3 dest = new Vector3(random_unit_circle.x, 0.0f, random_unit_circle.y) * sphere.radius;
            agent.destination = pos + dest;
        }
        else
            Debug.LogError("THE BIKE STATION HAS NOT A SPHERE COLLIDER OR AGENT HAS NOT AN AGENT COMPONENT!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
