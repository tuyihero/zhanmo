
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace NavMeshExtension
{
    /// <summary>
    /// Custom Editor for editing vertices and exporting the mesh.
    /// </summary>
    [CustomEditor(typeof(MapPosGroup))]
    public class MapPosGroupEditor : Editor
    {
        //navmesh object reference
        private MapPosGroup script;

        private bool placing;


        void OnEnable()
        {
            script = (MapPosGroup)target;
        }


        /// <summary>
        /// Custom inspector override for buttons.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space();

            if (GUILayout.Button("FitEnemies"))
            {
                var enemyPoses = script.GetComponentsInChildren<MapPosObj>();

                var fightKAll = script.GetComponent<FightSceneAreaKAllEnemy>();
                if (fightKAll != null)
                {
                    fightKAll._EnemyBornPos = new SerializeEnemyInfo[enemyPoses.Length];
                    for (int i = 0; i < enemyPoses.Length; ++i)
                    {
                        fightKAll._EnemyBornPos[i] = new SerializeEnemyInfo();
                        fightKAll._EnemyBornPos[i]._EnemyTransform = enemyPoses[i].transform;
                        fightKAll._EnemyBornPos[i]._EnemyDataID = enemyPoses[i]._MonsterId;
                        enemyPoses[i].ShowMonsterByID();
                    }
                }

                var fightKCnt = script.GetComponent<FightSceneAreaKEnemyCnt>();
                if (fightKCnt != null)
                {
                    fightKCnt._EnemyBornPos = new Transform[enemyPoses.Length];
                    for (int i = 0; i < enemyPoses.Length; ++i)
                    {
                        fightKCnt._EnemyBornPos[i] = enemyPoses[i].transform;
                        //fightKCnt._EnemyMotionID = enemyPoses[i]._MonsterId;
                        enemyPoses[i].ShowMonsterByID();
                    }
                }

                var fightBossCnt = script.GetComponent<FightSceneAreaKBossWithFish>();
                if (fightBossCnt != null)
                {
                    fightBossCnt._BossBornPos = enemyPoses[0].transform;
                    fightBossCnt._BossMotionID = enemyPoses[0]._MonsterId;
                    enemyPoses[0].ShowMonsterByID();

                    fightBossCnt._EnemyBornPos = new Transform[enemyPoses.Length - 1];
                    for (int i = 1; i < enemyPoses.Length; ++i)
                    {
                        fightBossCnt._EnemyBornPos[i - 1] = enemyPoses[i].transform;
                        //fightKCnt._EnemyMotionID = enemyPoses[i]._MonsterId;
                        enemyPoses[i].ShowMonsterByID();
                    }
                }
            }

            if (GUILayout.Button("RemoveAllShow"))
            {
                var enemyPoses = script.GetComponentsInChildren<MapPosObj>();

                for (int i = 0; i < enemyPoses.Length; ++i)
                {
                    enemyPoses[i].RemoveShow();
                }

            }

            if (GUILayout.Button("ExtentPoses"))
            {
                OnExtentPoses();
            }

        }
    

        /// <summary>
        /// Draw Scene GUI handles, circles and outlines for submesh vertices.
        /// <summary>
        public void OnSceneGUI()
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;
            Event e = Event.current;

            if (e.type == EventType.MouseDown && e.control)
            {
                Physics.Raycast(worldRay, out hitInfo);
                if (Physics.Raycast(worldRay, out hitInfo))
                {
                    CreateEnemyPosGO(hitInfo.point);
                }
                else
                {
                    Debug.Log("Not Hit Navmesh");
                }

            }
        }

        private GameObject CreateEnemyPosGO(Vector3 pos)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.AddComponent<MapPosObj>();
            go.name = "MapPos";
            go.transform.SetParent(script.transform);
            go.transform.position = pos;
            return go;
        }

        public void OnExtentPoses()
        {
            script.transform.position = script._FightEnemyBasePos.position;
            int rawCnt = Mathf.CeilToInt(script._EnemyCnt / (float)script._LineCnt);
            float startX = -rawCnt * 0.5f * script._Distance;
            float startY = -script._LineCnt * 0.5f * script._Distance;

            int lastClumnCnt = script._EnemyCnt % script._LineCnt;
            float lastStartX = (script._LineCnt - lastClumnCnt) * 0.5f * script._Distance;
            if (lastClumnCnt == 0)
            {
                lastStartX = 0;
            }

            for (int i = 0; i < script._EnemyCnt; ++i)
            {
                int raw = i / script._LineCnt;
                int clumn = i % script._LineCnt;

                Vector3 pos = script.transform.position;
                if (raw == rawCnt - 1)
                {
                    CreateEnemyPosGO(pos + new Vector3(raw * script._Distance + startX, 0, clumn * script._Distance + lastStartX + startY));
                }
                else
                {
                    CreateEnemyPosGO(pos + new Vector3(raw * script._Distance + startX, 0, clumn * script._Distance + startY));
                }

            }

        }

        
    }
}