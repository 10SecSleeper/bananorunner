using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BananoNetManager : NetworkManager
{
    
    public GameObject UICanvas;
    public GameObject ServerCanvas;
    public ServerReferee referee;

    public bool gameOver = false;
    public GameObject GameOverUI;
    public GameObject ConnectingUI;
    public GameObject DisconnectMsg;

    public string clientWallet;

    void Start()
    {

    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server Started");
        StartCoroutine(DelayReferee());
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

        serverBindAddress = "73.170.106.2";
        networkPort = 27000;
        networkAddress = "73.170.106.2";

        StartServer();
        Debug.Log("Testing");

        UICanvas.SetActive(false);
        ServerCanvas.SetActive(true);

    }

    public void StartAsClient()
    {

        networkAddress = "73.170.106.2";
        networkPort = 27000;

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
