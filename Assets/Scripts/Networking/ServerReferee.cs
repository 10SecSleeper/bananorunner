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

    public PlayerTracker.PlayerListInterface pinterface = new PlayerTracker.PlayerListInterface();


    int timer = 0;

    [SerializeField]
    int timelimit = 30;
    [SerializeField]
    int blocksize = 8;
    [SerializeField]
    float speed = 666f;
    [SerializeField]
    int waitDelay = 6;

    public void PlayerJoined(NetworkConnection conn, GameObject pobj)
    {

        string tmpwal = pobj.GetComponent<BananoCommunicator>().netWallet;

        Debug.Log(tmpwal);

        pinterface.AddPlayer(conn, pobj, timer, conn.address, tmpwal);

        ObstacleDictionary.ObstacleIndex tempIndex = new ObstacleDictionary.ObstacleIndex();

        tempIndex = odict.GenerateObstacleBlock(blocksize);

        pinterface.GetPlayerContainer(conn).SetGivenBananos(tempIndex.bananos);

        float sepTime = (float)(timelimit - waitDelay) / blocksize;

        pobj.GetComponent<BananoCommunicator>().TargetReceiveMessage(conn, tempIndex.generated, blocksize, sepTime, speed);

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

            if ((col + mis) <= (given+1) && (col+mis) >= (given-1))
            {
                db.AddBananos(wal, col);
            }
            else
            {
                db.AddFlag(wal);
            }

            // Add time to the player so the server can keep track of time played
            db.AddTime(wal, timelimit);

            p.Value.GetComponent<BananoCommunicator>().bananosCollected = 0;
            p.Value.GetComponent<BananoCommunicator>().bananosMissed = 0;

            ObstacleDictionary.ObstacleIndex tempIndex = new ObstacleDictionary.ObstacleIndex();

            tempIndex = odict.GenerateObstacleBlock(blocksize);

            pinterface.GetPlayerContainer(p.Key).SetGivenBananos(tempIndex.bananos);

            float sepTime = (float)(timelimit - waitDelay) / blocksize;
            p.Value.GetComponent<BananoCommunicator>().TargetReceiveMessage(p.Key, tempIndex.generated, blocksize, sepTime, speed);

        }

    }

}
