using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Apartment : CityElement
{
    [Header("Cyclist UI")]
    uint[] cyclistTypeAmount = new uint[5];
    public TextMeshProUGUI[] cyclistTypeAmountText;
    public Image[] cyclistTimerImg;

    [Header("Cyclist Instance")]
    public GameObject Cyclist;

    [Header("Cyclist Spawn")]
    public float SpawnRadius = 5.0f;
    public float timeToSpawn = 5.0f;
    float cyclistSpawnedAt = 0.0f;

    public Queue<AIAgent> cyclistsWaiting = new Queue<AIAgent>();
    public AIAgent[] cyclistWaitList;

    void Start()
    {
        cityManager = FindObjectOfType<CityManager>();
        cyclistSpawnedAt = Time.time;

        ////Debug purpose: Update sphere size
        //Vector3 scale = area.transform.localScale;
        //Vector3 pos = area.transform.position;
        //
        ////scale.y = Random.Range(1.1f, 7.82f);
        //scale.y = Random.Range(6.0f, 10.0f);
        //pos.y += scale.y/2;
        //
        //area.transform.position = pos;
        //area.transform.localScale = scale;
        area.SetActive(false);

        for (int i = 0; i < cyclistTypeAmount.Length; ++i)
        {
            cyclistTypeAmount[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Spawn Cyclists
        if (Time.time - cyclistSpawnedAt > timeToSpawn)
        {
            cyclistSpawnedAt = Time.time;
            SpawnCyclist();
        }

        // Send Cyclists to a BikeStation
        if (cyclistsWaiting.Count > 0)  // TODO-UI: Show UI of amount of cyclists waiting in an apartement, their wanted destination, and the waiting time of each (use list = queue.ToList())
        {
            foreach (BikeStation bikeStation in nearbyBikeStations)
            {
                if (bikeStation.bikeStock > 0)  //IMPROVE: Search for closed instead of picking first available
                {
                    AIAgent cyclist = cyclistsWaiting.Dequeue();
                    cyclist.SetDestination(bikeStation.gameObject, bikeStation.ArriveRadius);
                    cyclist.AgentStatus = AIAgent.AGENT_STATUS.WALKING;
                }
            }

            cyclistWaitList = cyclistsWaiting.ToArray();
        }

        for (InterestPoint.InterestPointType IP_type = InterestPoint.InterestPointType.IP_NONE + 1; IP_type != InterestPoint.InterestPointType.IP_MAX; ++IP_type)
        {
            foreach (AIAgent cyclist in cyclistWaitList)
            {
                if (cyclist.finalDestination.interestPointType == IP_type)
                {
                    RunCyclistTimer(cyclistTimerImg[(int)IP_type], cyclist);
                    break;
                }
            }
        }
    }

    private void SpawnCyclist() //TODO-UI: Momentarily show that a new cyclyst has spawned
    {
        // 1. Instantiate Cyclist around nearby, set position and destination
        AIAgent new_cyclist = Instantiate(Cyclist).GetComponent<AIAgent>();
        Vector2 random_circle = Random.insideUnitCircle * SpawnRadius;
        new_cyclist.gameObject.transform.position = gameObject.transform.position + new Vector3(random_circle.x, 0.0f, random_circle.y);

        // 2. Mark origin of cyclist
        new_cyclist.sourceApartment = this;
        EnqueueCyclist(new_cyclist);

        // 3. Pick a random destination (interest point)
        int IPIndex = Random.Range(0, cityManager.activeInterestPoints.Count - 1);
        new_cyclist.finalDestination = cityManager.activeInterestPoints[IPIndex];
    }

    public uint ConnectBikeStations()
    {
        uint nodesConnected = 0;

        // 1. Iterate list of all existing bike bases
        foreach (BikeStation bikeStation in cityManager.bikeStations)
        {
            // 2. Do a A->B vector from this base to each existing base
            if (InsideRadius(bikeStation.transform.position, bikeStationDetectRadius))
            {
                // 3. If magnitude of A->B is <= to radius, then add to correspodant list
                nearbyBikeStations.Add(bikeStation);

                // 4. Also, add yourself to the list of other newly connected node
                bikeStation.nearbyApartments.Add(this);

                ++nodesConnected;
            }
        }

        return nodesConnected;
    }

    public void EnqueueCyclist(AIAgent cyclist)
    {
        cyclistsWaiting.Enqueue(cyclist);
        int it = (int)cyclist.finalDestination.interestPointType;
        ++cyclistTypeAmount[it];
        cyclistTypeAmountText[it].text = cyclistTypeAmount[it].ToString();
        cyclistWaitList = cyclistsWaiting.ToArray();
    }

    public void DequeueCyclist(AIAgent cyclist)
    {
        cyclistsWaiting.Dequeue();
        int it = (int)cyclist.finalDestination.interestPointType;
        --cyclistTypeAmount[it];
        cyclistTypeAmountText[it].text = cyclistTypeAmount[it].ToString();
        cyclistWaitList = cyclistsWaiting.ToArray();
    }

    void RunCyclistTimer(Image img, AIAgent cyclist)
    {
        float timePassed = Time.time - cyclist.startedWaitingAt;
        if (timePassed < cyclist.waitTimeLimit)
            img.fillAmount = (cyclist.waitTimeLimit - timePassed) / cyclist.waitTimeLimit;
    }
}
