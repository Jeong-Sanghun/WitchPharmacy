﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;


//조제실에서 약재버튼 생성해주고 그걸로 조합해서 카운터매니저로 넘길거임.
public class RoomManager : MonoBehaviour    //SH
{
    class MedicineButton
    {
        public MedicineClass medicineClass;
        public GameObject buttonObject;
        public RectTransform buttonRect;
        public int medicineIndex;
        public int medicineQuant;
        public bool isActive;

        public MedicineButton(GameObject obj, int index, int quant,MedicineClass medicine)
        {
            medicineClass = medicine;
            buttonObject = obj;
            buttonRect = buttonObject.GetComponent<RectTransform>();
            medicineIndex = index;
            medicineQuant = quant;
            isActive = false;
        }
    }

    public CounterManager counterManager;
    GameManager gameManager;
    SaveDataClass saveData;
    List<MedicineClass> medicineDataList;
    List<int> ownedMedicineList;
    Dictionary<int,int> owningMedicineDictionary;
    //위는 기본적인 매니저들 그리고 데이터들

    //월드에 false로 미리 6개를 만들어놓는다. Instantiate하면 렉걸리니까. 그리고 어차피 6개 고정인 스크롤뷰임.
    //[SerializeField]
    //GameObject[] medicineScrollArray;
    //스크롤 뷰에 들어가있는 content들 위아래 길이조정 해줘야함.
    //버튼도 이 아래에 생성할거여서 따로 드래그앤드롭 해줘야함. GetChild로 받아왔는데 순서꼬이면 귀찮아짐.
    [SerializeField]
    RectTransform scrollContent;

    [SerializeField]
    GameObject scrollObject;

    [SerializeField]
    Image[] propertyButtonImageArray;

    //스크롤뷰에 들어가는 약재버튼 하나. 프리팹으로 만들어서 Instantiate해줄거.
    //프리팹들에 들어가는 것들을 다 받아오고, 시작할 때만 설정해주고 Instantiate해주고 그다음 버튼 새로 설정하고 Instantiate해주고 반복.
    [SerializeField]
    GameObject medicineButtonPrefab;
    [SerializeField]
    Text prefabButtonName;
    [SerializeField]
    Image prefabButtonIcon;
    [SerializeField]
    Text prefabButtonQuantity;
    [SerializeField]
    Text prefabButtonFirstEffectIcon;
    [SerializeField]
    Text prefabButtonSecondEffectIcon;
    [SerializeField]
    Text prefabButtonFirstEffectNumber;
    [SerializeField]
    Text prefabButtonSecondEffectNumber;

    List<MedicineButton> activedMedicineButtonList;
    List<MedicineButton> wholeMedicineButtonList;
    //int[] contentButtonQuantityArray;
    bool[] isButtonOn;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        medicineDataList = gameManager.medicineDataWrapper.medicineDataList;
        ownedMedicineList = saveData.ownedMedicineList;
        owningMedicineDictionary = saveData.owningMedicineDictionary;
        /*
        contentButtonQuantityArray = new int[6];
        for(int i = 0; i < contentButtonQuantityArray.Length; i++)
        {
            contentButtonQuantityArray[i] = 0;
        }*/
        isButtonOn = new bool[6];
        for(int i = 0; i < isButtonOn.Length; i++)
        {
            isButtonOn[i] = false;
        }
        activedMedicineButtonList = new List<MedicineButton>();
        wholeMedicineButtonList = new List<MedicineButton>();
        
        for(int i =0; i<ownedMedicineList.Count; i++)
        {
            //내가 가졌던거중에 없으면 컨티뉴
            int index = ownedMedicineList[i];
            if (!owningMedicineDictionary.ContainsKey(index))
            {
                continue;
            }
            int quantity = owningMedicineDictionary[index];
            MedicineClass medicine = medicineDataList[index];
            if(medicine.medicineImage == null)
            {
                StringBuilder builder = new StringBuilder("Items/");
                builder.Append(medicine.name);
                medicine.medicineImage = Resources.Load<Sprite>(builder.ToString());
            }
            prefabButtonIcon.sprite = medicine.medicineImage;
            prefabButtonName.text = medicine.name;
            prefabButtonQuantity.text = quantity.ToString();
            prefabButtonFirstEffectIcon.text = medicine.firstSymptom.ToString();
            prefabButtonSecondEffectIcon.text = medicine.secondSymptom.ToString();
            prefabButtonFirstEffectNumber.text = medicine.firstNumber.ToString();
            prefabButtonSecondEffectNumber.text = medicine.secondNumber.ToString();
            //버튼 세팅 다 해서 instantiate하고 리스트에 넣어줌.
            //이제 이 리스트에서 active하고 위치바꿔주고 그럴거임.
            GameObject buttonObject = Instantiate(medicineButtonPrefab, scrollContent.transform);
            buttonObject.SetActive(true);
            MedicineButton buttonClass = new MedicineButton(buttonObject,index,quantity,medicine);
            wholeMedicineButtonList.Add(buttonClass);


            /*
            if(medicine.firstSymptom == medicine.secondSymptom)
            {
                //요소 두개가 같은 경우.특수 아이템 전용
            }
            else
            {

                GameObject firstButton = Instantiate(medicineButtonPrefab, scrollContentArray[(int)medicine.firstSymptom].transform);
                RectTransform rect = firstButton.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(0, -90 - contentButtonQuantityArray[(int)medicine.firstSymptom] * 180);
                firstButton.SetActive(true);

                GameObject secondButton = Instantiate(medicineButtonPrefab, scrollContentArray[(int)medicine.secondSymptom].transform);
                rect = secondButton.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(0, -90 - contentButtonQuantityArray[(int)medicine.secondSymptom] * 180);
                secondButton.SetActive(true);

                contentButtonQuantityArray[(int)medicine.firstSymptom]++;
                contentButtonQuantityArray[(int)medicine.secondSymptom]++;
            }*/

        }

        /*
        for(int i = 0; i < 6; i++)
        {
            scrollContentArray[i].sizeDelta = new Vector2(0, 180 * contentButtonQuantityArray[i]);
        }*/

    }

    
    //속성 누르면 그 속성 아이템 뜨게하는 버튼
    public void PropertyListButton(int index)
    {
        int buttonQuantity = 0;
        if (isButtonOn[index] == true)
        {
            propertyButtonImageArray[index].color = Color.white;

            isButtonOn[index] = false;
            for (int i = 0; i < wholeMedicineButtonList.Count; i++)
            {
                if ((int)wholeMedicineButtonList[i].medicineClass.firstSymptom == index
                    || (int)wholeMedicineButtonList[i].medicineClass.secondSymptom == index)
                {
                    wholeMedicineButtonList[i].isActive = false;
                    for (int j = 0; j < isButtonOn.Length; j++)
                    {
                        if (j == index)
                        {
                            continue;
                        }
                        if(isButtonOn[j] == true && wholeMedicineButtonList[i].isActive == false)
                        {
                            if((int)wholeMedicineButtonList[i].medicineClass.firstSymptom == j
                                || (int)wholeMedicineButtonList[i].medicineClass.secondSymptom == j)
                            {
                                wholeMedicineButtonList[i].isActive = true;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            propertyButtonImageArray[index].color = Color.red;

            isButtonOn[index] = true;
            for (int i = 0; i < wholeMedicineButtonList.Count; i++)
            {
                if ((int)wholeMedicineButtonList[i].medicineClass.firstSymptom == index
                    || (int)wholeMedicineButtonList[i].medicineClass.secondSymptom == index)
                {
                    wholeMedicineButtonList[i].isActive = true;
                }
            }
        }

        for (int i = 0; i < wholeMedicineButtonList.Count; i++)
        {
            if (wholeMedicineButtonList[i].isActive)
            {
                wholeMedicineButtonList[i].buttonObject.SetActive(true);
                wholeMedicineButtonList[i].buttonRect.anchoredPosition = new Vector2(0, -90 - buttonQuantity * 180);
                buttonQuantity++;
            }
            if (wholeMedicineButtonList[i].isActive == false)
            {
                wholeMedicineButtonList[i].buttonObject.SetActive(false);
            }
        }

        scrollContent.sizeDelta = new Vector2(0, 180 * buttonQuantity);


        /*
        if (scrollObjectArray[index].activeSelf)
        {
            scrollObjectArray[index].SetActive(false);
        }
        else
        {
            scrollObjectArray[index].SetActive(true);
     
        }
        for (int i = 0; i < 6; i++)
        {
            if (i == index)
            {
                continue;
            }
            if (scrollObjectArray[i].activeSelf)
            {
                scrollObjectArray[i].SetActive(false);
            }
        }*/

    }

    public void DragDebug()
    {
        Debug.Log("드래깅");
    }
}