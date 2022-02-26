using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

public class TutorialMedicineManager : MonoBehaviour
{
    [SerializeField]
    TutorialRoomCounterManager roomCounterManager;
   
    [SerializeField]
    TutorialCounterManager counterManager;
    [SerializeField]
    TutorialCookedMedicineManager cookedMedicineManager;
    [SerializeField]
    TutorialSymptomChartManager symptomChartManager;
    [SerializeField]
    PotAnimationManager potAnimationManager;

    GameManager gameManager;
    List<MedicineClass> medicineDataList;
    //List<int> ownedMedicineList;
    List<OwningMedicineClass> owningMedicineList;
    //Dictionary<int, int> owningMedicineDictionary;
    //List<CookedMedicineData> cookedMedicineDataList;
    //위는 기본적인 매니저들 그리고 데이터들

    //스크롤 뷰에 들어가있는 content들 위아래 길이조정 해줘야함.
    //버튼도 이 아래에 생성할거여서 따로 드래그앤드롭 해줘야함. GetChild로 받아왔는데 순서꼬이면 귀찮아짐.
    [SerializeField]
    RectTransform regularScrollContent;
    //[SerializeField]
    //RectTransform specialScrollContent;
    //[SerializeField]
    //GameObject specialScrollUIParent;


    [SerializeField]
    Image[] propertyMainButtonImageArray;
    [SerializeField]
    Image[] propertySubButtonImageArray;
    [SerializeField]
    GameObject[] propertySubButtonLockArray;
    Button[] propertySubButtonComponentArray;
    [SerializeField]
    GameObject propertySubListParent;
    [SerializeField]
    Transform propertyObjectParent;

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
    Image prefabButtonFirstEffectIcon;
    [SerializeField]
    Image prefabButtonSecondEffectIcon;
    //[SerializeField]
    //Text prefabButtonFirstEffectNumber;
    //[SerializeField]
    //Text prefabButtonSecondEffectNumber;

    [SerializeField]
    GameObject medicineInstanceParent;


    List<MedicineButton> wholeMedicineButtonList;
    //List<MedicineButton> wholeSpecialMedicineButtonList;
    //int[] contentButtonQuantityArray;
    bool[] isMainButtonOn;
    bool[] isSubButtonOn;


    //이거 드래그앤드롭할떄 필요함
    [SerializeField]
    GameObject medicineObjectPrefab;        //약재 오브젝트 프리팹
    [SerializeField]
    GameObject[] potMedicineParentArray;
    List<GameObject> potMedicineObjectList;

    GameObject touchedObject;               //터치한 오브젝트
    RaycastHit2D hit;                         //터치를 위한 raycastHit
    public Camera cam;                      //레이캐스트를 위한 카메라.

    [SerializeField]
    GameObject roomUICanvas;    //카운터로 옮겨갈때 이거 바꿔줘야해.
    [SerializeField]
    GameObject counterUICanvas;


    //현재 팟 안에 들어가있는 메디슨의 리스트. 이게 존나 중요.
    //이거 심텀 차트 매니저에서 그대로 참조함.
    public List<MedicineButton> medicineInPotList;

    //약재를 다 끓여서 만들었는지
    //counterManager에서 받아올거임. 쿡한상태에서 간거하고 안하고 간거랑 다를테니까
    //countermanager에서 이 값을 변화시켜줌.
    [HideInInspector]
    public bool isPotCooked;
    CookedMedicine cookedMedicine;
    [SerializeField]
    Text cookedMedicineText;
    [SerializeField]
    GameObject cookButtonObject;
    [SerializeField]
    GameObject cookedMedicinePrefab;

    //현재 조제실에 있는가.
    //[HideInInspector]
    //public bool nowInRoom;
    [SerializeField]
    Text usedCoinText;


    //증상기록창에서 측정도구로 측정한 증상은 토글이 고정이 되어야함
    //그래서 어떤 걸 측정도구로 측정했는지 알아야함.
    bool isDraggingMedicineFromPot;
    bool isSpecialMedicine;

    //룸매니저에서 투 카운터버튼에서 사용
    public bool nowCookingAnimation;

    //[SerializeField]
    //Animator cookAnimator;
    float doubleClickTimer;
    int doubleClickIndex;
    int frostItemIndex = 0;
    int desireItemIndex = 0;
    bool frostAdded = false;
    bool desireAdded = false;
    bool onStartArrangement = true;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleton;
        medicineDataList = gameManager.medicineDataWrapper.medicineDataList;
        owningMedicineList = new List<OwningMedicineClass>();
        AddMedicineBySymptom(gameManager.medicineDataWrapper, Symptom.water, Symptom.fire);
        //ownedMedicineList = saveData.ownedMedicineList;
        isDraggingMedicineFromPot = false;
        isMainButtonOn = new bool[5];
        isSubButtonOn = new bool[5];
        propertySubButtonComponentArray = new Button[5];
        for (int i = 0; i < isMainButtonOn.Length; i++)
        {
            isMainButtonOn[i] = false;
            isSubButtonOn[i] = false;
            propertySubButtonComponentArray[i] = propertySubButtonImageArray[i].GetComponent<Button>();
        }
        wholeMedicineButtonList = new List<MedicineButton>();
        medicineInPotList = new List<MedicineButton>();
        potMedicineObjectList = new List<GameObject>();
        //wholeSpecialMedicineButtonList = new List<MedicineButton>();

        isPotCooked = false;
        cookButtonObject.SetActive(false);

        doubleClickTimer = 0;



        int buttonIndex = 0;
        for (int i = 0; i < owningMedicineList.Count; i++)
        {
            //내가 가졌던거중에 없으면 컨티뉴
            int index = owningMedicineList[i].medicineIndex;
            OwningMedicineClass owningMedicine = owningMedicineList[i];
            int cost = owningMedicine.medicineCost;
            //int quantity = owningMedicine.medicineQuantity;
            //if(quantity == 0)
            //{
            //    continue;
            //}
            MedicineClass medicine = medicineDataList[index];
            StringBuilder nameBuilder = new StringBuilder(medicine.firstName);
            nameBuilder.Append(" ");
            nameBuilder.Append(medicine.secondName);

            prefabButtonIcon.sprite = medicine.LoadImage();
            prefabButtonName.text = nameBuilder.ToString();
            if (medicine.GetFirstSymptom() == Symptom.none)
            {
                prefabButtonQuantity.text = null;
            }
            else
            {
                prefabButtonQuantity.text = cost.ToString();
            }

            //prefabButtonFirstEffectIcon.text = medicine.GetFirstSymptom().ToString();
            //prefabButtonSecondEffectIcon.text = medicine.GetSecondSymptom().ToString();
            //prefabButtonFirstEffectNumber.text = medicine.firstNumber.ToString();
            //prefabButtonSecondEffectNumber.text = medicine.secondNumber.ToString();
            prefabButtonFirstEffectIcon.sprite = medicine.GetIcon(medicine.GetFirstSymptom().ToString() + medicine.firstNumber.ToString());
            prefabButtonSecondEffectIcon.sprite = medicine.GetIcon(medicine.GetSecondSymptom().ToString() + medicine.secondNumber.ToString());
            //버튼 세팅 다 해서 instantiate하고 리스트에 넣어줌.
            //이제 이 리스트에서 active하고 위치바꿔주고 그럴거임.
            GameObject buttonObject = Instantiate(medicineButtonPrefab, regularScrollContent.transform);
            buttonObject.SetActive(true);
            GameObject propertyObject = Instantiate(medicineButtonPrefab, propertyObjectParent);
            propertyObject.SetActive(false);
            propertyObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-395, 115);
            propertyObject.GetComponent<Image>().color = Color.grey;
            propertyObject.GetComponent<Button>().enabled = false;
            Text quantText = buttonObject.transform.GetChild(3).GetComponent<Text>();
            Text propertyQuantText = propertyObject.transform.GetChild(3).GetComponent<Text>();
            EventTrigger propertyEvent = propertyObject.AddComponent<EventTrigger>();
            GameObject medicineObj = Instantiate(medicineObjectPrefab, medicineInstanceParent.transform);
            medicineObj.SetActive(false);
            medicineObj.GetComponent<SpriteRenderer>().sprite = medicine.LoadImage();
            MedicineButton buttonClass =
                new MedicineButton(buttonObject, index,
                medicine, propertyObject, medicineObj, quantText, propertyQuantText);
            buttonClass.owningMedicine = owningMedicine;
            wholeMedicineButtonList.Add(buttonClass);
            //이 위까지가 프리팹들 다 설정해서 whoelMedicineButtonList에 buttonClass를 추가하는거임.
            //wholeMedicineButtonList의 index는 바로 아랫줄과 onButtonUp Down Drag함수에서 사용하니 medicineDictionary와 혼동하지 않도록 유의
            //정훈아 미안해 이거 주석 다 못적겠어. 그냥 그러려니 해. 셋업이야.

            if(medicine.GetFirstSymptom() == Symptom.water && medicine.GetSecondSymptom() == Symptom.fire)
            {
                if((medicine.firstNumber==-1 && medicine.secondNumber == 2)
                    || (medicine.firstNumber == -1 && medicine.secondNumber == -2))
                {
                    if(medicine.secondNumber == 2)
                    {
                        desireItemIndex = index;
                        roomCounterManager.desireItemGlow = buttonClass.buttonObject.transform.
                            GetChild(6).GetComponent<Image>();
                    }
                    else if(medicine.secondNumber == -2)
                    {
                        frostItemIndex = index;
                        roomCounterManager.frostItemGlow = buttonClass.buttonObject.transform.
                            GetChild(6).GetComponent<Image>();
                    }
                    int delegateIndex = buttonIndex;
                    Button button = buttonClass.buttonObject.GetComponent<Button>();
                    button.onClick.AddListener(() => OnButtonDown(delegateIndex));


                    //scrollView의 각 버튼마다 index에 맞는 eventTrigger를 심어줘야함.
                    Transform iconObject = buttonObject.transform.GetChild(1);
                    EventTrigger buttonEvent = iconObject.GetComponent<EventTrigger>();

                    //버튼 이벤트
                    EventTrigger.Entry entry2 = new EventTrigger.Entry();
                    entry2.eventID = EventTriggerType.Drag;
                    entry2.callback.AddListener((data) => { OnButtonDrag((PointerEventData)data, delegateIndex); });
                    buttonEvent.triggers.Add(entry2);

                    //속성창 이벤트
                    EventTrigger.Entry entry3 = new EventTrigger.Entry();
                    entry3.eventID = EventTriggerType.PointerDown;
                    entry3.callback.AddListener((data) => { OnButtonDown(delegateIndex); });
                    propertyEvent.triggers.Add(entry3);
                }
            }
            

            propertyEvent.transform.GetChild(1).GetComponent<EventTrigger>().enabled = false;

            buttonIndex++;
            //wholeMedicienButtonList의 index임.

        }


        for (int i = 0; i < potMedicineParentArray.Length; i++)
        {
            potMedicineParentArray[i].SetActive(false);
        }

        //SpecialScrollAlign();
        //PropertyListButton(0);
        ArrangeMainButton(1);

    }

    void AddMedicineBySymptom(MedicineDataWrapper dataWrapper, Symptom firstSymptom, Symptom secondSymptom)
    {
        List<MedicineClass> dataList = dataWrapper.medicineDataList;
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].GetFirstSymptom() == firstSymptom && dataList[i].GetSecondSymptom() == secondSymptom)
            {
                OwningMedicineClass owningMedicine = new OwningMedicineClass(i, dataList[i].cost);
                owningMedicineList.Add(owningMedicine);
            }
            else if (dataList[i].GetFirstSymptom() == secondSymptom && dataList[i].GetSecondSymptom() == firstSymptom)
            {

                OwningMedicineClass owningMedicine = new OwningMedicineClass(i, dataList[i].cost);
                owningMedicineList.Add(owningMedicine);
            }
        }
    }


    //GameObject draggingObject;
    //약재 떨어뜨릴 때 약재를 꺼줘야해서.
    public void Update()
    {
        if (doubleClickIndex != -1)
        {
            doubleClickTimer += Time.deltaTime;
            if (doubleClickTimer > 0.8f)
            {
                doubleClickTimer = 0;
                doubleClickIndex = -1;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //손 뗄 때 내가 약을 들고있는 상태인지, 그거를 누구 위에 올려놓았는지 판단.
            OnButtonUp();

        }

        //if (Input.GetMouseButton(0) && isPotCooked == false && isDraggingMedicineFromPot == true
        //    && (roomCounterManager.isGlowing[(int)ActionKeyword.AddDesireGlow] ||
        //    roomCounterManager.isGlowing[(int)ActionKeyword.AddFrostGlow]))
        //{
        //    if (draggingObject != null)
        //    {
        //        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        //        draggingObject.transform.position = new Vector3(mousePos.x, mousePos.y, touchedObject.transform.position.z);
        //    }
        //}

    }


    void AddMedicineToPot()
    {
        if (isPotCooked)
        {
            return;
        }
        bool desire = roomCounterManager.isGlowing[(int)ActionKeyword.AddDesireGlow];
        bool frost = roomCounterManager.isGlowing[(int)ActionKeyword.AddFrostGlow];
        int medicineIndex = wholeMedicineButtonList[nowButtonIndex].medicineIndex;
        if (desire==false && frost == false)
        {
            return;
        }
        if(desire == true && medicineIndex == frostItemIndex)
        {
            return;
        }
        if(frost == true && medicineIndex == desireItemIndex)
        {
            return;
        }

        if (medicineIndex == frostItemIndex && frostAdded == true)
        {
            return;
        }
        if (medicineIndex == desireItemIndex && desireAdded == true)
        {
            return;
        }

        MedicineButton nowMedicineButton;
        GameObject inst;
        GameObject medicineObj;
        int nowPotIndex;
        nowPotIndex = medicineInPotList.Count;


        if (nowPotIndex >= 3)
        {
            Debug.Log("자리가 없어요");
            return;
        }
        nowMedicineButton = wholeMedicineButtonList[nowButtonIndex];
        UseCoin(nowMedicineButton.owningMedicine.medicineCost);
        medicineObj = nowMedicineButton.medicineObject;
        inst = Instantiate(medicineObj, potMedicineParentArray[nowPotIndex].transform);
        potMedicineParentArray[nowPotIndex].SetActive(true);

        potAnimationManager.SetPotColor(nowMedicineButton.medicineClass.GetSecondSymptom(), nowMedicineButton.medicineClass.secondNumber, nowPotIndex);
        medicineInPotList.Add(nowMedicineButton);
        //아무것도 아님이 뜨면 줄여주지 않음
        //nowMedicineButton.medicineQuant--;
        //nowMedicineButton.quantityText.text = nowMedicineButton.medicineQuant.ToString();
        //nowMedicineButton.propertyQuantityText.text = nowMedicineButton.medicineQuant.ToString();


        //nowMedicineButton.potMedicineObject = inst;
        potMedicineObjectList.Add(inst);
        inst.SetActive(true);
        inst.transform.localPosition = Vector3.zero;

        if (medicineInPotList.Count >= 1)
        {
            cookButtonObject.SetActive(true);
        }
        //if (nowMedicineButton.medicineClass.GetFirstSymptom() != Symptom.special)
        //{
        //    Debug.Log("뭐야");
        //    symptomChartManager.ChangeSymptomChartText();
        //}
        if (medicineIndex == frostItemIndex)
        {
            frostAdded = true;
            roomCounterManager.GlowNextDialog("AddFrostGlow");
        }
        if (medicineIndex == desireItemIndex)
        {
            desireAdded = true;
            roomCounterManager.GlowNextDialog("AddDesireGlow");
        }



    }
    

    public void PropertyListMainButton(int index)
    {
        if (roomCounterManager.isGlowing[(int)ActionKeyword.FireIconGlow] == false)
        {
            return;
        }
        ArrangeMainButton(index);
        roomCounterManager.GlowNextDialog("FireIconGlow");


    }

    public void ArrangeMainButton(int index)
    {
        int buttonQuantity = 0;
        if (isMainButtonOn[index] == true)
        {
            //propertyMainButtonImageArray[index].gameObject.SetActive(false);
            //propertySubButtonLockArray[index].SetActive(false);
            //propertySubButtonComponentArray[index].interactable = false;
            //propertySubListParent.SetActive(false);
            //isMainButtonOn[index] = false;
            //for (int i = 0; i < wholeMedicineButtonList.Count; i++)
            //{
            //    wholeMedicineButtonList[i].isActive = false;
            //}
        }
        else
        {
            propertyMainButtonImageArray[index].gameObject.SetActive(true);
            propertySubListParent.SetActive(true);
            isMainButtonOn[index] = true;
            for (int i = 0; i < isSubButtonOn.Length; i++)
            {
                if (isSubButtonOn[i])
                {
                    isSubButtonOn[i] = false;
                    propertySubButtonImageArray[i].color = Color.white;
                }
            }
            for (int i = 0; i < isMainButtonOn.Length; i++)
            {
                if (index == i)
                {
                    continue;
                }
                if (isMainButtonOn[i])
                {
                    propertySubButtonLockArray[i].SetActive(false);
                    propertySubButtonComponentArray[i].interactable = true;
                    isMainButtonOn[i] = false;
                    propertyMainButtonImageArray[i].gameObject.SetActive(false);
                }
            }
            propertySubButtonLockArray[index].SetActive(true);
            propertySubButtonComponentArray[index].interactable = false;
            for (int i = 0; i < wholeMedicineButtonList.Count; i++)
            {
                //if (wholeMedicineButtonList[i].zeroMedicine)
                //{
                //    wholeMedicineButtonList[i].isActive = false;
                //    continue;
                //}
                if (isMainButtonOn[(int)wholeMedicineButtonList[i].medicineClass.GetSecondSymptom()])
                {
                    wholeMedicineButtonList[i].isActive = true;
                }
                else
                {
                    wholeMedicineButtonList[i].isActive = false;
                }
            }
        }

        for (int i = 0; i < wholeMedicineButtonList.Count; i++)
        {
            //if (wholeMedicineButtonList[i].medicineClass.GetFirstSymptom() == Symptom.special && !isSpecialMedicine)
            //{
            //    wholeMedicineButtonList[i].isActive = false;
            //}
            if (wholeMedicineButtonList[i].isActive)
            {
                wholeMedicineButtonList[i].buttonObject.SetActive(true);
                wholeMedicineButtonList[i].buttonRect.anchoredPosition = new Vector2(0, -116 - buttonQuantity * 186);
                buttonQuantity++;
            }
            if (wholeMedicineButtonList[i].isActive == false)
            {
                wholeMedicineButtonList[i].propertyObject.SetActive(false);
                wholeMedicineButtonList[i].buttonObject.SetActive(false);
            }
        }

        regularScrollContent.sizeDelta = new Vector2(0, 186 * buttonQuantity);
    }

    public void PropertyListSubButton(int index)
    {
        if(roomCounterManager.isGlowing[(int)ActionKeyword.WaterSubIconGlow] == false)
        {
            return;
        }
        int buttonQuantity = 0;
        if (isSubButtonOn[index] == true)
        {
            propertySubButtonImageArray[index].color = Color.white;

            isSubButtonOn[index] = false;
            for (int i = 0; i < wholeMedicineButtonList.Count; i++)
            {
                wholeMedicineButtonList[i].isActive = true;
            }

            for (int i = 0; i < wholeMedicineButtonList.Count; i++)
            {
                //if (wholeMedicineButtonList[i].zeroMedicine)
                //{
                //    wholeMedicineButtonList[i].isActive = false;
                //    continue;
                //}
                if (isMainButtonOn[(int)wholeMedicineButtonList[i].medicineClass.GetSecondSymptom()])
                {
                    wholeMedicineButtonList[i].isActive = true;
                }
                else
                {
                    wholeMedicineButtonList[i].isActive = false;
                }
            }
        }
        else
        {
            propertySubButtonImageArray[index].color = Color.red;

            isSubButtonOn[index] = true;
            for (int i = 0; i < isSubButtonOn.Length; i++)
            {
                if (index == i)
                {
                    continue;
                }
                if (isSubButtonOn[i])
                {
                    isSubButtonOn[i] = false;
                    propertySubButtonImageArray[i].color = Color.white;
                }
            }
            for (int i = 0; i < wholeMedicineButtonList.Count; i++)
            {
                //if (wholeMedicineButtonList[i].zeroMedicine)
                //{
                //    wholeMedicineButtonList[i].isActive = false;
                //    continue;
                //}
                if (isMainButtonOn[(int)wholeMedicineButtonList[i].medicineClass.GetSecondSymptom()] && isSubButtonOn[(int)wholeMedicineButtonList[i].medicineClass.GetFirstSymptom()])
                {
                    wholeMedicineButtonList[i].isActive = true;
                }
                else
                {
                    wholeMedicineButtonList[i].isActive = false;
                }
            }
        }

        for (int i = 0; i < wholeMedicineButtonList.Count; i++)
        {
            //if (wholeMedicineButtonList[i].medicineClass.GetFirstSymptom() == Symptom.special && !isSpecialMedicine)
            //{
            //    wholeMedicineButtonList[i].isActive = false;
            //}
            if (wholeMedicineButtonList[i].isActive)
            {
                wholeMedicineButtonList[i].buttonObject.SetActive(true);
                wholeMedicineButtonList[i].buttonRect.anchoredPosition = new Vector2(0, -116 - buttonQuantity * 186);
                buttonQuantity++;
            }
            if (wholeMedicineButtonList[i].isActive == false)
            {
                wholeMedicineButtonList[i].propertyObject.SetActive(false);
                wholeMedicineButtonList[i].buttonObject.SetActive(false);
            }
        }

        regularScrollContent.sizeDelta = new Vector2(0, 186 * buttonQuantity);
        roomCounterManager.GlowNextDialog("WaterSubIconGlow");
    }


    int nowButtonIndex = -1;
    bool dragged = false;
    //여기서 인덱스는 버튼의 인덱스다. wholeButton의 인덱스 메디슨의 인덱스가아님
    //버튼을 드래그해서 팟에 넣는거.
    void OnButtonDrag(PointerEventData data, int index)
    {
        bool desire = roomCounterManager.isGlowing[(int)ActionKeyword.AddDesireGlow];
        bool frost = roomCounterManager.isGlowing[(int)ActionKeyword.AddFrostGlow];
        int medicineIndex = wholeMedicineButtonList[index].medicineIndex;

        if (desire == false && frost == false)
        {
            return;
        }
        if (desire == true && medicineIndex == frostItemIndex)
        {
            return;
        }
        if (frost == true && medicineIndex == desireItemIndex)
        {
            return;
        }
        if (isPotCooked)
        {
            return;
        }
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
        wholeMedicineButtonList[index].medicineObject.SetActive(true);
        wholeMedicineButtonList[index].medicineObject.transform.position = new Vector3(mousePos.x, mousePos.y, -8);

        nowButtonIndex = index;
        dragged = true;

    }

    //드래그하고서 클릭 뗐을 때
    void OnButtonUp()
    {
        if (roomCounterManager.isGlowing[(int)ActionKeyword.AddDesireGlow] == false
           && roomCounterManager.isGlowing[(int)ActionKeyword.AddFrostGlow] == false)
        {
            return;
        }
        if (isPotCooked)
        {
            return;
        }

        if (nowButtonIndex != -1 && dragged == true)
        {
            wholeMedicineButtonList[nowButtonIndex].medicineObject.SetActive(false);
        }
        else
        {
            return;
        }

        //nowButtonIndex = -1;
        dragged = false;
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
        if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
        {
            touchedObject = hit.collider.gameObject;
            //Ray에 맞은 콜라이더를 터치된 오브젝트로 설정
            if (touchedObject.CompareTag("Pot"))
            {
                AddMedicineToPot();
            }
        }
    }




    //약재 하나 버튼 클릭했을 때
    public void OnButtonDown(int index)
    {
        bool choose = roomCounterManager.isGlowing[(int)ActionKeyword.ItemForceChoose];
        bool desire = roomCounterManager.isGlowing[(int)ActionKeyword.AddDesireGlow];
        bool frost = roomCounterManager.isGlowing[(int)ActionKeyword.AddFrostGlow];
        int medicineIndex = wholeMedicineButtonList[index].medicineIndex;
        if (choose == true && medicineIndex == frostItemIndex)
        {
            return;
        }

        if (choose == true || desire == true || frost == true)
        {
            if (desire == true && medicineIndex == frostItemIndex)
            {
                return;
            }
            if (frost == true && medicineIndex == desireItemIndex)
            {
                return;
            }

            if (doubleClickIndex == index)
            {
                nowButtonIndex = index;
                AddMedicineToPot();
            }
            if (nowButtonIndex == index)
            {
                wholeMedicineButtonList[nowButtonIndex].propertyObject.SetActive(false);
                wholeMedicineButtonList[nowButtonIndex].medicineNameText.fontStyle = FontStyle.Normal;
                wholeMedicineButtonList[nowButtonIndex].buttonObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);

                nowButtonIndex = -1;
            }
            else
            {
                wholeMedicineButtonList[index].propertyObject.SetActive(true);
                wholeMedicineButtonList[index].medicineNameText.fontStyle = FontStyle.Bold;
                wholeMedicineButtonList[index].buttonObject.GetComponent<Image>().color = Color.white;
                if (nowButtonIndex != -1)
                {
                    wholeMedicineButtonList[nowButtonIndex].propertyObject.SetActive(false);
                    wholeMedicineButtonList[nowButtonIndex].medicineNameText.fontStyle = FontStyle.Normal;
                    wholeMedicineButtonList[nowButtonIndex].buttonObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }
                nowButtonIndex = index;
            }
            doubleClickIndex = index;
            if (choose == true)
            {
                roomCounterManager.GlowNextDialog("ItemForceChoose");
            }

        }


    }


    IEnumerator CookAnimationCoroutine()
    {
        potAnimationManager.PotWorldAnimation(true);
        yield return new WaitForSeconds(0.8f);
        potAnimationManager.PotUIAnimation(true);
        potAnimationManager.PotDropAnimation();
        yield return new WaitForSeconds(1.4f);

        for (int i = 0; i < potMedicineObjectList.Count; i++)
        {
            potMedicineObjectList[i].SetActive(false);
        }
        potAnimationManager.PotWorldAnimation(false);
        yield return new WaitForSeconds(3f);
        CookFunction();
        potAnimationManager.PotUIAnimation(false);
        nowCookingAnimation = false;
        roomCounterManager.GlowNextDialog("CookButtonClick");
    }

    void CookFunction()
    {
        cookedMedicineText.gameObject.SetActive(true);
        int medicineCount = medicineInPotList.Count;
        int[] indexArray = new int[medicineCount];
        bool[] noneMedicineArray = new bool[medicineCount];

        for (int i = 0; i < 3; i++)
        {
            potAnimationManager.UnSetPotColor(i, false);
        }

        cookedMedicine = new CookedMedicine();
        cookedMedicine.medicineCount = medicineCount;
        for (int i = 0; i < medicineCount; i++)
        {
            //medicineInPotList[i].medicineQuant--;
            //medicineInPotList[i].owningMedicine.medicineQuantity--;
            if (medicineInPotList[i].medicineClass.GetFirstSymptom() == Symptom.special)
            {
                cookedMedicine.specialArray[i] = true;
            }
            else
            {
                cookedMedicine.specialArray[i] = false;
            }
            indexArray[i] = medicineInPotList[i].medicineIndex;
        }
        cookedMedicine.medicineArray = indexArray;

        StringBuilder builder;

        if (medicineCount == 1)
        {
            builder = new StringBuilder(medicineInPotList[0].medicineClass.firstName);
            builder.Append(" ");
            builder.Append(medicineInPotList[0].medicineClass.secondName);
        }
        else if (medicineCount == 2)
        {
            builder = new StringBuilder(medicineInPotList[0].medicineClass.firstName);
            builder.Append(" ");
            builder.Append(medicineInPotList[1].medicineClass.secondName);
        }
        else
        {
            builder = new StringBuilder(medicineInPotList[0].medicineClass.firstName);
            builder.Append(" ");
            builder.Append(medicineInPotList[1].medicineClass.firstName);
            builder.Append(" ");
            builder.Append(medicineInPotList[2].medicineClass.secondName);
        }
        cookedMedicine.name = builder.ToString();
        //더미임
        cookedMedicineText.text = cookedMedicine.name;

        for (int i = 0; i < medicineCount; i++)
        {
            potMedicineObjectList[i].SetActive(false);
        }

        cookedMedicinePrefab.SetActive(true);
        cookedMedicine.medicineObject = cookedMedicinePrefab;
        cookedMedicineManager.CookedMedicineManagerSetting(cookedMedicine);
        for (int i = 0; i < medicineInPotList.Count; i++)
        {

            OwningMedicineClass owningMedicine = medicineInPotList[i].owningMedicine;
            if (!owningMedicineList.Contains(owningMedicine))
            {
                continue;
            }


        }
        for (int i = 0; i < 5; i++)
        {
            if (isMainButtonOn[i] == true)
            {
                ArrangeMainButton(i);
            }
        }

        medicineInPotList.Clear();
        potMedicineObjectList.Clear();

    }

    //아시발 코드길어짅다;;;;;;
    //끓인다 버튼 누를 때.
    public void OnCookButton()
    {
        if (isPotCooked == true)
        {
            return;
        }
        if (roomCounterManager.isGlowing[(int)ActionKeyword.CookButtonClick] == false)
        {
            return;
        }
        isPotCooked = true;
        cookButtonObject.SetActive(false);
        nowCookingAnimation = true;
        StartCoroutine(CookAnimationCoroutine());

    }

    public void CookedMedicineRemoved()
    {
        isPotCooked = false;
        cookedMedicine = null;
        cookedMedicineText.gameObject.SetActive(false);

    }

    public void UseCoin(int cost)
    {
        counterManager.CoinTextChange();
        usedCoinText.text = "-" + cost.ToString();
        usedCoinText.color = new Color(1, 1, 1, 1);
        StartCoroutine(SceneManager.inst.FadeModule_Text(usedCoinText, 1, 0, 2));
    }
}
