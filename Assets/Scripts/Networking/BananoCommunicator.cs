using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BananoCommunicator : NetworkBehaviour {

    [SerializeField]
    private string localGameVersion = "b3";

    [SyncVar]
    public string netGameVersion = "";

    public GameObject objectSpawner;
    GameObject ump;

    public int bananosCollected = 0;

    public int bananosMissed = 0;

    public string netWallet = "";

	// Use this for initialization
	void Awake () {
  
    }

    public override void OnStartLocalPlayer()
    {
        NetworkIdentity nw = GetComponent<NetworkIdentity>();
        ump = GameObject.FindGameObjectWithTag("Umpire");

        ump.GetComponent<Umpire>().playercomm = this;

        base.OnStartLocalPlayer();

        CmdBroadcastVersion(localGameVersion);
        CmdUpdateWallet(PlayerPrefs.GetString("LocalWallet"));
        GameObject.FindGameObjectWithTag("ScenerySpawner").GetComponent<SceneryDictionary>().StartScenery();
    }

    [Command]
    void CmdBroadcastVersion(string v)
    {
        netGameVersion = v;
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
        Debug.Log(separationTime);

        bananosCollected = 0;
        bananosMissed = 0;

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

        netman.gameOver = true;

        netman.StopClient();
    }

}
