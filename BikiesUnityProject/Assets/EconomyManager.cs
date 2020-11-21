﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [Header("Economy Balance & Costs")]
    public uint starting_money = 100000;
    public uint bike_station_cost = 30000;
    public uint bike_individual_cost = 1000;
    [Range(0.0f,1.0f)]
    public float refund_percentage = 0.5f;

    [Header("Internal values")]
    //Money that the PLAYER has available!!
    [SerializeField]
    private int wallet = 0;

    // Start is called before the first frame update
    void Start()
    {
        wallet = (int)starting_money;
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
            return true;
        }
        Debug.Log("Couldn't buy a bike station!");
        return false;
    }

    //This function returns TRUE on a succesfull buy operation of a single bike, if the player can't afford the station FALSE is returned and no money is expended
    public bool BuyNewBike()
    {
        if(wallet >= bike_individual_cost)
        {
            wallet -= (int)bike_individual_cost;
            Debug.Log("Bought a new bike!");
            return true;
        }
        Debug.Log("Couldn't buy a new bike!");
        return false;
    }

    public void AddWalletMoney(int ammount)
    {
        wallet += ammount;
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

    public int RefundRemovedBikeStation()
    {
        int money_refunded = (int)(bike_station_cost * refund_percentage);
        wallet += money_refunded;
        return money_refunded;
    }
}