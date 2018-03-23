using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemeContainer : MonoBehaviour {

	// Use this for initialization
	void Start () {

        GameObject spawner = GameObject.FindGameObjectWithTag("ObjectSpawner");
        MemeDictionary mdict = spawner.GetComponent<MemeDictionary>();
        float speed = GetComponent<ObstacleMover>().speed;

        GameObject toSpawn = mdict.memeDict[Random.Range(0, mdict.memeDict.Count)];

        if (toSpawn != null)
        {
            toSpawn.GetComponent<ObstacleMover>().speed = speed;
            toSpawn.transform.position = transform.position;
            Instantiate(toSpawn);
        }

        Destroy(this.gameObject);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
