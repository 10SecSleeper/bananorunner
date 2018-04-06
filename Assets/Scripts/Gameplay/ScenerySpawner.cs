using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenerySpawner : MonoBehaviour {

    public float speed = 600f;
    public SceneryDictionary sd;

    int timer = 0;
    int timerMax = 0;

    int minTime = 1;
    int maxTime = 3;

	// Use this for initialization
	void Start () {

        sd = GetComponentInParent<SceneryDictionary>();

	}

    public void StartScenery()
    {
        InvokeRepeating("SecondUpdate", 1, 1);
        timerMax = Random.Range(minTime, maxTime + 1);
    }
	
    void SecondUpdate()
    {

        timer += 1;

        if (timer >= timerMax)
        {
            GameObject inst = sd.SceneryDict[Random.Range(0, sd.SceneryDict.Count)];
            inst.transform.position = transform.position;

            inst.GetComponent<ObstacleMover>().speed = speed;
            Instantiate(inst);

            timer = 0;
            timerMax = Random.Range(minTime, maxTime + 1);
        }

    }

	// Update is called once per frame
	void Update () {
		
	}
}
