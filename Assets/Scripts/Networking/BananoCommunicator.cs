using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using SimpleJSON;

public class BananoCommunicator : MonoBehaviour {

    // Game tracking vars
    private int bananosCollected = 0;
    private int bananosMissed = 0;


    // UI Elements
    [SerializeField]
    GameObject gameOverUI;

    [SerializeField]
    GameObject connectingUI;

    [SerializeField]
    GameObject captchaUI;
    [SerializeField]
    Text captchaText;

    [SerializeField]
    GameObject errorUI;
    [SerializeField]
    Text errorText;

    [SerializeField]
    GameObject scoreUI;
    ScoreUIManager sM;

    [SerializeField]
    GameObject SceneryObj;

    [SerializeField]
    LevelStringAdapter lsa;

    System.Action<JSONNode> validationGot;
    System.Action<JSONNode> gamepacketGot;

    string rootURL = "http://localhost:8000";
    string validationURL = "/amivalid";
    string captchaURL = "/robochecker";
    string packetURL = "/gamepacket";

    public class ErrorCode
    {
        public int code;
        public string message;

        public ErrorCode(int i, string m)
        {
            this.code = i;
            this.message = m;
        }
    }

    private void Start()
    {
        sM = scoreUI.GetComponent<ScoreUIManager>();
        validationGot += ValidationReceipt;
        gamepacketGot += GamePacketReceipt;

        // Send out a packet to the server telling it we have started our game.
        StartCoroutine(ValidateClient(rootURL + validationURL, PlayerPrefs.GetString("LocalWallet"), ScoreKeeper.gameVersion, validationGot));
    }

    // A coroutine which loads data from the json url set in vars
    public static IEnumerator ValidateClient(string url, string wallet, string gameversion, System.Action<SimpleJSON.JSONNode> action)
    {
        WWWForm wwwf = new WWWForm();
        wwwf.AddField("wallet", wallet);
        wwwf.AddField("version", gameversion);

        UnityWebRequest www = UnityWebRequest.Post(url, wwwf);
        yield return www.SendWebRequest();

        if (www.isDone)
        {
            if (string.IsNullOrEmpty(www.error))
            {
                var N = JSONNode.Parse(www.downloadHandler.text);

                action(N);
            }
            else
            {
                var error = new ErrorCode(2, "The server could not be reached right now. Please try again later!");

                Debug.Log(JsonUtility.ToJson(error));

                action(JSONNode.Parse(JsonUtility.ToJson(error)));

            }
        }
    }

    // A coroutine that sends in game data and grabs new blocks.
    public static IEnumerator SendGamePacket(string url, int collected, int missed, string wallet, string gameversion, System.Action<JSONNode> action)
    {
        Debug.Log("Sending game packet...");

        WWWForm wwwf = new WWWForm();
        wwwf.AddField("wallet", wallet);
        wwwf.AddField("version", gameversion);
        wwwf.AddField("collected", collected.ToString());
        wwwf.AddField("missed", missed.ToString());

        UnityWebRequest www = UnityWebRequest.Post(url, wwwf);
        yield return www.SendWebRequest();

        if (www.isDone)
        {
            if (string.IsNullOrEmpty(www.error))
            {
                var N = JSONNode.Parse(www.downloadHandler.text);

                action(N);
            }
            else
            {
                var error = new ErrorCode(2, "The server could not be reached right now. Please try again later!");
                action(JSONNode.Parse(JsonUtility.ToJson(error)));
            }
        }
    }

    public void CollectBanano()
    {
        bananosCollected += 1;
    }

    public void MissBanano()
    {
        bananosMissed += 1;
    }

    public void GameOver()
    {
        ScoreKeeper.gameOver = true;
        QuitGame();
    }

    public void QuitGame()
    {
        SceneManager.LoadSceneAsync(0);
    }


    public void Rcheckpoint()
    {
        // Send out a packet to the server that we completed the real obstacles in the block
        StartCoroutine(SendGamePacket(rootURL + packetURL, bananosCollected, bananosMissed, PlayerPrefs.GetString("LocalWallet"), ScoreKeeper.gameVersion, gamepacketGot));
        bananosCollected = 0;
        bananosMissed = 0;
    }

    // Receipt of POST for a standard game packet
    public void GamePacketReceipt(JSONNode N)
    {
        Debug.Log("Got packet");
        Debug.Log(N["message"]);

        // Player is still valid. Get the next block
        if (N["code"] == 1)
        {
            var wBlock = N["block"];
            var text = wBlock["text"];
            var length = wBlock["length"];
            var time = wBlock["time"];
            var delay = wBlock["delay"];
            var speed = wBlock["speed"];

            // Send this new block and data to the manager
            lsa.ReceiveNewBlock(text, length, time, delay, speed);

        }
        else if (N["code"] == 0)
        {
            ClearField();
            SceneryObj.GetComponent<SceneryDictionary>().StopScenery();

            captchaText.text = N["message"];
            captchaUI.SetActive(true);
        }
        else if (N["code"] == 2)
        {
            errorText.text = N["message"];
            errorUI.SetActive(true);

            ClearField();
            SceneryObj.GetComponent<SceneryDictionary>().StopScenery();
        }

    }

    public void ClearField()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(g);
        }
    }

    // Receipt of POST for the initial validation packet
    public void ValidationReceipt(JSONNode N)
    {
        Debug.Log(N["message"]);

        if(N["code"] == 0)
        {
            ClearField();
            SceneryObj.GetComponent<SceneryDictionary>().StopScenery();

            captchaText.text = N["message"];
            captchaUI.SetActive(true);
        }
        else if (N["code"] == 1)
        {
            sM.StartTimer();
            SceneryObj.GetComponent<SceneryDictionary>().StartScenery();

            StartCoroutine(SendGamePacket(rootURL + packetURL, bananosCollected, bananosMissed, PlayerPrefs.GetString("LocalWallet"), ScoreKeeper.gameVersion, gamepacketGot));
        }
        else if (N["code"] == 2)
        {
            errorText.text = N["message"];
            errorUI.SetActive(true);

            ClearField();
            SceneryObj.GetComponent<SceneryDictionary>().StopScenery();

        }

        connectingUI.SetActive(false);

        
    }

    private void OnApplicationPause(bool pause)
    {
        QuitGame();
    }

    public void OpenCaptcha()
    {
        Application.OpenURL(rootURL + captchaURL + "&wallet=" + PlayerPrefs.GetString("LocalWallet") );
        captchaUI.SetActive(false);

        QuitGame();

    }

}
