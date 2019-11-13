using UnityEngine;
using System.Collections;

public class InstanceBase<T> : MonoBehaviour
{
    private static T _Instance;

    public static T Instance
    {
        get
        {
            return _Instance;
        }
    }

    public static void SetInstance(T instance)
    {
        _Instance = instance;
    }

}
