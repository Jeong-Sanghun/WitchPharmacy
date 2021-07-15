using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//머쉬베놈 버튼
public class DocumentFolderButtonClass
{
    public int index;
    public string folderName;
    public List<DocumentButtonClass> documentButtonList;
    public GameObject folderCanvasObject;
    public GameObject folderButtonObject;

    public DocumentFolderButtonClass(int _index, string folder,GameObject folderCanvasObj, GameObject folderButtonObj)
    {
        index = _index;
        folderName = folder;
        folderCanvasObject = folderCanvasObj;
        folderButtonObject = folderButtonObj;
        documentButtonList = new List<DocumentButtonClass>();
    }

    public void AddDocument(DocumentButtonClass button)
    {
        documentButtonList.Add(button);
    }

   
}
