using UnityEngine;
using System.Collections;

public class UIInstanceBase<T> : UIBase
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

    public void OnEnable()
    {
        _Instance = this.GetComponent<T>();
    }

    public void OnDisable()
    {
        _Instance = default(T);
    }

}
