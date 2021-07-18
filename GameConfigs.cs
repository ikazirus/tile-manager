using System.Collections.Generic;
using UnityEngine;

public class GameConfigs : MonoBehaviour
{
    public static GameConfigs Instance;
    public List<GameObject> roomObjects;
    public List<Room> rooms;

    void Awake()
    {
        foreach (GameObject go in roomObjects)
        {
            rooms.Add(go.GetComponent<Room>());
        }
        Instance = this;
    }


}
