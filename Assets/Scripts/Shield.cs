using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]

    public float rotationsPerSecond = 0.1f;



    [Header("Set Dynamically")]

    public int levelShown = 0;

    // this non public variable will not appear int he inspector
    Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        // read the currnet shield level from the hero singleton
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);

        //if this different from levelShown...
        if (levelShown != currLevel)
        {
            levelShown = currLevel;

            //adjust the texture offset to show different shield level
            mat.mainTextureOffset = new Vector3(0.2f * levelShown, 0);
        }

        // Rotate the shield a bit every frame in a time-baded way
        float rZ = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);

    }
}
