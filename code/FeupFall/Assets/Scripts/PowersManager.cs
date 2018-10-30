using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowersManager : MonoBehaviour
{

    [SerializeField]
    private float powerupRate;
    [SerializeField]
    private float recycleOffset;
    [SerializeField]
    private List<GameObject> availablePowerups;

    private List<GameObject> lPowerups;
    private float generationOffset = 10;
    private float lstGeneration;
    private float leftLimit = -2.483195f;
    private float rightLimit = 2.4903f;
    private int maxIterations = 5;

    // Use this for initialization
    void Start()
    {

        lstGeneration = Player.playerPosition.y - generationOffset;
        lPowerups = new List<GameObject>();
        Recycle();
    }

    // Update is called once per frame
    void Update()
    {

        if (Mathf.Abs(lstGeneration - Player.playerPosition.y) < generationOffset)
            Recycle();
    }

    private bool overlaps(GameObject go, string tag)
    {
        GameObject[] platforms = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject plat in platforms)
        {
            if (go.GetComponent<Renderer>().bounds.Intersects(plat.GetComponent<Renderer>().bounds))
                return true;
        }
        return false;
    }

    private GameObject CreateNewPowerup()
    {
        Vector3 pos = new Vector3();
        GameObject newPowerup, go = null;
        var iteration = 0;
        var valid = false;

        //select type of powerup
        newPowerup = availablePowerups[Random.Range(0,availablePowerups.Count -1)];

        //powerup cannot overlap platform position
       while(!valid && iteration < maxIterations)
        {
            //determine pos
            pos.x = Random.Range(leftLimit, rightLimit);
            pos.y = lstGeneration;
            pos.z = 0;

            go = Instantiate(newPowerup);
            go.transform.Translate(pos.x, pos.y, pos.z);

            Debug.Log("Size:" + go.GetComponent<Renderer>().bounds.size);
            Debug.Log("Pos:" + go.GetComponent<Renderer>().transform.position);
            valid = !overlaps(go, "platformNormal");
            if (valid)
                valid = !overlaps(go, "platformNormal");

            if (!valid)
                Destroy(go);

            iteration++;
        }

        if (!valid)
            Destroy(go);
        return go;
    }

    private void Recycle()
    {
        if (Random.Range(0f, 1f) < powerupRate && availablePowerups.Count > 0)
        {
            lstGeneration = lstGeneration - recycleOffset - Random.Range(0f, recycleOffset);
            var newPowerup = CreateNewPowerup();
            if (newPowerup != null)
                lPowerups.Add(newPowerup);
        }

        //clear powerups above screen
        for (var i = lPowerups.Count - 1; i >= 0; i--)
        {
            //Powerup already used
            if (lPowerups[i] == null)
                lPowerups.RemoveAt(i);
            else if (lPowerups[i].transform.position.y - Player.playerPosition.y > generationOffset)
            {
                Destroy(lPowerups[i]);
                lPowerups.RemoveAt(i);
            }
        }
    }
}