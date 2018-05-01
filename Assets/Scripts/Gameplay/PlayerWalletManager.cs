using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalletManager : MonoBehaviour {

    public void WriteLocalWallet(string wallet)
    {
        PlayerPrefs.SetString("LocalWallet", wallet.ToLower());
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
