using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;
    public float randomX;
    public float yPosition;
    public float randomZ;
    public int maxSpawn;
    [Space]
    [Header("Characteristic")]
    public float maxHeight;
    public float minHeight;

    public Transform parent;
    public Transform[] children;

    private void Start()
    {
        for (int i = 0; i < maxSpawn; i++)
        {
            int randomIndex = Random.Range(0, prefabsToSpawn.Length);
            float randomPositionX = Random.Range(-randomX, randomX);
            float randomPositionZ = Random.Range(-randomZ, randomZ);


            Vector3 randomPosition = new Vector3(randomPositionX, yPosition, randomPositionZ);

            Instantiate(prefabsToSpawn[randomIndex], randomPosition, Quaternion.identity, parent);
        }

        children = parent.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            float height = Random.Range(minHeight, maxHeight);
            child.localScale = new Vector3(child.localScale.x, height, child.localScale.z);
        }
    }
}
