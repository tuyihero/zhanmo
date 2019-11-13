
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class CommonTool : Editor
{

    #region disableCollider

    [MenuItem("TyTools/Map/DisableMapCollider")]
    public static void DisableMapCollider()
    {
        var selects = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel);
        foreach (var selectGO in selects)
        {
            DisableCollider(selectGO as GameObject);
        }
    }

    private static void DisableCollider(GameObject particleObj)
    {
        var colliders = particleObj.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }

    #endregion

    #region disableshadow

    [MenuItem("TyTools/Model/DisableShadow")]
    public static void DisableShadow()
    {
        var selects = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel);
        foreach (var selectGO in selects)
        {
            DisableShadow(selectGO as GameObject);
        }
    }

    private static void DisableShadow(GameObject particleObj)
    {
        var skinRenders = particleObj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var render in skinRenders)
        {
            render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            render.receiveShadows = false;
        }

        var meshRenders = particleObj.GetComponentsInChildren<MeshRenderer>();
        foreach (var render in meshRenders)
        {
            render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            render.receiveShadows = false;
        }
    }

    #endregion

    #region 

    [MenuItem("TyTools/Test/GemSetTest")]
    public static void GemSetTest()
    {
        //foreach (var gemSetRecord in Tables.TableReader.GemSet.Records.Values)
        //{
        //    GemSetGemsCriticSelf(gemSetRecord);
        //}
        int gemCnt = Tables.TableReader.GemTable.Records.Count;
        var gemList = new List<Tables.GemTableRecord>(Tables.TableReader.GemTable.Records.Values);
        List<Tables.GemTableRecord> gemSuit = new List<Tables.GemTableRecord>() { null, null, null, null, null, null};
        List<string> gemSuitStrs = new List<string>();
        for (int slot1 = 0; slot1 < gemCnt; ++slot1)
        {
            //gemSuit.Clear();
            gemSuit[0] = (gemList[slot1]);
            for (int slot2 = slot1 + 1; slot2 < gemCnt; ++slot2)
            {
                gemSuit[1] = (gemList[slot2]);
                for (int slot3 = slot2 + 1; slot3 < gemCnt; ++slot3)
                {
                    gemSuit[2] = (gemList[slot3]);
                    for (int slot4 = slot3 + 1; slot4 < gemCnt; ++slot4)
                    {
                        gemSuit[3] = (gemList[slot4]);
                        for (int slot5 = slot4 + 1; slot5 < gemCnt; ++slot5)
                        {
                            gemSuit[4] = (gemList[slot5]);
                            for (int slot6 = slot5 + 1; slot6 < gemCnt; ++slot6)
                            {
                                gemSuit[5] = (gemList[slot6]);

                                string gemSuitStr = IsGemSuitFit(gemSuit);
                                if (!string.IsNullOrEmpty(gemSuitStr))
                                {
                                    gemSuitStrs.Add(gemSuitStr);
                                    Debug.Log("Gemsuit:" + GemSuitIdx + "," + gemSuitStr);
                                    ++GemSuitIdx;
                                }
                            }
                        }
                    }
                }
            }
        }
        string fileName = "GemSuitGems";
        string path = Application.dataPath + fileName + ".txt";
        var fileStream = File.Create(path);
        var streamWriter = new StreamWriter(fileStream);

        for (int i = 0; i < _PreIdxs.Count; ++i)
        {

            streamWriter.WriteLine(gemSuitStrs[_PreIdxs[i]]);
        }

        streamWriter.Close();
    }

    private static List<string> _GemDefence = new List<string>() { "70003", "70004", "70014" };
    private static List<List<string>> _GemConflict = new List<List<string>>()
    {
        new List<string>() { "70005", "70019"},
        new List<string>() { "70007", "70016"},
        new List<string>() { "70008", "70017"},
        new List<string>() { "70010", "70018"},
    };
    private static List<List<string>> _BaseAttrConflict = new List<List<string>>()
    {
        new List<string>() { "70000", "70011"},
        new List<string>() { "70001", "70012"},
        new List<string>() { "70002", "70013"},
    };
    private static int GemSuitIdx = 0;
    private static List<int> _PreIdxs = new List<int>() { 23, 83, 28, 145, 102, 138, 105, 86, 24, 155, 149, 107, 111, 13, 25, 84, 156, 159, 104, 139, 106, 87, 15, 134, 148, 150, 115, 18, 0, 19, 20, 30, 40, 50, 60, 70, 80, 90, 110, 120, 130, 140, 151, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 290, 300, 306, 301, 291, 281, 271, 261, 251, 241, 231, 221 };
    private static string IsGemSuitFit(List<Tables.GemTableRecord> gemSuit)
    {
        string gemSuitNames = "";
        if (gemSuit.Count == 6)
        {
            List<int> levelMat = new List<int>();
            int defenceCnt = 0;
            int conflictEle = -1;
            int conflictBase = -1;
            for (int i = 0; i < gemSuit.Count; ++i)
            {
                if (gemSuit[i] == null)
                    break;

                //if (!levelMat.Contains(gemSuit[i].LevelUpParam))
                //{
                //    levelMat.Add(gemSuit[i].LevelUpParam);
                //}
                gemSuitNames += "\t" + gemSuit[i].Id;

                if (_GemDefence.Contains(gemSuit[i].Id))
                {
                    ++defenceCnt;
                }

                for (int j = 0; j < _GemConflict.Count; ++j)
                {
                    if (_GemConflict[j].Contains(gemSuit[i].Id))
                    {
                        if (conflictEle < 0)
                        {
                            conflictEle = j;
                        }
                        else if(conflictEle != j)
                        {
                            conflictEle = 5;
                        }
                    }
                }

                for (int j = 0; j < _BaseAttrConflict.Count; ++j)
                {
                    if (_BaseAttrConflict[j].Contains(gemSuit[i].Id))
                    {
                        if (conflictBase < 0)
                        {
                            conflictBase = j;
                        }
                        else if (conflictBase != j)
                        {
                            conflictBase = 5;
                        }
                    }
                }
            }
            if (levelMat.Count < 4)
            {
                gemSuitNames = "";
                //Debug.Log("Gemsuit:" + gemStr);
            }
            if (defenceCnt != 1)
            {
                gemSuitNames = "";
            }
            if (conflictEle == 5)
            {
                gemSuitNames = "";
            }
            else if(!string.IsNullOrEmpty(gemSuitNames))
            {
                gemSuitNames += "\t";
                if (conflictEle == 0)
                {
                    gemSuitNames += " Fire";
                }
                else if (conflictEle == 1)
                {
                    gemSuitNames += " Wind";
                }
                else if (conflictEle == 2)
                {
                    gemSuitNames += " Light";
                }
                else if (conflictEle == 3)
                {
                    gemSuitNames += " Cold";
                }
            }

            if (conflictBase == 5)
            {
                gemSuitNames = "";
            }
            else if(!string.IsNullOrEmpty(gemSuitNames))
            {
                gemSuitNames += "\t";
                if (conflictBase == 0)
                {
                    gemSuitNames += " Str";
                }
                else if (conflictBase == 1)
                {
                    gemSuitNames += " Dex";
                }
                else if (conflictBase == 2)
                {
                    gemSuitNames += " Int";
                }
            }
        }

        return gemSuitNames;
    }


    [MenuItem("TyTools/Test/GemSetGroupTest")]
    public static void GemSetCriticTest()
    {
        foreach (var gemSetRecordA in Tables.TableReader.GemSet.Records.Values)
        {
            foreach (var gemSetRecordB in Tables.TableReader.GemSet.Records.Values)
            {
                if (gemSetRecordA == gemSetRecordB)
                    continue;

                GemSetCriticTableTest(gemSetRecordA, gemSetRecordB);
            }
        }
    }

    [MenuItem("TyTools/Test/GemMat")]
    public static void GemMat()
    {
        List<int> _GemMat = new List<int>();
        foreach (var gemSetRecordA in Tables.TableReader.GemSet.Records.Values)
        {
            _GemMat.Clear();
            //foreach (var gemRecord in gemSetRecordA.Gems)
            //{
            //    if (!_GemMat.Contains(gemRecord.LevelUpParam))
            //    {
            //        _GemMat.Add(gemRecord.LevelUpParam);
            //    }
            //}
            Debug.Log("GemSet cost mat typeCnt:" + _GemMat.Count);
        }
    }

    public static void GemSetCriticTableTest(Tables.GemSetRecord gemSetA, Tables.GemSetRecord gemSetB)
    {
        Debug.Log("gemSetCritic:" + gemSetA.Id + "," + gemSetB.Id);
    }

    [MenuItem("TyTools/Test/NumaricTest")]
    public static void NumaricTest()
    {
        int testNum = 1500;
        for(int i = 0; i< 100; ++i)
        {
            Debug.Log(testNum);
            int decNum = Mathf.CeilToInt( testNum * 0.03f);
            decNum = Mathf.Max(decNum, Mathf.CeilToInt(testNum / 100 + 1));
            testNum = testNum - decNum;
        }
    }

    [MenuItem("TyTools/Test/StageNumaric")]
    public static void StageNumaric()
    {
        var stagetables = Tables.TableReader.StageInfo.Records;
        foreach (var stageInfo in stagetables)
        {
            if (stageInfo.Value.StageType == Tables.STAGE_TYPE.NORMAL)
            {
                GetStageNumaric(stageInfo.Value);
            }
        }
    }

    private static void GetStageNumaric(Tables.StageInfoRecord stageInfo)
    {
        var sceneLogic = Resources.Load("FightSceneLogic/" + stageInfo.FightLogicPath) as GameObject;
        var enemyGroups = sceneLogic.GetComponentsInChildren<FightSceneAreaKAllEnemy>();
        int enemyGroupCnt = enemyGroups.Length;
        int enemyCnt = 0;
        foreach (var enemyGroup in enemyGroups)
        {
            enemyCnt += enemyGroup._EnemyBornPos.Length;
        }
        Debug.Log("Stage " + stageInfo.Name + ": enemyGroup " + enemyGroupCnt + ", enemyCnt " + enemyCnt);
    }
    #endregion

    #region change skill char damage

    [MenuItem("GameObject/UI/Fight/SetSkillCharDamage")]
    public static void SetSkillCharDamage()
    {
        Object[] selection = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel);
        if (selection.Length > 0 && selection[0] is GameObject)
        {
            var damages = (selection[0] as GameObject).GetComponentsInChildren<ImpactDamage>(true);
            foreach (var damage in damages)
            {
                damage._IsCharSkillDamage = true;
            }
        }
    }

    #endregion

    #region particle shader

    [MenuItem("TyTools/Particle/ChangeScene")]
    public static void ParticleChangeShader()
    {
        var selections = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        var paricleShader = Shader.Find("TYImage/Particles/Additive");

        foreach (var selected in selections)
        {
            var selectedGO = selected as GameObject;
            if (selectedGO == null)
                return;

            ParticleSystem[] paricals = selectedGO.GetComponentsInChildren<ParticleSystem>();
            var selfPart = selectedGO.GetComponents<ParticleSystem>();
            List<ParticleSystem> partList = new List<ParticleSystem>(paricals);
            partList.AddRange(selfPart);
            foreach (var particle in partList)
            {
                var renderer = particle.GetComponent<Renderer>();
                
                if (renderer != null)
                {
                    if (renderer.sharedMaterial == null)
                        continue;

                    if (renderer.sharedMaterial.shader == null)
                        continue;

                    renderer.sharedMaterial.shader = paricleShader;
                }
            }
        }

        
        
    }

    #endregion

    #region skill motin

    [MenuItem("TyTools/SkillMotion/SkillSuperArmor")]
    public static void SkillSuperArmor()
    {
        var selections = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        foreach (var selected in selections)
        {
            var selectedGO = selected as GameObject;
            if (selectedGO == null)
                continue;

            ObjMotionSkillBase skillBase = selectedGO.GetComponent<ObjMotionSkillBase>();
            if (skillBase == null)
                continue;

            var hitEvents = new List<ImpactHit>(selectedGO.GetComponentsInChildren<ImpactHit>(true));
            //var hitEvents = new List<Transform>(selectedGO.GetComponentsInChildren<Transform>());
            List<int> colliderIDs = new List<int>();
            foreach (var hitEvent in hitEvents)
            {
                var select = hitEvent.GetComponent<SelectBase>();
                if (select != null)
                {
                    colliderIDs.Add(select._ColliderID);
                }
            }

            colliderIDs.Sort();
            if (colliderIDs.Count > 0)
            {
                Debug.Log("start hit event id:" + colliderIDs[0]);
                skillBase._SuperArmorColliderID = colliderIDs[0];
                
            }

            EditorUtility.SetDirty(selected);
        }

        AssetDatabase.SaveAssets();

    }

    #endregion

    #region texture

    [MenuItem("TyTools/Editor/CheckTexture")]
    public static void CheckTexture()
    {
        var selects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (var selectGO in selects)
        {
            if (selectGO is Texture)
            {
                string assetPath = AssetDatabase.GetAssetPath(selectGO);
                TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (textureImporter.mipmapEnabled)
                {
                    textureImporter.mipmapEnabled = false;
                    Debug.Log(selectGO.name + "," + textureImporter.mipmapEnabled);
                    //AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                }
            }
        }
        
    }
    

    #endregion
}
