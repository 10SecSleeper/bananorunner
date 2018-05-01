using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryDictionary : MonoBehaviour {

    [SerializeField]
    public List<GameObject> SceneryDict = new List<GameObject>();

    private void Start()
    {

    }

    public void StartScenery()
    {

        if (!PlayerPrefs.HasKey("SceneryDisabled") || PlayerPrefs.GetInt("SceneryDisabled") == 0)
        {
            foreach (ScenerySpawner s in GetComponentsInChildren<ScenerySpawner>())
            {
                s.StartScenery();
            }
        }
        else
        {
            return;
        }

        GetComponent<AudioSource>().Play();

    }

    public void StopScenery()
    {
        foreach (ScenerySpawner s in GetComponentsInChildren<ScenerySpawner>())
        {
            s.StopScenery();
        }

        GetComponent<AudioSource>().Stop();
    }

}
