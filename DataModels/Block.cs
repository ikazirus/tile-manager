using UnityEngine;

[System.Serializable]
public class Block 
{
    public int id;
    public Room room;
    public BlockType type;
    public BlockRotation rotation;
    public float positionX;
    public float positionZ;

    public Block(int id,float positionX, float positionZ)
    {
        this.id = id;
        this.room = Room.Unavailable;
        this.type = BlockType.MidTile;
        this.rotation = BlockRotation.R_0;
        this.positionX = positionX;
        this.positionZ = positionZ;
    }

    public Block(int id, Room room, BlockType type, BlockRotation rotation, float positionX,float positionZ)
    {
        this.id = id;
        this.room = room;
        this.type = type;
        this.rotation = rotation;
        this.positionX = positionX;
        this.positionZ = positionZ;
    }
}

[System.Serializable]
public class BlockPrefabs
{
    public Room room;
    public GameObject mid;
    public GameObject wall;
    public GameObject corner;
    public GameObject singleDoor;
    public GameObject leftDoor;
    public GameObject midDoor;
    public GameObject rightDoor;
}
