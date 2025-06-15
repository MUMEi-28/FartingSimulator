using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceController : MonoBehaviour
{
    public int currentLevel;
    public float currentXp = 0;
    public float xpNeededToLevelUp = 100f;
    public float xpMultiplier;
    public float sliderLerpSpeed;

    private FartMeter fart;
    public Slider slider;
    public TMP_Text levelText;
    private void Start()
    {
        SetCurrentLevel();

        fart = FindObjectOfType<FartMeter>();

        slider = transform.GetChild(0).GetComponent<Slider>();
        SetMaxFart();
        SetFartValue();
        SetLevel();
    }
    private void Update()
    {
        SetFartValue();
    }
    public void GainXpFarting(float xpToGain)
    {
        currentXp += xpToGain * Time.deltaTime;

        if (currentXp >= xpNeededToLevelUp)
        {
            LevelUp();
        }
    }  
    public void GainXpDefeating(float xpToGain)
    {
        while (xpToGain > 0)
        {
            float remainingXpToLevelUp = xpNeededToLevelUp - currentXp;

            if (xpToGain >= remainingXpToLevelUp)
            {
                xpToGain -= remainingXpToLevelUp;
                currentXp = 0;
                LevelUp();
            }
            else
            {
                currentXp += xpToGain;
                xpToGain = 0;
            }
        }
    }
    public void LevelUp()
    {
        currentXp -= xpNeededToLevelUp;
        currentLevel++;
    //    xpNeededToLevelUp = (currentLevel * xpMultiplier) * 100;      //  HARDER LEVELING SYSTEM


        SetMaxFart();
        SetLevel();
   //     AddMaxFartValue();
        UpdateExperienceMoney();
    }
    private void SetCurrentLevel()
    {
        //  THIS MAKES SURE THAT THE CURRENT LEVEL IS NOT RESETED TO ZERO
   //     currentLevel = PlayerPrefs.GetInt("EXP");

        if (PlayerPrefs.HasKey("EXP"))
        {
            currentLevel = PlayerPrefs.GetInt("EXP");
        }
        else
        {
            currentLevel = 0;
        }
    }
    private void UpdateExperienceMoney()
    {
        PlayerPrefs.SetInt("EXP", currentLevel);
    }

    private void SetMaxFart()
    {
        slider.maxValue = xpNeededToLevelUp;
    }
    private void SetFartValue()
    {
        float targetValue = currentXp;
        float currentValue = slider.value;
        float newValue = Mathf.Lerp(currentValue, targetValue, sliderLerpSpeed);
        slider.value = newValue;
    }
    private void SetLevel()
    {
        levelText.text = currentLevel.ToString();
    }
}
