using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIManager : MonoBehaviour {

    int pendingscore;
    int score;

    [SerializeField]
    Text pendingtext;
    [SerializeField]
    Text scoretext;


	// Use this for initialization
	void Start () {

        pendingscore = 0;
        score = 0;
		
	}

    public void AddPoint()
    {
        pendingscore += 1;
        pendingtext.text = pendingscore.ToString();
    }
	
	
    public void TallyPoints()
    {

        score += pendingscore;
        pendingscore = 0;

        scoretext.text = score.ToString();
        pendingtext.text = pendingscore.ToString();

        PlayerPrefs.SetInt("Bananos", score);

    }

}
