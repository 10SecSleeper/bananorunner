using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuValidator : MonoBehaviour {

    [SerializeField]
    int validLength = 64;

    [SerializeField]
    InputField walletInput;

    [SerializeField]
    Button startGame;

    // Use this for initialization
    void Start() {

        walletInput.text = PlayerPrefs.GetString("LocalWallet");
        ValidateWallet();

    }

    // Update is called once per frame
    void FixedUpdate() {

        ValidateWallet();

    }

    void ValidateWallet()
    {

        string walID = PlayerPrefs.GetString("LocalWallet");

        if (walID.Length == validLength)
        {
            startGame.interactable = true;
        }
        else startGame.interactable = false;

    }
}
