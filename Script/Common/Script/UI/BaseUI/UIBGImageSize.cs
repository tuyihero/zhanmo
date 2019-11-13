using UnityEngine;
using System.Collections;

public class UIBGImageSize : MonoBehaviour
{
    private RectTransform _RectTransform;
    private Vector2 _OrgSize;
    private static Vector2 _UIScale = new Vector2(1280, 760);

	void Awake ()
    {
        _RectTransform = GetComponent<RectTransform>();
        if (_RectTransform == null)
        {
            gameObject.SetActive(false);
            return;
        }
        _OrgSize = _RectTransform.sizeDelta;
        Debug.Log("ScreenSize:" + Screen.width + "," + Screen.height);
    }

	void Update ()
    {
        SetScreenSize();
    }

    private void SetScreenSize()
    {
        float sizePersent = _OrgSize.x / _OrgSize.y;
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        float screenPersent = screenSize.x / screenSize.y;
        
        if (sizePersent < screenPersent)
        {
            float sizeRate = _UIScale.y / Screen.height;
            _RectTransform.sizeDelta = new Vector2(Screen.width * sizeRate, Screen.width / sizePersent * sizeRate);
        }
        else
        {
            float sizeRate = _UIScale.x / Screen.width;
            _RectTransform.sizeDelta = new Vector2(Screen.height * sizePersent * sizeRate, Screen.height * sizeRate);
        }
    }
}
