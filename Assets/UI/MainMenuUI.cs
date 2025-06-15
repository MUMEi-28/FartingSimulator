using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private void Start()
    {
        //  THIS MAKES SURE THAT THE PlayerScenePositioning MAINMENU PLAYERPREFS IS TRUE
        //    PlayerPrefs.SetInt("MainMenu", 1);
        //     Debug.LogWarning("MAIN MENU PLAYER PREF SET TO TRUE");

        //  THIS MAKES SURE THAT WHEN USER RETURN TO MAIN MENU AND WHEN GOING TO MAP 1 SCENE THE PLAYER POSITION WILL BE IN THE HOUSE
       /* PlayerPrefs.DeleteKey("PlayerPosX");
        PlayerPrefs.DeleteKey("PlayerPosY");
        PlayerPrefs.DeleteKey("PlayerPosZ");*/
       

    }
    public void Play()
    {
  //      SceneManager.LoadSceneAsync("Map 1");

        //      SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;    //  MAKE SURE THE GAME IS NOT PAUSED
    }
    
    public void ReturnMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");

        Time.timeScale = 1f;    //  MAKE SURE THE GAME IS NOT PAUSED


        PlayerPrefs.Save();//   MAKES SURE THAT ALL THE DATAS CURRENTLY IS SAVED BEFORE GOING TO MAIN MENU
    }
    public void Quit()
    {
        Application.Quit();
        PlayerPrefs.Save();//   MAKES SURE THAT ALL THE DATAS CURRENTLY IS SAVED BEFORE QUITING
    }

}
