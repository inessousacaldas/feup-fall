using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    [SerializeField]
    private GameObject destructiblePlatform;
    [SerializeField]
    private GameObject fixedPlatform;
    [SerializeField]
    private float platformRate;
    [SerializeField]
    private float destructiblePerc;
    [SerializeField]
    private float recycleOffset;
    [SerializeField]
    private float neighbourPlatPerc;

    private List<GameObject> lPlatforms;
    private float generationOffset = 10;
    private float lstGeneration;
    private float leftLimit = -2.483195f;
    private float rightLimit = 2.4903f;
    private float deltaSpace = 0.5f;


    // Use this for initialization
    void Start()
    {

        lstGeneration = Player.playerPosition.y - generationOffset;
        lPlatforms = new List<GameObject>();
        Recycle();
    }

    // Update is called once per frame
    void Update()
    {

        if (Mathf.Abs(lstGeneration - Player.playerPosition.y) < generationOffset)
            Recycle();
    }

    private bool overlapsPowerups(GameObject go)
    {
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (GameObject plat in platforms)
        {
            if (go.GetComponent<Renderer>().bounds.Intersects(plat.GetComponent<Renderer>().bounds))
                return true;
        }
        return false;
    }

    private bool canPlayerPass(Vector3 lastPos, int dir)
    {
        // if there is space to right to last platform placed
        if (dir > 0 && lastPos.x < rightLimit - deltaSpace)
            return true;
        // if there is space to left to last platform placed
        else if (dir < 0 && lastPos.x > leftLimit + deltaSpace)
            return true;
        else if (lPlatforms.Count > 0)
        {
            //get the first platform placed on same height
            var i = lPlatforms.Count;
            var tmp = lPlatforms[i - 1].transform.position.y;
            while (lastPos.y == tmp && i > 0)
            {
                i--;
                tmp = lPlatforms[i].transform.position.y;
            }
            if (i == 0)
                tmp = lPlatforms[0].transform.position.x;
            tmp = lPlatforms[i + 1].transform.position.x;
            // if there is space to left to first platform placed
            if (tmp < 0 && tmp > leftLimit + deltaSpace)
                return true;
            // if there is space to right to first platform placed
            else if (tmp > 0 && tmp < rightLimit - deltaSpace)
                return true;
            // if first platform placed was in the middle
            else if (tmp == 0)
                return true;
        }
        return false;
    }

    private GameObject CreateNewPlatform()
    {
        Vector3 pos;
        GameObject newPlatform;

        //select type of platform
        if (Random.Range(0f, 1f) < destructiblePerc)
            newPlatform = destructiblePlatform;
        else newPlatform = fixedPlatform;

        //determine pos
        pos.x = Random.Range(leftLimit, rightLimit);
        pos.y = lstGeneration;
        pos.z = 0;

        var go = Instantiate(newPlatform);
        go.transform.Translate(pos.x, pos.y, pos.z);

        if (overlapsPowerups(go))
        {
            Destroy(go);
            return null;
        }
        return go;
    }

    private GameObject CreateNeighbour(GameObject goToClone, int neighbourCnt, ref int direction)
    {
        var newNeighbour = Instantiate(goToClone);
        var size = goToClone.GetComponent<Renderer>().bounds.size;

        //there must be available space to place platform (check left side and right side)
        var goToClonePos = goToClone.transform.position;

        //if no direction selected, select randomly if left or right
        if (direction == 0)
        {
            direction = 1;
            if (Random.Range(0f, 1f) < 0.5)
                direction *= -1;
        }

        //Test one direction
        var newX = goToClonePos.x + direction * size.x;
        if (newX >= leftLimit && newX <= rightLimit)
            newNeighbour.transform.Translate(direction * size.x, 0, 0);
        //Test other direction if previous failed
        else
        {
            direction *= -1;
            newX = goToClonePos.x + direction * size.x;
            //don't change direction if neighbourCnt > 0
            if (newX >= leftLimit && newX <= rightLimit && neighbourCnt == 0)
                newNeighbour.transform.Translate(direction * size.x, 0, 0);
            else
            {
                Destroy(newNeighbour);
                return null;
            }
        }

        //platform must leave space to player progress
        if (canPlayerPass(newNeighbour.transform.position, direction) && !overlapsPowerups(newNeighbour))
            return newNeighbour;
        else
        {
            Destroy(newNeighbour);
            return null;
        }
    }

    private void Recycle()
    {
        if (Random.Range(0f, 1f) < platformRate)
        {

            //read - as +, as absolute number go higher and higher
            lstGeneration = lstGeneration - recycleOffset - Random.Range(0f, recycleOffset);
            var newPlatform = CreateNewPlatform();
            if (newPlatform != null)
                lPlatforms.Add(newPlatform);

            var neighbourPlatform = newPlatform;
            var dir = 0;
            var neighbourCnt = 0;

            while (Random.Range(0f, 1f) < neighbourPlatPerc && neighbourPlatform != null)
            {
                neighbourPlatform = CreateNeighbour(neighbourPlatform, neighbourCnt, ref dir);
                if (neighbourPlatform != null)
                {
                    neighbourCnt++;
                    lPlatforms.Add(neighbourPlatform);
                }
            }
        }

        //clear platforms above screen
        for (var i = lPlatforms.Count - 1; i >= 0; i--)
        {
            //Destroyed by projetile
            if (lPlatforms[i] == null)
                lPlatforms.RemoveAt(i);
            else if (lPlatforms[i].transform.position.y - Player.playerPosition.y > generationOffset)
            {
                Destroy(lPlatforms[i]);
                lPlatforms.RemoveAt(i);

            }
        }
    }
}
