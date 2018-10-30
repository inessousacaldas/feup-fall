using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float xOffset = 0;
    [SerializeField]
    private float yOffset = 0;

    private Vector3 offset;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void LateUpdate () {

        transform.position = new Vector3(transform.position.x + xOffset, player.transform.position.y + yOffset, transform.position.z);
	}
}
