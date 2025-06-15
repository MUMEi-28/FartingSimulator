using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    private void Start()
    {
        ResetPlayerPositioning();
    }
    private void ResetPlayerPositioning()
    {
        //  MAKES SURE THAT WHEN RELOADING FROM SCENE THE PLAYER POSITION WILL BE RESETTED TO THE HOUSE
        PlayerPrefs.DeleteKey("PlayerPosX");
        PlayerPrefs.DeleteKey("PlayerPosY");
        PlayerPrefs.DeleteKey("PlayerPosZ");
    }
    public void LoadScene(int sceneID)
    {
        StartCoroutine(LoadSceneAsync(sceneID));
    //    LoadSceneAsync(sceneID);
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        
        //LOADING SCREEN ACTIVE

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            // SLIDER UPDATE
            print(progressValue);
            yield return null;
        }
    }
}
