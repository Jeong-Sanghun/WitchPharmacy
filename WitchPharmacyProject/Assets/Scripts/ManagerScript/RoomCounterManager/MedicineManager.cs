using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;


//조제실에서 약재버튼 생성해주고 그걸로 조합해서 카운터매니저로 넘길거임.
public class MedicineManager : MonoBehaviour    //SH
{

    [SerializeField]
    CounterManager counterManager;
    [SerializeField]
    CookedMedicineManager cookedMedicineManager;
    [SerializeField]
    CounterDialogManager counterDialogManager;
    [SerializeField]
    SymptomChartManager symptomChartManager;

    GameManager gameManager;
    SaveDataClass saveData;
    List<MedicineClass> medicineDataList;
    List<SpecialMedicineClass> specialMedicineDataList;
    List<int> ownedMedicineList;
    List<OwningMedicineClass> owningMedicineList;
    List<OwningMedicineClass> owningSpecialMedicineList;
    //Dictionary<int, int> owningMedicineDictionary;
    //List<CookedMedicineData> cookedMedicineDataList;
    //위는 기본적인 매니저들 그리고 데이터들

    //스크롤 뷰에 들어가있는 content들 위아래 길이조정 해줘야함.
    //버튼도 이 아래에 생성할거여서 따로 드래그앤드롭 해줘야함. GetChild로 받아왔는데 순서꼬이면 귀찮아짐.
    [SerializeField]
    RectTransform regularScrollContent;
    [SerializeField]
    RectTransform specialScrollContent;
    [SerializeField]
    GameObject regularScrollUIParent;
    [SerializeField]
    GameObject specialScrollUIParent;

    [SerializeField]
    GameObject scrollObject;

    [SerializeField]
    Image[] propertyButtonImageArray;

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
    Text prefabButtonFirstEffectIcon;
    [SerializeField]
    Text prefabButtonSecondEffectIcon;
    [SerializeField]
    Text prefabButtonFirstEffectNumber;
    [SerializeField]
    Text prefabButtonSecondEffectNumber;

    [SerializeField]
    GameObject medicineInstanceParent;


    List<MedicineButton> wholeMedicineButtonList;
    List<MedicineButton> wholeSpecialMedicineButtonList;
    //int[] contentButtonQuantityArray;
    bool[] isButtonOn;


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
    Vector3 cookedMedicineCounterPos;

    //현재 조제실에 있는가.
    [HideInInspector]
    public bool nowInRoom;

    RandomVisitorClass nowVisitor;
    [SerializeField]
    GameObject binObject;


    //증상기록창에서 측정도구로 측정한 증상은 토글이 고정이 되어야함
    //그래서 어떤 걸 측정도구로 측정했는지 알아야함.
    bool isDraggingMedicineFromPot;
    bool isSpecialMedicine;

    //룸매니저에서 투 카운터버튼에서 사용
    public bool nowCookingAnimation;

    [SerializeField]
    Animator cookAnimator;
    float doubleClickTimer;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        medicineDataList = gameManager.medicineDataWrapper.medicineDataList;
        ownedMedicineList = saveData.ownedMedicineList;
        owningMedicineList = saveData.owningMedicineList;
        owningSpecialMedicineList = saveData.owningSpecialMedicineList;
        specialMedicineDataList = gameManager.specialMedicineDataWrapper.specialMedicineDataList;
        isDraggingMedicineFromPot = false;
        isButtonOn = new bool[5];
        for (int i = 0; i < isButtonOn.Length; i++)
        {
            isButtonOn[i] = false;
        }
        wholeMedicineButtonList = new List<MedicineButton>();
        medicineInPotList = new List<MedicineButton>();
        potMedicineObjectList = new List<GameObject>();
        wholeSpecialMedicineButtonList = new List<MedicineButton>();

        isPotCooked = false;
        cookButtonObject.SetActive(false);


        cookedMedicineCounterPos = new Vector3(-100, -555, 0);

        nowInRoom = false;
        doubleClickTimer = 0;



        int buttonIndex = 0;
        for (int i = 0; i < owningMedicineList.Count; i++)
        {
            //내가 가졌던거중에 없으면 컨티뉴
            int index = owningMedicineList[i].medicineIndex;
            OwningMedicineClass owningMedicine = owningMedicineList[i];
            int quantity = owningMedicine.medicineQuantity;
            if(quantity == 0)
            {
                continue;
            }
            MedicineClass medicine = medicineDataList[index];
            StringBuilder nameBuilder = new StringBuilder(medicine.firstName);
            nameBuilder.Append(" ");
            nameBuilder.Append(medicine.secondName);

            prefabButtonIcon.sprite = medicine.LoadImage();
            prefabButtonName.text = nameBuilder.ToString();
            if(medicine.GetFirstSymptom() == Symptom.none)
            {
                prefabButtonQuantity.text = null;
            }
            else
            {
                prefabButtonQuantity.text = quantity.ToString();
            }
            
            prefabButtonFirstEffectIcon.text = medicine.GetFirstSymptom().ToString();
            prefabButtonSecondEffectIcon.text = medicine.GetSecondSymptom().ToString();
            prefabButtonFirstEffectNumber.text = medicine.firstNumber.ToString();
            prefabButtonSecondEffectNumber.text = medicine.secondNumber.ToString();
            //버튼 세팅 다 해서 instantiate하고 리스트에 넣어줌.
            //이제 이 리스트에서 active하고 위치바꿔주고 그럴거임.
            GameObject buttonObject = Instantiate(medicineButtonPrefab, regularScrollContent.transform);
            buttonObject.SetActive(true);
            GameObject propertyObject = Instantiate(medicineButtonPrefab, propertyObjectParent);
            propertyObject.SetActive(false);
            propertyObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-450, 250);
            propertyObject.GetComponent<Image>().color = Color.grey;
            propertyObject.GetComponent<Button>().enabled = false;
            Text quantText = buttonObject.transform.GetChild(2).GetComponent<Text>();
            Text propertyQuantText = propertyObject.transform.GetChild(2).GetComponent<Text>();
            EventTrigger propertyEvent = propertyObject.AddComponent<EventTrigger>();
            GameObject medicineObj = Instantiate(medicineObjectPrefab,medicineInstanceParent.transform);
            medicineObj.SetActive(false);
            medicineObj.GetComponent<SpriteRenderer>().sprite = medicine.LoadImage();
            MedicineButton buttonClass =
                new MedicineButton(buttonObject, index, quantity,
                medicine, propertyObject,medicineObj,quantText,propertyQuantText);
            buttonClass.owningMedicine = owningMedicine;
            wholeMedicineButtonList.Add(buttonClass);
            //이 위까지가 프리팹들 다 설정해서 whoelMedicineButtonList에 buttonClass를 추가하는거임.
            //wholeMedicineButtonList의 index는 바로 아랫줄과 onButtonUp Down Drag함수에서 사용하니 medicineDictionary와 혼동하지 않도록 유의
            //정훈아 미안해 이거 주석 다 못적겠어. 그냥 그러려니 해. 셋업이야.

            int delegateIndex = buttonIndex;
            Button button = buttonClass.buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => OnButtonDown(delegateIndex));


            //scrollView의 각 버튼마다 index에 맞는 eventTrigger를 심어줘야함.
            Transform iconObject = buttonObject.transform.GetChild(0);
            EventTrigger buttonEvent = iconObject.GetComponent<EventTrigger>();

            //버튼 이벤트
            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.Drag;
            entry2.callback.AddListener((data) => { OnButtonDrag((PointerEventData)data, delegateIndex); });
            buttonEvent.triggers.Add(entry2);

            //속성창 이벤트
            EventTrigger.Entry entry3 = new EventTrigger.Entry();
            entry3.eventID = EventTriggerType.Drag;
            entry3.callback.AddListener((data) => { OnButtonDrag((PointerEventData)data, delegateIndex); });
            propertyEvent.triggers.Add(entry2);

            propertyEvent.transform.GetChild(0).GetComponent<EventTrigger>().enabled = false;

            buttonIndex++;
            //wholeMedicienButtonList의 index임.

        }
        buttonIndex = 0;
        for (int i = 0; i < owningSpecialMedicineList.Count; i++)
        {
            //내가 가졌던거중에 없으면 컨티뉴
            int index = owningSpecialMedicineList[i].medicineIndex;
            OwningMedicineClass owningMedicine = owningSpecialMedicineList[i];
            int quantity = owningMedicine.medicineQuantity;
            if (quantity == 0)
            {
                continue;
            }
            SpecialMedicineClass medicine = specialMedicineDataList[index];
            StringBuilder nameBuilder = new StringBuilder(medicine.firstName);
            nameBuilder.Append(" ");
            nameBuilder.Append(medicine.secondName);

            prefabButtonIcon.sprite = medicine.LoadImage();
            prefabButtonName.text = nameBuilder.ToString();
            prefabButtonQuantity.text = quantity.ToString();

            prefabButtonFirstEffectIcon.text = null;
            prefabButtonSecondEffectIcon.text = null;
            prefabButtonFirstEffectNumber.text = null;
            prefabButtonSecondEffectNumber.text = null;
            //버튼 세팅 다 해서 instantiate하고 리스트에 넣어줌.
            //이제 이 리스트에서 active하고 위치바꿔주고 그럴거임.
            GameObject buttonObject = Instantiate(medicineButtonPrefab, specialScrollContent.transform);
            buttonObject.SetActive(true);
            GameObject propertyObject = Instantiate(medicineButtonPrefab, propertyObjectParent);
            propertyObject.SetActive(false);
            propertyObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-450, 250);
            propertyObject.GetComponent<Image>().color = Color.grey;
            propertyObject.GetComponent<Button>().enabled = false;
            Text quantText = buttonObject.transform.GetChild(2).GetComponent<Text>();
            Text propertyQuantText = propertyObject.transform.GetChild(2).GetComponent<Text>();
            EventTrigger propertyEvent = propertyObject.AddComponent<EventTrigger>();
            GameObject medicineObj = Instantiate(medicineObjectPrefab, medicineInstanceParent.transform);
            medicineObj.SetActive(false);
            medicineObj.GetComponent<SpriteRenderer>().sprite = medicine.LoadImage();
            MedicineClass cast = new MedicineClass(medicine);
            MedicineButton buttonClass =
                new MedicineButton(buttonObject, index, quantity,
                 cast, propertyObject, medicineObj, quantText, propertyQuantText);
            buttonClass.owningMedicine = owningMedicine;
            buttonClass.isActive = true;
            wholeSpecialMedicineButtonList.Add(buttonClass);
            //이 위까지가 프리팹들 다 설정해서 whoelMedicineButtonList에 buttonClass를 추가하는거임.
            //wholeMedicineButtonList의 index는 바로 아랫줄과 onButtonUp Down Drag함수에서 사용하니 medicineDictionary와 혼동하지 않도록 유의
            //정훈아 미안해 이거 주석 다 못적겠어. 그냥 그러려니 해. 셋업이야.

            int delegateIndex = buttonIndex;
            Button button = buttonClass.buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => OnButtonDown(delegateIndex));


            //scrollView의 각 버튼마다 index에 맞는 eventTrigger를 심어줘야함.
            Transform iconObject = buttonObject.transform.GetChild(0);
            EventTrigger buttonEvent = iconObject.GetComponent<EventTrigger>();

            //버튼 이벤트
            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.Drag;
            entry2.callback.AddListener((data) => { OnButtonDrag((PointerEventData)data, delegateIndex); });
            buttonEvent.triggers.Add(entry2);

            //속성창 이벤트
            EventTrigger.Entry entry3 = new EventTrigger.Entry();
            entry3.eventID = EventTriggerType.Drag;
            entry3.callback.AddListener((data) => { OnButtonDrag((PointerEventData)data, delegateIndex); });
            propertyEvent.triggers.Add(entry2);

            propertyEvent.transform.GetChild(0).GetComponent<EventTrigger>().enabled = false;

            buttonIndex++;
            //wholeMedicienButtonList의 index임.

        }

        SpecialScrollAlign();
        PropertyListButton(0);
        PropertyListButton(0);

    }

    GameObject draggingObject;
    //약재 떨어뜨릴 때 약재를 꺼줘야해서.
    public void Update()
    {
        if (counterManager.endSales || counterDialogManager.nowTalking)
        {
            return;
        }
        if(nowButtonIndex != -1)
        {
            doubleClickTimer += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            //손 뗄 때 내가 약을 들고있는 상태인지, 그거를 누구 위에 올려놓았는지 판단.
            OnButtonUp();
        }
        if (Input.GetMouseButtonDown(0) && isPotCooked == false)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
            if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
            {
                touchedObject = hit.collider.gameObject;

                //Ray에 맞은 콜라이더를 터치된 오브젝트로 설정
                if (touchedObject.CompareTag("PotMedicine"))
                {
                    bool breaking = false;
                    for (int i = 0; i < medicineInPotList.Count; i++)
                    {
                        for(int j = 0; j < 3; j++)
                        {
                            if (touchedObject == potMedicineParentArray[j])
                            {
                                draggingObject = medicineInPotList[i].medicineObject;
                                draggingObject.SetActive(true);
                                touchedObject.SetActive(false);
                                breaking = true;
                                break;
                            }
                        }
                        if (breaking)
                        {
                            break;
                        }
                    }
                    isDraggingMedicineFromPot = true;
                    Debug.Log("왜안돼");
                    binObject.SetActive(true);
                }
            }
        }
        if (Input.GetMouseButton(0) && isPotCooked == false && isDraggingMedicineFromPot == true)
        {
            if(draggingObject != null)
            {
                Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                draggingObject.transform.position = new Vector3(mousePos.x, mousePos.y, touchedObject.transform.position.z);
            }
        }
        if (Input.GetMouseButtonUp(0) && isPotCooked == false && isDraggingMedicineFromPot == true)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
            if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
            {
                GameObject bin = hit.collider.gameObject;
                if (bin == binObject)
                {
                    RemoveMedicineFromPot();
                }
            }
            draggingObject.SetActive(false);
            touchedObject.SetActive(true);
            binObject.SetActive(false);
            isDraggingMedicineFromPot = false;
        }
    }

    void RemoveMedicineFromPot()
    {
        if (isSpecialMedicine)
        {
            MedicineButton medicine = medicineInPotList[0];
            potMedicineObjectList[0].SetActive(false);
            medicine.medicineQuant++;
            medicine.quantityText.text = medicine.medicineQuant.ToString();
            medicine.propertyQuantityText.text = medicine.medicineQuant.ToString();
            potMedicineObjectList.Clear();
            medicineInPotList.Clear();
            cookButtonObject.SetActive(false);
        }
        else
        {
            int listIndex = touchedObject.transform.GetSiblingIndex();
            if (medicineInPotList.Count > listIndex)
            {
                MedicineButton medicine = medicineInPotList[listIndex];
                medicine.medicineQuant++;
                medicine.quantityText.text = medicine.medicineQuant.ToString();
                medicine.propertyQuantityText.text = medicine.medicineQuant.ToString();
                potMedicineObjectList[listIndex].SetActive(false);

                if (medicineInPotList.Count == 2)
                {
                    if (listIndex == 0)
                    {
                        potMedicineObjectList[1].transform.SetParent(potMedicineParentArray[0].transform);
                        potMedicineObjectList[1].transform.localPosition = Vector3.zero;
                    }
                }
                else if (medicineInPotList.Count == 3)
                {
                    if (listIndex == 0)
                    {
                        potMedicineObjectList[1].transform.SetParent(potMedicineParentArray[0].transform);
                        potMedicineObjectList[1].transform.localPosition = Vector3.zero;
                        potMedicineObjectList[2].transform.SetParent(potMedicineParentArray[1].transform);
                        potMedicineObjectList[2].transform.localPosition = Vector3.zero;
                    }
                    else if (listIndex == 1)
                    {
                        potMedicineObjectList[2].transform.SetParent(potMedicineParentArray[1].transform);
                        potMedicineObjectList[2].transform.localPosition = Vector3.zero;
                    }
                }
                potMedicineObjectList.RemoveAt(listIndex);
                medicineInPotList.RemoveAt(listIndex);
                symptomChartManager.ChangeSymptomChartText();

                if (medicineInPotList.Count == 0)
                {
                    cookButtonObject.SetActive(false);
                }


            }
        }
       
    }

    void SpecialScrollAlign()
    {

        int buttonQuantity = 0;
        for (int i = 0; i < wholeSpecialMedicineButtonList.Count; i++)
        {
            if(wholeSpecialMedicineButtonList[i].medicineQuant == 0)
            {
                wholeSpecialMedicineButtonList[i].isActive = false;
            }
            if (wholeSpecialMedicineButtonList[i].isActive)
            {
                wholeSpecialMedicineButtonList[i].buttonObject.SetActive(true);
                wholeSpecialMedicineButtonList[i].buttonRect.anchoredPosition = new Vector2(0, -90 - buttonQuantity * 180);
                buttonQuantity++;
            }
            if (wholeSpecialMedicineButtonList[i].isActive == false)
            {
                wholeMedicineButtonList[i].buttonObject.SetActive(false);
            }
        }

        specialScrollContent.sizeDelta = new Vector2(0, 180 * buttonQuantity);
    }


    //속성 누르면 그 속성 아이템 뜨게하는 버튼
    public void PropertyListButton(int index)
    {
        if (counterManager.endSales)
        {
            return;
        }
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


                if (wholeMedicineButtonList[i].medicineQuant == 0)
                {
                    wholeMedicineButtonList[i].isActive = false;
                    continue;
                }
                if (pushedButton == 0)
                {
                    wholeMedicineButtonList[i].isActive = true;
                    continue;
                }


                wholeMedicineButtonList[i].isActive = false;
                if (pushedButton > 1)
                {
                    if (isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.GetFirstSymptom()] &&
    isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.GetSecondSymptom()])
                    {
                        wholeMedicineButtonList[i].isActive = true;
                    }
                }
                //else
                //{
                //    //                    if (isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.GetFirstSymptom()] ||
                //    //isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.GetSecondSymptom()])
                //    //                    {
                //    //                        wholeMedicineButtonList[i].isActive = true;
                //    //                    }

                //    wholeMedicineButtonList[i].isActive = true;
                //}
            }
        }
        else
        {
            propertyButtonImageArray[index].color = Color.red;

            isButtonOn[index] = true;
            int pushedButton = 0;
            for(int i = 0; i < 5; i++)
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
                //if (wholeMedicineButtonList[i].medicineClass.GetFirstSymptom() == Symptom.none)
                //{
                //    wholeMedicineButtonList[i].isActive = true;
                //    continue;
                //}
                if (pushedButton > 1)
                {
                    if (isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.GetFirstSymptom()] &&
    isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.GetSecondSymptom()])
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
                    if (isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.GetFirstSymptom()] ||
isButtonOn[(int)wholeMedicineButtonList[i].medicineClass.GetSecondSymptom()])
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

        regularScrollContent.sizeDelta = new Vector2(0, 180 * buttonQuantity);

    }

    int nowButtonIndex = -1;
    bool dragged = false;
    //여기서 인덱스는 버튼의 인덱스다. wholeButton의 인덱스 메디슨의 인덱스가아님
    //버튼을 드래그해서 팟에 넣는거.
    void OnButtonDrag(PointerEventData data, int index)
    {
        if (isSpecialMedicine)
        {
            if ((wholeSpecialMedicineButtonList[index].medicineQuant <= 0) || isPotCooked)
            {
                return;
            }
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
            wholeSpecialMedicineButtonList[index].medicineObject.SetActive(true);
            wholeSpecialMedicineButtonList[index].medicineObject.transform.position = new Vector3(mousePos.x, mousePos.y, -8);
        }
        else
        {
            if ((wholeMedicineButtonList[index].medicineQuant <= 0) || isPotCooked)
            {
                return;
            }
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
            wholeMedicineButtonList[index].medicineObject.SetActive(true);
            wholeMedicineButtonList[index].medicineObject.transform.position = new Vector3(mousePos.x, mousePos.y, -8);
        }

        nowButtonIndex = index;
        dragged = true;

    }

    //드래그하고서 클릭 뗐을 때
    void OnButtonUp()
    {
        if (isPotCooked)
        {
            return;
        }
        if (isSpecialMedicine)
        {
            if (nowButtonIndex != -1 && dragged == true)
            {
                wholeSpecialMedicineButtonList[nowButtonIndex].medicineObject.SetActive(false);
            }
            else
            {
                return;
            }

        }
        else
        {
            if (nowButtonIndex != -1 && dragged == true)
            {
                wholeMedicineButtonList[nowButtonIndex].medicineObject.SetActive(false);
            }
            else
            {
                return;
            }

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

    void AddMedicineToPot()
    {
        MedicineButton nowMedicineButton;
        GameObject inst;
        GameObject medicineObj;
        int nowPotIndex;
        nowPotIndex = medicineInPotList.Count;

        if (isSpecialMedicine)
        {
            if (nowPotIndex >= 1)
            {
                Debug.Log("자리가 없어요");
                return;
            }
            nowMedicineButton = wholeSpecialMedicineButtonList[nowButtonIndex];
            medicineObj = nowMedicineButton.medicineObject;
            inst = Instantiate(medicineObj, potMedicineParentArray[1].transform);
        }
        else
        {
            if (nowPotIndex >= 3)
            {
                Debug.Log("자리가 없어요");
                return;
            }
            nowMedicineButton = wholeMedicineButtonList[nowButtonIndex];
            medicineObj = nowMedicineButton.medicineObject;
            inst = Instantiate(medicineObj, potMedicineParentArray[nowPotIndex].transform);
        }



        medicineInPotList.Add(nowMedicineButton);
        //아무것도 아님이 뜨면 줄여주지 않음
        nowMedicineButton.medicineQuant--;
        nowMedicineButton.quantityText.text = nowMedicineButton.medicineQuant.ToString();
        nowMedicineButton.propertyQuantityText.text = nowMedicineButton.medicineQuant.ToString();

        
        //nowMedicineButton.potMedicineObject = inst;
        potMedicineObjectList.Add(inst);
        inst.SetActive(true);
        inst.transform.localPosition = Vector3.zero;

        if (medicineInPotList.Count >= 1)
        {
            cookButtonObject.SetActive(true);
        }
        if (!isSpecialMedicine)
        {
            symptomChartManager.ChangeSymptomChartText();
        }
        


    }

    

    //약재 하나 버튼 클릭했을 때
    public void OnButtonDown(int index)
    {
        if (isSpecialMedicine)
        {
            wholeSpecialMedicineButtonList[index].propertyObject.SetActive(true);
            wholeSpecialMedicineButtonList[index].buttonObject.GetComponent<Image>().color = Color.grey;
            if (nowButtonIndex != -1)
            {
                wholeSpecialMedicineButtonList[nowButtonIndex].propertyObject.SetActive(false);
                wholeSpecialMedicineButtonList[nowButtonIndex].buttonObject.GetComponent<Image>().color = Color.white;
            }
        }
        else
        {
            wholeMedicineButtonList[index].propertyObject.SetActive(true);
            wholeMedicineButtonList[index].buttonObject.GetComponent<Image>().color = Color.grey;
            if (nowButtonIndex != -1)
            {
                wholeMedicineButtonList[nowButtonIndex].propertyObject.SetActive(false);
                wholeMedicineButtonList[nowButtonIndex].buttonObject.GetComponent<Image>().color = Color.white;
            }
        }
        

        if(nowButtonIndex == index)
        {
            //따블클릭
            if(doubleClickTimer <0.5f)
                AddMedicineToPot();
            nowButtonIndex = -1;
        }
        else
        {
            doubleClickTimer = 0;
            nowButtonIndex = index;
        }

        dragged = false;

    }


    IEnumerator CookAnimationCoroutine()
    {
        cookAnimator.SetBool("isCooking", true);
        yield return new WaitForSeconds(1.5f);
        CookFunction();
        cookAnimator.SetBool("isCooking", false);
        nowCookingAnimation = false;
    }

    void CookFunction()
    {

        int medicineCount = medicineInPotList.Count;
        int[] indexArray = new int[medicineCount];
        bool[] noneMedicineArray = new bool[medicineCount];

        cookedMedicine = new CookedMedicine();
        cookedMedicine.medicineCount = medicineCount;
        for (int i = 0; i < medicineCount; i++)
        {
            medicineInPotList[i].owningMedicine.medicineQuantity--;
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
            //if(medicineInPotList[i].medicineClass.GetFirstSymptom() == Symptom.none)
            //{
            //    continue;
            //}
            OwningMedicineClass owningMedicine = medicineInPotList[i].owningMedicine;
            owningMedicine.medicineQuantity = medicineInPotList[i].medicineQuant;
            if (owningMedicine.medicineQuantity <= 0)
            {
                owningMedicineList.Remove(owningMedicine);
                medicineInPotList[i].zeroMedicine = true;
            }

        }
        for (int i = 0; i < 5; i++)
        {
            if (isButtonOn[i] == true)
            {
                PropertyListButton(i);
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
        isPotCooked = true;
        cookButtonObject.SetActive(false);
        nowCookingAnimation = true;
        StartCoroutine(CookAnimationCoroutine());

    }

    //쿡드매니저에서 불러옴. 버려지면, 혹은 손님한테 낸다면.
    public void CookedMedicineRemoved()
    {
        isPotCooked = false;
        cookedMedicine = null;
    }
    //카운터매니저에서 불러옴.
    public void VisitorVisits(RandomVisitorClass visitor)
    {
        nowVisitor = visitor;
        isSpecialMedicine = false;
        regularScrollUIParent.SetActive(true);
        specialScrollUIParent.SetActive(false);
    }
    //카운터매니저
    public void SpecialVisitorVisits()
    {
        isSpecialMedicine = true;
        regularScrollUIParent.SetActive(false);
        specialScrollUIParent.SetActive(true);
    }

    public void ToCounterButton(bool isMedicineOnTray)
    {
        if (isPotCooked)
        {
            if (isMedicineOnTray == false)
            {
                medicineObjectPrefab.SetActive(false);
            }

        }
    }

    public void ToRoomButton()
    {
        if (isPotCooked)
        {
            //이거 손님한테 주고 돌아올 때 새롭게해줘야함.
            medicineObjectPrefab.SetActive(true);
        }
    }
}
