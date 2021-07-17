using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletSaveManager : MonoBehaviour
{

    [SerializeField]
    GameObject saveLoadCanvas;
    [SerializeField]
    GameObject saveButtonCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveCanvasActive(bool active)
    {
        saveLoadCanvas.SetActive(active);
    }

    public void ForceSaveButtonActive(bool active)
    {

    }
}
