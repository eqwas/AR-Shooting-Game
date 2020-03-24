using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalManager : MonoBehaviour
{
    public GameObject MainCamera;

    public GameObject Sponza;

    public GameObject[] Enemies;
    public int count=0;
    private Material[] SponzaMaterials;
    public static bool isSet;
    public static bool isStart;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        isSet = false;
        isStart = false;
        SponzaMaterials = Sponza.GetComponent<Renderer>().sharedMaterials;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPositionInPortalSpace = transform.InverseTransformPoint(MainCamera.transform.position);
        //camPositionInPortalSpace.y < 0.5f
        if (camPositionInPortalSpace.y < 0.5f)
        {
            // Disable Stencil test
            
            if (isStart == true)
            {
                for (int i = 0; i < SponzaMaterials.Length; ++i)
                {
                    SponzaMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Always);
                }
                // 켜야되
                Player.GetComponent<PlayerController>().B_Fire = true;
                for (int i = 0; i < Enemies.Length; ++i)
                {
                   Enemies[i].SetActive(true);

                   Enemies[i].GetComponent<EnemyController>().B_RUN = true;
                  
                }
                
                isSet = true;
                
            }
        }
        else
        {
            // Enable Stencil test
            for (int i = 0; i < SponzaMaterials.Length; ++i)
            {
                SponzaMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Equal);
            }

            // 꺼야되
        }
    }

}
