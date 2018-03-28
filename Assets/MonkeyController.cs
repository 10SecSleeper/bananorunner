using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyController : MonoBehaviour {

    private Vector3 velocity = Vector3.zero;

    Rigidbody rb;
    [SerializeField]
    Animator anim;
    MonkeyLogic logic;

    [SerializeField]
    Umpire ump;

    [SerializeField]
    List<AudioClip> MonkeySounds = new List<AudioClip>();

    AudioSource audplayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ObstacleMover>() != null)
        {
            if (other.gameObject.GetComponent<ObstacleMover>().banano)
            {
                Debug.Log("TOUCHING BANANO!");
                ump.CollectBanano();
                Destroy(other.gameObject);
            }
        }
    }

    // Use this for initialization
    void Awake () {

        rb = GetComponent<Rigidbody>();
        logic = GetComponent<MonkeyLogic>();
        audplayer = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {

        if ( Input.GetButton("MoveLeft") && !Input.GetButton("MoveRight") )
        {
            Move(1);
        }
        else if ( Input.GetButton("MoveRight") && !Input.GetButton("MoveLeft") )
        {
            Move(2);
        }
        else
        {
            Move(0);
        }


        if (Input.GetButtonDown("Jump") && transform.position.y < 2.7f )
        {
            Jump();
            MonkeySound();
        }

        if (transform.position.y > 2.7f)
        {
            anim.SetBool("OnGround", false);
        }
        else
            anim.SetBool("OnGround", true);
	}

    void MonkeySound()
    {
        if (MonkeySounds.Count > 0)
        {
            audplayer.Stop();
            int r = Random.Range(0, MonkeySounds.Count);

            audplayer.clip = MonkeySounds[r];
            audplayer.Play();
        }
    }

    void Jump()
    {
        rb.AddForce(new Vector3(0, 11f), ForceMode.Impulse);
    }

    void Move(int id)
    {
        Vector3 tempPos = new Vector3(0, transform.position.y, -2.5f);

        if (logic.colliding == true)
        {
            tempPos.z = transform.position.z;
        }

        switch (id)
        {
            case 1:
                tempPos.x = -2.5f;
                break;
            case 2:
                tempPos.x = 2.5f;
                break;
            default:
                tempPos.x = 0;
                break;
        }

        transform.position = Vector3.SmoothDamp(transform.position, tempPos, ref velocity, 0.3f);

        if (Mathf.Abs(transform.position.x) < Mathf.Abs(tempPos.x) - 0.86f && id != 0)
        {
            anim.SetFloat("Turn", tempPos.x / 4, 0.1f, Time.deltaTime);
        }
        else
            anim.SetFloat("Turn", 0, 0.1f, Time.deltaTime);
        
    }
}
