using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PolutionBar : MonoBehaviour
{

    public Slider slider;

    private int uiPolution;
    private int currentPolution = 50;
    public int maxPolution = 100;

    public int polutionIncrease = 10;
    public int polutionDecrease = 10;

    public GameObject winScreen;
    public GameObject loseScreen;

    private void Awake()
    {

        SetMaxPolution(maxPolution);
    }


    // this is only for debug
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentPolution += 5;
            SetPolution(currentPolution);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {

            currentPolution -= 5;
            SetPolution(currentPolution);
        }

        if (currentPolution <= slider.minValue)
        {
            LoseCondition();
        }
        else if (currentPolution >= slider.maxValue)
        {
            WinCondition();
        }

        //SetPolution(uiPolution);
    }

    public void SetPolution(int polution)
    {
        slider.value = polution;
    }

    public void IncreasePolution()
    {
        slider.value += polutionIncrease;
    }

    public void DecreasePolution()
    {
        slider.value += polutionDecrease;
    }

    public void SetMaxPolution(int maxPolution)
    {
        slider.maxValue = maxPolution;
        currentPolution = (int)(maxPolution * 0.5f);
        uiPolution = (int)(maxPolution * 0.5f);
    }

    void CurrentPolution(int globalPolution)
    {
        uiPolution = globalPolution;
    }

    public void LoseCondition()
    {
        if (!loseScreen.activeSelf)
        {
            loseScreen.SetActive(true);
        }

        gameObject.SetActive(false);
    
    }


    public void WinCondition()
    {
        if (!winScreen.activeSelf)
        {
            winScreen.SetActive(true);
        }

        gameObject.SetActive(false);

    }


}
