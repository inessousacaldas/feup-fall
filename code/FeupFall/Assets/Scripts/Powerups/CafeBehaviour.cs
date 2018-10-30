using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeBehaviour : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float time;

    [SerializeField]
    private float slowRatio;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player script = (Player)player.GetComponent(typeof(Player));
            script.slowSpeed(time, slowRatio);
        }
    }
}
