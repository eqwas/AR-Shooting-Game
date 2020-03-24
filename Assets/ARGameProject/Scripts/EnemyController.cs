using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EnemyController : MonoBehaviour
{
    public enum E_EnemyType
    {
        BLUE,
        RED,
        YELLOW 
    };
    public AudioSource audio;

    public GameObject Bullet;

    public GameObject Player;

    public bool B_RUN = false;
    bool isDie = false;
    public int HP;

    public float Speed;

    public bool isMove;

    public E_EnemyType EnemyType;

    private float m_lastTimeAfterFire;
    private float m_fireDelayTime;

    private float m_lastTimeAfterChangeDirection;
    private float m_changeDirectionDelayTime;
    private Vector3 m_movementDirection;
    private Vector3 m_movementPerSecond;

   // public GameObject boom;

    // Start is called before the first frame update
    void Start()
    {
        //audio = GetComponent<AudioSource>();
        m_lastTimeAfterFire = 0f;
        m_fireDelayTime = 1f;

        m_lastTimeAfterChangeDirection = 0f;
        m_changeDirectionDelayTime = 3f;

        m_movementDirection = Vector3.zero;
        m_movementPerSecond = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(B_RUN == false)
        {
            return;
        }

        // fire bullet
        m_lastTimeAfterFire += Time.deltaTime;
        if (m_lastTimeAfterFire > m_fireDelayTime && UiManager.isgaming()) 
        {
            m_lastTimeAfterFire = 0f;
           // m_fireDelayTime = Random.Range(1f, 3f);
            switch (EnemyType)
            {
                case E_EnemyType.BLUE:
                    {
                        m_fireDelayTime = Random.Range(0.5f, 1.0f);
                        break;
                    }

                case E_EnemyType.RED:
                    {
                        m_fireDelayTime = 2f;
                        break;
                    }

                case E_EnemyType.YELLOW:
                    {
                        m_fireDelayTime = 1.5f;
                        break;
                    }
            }
            _BulletFire();
        }

        // re-caclute move direction
        m_lastTimeAfterChangeDirection += Time.deltaTime;
        if(m_lastTimeAfterChangeDirection > m_changeDirectionDelayTime)
        {
            m_lastTimeAfterChangeDirection = 0f;
            switch (EnemyType)
            {
                case E_EnemyType.BLUE:
                    {
                        m_changeDirectionDelayTime = 3.0f;
                        break;
                    }

                case E_EnemyType.RED:
                    {
                        m_changeDirectionDelayTime = 20.0f;
                        break;
                    }

                case E_EnemyType.YELLOW:
                    {
                        m_changeDirectionDelayTime = 2.0f;
                        break;
                    }
            }
            //m_changeDirectionDelayTime = Random.Range(3f, 6f);
            _calculateNewMovementVector();
        }

        // move enemy
        if(isMove==true)
        transform.Translate(m_movementPerSecond * Time.deltaTime);
    }

    public void Die()
    {
        isDie = true;
        //transform.Rotate(new Vector3(0f, 0f, 180f));
        
        SphereCollider coll = gameObject.GetComponent<SphereCollider>();

        Rigidbody rigid = gameObject.GetComponent<Rigidbody>();
        //Vector2 dieVelo = new Vector2(0, 30f);
        rigid.constraints = RigidbodyConstraints.None;
        rigid.useGravity = true;
        rigid.AddForce(transform.forward*-50.0f, ForceMode.Force);
        //StartCoroutine("Fade");
        //Instantiate(boom, transform.position, boom.transform.rotation);
        voxelEffect();
        Destroy(gameObject,1.5f);
        
        


    }
    public void voxelEffect()
    {
        for(int i = 0; i < 50; i++)
        {

            GameObject vox = GameObject.CreatePrimitive(PrimitiveType.Cube);
            vox.transform.position = transform.position;
            vox.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            vox.AddComponent<Rigidbody>();

            PhysicMaterial m = new PhysicMaterial();
            m.bounciness = 1f;
            m.bounceCombine = PhysicMaterialCombine.Maximum;
            switch (EnemyType)
            {
                case E_EnemyType.BLUE:
                    {
                        vox.GetComponent<MeshRenderer>().material.color = Color.blue;

                        break;
                    }

                case E_EnemyType.RED:
                    {
                        vox.GetComponent<MeshRenderer>().material.color = Color.red;
                        break;
                    }

                case E_EnemyType.YELLOW:
                    {
                        vox.GetComponent<MeshRenderer>().material.color = Color.yellow;
                        break;
                    }
            }
            
            vox.GetComponent<Collider>().material = m;


        } 
    }
    /*
    IEnumerator Fade()
    {
        if (UiManager.isgaming())
        {
            GameObject forfade = null;
            forfade = this.transform.FindChild("defult").gameObject;
           
            for (float i = 1f; i >= 0; i -= 0.02f)
            {
                Color color = new Vector4(1, 1, 1, i);
                forfade.GetComponent<MeshRenderer>().material.color = color;
                yield return 0;

            }

        }
        else
        {
            Color color = new Vector4(1, 1, 1, 1f);
            this.color = color;
            yield return 0;
      
        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BULLET"))
        {
            audio.Play();
            HP--;
            //HP -= other.gameObject.GetComponent<PlayerBulletController>().Damage;
            if (HP == 0)
            {
                Die();
                KillManager.setKillScore();//킬스코어 올리기
                if (KillManager.killscore == ARGameController.end)
                {
                    KillManager.resetKillScore();
                    UiManager.scopeMessenger = false;
                    SceneManager.LoadScene("Win");//승리시 win 씬으로
                }

            }
        }
    }

    // bounce on wall
    private void OnCollisionEnter(Collision collision)
    {
        /*
        Vector3 newDirection;
        float d;
        do
        {
            newDirection = Quaternion.AngleAxis(Random.Range(0f, 360f), transform.up) * transform.forward;
            d = Vector3.Dot(newDirection, collision.contacts[0].normal);
        } while (d < 0.5f);
        m_movementDirection = newDirection;
        m_movementPerSecond = m_movementDirection * Speed;
        transform.Translate(m_movementPerSecond * 0.03f);
        */
        m_movementDirection = Vector3.Reflect(m_movementDirection, collision.contacts[0].normal);
        m_movementPerSecond = m_movementDirection * Speed;
    }

    private void _calculateNewMovementVector()
    {
        m_movementDirection = Quaternion.AngleAxis(Random.Range(0f, 360f), transform.up) * transform.forward;
        m_movementPerSecond = m_movementDirection * Speed;
    }

    private void _BulletFire()
    {
        if (isDie == false)
        {
            switch (EnemyType)
            {
                case E_EnemyType.BLUE:
                    {

                        _BlueBulletFire();
                        break;
                    }

                case E_EnemyType.RED:
                    {
                        _RedBulletFire();
                        break;
                    }

                case E_EnemyType.YELLOW:
                    {
                        _YellowBulletFire();
                        break;
                    }
            }
        }
    }

    private void _BlueBulletFire()
    {
        GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<EnemyYellowBulletController>().Direction =
        Quaternion.AngleAxis(Random.Range(0f, 360f), transform.up) * transform.forward;
        bullet.GetComponent<EnemyYellowBulletController>().Speed = 5f;
        /*
        const int NUM_BULLETS = 1;
        GameObject[] bullets = new GameObject[NUM_BULLETS];
        for (int i = 0; i < NUM_BULLETS; ++i)
        {
            bullets[i] = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullets[i].GetComponent<EnemyBulletController>().Direction = Vector3.Normalize(Player.transform.position - transform.position);
            bullets[i].GetComponent<EnemyBulletController>().Speed *= i;
        }
        */
    }

    private void _RedBulletFire()
    {
        const int NUM_BULLETS = 18;
        const float DELTA_ANGLE = 360.0f / NUM_BULLETS;
        GameObject[] bullets = new GameObject[NUM_BULLETS];
        Vector3 toPlayer = Vector3.Normalize(Player.transform.position - transform.position);
        Vector3 rotateAxis = transform.up;
        for (int i = 0; i < NUM_BULLETS; ++i)
        {
            bullets[i] = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullets[i].GetComponent<EnemyBulletController>().Direction = Quaternion.AngleAxis(DELTA_ANGLE * i, rotateAxis) * toPlayer;
            //bullets[i].GetComponent<EnemyBulletController>().Speed += 0.0f;
        }
    }

    private void _YellowBulletFire()
    {
        const int NUM_BULLETS = 1;
        GameObject[] bullets = new GameObject[NUM_BULLETS];
        for (int i = 0; i < NUM_BULLETS; ++i)
        {
            bullets[i] = Instantiate(Bullet, transform.position, Quaternion.identity);
            bullets[i].GetComponent<EnemyBulletController>().Direction = Vector3.Normalize(Player.transform.position - transform.position);
            //bullets[i].GetComponent<EnemyBulletController>().Speed *= i;
        }
        /*
        GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
        bullet.GetComponent<EnemyYellowBulletController>().Direction =
            Quaternion.AngleAxis(Random.Range(0f, 360f), transform.up) * transform.forward;
        bullet.GetComponent<EnemyYellowBulletController>().Speed = Random.Range(5f, 8f);
    */
    }
}
