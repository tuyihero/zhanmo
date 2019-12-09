using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Xml;
using UnityEditor.Animations;

public class AnimaToUnity : MonoBehaviour
{
    [MenuItem("TFImage/splitSprite")]
    public static void SplitSprite()
    {
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (var selectGO in selection)
        {
            if (selectGO is Texture2D)
            {
                var texturePath = AssetDatabase.GetAssetPath(selectGO);
                Debug.Log("texturePath:" + texturePath);
                var importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Multiple;

                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
                
                List<SpriteMetaData> spriteMetaDatas = ReadPListInfo(texturePath.Replace(".png", ".plist"), texture.width, texture.height);
                //List<SpriteMetaData> spriteMetaDatas = new List<SpriteMetaData>();

                //SpriteMetaData spriteData = new SpriteMetaData();
                //spriteData.alignment = 1;
                //spriteData.name = "weapon_1";
                //spriteData.rect = new Rect(381 , 2, 378, 64);
                //spriteData.pivot = new Vector2(378, 64);

                //spriteMetaDatas.Add(spriteData);

                importer.spritesheet = spriteMetaDatas.ToArray();

                Debug.Log("importer.spritesheet:" + importer.spritesheet.Length);

                importer.SaveAndReimport();
            }
        }

    }

    [MenuItem("TFImage/spritePovit")]
    public static void SpritePovit()
    {
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        Dictionary<string, List<AnimationClip>> animations = new Dictionary<string, List<AnimationClip>>();
        foreach (var selectGO in selection)
        {
            var resPath = AssetDatabase.GetAssetPath(selectGO);
            if (!resPath.EndsWith(".plist"))
                continue;

            List<SpriteMetaData> spriteMetaDatas = ReadPListPovit(resPath);

            foreach (var spriteData in spriteMetaDatas)
            {
                string newFoldName = Path.GetFileNameWithoutExtension(resPath);
                newFoldName = newFoldName.Substring(0, newFoldName.IndexOf('_'));

                string imgPath = resPath.Replace(Path.GetFileName(resPath), newFoldName + "/" + spriteData.name);
                var importer = AssetImporter.GetAtPath(imgPath) as TextureImporter;
                if (importer == null)
                {
                    Debug.LogError("importer error:" + imgPath + "," + resPath);
                    continue;
                }
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritesheet = new SpriteMetaData[1]{ spriteData};
                importer.spritePivot = spriteData.pivot;
                TextureImporterSettings texSetting = new TextureImporterSettings();
                importer.ReadTextureSettings(texSetting);
                texSetting.spritePivot = spriteData.pivot;
                texSetting.spriteAlignment = (int)SpriteAlignment.Custom;
                importer.SetTextureSettings(texSetting);
                importer.SaveAndReimport();
            }

            string foldName = Path.GetDirectoryName(resPath);
            if (!animations.ContainsKey(foldName))
            {
                animations.Add(foldName, new List<AnimationClip>());
            }
            //animations[foldName].Add(BuildAnimationClip(resPath.Replace(".plist", ""), Path.GetDirectoryName(resPath)));
        }

        //foreach (var anim in animations)
        //{
        //    BuildAnimationController(anim.Value, anim.Key + "/" + Path.GetFileNameWithoutExtension(anim.Key));
        //}
    }

    [MenuItem("TFImage/CreateAnimClip")]
    public static void CreateAnimClip()
    {
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel);
        List<AnimationClip> animations = new List<AnimationClip>();
        string animFold = "";
        foreach (var selectGO in selection)
        {
            var texturePath = AssetDatabase.GetAssetPath(selectGO);
            string foldPath = Application.dataPath + texturePath.Replace("Assets", "");
            var directs = Directory.GetDirectories(foldPath, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var direct in directs)
            {
                var foldName = Path.GetFileNameWithoutExtension(direct);
                if (foldName == "effect")
                    continue;

                animations.Add(BuildAnimationClip(direct, texturePath));
            }
            animFold = texturePath;
        }
        if (animations.Count > 0)
        {
            BuildAnimationController(animations, animFold + "/" + Path.GetFileNameWithoutExtension(animFold));
        }

    }

    public static List<SpriteMetaData> ReadAtlasInfo(string path)
    {
        Debug.Log("ReadAtlasInfo:" + path);
        List<SpriteMetaData> spriteMetaDatas = new List<SpriteMetaData>();

        if (!File.Exists(path))
        {
            Debug.Log("ReadAtlasInfo error:" + path);
            return spriteMetaDatas;
        }
        StreamReader streamReader = new StreamReader(path);
        var emptyLine = streamReader.ReadLine();
        var resourceName = streamReader.ReadLine();
        var imgSizeStr = streamReader.ReadLine().Split(':')[1].Trim(' ').Split(',');
        Vector2 imgSize = new Vector2(int.Parse(imgSizeStr[0].Trim(' ')), int.Parse(imgSizeStr[1].Trim(' ')));
        var format = streamReader.ReadLine();
        var filter = streamReader.ReadLine();
        var repeat = streamReader.ReadLine();

        int idx = 0;
        SpriteMetaData newSprite = new SpriteMetaData();
        while (!streamReader.EndOfStream)
        {
            ++idx;
            newSprite = new SpriteMetaData();

            newSprite.alignment = 0;
            newSprite.name = streamReader.ReadLine().Trim(' ');
            var rotate = streamReader.ReadLine().Contains("true");
            var posStr = streamReader.ReadLine().Split(':')[1].Trim(' ').Split(',');
            Vector2 pos = new Vector2(int.Parse(posStr[0].Trim(' ')), int.Parse(posStr[1].Trim(' ')));
            var sizeStr = streamReader.ReadLine().Split(':')[1].Trim(' ').Split(',');
            Vector2 size = new Vector2(int.Parse(sizeStr[0].Trim(' ')), int.Parse(sizeStr[1].Trim(' ')));
            if (rotate)
            {
                newSprite.rect = new Rect(pos[0], imgSize[1] - pos[1] - size[0], size[1], size[0]);
            }
            else
            {
                newSprite.rect = new Rect(pos[0], imgSize[1] - pos[1] - size[1], size[0], size[1]);
            }
            var pivot = streamReader.ReadLine().Split(':')[1].Trim(' ').Split(',');
            newSprite.pivot = new Vector2(0.5f, 0.5f);
            var offset = streamReader.ReadLine();
            var index = streamReader.ReadLine();
            newSprite.border = new Vector4(0, 0, 0, 0);

            spriteMetaDatas.Add(newSprite);
        }
        streamReader.Close();

        return spriteMetaDatas;
    }

    public static List<SpriteMetaData> ReadPListInfo(string path, int imgWidth, int imgHeight)
    {
        if (!path.Contains(".plist"))
        {
            return null;
        }

        List<SpriteMetaData> SplitList = new List<SpriteMetaData>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);
        var FramesNode = xmlDoc.SelectSingleNode("plist/dict");
        if (FramesNode == null)
            return null;

        XmlNode frameRootNode = null;
        foreach (XmlNode node in FramesNode.ChildNodes)
        {
            if (node.InnerText == "frames")
            {
                frameRootNode = node.NextSibling;
                break;
            }
        }
        if (frameRootNode == null)
            return null;

        var keys = frameRootNode.SelectNodes("key");
        int baseOffsetX = 0, baseOffsetY = 0;
        foreach (XmlNode keyNode in keys)
        {
            SpriteMetaData splitInfo = new SpriteMetaData();
            splitInfo.name = Path.GetFileName(keyNode.InnerText);

            XmlNode valuesNode = keyNode.NextSibling;
            XmlNode subKeyNode = valuesNode.FirstChild;
            int width = 0, height = 0, x = 0, y = 0,offsetX = 0, offsetY = 0;
           
            bool rotate = false;
            while (subKeyNode != null && subKeyNode.NextSibling != null)
            {
                XmlNode valueNode = subKeyNode.NextSibling;
                if (subKeyNode.InnerText == "width")
                    width = int.Parse(valueNode.InnerText);
                if (subKeyNode.InnerText == "height")
                    height = int.Parse(valueNode.InnerText);
                if (subKeyNode.InnerText == "x")
                    x = int.Parse(valueNode.InnerText);
                if (subKeyNode.InnerText == "y")
                    y = int.Parse(valueNode.InnerText);
                if (subKeyNode.InnerText == "offset")
                {
                    string frameStr = valueNode.InnerText;
                    //frameStr = frameStr.Substring(2, frameStr.Length - 4);
                    string[] framStrs = frameStr.Split(',');
                    offsetX = int.Parse(framStrs[0].Trim('{', '}'));
                    offsetY = int.Parse(framStrs[1].Trim('{', '}'));

                    if (baseOffsetX == 0 && baseOffsetY == 0)
                    {
                        baseOffsetX = offsetX;
                        baseOffsetY = offsetY;
                    }
                }
                if (subKeyNode.InnerText == "frame")
                {
                    string frameStr = valueNode.InnerText;
                    //frameStr = frameStr.Substring(2, frameStr.Length - 4);
                    string[] framStrs = frameStr.Split(',');
                    x = int.Parse(framStrs[0].Trim('{', '}'));
                    y = int.Parse(framStrs[1].Trim('{', '}'));
                    width = int.Parse(framStrs[2].Trim('{', '}'));
                    height = int.Parse(framStrs[3].Trim('{', '}'));
                }


                if (subKeyNode.InnerText == "rotated")
                {
                    if (valueNode.Name == "true")
                    {
                        int temp = width;
                        width = height;
                        height = temp;
                        rotate = true;
                    }
                }



                subKeyNode = valueNode.NextSibling;
            }

            splitInfo.alignment = (int)SpriteAlignment.Custom;
            splitInfo.rect = new Rect(x, imgHeight - y - height, width, height);
            if (rotate)
            {
                splitInfo.pivot = new Vector2(((float)(baseOffsetY - offsetY)) / width, (height * 0.5f - (baseOffsetX - offsetX)) / height);
            }
            else
            {
                splitInfo.pivot = new Vector2((width * 0.5f - offsetX) / width, (height * 0.5f - offsetY) / height);
            }
            Debug.Log("splitInfo.pivot:" + splitInfo.pivot);
            splitInfo.border = new Vector4(0, 0, 0, 0);

            SplitList.Add(splitInfo);
        }


        return SplitList;

    }

    public static List<SpriteMetaData> ReadPListPovit(string path)
    {
        if (!path.Contains(".plist"))
        {
            return null;
        }
        Debug.Log("ReadPListPovit:" + path);
        List<SpriteMetaData> SplitList = new List<SpriteMetaData>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);
        var FramesNode = xmlDoc.SelectSingleNode("plist/dict");
        if (FramesNode == null)
            return null;

        XmlNode frameRootNode = null;
        foreach (XmlNode node in FramesNode.ChildNodes)
        {
            if (node.InnerText == "frames")
            {
                frameRootNode = node.NextSibling;
                break;
            }
        }
        if (frameRootNode == null)
            return null;

        var keys = frameRootNode.SelectNodes("key");
        float baseOffsetX = 0, baseOffsetY = 0;
        foreach (XmlNode keyNode in keys)
        {
            SpriteMetaData splitInfo = new SpriteMetaData();
            splitInfo.name = Path.GetFileName(keyNode.InnerText);

            XmlNode valuesNode = keyNode.NextSibling;
            XmlNode subKeyNode = valuesNode.FirstChild;
            int width = 0, height = 0, x = 0, y = 0;
            float offsetX = 0, offsetY = 0;

            bool rotate = false;
            while (subKeyNode != null && subKeyNode.NextSibling != null)
            {
                XmlNode valueNode = subKeyNode.NextSibling;
                if (subKeyNode.InnerText == "width")
                    width = int.Parse(valueNode.InnerText);
                if (subKeyNode.InnerText == "height")
                    height = int.Parse(valueNode.InnerText);
                if (subKeyNode.InnerText == "x")
                    x = int.Parse(valueNode.InnerText);
                if (subKeyNode.InnerText == "y")
                    y = int.Parse(valueNode.InnerText);
                if (subKeyNode.InnerText == "offset")
                {
                    string frameStr = valueNode.InnerText;
                    //frameStr = frameStr.Substring(2, frameStr.Length - 4);
                    string[] framStrs = frameStr.Split(',');
                    offsetX = float.Parse(framStrs[0].Trim('{', '}'));
                    offsetY = float.Parse(framStrs[1].Trim('{', '}'));

                    if (baseOffsetX == 0 && baseOffsetY == 0)
                    {
                        baseOffsetX = offsetX;
                        baseOffsetY = offsetY;
                    }
                }
                if (subKeyNode.InnerText == "frame")
                {
                    string frameStr = valueNode.InnerText;
                    //frameStr = frameStr.Substring(2, frameStr.Length - 4);
                    string[] framStrs = frameStr.Split(',');
                    x = int.Parse(framStrs[0].Trim('{', '}'));
                    y = int.Parse(framStrs[1].Trim('{', '}'));
                    width = int.Parse(framStrs[2].Trim('{', '}'));
                    height = int.Parse(framStrs[3].Trim('{', '}'));
                }


                //if (subKeyNode.InnerText == "rotated")
                //{
                //    if (valueNode.Name == "true")
                //    {
                //        int temp = width;
                //        width = height;
                //        height = temp;
                //        rotate = true;
                //    }
                //}



                subKeyNode = valueNode.NextSibling;
            }

            splitInfo.alignment = (int)SpriteAlignment.Custom;
            //if (rotate)
            //{
            //    splitInfo.pivot = new Vector2(((float)(baseOffsetY - offsetY)) / width, (height * 0.5f - (baseOffsetX - offsetX)) / height);
            //}
            //else
            {
                Vector2 pivotPix = new Vector2(((int)(width * 0.5f) - offsetX), ((int)(height * 0.5f) - offsetY));
                Debug.Log("pivotPix:" + pivotPix);
                splitInfo.pivot = new Vector2(((float)((int)(width * 0.5f) - offsetX)) / width, ((float)((int)(height * 0.5f) - offsetY)) / height);
            }
            Debug.Log("splitInfo.pivot:" + splitInfo.pivot);
            splitInfo.border = new Vector4(0, 0, 0, 0);

            SplitList.Add(splitInfo);
        }


        return SplitList;

    }

    public static AnimationClip BuildAnimationClip(string imageDirect, string animPath)

    {

        string animationName = Path.GetFileNameWithoutExtension(imageDirect);

        //查找所有图片，因为我找的测试动画是.jpg

        string[] images = Directory.GetFiles(imageDirect,"*.png");

        AnimationClip clip = new AnimationClip();
        
        AnimationUtility.SetAnimationType(clip, ModelImporterAnimationType.Generic);

        EditorCurveBinding curveBinding = new EditorCurveBinding();

        curveBinding.type = typeof(SpriteRenderer);

        curveBinding.path = "";

        curveBinding.propertyName = "m_Sprite";

        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[images.Length];

        //动画长度是按秒为单位，1/10就表示1秒切10张图片，根据项目的情况可以自己调节

        float frameTime = 1 / 10f;

        for (int i = 0; i < images.Length; i++)
        {

            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(DataPathToAssetPath(images[i]));

            keyFrames[i] = new ObjectReferenceKeyframe();

            keyFrames[i].time = frameTime * i;

            keyFrames[i].value = sprite;

        }

        //动画帧率，30比较合适

        clip.frameRate = 30;

        //有些动画我希望天生它就动画循环

        if (animationName.IndexOf("idle") >= 0)

        {

            //设置idle文件为循环动画

            SerializedObject serializedClip = new SerializedObject(clip);

            AnimationClipSettings clipSettings = new AnimationClipSettings();

            clipSettings.loopTime = true;

            serializedClip.ApplyModifiedProperties();

        }

        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);

        AssetDatabase.CreateAsset(clip, animPath + "/" + animationName + ".anim");

        AssetDatabase.SaveAssets();

        return clip;

    }

    static AnimatorController BuildAnimationController(List<AnimationClip> clips, string filePath)
    {

        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(filePath + ".controller");

        AnimatorControllerLayer layer = animatorController.layers[0];

        AnimatorStateMachine sm = layer.stateMachine;

        foreach (AnimationClip newClip in clips)

        {

            AnimatorState state = sm.AddState(newClip.name);

            state.motion = newClip;

        }

        AssetDatabase.SaveAssets();

        return animatorController;

    }

    static void BuildPrefab(string filePath, List<AnimationClip> anims)
    {

        //生成Prefab 添加一张预览用的Sprite
        GameObject go = new GameObject();

        go.name = Path.GetFileNameWithoutExtension(filePath);

        SpriteRenderer spriteRender = go.AddComponent<SpriteRenderer>();

        Animation animation = go.AddComponent<Animation>();

        foreach (var clip in anims)
        {
            animation.AddClip(clip, clip.name);
        }

        PrefabUtility.SaveAsPrefabAsset(go, filePath + "/" + go.name + ".prefab");

        DestroyImmediate(go);

    }

    public static string DataPathToAssetPath(string path)
    {
        //return path;
        if (Application.platform == RuntimePlatform.WindowsEditor)
            return path.Substring(path.IndexOf("Assets/"));
        //return path.Substring(path.IndexOf("Assets\\"));

        else

            return path.Substring(path.IndexOf("Assets/"));

    }

    #region scene

    public class MapImgInfo
    {
        public string fileName;
        public string filePath;
        public int fileIdx;
    }

    [MenuItem("TFImage/BuildMap")]
    public static void BuildMap()
    {
        GameObject mapGO = null;
        GameObject farGO1 = null, farGO2 = null, farGO3 = null, groundGO = null, nearGO = null;
        float lastPosFar1 = 0, lastPosFar2 = 0, lastPosFar3 = 0, lastPosGround = 0, lastPosNear = 0;
        List<Texture2D> textures = new List<Texture2D>();
        List<MapImgInfo> mapImgInfos = new List<MapImgInfo>();
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (var selectGO in selection)
        {
            if (selectGO is Texture2D)
            {
                var texturePath = AssetDatabase.GetAssetPath(selectGO);
                var fileName = Path.GetFileNameWithoutExtension(texturePath);
                MapImgInfo mapImgInfo = new MapImgInfo();
                mapImgInfo.fileName = fileName;
                mapImgInfo.filePath = texturePath;
                mapImgInfo.fileIdx = int.Parse(fileName.Replace("far", "").Replace("ground", "").Replace("near", ""));

                int insertIdx = 0;
                for (int i = 0; i < mapImgInfos.Count; ++i)
                {
                    if (mapImgInfos[i].fileIdx > mapImgInfo.fileIdx)
                    {
                        insertIdx = i;
                        break;
                    }
                    else
                    {
                        insertIdx = i + 1;
                    }
                }

                mapImgInfos.Insert(insertIdx, mapImgInfo);
            }
        }

        foreach (var mapImgInfo in mapImgInfos)
        {
            var texturePath = mapImgInfo.filePath;
            if (mapGO == null)
            {
                string goName = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(texturePath));
                mapGO = new GameObject(goName);
                mapGO.AddComponent<SceneAnimController>();
                mapGO.transform.position = new Vector3(0, 2.25f, 0);

                farGO3 = new GameObject("Far3");
                farGO3.transform.SetParent(mapGO.transform);
                farGO3.transform.localPosition = Vector3.zero;

                farGO2 = new GameObject("Far2");
                farGO2.transform.SetParent(mapGO.transform);
                farGO2.transform.localPosition = Vector3.zero;

                farGO1 = new GameObject("Far1");
                farGO1.transform.SetParent(mapGO.transform);
                farGO1.transform.localPosition = Vector3.zero;

                groundGO = new GameObject("Ground");
                groundGO.transform.SetParent(mapGO.transform);
                groundGO.transform.localPosition = Vector3.zero;

                nearGO = new GameObject("Near");
                nearGO.transform.SetParent(mapGO.transform);
                nearGO.transform.localPosition = Vector3.zero;
            }

            var fileName = Path.GetFileNameWithoutExtension(texturePath);
            GameObject imgGO = new GameObject(fileName);
            var spriteRender = imgGO.AddComponent<SpriteRenderer>();
            spriteRender.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);

            if (fileName.Contains("far3"))
            {
                imgGO.transform.SetParent(farGO3.transform);
                imgGO.transform.localPosition = new Vector3((lastPosFar3), 0, 0);
                lastPosFar3 += spriteRender.sprite.rect.width * 0.01f;
                spriteRender.sortingOrder = -30;
            }
            else if (fileName.Contains("far2"))
            {
                imgGO.transform.SetParent(farGO2.transform);
                imgGO.transform.localPosition = new Vector3((lastPosFar2), 0, 0);
                lastPosFar2 += spriteRender.sprite.rect.width * 0.01f;
                spriteRender.sortingOrder = -20;
            }
            else if (fileName.Contains("far1"))
            {
                imgGO.transform.SetParent(farGO1.transform);
                imgGO.transform.localPosition = new Vector3((lastPosFar1), 0, 0);
                lastPosFar1 += spriteRender.sprite.rect.width * 0.01f;
                spriteRender.sortingOrder = -10;
            }
            else if (fileName.Contains("ground"))
            {
                imgGO.transform.SetParent(groundGO.transform);
                imgGO.transform.localPosition = new Vector3((lastPosGround), 0, 0);
                lastPosGround += spriteRender.sprite.rect.width * 0.01f;
                spriteRender.sortingOrder = 0;
            }
            else if (fileName.Contains("near"))
            {
                imgGO.transform.SetParent(nearGO.transform);
                imgGO.transform.localPosition = new Vector3((lastPosNear), 0, 0);
                lastPosNear += spriteRender.sprite.rect.width * 0.01f;
                spriteRender.sortingOrder = 100;
            }

            
        }
    }



    #endregion

    #region combine image fold

    [MenuItem("TFImage/combineFold")]
    public static void CombineFold()
    {
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (var selectGO in selection)
        {
            if (selectGO is AnimatorController)
            {
                var texturePath = AssetDatabase.GetAssetPath(selectGO);
                string foldPath = Path.GetDirectoryName(Application.dataPath + texturePath.Replace("Assets", "")) + "/";

                var directs = Directory.GetDirectories(Path.GetDirectoryName(foldPath), "*.*", SearchOption.TopDirectoryOnly);
                Dictionary<string, List<string>> newDirects = new Dictionary<string, List<string>>();
                foreach (var direct in directs)
                {
                    string directName = Path.GetFileNameWithoutExtension(direct);
                    var directNameSplits = directName.Split('_');
                    if (!newDirects.ContainsKey(directNameSplits[0]))
                    {
                        newDirects.Add(directNameSplits[0], new List<string>());
                    }
                    newDirects[directNameSplits[0]].AddRange(Directory.GetFiles(direct, "*.png", SearchOption.TopDirectoryOnly));
                }

                foreach (var newDirect in newDirects)
                {
                    string newFold = foldPath + "/" + newDirect.Key;
                    Directory.CreateDirectory(newFold);
                    foreach (var imagePath in newDirect.Value)
                    {
                        File.Move(imagePath, newFold + "/" + Path.GetFileName(imagePath));
                    }
                }

                foreach (var oldDirect in directs)
                {
                    try
                    {
                        Directory.Delete(oldDirect);
                    }
                    catch (System.Exception e)
                    { }
                }
            }
            
        }

    }

    [MenuItem("TFImage/ClearFold")]
    public static void ClearFold()
    {
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (var selectGO in selection)
        {
            if (selectGO is AnimatorController || selectGO is AnimationClip)
            {
                var texturePath = AssetDatabase.GetAssetPath(selectGO);
                string foldPath = Application.dataPath + texturePath.Replace("Assets", "");
                File.Delete(foldPath);
            }
            else if (selectGO is DefaultAsset)
            {
                try
                {
                    var texturePath = AssetDatabase.GetAssetPath(selectGO);
                    string foldPath = Application.dataPath + texturePath.Replace("Assets", "");
                    Directory.Delete(foldPath);
                }
                catch (System.Exception e)
                { }
            }

        }

    }

    [MenuItem("TFImage/CreatePrefab")]
    public static void CreatePrefab()
    {
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        Dictionary<string, AnimatorController> effectControllerDict = new Dictionary<string, AnimatorController>();

        foreach (var selectGO in selection)
        {
            if (selectGO is AnimatorController)
            {
                if (selectGO.name.Equals("effect"))
                {
                    var effectAnimController = AssetDatabase.GetAssetPath(selectGO);
                    effectControllerDict.Add(Path.GetDirectoryName(Path.GetDirectoryName(effectAnimController)), selectGO as AnimatorController);
                    continue;
                }
            }
        }

        foreach (var selectGO in selection)
        {
            if (selectGO is AnimatorController)
            {
                if (selectGO.name.Equals("effect"))
                {
                    continue;
                }

                var nameSplits = selectGO.name.Split('_');
                string prefabName = "Boss" + nameSplits[nameSplits.Length - 1];

                GameObject montionBase = new GameObject(prefabName);
                montionBase.AddComponent<MotionManager>();

                GameObject model = new GameObject(prefabName);
                GameObject bodyAnim = new GameObject("Body");
                bodyAnim.transform.SetParent(model.transform);
                var bodyAimator = bodyAnim.AddComponent<Animator>();
                bodyAimator.runtimeAnimatorController = selectGO as RuntimeAnimatorController;
                bodyAnim.AddComponent<SpriteRenderer>();

                GameObject effectAnim = new GameObject("Effect");
                effectAnim.transform.SetParent(bodyAnim.transform);
                effectAnim.transform.localPosition = Vector3.zero;
                effectAnim.transform.localRotation = Quaternion.Euler(Vector3.zero);
                effectAnim.transform.localScale = new Vector3(2, 2, 2);
                var texturePath = AssetDatabase.GetAssetPath(selectGO);
                if (effectControllerDict.ContainsKey(Path.GetDirectoryName(texturePath)))
                {
                    var effectAnimController = effectControllerDict[Path.GetDirectoryName(texturePath)];
                    var effectAimator = effectAnim.AddComponent<Animator>();
                    effectAimator.runtimeAnimatorController = effectAnimController as RuntimeAnimatorController;
                    effectAnim.AddComponent<SpriteRenderer>();
                }
                
            }
        }

    }

    #endregion
}
