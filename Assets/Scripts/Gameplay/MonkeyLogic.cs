using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyLogic : MonoBehaviour {

    [SerializeField]
    Umpire ump;

    public bool colliding = false;

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.GetComponent<ObstacleMover>() != null)
        {

            colliding = true;

            if (col.gameObject.GetComponent<ObstacleMover>().banano)
            {
                ump.CollectBanano();
                Destroy(col.gameObject);
            }
            else if (col.gameObject.GetComponent<ObstacleMover>().deadly)
            {
                ump.EndGame();
            }
            else if (col.gameObject.name == "GameOverTrigger")
            {
                ump.EndGame();
            }

        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<ObstacleMover>() != null)
        {
            colliding = false;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float z = Mathf.Clamp(transform.position.z, -20f, -2.5f);
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, z);

        transform.position = newPos;
		
	}
}
