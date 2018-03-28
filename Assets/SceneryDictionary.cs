using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryDictionary : MonoBehaviour {

    [SerializeField]
    public List<GameObject> SceneryDict = new List<GameObject>();

    
    public void StartScenery()
    {
        
        foreach (ScenerySpawner s in GetComponentsInChildren<ScenerySpawner>())
        {
            s.StartScenery();
        }

        GetComponent<AudioSource>().Play();

    }

}
