using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> path;
    public float speed = 0.1f;

    private Vector3 directionVector;
    private int nextPointIndex;
    private Vector3 nextPointPos;
    private int reversed = 1;
    public int enemy_no;

    void Start()
    {
        transform.position = path[0].transform.position;
        nextPointIndex = 1;
        nextPointPos = path[nextPointIndex].transform.position;
        directionVector = (nextPointPos - transform.position).normalized;
        Debug.Log(directionVector);
        float angle = Mathf.Atan2(directionVector.x, -directionVector.y) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Debugging" + PlayerPrefs.GetInt("Enemy_no " + enemy_no));
        if (PlayerPrefs.GetInt("Enemy_no " + enemy_no) == 1) 
        {
            Destroy(this.gameObject);
        }
        
        if ((nextPointPos - transform.position).magnitude < 0.1f)
        {
            transform.position = nextPointPos;
            nextPointIndex += reversed;
            if (nextPointIndex >= path.Count || nextPointIndex <=0)
            {
                reversed *= -1;
            }
            
            nextPointPos = path[nextPointIndex].transform.position;
            directionVector = (nextPointPos - transform.position).normalized;
            float angle = Mathf.Atan2(directionVector.x, -directionVector.y) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3 (0, 0, angle);
            
            Debug.Log(directionVector);
        }
        transform.position += directionVector*Time.deltaTime*speed;
    }

    void OnDestroy()
    {
        if (!GameController.death)
        {
           GameController.exp += 5;
           Debug.Log("Enemy killed bruh");
           Debug.Log(GameController.exp);

            //store a value inside a file associated with enemy#. 1 for defeated, 0 for not defeated
           PlayerPrefs.SetInt("Enemy_no " + enemy_no.ToString(), 1);
           Debug.Log("Setting to defeated " + PlayerPrefs.GetInt("Enemy_no " + enemy_no).ToString());
        }
        
    }
}
