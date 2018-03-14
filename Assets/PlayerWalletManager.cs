using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalletManager : MonoBehaviour {

    public void WriteLocalWallet(string wallet)
    {
        PlayerPrefs.SetString("LocalWallet", wallet);

        Debug.Log(PlayerPrefs.GetString("LocalWallet"));
    }

    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
