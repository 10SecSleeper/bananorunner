using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryDictionary : MonoBehaviour {

    [SerializeField]
    public List<GameObject> SceneryDict = new List<GameObject>();

    [SerializeField]
    GameObject inGameUI;

    
    public void StartScenery()
    {

        inGameUI.SetActive(true);
        
        foreach (ScenerySpawner s in GetComponentsInChildren<ScenerySpawner>())
        {
            s.StartScenery();
        }

        GetComponent<AudioSource>().Play();

    }

}
