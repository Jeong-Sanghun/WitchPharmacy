using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//여백의 미 버튼
public class DocumentButtonClass
{
    public int index;
    public DocumentBundle bundle;
    public GameObject buttonObject;
    public GameObject documentCanvas;
    public bool isOpened;

    public DocumentButtonClass(int _index, DocumentBundle doc, GameObject buttonObj)
    {
        index = _index;
        bundle = doc;
        buttonObject = buttonObj;
    }


    public void SetupDocument(GameObject canvas)
    {
        isOpened = true;
        documentCanvas = canvas;
        //여기서 겟 차일드 지랄지랄 해주면 된다.

        documentCanvas.SetActive(true);

    }

    public void ActiveDocument(bool active)
    {
        documentCanvas.SetActive(active);
    }

    
}
