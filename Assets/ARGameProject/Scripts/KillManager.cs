using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillManager : MonoBehaviour
{
    public static int killscore=0;

 
    public static void resetKillScore()
    {
        killscore = 0;
    }
    public static void setKillScore()
    {
        killscore++;
    }
    public static int getKillScore()
    {
        return killscore;
   
    }
    // Start is called before the first frame update
    void OnGUI()
    {
        if (UiManager.isgaming())
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginVertical();
            GUILayout.Space(150);
            GUILayout.BeginHorizontal();
            GUILayout.Space(80);
            GUIStyle style = new GUIStyle();
            style.fontSize = 80;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.black;

            string killstr = "Kill :";

            GUILayout.Label(killstr + killscore.ToString(), style);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
