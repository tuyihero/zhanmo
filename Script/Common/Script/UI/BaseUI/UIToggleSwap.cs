using UnityEngine;
using System.Collections;

public class UIToggleSwap : MonoBehaviour
{

    public GameObject _ToggleOnGO;
    public GameObject _ToggleOffGO;

    public void OnToggleSwap(bool isOn)
    {
        if (isOn)
        {
            _ToggleOnGO.SetActive(true);
            _ToggleOffGO.SetActive(false);
        }
        else
        {
            _ToggleOnGO.SetActive(false);
            _ToggleOffGO.SetActive(true);
        }
    }
}
