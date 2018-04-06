using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioToggler : MonoBehaviour {

	// Use this for initialization
	void Start () {

        AudioListener al = GetComponent<AudioListener>();

        if (PlayerPrefs.GetInt("AudioMute") == 1)
        {
            AudioListener.pause = true;
        }
        else
        {
            AudioListener.pause = false;
        }
		
	}

    // Update is called once per frame
    void Update () {
		
	}
}
