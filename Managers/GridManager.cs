using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows;
    public int columns;
    public GameObject initialBlock;    

    void Start()
    {
        int id = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject obj = (GameObject)Instantiate(initialBlock);
                float x = i * initialBlock.transform.localScale.x;
                float z = j * initialBlock.transform.localScale.z;
                obj.transform.position = new Vector3(x, 0,z);
                obj.transform.SetParent(transform);
                BlockManager blockManager = obj.GetComponent<BlockManager>();
                blockManager.block = new Block(id,Room.Unavailable,BlockType.MidTile,BlockRotation.R_0,x,z);
                id++;
            }
        }
        
    }


    void Update()
    {
        
    }
}
