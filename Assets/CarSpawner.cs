using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] cars;
    public int maxCars;
    public float spawnTimer;
    private void Update()
    {
        if (transform.childCount < maxCars)
        {
            InvokeRepeating("Spawn", spawnTimer, spawnTimer);
        }
        else
        {
            CancelInvoke("Spawn");
        }
    }
    private void Spawn()
    {
        int randomInt = Random.Range(0, cars.Length);
        Instantiate(cars[randomInt], transform.position, Quaternion.identity, transform);
    }
}
