using UnityEngine;
using System.Collections;

public class Managers : MonoBehaviour
{
    private static Managers _instance;

    public static T GetManager<T>() where T : Manager
    {
        return (T) _instance.GetComponent<T>();
    }

    public Managers()
    {
        _instance = this;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
