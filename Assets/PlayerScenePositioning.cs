using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScenePositioning : MonoBehaviour
{
    public Vector3 mainHouse;
    private void Awake()
    {
        /*PlayerPrefs.DeleteKey("PlayerPosX");
        PlayerPrefs.DeleteKey("PlayerPosY");
        PlayerPrefs.DeleteKey("PlayerPosZ");*/
        if (!PlayerPrefs.HasKey("PlayerPosX") && !PlayerPrefs.HasKey("PlayerPosY") && !PlayerPrefs.HasKey("PlayerPosZ"))
        {
            PlayerPrefs.SetFloat("PlayerPosX", mainHouse.x);
            PlayerPrefs.SetFloat("PlayerPosY", mainHouse.y);
            PlayerPrefs.SetFloat("PlayerPosZ", mainHouse.z);

            transform.position = mainHouse;
            print("PLAYER HAS NO SAVED KEY");
        }
        else
        {
            ReloadPosition();
            print("RELOADED POSITION");
        }
    }
    //  SAVES THE POSITION OF THE PLAYER 
    private void ReloadPosition()
    {
        float playerPosX = PlayerPrefs.GetFloat("PlayerPosX");
        float playerPosY = PlayerPrefs.GetFloat("PlayerPosY");
        float playerPosZ = PlayerPrefs.GetFloat("PlayerPosZ");

        print(playerPosX + "\"" + playerPosY + "\"" + playerPosZ);
        transform.position = new Vector3(playerPosX, playerPosY, playerPosZ);
    }
}
