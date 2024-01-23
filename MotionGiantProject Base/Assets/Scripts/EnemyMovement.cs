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
}
