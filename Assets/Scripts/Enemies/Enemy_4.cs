using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///Part is another serializable data storage class just like weapondefinition
///</summary>
[System.Serializable]
public class Part
{
    //these three fields need to be defined in the Inspector pane
    public string name;
    public float health;
    public string[] protectedBy;

    //These two fields are set automatically in start
    // caching like this makes it faster and easier to find these later
    [HideInInspector]
    public GameObject go;
    [HideInInspector]
    public Material mat;
}





/// <summary>
/// Enemy_4 will start offscreen and then pick a random point of screento move to.
/// Once it has arrived, it will pack anouter random point and
/// continue unitl the player has shot it down
/// </summary>
/// 
public class Enemy_4 : Enemy
{
    [Header("Set in Inspector: Enemy_3")]
    public Part[] parts;

    private Vector3 p0, p1;
    private float timeStart;
    private float duration = 4;

    private void Start()
    {
        p0 = p1 = pos;

        InitMovement();


        //cach gameobject & material of each part in parts
        Transform t;
        foreach(Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                prt.mat = prt.go.GetComponent<Renderer>().material;
            }
        }
    }

    void InitMovement()
    {
        p0 = p1;

        //assign new on-screen loc to p1
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;

        p1.x = Random.Range(-widMinRad, widMinRad);
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);

        //reset the time
        timeStart = Time.time;
    }


    public override void Move()
    {
        float u = (Time.time - timeStart) / duration;

        if (u >= 1)
        {
            InitMovement();
            u = 0;
        }

        u = 1 - Mathf.Pow(1 - u, 2); // apply ease out easing to u
        pos = (-1u) * p0 + u * p1; // simple linear interpoation


    }


    //These two function find a part in parts based on name or gameobject
    Part FindPart(string n)
    {
        foreach(Part prt in parts)
        {
            if (prt.name ==n)
            {
                return (prt);
            }
        }
        return null;
    }

    Part FindPart(GameObject go)
    {
        foreach(Part prt in parts)
        {
            if (prt.go == go)
            {
                return prt;
            }

        }
        return null;
    }


    //these functions return true if the part has been destroyed
    bool Destroyed(GameObject go)
    {
        return (Destroyed(FindPart(go)));
    }

    bool Destroyed(string n)
    {
        return (Destroyed(FindPart(n)));
    }

    bool Destroyed(Part prt)
    {
        if (prt == null)
        {
            return true;
        }

        //retuns the result ofthe comparison prt.health <= 0
        //if prt.health is 0 or les
        return (prt.health <= 0);

    }

    //this changes the colors of just one part to red
    void ShowLocalizedDamege(Material m)
    {
        m.color = Color.red;
        damageDoneTime = Time.time + showDamageDuration;
        showingDamage = true;
    }

    // this will override the onCollision enter that is part of enemy
    private void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        switch(other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                // if enemy is off screen, don do damage
                if (!bndCheck.isOnScreen)
                {
                    Destroy(other);
                    break;
                }

                //hurt this enemy
                GameObject goHit = coll.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);
                if (prtHit == null)
                {
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }

                //check weether this part is still protected
                if (prtHit.protectedBy != null)
                {
                    foreach(string s in prtHit.protectedBy)
                    {
                        //if one of the protecting parts hasn't been destroyed...
                        if (!Destroyed(s))
                        {
                            //then don't damage this part yet
                            Destroy(other);
                            return;
                        }
                    }
                }
                //it's not protected, so make it take damage
                // get the damage amount from the projectile.type and main.W_DEFS
                prtHit.health -= Main.GetWeaponDefinition(p.type).damageOnHit;

                //show damage on the part
                ShowLocalizedDamege(prtHit.mat);

                if(prtHit.health <= 0)
                {
                    //instead of destroying this enemy, disable the damaged part
                    prtHit.go.SetActive(false);
                }

                //Check to see if the whole ship is destroyed
                bool allDestroyed = true; //assume it is destroyed
                foreach(Part prt in parts)
                {
                    if (!Destroyed(prt)) // if a part still exists
                    {
                        allDestroyed = false;
                        break;
                    }
                }
                if (allDestroyed) // iof completly destroyed
                {
                    //tell the main singleton that this ship was destroyted
                    Main.S.shipDestroyed(this);
                    //Destroy this enemy
                    Destroy(this.gameObject);
                }
                Destroy(other); // destroy the projectile
                break;
        }
    }

}
