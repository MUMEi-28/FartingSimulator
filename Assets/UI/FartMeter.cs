using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FartMeter : MonoBehaviour
{
    public float maxFart;
    public float currentFart;

    private Slider slider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("CurrentFart"))
        {
            maxFart = PlayerPrefs.GetFloat("CurrentFart");
        }
        else
        {
            maxFart = 3;
        }
        currentFart = maxFart;

        slider = GetComponentInChildren<Slider>();

        SetMaxFart();
    }
    public void SetMaxFart()
    {
        slider.maxValue = maxFart;
        slider.value = currentFart;

        PlayerPrefs.SetFloat("CurrentFart", maxFart);
    }
    public void ReduceFart()
    {
        currentFart -= Time.deltaTime;
        UpdateFartMeter();  
    }
    public void UpdateFartMeter()
    {
       slider.value = currentFart;
    }
}
