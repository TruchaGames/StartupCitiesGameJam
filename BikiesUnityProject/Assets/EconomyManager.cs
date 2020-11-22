using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [Header("Starting Economy")]
    public uint starting_money = 100000;
    public uint starting_vans = 1;

    [Header("Costs")]
    public uint bike_individual_cost = 1000;
    public uint bike_station_cost = 30000;
    public uint van_cost = 1000;

    [Header("Income")]
    public uint bike_rent_income = 1000;

    [Range(0.0f,1.0f)]
    public float refund_percentage = 0.5f;

    [Header("Internal values")]
    //Money that the PLAYER has available!!
    [SerializeField]
    private int wallet = 0;

    //Vans that the PLAYER has available!!
    [SerializeField]
    private uint vans = 0;

    [Header("Audio Events")]
    public AK.Wwise.Event pay_money;
    public AK.Wwise.Event receive_money;
    public AK.Wwise.Event play_music;
    public AK.Wwise.Event stop_all;

    // Start is called before the first frame update
    void Start()
    {
        wallet = (int)starting_money;
        vans = starting_vans;

        play_music.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This function returns TRUE on a succesfull buy operation, if the player can't afford the station FALSE is returned and no money is expended
    public bool BuyBikeStation()
    {
        if (wallet >= bike_station_cost)
        {
            wallet -= (int)bike_station_cost;
            Debug.Log("Bought bike station!");
            pay_money.Post(gameObject); // Play Audio
            return true;
        }
        Debug.Log("Couldn't buy a bike station!");
        return false;
    }

    private void OnDestroy()
    {
        stop_all.Post(gameObject);
    }

    //This function returns TRUE on a succesfull buy operation of a single bike, if the player can't afford the station FALSE is returned and no money is expended
    public bool BuyNewBike()
    {
        if(wallet >= bike_individual_cost)
        {
            wallet -= (int)bike_individual_cost;
            Debug.Log("Bought a new bike!");
            pay_money.Post(gameObject); // Play Audio
            return true;
        }
        Debug.Log("Couldn't buy a new bike!");
        return false;
    }

    public void BuyNewBikeUI()
    {
        if (wallet >= bike_individual_cost)
        {
            wallet -= (int)bike_individual_cost;
            Debug.Log("Bought a new bike!");
            pay_money.Post(gameObject); // Play Audio
            return;
        }
        Debug.Log("Couldn't buy a new bike!");
        return;
    }

    public bool BuyNewVan()
    {
        if (wallet >= van_cost)
        {
            wallet -= (int)van_cost;
            Debug.Log("Bought a new van!");
            pay_money.Post(gameObject); // Play Audio
            return true;
        }
        Debug.Log("Couldn't buy a new van!");
        return false;
    }

    public void BuyNewVanUI()
    {
        if (wallet >= van_cost)
        {
            wallet -= (int)van_cost;
            Debug.Log("Bought a new van!");
            pay_money.Post(gameObject); // Play Audio
            return;
        }
        Debug.Log("Couldn't buy a new van!");
        return;
    }

    public void BikeRentIncome()
    {
        wallet += (int)bike_rent_income;
    }

    public uint AddVan()
    {
        return ++vans;
    }

    public uint RemoveVan()
    {
        return --vans;
    }

    public void AddWalletMoney(int ammount)
    {
        wallet += ammount;
        receive_money.Post(gameObject);
    }

    //This utility function (To consult) Returns True if the user can buy a bije station, but won't spend money
    public bool CanBuyNewBikeStation()
    {
        if (wallet >= bike_station_cost)
        {
            return true;
        }
        return false;
    }

    public bool CanBuyNewBike()
    {
        if (wallet >= bike_individual_cost)
        {
            return true;
        }
        return false;
    }

    public bool CanBuyNewVan()
    {
        if (wallet >= van_cost)
        {
            return true;
        }
        return false;
    }

    public int RefundRemovedBikeStation()
    {
        int money_refunded = (int)(bike_station_cost * refund_percentage);
        wallet += money_refunded;
        receive_money.Post(gameObject);
        return money_refunded;
    }

    public int getWallet()
    {
        return wallet;
    }
    public uint getVans()
    {
        return vans;
    }
}
