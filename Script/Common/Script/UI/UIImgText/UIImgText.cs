using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIImgText : MonoBehaviour
{
    public UIImgFont _ImgFont;

    protected string _text = "";
    [SerializeField]
    public string text
    {
        get
        {
            return _text;
        }
        set
        {
            _text = value;
            ShowImage(_text);
        }
    }

    protected Transform _CharRoot;
    protected void InitCharRoot()
    {
        if (_CharRoot != null)
            return;

        _CharRoot = transform.Find("CharRoot");

        if (_CharRoot == null)
        {
            var charRoot = new GameObject("CharRoot");

            var horizon = charRoot.AddComponent<HorizontalLayoutGroup>();
            horizon.childAlignment = TextAnchor.UpperLeft;
            horizon.childForceExpandHeight = false;
            horizon.childForceExpandWidth = false;
            var layoutElement = charRoot.AddComponent<LayoutElement>();
            layoutElement.ignoreLayout = true;

            _CharRoot = charRoot.transform;
            _CharRoot.SetParent(transform);
            _CharRoot.localPosition = Vector3.zero;
            _CharRoot.localRotation = Quaternion.Euler(Vector3.zero);
            _CharRoot.localScale = Vector3.one;
        }
    }

    #region

    protected Stack<Image> _IdleImgs = new Stack<Image>();

    protected Image PopIdleImage()
    {
        InitCharRoot();
        if (_IdleImgs == null || _IdleImgs.Count == 0)
        {
            var imageGO = new GameObject("charImg");
            var image = imageGO.AddComponent<Image>();
            imageGO.transform.SetParent(_CharRoot);
            imageGO.transform.localPosition = Vector2.zero;
            imageGO.transform.localRotation = Quaternion.Euler(Vector3.zero);
            imageGO.transform.localScale = Vector3.one;
            image.gameObject.SetActive(true);
            return image;
        }
        else
        {
            var image = _IdleImgs.Pop();
            image.gameObject.SetActive(true);
            return image;
        }
    }

    protected List<Image> _CharImages = new List<Image>();

    protected void ClearImage()
    {
        for (int i = 0; i < _CharImages.Count; ++i)
        {
            _CharImages[i].gameObject.SetActive(false);
            _IdleImgs.Push(_CharImages[i]);
        }
        _CharImages.Clear();
    }

    protected void ShowImage(string text)
    {
        ClearImage();
        _ImgFont.InitChars();
        for (int i = 0; i < text.Length; ++i)
        {
            var image = PopIdleImage();
            if (!_ImgFont._DictImgChars.ContainsKey(text[i]))
            {
                Debug.LogError("No Img Char:" + text[i]);
                continue;
            }
            var charImg = _ImgFont._DictImgChars[text[i]];
            image.sprite = charImg._Image;
            image.rectTransform.sizeDelta = new Vector2(charImg._CharWidth, charImg._CharHeight);
            image.rectTransform.SetAsLastSibling();

            _CharImages.Add(image);
        }
    }
    #endregion
}
