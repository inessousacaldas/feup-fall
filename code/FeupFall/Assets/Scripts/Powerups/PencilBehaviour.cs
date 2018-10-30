using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilBehaviour : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float time;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player script = (Player)player.GetComponent(typeof(Player));
            script.unsetConsume(time);
        }
    }
}
