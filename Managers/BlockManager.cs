using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public Block block;
    public GameObject initialPrefab;
    public GameObject selectPrefab;
    public BlockPrefabs[] blockPrefabs;

    void Start()
    {
        

    }

    void Update()
    {
        AssignPrefab();
        ChangePrefab();
    }

    public void ChangePrefab()
    {
        switch (block.rotation)
        {
            case BlockRotation.R_0:
                //gameObject.transform.localRotation =  Quaternion.Euler(0, 0, 0);
                break;
            case BlockRotation.R_90:
                gameObject.transform.localRotation =  Quaternion.Euler(0, 90, 0);
                break;
            case BlockRotation.R_180:
                gameObject.transform.localRotation =  Quaternion.Euler(0, 180, 0);
                break;
            case BlockRotation.R_270:
                gameObject.transform.localRotation =  Quaternion.Euler(0, 270, 0);
                break;
            default:
                break;
        }

        gameObject.transform.SetParent(transform);
    }

    public void AssignPrefab()
    {
        switch (block.room)
        {
            case Room.Unavailable:

                break;
            case Room.Available:
                break;
            case Room.Construction:
                break;
            case Room.Empty:
                break;
            case Room.Store:
                break;
            case Room.Armory:
                break;
            case Room.Corridor:
                break;
            default:
                break;
        }
    }

    public void CheckAdjacents()
    {

    }

}

