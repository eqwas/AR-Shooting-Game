﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Handheld.Vibrate();
            PortalManager.isStart = true;

            Destroy(gameObject);
        }
    }
}
