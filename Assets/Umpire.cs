using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Umpire : MonoBehaviour {

    public BananoCommunicator playercomm;

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponent<ObstacleMover>() != null)
        {
            if (collision.gameObject.GetComponent<ObstacleMover>().banano)
            {
                playercomm.CmdMissBanano();
                Destroy(collision.gameObject);
            }
            else
                Destroy(collision.gameObject);
        }

        else if (collision.gameObject.name == "MonkeyPlayer")
        {
            playercomm.GameOver();
            Destroy(collision.gameObject);
        }

        else return;

    }

    public void CollectBanano()
    {
        playercomm.CmdCollectBanano();
    }

    public void EndGame()
    {
        playercomm.GameOver();
    }

}
