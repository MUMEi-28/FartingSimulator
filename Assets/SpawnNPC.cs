using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNPC : MonoBehaviour
{
    private Transform NPCparent;
    private Transform spawnArea;
    public GameObject[] NPCS;
    private int randomColorNpc;

    public float spawnWaitTime;
    public int maxNpcNumber;
    private int NpcSize;
    private void Awake()
    {   
        NPCparent = transform.GetChild(0);
        spawnArea = transform.GetChild(0);

        randomColorNpc = Random.Range(0, NPCS.Length);
        NpcSize = Random.Range(1, maxNpcNumber);

        for (int i = 0; i < NpcSize; i++)
        {
            Instantiate(NPCS[randomColorNpc], spawnArea.position, Quaternion.identity, NPCparent);
        }
    }
    private void Update()
    {
        if (NPCparent.childCount < NpcSize)
        {
            Invoke(nameof(SpawnNpcTime), spawnWaitTime);
        }
    }
    private void SpawnNpcTime()
    {
            Instantiate(NPCS[randomColorNpc], spawnArea.position, Quaternion.identity, NPCparent);
            CancelInvoke(nameof(SpawnNpcTime));
    }
}
