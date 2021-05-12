using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedicineButton
{

    public MedicineClass medicineClass; //아이템의 약 속성이 뭔지
    public GameObject buttonObject;     //스크롤뷰에 들어가있는 버튼 오브젝트
    public GameObject propertyObject;   //클릭하면 속성창이 떠야되는데 그 속성창 오브젝트
    public RectTransform buttonRect;    //버튼 rect 계속 getComponent해주기 귀찮아서
    public GameObject medicineObject;   //드래그앤드롭시 약재 오브젝트
    public OwningMedicineClass owningMedicine;     //owningMedicineList에서 가져오는거
    public int medicineIndex;           //약재 딕셔너리의 인덱스
    public int medicineQuant;           //약재 몇개인지
    public Text quantityText;
    public Text propertyQuantityText;
    public bool isActive;               //지금 activeSelf상태. 속성을 껐다켰다 해줘야해서.
    public bool isOnPot;
    public bool zeroMedicine;
    public GameObject potMedicineObject;    //팟 위에 둥실둥실 떠있는 오브젝트

    public MedicineButton(GameObject obj, int index, int quant,
        MedicineClass medicine, GameObject property, GameObject medicineObj,
        Text quantText, Text propertyQuantText)
    {
        medicineClass = medicine;
        buttonObject = obj;
        propertyObject = property;
        medicineObject = medicineObj;
        buttonRect = buttonObject.GetComponent<RectTransform>();
        medicineIndex = index;
        medicineQuant = quant;
        quantityText = quantText;
        propertyQuantityText = propertyQuantText;
        isActive = false;
        zeroMedicine = false;
    }




}