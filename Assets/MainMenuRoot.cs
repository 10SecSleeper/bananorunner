using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuRoot : MonoBehaviour {

    [SerializeField]
    public string currentGameVersion = "3.0";

    [SerializeField]
    TextMeshProUGUI versionNum;

    public void StartClient()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    private void Start()
    {
        ScoreKeeper.gameVersion = currentGameVersion;
        versionNum.text = currentGameVersion;
    }

}
