using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

public class StoreMedicineManager : MonoBehaviour,IStore {

    const int nTypesOfMedicine = 4;
    const int rQuantityToSell = 10;
    GameManager gameManager;
    SaveDataClass saveData;
    RegionProperty[] regionPropertyArray;
    List<int> unlockedRegionIndex;
    List<MedicineClass> medicineDataList;
    List<int> ownedMedicineList;
    List<OwningMedicineClass> owningMedicineList;
    //Dictionary<int, int> owningMedicineDictionary;
    //List<CookedMedicineData> cookedMedicineDataList;
    //위는 기본적인 매니저들 그리고 데이터들

    //스크롤 뷰에 들어가있는 content들 위아래 길이조정 해줘야함.
    //버튼도 이 아래에 생성할거여서 따로 드래그앤드롭 해줘야함. GetChild로 받아왔는데 순서꼬이면 귀찮아짐.
    [SerializeField]
    RectTransform scrollContent;


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
    Text prefabButtonCost;
    [SerializeField]
    Text prefabButtonFirstEffectIcon;
    [SerializeField]
    Text prefabButtonSecondEffectIcon;
    [SerializeField]
    Text prefabButtonFirstEffectNumber;
    [SerializeField]
    Text prefabButtonSecondEffectNumber;

    [SerializeField]
    GameObject popupParent;
    [SerializeField]
    Text popupQuantityText;
    [SerializeField]
    Slider quantSlider;

    [SerializeField]
    GameObject notEnoughCoinPopup;
    [SerializeField]
    Text coinText;


    List<MedicineButton> wholeMedicineButtonList;
    //int[] contentButtonQuantityArray;
    bool[] isButtonOn;

    int nowButtonIndex;




    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        unlockedRegionIndex = saveData.unlockedRegionIndex;
        regionPropertyArray = gameManager.regionPropertyWrapper.regionPropertyArray;
        medicineDataList = gameManager.medicineDataWrapper.medicineDataList;
        ownedMedicineList = saveData.ownedMedicineList;
        owningMedicineList = saveData.owningMedicineList;
        isButtonOn = new bool[5];
        for (int i = 0; i < isButtonOn.Length; i++)
        {
            isButtonOn[i] = false;
        }
        wholeMedicineButtonList = new List<MedicineButton>();

        List<int> appearingMedicineList = new List<int>();

        for (int i = 0; i < unlockedRegionIndex.Count; i++)
        {
            int count = 0;
            int[] availableMedicineArray = regionPropertyArray[unlockedRegionIndex[i]].regionAvailableMedicine;
            for (int j = 0; j < availableMedicineArray.Length; j++)
            {
                //이미 appearing에 넣었다면 다시 안넣어야 되니까 생각 안함.
                if (appearingMedicineList.Contains(availableMedicineArray[j]))
                {
                    continue;
                }
                if (ownedMedicineList.Contains(availableMedicineArray[j]))
                {
                    count++;
                }
            }
            //내가 해금한 지역의 약재중 n보다 큰 종류를 가진적이 있다면.
            if(count > nTypesOfMedicine)
            {
                int addingCount =0;
                while(addingCount < nTypesOfMedicine)
                {
                    for (int j = 0; j < availableMedicineArray.Length; j++)
                    {
                        if (appearingMedicineList.Contains(availableMedicineArray[j]))
                        {
                            continue;
                        }
                        if (ownedMedicineList.Contains(availableMedicineArray[j]))
                        {
                            int rand = Random.Range(0, 2);
                            if (rand == 0)
                            {
                                addingCount++;
                                appearingMedicineList.Add(availableMedicineArray[j]);
                            }
                            
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < availableMedicineArray.Length; j++)
                {
                    if (ownedMedicineList.Contains(availableMedicineArray[j]))
                    {
                        appearingMedicineList.Add(availableMedicineArray[j]);
                    }
                }
            }
        }

        int buttonIndex = 0;
        for (int i = 0; i < appearingMedicineList.Count; i++)
        {
            //내가 가졌던거중에 없으면 컨티뉴
            int index = appearingMedicineList[i];
            if (index == 0)
            {
                continue;
            }
            OwningMedicineClass owningMedicine = null;
            for (int j = 0; j < owningMedicineList.Count; j++)
            {
                if (owningMedicineList[j].medicineIndex == index)
                {
                    owningMedicine = owningMedicineList[j];
                    break;
                }

            }
            if (owningMedicine == null)
            {
                owningMedicine = new OwningMedicineClass();
                owningMedicine.medicineIndex = index;
                owningMedicine.medicineQuantity = 0;
                owningMedicineList.Add(owningMedicine);
            }
            int quantity = rQuantityToSell;
            MedicineClass medicine = medicineDataList[index];
            StringBuilder nameBuilder = new StringBuilder(medicine.firstName);
            nameBuilder.Append(" ");
            nameBuilder.Append(medicine.secondName);
            medicine.LoadImage();
            prefabButtonIcon.sprite = medicine.medicineImage;
            prefabButtonName.text = nameBuilder.ToString();
            prefabButtonCost.text = medicine.cost.ToString();
            if (medicine.firstSymptom == Symptom.none)
            {
                prefabButtonQuantity.text = null;
            }
            else
            {
                prefabButtonQuantity.text = quantity.ToString();
            }

            prefabButtonFirstEffectIcon.text = medicine.firstSymptom.ToString();
            prefabButtonSecondEffectIcon.text = medicine.secondSymptom.ToString();
            prefabButtonFirstEffectNumber.text = medicine.firstNumber.ToString();
            prefabButtonSecondEffectNumber.text = medicine.secondNumber.ToString();
            //버튼 세팅 다 해서 instantiate하고 리스트에 넣어줌.
            //이제 이 리스트에서 active하고 위치바꿔주고 그럴거임.
            GameObject buttonObject = Instantiate(medicineButtonPrefab, scrollContent.transform);
            buttonObject.SetActive(true);


            Text quantText = buttonObject.transform.GetChild(2).GetComponent<Text>();


            MedicineButton buttonClass =
                new MedicineButton(buttonObject, index, quantity,
                medicine, null, null, quantText, null);
            buttonClass.owningMedicine = owningMedicine;
            wholeMedicineButtonList.Add(buttonClass);
            //이 위까지가 프리팹들 다 설정해서 whoelMedicineButtonList에 buttonClass를 추가하는거임.
            //wholeMedicineButtonList의 index는 바로 아랫줄과 onButtonUp Down Drag함수에서 사용하니 medicineDictionary와 혼동하지 않도록 유의
            //정훈아 미안해 이거 주석 다 못적겠어. 그냥 그러려니 해. 셋업이야.

            int delegateIndex = buttonIndex;
            Button button = buttonClass.buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => OnButtonDown(delegateIndex));

            buttonIndex++;
            //wholeMedicienButtonList의 index임.

        }

        coinText.text = saveData.coin.ToString();

    }


    //속성 누르면 그 속성 아이템 뜨게하는 버튼
    public void PropertyListButton(int index)
    {
        int buttonQuantity = 0;
        if (isButtonOn[index] == true)
        {
            propertyButtonImageArray[index].color = Color.white;

            isButtonOn[index] = false;
            int pushedButton = 0;
            for (int i = 0; i < 5; i++)
            {
                if (isButtonOn[i])
                {
                    pushedButton++;
                }
            }
            for (int i = 0; i < wholeMedicineButtonList.Count; i++)
            {
                if (wholeMedicineButtonList[i].zeroMedicine)
                {
                    wholeMedicineButtonList[i].isActive = false;
                    continue;
                }
                if (pushedButton == 0)
                {
                    wholeMedicineButtonList[i].isActive = false;
                    continue;
                }
                if (wholeMedicineButtonList[i].medicineClass.firstSymptom == Symptom.none)
                {
                    wholeMedicineButtonList[i].isActive = true;
                    continue;
                }

                wholeMedicineButtonList[i].isActive = false;

                if (pushedButton > 1)
                {
                    if (isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.firstSymptom] &&
    isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.secondSymptom])
                    {
                        wholeMedicineButtonList[i].isActive = true;
                    }
                }
                else
                {
                    if (isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.firstSymptom] ||
isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.secondSymptom])
                    {
                        wholeMedicineButtonList[i].isActive = true;
                    }
                }
            }
        }
        else
        {
            propertyButtonImageArray[index].color = Color.red;

            isButtonOn[index] = true;
            int pushedButton = 0;
            for (int i = 0; i < 5; i++)
            {
                if (isButtonOn[i])
                {
                    pushedButton++;
                }
            }
            for (int i = 0; i < wholeMedicineButtonList.Count; i++)
            {
                if (wholeMedicineButtonList[i].zeroMedicine)
                {
                    wholeMedicineButtonList[i].isActive = false;
                    continue;
                }
                if (wholeMedicineButtonList[i].medicineClass.firstSymptom == Symptom.none)
                {
                    wholeMedicineButtonList[i].isActive = true;
                    continue;
                }
                if (pushedButton > 1)
                {
                    if (isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.firstSymptom] &&
    isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.secondSymptom])
                    {
                        wholeMedicineButtonList[i].isActive = true;
                    }
                    else
                    {
                        wholeMedicineButtonList[i].isActive = false;
                    }
                }
                else
                {
                    if (isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.firstSymptom] ||
isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.secondSymptom])
                    {
                        wholeMedicineButtonList[i].isActive = true;
                    }
                    else
                    {
                        wholeMedicineButtonList[i].isActive = false;
                    }
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

    }

    //버튼 클릭했을 때. 인덱스는 wholeButtonList의 index임. 거기안에 medicineClass들어있음.
    //팝업올라옴
    public void OnButtonDown(int index)
    {
        if (popupParent.activeSelf)
        {
            return;
        }

        nowButtonIndex = index;
        popupQuantityText.text = "0";
        quantSlider.value = 0;
        if(!wholeMedicineButtonList[index].zeroMedicine)
            popupParent.SetActive(true);
    }

    public void OnSliderValueChange()
    {
        popupQuantityText.text = ((int)(wholeMedicineButtonList[nowButtonIndex].medicineQuant * quantSlider.value)).ToString();
    }

    //그 버튼눌러서 1개씩 올라가느넉
    public void OnQuantityChangeButton(bool plus)
    {
        int one;
        if (plus)
        {
            one = 1;
        }
        else
        {
            one = -1;
        }
        float ratio = 1.0f / wholeMedicineButtonList[nowButtonIndex].medicineQuant;
        //float value = (int)(wholeMedicineButtonList[nowButtonIndex].medicineQuant * quantSlider.value) *ratio;
        //quantSlider.value = value;
        
        if(quantSlider.value + one * ratio <0)
        {
            quantSlider.value = 0;
        }
        else if(quantSlider.value + one * ratio > 1)
        {
            quantSlider.value = 1;
        }
        else
        {
            quantSlider.value += one * ratio;
        }

        popupQuantityText.text =((int)(wholeMedicineButtonList[nowButtonIndex].medicineQuant * quantSlider.value)).ToString();

    }

    //살게요버튼
    public void OnBuyButton()
    {
        int quant = (int)(wholeMedicineButtonList[nowButtonIndex].medicineQuant * quantSlider.value);
        popupParent.SetActive(false);
        if (quant == 0)
        {
            return;
        }
        else
        {
            if(saveData.coin - quant* wholeMedicineButtonList[nowButtonIndex].medicineClass.cost < 0)
            {
                //여기서 에러메시지 표출
                notEnoughCoinPopup.SetActive(true);
                return;
            }
            saveData.coin -= quant * wholeMedicineButtonList[nowButtonIndex].medicineClass.cost;
            wholeMedicineButtonList[nowButtonIndex].medicineQuant -= quant;
            if (wholeMedicineButtonList[nowButtonIndex].medicineQuant <= 0)
            {
                wholeMedicineButtonList[nowButtonIndex].zeroMedicine = true;
            }
            coinText.text = saveData.coin.ToString();

            wholeMedicineButtonList[nowButtonIndex].owningMedicine.medicineQuantity += quant;
            wholeMedicineButtonList[nowButtonIndex].quantityText.text = wholeMedicineButtonList[nowButtonIndex].medicineQuant.ToString();
        }
        if (wholeMedicineButtonList[nowButtonIndex].zeroMedicine)
        {
            for (int i = 0; i < isButtonOn.Length; i++)
            {
                if (isButtonOn[i] == true)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        PropertyListButton(i);
                    }
                }

            }
        }
    }

    public void OnNotEnoughCoinPopupButton()
    {
        notEnoughCoinPopup.SetActive(false);

    }

    public void OnPopupBackButton()
    {
        popupParent.SetActive(false);
        nowButtonIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
