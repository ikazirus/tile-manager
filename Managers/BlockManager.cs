using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public BlockModel blockModel;
    void Update()
    {
        BuildBlock();
    }

    private void BuildBlock()
    {
        if (blockModel.roomType != RoomType.Empty)
        {
            switch (blockModel.blockType)
            {
                case BlockType.midFloor:
                    break;
                case BlockType.wall:
                    blockModel.prefabs.wall.SetActive(true);
                    break;
                case BlockType.singleDoor:
                    blockModel.prefabs.singleDoor.SetActive(true);
                    break;
                case BlockType.leftDoor:
                    blockModel.prefabs.wall.SetActive(true);
                    break;
                case BlockType.midDoor:
                    blockModel.prefabs.midDoor.SetActive(true);
                    break;
                case BlockType.rightDoor:
                    blockModel.prefabs.rightDoor.SetActive(true);
                    break;
                case BlockType.corner:
                    blockModel.prefabs.corner.SetActive(true);
                    break;
                case BlockType.cornerDoor:
                    blockModel.prefabs.cornerDoor.SetActive(true);
                    break;
                default:
                    break;
            }

            switch (blockModel.rotation)
            {
                case Rotation.R_0:
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case Rotation.R_90:
                    gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case Rotation.R_180:
                    gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case Rotation.R_270:
                    gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
                default:
                    break;
            }
        }
    }

    private void reset()
    {
        blockModel.prefabs.wall.SetActive(false);
        blockModel.prefabs.corner.SetActive(false);
        blockModel.prefabs.singleDoor.SetActive(false);
        blockModel.prefabs.leftDoor.SetActive(false);
        blockModel.prefabs.rightDoor.SetActive(false);
        blockModel.prefabs.midDoor.SetActive(false);
        blockModel.prefabs.cornerDoor.SetActive(false);
    }

}
