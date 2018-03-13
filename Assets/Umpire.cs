﻿using System.Collections;
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
            }
        }

        else if (collision.gameObject.name == "MonkeyPlayer")
        {
            playercomm.GameOver();
        }
        
        Destroy(collision.gameObject);

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