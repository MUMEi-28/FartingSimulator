using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreloadManager : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // The name of the scene to load after preloading is complete.
    [SerializeField] private float delayTime = 2f; // The amount of time to delay before loading the scene.
    public Slider loadingSlider;
    private IEnumerator Start()
    {
        // Load the scene additively in the background.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Update the loading progress bar until the scene is fully loaded.
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("Loading progress: " + progress);

            // Update the loading progress UI here...
            loadingSlider.value = progress * 100;

            // Wait for the next frame to continue.
            yield return null;
        }

        loadingSlider.value = 100;

        // Wait for a delay before allowing the scene to activate.
        yield return new WaitForSeconds(delayTime);

        // Activate the loaded scene.
        asyncLoad.allowSceneActivation = true;

        // Unload the Preload Scene.
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        print("UNLOAD");
        print("UNLOAD");
        print("UNLOAD");
        print("UNLOAD");
        print("UNLOAD");
    }
}
