using UnityEngine;

public class SelectedComponent : MonoBehaviour
{

    void Start()
    {
        GameObject obj = transform.parent.gameObject;
    }

    private void OnDestroy()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }
}
