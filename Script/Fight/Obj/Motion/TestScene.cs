using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        StartMotion();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void StartMotion()
    {
        Tables.TableReader.ReadTables();
        var motions = GameObject.FindObjectsOfType<MotionManager>();
        foreach (var motion in motions)
        {
            motion.InitMotion();
        }
    }
}
