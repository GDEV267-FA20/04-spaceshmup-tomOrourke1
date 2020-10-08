using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S; //Singleton


    [Header("Set in Inspector")]
    //these fields control the movement of the ship
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;

    public GameObject projectilePrefab;
    public float projectileSpeed = 40;

    public Weapon[] weapons;

    [Header("Set Dynamically")]
    //public float shieldLevel = 1;
    [SerializeField]
    private float _shieldLevel = 1;
    private GameObject lastTriggerGo = null;

    //Declare a new delagate type weaponFireDelegate
    public delegate void WeaponFireDelegate();

    //create a weponfire delegate field named firedelegate.
    public WeaponFireDelegate fireDelegate;

    private void Start()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assieng second Hero.S!");
        }

        //        fireDelegate += TempFire;


        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
    }


    private void Update()
    {
        // Pull in information from Input class

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        //change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        //rotate the ship to make it feel more dynamic
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            TempFire();
//        }
        
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
    }
    void TempFire()
    {

        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;

        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        //        rigidB.velocity = Vector3.up * projectileSpeed;


        Projectile proj = projGO.GetComponent<Projectile>();
        proj.type = WeaponType.blaster;
        float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
        rigidB.velocity = Vector3.up * tSpeed;



    }


    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;

        GameObject go = rootT.gameObject;

        print("Triggered: " + go.name);

        // Make sure it's not the same triggering go as last time

        if (go == lastTriggerGo)
        {
            return;
        }

        lastTriggerGo = go; 

        if (go.tag == "Enemy")
        { 
            shieldLevel--; 
            Destroy(go);          
        }
        else if(go.tag == "PowerUp")
        {
            AbsorbPowerUp(go);
        }
        else
        {
            print("Triggered by non-Enemy: " + go.name);  
        }
    }
    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();

        Debug.Log("AbsorbedPowerup\n\n: " + pu.type + " \n");
        switch (pu.type)
        {
            case WeaponType.shield:
                Debug.Log("AbsorbPowerUP == SHIELD: " + pu.type);
                shieldLevel++;
                break;
            default:
                if(pu.type == weapons[0].type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        w.SetType(pu.type);
                    }
                    
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;

        }
        pu.AbsorbedBy(this.gameObject);
    }
    public float shieldLevel
    {
        get
        {
            return (_shieldLevel);  
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);     
            if (value < 0)
            {    
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }

        }

    }


    Weapon GetEmptyWeaponSlot()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].type == WeaponType.none)
            {
                return weapons[i];
            }
        }
        return (null);
    }

    void ClearWeapons()
    {
        foreach(Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }





}
