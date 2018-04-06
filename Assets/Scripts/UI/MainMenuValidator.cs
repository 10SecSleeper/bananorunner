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

    [SerializeField]
    GameObject disclaimerUI;

    [SerializeField]
    Text muteButton;

    [SerializeField]
    GameObject ServerButton;
    [SerializeField]
    GameObject ClearDataButton;


    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.P))
        {
            ServerButton.SetActive(true);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L))
        {
            ClearDataButton.SetActive(true);
        }
    }


    // Use this for initialization
    void Start() {

        walletInput.text = PlayerPrefs.GetString("LocalWallet");
        ValidateWallet();

        if (!PlayerPrefs.HasKey("AudioMute"))
        {
            PlayerPrefs.SetInt("AudioMute", 0);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.GetInt("AudioMute") == 1)
        {
            muteButton.text = "Sound Off";
        }
        else
        {
            muteButton.text = "Sound On";
        }

        if (!PlayerPrefs.HasKey("SeenDisclaimer1.1"))
        {
            disclaimerUI.SetActive(true);
        }

    }

    private void OnEnable()
    {

        if (PlayerPrefs.GetInt("AudioMute") == 1)
        {
            muteButton.text = "Sound Off";
        }
        else
        {
            muteButton.text = "Sound On";
        }

    }

    public void AgreeDisclaimer()
    {
        PlayerPrefs.SetInt("SeenDisclaimer1.1", 1);
        PlayerPrefs.Save();
        disclaimerUI.SetActive(false);
    }

    public void MuteSetting()
    {
        //Debug.Log("Changing Audio");

        if (PlayerPrefs.GetInt("AudioMute") == 0)
        {
            PlayerPrefs.SetInt("AudioMute", 1);
            muteButton.text = "Sound Off";
        }
        else
        {
            muteButton.text = "Sound On";
            PlayerPrefs.SetInt("AudioMute", 0);
        }

        PlayerPrefs.Save();

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

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenDiscord()
    {
        Application.OpenURL("https://discord.gg/f7nNeTv");
    }

    public void OpenReddit()
    {
        Application.OpenURL("https://www.reddit.com/r/bananocoin");
    }

    public void OpenVault()
    {
        Application.OpenURL("https://vault.banano.co.in");
    }
}
