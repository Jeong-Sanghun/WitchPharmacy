using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//여백의 미 버튼
public class DocumentButtonClass
{
    public int index;
    public DocumentBundle bundle;
    public OwningDocumentClass owningDocumentClass;
    public GameObject buttonObject;
    public GameObject documentCanvas;
    public Image documentImage;
    public Text documentText;
    public Text gainedRegionText;
    public DocumentCondition documentCondition;
    public bool isOpened;

    public DocumentButtonClass(int _index,DocumentCondition condition, DocumentBundle doc,OwningDocumentClass ownDoc, GameObject buttonObj)
    {
        owningDocumentClass = ownDoc;
        index = _index;
        bundle = doc;
        buttonObject = buttonObj;
        documentCondition = condition;
        Text buttonText = buttonObject.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        buttonText.text = condition.ingameName;
    }


    public void SetupDocument(GameObject canvas, RegionPropertyWrapper regionWrapper,UILanguagePack languagePack)
    {
        isOpened = true;
        documentCanvas = canvas;
        //여기서 겟 차일드 지랄지랄 해주면 된다.
        documentImage = canvas.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        documentText = canvas.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>();
        gainedRegionText = canvas.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetComponent<Text>();
        documentImage.sprite = bundle.LoadSprite();
        documentText.text = bundle.document;
        string regionName = null;
        for(int i =0;i < regionWrapper.regionPropertyArray.Length; i++)
        {
            if(owningDocumentClass.gainedRegion == regionWrapper.regionPropertyArray[i].regionFileName)
            {
                regionName = regionWrapper.regionPropertyArray[i].regionName;
            }
        }
        gainedRegionText.text = languagePack.documentGainedRegion + regionName;



    }

    public void ActiveDocument(bool active)
    {
        documentCanvas.SetActive(active);
    }

    
}
