using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
        public int missableBananos = 0;

    }

    public ObstacleIndex GenerateObstacleBlock (int blockSize)
    {

        ObstacleIndex index = new ObstacleIndex();

        for (int i = 0; i < blockSize; i++)
        {
            int thisBlockBananos = 0;

            int one = Random.Range(0, obstacles.Count);
            int two = Random.Range(0, obstacles.Count);
            int three = Random.Range(0, obstacles.Count);

            if (obstacles[one].impenetrable && obstacles[two].impenetrable && obstacles[three].impenetrable)
            {
                two = 0;
            }

            if (obstacles[one].banano)
            {
                index.bananos += 1;
                thisBlockBananos++;
            }
                
            if (obstacles[two].banano)
            {
                index.bananos += 1;
                thisBlockBananos++;
            }
            if (obstacles[three].banano)
            {
                thisBlockBananos++;
                index.bananos += 1;

            }

            switch (thisBlockBananos)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    index.missableBananos += 1;
                    break;
                case 3:
                    index.missableBananos += 2;
                    break;
                default:
                    break;
            }

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
