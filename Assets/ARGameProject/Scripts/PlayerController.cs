using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int HP;
    public AudioSource audio;
    public AudioSource beat;
    public GameObject ARCamera;
    public GameObject Bullet;

    public bool B_Fire = false;


    void Update()
    {
        if(B_Fire == true)
        {
            _generateBullet();
        }
    }

    private void _generateBullet()
    {

        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }
        
        var bullet = GameObject.Instantiate(Bullet, ARCamera.transform.position + ARCamera.transform.forward * 0.3f, Quaternion.identity);
        bullet.GetComponent<PlayerBulletController>().Direction = ARCamera.transform.forward;
        audio.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BULLET") && UiManager.isUnbeatable==false)
        {
            HP--;
           // HP -= other.gameObject.GetComponent<EnemyBulletController>().Damage;
            UiManager.shootedMessenger = true;//피격 이펙트
            beat.Play();
            Handheld.Vibrate();
            if (HP == 0)
            {
                Destroy(gameObject);
                KillManager.resetKillScore();
                UiManager.scopeMessenger = false;
                SceneManager.LoadScene("Over");
            }
        }
       /* else if (other.CompareTag("START"))
        {
            s
        }*/
    }
    // 체력 UI
    void OnGUI()
    {
        if (UiManager.isgaming())
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginVertical();
            GUILayout.Space(60);
            GUILayout.BeginHorizontal();
            GUILayout.Space(80);
            GUIStyle style = new GUIStyle();
            style.fontSize = 80;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.red;

            string hpspace = "";
            for (int i = 1; i <= HP; i++)
            {
                hpspace += "♥  ";

            }
            GUILayout.Label(hpspace, style);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
