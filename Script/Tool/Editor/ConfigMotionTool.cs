using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ConfigMotionTool
{
    [MenuItem("Assets/LoadAsset/LoadAnimations")]
    static void LoadAnimations()
    {
        LoadAnimationsInner();
    }

    [MenuItem("Assets/LoadAsset/CreateMotion")]
    static void CreateMotion()
    {
        CreateMotionInner();
    }

    #region 

    private static void LoadAnimationsInner()
    {
        var animations = GameObject.FindObjectsOfType<Animation>();
        foreach (var animation in animations)
        {
            CreateAnim(animation);
        }
    }

    private static void CreateAnim(Animation anim)
    {
        string animPath = Application.dataPath + "\\FightCraft\\Res\\Model\\Animation\\" + anim.name + "\\";
        if (!Directory.Exists(animPath))
            Directory.CreateDirectory(animPath);

        var enumerator = anim.GetEnumerator();
        List<AnimationClip> animClipList = new List<AnimationClip>();
        while (enumerator.MoveNext())
        {
            AnimationState animState = enumerator.Current as AnimationState;
            AnimationClip animClip = GameObject.Instantiate(animState.clip) as AnimationClip;
            animClip.name = animState.clip.name;
            AssetDatabase.CreateAsset(animClip, animPath + animClip.name + ".anim");
            animClipList.Add(animClip);
        }

        //var go = anim.gameObject;
        //GameObject.DestroyImmediate(anim);
        //var newAnim = go.AddComponent<Animation>();
        //foreach (var clip in animClipList)
        //{
        //    newAnim.AddClip(clip, clip.name);
        //}
    }

    #endregion

    #region CreateMotionInner

    private static void CreateMotionInner()
    {
        var animFoldPath = Application.dataPath + "\\FightCraft\\Res\\Model\\Animation\\";
        var animFolds = Directory.GetDirectories(animFoldPath);
        foreach (var animFold in animFolds)
        {
            if (animFold.Contains("Hero"))
            {
                CreateHeroMotion(animFold.Replace(Application.dataPath, "Assets"));
            }
        }
    }

    private static void CreateHeroMotion(string animFold)
    {
        var animPaths = Directory.GetFiles(animFold);
        Dictionary<string, AnimationClip> animClips = new Dictionary<string, AnimationClip>();
        foreach (var animPath in animPaths)
        {
            var anim = AssetDatabase.LoadAssetAtPath<AnimationClip>(animPath);
            if (anim != null)
            {
                animClips.Add(anim.name, anim);
            }
        }

        var modelBase = Resources.Load<GameObject>("ModelBase/_ModelBase");
        var modelInstance = GameObject.Instantiate<GameObject>(modelBase);
        if (modelInstance == null)
        {
            Debug.LogError("ModelBaseError");
            return;
        }

        modelInstance.name = Path.GetFileName(animFold);
        var motionBase = modelInstance.GetComponent<BaseMotionManager>();
        motionBase._IdleAnim = animClips["Act_Stand_01"];
        motionBase._MoveAnim = animClips["Act_Move_01"];
        motionBase._HitAnim = animClips["Act_Hit_01"];
        motionBase._FlyAnim = animClips["Act_Fly_01"];
        motionBase._RiseAnim = animClips["Act_Rise_01"];
    }

    #endregion
}
