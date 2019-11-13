using UnityEngine;
using System.Collections;

public class ShowFPS : MonoBehaviour {

    public float f_UpdateInterval = 0.5F;

    private float f_LastInterval;

    private int i_Frames = 0;

    private float f_Fps;

    private GUIStyle style;
    private Color color = Color.green;
    public Rect startRect = new Rect(0, 0, 50, 50);

    void Start() 
    {
		//Application.targetFrameRate=60;

        f_LastInterval = Time.realtimeSinceStartup;

        i_Frames = 0;
    }

    void OnGUI() 
    {

        if (style == null)
        {
            style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.white;
            style.alignment = TextAnchor.MiddleCenter;
        }

        GUI.color = color;
        Rect rect = startRect;
        rect.x += Screen.width / 2 - rect.width / 2;
        startRect = GUI.Window(0, rect, DoMyWindow, "");
        startRect.x -= Screen.width / 2 - rect.width / 2;

    }

    void DoMyWindow(int windowID)
    {
        GUI.Label(new Rect(0, -15, startRect.width, startRect.height), f_Fps + " FPS", style);
    }

    void Update() 
    {
        ++i_Frames;

        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval) 
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

            i_Frames = 0;

            f_LastInterval = Time.realtimeSinceStartup;
        }
    }
}
