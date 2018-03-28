using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoContainer : MonoBehaviour {

    [SerializeField]
    InputField walletField;
    [SerializeField]
    Text ipField;
    [SerializeField]
    Text bananosField;
    [SerializeField]
    Text redflagsField;

	public void SetupContainer(string wallet, string ip, string bananos, string redflags)
    {
        walletField.text = wallet;
        ipField.text = ip;
        bananosField.text = bananos;
        redflagsField.text = redflags;
    }
}
