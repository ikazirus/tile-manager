using System.Collections.Generic;
using UnityEngine;



public class LevelManager : MonoBehaviour
{
    [SerializeField] private Texture2D map;
    private List<Room> rooms;

    public List<MapCoordinates> selectedCoords = new List<MapCoordinates>();
    public int selectedRoomIndex = 0;


    void Start()
    {
        rooms = GameConfigs.Instance.rooms;
        GenerateLevel();
    }

    public void saveData()
    {
        foreach (MapCoordinates coord in selectedCoords)
        {
            map.SetPixel(coord.x, coord.y, rooms[selectedRoomIndex].baseColor);
        }
        map.Apply();
        selectedCoords.Clear();
        //selectedRoomIndex = 0;
        GenerateLevel();
    }


    private void GenerateLevel()
    {
        if (gameObject.transform.childCount > 0)
            foreach (Transform child in gameObject.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

        int id = 0;
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateBlock(x, y, id);
                id++;
            }
        }
    }

    private void GenerateBlock(int x, int y, int id)
    {
        Color color = map.GetPixel(x, y);
        MapCoordinates mapCoordinates = new MapCoordinates(x, y);

        if (color.a < 1)
        {
            GameObject blockObject = (GameObject)Instantiate(rooms[0].prefab);
            blockObject.transform.SetParent(gameObject.transform);
            BlockManager block = blockObject.GetComponent<BlockManager>();
            block.blockModel.blockID = id;
            block.blockModel.roomType = RoomType.Empty;
            block.blockModel.coordinates = mapCoordinates;
            blockObject.transform.position = new Vector3(x, +3, y);
            return;
        }
        else
        {
            Color colorT = map.GetPixel(x, y - 1);
            Color colorL = map.GetPixel(x - 1, y);
            Color colorR = map.GetPixel(x + 1, y);
            Color colorB = map.GetPixel(x, y + 1);
            foreach (Room room in rooms)
            {
                if (room.baseColor.r == color.r && room.baseColor.g == color.g && room.baseColor.b == color.b)
                {
                    GameObject blockObject = (GameObject)Instantiate(room.prefab);
                    BlockManager block = blockObject.GetComponent<BlockManager>();
                    block.blockModel.blockID = id;
                    block.blockModel.roomType = room.roomType;
                    block.blockModel.coordinates = mapCoordinates;

                    if (color.Equals(colorT) && color.Equals(colorL) && color.Equals(colorR) && color.Equals(colorB))
                    {
                        block.blockModel.rotation = Rotation.R_0;
                        block.blockModel.blockType = BlockType.midFloor;
                    }
                    else
                    {
                        if (colorL.Equals(new Color(0, 0, 0, 1)) && colorT.Equals(new Color(0, 0, 0, 1)))
                            block.blockModel.blockType = BlockType.corner;
                        else if (colorR.Equals(new Color(0, 0, 0, 1)) && colorT.Equals(new Color(0, 0, 0, 1)))
                            block.blockModel.blockType = BlockType.corner;
                        else if (colorL.Equals(new Color(0, 0, 0, 1)) && colorB.Equals(new Color(0, 0, 0, 1)))
                            block.blockModel.blockType = BlockType.corner;
                        else if (colorR.Equals(new Color(0, 0, 0, 1)) && colorB.Equals(new Color(0, 0, 0, 1)))
                            block.blockModel.blockType = BlockType.corner;
                        else
                            block.blockModel.blockType = BlockType.cornerDoor;


                        if (!color.Equals(colorB) && !color.Equals(colorR))
                        {
                            block.blockModel.rotation = Rotation.R_0;
                        }
                        else if (!color.Equals(colorR) && !color.Equals(colorT))
                        {
                            block.blockModel.rotation = Rotation.R_90;
                        }
                        else if (!color.Equals(colorT) && !color.Equals(colorL))
                        {
                            block.blockModel.rotation = Rotation.R_180;
                        }
                        else if (!color.Equals(colorL) && !color.Equals(colorB))
                        {
                            block.blockModel.rotation = Rotation.R_270;
                        }
                        else
                        {

                            if (!color.Equals(colorR))
                            {
                                block.blockModel.rotation = Rotation.R_0;
                            }
                            else if (!color.Equals(colorT))
                            {
                                block.blockModel.rotation = Rotation.R_90;
                            }
                            else if (!color.Equals(colorL))
                            {
                                block.blockModel.rotation = Rotation.R_180;
                            }
                            else if (!color.Equals(colorB))
                            {
                                block.blockModel.rotation = Rotation.R_270;
                            }

                            if (colorL.Equals(new Color(0, 0, 0, 1)) || colorR.Equals(new Color(0, 0, 0, 1)) || colorB.Equals(new Color(0, 0, 0, 1)) || colorT.Equals(new Color(0, 0, 0, 1)))
                                block.blockModel.blockType = BlockType.wall;
                            else
                                block.blockModel.blockType = BlockType.midDoor;
                        }
                    }
                    blockObject.transform.SetParent(gameObject.transform);
                    blockObject.transform.position = new Vector3(x, +3, y);
                }
            }
        }
    }
}
