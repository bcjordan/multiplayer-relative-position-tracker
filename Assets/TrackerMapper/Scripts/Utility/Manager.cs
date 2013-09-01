using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
