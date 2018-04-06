using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDictionary : MonoBehaviour {

    [System.Serializable]
    public class Obstacle
    {

        public GameObject obj;
        public string name;
        public bool impenetrable;
        public bool interactive;
        public bool banano;

    }

    [SerializeField]
    public List<Obstacle> obstacles = new List<Obstacle>();

    public class ObstacleIndex
    {

        public int bananos = 0;
        public string generated = "";

    }

    public ObstacleIndex GenerateObstacleBlock (int blockSize)
    {

        ObstacleIndex index = new ObstacleIndex();

        for (int i = 0; i < blockSize; i++)
        {

            int one = Random.Range(0, obstacles.Count);
            int two = Random.Range(0, obstacles.Count);
            int three = Random.Range(0, obstacles.Count);

            if (obstacles[one].impenetrable && obstacles[two].impenetrable && obstacles[three].impenetrable)
            {
                two = 0;
            }

            if (obstacles[one].banano)
                index.bananos += 1;
            if (obstacles[two].banano)
                index.bananos += 1;
            if (obstacles[three].banano)
                index.bananos += 1;

            string bit = one + "," + two + "," + three;

            index.generated += bit;

            if (i < blockSize - 1)
            {
                index.generated += "|";
            }

        }

        return index;

    }

}
