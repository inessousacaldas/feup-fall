using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour {

    [SerializeField]
    private GameObject player;
    private float initialY;
    private float distance = 0f;
    private float cloneDistance = 5f;
    private float destroyDistance = 20f;
    private float heightWall = 35 * 0.5668f;
    private bool clone = true;

    // Use this for initialization
    void Start () {

        distance = 0f;
        clone = true;
        initialY = player.transform.position.y;
        //Debug.Log("initial : " + initialY + " " + distance);

    }

    // Update is called once per frame
    void LateUpdate() {

        distance = Mathf.Abs(player.transform.position.y - this.initialY);
       //Debug.Log("Distance: " + distance);
        if (distance > 2* heightWall)
            Destroy(gameObject);

        else if (clone && distance > heightWall)
        {
            clone = false;
            //Debug.Log("MAx DIST");
            Vector3 newPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - heightWall, gameObject.transform.position.z) ;

            Instantiate(gameObject, newPos, Quaternion.identity);

        }
    }


}
