using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Set in Inpsector")]
    public GameObject poi;
    public GameObject[] panels;
    public float scrillSpeed = -30f;

    //motion controls how much panels reach to player
    public float motionMult = 0.25f;

    private float panelHt;
    private float depth;





    private void Start()
    {
        panelHt = panels[0].transform.localScale.y;
        depth = panels[0].transform.position.z;

        //set initial pos
        panels[0].transform.position = new Vector3(0, 0, depth);
        panels[1].transform.position = new Vector3(0, panelHt, depth);



    }



    private void Update()
    {
        float tY, tX = 0;
        tY = Time.time * scrillSpeed % panelHt + (panelHt * 0.5f);

        if (poi != null)
        {
            tX = -poi.transform.position.x * motionMult;
        }


        //position panels[0]
        panels[0].transform.position = new Vector3(tX, tY, depth);

        //then position panels [1]

        if (tY >= 0)
        {
            panels[1].transform.position = new Vector3(tX, tY - panelHt, depth);

        }
        else
        {
            panels[1].transform.position = new Vector3(tX, tY + panelHt, depth);
        }







    }


}