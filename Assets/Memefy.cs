using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class Memefy : MonoBehaviour {

    AudioSource aud;

	// Use this for initialization
	void Start () {

        aud = GetComponent<AudioSource>();

        GameObject[] memes = GameObject.FindGameObjectsWithTag("Meme");

        bool play = true;

        foreach (GameObject joke in memes)
        {
            AudioSource audt = joke.GetComponent<AudioSource>();

            if (audt.isPlaying == true && joke != this.gameObject)
            {
                play = false;
            }

        }

        if (play)
            aud.Play();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
