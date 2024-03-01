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

    public List<Vector3> path;
    void Start()
    {
        spawner = transform.Find("spawner");
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void FollowPlayer()
    {
        transform.position += (playerPos.position - transform.position).normalized * Time.deltaTime * Speed;
    }
    void attackPlayer()
    {
        if (attackTime <= 0)
        {
            Instantiate(shoot, spawner.position, Quaternion.identity);
            attackTime = startTimeAttack;
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
            Debug.Log("Yes?");
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
    // Update is called once per frame
    void Update()
    {
        // There are 3 enemy models
        // ..Patroll
        // ..Follow Player (When Visible)
        // ..Follow Player till wall reached
        //isPlayerInCleanSight();
        //return;
        //Debug.Log(mode);

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
            
    }
}
