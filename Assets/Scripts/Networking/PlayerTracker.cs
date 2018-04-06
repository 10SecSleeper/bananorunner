using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerTracker : NetworkBehaviour {

    public class PlayerDB
    {

        public void SavePlayerData(string wallet, string ip, string bananosEarned, string redflags, string secondsPlayed)
        {
            string toStore = ip + "|" + bananosEarned + "|" + redflags + "|" + secondsPlayed;

            Debug.Log("Has key? " + PlayerPrefs.HasKey(wallet));

            if (!PlayerPrefs.HasKey(wallet))
            {
                Debug.Log("We do not have this key yet. We must add it " + wallet);

                string tmpWalletStore = "";

                if (!PlayerPrefs.HasKey("AllWallets"))
                {
                    tmpWalletStore = wallet;
                    PlayerPrefs.SetString("AllWallets", tmpWalletStore);
                    PlayerPrefs.Save();
                }
                else if (PlayerPrefs.GetString("AllWallets") == wallet)
                {

                }
                else
                {
                    tmpWalletStore = PlayerPrefs.GetString("AllWallets") + "|" + wallet;
                    PlayerPrefs.SetString("AllWallets", tmpWalletStore);
                    PlayerPrefs.Save();
                } 
            }


            PlayerPrefs.SetString(wallet, toStore);

            
            
        }

        public void RemovePlayerData(string wallet)
        {

            string[] wallets = PlayerPrefs.GetString("AllWallets").Split('|');
            string tmpwallet = "";

            for(int i = 0; i < wallets.Length; i++)
            {

                if (wallets[i] != wallet)
                {
                    tmpwallet += wallets[i];
                }
                else
                {
                    Debug.Log("Removing player with wallet " + wallet);
                }

                if (i > 0 && i != wallets.Length)
                {
                    tmpwallet += "|";
                }

            }

            PlayerPrefs.SetString("AllWallets", tmpwallet);
            PlayerPrefs.DeleteKey(wallet);
            PlayerPrefs.Save();

        }

        public string[] GetPlayerData(string wallet)
        {

            string[] tempString = new string[4] { "NULL", "0", "0", "0" };

            if (PlayerPrefs.HasKey(wallet))
            {
                tempString = PlayerPrefs.GetString(wallet).Split('|');
            }

            return tempString;

        }

        public void AddFlag(string wallet)
        {
            string[] tempString = GetPlayerData(wallet);
            int flags = 0;
            int.TryParse(tempString[2], out flags);

            flags += 1;

            SavePlayerData(wallet, tempString[0], tempString[1], flags.ToString(), tempString[3]);
        }

        public void AddBananos(string wallet, int bananos)
        {
            string[] tempString = GetPlayerData(wallet);

            int bans = 0;
            int.TryParse(tempString[1], out bans);

            bans += bananos;

            SavePlayerData(wallet, tempString[0], bans.ToString(), tempString[2], tempString[3]);

        }

        public void AddTime(string wallet, int timeToAdd)
        {
            string[] tempString = GetPlayerData(wallet);

            int time = 0;
            int.TryParse(tempString[3], out time);

            time += timeToAdd;

            SavePlayerData(wallet, tempString[0], tempString[1], tempString[2], time.ToString());

        }

    }

    [System.Serializable]
    public class PlayerContainer
    {

        private NetworkConnection conn;
        private GameObject playerObj;

        public int timeInterval;

        private string networkip;
        private string walletaddress;

        private int bananosGiven;
        private int bananosCollected;

        public void SetupPlayerInfo(NetworkConnection conn, GameObject playerObj, int curTime, string ip, string wallet)
        {

            this.conn = conn;
            this.playerObj = playerObj;
            this.timeInterval = curTime;
            this.networkip = ip;
            this.walletaddress = wallet;
        }

        public void SubmitBananos(int b)
        {
            this.bananosCollected = b;
        }

        public void SetGivenBananos(int b)
        {
            this.bananosGiven = b;
        }

        public int GetBananosGiven()
        {
            return bananosGiven;
        }

        public NetworkConnection GetConn()
        {
            return this.conn;
        }

        public GameObject GetObj()
        {
            return this.playerObj;
        }

        public string GetIp()
        {
            return this.networkip;
        }

        public string GetWallet()
        {
            return this.walletaddress;
        }

    }

    [System.Serializable]
    public class PlayerListInterface
    {

        public List<PlayerContainer> container = new List<PlayerContainer>();

        public void AddPlayer(NetworkConnection conn, GameObject playerObj, int curTime, string ip, string wallet)
        {

            PlayerContainer tempP = new PlayerContainer();
            PlayerDB db = new PlayerDB();

            tempP.SetupPlayerInfo(conn, playerObj, curTime, ip, wallet);

            container.Add(tempP);

            if (!PlayerPrefs.HasKey(wallet))
            {
                db.SavePlayerData(wallet, ip, "0", "0", "0");
                Debug.Log("New player added from " + ip + ". Their wallet is " + wallet);
            }
            else
            {
                Debug.Log("Player joined. Already in database at " + ip);
            }

        }

        public void RemovePlayer(NetworkConnection conn)
        {

            for (int i = 0; i < container.Count; i++)
            {

                if (conn == container[i].GetConn())
                {
                    container.Remove(container[i]);
                    return;
                }

            }

        }

        public PlayerContainer GetPlayerContainer(NetworkConnection conn)
        {
            foreach (PlayerContainer p in container)
            {
                if (p.GetConn() == conn)
                {
                    return p;
                }
            }

            return null;
        }

        public void SubmitBananos(NetworkConnection conn, int bananos)
        {

            foreach (var p in container)
            {
                if (conn == p.GetConn())
                {
                    p.SubmitBananos(bananos);
                }
            }

        }

        public Dictionary<NetworkConnection, GameObject> PlayersOnThisInterval(int i)
        {

            Dictionary<NetworkConnection, GameObject> dict = new Dictionary<NetworkConnection, GameObject>();

            foreach (var p in container)
            {

                if (p.timeInterval == i)
                {
                    dict.Add(p.GetConn(), p.GetObj());
                }

            }

            return dict;

        }

    }


}
