using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LevelStringAdapter : NetworkBehaviour {

    string newLevelBlock;

    string[] obstacleLineBuffer;

    string[] lineDecoder = new string[3];

    int counter = 0;
    int counterMax = 0;

    [SerializeField]
    ObstacleDictionary odict;

    [SerializeField]
    GameObject checkmarkPrefab;

    [SerializeField]
    GameObject invisPrefab;

    int blocksize;
    float sepTime;
    float delayTime;
    float speed;

    int junkcounter = 0;
    int junkcounterMax = 0;

    List<BlockChain> bchain = new List<BlockChain>();

    public class BlockChain
    {
        string text;
        int size;
        float totalTime;
        float delayTime;
        float speed;

        public BlockChain(string b, int size, float totalTime, float delay, float speed)
        {
            this.text = b;
            this.size = size;
            this.totalTime = totalTime;
            this.delayTime = delay;
            this.speed = speed;
        }

    }

    public void ReceiveNewBlock(string block, int blocksize, float totalTime, float delayTime, float speed)
    {
        //bchain.Add(new BlockChain(block, blocksize, totalTime, delayTime, speed));

        this.blocksize = blocksize;
        this.delayTime = delayTime;
        this.sepTime = (totalTime - delayTime) / blocksize;
        this.speed = speed;
        this.junkcounterMax = Mathf.FloorToInt(sepTime * (delayTime-1) );
        this.junkcounter = junkcounterMax;

        newLevelBlock = block;

        //Debug.Log(newLevelBlock);

        counterMax = blocksize;

        obstacleLineBuffer = new string[blocksize];
        obstacleLineBuffer = newLevelBlock.Split('|');

        counter = 0;
        InvokeRepeating("SpawnObstacleLine", 0f, sepTime);
    }

	// Use this for initialization
	void Start () {

        NetworkCRC.scriptCRCCheck = true;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnObstacleLine() {

        if (counter == counterMax)
        {

            CancelInvoke("SpawnObstacleLine");

            InvokeRepeating("SpawnJunkLine", 0f, sepTime);

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

            if (toSpawn == null && i != 0)
            {
                //Debug.Log("Mismatch");
                Application.Quit();
            }

        }

        counter += 1;
    }

    void SpawnJunkLine()
    {
        if (junkcounter == 0)
        {
            CancelInvoke("SpawnJunkLine");
            
            GameObject spawnMe = checkmarkPrefab;
            spawnMe.GetComponent<ObstacleMover>().speed = speed;
            spawnMe.transform.position = transform.position;
            Instantiate(spawnMe);
            // Spawn the referee line here.
            return;
        }
        else
        {
            if (junkcounter == Mathf.FloorToInt(junkcounterMax/2))
            {
                GameObject spawnMe = invisPrefab;
                spawnMe.GetComponent<ObstacleMover>().speed = speed;
                spawnMe.transform.position = transform.position;
                Instantiate(spawnMe);
            }

            //Debug.Log("Spawning Junk Line");

            int loseImp = Random.Range(0, 3);

            for (int i = 0; i < 3; i++)
            {
                // Get a random int
                int p = Random.Range(0, odict.obstacles.Count);

                // If it's a banano, set the object to be air instead
                if (odict.obstacles[p].banano)
                {
                    p = 0;
                }

                if (i == loseImp && odict.obstacles[p].impenetrable)
                {
                    p = 0;
                }

                // Generate the item
                if (p != 0)
                {
                    GameObject tempSpawn = Instantiate(odict.obstacles[p].obj);
                    ObstacleMover om = tempSpawn.GetComponent<ObstacleMover>();
                    
                    om.speed = speed;
                    float tmpX = 0;

                    switch (i)
                    {
                        case 0:
                            tmpX = -2.5f;
                            break;
                        case 1:
                            break;
                        case 2:
                            tmpX = 2.5f;
                            break;
                        default:
                            break;
                    }

                    tempSpawn.tag = "Obstacle";
                    tempSpawn.transform.position = transform.position;
                    tempSpawn.transform.position = new Vector3(tmpX, transform.position.y, transform.position.z);
                }
            }

            junkcounter--;
        }

        
    }

}
