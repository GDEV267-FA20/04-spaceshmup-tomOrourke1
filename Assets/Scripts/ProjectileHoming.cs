using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHoming : Projectile
{

    private Vector3 velocity;

    private float speed = 25;

    private GameObject[] enemies;
   
    private float radius = 40f;

    private int maskIndex;


    private float timer, duration = 5;
    private float startTime;

    private Collider[] hitsCollected;

    GameObject target;


    
    
    private void Start()
    {
        startTime = Time.time;
        hitsCollected = Physics.OverlapSphere(transform.position, radius, 1 << 10);
        enemies = new GameObject[hitsCollected.Length];
        for(int i = 0; i < hitsCollected.Length; i++)
        {
            enemies[i] = hitsCollected[i].gameObject;
        }


        GameObject previous = null;
        bool prevBool = false;

        for (int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i].transform.position.y > transform.position.y)
            {
                if (!prevBool)
                {
                    target = enemies[i];
                    previous = target;
                    prevBool = true;
                }
                else
                {
                    target = enemies[i];

                    if (target.transform.position.magnitude > previous.transform.position.magnitude)
                    {
                        target = previous;
                    }


                }
            }
            

        }





    }
    protected override void HomeIn()
    {
        Vector3 dir = Vector3.up;
        if (target != null)
        {
            dir = (target.transform.position - transform.position).normalized;
            
        }
        else
        {
            Destroy(this.gameObject);
        }
        


        rigid.velocity = dir * speed;

        transform.rotation = Quaternion.LookRotation(Vector3.back, dir);


        timer = Time.time - startTime;
        if(timer >= duration)
        {
            Destroy(this.gameObject);
        }

       


    }
    



}
