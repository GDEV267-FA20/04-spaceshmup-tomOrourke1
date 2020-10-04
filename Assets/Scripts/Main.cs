using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;

    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;     
    public float enemySpawnPerSecond = 0.5f; 
    public float enemyDefaultPadding = 1.5f;

    public WeaponDefinition[] weaponDefinitions;

    private BoundsCheck bndCheck;


    void Awake()
    {
        S = this;

        bndCheck = GetComponent<BoundsCheck>();

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);


        // A generic dictionary with weaponType as the key
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }



    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);       
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);  

        float enemyPadding = enemyDefaultPadding;       

        if (go.GetComponent<BoundsCheck>() != null)
        {          
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);             

    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        SceneManager.LoadScene("_Scene_0");
    }

    /// <summary>
    /// Static funcion that gets a weaponDefinition from the weaponDict
    /// protecteed field of the main cladd 
    /// 
    /// </summary>
    /// <returns> the weaponDefinitiyns or, if there is no weaponDeinitions
    /// the weaponType passed in, returns a new WeaponDefinition with a 
    /// weaponType of none</returns>
    /// <param name="wt"> The weapon Type of the desired weaponDefinitions</param>
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        // Check to make sure that the key exists in the Dictionary
        // Attempting to retrieve a key that didn't exist, would throw an error,
        // so the following if statement is important.
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }

        // This returns a new WeaponDefinition with a type of WeaponType.none,
        //   which means it has failed to find the right WeaponDefinition
        return (new WeaponDefinition());

    }

}
