using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [Header("Set in Inspector")]
    //this is an unusual but andy use of vector2s. x hols a min calue and y a max value for a random.range
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 6f;
    public float fadeTime = 4f;

    [Header("Set Dynamically")]
    public WeaponType type;
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;
    public float birthTime;

    private Rigidbody rb;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;


    private void Awake()
    {
        //Find the Cube reference
        cube = transform.Find("Cube").gameObject;
        //find the textMesh and other components
        letter = GetComponent<TextMesh>();
        rb = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        //set a random velocity
        Vector3 vel = Random.onUnitSphere;

        vel.z = 0;
        vel.Normalize();

        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rb.velocity = vel;

        //set rot
        transform.rotation = Quaternion.identity;

        //set the rotPErsec
        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), 
                    Random.Range(rotMinMax.x, rotMinMax.y), 
                    Random.Range(rotMinMax.x, rotMinMax.y));

        birthTime = Time.time;
    }


    private void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);


        //fade out the powerup over time;; 10 sec and 4 fade sec
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;


        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }



        if(u > 0)
        {
            Color c = cubeRend.material.color;

            c.a = 1f - u;

            cubeRend.material.color = c;

            c = letter.color;

            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }


        if (!bndCheck.isOnScreen)
        {
            Destroy(gameObject);
        }



    }


    public void SetType(WeaponType wt)
    {
        //Grab the weaponDefinition from main
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        //set he color of the cube child
        cubeRend.material.color = def.color;

        //letter.color = def.color; // we could colorize the letter too
        letter.text = def.letter;
        type = wt;
    }

    public void AbsorbedBy(GameObject target)
    {
        //called when powerup is collected
        //would tween into target and shrink in size
        //but just destroy it for now
        Destroy(this.gameObject);
    }


}
