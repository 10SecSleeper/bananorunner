using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {

    [SerializeField]
    Text Bananos;

    [SerializeField]
    Text RoundBananos;

	// Use this for initialization
	void Start () {
        ScoreKeeper.gameOver = false;
        SetBananos();
	}

    public void SetBananos()
    {
        Bananos.text = PlayerPrefs.GetInt("Bananos").ToString() + " Bananos";
        RoundBananos.text = ScoreKeeper.roundScore.ToString() + " Bananos";
        ScoreKeeper.roundScore = 0;
    }

    public void QuitGame()
    {
        gameObject.SetActive(false);
    }

}
