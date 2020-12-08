﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airPatrol : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public float speed = 2f;
    public float waitTime = 3f;
    bool canGo = true;
    void Start()
    {
        gameObject.transform.position = new Vector3(point1.position.x, point1.position.y, transform.position.z);   
    }

    void Update()
    {
        if (canGo)
            transform.position = Vector3.MoveTowards(transform.position, point1.position, speed * Time.deltaTime);

        if(transform.position == point1.position)
        {
            Transform temp = point1;
            point1 = point2;
            point2 = temp;
            canGo = false;
            StartCoroutine(Waiting());
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        if(transform.rotation.y == 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
        canGo = true;
    }
}
