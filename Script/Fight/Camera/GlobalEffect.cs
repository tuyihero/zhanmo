using UnityEngine;
using System.Collections;

public class GlobalEffect : MonoBehaviour
{

	// Use this for initialization
	void Start () {
        _Instance = this;
    }
	
	// Update is called once per frame
	void Update ()
    {
        PauseUpdate();
    }

    void FixedUpdate()
    {
        
    }

    #region static

    private static GlobalEffect _Instance;

    public static GlobalEffect Instance
    {
        get
        {
            return _Instance;
        }
    }

    #endregion

    #region global pause

    private float _PauseTime;
    private float _PauseStart;

    public void Pause(float time)
    {
        Time.timeScale = 0;
        _PauseTime = time;
        _PauseStart = Time.realtimeSinceStartup;
    }

    public void PauseUpdate()
    {
        if (_PauseTime <=  0)
        {
            return;
        }

        if (Time.realtimeSinceStartup - _PauseStart > _PauseTime)
        {
            _PauseTime = 0;
            _PauseStart = 0;

            if (Time.timeScale == 0)
                Time.timeScale = 1;
        }
    }

    #endregion
}
