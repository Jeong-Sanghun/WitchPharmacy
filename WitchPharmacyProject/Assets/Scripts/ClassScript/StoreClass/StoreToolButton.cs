using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StoreToolButton
{
    public int buttonIndex;
    public StoreToolClass storeTool;

    public GameObject buttonObject;     //스크롤뷰에 들어가있는 버튼 오브젝트
    public RectTransform buttonRect;    //버튼 rect 계속 getComponent해주기 귀찮아서
    public OwningToolClass owningTool;     //owningMedicineList에서 가져오는거
    public int toolIndex;           //약재 딕셔너리의 인덱스
    public int toolQuant;           //약재 몇개인지
    public Text quantityText;
    public bool zeroTool;

    public StoreToolButton(GameObject obj, int index, int quant,
        Text quantText)
    {
        buttonObject = obj;
        buttonRect = buttonObject.GetComponent<RectTransform>();
        toolIndex = index;
        toolQuant = quant;
        quantityText = quantText;
        zeroTool = false;
    }
}
