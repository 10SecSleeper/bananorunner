using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour {

    Vector3 vel;
    [SerializeField]
    public float speed = 200f;

    [SerializeField]
    public bool banano;

    [SerializeField]
    public bool deadly;

    Rigidbody rb;

	// Use this for initialization
	void Awake () {

	}

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = transform.position - transform.forward * speed * Time.deltaTime;

        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref vel, 0.5f);
    }
}
