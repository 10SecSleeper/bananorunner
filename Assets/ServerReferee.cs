using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerReferee : NetworkBehaviour {

    [SerializeField]
    BananoNetManager netMan;

    [SerializeField]
    ObstacleDictionary odict;

    [SerializeField]
    PlayerTracker ptracker;

    PlayerTracker.PlayerListInterface pinterface = new PlayerTracker.PlayerListInterface();


    int timer = 0;

    [SerializeField]
    float timelimit = 30f;
    [SerializeField]
    int blocksize = 8;
    [SerializeField]
    float speed = 666f;

    public void PlayerJoined(NetworkConnection conn, GameObject pobj)
    {

        pinterface.AddPlayer(conn, pobj, timer, conn.address, "");

        ObstacleDictionary.ObstacleIndex tempIndex = new ObstacleDictionary.ObstacleIndex();

        tempIndex = odict.GenerateObstacleBlock(blocksize);

        pinterface.GetPlayerContainer(conn).SetGivenBananos(tempIndex.bananos);

        pobj.GetComponent<BananoCommunicator>().TargetReceiveMessage(conn, tempIndex.generated, blocksize, (timelimit-6)/blocksize, speed);

    }

    public void PlayerQuit(NetworkConnection conn)
    {

        pinterface.RemovePlayer(conn);

    }

    public void StartRefereeService()
    {

        StartCoroutine(WaitForLoad(GameObject.FindGameObjectWithTag("ObjectSpawner")) );
        
    }

    IEnumerator WaitForLoad(GameObject obj)
    {
        while (obj == null)
            yield return new WaitForSeconds(0.1f);

        InvokeRepeating("RefereeUpdate", 1, 1);
        odict = GameObject.FindGameObjectWithTag("ObjectSpawner").GetComponent<ObstacleDictionary>();
    }

    void Timer()
    {

        if (timer < timelimit)
        {
            timer += 1;
        }
        else
        {
            timer = 0;
        }

    }

    void RefereeUpdate()
    {

        Timer();
        //Debug.Log(timer);
        Dictionary<NetworkConnection, GameObject> tempP = pinterface.PlayersOnThisInterval(timer);

        foreach (var p in tempP)
        {
            int given = pinterface.GetPlayerContainer(p.Key).GetBananosGiven();

            int col = p.Value.GetComponent<BananoCommunicator>().bananosCollected;
            int mis = p.Value.GetComponent<BananoCommunicator>().bananosMissed;
            Debug.Log("This player collected " + col + " Banano and missed " + mis + " Banano.");
            Debug.Log("They were originally given " + given + " Banano.");

            PlayerTracker.PlayerDB db = new PlayerTracker.PlayerDB();
            string wal = pinterface.GetPlayerContainer(p.Key).GetWallet();

            if ((col + mis) == given)
            {
                Debug.Log("Player is acting in a legit manner.");
                db.AddBananos(wal, col);
            }
            else
            {
                Debug.Log("Player is acting suspicious.");
                db.AddFlag(wal);
            }

            p.Value.GetComponent<BananoCommunicator>().bananosCollected = 0;
            p.Value.GetComponent<BananoCommunicator>().bananosMissed = 0;

            ObstacleDictionary.ObstacleIndex tempIndex = new ObstacleDictionary.ObstacleIndex();

            tempIndex = odict.GenerateObstacleBlock(blocksize);

            pinterface.GetPlayerContainer(p.Key).SetGivenBananos(tempIndex.bananos);

            p.Value.GetComponent<BananoCommunicator>().TargetReceiveMessage(p.Key, tempIndex.generated, blocksize, (timelimit - 6) / blocksize, speed);

        }

    }

}
