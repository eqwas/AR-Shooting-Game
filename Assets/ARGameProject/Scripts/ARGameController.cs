using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.SceneManagement;

public class ARGameController : MonoBehaviour
{
    public GameObject Portal;
    public AudioSource background_audio;

    public GameObject ARCamera;

    public GridGenerator Generator;

   // public GameObject Player;

   // public GameObject[] Enemies;

    public static int end=0;

    private bool isAnchorMade = false;

    private void Start()
    {   
        
   
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnchorMade == false)
        {
            _generateAnchor();
        }
        else
        {
            // Make player be able to fire
            
            // Make Enemies start to move and fire
            /*
            if (PortalManager.isSet == true)
            {
                Player.GetComponent<PlayerController>().B_Fire = true;
                
                for (int i = 0; i < Enemies.Length; ++i)
                {
                    Enemies[i].SetActive(true);
                    Enemies[i].GetComponent<EnemyController>().B_RUN = true;
                }

            }
            */
            Destroy(gameObject);
        }
    }
 
    private void _generateAnchor()
    {
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        TrackableHit hit;
        if (Frame.Raycast(touch.position.x, touch.position.y, TrackableHitFlags.PlaneWithinPolygon, out hit))
        {
            // Enable the portal
            Portal.SetActive(true);

            // Create a new Anchor
            Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);

            // Set the position of the portal to be the same as the hit position
            Portal.transform.position = hit.Pose.position;
            Portal.transform.rotation = hit.Pose.rotation;

            // We want the portal to face the camera
            Vector3 cameraPosition = ARCamera.transform.position;

            // The portal should only rotate around the Y axis
            cameraPosition.y = hit.Pose.position.y;

            // Rotate the portal to face the camera
            Portal.transform.LookAt(cameraPosition, Portal.transform.up);

            //Create UI 스코프 켜기
            UiManager.scopeMessenger = true;
            background_audio.Play();
            // Attach Portal to the anchor
            Portal.transform.parent = anchor.transform;

            // indicate that anchor have been made.
            isAnchorMade = true;

            // we don't need plane generator from this point
            Generator.Run = false;
        }
    }

}
