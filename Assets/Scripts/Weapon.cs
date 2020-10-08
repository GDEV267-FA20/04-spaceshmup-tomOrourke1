using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This is an enum of the varouse possible weapon types.
/// it also includes type to allow a shield power up
/// items marked [ni] below are not implemented in the IGDPD booi.
/// </summary>
/// 

public enum WeaponType
{
    none, 
    blaster,
    spread,
    phaser, //NI
    missile, //NI
    laser, //NI
    shield,
    bigBeam
}
/// <summary>
/// THe weapon definition class allows you to set the properties of a specific weapon in the inspector. the main class has
/// a narray fo weapon definitions that makes this possible
/// </summary>

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;
    public Color color = Color.white;
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;
    public float continuousDamage = 0;
    public float delayBetweenSHots = 0;
    public float velocity = 20;

}
public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime;
    private Renderer collarRend;

    private void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        //call setytype() for the defaul )type of weaponType.none
        Debug.Log("_type: " + _type);
        SetType(_type);

        //Dynamically create an anchor for all projectiles
        if(PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        //find the fireDelegate of the root gameobject
        GameObject rootGO = transform.root.gameObject;
        if(rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }

    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }

    public void SetType(WeaponType wt)
    {
        _type = wt;

        if(type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }

        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color= def.color;
        lastShotTime = 0;
    }


    public void Fire()
    {
        //If this . game Object is inactive, retunr
        if (!gameObject.activeInHierarchy) return;
        
        //if it hasn't been enough time between shots, return
        if(Time.time - lastShotTime < def.delayBetweenSHots)
        {
            return;
        }

        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }

        switch (type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;
            case WeaponType.spread:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                p = MakeProjectile();     // Make right Projectile
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile();     // Make left Projectile
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
            case WeaponType.bigBeam:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;

        }

    }


    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);

        if(transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }

        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return (p);



    }

}
