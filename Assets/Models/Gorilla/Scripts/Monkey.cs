using UnityEngine;
using System.Collections;

public class Monkey : MonoBehaviour {
    private IEnumerator coroutine;
    Animator monkey;

	// Use this for initialization
	void Start () {

        monkey = GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () {
        if ((Input.GetKey(KeyCode.W)) || (Input.GetKey("down")))
        {
            monkey.SetBool("walk", true);
            monkey.SetBool("left", false);
            monkey.SetBool("right", false);
            monkey.SetBool("idle", false);
            monkey.SetBool("run", false);
            monkey.SetBool("runleft", false);
            monkey.SetBool("runright", false);
            monkey.SetBool("hop", false);
            monkey.SetBool("hopleft", false);
            monkey.SetBool("hopright", false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            monkey.SetBool("left", true);
            monkey.SetBool("right", false);
            monkey.SetBool("walk", false);
            monkey.SetBool("idle", false);
            monkey.SetBool("run", false);
            monkey.SetBool("runleft", false);
            monkey.SetBool("runright", false);
            monkey.SetBool("hop", false);
            monkey.SetBool("hopleft", false);
            monkey.SetBool("hopright", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            monkey.SetBool("right", true);
            monkey.SetBool("left", false);
            monkey.SetBool("walk", false);
            monkey.SetBool("idle", false);
            monkey.SetBool("run", false);
            monkey.SetBool("runleft", false);
            monkey.SetBool("runright", false);
            monkey.SetBool("hop", false);
            monkey.SetBool("hopleft", false);
            monkey.SetBool("hopright", false);
        }
        if ((Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.Keypad5)))
        {
            monkey.SetBool("idle", true);
            monkey.SetBool("walk", false);
            monkey.SetBool("right", false);
            monkey.SetBool("left", false);
            monkey.SetBool("run", false);
            monkey.SetBool("runleft", false);
            monkey.SetBool("runright", false);
            monkey.SetBool("hop", false);
            monkey.SetBool("hopleft", false);
            monkey.SetBool("hopright", false);
        }
        if (Input.GetKey("up"))
        {
            monkey.SetBool("run", true);
            monkey.SetBool("walk", false);
            monkey.SetBool("left", false);
            monkey.SetBool("right", false);
            monkey.SetBool("runleft", false);
            monkey.SetBool("runright", false);
            monkey.SetBool("hop", false);
            monkey.SetBool("hopleft", false);
            monkey.SetBool("hopright", false);
        }
        if (Input.GetKey("left"))
        {
            monkey.SetBool("runleft", true);
            monkey.SetBool("walk", false);
            monkey.SetBool("left", false);
            monkey.SetBool("right", false);
            monkey.SetBool("run", false);
            monkey.SetBool("runright", false);
            monkey.SetBool("hop", false);
            monkey.SetBool("hopleft", false);
            monkey.SetBool("hopright", false);
        }
        if (Input.GetKey("right"))
        {
            monkey.SetBool("runright", true);
            monkey.SetBool("walk", false);
            monkey.SetBool("left", false);
            monkey.SetBool("right", false);
            monkey.SetBool("runleft", false);
            monkey.SetBool("run", false);
            monkey.SetBool("hop", false);
            monkey.SetBool("hopleft", false);
            monkey.SetBool("hopright", false);
        }
        if (Input.GetKey(KeyCode.Keypad8))
        {
            monkey.SetBool("hop", true);
            monkey.SetBool("hopleft", false);
            monkey.SetBool("hopright", false);
            monkey.SetBool("run", false);
            monkey.SetBool("runleft", false);
            monkey.SetBool("runright", false);
            monkey.SetBool("idle", false);
        }
        if (Input.GetKey(KeyCode.Keypad4))
        {
            monkey.SetBool("hopleft", true);
            monkey.SetBool("hopright", false);
            monkey.SetBool("hop", false);
            monkey.SetBool("run", false);
            monkey.SetBool("runleft", false);
            monkey.SetBool("runright", false);
            monkey.SetBool("idle", false);
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            monkey.SetBool("hopright", true);
            monkey.SetBool("hopleft", false);
            monkey.SetBool("hop", false);
            monkey.SetBool("run", false);
            monkey.SetBool("runleft", false);
            monkey.SetBool("runright", false);
            monkey.SetBool("idle", false);
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            monkey.SetBool("roll", true);
            monkey.SetBool("idle", false);
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            monkey.SetBool("eat", true);
            monkey.SetBool("idle", false);
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            monkey.SetBool("happy", true);
            monkey.SetBool("idle", false);
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            monkey.SetBool("jump", true);
            monkey.SetBool("idle", false);
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            monkey.SetBool("somersault", true);
            monkey.SetBool("idle", false);
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.Alpha0))
        {
            monkey.SetBool("die", true);
            monkey.SetBool("idle", false);
            monkey.SetBool("hop", false);
            monkey.SetBool("hopleft", false);
            monkey.SetBool("hopright", false);
            monkey.SetBool("walk", false);
            monkey.SetBool("left", false);
            monkey.SetBool("right", false);
            monkey.SetBool("run", false);
            monkey.SetBool("runleft", false);
            monkey.SetBool("runright", false);
        }
    }
    IEnumerator idle()
    {
        yield return new WaitForSeconds(0.5f);
        monkey.SetBool("idle", true);
        monkey.SetBool("eat", false);
        monkey.SetBool("happy", false);
        monkey.SetBool("jump", false);
        monkey.SetBool("roll", false);
        monkey.SetBool("somersault", false);
    }
}
