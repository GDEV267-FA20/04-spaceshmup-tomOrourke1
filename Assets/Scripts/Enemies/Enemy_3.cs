using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    // Enemy_3 will move following a Bezier curve, which is a linear
    //   interpolation between more than two points.

    [Header("Set in Inspector: Enemy_3")]
    public float lifeTime = 5f;

    [Header("Set Dynamically: Enemy_3")]
    public Vector3[] points;
    public float birthTime;

    private void Start()
    {
        points = new Vector3[3];

        points[0] = pos;

        //set min and max
        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth + bndCheck.radius;

        Vector3 v;

        //random middle pos
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = -bndCheck.camHeight * Random.Range(2.75f, 2);

        points[1] = v;

        //random final pos
        v = Vector3.zero;
        v.y = pos.y;
        v.x = Random.Range(xMin, xMax);

        points[2] = v;

        //set birthtime
        birthTime = Time.time;
    }




    public override void Move()
    {



















    }










}
