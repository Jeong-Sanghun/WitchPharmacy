using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//머쉬베놈 버튼
public class DocumentFolderButtonClass
{
    public int index;
    public string folderName;
    public List<DocumentButtonClass> documentButtonList;
    public Text folderButtonText;
    public GameObject folderCanvasObject;
    public GameObject folderButtonObject;
    public Transform documentButtonScrollParent;

    public DocumentFolderButtonClass(int _index, string folder,GameObject folderCanvasObj, GameObject folderButtonObj)
    {
        index = _index;
        folderName = folder;
        folderCanvasObject = folderCanvasObj;
        folderButtonObject = folderButtonObj;
        
        folderButtonText = folderButtonObj.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        folderButtonText.text = folder;
        documentButtonScrollParent = folderCanvasObject.transform.GetChild(1).GetChild(0).GetChild(0);
        documentButtonList = new List<DocumentButtonClass>();
    }

    public void AddDocument(DocumentButtonClass button)
    {
        documentButtonList.Add(button);
    }

    public void SetupContentRect()
    {
        RectTransform contentRect = documentButtonScrollParent.GetComponent<RectTransform>();
        for(int i = 0; i < documentButtonList.Count; i++)
        {
            RectTransform buttonRect = documentButtonList[i].buttonObject.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector3(0, -120 - 200 * i, 0);
        }

        contentRect.sizeDelta = new Vector2(0, 20 + 220 * documentButtonList.Count);
    }

   
}
