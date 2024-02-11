using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public float smoothSpeed = 0.125f;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPosition = player.transform.position - offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
