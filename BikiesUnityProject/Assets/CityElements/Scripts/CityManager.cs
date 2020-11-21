using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum to know if we are CREATING  a station or MOVING one that we already have
enum BuildingMode
{
    NONE = 0,
    REPLACING,
    CREATING
}

public class CityManager : MonoBehaviour
{ 
    [Header("Game parameters")]
    // -- Amount of active apartments & interest points at the start of the game
    public int startApartments = 0;
    public int startInterestPoints = 0;

    // -- Activating period of the unactive apartments/interest points, in seconds
    // -- Explanation: Every "apartmentActivationPeriod" seconds, an unactive Apartment will activate. Same with interest points
    public float apartmentActivationPeriod = 30.0f;
    public float interestPointActivationPeriod = 30.0f;

    // -- Timers
    private float apartmentActivationTimer = 0.0f;
    private float interestPointActivationTimer = 0.0f;
    
    // -- Bike station prefab. Will be used to instantiate new ones when we want to create new stations
    public GameObject stationPrefab;

    //  --Lists of elements
    [Header("Lists of Nodes")]
    public List<BikeStation> bikeStations;
    private List<Apartment> unactiveApartments;
    private List<InterestPoint> unactiveInterestPoints;

    public List<Apartment> activeApartments = new List<Apartment>();
    public List<InterestPoint> activeInterestPoints = new List<InterestPoint>();


    // -- BUILDING TOOLS 
    private bool placingBikeStation = false;
    // -- When we want to add or move a bike station, this object will be the one we will refer to

    private GameObject stationBeingplaced;
    private BuildingMode buildingMode = BuildingMode.NONE;

    //In case we move a station, we save its initial position, in case we want to cancel and get it back to where it previously was
    private Vector3 stationStartPosition;

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

        if (placingBikeStation)
            PlaceBikeStation();

        //DEBUG
        if (Input.GetKeyDown(KeyCode.Space) && !placingBikeStation)
            CreateBikeStation();
        //DEBUG
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
        if (apartmentActivationTimer > apartmentActivationPeriod)
        {                                                                   //Check timers and periods
            apartmentActivationTimer = 0.0f;                                //Reset timer
            ActivateApartment();                                            //Activate
        }

        if (interestPointActivationTimer > interestPointActivationPeriod)    //Check timers and periods
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

        // Call to fill the list of near bike stations
        _apartment.ConnectBikeStations();
    }

    // -- Activates a random unactive InterestPoint  
    private void ActivateInterestPoint()
    {
        if (unactiveInterestPoints.Count <= 0)
            return;

        InterestPoint _interestPoint = unactiveInterestPoints[Random.Range(0, unactiveInterestPoints.Count)];

        //Activate the apartment and add it to the list of active apartments
        _interestPoint.Activate();
        activeInterestPoints.Add(_interestPoint);

        //Remove it from the list of unactive apartments
        unactiveInterestPoints.Remove(_interestPoint);

        // Call to fill the list of near bike stations
        _interestPoint.ConnectBikeStations();
    }

    // -- Call this function when we want the player to create a new station
    public void CreateBikeStation()
    {
        stationBeingplaced = Instantiate(stationPrefab);
        placingBikeStation = true;
        buildingMode = BuildingMode.CREATING;
    }

    // -- Call this function when we want to move a station
    public void MoveBikeStation(GameObject station)
    {
        stationBeingplaced = station;
        placingBikeStation = true;
        buildingMode = BuildingMode.REPLACING;
    }

    // --  Method to place bike stations in the map
    private void PlaceBikeStation()
    {
        int rayLayer = 1 << 9;

        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo,100f,rayLayer);

        Debug.DrawLine(Camera.main.ScreenPointToRay(Input.mousePosition).origin , hitInfo.point);
        Debug.Log(hitInfo.point);

        if (hit)
        {
            stationBeingplaced.transform.position = hitInfo.point;
        }

        // -- LEFT click to place the station
        if (Input.GetMouseButtonDown(0) /*&& position available -- need to check if the station fits in the current position*/)
        {
            placingBikeStation = false;
            //Coordinate all points to include the station in their list, etc,etc.

        }

        // -- RIGHT click or ESCAPE to CANCEL
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            placingBikeStation = false;
            
            if (buildingMode == BuildingMode.CREATING)
            {
                //MANAGE LISTS IF NEEDED
                Destroy(stationBeingplaced);
            }
            else if (buildingMode == BuildingMode.REPLACING)
            {
                //MANAGE LISTS IF NEEDED
                stationBeingplaced.transform.position = stationStartPosition;
            }
        }
    }
}
