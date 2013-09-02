using UnityEngine;
using System.Collections;

public class Manager : uLink.MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
