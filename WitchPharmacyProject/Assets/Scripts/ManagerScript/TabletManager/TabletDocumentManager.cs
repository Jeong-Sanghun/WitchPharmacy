﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class TabletDocumentManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    List<OwningDocumentClass> owningDocumentList;
    List<DocumentBundle> documentBundleList;
    List<DocumentCondition> documentConditionList;
    List<DocumentFolderButtonClass> wholeFolderButtonList;
    int nowFolderButtonIndex;
    int nowOpenedFolderIndex;
    int nowOpenedDocumentIndex;
    GameObject nowOpenedDocumentCanvas;

    [SerializeField]
    GameObject wholeFolderParentObject;
    [SerializeField]
    Transform folderScrollParent;
    [SerializeField]
    GameObject folderButtonPrefab;
    [SerializeField]
    GameObject folderCanvasPrefab;
    [SerializeField]
    GameObject oneLineDocumentButtonPrefab;
    [SerializeField]
    GameObject documentParentPrefab;
    [SerializeField]
    Text directoryText;
    [SerializeField]
    Transform directoryCanvas;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        owningDocumentList = saveData.owningDocumentList;
        documentConditionList = gameManager.documentConditionWrapper.documentConditionList;
        folderButtonPrefab.SetActive(false);
        folderCanvasPrefab.SetActive(false);
        oneLineDocumentButtonPrefab.SetActive(false);
        documentParentPrefab.SetActive(false);
        nowOpenedFolderIndex = -1;
        nowOpenedDocumentIndex = -1;
        UpdateDirectory();

        documentBundleList = new List<DocumentBundle>();
        wholeFolderButtonList = new List<DocumentFolderButtonClass>();
        nowFolderButtonIndex = 0;
        for(int i = 0; i < owningDocumentList.Count; i++)
        {
            //DocumentBundle bundle = gameManager.LoadDocumentBundle(owningDocumentList[i].name);
            //documentBundleList.Add(bundle);
            SetupDocument(owningDocumentList[i]);
        }
    }

    //오우닝 다큐먼트 클래스 하나 넣어서 만듦.
    void SetupDocument(OwningDocumentClass owningDocument)
    {
        DocumentFolderButtonClass folderButton = null;
        DocumentCondition conditionData = null;
        DocumentBundle bundle = null;
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
        bundle = gameManager.LoadDocumentBundle(conditionData.fileName);
        for (int i = 0; i < wholeFolderButtonList.Count; i++)
        {
            if(wholeFolderButtonList[i].folderName == conditionData.folderIngameName)
            {
                folderButton = wholeFolderButtonList[i];
                break;
            }
        }
        if(folderButton == null)
        {
            GameObject folderCanvasInst = Instantiate(folderCanvasPrefab, wholeFolderParentObject.transform);
            GameObject folderButtonInst = Instantiate(folderButtonPrefab, folderScrollParent);
            folderButtonInst.SetActive(true);
            Button button = folderButtonInst.transform.GetChild(1).GetComponent<Button>();
            int delegateIndex = nowFolderButtonIndex;
            button.onClick.AddListener(() => OnFolderButtonDown(delegateIndex));
            folderButton = new DocumentFolderButtonClass(nowFolderButtonIndex, conditionData.folderIngameName,
                folderCanvasInst, folderButtonInst);
            wholeFolderButtonList.Add(folderButton);
            FolderButtonAlign();
            nowFolderButtonIndex++;
        }
        GameObject docButtonObj = Instantiate(oneLineDocumentButtonPrefab, folderButton.documentButtonScrollParent);
        docButtonObj.SetActive(true);
        Button docButtonComponent = docButtonObj.transform.GetChild(1).GetComponent<Button>();
        int deleIndex = folderButton.documentButtonList.Count;
        docButtonComponent.onClick.AddListener(() => OnDocumentButtonDown(deleIndex));
        
        DocumentButtonClass documentButton = new DocumentButtonClass(folderButton.documentButtonList.Count,
            conditionData,bundle,owningDocument, docButtonObj);
       
        folderButton.AddDocument(documentButton);
        folderButton.SetupContentRect();
        directoryCanvas.SetAsLastSibling();

    }


    void FolderButtonAlign()
    {
        for(int i = 0; i < wholeFolderButtonList.Count; i++)
        {
            wholeFolderButtonList[i].folderButtonObject.GetComponent<RectTransform>().anchoredPosition
                 = new Vector3(0, -120 - 220 * i, 0);
        }
        folderScrollParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 20 + 220 * wholeFolderButtonList.Count);
    }

    void OnFolderButtonDown(int index)
    {
        nowOpenedFolderIndex = index;
        wholeFolderButtonList[index].folderCanvasObject.SetActive(true);
        UpdateDirectory();
    }
    public void OnFolderBackButton()
    {
        wholeFolderButtonList[nowOpenedFolderIndex].folderCanvasObject.SetActive(false);
        nowOpenedFolderIndex = -1;
        UpdateDirectory();
    }

    void OnDocumentButtonDown(int index)
    {
        if(nowOpenedFolderIndex == -1)
        {
            return;
        }
        DocumentFolderButtonClass folder = wholeFolderButtonList[nowOpenedFolderIndex];
        nowOpenedDocumentIndex = index;
        DocumentButtonClass document = folder.documentButtonList[index];
        if(document.isOpened == false)
        {
            GameObject canvas = Instantiate(documentParentPrefab, wholeFolderParentObject.transform);
            //Tlqkf zz
            document.SetupDocument(canvas,gameManager.regionPropertyWrapper,gameManager.languagePack);
            directoryCanvas.SetAsLastSibling();
        }
        nowOpenedDocumentCanvas = document.documentCanvas;
        nowOpenedDocumentCanvas.SetActive(true);
        UpdateDirectory();

    }
    public void OnDocumentBackButton()
    {
        if(nowOpenedDocumentCanvas== null)
        {
            return;
        }
        nowOpenedDocumentIndex = -1;
        nowOpenedDocumentCanvas.SetActive(false);
        UpdateDirectory();
    }

    public void WholeDocumentOpenButton(bool active)
    {
        wholeFolderParentObject.SetActive(active);
    }

    void UpdateDirectory()
    {

        StringBuilder builder = new StringBuilder("main :"); 
        if (nowOpenedFolderIndex != -1)
        {
            builder.Append(wholeFolderButtonList[nowOpenedFolderIndex].folderName);
            if (nowOpenedDocumentIndex != -1)
            {
                builder.Append("/");
                builder.Append(wholeFolderButtonList[nowOpenedFolderIndex].documentButtonList[nowOpenedDocumentIndex].documentCondition.ingameName);
            }
        }
        directoryText.text = builder.ToString();
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
