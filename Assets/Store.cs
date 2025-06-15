using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Store : MonoBehaviour
{
    [Header("Fart")]
    public TMP_Text maxFartText;
    public int fartPrice;
    public Button fartButton;
    public float maxFart;

    public TMP_Text currentLevelText;
    public int currentXP;
    
    public bool deleteKey;
    private void Start()
    {
        if (deleteKey)
        {
            PlayerPrefs.DeleteAll();
            Debug.LogWarning("DATA DELETED");
        }
        GetCurrentLevel();

        if (PlayerPrefs.HasKey("CurrentFart"))
        {
            maxFart = PlayerPrefs.GetFloat("CurrentFart");
            print(true);
        }
        else
        {
            PlayerPrefs.SetFloat("CurrentFart", maxFart);
            print(false);
        }

        maxFartText.text = maxFart.ToString();

        UpdateCurrentLevelText();
    }
    public void Update()
    {
        if (PlayerPrefs.GetInt("EXP") >= fartPrice)
        {
            fartButton.interactable = true;
        }
        else
        {
            fartButton.interactable = false;
        }
    }
    public void BuyFart()
    {
        //    GetCurrentLevel();
        maxFart += 1.0f;
        PlayerPrefs.SetFloat("CurrentFart", maxFart);

        maxFartText.text = maxFart.ToString();

        ReduceXP(fartPrice);
     //   PlayerPrefs.Save();   

        //ADD MORE FART TIME
        
    }
    public void BuyCharacter(int price)
    {
        ReduceXP(price);
    }
    public void SetCharacter(int characterIndex)
    {
        PlayerPrefs.SetInt("CurrentCharacter", characterIndex);     //  WILL BE USED LATER FOR CHARACTER CHANGING
        print(PlayerPrefs.GetInt("CurrentCharacter"));
    }
   
    private void ReduceXP(int price)
    {
        currentXP -= price;

        PlayerPrefs.SetInt("EXP", currentXP);

        UpdateCurrentLevelText();
    }


    private void UpdateCurrentLevelText()
    {
        currentLevelText.text = GetCurrentLevel().ToString();
    }
    private int GetCurrentLevel()
    {
        return currentXP = PlayerPrefs.GetInt("EXP");
    }
}
