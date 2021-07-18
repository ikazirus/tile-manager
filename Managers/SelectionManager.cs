using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    private Material highlightMaterial;
    [SerializeField]
    private Material emptytMaterial;

    [SerializeField]
    private LevelManager levelManager;


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var selection = hit.transform;
                if (selection.CompareTag("EmptyBlock"))
                {
                    var selectionRenderer = selection.GetComponent<Renderer>();
                    if (selectionRenderer != null)
                    {
                        if (selectionRenderer.material != highlightMaterial)
                            levelManager.selectedCoords.Add(selection.gameObject.GetComponentInParent<BlockManager>().blockModel.coordinates);
                        selectionRenderer.material = highlightMaterial;
                    }
                }
            }
        }
    }

    public void SelectRoom(int index)
    {
        levelManager.selectedRoomIndex = index;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
