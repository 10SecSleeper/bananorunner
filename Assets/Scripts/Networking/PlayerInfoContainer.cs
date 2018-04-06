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
    [SerializeField]
    Text timePlayedField;

	public void SetupContainer(string wallet, string ip, string bananos, string redflags, string timeEllapsed)
    {
        walletField.text = wallet;
        ipField.text = ip;
        bananosField.text = "Bananos: " + bananos;
        redflagsField.text = redflags;
        timePlayedField.text = "Time: " + ReturnTime(timeEllapsed);
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
}
