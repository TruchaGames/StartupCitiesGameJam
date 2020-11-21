using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    // ~~~~ PROVISIONAL ~~~~~~
    
    // -- Amount of active apartments & interest points at the start of the game
    public int startApartments =0;
    public int startInterestPoints=0;

    // -- Activating period of the unactive apartments /interest points, in seconds
    // -- Explanation: Every "apartmentActivationPeriod" seconds, an unactive Apartment will activate. Same with interest points
    public float apartmentActivationPeriod = 30.0f;
    public float interesPointActivationPeriod = 30.0f;

    // -- Timers
    private float apartmentActivationTimer = 0.0f;
    private float interestPointActivationTimer = 0.0f;

    // ~~~~ PROVISIONAL ~~~~~~

    private List<Apartment> unactiveApartments;
    private List<InterestPoint> unactiveInterestPoints;

    private List<Apartment> activeApartments = new List<Apartment>();
    private List<InterestPoint> activeInterestPoints = new List<InterestPoint>();

    // Start is called before the first frame update
    void Start()
    {
        //Set activation timers to 0
        apartmentActivationTimer = 0.0f;
        interestPointActivationTimer = 0.0f;

        //  -- Add all the Apartments & InteresPoints to the lists
        unactiveApartments = new List<Apartment>(GetComponentsInChildren<Apartment>());
        unactiveInterestPoints = new List<InterestPoint>(GetComponentsInChildren<InterestPoint>());
        
        //  -- Activate some points if we need to
        
        //First apartments
        for (int i = 0; i < startApartments; ++i)
            ActivateApartment();

        //Then interest points
        for (int i = 0; i < startInterestPoints; ++i)
            ActivateInterestPoint();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateActivationTimers();
        CheckForActivations();
    }

    // -- Update timers
    private void UpdateActivationTimers()
    {
        apartmentActivationTimer += Time.deltaTime;
        interestPointActivationTimer += Time.deltaTime;
    }

    // -- Check activation timers and activate points if we have to
    private void CheckForActivations()
    {
        if (apartmentActivationTimer > apartmentActivationPeriod){          //Check timers and periods
            apartmentActivationTimer = 0.0f;                                //Reset timer
            ActivateApartment();                                            //Activate
        }

        if (interestPointActivationTimer > interesPointActivationPeriod)    //Check timers and periods
        {                                                                   
            interestPointActivationTimer = 0.0f;                            //Reset timer
            ActivateInterestPoint();                                        //Activate
        }
    }

    // -- Activates a random unactive Apartment
    private void ActivateApartment()
    {
        if (unactiveApartments.Count <= 0)
            return;

        Apartment _apartment = unactiveApartments[Random.Range(0, unactiveApartments.Count)];

        //Activate the apartment and add it to the list of active apartments
        _apartment.Activate();
        activeApartments.Add(_apartment);

        //Remove it from the list of unactive apartments
        unactiveApartments.Remove(_apartment);
    }

    // -- Activates a random unactive InterestPoint  
    private void ActivateInterestPoint()
    {
        if (unactiveInterestPoints.Count <= 0)
            return;

        InterestPoint _interesPoint = unactiveInterestPoints[Random.Range(0, unactiveInterestPoints.Count)];

        //Activate the apartment and add it to the list of active apartments
        _interesPoint.Activate();
        activeInterestPoints.Add(_interesPoint);

        //Remove it from the list of unactive apartments
        unactiveInterestPoints.Remove(_interesPoint);
    }
}
