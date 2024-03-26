using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    private Transform spawner;
    public GameObject shoot;
    public float attackRange = 10.0f;
    public float attackTime;
    public float startTimeAttack;

    private string mode = "new";
    public float Speed = 2f;
    private float PatrolSpeed = 1f;
    Transform playerPos;

    private Vector3 directionVector;
    private int nextPointIndex;
    private Vector3 nextPointPos;
    private int reversed = 1;
    public int enemy_no;

    private float hp = 100;

    public List<Vector3> path;

    private float colorTimeCounter;

    public bool MedEnemy = false;

    private Vector3 originalScale;
    void Start()
    {
        spawner = transform.Find("spawner");
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        originalScale = transform.localScale;
    }


    void FollowPlayer()
    {
        transform.position += (playerPos.position - transform.position).normalized * Time.deltaTime * Speed;
    }
    void attackPlayer()
    {
        if (attackTime <= 0)
        {
            if (MedEnemy)
            {
                var newProjectile = Instantiate(shoot, spawner.position, Quaternion.identity);
                newProjectile.GetComponent<ShootTowardsPlayer>().initalRotate = (-30);

                newProjectile = Instantiate(shoot, spawner.position, Quaternion.identity);
                newProjectile.GetComponent<ShootTowardsPlayer>().initalRotate = (0);

                newProjectile = Instantiate(shoot, spawner.position, Quaternion.identity);
                newProjectile.GetComponent<ShootTowardsPlayer>().initalRotate = (30);

                SoundFXManager.instance.PlaySoundFXClip("BigEnemyShot", this.transform);

                attackTime = startTimeAttack;
            }
            else
            {
                Instantiate(shoot, spawner.position, Quaternion.identity);
                attackTime = startTimeAttack;
            }
        }
    }
    bool isPlayerInAttackRange()
    {
        if (Vector3.Distance(playerPos.position, transform.position) < attackRange && isPlayerInCleanSight())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    int findClosestPath()
    {
        int closest = -1;
        float closestDistance = 9999f;
        int walls = LayerMask.GetMask("Obstacle");

        for (int index = 0; index < path.Count; index += 1)
        {

            if (Vector3.Distance(path[index], transform.position) < closestDistance && Physics2D.Linecast(path[index], transform.position, walls).collider == null)
            {
                closest = index;
                closestDistance = (Vector3.Distance(path[index], transform.position));
            }
        }
        return closest;
    }

    void setPatrolPath()
    {
        int closest = findClosestPath();
        if (closest == -1)
        {
            directionVector = new Vector3(0, 0, 0);
            nextPointIndex = -1;
            return;
        }
        nextPointIndex = closest;
        nextPointPos = path[closest];
        directionVector = (nextPointPos - transform.position).normalized;
    }

    void keepPatrolling()
    {
        if (nextPointIndex == -1)
        {
            transform.position += directionVector * Time.deltaTime * Speed;
            return;
        }
        if ((nextPointPos - transform.position).magnitude < 0.1f)
        {
            //Debug.Log("Yes?");
            transform.position = nextPointPos;
            nextPointIndex += reversed;
            if (nextPointIndex >= path.Count || nextPointIndex <= 0)
            {
                reversed *= -1;
                
            }
            if (nextPointIndex >= path.Count)
            {
                nextPointIndex += reversed;
            }
            nextPointPos = path[nextPointIndex];
            directionVector = (nextPointPos - transform.position).normalized;
            //float angle = Mathf.Atan2(directionVector.x, -directionVector.y) * Mathf.Rad2Deg;
            //transform.eulerAngles = new Vector3(0, 0, angle);

            //Debug.Log(directionVector);
        }
        transform.position += directionVector * Time.deltaTime * PatrolSpeed;

    }
    bool isPlayerInCleanSight()
    {
        int walls = LayerMask.GetMask("Obstacle");

        RaycastHit2D hit = Physics2D.Linecast(transform.position, playerPos.position, walls);
        //Debug.Log(results);
        //Debug.Log(hit.collider);
        if (hit.collider!=null)
        {
            return false;
        }
        return true;
    }
    void FacePlayer()
    {
        if((playerPos.position - transform.position).x > 0)
        {
            var scale = (originalScale);
            scale.x *= -1;
            transform.localScale = scale;
        } else
        {
            transform.localScale = originalScale;
        }
    }
    // Update is called once per frame
    void Update()
    {
        colorTimeCounter -= Time.deltaTime;
        if (colorTimeCounter < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        // There are 3 enemy models
        // ..Patroll
        // ..Follow Player (When Visible)
        // ..Follow Player till wall reached
        //isPlayerInCleanSight();
        //return;
        //Debug.Log(mode);
        FacePlayer();

        bool isPlayerAttackable = isPlayerInAttackRange();
        bool isPlayerVisible = isPlayerInCleanSight();
        //Debug.Log((isPlayerAttackable, isPlayerVisible));
        if (isPlayerAttackable)
        {
            mode = "attack";
            attackPlayer();
        } else if (isPlayerVisible)
        {
            mode = "follow";
            FollowPlayer();
        } else if (mode == "patrol")
        {
            keepPatrolling();
        }
        else
        {
            mode = "patrol";
            setPatrolPath();
        }
        
        

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }

        //Debug.Log("Debugging" + enemy_no.ToString() + " to " + PlayerPrefs.GetInt("Enemy_no " + enemy_no));
        //check if enemy was previously defeated 
        if (PlayerPrefs.GetInt("Enemy_no " + enemy_no) == 1)
        {
            Destroy(this.gameObject);
        }
    }
    

    public void RemoveEnemy()
    {   //check if player is not dead
        if (!GameController.death)
        {
            //check if the enemy wasnt defeated before (store a value inside a file associated with enemy#. 1 for removed, 0 for not defeated)
            if (PlayerPrefs.GetInt("Enemy_no " + enemy_no) == 0)
            {
                GameController.exp += 5;
                Debug.Log("Enemy killed bruh");
                Debug.Log(GameController.exp);

                //set enemy status to defeated
                PlayerPrefs.SetInt("Enemy_no " + enemy_no, 1);
                Debug.Log("setting" + enemy_no.ToString() + " to " + PlayerPrefs.GetInt("Enemy_no " + enemy_no));
            }

        }

    }
    public void TakeDamage(float hitpoints)
    {
        hp -= hitpoints;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        colorTimeCounter = 0.1f;
        if (hp <= 0)
        {
            RemoveEnemy();
        }
    }
    //void OnDestroy()
    //{
    //    if (!GameController.death)
    //    {
    //        GameController.exp += 5;
    //        Debug.Log("Enemy killed bruh");
    //        Debug.Log(GameController.exp);

    //        //store a value inside a file associated with enemy#. 1 for defeated, 0 for not defeated
    //        PlayerPrefs.SetInt("Enemy_no " + enemy_no.ToString(), 1);
    //        Debug.Log("Setting to defeated " + PlayerPrefs.GetInt("Enemy_no " + enemy_no).ToString());
    //    }

    //}
    /*
    void OnDestroy()
    {
        if (!GameController.death)
        {   
            if (PlayerPrefs.GetInt("Enemy_no " + enemy_no) == 0)
            {
                GameController.exp += 5;
                Debug.Log("Enemy killed bruh");
                Debug.Log(GameController.exp);

                //store a value inside a file associated with enemy#. 1 for defeated, 0 for not defeated
                PlayerPrefs.SetInt("Enemy_no " + enemy_no, 1);
                Debug.Log("setting" + enemy_no.ToString() + " to " + PlayerPrefs.GetInt("Enemy_no " + enemy_no));
            }
                
        }

    }
    */
}
