using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

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

    bool onFloor;

    bool moreGrav;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ObstacleMover>() != null)
        {
            if (other.gameObject.GetComponent<ObstacleMover>().banano)
            {
                ump.CollectBanano();
                Destroy(other.gameObject);
            }
            else if (other.gameObject.tag == "CheckPoint")
            {
                ump.CheckPoint();
            }
            else if (other.gameObject.tag == "RCP")
            {
                ump.RcheckPoint();
            }

        }
    }

    // Use this for initialization
    void Awake () {

        rb = GetComponent<Rigidbody>();
        logic = GetComponent<MonkeyLogic>();
        audplayer = GetComponent<AudioSource>();
        NetworkCRC.scriptCRCCheck = true;
    }

    void PCControls()
    {
        if (Input.GetButton("MoveLeft") && !Input.GetButton("MoveRight"))
        {
            Move(1);
        }
        else if (Input.GetButton("MoveRight") && !Input.GetButton("MoveLeft"))
        {
            Move(2);
        }
        else
        {
            Move(0);
        }


        if (Input.GetButtonDown("Jump") && onFloor)
        {
            Jump();
            onFloor = false;
        }

        if (!Input.GetButton("Jump") && !onFloor)
        {
            moreGrav = true;
        }
        else moreGrav = false;
    }

    void MobileControls()
    {
        if (CrossPlatformInputManager.GetButton("MoveLeft") && !CrossPlatformInputManager.GetButton("MoveRight"))
        {
            Move(1);
        }
        else if (CrossPlatformInputManager.GetButton("MoveRight") && !CrossPlatformInputManager.GetButton("MoveLeft"))
        {
            Move(2);
        }
        else
        {
            Move(0);
        }

        if (!CrossPlatformInputManager.GetButton("Jump") && !onFloor)
        {
            moreGrav = true;
        }
        else moreGrav = false;


        if (CrossPlatformInputManager.GetButtonDown("Jump") && onFloor)
        {
            Jump();
            onFloor = false;
        }
    }
	
	// Update is called once per frame
	void Update () {

        GetComponent<Collider>().enabled = true;

        if (Application.isMobilePlatform)
            MobileControls();
        else PCControls();
        

        if (!onFloor)
        {
            anim.SetBool("run", false);
            anim.SetBool("hop", true);
        }
        else
        {
            anim.SetBool("hop", false);
            anim.SetBool("run", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            onFloor = true;
            //Debug.Log("onFloor");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            onFloor = false;
            //Debug.Log("offFloor");
        }
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
        MonkeySound();
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

        if (id == 1 && transform.position.x > -2.0f)
        {
            anim.SetBool("run", false);
            anim.SetBool("runleft", true);
        }
        else if (id == 2 && transform.position.x < 2.0f)
        {
            anim.SetBool("run", false);
            anim.SetBool("runright", true);
        }
        else
        {
            anim.SetBool("runleft", false);
            anim.SetBool("runright", false);
            anim.SetBool("run", true);
        }

    }

    private void FixedUpdate()
    {

        if (moreGrav)
        {
            rb.AddForce(new Vector3(0, -9f), ForceMode.Acceleration);
        }
        
    }
}
