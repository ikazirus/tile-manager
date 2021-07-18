using UnityEngine;

[System.Serializable]
public class Room
{
    public int id;
    public RoomType roomType;
    public string name;
    public float costPerBlock;
    public Color baseColor;
    public GameObject prefab;
}

public enum RoomType
{
    Unavailable,
    Available,
    Construction,
    Empty,
    Store,
    Armory,
    Corridor,
}