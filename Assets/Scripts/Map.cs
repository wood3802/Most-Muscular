using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour
{
    public int x, y;
    int[,] map;
    public GameObject wall;
    GameObject[] walls;
    public GameObject wallsObject;
    
    // Start is called before the first frame update
    void Start()
    {
        walls = new GameObject[x * y];
        map = new int[x, y];
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                map[i, j] = 1;
                walls[i * y + j] = Instantiate<GameObject>(wall, new Vector3(i, 0, j), Quaternion.identity);
                walls[i * y + j].transform.parent = wallsObject.transform;
                if(i != 0 && j != 0 && j != map.GetLength(1)-1 && i != map.GetLength(0)-1)
                {
                    walls[i * y + j].SetActive(false);
                    map[i, j] = 0;
                }
            }
        }
    }
}
