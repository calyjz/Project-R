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

    Transform playerPos;
    void Start()
    {
        spawner = transform.Find("spawner");
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(playerPos.position, transform.position) < attackRange)
        {
            if(attackTime <= 0)
            {
                Instantiate(shoot, spawner.position, Quaternion.identity);
                attackTime = startTimeAttack;
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
            
    }
}
