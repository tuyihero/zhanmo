using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraPicture : MonoBehaviour {

    [MenuItem("TyTools/CameraPicture")]
    public static void WeaponPicture ()
    {
        //var weapon = Selection.GetFiltered<GameObject>(SelectionMode.Assets);
        //foreach (var weapon in weapons)
        //{
        //    var weaponInstance = GameObject.Instantiate(weapon);
        //    Camera.main.gameObject.
        //}
        var earthCamera = Camera.main;
        var modelMesh = GameObject.FindObjectOfType<Animation>();
        string filename = "D:\\unityProject\\FightCraft\\FightCraft\\Assets\\FightCraft\\Resources\\Icon\\monster\\";
        filename += modelMesh.name + ".jpg";
        RenderTexture renderTexture;
        //深度问题depth
        renderTexture = new RenderTexture(1024 , 1024, 24);
        earthCamera.targetTexture = renderTexture;
        earthCamera.Render();

        Texture2D myTexture2D = new Texture2D(1024, 1024);
        RenderTexture.active = renderTexture;
        myTexture2D.ReadPixels(new Rect(0, 0, 1024, 1024), 0, 0);
        myTexture2D.Apply();
        byte[] bytes = myTexture2D.EncodeToJPG();
        myTexture2D.Compress(true);
        myTexture2D.Apply();
        RenderTexture.active = null;


        System.IO.File.WriteAllBytes(filename, bytes);
        //Debug.Log (string.Format ("截屏了一张图片: {0}", filename));  
        Debug.Log("保存图片的路径" + filename);

        earthCamera.targetTexture = null;
        //GameObject.Destroy(renderTexture); 

    }

    [MenuItem("TyTools/BindWeapon")]
    public static void BindWeapon()
    {
        var weapon = Selection.GetFiltered<GameObject>(SelectionMode.Assets);
        var weaponInstance = GameObject.Instantiate<GameObject>(weapon[0]);
        var earthCamera = Camera.main;
        for (int i = 0; i < earthCamera.transform.childCount; ++i)
        {
            GameObject.DestroyImmediate(earthCamera.transform.GetChild(i).gameObject);
        }

        //weaponInstance.transform.SetParent(earthCamera.transform);
        weaponInstance.transform.position = new Vector3(0, -0.5f, -3.5f);
        weaponInstance.transform.rotation = Quaternion.Euler( new Vector3(8.4f, 229, 0));
    }
}
