using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimaSprite : MonoBehaviour
{
    public List<string> _TexturePathes;
    public List<SpriteRenderer> _SpriteBones;

    private Dictionary<string, List<SpriteRenderer>> _SpriteRenders;

    public void Start()
    {
        SetBoneSprites();
    }

    public void InitBoneSprites()
    {
        if (_SpriteRenders != null)
        {
            return;
        }

        _SpriteRenders = new Dictionary<string, List<SpriteRenderer>>();
        if (_SpriteBones == null || _SpriteBones.Count == 0)
        {
            var spriteBones = transform.GetComponentsInChildren<MeshFilter>(true);
            foreach (var spriteBone in spriteBones)
            {
                var boneGO = spriteBone.gameObject;
                GameObject.DestroyImmediate(spriteBone);
                GameObject.DestroyImmediate(boneGO.GetComponent<MeshRenderer>());
                var spriteRender = boneGO.AddComponent<SpriteRenderer>();
                _SpriteBones.Add(spriteRender);
                
                if (!_SpriteRenders.ContainsKey(boneGO.name))
                {
                    _SpriteRenders.Add(boneGO.name, new List<SpriteRenderer>());
                }
                _SpriteRenders[boneGO.name].Add(spriteRender);
            }
        }

        //_SpriteRenders = new Dictionary<string, SpriteRenderer>();

    }

    public void SetBoneSprites()
    {
        InitBoneSprites();


        foreach (var texturePath in _TexturePathes)
        {

            var spriteItems = Resources.LoadAll<Sprite>(texturePath);
            foreach (var spriteItem in spriteItems)
            {
                if (_SpriteRenders.ContainsKey(spriteItem.name))
                {
                    foreach (var spriteRender in _SpriteRenders[spriteItem.name])
                    {
                        spriteRender.sprite = spriteItem;
                    }
                }
            }
        }


    }
}
