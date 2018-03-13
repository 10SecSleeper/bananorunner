using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyLogic : MonoBehaviour {

    [SerializeField]
    Umpire ump;

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.GetComponent<ObstacleMover>() != null)
        {

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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.position.z > -2.5f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -2.45f);
        }
		
	}
}
