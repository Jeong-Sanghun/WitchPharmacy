using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletDocumentManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    List<OwningDocumentClass> owningDocumentList;
    List<DocumentBundle> documentBundleList;
    List<DocumentCondition> documentConditionList;
    List<DocumentFolderButtonClass> wholeFolderButtonList;
    int nowFolderButtonIndex;

    [SerializeField]
    GameObject wholeFolderParentObject;
    [SerializeField]
    Transform folderScrollParent;
    [SerializeField]
    GameObject folderButtonPrefab;
    [SerializeField]
    GameObject folderCanvasPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        owningDocumentList = saveData.owningDocumentList;
        documentConditionList = gameManager.documentConditionWrapper.documentConditionList;
        documentBundleList = new List<DocumentBundle>();
        wholeFolderButtonList = new List<DocumentFolderButtonClass>();
        nowFolderButtonIndex = 0;
        for(int i = 0; i < owningDocumentList.Count; i++)
        {
            DocumentBundle bundle = gameManager.LoadDocumentBundle(owningDocumentList[i].name);
            documentBundleList.Add(bundle);
        }
    }


    void SetupFolder(OwningDocumentClass owningDocument)
    {
        DocumentFolderButtonClass folderButton = null;
        DocumentCondition conditionData = null;
        for(int i = 0; i < documentConditionList.Count; i++)
        {
            if(owningDocument.name == documentConditionList[i].fileName)
            {
                conditionData = documentConditionList[i];
                break;
            }
        }
        if(conditionData == null)
        {
            Debug.LogError("좆됐다 컨디션데이터가 없다");
            return;
        }
        for(int i = 0; i < wholeFolderButtonList.Count; i++)
        {
            if(wholeFolderButtonList[i].folderName == conditionData.folderFileName)
            {
                folderButton = wholeFolderButtonList[i];
                break;
            }
        }
        if(folderButton == null)
        {
            GameObject folderCanvasInst = Instantiate(folderCanvasPrefab, wholeFolderParentObject.transform);
            GameObject folderButtonInst = Instantiate(folderButtonPrefab, folderScrollParent);
            folderButton = new DocumentFolderButtonClass(nowFolderButtonIndex, conditionData.folderIngameName, folderCanvasInst, folderButtonInst);
            nowFolderButtonIndex++;

        }
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
