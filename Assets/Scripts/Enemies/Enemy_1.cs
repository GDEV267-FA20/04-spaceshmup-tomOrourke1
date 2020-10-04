using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{

    [Header("Set in Inspector: Enemy_1")]
    //# seconds for a full sine wave
    public float waveFrequency = 2f;

    //sinewave widthin meters
    public float waveWidth = 4f;
    public float waveRotY = 45f;

    private float x0;
    private float birthTime;


    private void Start()
    {
        x0 = pos.x;

        birthTime = Time.time;
    }


    public override void Move()
    {
        // cause pos is a property, you can't directly set pos.x
        // so get the pos ad an editable vector 3
        Vector3 tempPos = pos;

        //theta adjusts basxed on time
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;

        //rotate a bit about y
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);

        //Base.Move() still handles the movement down in y
        base.Move();

        //print(bndCheck.isOnScreen);
    }


}
