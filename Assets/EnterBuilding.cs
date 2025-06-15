using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterBuilding : MonoBehaviour
{
    private string sceneToMove;
    public GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Enter(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
        sceneToMove = sceneName;

        //   PlayerPrefs.SetInt(sceneName, 0);   // THIS MAKES SURE THAT THE CURRENT SCENE IS NOW FALSE
        //    Debug.LogWarning("PLAYE PREFS " + sceneToMove + " IS SET TO FALSE");
    }
    //  SAVE POSITION SHOULD ONLY BE USED WHEN ENTERING, WHEN USED ON EXITING THE EXIT POSITION WILL BE SAVED THEN WILL BE RELOADED ON NEXT SCENE
    public void SavePosition()
    {
        PlayerPrefs.SetFloat("PlayerPosX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", player.transform.position.z);

        print("SAVED POSITION");
        print(PlayerPrefs.GetFloat("PlayerPosX") + " " +
        PlayerPrefs.GetFloat("PlayerPosY") + " " +
        PlayerPrefs.GetFloat("PlayerPosZ"));
    }
    


    //  IF BUTTON CLICK THEN SET SCENE NAME PREFS TO TRUE
    //  
    /*  public void PlayerPrefsBoolean()
       {
          PlayerPrefs.SetInt(sceneToMove, 1); //  THIS SETS THE BOOLEAN
          Debug.LogWarning("PLAYE PREFS " + sceneToMove + " IS SET TO TRUE");
       }*/
}
