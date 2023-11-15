using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    Rigidbody2D rb;
    [HideInInspector]
    public bool patroling = true;
    public Transform[] path;
    int currentPoint;
    public Transform currentGoal;
    public float patrolSpeed = 3f;

    void Start()
    {
        ChangeGoal();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(patroling)
            PatrolPositions();
    }

    void PatrolPositions()
    {
        if (Vector2.Distance(transform.position, currentGoal.position) <= float.Epsilon)
            ChangeGoal();

        Vector2 temp = Vector2.MoveTowards(transform.position, currentGoal.position,
            patrolSpeed * Time.deltaTime);
        rb.MovePosition(temp);
    }

    void ChangeGoal()
    {
        if(currentPoint == path.Length - 1)
        {
            currentPoint = 0;
            currentGoal = path[0];
        }
        else
        {
            currentPoint++;
            currentGoal = path[currentPoint];
        }
    }
}
