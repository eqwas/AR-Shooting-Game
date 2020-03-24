using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    public Transform BulletTransform;

    public Vector3 Direction;
   // public AudioSource audio;
    public int Damage;

    public float Speed;

    public float LifeTime;

    // Start is called before the first frame update
    void Start()
    {
      //  audio.Play();
        Invoke("_destroy", LifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        BulletTransform.Translate(Direction * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CHARACTER") || other.CompareTag("WALL"))
        {
            _destroy();
        }
    }

    private void _destroy()
    {
        Destroy(gameObject);
    }
}
