using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using UnityEngine;

public class PlayerViewPort : MonoBehaviour {

    [SerializeField]
    GameObject containerObj;
    [SerializeField]
    GameObject containerPrefab;

    [SerializeField]
    ServerReferee referee;

    [SerializeField]
    BananoNetManager netman;

    bool onlineMode;
    int onlineCount;

    void Awake()
    {
        if (netman.headlessMode)
        {
            InvokeRepeating("CSVExport", 5f, 125f);
        }

        onlineCount = referee.pinterface.container.Count;
        InvokeRepeating("RefreshView", 0, 5f);
    }

    void FixedUpdate()
    {
        if (referee.pinterface.container.Count != onlineCount && onlineMode)
        {
            ShowOnlinePlayers();
            onlineCount = referee.pinterface.container.Count;
        }
    }

    void RefreshView()
    {
        if (onlineMode)
        {
            ShowOnlinePlayers();
        }
        else
        {
            ShowAllPlayers();
        }
    }

    public void ClearViewport()
    {
        if (containerObj.transform.childCount == 0)
            return;

        foreach(Transform child in containerObj.transform.GetComponentsInChildren<Transform>())
        {
            if (child != containerObj.transform)
                Destroy(child.gameObject);
        }
    }

    public void ShowOnlinePlayers()
    {
        onlineMode = true;
        ClearViewport();

        PlayerTracker.PlayerDB db = new PlayerTracker.PlayerDB();

        foreach (PlayerTracker.PlayerContainer c in referee.pinterface.container)
        {
            string tmpwallet = c.GetWallet();

            string[] playerdat = db.GetPlayerData(tmpwallet);

            GameObject newContainer = Instantiate(containerPrefab, containerObj.transform);

            newContainer.GetComponent<PlayerInfoContainer>().SetupContainer(tmpwallet, playerdat[0], playerdat[1], playerdat[2], playerdat[3]);

        }


    }

	public void ShowAllPlayers()
    {
        onlineMode = false;
        ClearViewport();

        PlayerTracker.PlayerDB db = new PlayerTracker.PlayerDB();

        if (PlayerPrefs.HasKey("AllWallets"))
        {
            string[] allwallets = PlayerPrefs.GetString("AllWallets").Split('|');

            foreach (string s in allwallets)
            {

                string[] playerdat = db.GetPlayerData(s);

                GameObject newContainer = Instantiate(containerPrefab, containerObj.transform);

                newContainer.GetComponent<PlayerInfoContainer>().SetupContainer(s, playerdat[0], playerdat[1], playerdat[2], playerdat[3]);

            }

        }

    }

    public void CSVExport()
    {

        List<string[]> rowData = new List<string[]>();

        string[] rowDataTemp = new string[5];
        rowDataTemp[0] = "Wallet ID";
        rowDataTemp[1] = "Network Address";
        rowDataTemp[2] = "Bananos Earned";
        rowDataTemp[3] = "Red Flags";
        rowDataTemp[4] = "Time On Server (Seconds)";
        rowData.Add(rowDataTemp);
        
        PlayerTracker.PlayerDB db = new PlayerTracker.PlayerDB();

        if (PlayerPrefs.HasKey("AllWallets"))
        {
            string[] allwallets = PlayerPrefs.GetString("AllWallets").Split('|');

            foreach(string s in allwallets)
            {

                rowDataTemp = new string[5];
                string[] playerdat = db.GetPlayerData(s);

                rowDataTemp[0] = s;
                rowDataTemp[1] = playerdat[0];
                rowDataTemp[2] = playerdat[1];
                rowDataTemp[3] = playerdat[2];
                rowDataTemp[4] = playerdat[3];
                rowData.Add(rowDataTemp);
            }
        }


        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        if (rowData.Count > 0)
        {

            string filePath = Application.dataPath + "/" + "BRunner_Export.csv";
            StreamWriter outStream = System.IO.File.CreateText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();

        }

    }

    public void ExitServer()
    {
        netman.StopServer();
    }

}
