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
    [SerializeField]
    Text timetext;

    int time = 0;

	// Use this for initialization
	void Start () {

        pendingscore = 0;
        score = 0;
        InvokeRepeating("Count", 0f, 1f);
	}

    void Count()
    {
        time++;
        timetext.text = ReturnTime(time.ToString());
    }

    string ReturnTime(string seconds)
    {
        if (seconds == "0")
        {
            return "0 Secs";
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

        temp = (hours + " Hours, " + minutes + " Mins, " + secs + " Secs");
        return temp;

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
        PlayerPrefs.Save();

    }

    public void ExitGame()
    {
        GameObject.FindGameObjectWithTag("NetMan").GetComponent<BananoNetManager>().StopClient();
    }

}
