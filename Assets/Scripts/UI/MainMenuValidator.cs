using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using EpochTime;
using SimpleJSON;
using TMPro;

public class MainMenuValidator : MonoBehaviour {

    [SerializeField]
    TMP_InputField walletInputField;

    [SerializeField]
    Button startGameButton;

    [SerializeField]
    GameObject disclaimerUI;
    [SerializeField]
    Text disclaimerText;

    [SerializeField]
    GameObject creditsUI;

    [SerializeField]
    GameObject gameoverUI;

    [SerializeField]
    public string jsonURL = "";

    [SerializeField]
    TextMeshProUGUI muteButtonText;

    [SerializeField]
    TextMeshProUGUI shadowsText;

    [SerializeField]
    GameObject ServerButton;

    [SerializeField]
    MainMenuRoot rootMM;

    [SerializeField]
    TextMeshProUGUI eventCountdown;
    [SerializeField]
    TextMeshProUGUI eventLabel;

    [SerializeField]
    TextMeshProUGUI limitFPSText;

    [SerializeField]
    GameObject settingsUI;

    int currentTime = 0;
    int secondsToEvent = 0;
    int secondsToEnd = 0;
    int eventEndTime = 0;
    int eventTime = 0;

    bool gotServerData = false;

    System.Action<List<int>, List<string>> gotServerJson;

    private void Update()
    {
        // Shortcut for server
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.P))
        {
            ServerButton.SetActive(true);
        }

    }

    // Timer that helps with menu countdown to events
    void Timer()
    {
        secondsToEvent -= 1;
        secondsToEnd -= 1;

        if (gotServerData && secondsToEnd >= 0)
        {

            if (secondsToEvent > 0)
            {
                eventLabel.text = "Event starts in: ";
                eventCountdown.text = ReturnTime(Mathf.FloorToInt(secondsToEvent).ToString());
            }
            else if (secondsToEvent < 0 && secondsToEnd > 0)
            {
                eventLabel.text = "Event ends in: ";
                eventCountdown.text = ReturnTime(Mathf.FloorToInt(secondsToEnd).ToString());
            }
        }
        else
        {
            eventLabel.text = "Event not ready: ";
            eventCountdown.text = "-- -- --";
        }
    }

    // Use this for initialization
    void Start() {

        // Add our action callback to the action for our JSON server message
        gotServerJson += RetrievedServerMessages;

        // Nullify timer stuff

        // Start the coroutine to grab server data
        if (!ScoreKeeper.gameOver)
            StartCoroutine(GrabServerMessage(jsonURL, gotServerJson));

        // Set the player pref game version
        PlayerPrefs.SetString("GameVersion", rootMM.currentGameVersion);
        PlayerPrefs.Save();

        // Load the wallet if it exists

        if (PlayerPrefs.HasKey("LocalWallet"))
        {
            walletInputField.text = PlayerPrefs.GetString("LocalWallet");
            SetWallet(PlayerPrefs.GetString("LocalWallet"));
        }
        

        // If the player hasn't seem the disclaimer, open it AND clear all local data
        if (!PlayerPrefs.HasKey("SeenDisclaimer" + rootMM.currentGameVersion))
        {
            PlayerPrefs.DeleteKey("Bananos");
            ShowDisclaimer();
        }

        InitGFXSettings();
        InitAudioSetting();

        if (ScoreKeeper.gameOver)
        {
            gameoverUI.SetActive(true);
        }

    }

    void InitAudioSetting()
    {
        if (!PlayerPrefs.HasKey("AudioMute") || PlayerPrefs.GetInt("AudioMute") == 0)
        {
            muteButtonText.text = "Sound is ON";
        }
        else
        {
            muteButtonText.text = "Sound is OFF";
        }
    }

    public void ChangeAudioSetting()
    {
        if (!PlayerPrefs.HasKey("AudioMute") || PlayerPrefs.GetInt("AudioMute") == 0)
        {
            PlayerPrefs.SetInt("AudioMute", 1);
            muteButtonText.text = "Sound is OFF";
        }
        else
        {
            PlayerPrefs.SetInt("AudioMute", 0);
            muteButtonText.text = "Sound is ON";
        }
        PlayerPrefs.Save();
    }

    // Callback for after server data loads in
    void RetrievedServerMessages(List<int> ints, List<string> strings)
    {
        currentTime = Epoch.Current();
        secondsToEvent = ints[0] - currentTime;
        secondsToEnd = ints[1] - currentTime;

        eventTime = ints[0];
        eventEndTime = ints[1];
        disclaimerText.text = strings[0];

        if (ScoreKeeper.gameVersion != strings[2])
        {
            disclaimerText.text = "Your game client is out of date!\n\nPlease update your client!";
            ShowDisclaimer();
        }

        if (!PlayerPrefs.HasKey("ClientAddress"))
        {
            PlayerPrefs.SetString("ClientAddress", strings[1]);
            PlayerPrefs.SetInt("ClientPort", ints[2]);
            PlayerPrefs.Save();
        }

        gotServerData = true;

        // Start our countdown timer logic
        InvokeRepeating("Timer", 0, 1f);
    }

    // A coroutine which loads data from the json url set in vars
    public static IEnumerator GrabServerMessage(string url, System.Action<List<int>, List<string>> action)
    {
        WWW www = new WWW(url);
        yield return www;

        if (www.isDone)
        {
            if (string.IsNullOrEmpty(www.error))
            {
                var N = JSONNode.Parse(www.text);

                List<int> ints = new List<int>();
                List<string> strings = new List<string>();

                // Server message is strings[0]
                string msg = N["brunner"]["message"];
                strings.Add(msg);

                int timeToEvent;
                int timeToEnd;

                string timeToStartString = N["brunner"]["timer"]["start"];
                string timeToEndString = N["brunner"]["timer"]["end"];

                int.TryParse(timeToStartString, out timeToEvent);
                int.TryParse(timeToEndString, out timeToEnd);

                // Time to event is ints[0]
                ints.Add(timeToEvent);

                // Time to event END is ints[1]
                ints.Add(timeToEnd);

                int addressRand = Random.Range(0, N["brunner"]["servers"].Count);

                string address = N["brunner"]["servers"][addressRand]["address"];

                // Network address is strings[1]
                strings.Add(address);

                int port;
                string portString = N["brunner"]["servers"][addressRand]["port"];

                int.TryParse(portString, out port);

                // Network port is ints[2]
                ints.Add(port);

                string version = N["brunner"]["version"];

                // Game version is strings[2]
                strings.Add(version);

                action(ints, strings);
            }
        }
    }

    // What the game should do when the player agrees to the disclaimer
    public void AgreeDisclaimer()
    {
        disclaimerUI.SetActive(false);
    }

    // Called by a button to show the disclaimer
    public void ShowDisclaimer()
    {
        PlayerPrefs.SetInt("SeenDisclaimer" + rootMM.currentGameVersion, 1);
        PlayerPrefs.Save();
        disclaimerUI.SetActive(true);
    }


    void InitGFXSettings()
    {
        if (!PlayerPrefs.HasKey("LimitFPS"))
        {
            PlayerPrefs.SetInt("LimitFPS", 60);
            PlayerPrefs.Save();
            Application.targetFrameRate = 300;
            limitFPSText.text = "Limit FPS ON";
            return;
        }

        if (PlayerPrefs.HasKey("LimitFPS"))
        {
            Application.targetFrameRate = PlayerPrefs.GetInt("LimitFPS");

            if (PlayerPrefs.GetInt("LimitFPS") != -1)
                limitFPSText.text = "Limit FPS ON";
            else
                limitFPSText.text = "Limit FPS OFF";
        }
            
        if (!PlayerPrefs.HasKey("ShadowsDisabled") || PlayerPrefs.GetInt("ShadowsDisabled") == 0)
        {
            shadowsText.text = "Shadows ON";
            QualitySettings.shadows = ShadowQuality.All;
        }
        else
        {
            shadowsText.text = "Shadows OFF";
            QualitySettings.shadows = ShadowQuality.Disable;
        }

    }

    public void ShadowsSettingChange()
    {
        if (!PlayerPrefs.HasKey("ShadowsDisabled") || PlayerPrefs.GetInt("ShadowsDisabled") == 0)
        {
            shadowsText.text = "Shadows OFF";
            PlayerPrefs.SetInt("ShadowsDisabled", 1);
            QualitySettings.shadows = ShadowQuality.Disable;
        }
        else
        {
            shadowsText.text = "Shadows ON";
            PlayerPrefs.SetInt("ShadowsDisabled", 0);
            QualitySettings.shadows = ShadowQuality.All;
        }
        PlayerPrefs.Save();
    }

    public void LimitFPSSetting()
    {
        if (!PlayerPrefs.HasKey("LimitFPS"))
        {
            PlayerPrefs.SetInt("LimitFPS", 60);
            PlayerPrefs.Save();
            Application.targetFrameRate = 300;
            limitFPSText.text = "Limit FPS ON";
            return;
        }
        else if (PlayerPrefs.GetInt("LimitFPS") != -1)
        {
            PlayerPrefs.SetInt("LimitFPS", -1);
            PlayerPrefs.Save();
            Application.targetFrameRate = -1;
            limitFPSText.text = "Limit FPS OFF";
            return;
        }
        else
        {
            PlayerPrefs.SetInt("LimitFPS", 60);
            PlayerPrefs.Save();
            Application.targetFrameRate = 60;
            limitFPSText.text = "Limit FPS ON";
            return;
        }
    }

    public void SetWallet(string s)
    {
        string walID = s;

        Regex rgx = new Regex("(ban)(_)([13]{1})([13456789abcdefghijkmnopqrstuwxyz]{59})");

        if (rgx.IsMatch(walID) && walID.Length == 64)
        {
            PlayerPrefs.SetString("LocalWallet", s.ToLower());
            PlayerPrefs.Save();
            startGameButton.interactable = true;
        }
        else startGameButton.interactable = false;
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

    public void OpenCredits()
    {
        creditsUI.SetActive(true);
    }

    public void OpenSettings()
    {
        settingsUI.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsUI.SetActive(false);
    }

    public void ClearNetwork()
    {
        PlayerPrefs.DeleteKey("ClientAddress");
        PlayerPrefs.DeleteKey("ClientPort");
        PlayerPrefs.Save();

        // Start the coroutine to grab server data
        StartCoroutine(GrabServerMessage(jsonURL, gotServerJson));
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    // Method for getting the time (in seconds) converted to hhmmss
    string ReturnTime(string seconds)
    {
        if (seconds == "0")
        {
            return "0S";
        }

        int time = 0;
        int.TryParse(seconds, out time);

        string temp = "";

        int hours = 0;
        int minutes = 0;
        int secs = 0;

        secs = time;
        minutes = Mathf.FloorToInt(time / 60);
        hours = Mathf.FloorToInt(minutes / 60);

        if (minutes >= 1)
        {
            // Remove the total rounded minutes from total seconds
            secs -= minutes * 60;
        }

        if (hours >= 1)
        {
            // Remove total rounded hours from total minutes;
            minutes -= hours * 60;
        }

        temp = (hours + "H " + minutes + "M " + secs + "S");
        return temp;
    }
}
