using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BananoCommunicator : NetworkBehaviour {

    public GameObject objectSpawner;
    GameObject ump;

    [SyncVar]
    public int bananosCollected = 0;

    [SyncVar]
    public int bananosMissed = 0;

    [SyncVar]
    public string netWallet = "";

	// Use this for initialization
	void Awake () {
        NetworkIdentity nw = GetComponent<NetworkIdentity>();
        ump = GameObject.FindGameObjectWithTag("Umpire");

        ump.GetComponent<Umpire>().playercomm = this;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        CmdUpdateWallet(PlayerPrefs.GetString("LocalWallet"));
    }

    [Command]
    void CmdUpdateWallet(string w)
    {
        netWallet = w;
    }

	// Update is called once per frame
	void Update () {
		
	}

    [TargetRpc]
    public void TargetReceiveMessage(NetworkConnection target, string msg, int blocksize, float separationTime, float speed)
    {

        objectSpawner = GameObject.FindGameObjectWithTag("ObjectSpawner");
        objectSpawner.GetComponent<LevelStringAdapter>().ReceiveNewBlock(msg, blocksize, separationTime, speed);

    }

    [Command]
    public void CmdCollectBanano()
    {
        bananosCollected += 1;
    }

    [Command]
    public void CmdMissBanano()
    {
        bananosMissed += 1;
    }

    public void GameOver()
    {

        Debug.Log("Game Over!");
        BananoNetManager netman = GameObject.FindGameObjectWithTag("NetMan").GetComponent<BananoNetManager>();

        netman.StopClient();
    }

}
