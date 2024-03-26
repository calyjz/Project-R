using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTowardsPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 startPos;
    Vector3 endPos;


    public bool killOtherEnemies = true;

    private Vector3 movingDir;

    private float startTime;
    public float speed = 1.0f;
    public float spawnTime = 2f;

    public LayerMask enemiesLayer;
    private float totalTime = 0f;

    public LayerMask wallsLayer;

    // For Medium Enemies
    public float initalRotate = 0;

    public bool Deflectable = true;
    void Start()
    {
        startPos = transform.position;
        Debug.Log(initalRotate);
        movingDir = (Quaternion.Euler(0, 0, initalRotate) * (GameObject.FindGameObjectWithTag("Player").transform.position- transform.position).normalized).normalized * 10f;
        startTime = Time.time;
    }

    /// For the triple projectile shot
    //public void RotateDir(float angle)
    //{
    //    movingDir = Quaternion.Euler(0, angle, 0) * (movingDir);
    //}



    void Update()
    {
        float timeSpent= (Time.time - startTime);

        if (timeSpent > spawnTime)
        {
            Destroy(gameObject);
        }
        float distCovered = (Time.time - startTime) * speed;
        transform.position = startPos+distCovered* movingDir;

        if (killOtherEnemies)
        {
            Collider2D[] enemiesSelf = Physics2D.OverlapCircleAll(transform.position, 0.3f, enemiesLayer);

            if (enemiesSelf.Length > 0)
            {
                for (int i = 0; i < enemiesSelf.Length; i++)
                {

                    if (timeSpent > 0.22f)
                    {
                        //try
                        //{
                        if (!enemiesSelf[i].gameObject.GetComponent<Enemy>().MedEnemy)
                        {
                            SoundFXManager.instance.PlaySoundFXClip("MonsterTakesDamage", this.transform);
                            enemiesSelf[i].gameObject.GetComponent<Enemy>().TakeDamage(70.00f);

                        }
                        //}
                        //catch
                        //{
                        //    enemiesSelf[i].gameObject.GetComponent<EnemyMed>().TakeDamage(70);

                        //}
                        Destroy(gameObject);
                    }

                }
            }
        }


        Collider2D[] wallHit = Physics2D.OverlapCircleAll(transform.position, 0.12f, wallsLayer);
        if (wallHit.Length>0)
        {
            Destroy(gameObject);
        }
    }

    public void deflect(Vector3 normal)
    {
        movingDir = Vector2.Reflect(movingDir, normal);
        startPos = transform.position;
        totalTime += Time.time - startTime;
        startTime = Time.time-0.05f;
    }

}
