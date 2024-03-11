using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTowardsPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 startPos;
    Vector3 endPos;
    
    private Vector3 movingDir;

    private float startTime;
    public float speed = 1.0f;
    public float spawnTime = 2f;

    public LayerMask enemiesLayer;
    private float totalTime = 0f;

    public LayerMask wallsLayer;
    void Start()
    {
        startPos = transform.position;
        movingDir = (GameObject.FindGameObjectWithTag("Player").transform.position- transform.position).normalized * 10f;
        startTime = Time.time;
    }


    //IEnumerator LerpFunction(Vector3 targePosition)
    //{
    //    float time = 0;
    //    Vector3 startPosition = transform.position;
    //    while (time < duration)
    //    {
    //        transform.position = Vector3.Lerp(startPos, endPos,)
    //        time += Time.deltaTime;
    //    }
    //}
    // Update is called once per frame
    void Update()
    {
        float timeSpent= (Time.time - startTime);
        //Debug.Log(fractionOfJourney);
        //Debug.Log(timeSpent);
        if (timeSpent > spawnTime)
        {
            Destroy(gameObject);
        }
        float distCovered = (Time.time - startTime) * speed;
        //whar 
        transform.position = startPos+distCovered* movingDir;

        //rigid body it collider
        Collider2D[] enemiesSelf = Physics2D.OverlapCircleAll(transform.position, 0.12f, enemiesLayer);

        //// Reflect the bullet's direction if it hits an obstacle
        if (enemiesSelf.Length > 0)
        {
            for (int i = 0; i < enemiesSelf.Length; i++)
            {
                
                if(timeSpent>0.22f)
                {
                    Destroy(enemiesSelf[i].gameObject);
                    //Debug.Log("De");
                    Destroy(gameObject);
                }
                
            }
                //    // Get the normal of the first obstacle hit
                ///    Vector2 normal = (transform.position - obstacles[0].transform.position).normalized;

                //    // Reflect the direction
                //    movingDir = Vector2.Reflect(movingDir, normal);
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

    //void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision);
    //    //if (collision.gameObject.tag == "myObject")//you can also have a specific name as well
    //    //{
    //    //    do something
    //    //}
    //}
    //void CollisionEnter2D(Collision2D collide) {
    //    Debug.Log(collide);
    //    movingDir = Vector3.Reflect(movingDir, collide.contacts[0].normal);
    //}
}
