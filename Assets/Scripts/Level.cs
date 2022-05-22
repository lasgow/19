using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level instance;
    public GameObject player;
    private void Awake()
    {
        instance = this;
    }

}


