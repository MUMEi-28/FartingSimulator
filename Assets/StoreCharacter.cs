using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreCharacter : MonoBehaviour
{
    public Button buyButton;
    public Button useButton;
    public GameObject textUI;
    public bool isBought;

    [Header("Character Data")]
    public string characterData;

    public int currentEXP;
    public int price;
   
    private void Start()
    {
        textUI = transform.GetChild(0).GetChild(0).gameObject;

        if (PlayerPrefs.GetInt("Main0") == 0)
        {
            PlayerPrefs.SetInt("Main0", 1);
        }
        //      PlayerPrefs.SetInt(characterData, 0);    //  IF 1 THEN RETURN TRUE

        if (PlayerPrefs.GetInt(characterData) == 1) //  IF IS BOUGHT THEN RETURN TRUE
        {
            buyButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(true);

            textUI.gameObject.SetActive(false);
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            useButton.gameObject.SetActive(false);

            textUI.gameObject.SetActive(true);
        }
        //    IsCharacterBought();
        currentEXP = PlayerPrefs.GetInt("EXP");
    }
    private void Update()
    {
        SetInteractibleButtons();
    }
    private void SetInteractibleButtons()
    {
        // 500 > 300
        if (PlayerPrefs.GetInt("EXP") >= price)
        {
            buyButton.interactable = true;
        }
        else
        {
            buyButton.interactable = false;
        }

    }
    public void DeactivateBuyButton()   //  WHEN THE USER BUY THE BUY BUTTON WILL DEACTIVATE MAKING THE USE BUTTON TO SHOW
    {
        buyButton.gameObject.SetActive(false);
        useButton.gameObject.SetActive(true);

        IsCharacterBought();
        isBought = true;
        textUI.SetActive(false);    
    }
    public void IsCharacterBought()
    {
        isBought = PlayerPrefs.GetInt(characterData) == 1;

        print(PlayerPrefs.GetInt(characterData));
    }
    public void SetCharacterBought()
    {
        PlayerPrefs.SetInt(characterData, 1);    //  IF 1 THEN RETURN TRUE
        print(PlayerPrefs.GetInt(characterData));
    }

}

