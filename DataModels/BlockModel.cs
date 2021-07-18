using UnityEngine;

[System.Serializable]
public class BlockModel
{
    public int blockID;
    public MapCoordinates coordinates;
    public Rotation rotation;
    public BlockType blockType;
    public RoomType roomType;
    public BlockPrefab prefabs;

    public BlockModel(int blockID, MapCoordinates coordinates, Rotation rotation, BlockType blockType)
    {
        this.blockID = blockID;
        this.coordinates = coordinates;
        this.rotation = rotation;
        this.blockType = blockType;
    }
}

[System.Serializable]
public class BlockPrefab
{
    public GameObject midFloor;
    public GameObject wall;
    public GameObject corner;
    public GameObject singleDoor;
    public GameObject leftDoor;
    public GameObject midDoor;
    public GameObject rightDoor;
    public GameObject cornerDoor;
}

[System.Serializable]
public class MapCoordinates
{
    public int x;
    public int y;

    public MapCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public enum BlockType
{
    midFloor,
    wall, corner,
    singleDoor,
    leftDoor,
    midDoor,
    rightDoor,
    cornerDoor
}

public enum Rotation
{
    R_0,
    R_90,
    R_180,
    R_270
}