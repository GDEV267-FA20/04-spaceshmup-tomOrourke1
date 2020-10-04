using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")] //

    public float speed = 10f;    
    public float fireRate = 0.3f; 
    public float health = 10;
    public int score = 100;
    protected BoundsCheck bndCheck;

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }
    public Vector3 pos
    {    
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }



    void Update()
    {
        Move();

        if (bndCheck != null && bndCheck.offDown)
        {
            if (pos.y < bndCheck.camHeight - bndCheck.radius)
            {
                Destroy(gameObject);
            }
        }
    }



    public virtual void Move()
    {     
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;

    }



    //void OnCollisionEnter(Collision coll)
    //{
    //    GameObject otherGO = coll.gameObject;   

    //    if (otherGO.tag == "ProjectileHero")
    //    {    
    //        Destroy(otherGO); 
    //        Destroy(gameObject); 
    //    }
    //    else
    //    {
    //        print("Enemy hit by non-ProjectileHero: " + otherGO.name);  
    //    }

    //}
    private void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;

        switch(otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();

                if(!bndCheck.isOnScreen) // dont damage enemy off screen
                {
                    Destroy(otherGO);
                    break;
                }

                //hurt enemy // get the damage amount from the main
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;

                if(health <= 0)
                {
                    //destroy this enemy
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;
            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;

        }




    }






}
