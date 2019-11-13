using UnityEngine;
using System.Collections;

public class MapPosObj : MonoBehaviour
{

    public string _MonsterId = "23";

    public void ShowMonsterByID()
    {
        if (Tables.TableReader.MonsterBase == null)
        {
            Tables.TableReader.ReadTables();
        }
        var monsterBase = Tables.TableReader.MonsterBase.GetRecord(_MonsterId);
        if (monsterBase == null)
        {
            Debug.Log("MonsterBase Null:" + _MonsterId);
            var priObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            priObj.transform.SetParent(transform);
            priObj.transform.localPosition = Vector3.zero;
            priObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
            priObj.name = "ShowChil";
            return;
        }

        StartCoroutine(ShowModel(monsterBase));
        
    }

    private IEnumerator ShowModel(Tables.MonsterBaseRecord monsterBase)
    {
        GameObject modelBase = null;
        yield return ResourceManager.Instance.LoadPrefab("ModelBase/" + monsterBase.Name, (resName, resData, hash) =>
        {
            var resource = resData;
            modelBase = GameObject.Instantiate(resource);
            modelBase.transform.SetParent(transform);
            modelBase.transform.localPosition = Vector3.zero;
            modelBase.transform.localRotation = Quaternion.Euler(Vector3.zero);
            modelBase.name = "ShowChil";
            modelBase.SetActive(true);
        });


        yield return ResourceManager.Instance.LoadPrefab("Model/" + monsterBase.ModelPath, (resName, resData, hash) =>
        {
            var modelRes = resData;
            var modelObj = GameObject.Instantiate(modelRes);
            modelObj.transform.SetParent(modelBase.transform);
            modelObj.transform.localPosition = Vector3.zero;
            modelObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
            modelObj.SetActive(true);
        });
    }

    public void RemoveShow()
    {
        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            GameObject.DestroyImmediate(meshFilter);
        }

        var meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            GameObject.DestroyImmediate(meshRenderer);
        }

        var collider = GetComponent<Collider>();
        if (collider != null)
        {
            GameObject.DestroyImmediate(collider);
        }

        var showChil = transform.Find("ShowChil");
        if (showChil != null)
        {
            GameObject.DestroyImmediate(showChil.gameObject);
        }
    }
}
