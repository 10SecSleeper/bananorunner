using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class BananoNetManager : NetworkManager
{

    [SerializeField]
    public bool headlessMode;
    
    public GameObject UICanvas;
    public GameObject ServerCanvas;
    public ServerReferee referee;

    public bool gameOver = false;
    public GameObject GameOverUI;
    public GameObject ConnectingUI;
    public GameObject DisconnectMsg;

    string netAdd = "73.170.106.2";
    int netPort = 27000;

    public string clientWallet;

    [SerializeField]
    private string currentGameVersion = "b3";

    void Start()
    {

        LoadNetConfig();

        if (headlessMode)
            StartAsServer();

    }

    void LoadNetConfig()
    {

        string filePath = Application.dataPath + "/" + "networkConfig.txt";
        string tmpString = "";


        if (System.IO.File.Exists(filePath))
        {
            tmpString = System.IO.File.ReadAllText(filePath);

            string ip = tmpString.Split(':')[0];
            string port = tmpString.Split(':')[1];

            netAdd = ip;
            int.TryParse(port, out netPort);
        }
        else
        {
            var sr = File.CreateText(filePath);

            string add = netAdd + ":" + netPort.ToString();

            sr.WriteLine(add);
            sr.Close();

            return;
        }

    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server Started");
        StartCoroutine(DelayReferee());
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        ServerCanvas.SetActive(false);
        UICanvas.SetActive(true);
    }

    IEnumerator DelayReferee()
    {
        yield return new WaitForSeconds(2);
        referee.StartRefereeService();
    }


    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        Debug.Log("Player Joined");
    }


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);


        StartCoroutine(DelayPlayerAdd(conn, player));
        
    }

    IEnumerator DelayPlayerAdd(NetworkConnection conn, GameObject player)
    {
        yield return new WaitForSeconds(2);

        if (player.GetComponent<BananoCommunicator>().netGameVersion != currentGameVersion)
        {
            conn.Disconnect();
        }
        else
            referee.PlayerJoined(conn, player);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        referee.PlayerQuit(conn);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        ConnectingUI.SetActive(false);
    }

    public void StartAsServer()
    {

        serverBindAddress = netAdd;
        networkPort = netPort;
        networkAddress = netAdd;

        StartServer();
        

        UICanvas.SetActive(false);
        ServerCanvas.SetActive(true);

    }

    public void StartAsClient()
    {

        networkAddress = netAdd;
        networkPort = netPort;

        StartClient();

        GameOverUI.SetActive(false);
        UICanvas.SetActive(false);
        ConnectingUI.SetActive(true);

    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        UICanvas.SetActive(true);
        ConnectingUI.SetActive(false);

        if (!gameOver)
        {
            UICanvas.SetActive(true);
            DisconnectMsg.SetActive(true);
        }

        if (gameOver)
        {
            GameOverUI.SetActive(true);
            GameOverUI.GetComponent<GameOverScreen>().SetBananos();
            gameOver = false;
        }

    }


    public override void OnStopClient()
    {
        base.OnStopClient();

        if (!gameOver)
            UICanvas.SetActive(true);

        if (gameOver)
        {
            GameOverUI.SetActive(true);
            GameOverUI.GetComponent<GameOverScreen>().SetBananos();
            gameOver = false;
        }

        ConnectingUI.SetActive(false);
    }

}
