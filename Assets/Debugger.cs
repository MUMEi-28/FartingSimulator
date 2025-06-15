using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    private void Start()
    {
        print(PlayerPrefs.GetInt("EXP"));
    }
}
