using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTransfer : MonoBehaviour
{

    public static int Score { get; set; }
    private static DataTransfer instance = null;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
