using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStringAdapter : MonoBehaviour {

    string newLevelBlock;

    string[] obstacleLineBuffer;

    string[] lineDecoder = new string[3];

    int counter = 0;
    int counterMax = 0;

    [SerializeField]
    ObstacleDictionary odict;

    [SerializeField]
    GameObject checkmarkPrefab;

    int blocksize;
    float sepTime;
    float speed;

    public void ReceiveNewBlock(string block, int blocksize, float separationTime, float speed)
    {
        this.blocksize = blocksize;
        this.sepTime = separationTime;
        this.speed = speed;

        newLevelBlock = block;

        Debug.Log(newLevelBlock);

        counterMax = blocksize;

        obstacleLineBuffer = new string[blocksize];
        obstacleLineBuffer = newLevelBlock.Split('|');

        counter = 0;
        Debug.Log(separationTime);
        InvokeRepeating("SpawnObstacleLine", 0f, separationTime);
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnObstacleLine() {

        if (counter == counterMax)
        {
            CancelInvoke("SpawnObstacleLine");

            GameObject spawnMe = checkmarkPrefab;
            spawnMe.GetComponent<ObstacleMover>().speed = speed;
            spawnMe.transform.position = transform.position;
            Instantiate(spawnMe);
            

            return;
        }
        lineDecoder = obstacleLineBuffer[counter].Split(',');

        for (int x = 0; x < 3; x++)
        {
            string c = lineDecoder[x];
            GameObject toSpawn = null;
            float xPos = 0;
            int i;

            int.TryParse(c, out i);

            if (odict.obstacles[i] != null)
                toSpawn = odict.obstacles[i].obj;

            switch (x)
            {
                case 0:
                    xPos = -2.5f;
                    break;
                case 1:
                    xPos = 0;
                    break;
                case 2:
                    xPos = 2.5f;
                    break;
                default:
                    break;
            }

            if (toSpawn != null)
            {
                toSpawn.transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
                if (toSpawn.GetComponent<ObstacleMover>() != null)
                    toSpawn.GetComponent<ObstacleMover>().speed = speed;
                Instantiate(toSpawn);
            }

        }

        counter += 1;

    }

}
