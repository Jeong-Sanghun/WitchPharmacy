using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;


//조제실에서 약재버튼 생성해주고 그걸로 조합해서 카운터매니저로 넘길거임.
public class RoomManager : MonoBehaviour    //SH
{

    [SerializeField]
    CounterManager counterManager;
    [SerializeField]
    CookedMedicineManager cookedMedicineManager;
    GameManager gameManager;
    SaveDataClass saveData;
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

    Vector3 cameraRoomPos;  //카메라 포지션 결정해줘야돼서. 화면바꾸기
    Vector3 cameraCounterPos;
    [SerializeField]
    GameObject roomUICanvas;    //카운터로 옮겨갈때 이거 바꿔줘야해.
    [SerializeField]
    GameObject counterUICanvas;
    

    //현재 팟 안에 들어가있는 메디슨의 리스트. 이게 존나 중요.
    [SerializeField]
    List<MedicineButton> medicineInPotList;

    //약재를 다 끓여서 만들었는지
    //counterManager에서 받아올거임. 쿡한상태에서 간거하고 안하고 간거랑 다를테니까
    //countermanager에서 이 값을 변화시켜줌.
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
    public bool nowInRoom;

    [SerializeField]
    Text[] symptomChartTextArray;
    [SerializeField]
    GameObject symptomChartObject;
    RandomVisitorClass nowVisitor;

    //증상기록창에서 측정도구로 측정한 증상은 토글이 고정이 되어야함
    //그래서 어떤 걸 측정도구로 측정했는지 알아야함.
    public bool[] symptomMeasuredArray;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        medicineDataList = gameManager.medicineDataWrapper.medicineDataList;
        ownedMedicineList = saveData.ownedMedicineList;
        owningMedicineList = saveData.owningMedicineList;
        isButtonOn = new bool[6];
        for (int i = 0; i < isButtonOn.Length; i++)
        {
            isButtonOn[i] = false;
        }
        wholeMedicineButtonList = new List<MedicineButton>();
        medicineInPotList = new List<MedicineButton>();
        potMedicineObjectList = new List<GameObject>();

        isPotCooked = false;
        cookButtonObject.SetActive(false);

        cameraRoomPos = new Vector3(51.2f, 0, -10);
        cameraCounterPos = new Vector3(0, 0, -10);
        cookedMedicineCounterPos = new Vector3(-100, -555, 0);

        nowInRoom = false;

        symptomMeasuredArray = new bool[5];
        for(int i = 0; i < 5; i++)
        {
            symptomMeasuredArray[i] = false;
        }



        int buttonIndex = 0;
        for (int i = 0; i < ownedMedicineList.Count; i++)
        {
            //내가 가졌던거중에 없으면 컨티뉴
            int index = ownedMedicineList[i];
            OwningMedicineClass owningMedicine = null;
            for(int j = 0; j < owningMedicineList.Count; j++)
            {
                if(owningMedicineList[j].medicineIndex == index)
                {
                    owningMedicine = owningMedicineList[j];
                    break;
                }
                
            }
            if(owningMedicine == null)
            {
                continue;
            }
            int quantity = owningMedicine.medicineQuantity;
            MedicineClass medicine = medicineDataList[index];
            StringBuilder nameBuilder = new StringBuilder(medicine.firstName);
            nameBuilder.Append(" ");
            nameBuilder.Append(medicine.secondName);
            if (medicine.medicineImage == null)
            {
                StringBuilder builder = new StringBuilder("Items/");
                builder.Append(nameBuilder.ToString());
                medicine.medicineImage = Resources.Load<Sprite>(builder.ToString());
            }
            prefabButtonIcon.sprite = medicine.medicineImage;
            prefabButtonName.text = nameBuilder.ToString();
            if(medicine.firstSymptom == Symptom.none)
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
            medicineObj.GetComponent<SpriteRenderer>().sprite = medicine.medicineImage;
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

    }

    //약재 떨어뜨릴 때 약재를 꺼줘야해서.
    public void Update()
    {
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
                    int listIndex = touchedObject.transform.GetSiblingIndex();
                    if(medicineInPotList.Count > listIndex)
                    {
                        MedicineButton medicine = medicineInPotList[listIndex];
                        medicine.medicineQuant++;
                        medicine.quantityText.text = medicine.medicineQuant.ToString();
                        medicine.propertyQuantityText.text = medicine.medicineQuant.ToString();
                        potMedicineObjectList[listIndex].SetActive(false);

                        if(medicineInPotList.Count == 2)
                        {
                            if(listIndex == 0)
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
                            else if(listIndex== 1)
                            {
                                potMedicineObjectList[2].transform.SetParent(potMedicineParentArray[1].transform);
                                potMedicineObjectList[2].transform.localPosition = Vector3.zero;
                            }
                        }
                        potMedicineObjectList.RemoveAt(listIndex);
                        medicineInPotList.RemoveAt(listIndex);
                        ChangeSymptomChartText();

                        if (medicineInPotList.Count < 3)
                        {
                            cookButtonObject.SetActive(false);
                        }


                    }
                }
            }
        }
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
            for (int i = 0; i < 6; i++)
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
                if(pushedButton == 0)
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
            for(int i = 0; i < 6; i++)
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

    int nowButtonIndex = -1;
    bool dragged = false;
    //여기서 인덱스는 버튼의 인덱스다. wholeButton의 인덱스 메디슨의 인덱스가아님
    //버튼을 드래그해서 팟에 넣는거.
    void OnButtonDrag(PointerEventData data, int index)
    {
        if(wholeMedicineButtonList[index].medicineQuant <= 0 || isPotCooked)
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
        int buttonIndex = nowButtonIndex;
        //nowButtonIndex = -1;
        dragged = false;
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
        if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
        {
            touchedObject = hit.collider.gameObject;
            //Ray에 맞은 콜라이더를 터치된 오브젝트로 설정
            if (touchedObject.CompareTag("Pot"))
            {
                MedicineButton nowMedicineButton = wholeMedicineButtonList[buttonIndex];
                int nowPotIndex = medicineInPotList.Count;
                if (nowPotIndex >= 3)
                {
                    Debug.Log("자리가 없어요");
                    return;
                }
                
                medicineInPotList.Add(nowMedicineButton);
                //아무것도 아님이 뜨면 줄여주지 않음
                if (nowMedicineButton.medicineClass.firstSymptom != Symptom.none)
                {
                    nowMedicineButton.medicineQuant--;
                    nowMedicineButton.quantityText.text = nowMedicineButton.medicineQuant.ToString();
                }
                nowMedicineButton.propertyQuantityText.text = nowMedicineButton.medicineQuant.ToString();
                GameObject medicineObj = nowMedicineButton.medicineObject;
                GameObject inst = Instantiate(medicineObj, potMedicineParentArray[nowPotIndex].transform);
                //nowMedicineButton.potMedicineObject = inst;
                potMedicineObjectList.Add(inst);
                inst.SetActive(true);
                inst.transform.localPosition = Vector3.zero;

                if(medicineInPotList.Count >=3)
                {
                    cookButtonObject.SetActive(true);
                }

                ChangeSymptomChartText();

            }
        }
    }

    //버튼 클릭했을 때
    public void OnButtonDown(int index)
    {
        wholeMedicineButtonList[index].propertyObject.SetActive(true);
        wholeMedicineButtonList[index].buttonObject.GetComponent<Image>().color = Color.grey;
        if(nowButtonIndex != -1)
        {
            wholeMedicineButtonList[nowButtonIndex].propertyObject.SetActive(false);
            wholeMedicineButtonList[nowButtonIndex].buttonObject.GetComponent<Image>().color = Color.white;
        }

        if(nowButtonIndex == index)
        {
            nowButtonIndex = -1;
        }
        else
        {
            nowButtonIndex = index;
        }

        dragged = false;

    }

    //아시발 코드길어짅다;;;;;;
    //끓인다 버튼 누를 때.
    public void OnCookButton()
    {
        if(isPotCooked == true)
        {
            return;
        }
        int[] indexArray = new int[3];
        bool[] noneMedicineArray = new bool[3];
        int noneMedicineNumber = 0;
        cookButtonObject.SetActive(false);
        cookedMedicine = new CookedMedicine();
        for (int i = 0; i < 3; i++)
        {
            medicineInPotList[i].owningMedicine.medicineQuantity--;
            indexArray[i] = medicineInPotList[i].medicineIndex;
            if(medicineInPotList[i].medicineClass.firstSymptom == Symptom.none)
            {
                noneMedicineArray[i] = true;
                noneMedicineNumber++;
            
            }
            else
            {
                noneMedicineArray[i] = false;
            }
        }
        cookedMedicine.medicineArray = indexArray;

        StringBuilder builder;
        if(noneMedicineNumber >= 2)
        {
            int trueIndex = -1;
            for(int i = 0; i < 3; i++)
            {
                if (!noneMedicineArray[i])
                {
                    trueIndex = i;
                    break;
                }
            }
            if(trueIndex == -1)
            {
                builder = new StringBuilder(medicineInPotList[0].medicineClass.firstName);
                builder.Append(" ");
                builder.Append(medicineInPotList[0].medicineClass.secondName);
            }
            else
            {
                builder = new StringBuilder(medicineInPotList[trueIndex].medicineClass.firstName);
                builder.Append(" ");
                builder.Append(medicineInPotList[trueIndex].medicineClass.secondName);
            }
        }
        else if(noneMedicineNumber == 1)
        {
            int noneIndex = -1;
            for (int i = 0; i < 3; i++)
            {
                if (noneMedicineArray[i])
                {
                    noneIndex = i;
                    break;
                }
            }

            if(noneIndex == 0)
            {
                builder = new StringBuilder(medicineInPotList[1].medicineClass.firstName);
                builder.Append(" ");
                builder.Append(medicineInPotList[2].medicineClass.secondName);
            }
            else if(noneIndex == 1)
            {
                builder = new StringBuilder(medicineInPotList[0].medicineClass.firstName);
                builder.Append(" ");
                builder.Append(medicineInPotList[2].medicineClass.secondName);
            }
            else
            {
                builder = new StringBuilder(medicineInPotList[0].medicineClass.firstName);
                builder.Append(" ");
                builder.Append(medicineInPotList[1].medicineClass.secondName);
            }
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

        for(int i = 0; i < 3; i++)
        {
            potMedicineObjectList[i].SetActive(false);
        }

        cookedMedicinePrefab.SetActive(true);
        cookedMedicine.medicineObject = cookedMedicinePrefab;
        cookedMedicineManager.CookedMedicineManagerSetting(cookedMedicine);
        for(int i = 0; i < medicineInPotList.Count; i++)
        {
            if(medicineInPotList[i].medicineClass.firstSymptom == Symptom.none)
            {
                continue;
            }
            OwningMedicineClass owningMedicine = medicineInPotList[i].owningMedicine;
            owningMedicine.medicineQuantity = medicineInPotList[i].medicineQuant;
            if (owningMedicine.medicineQuantity <=0)
            {
                owningMedicineList.Remove(owningMedicine);
                medicineInPotList[i].zeroMedicine = true;
            }
            
        }
        for(int i = 0; i < 6; i++)
        {
            if(isButtonOn[i] == true)
            {
                PropertyListButton(i);
            }
        }
        
        medicineInPotList.Clear();
        potMedicineObjectList.Clear();
        isPotCooked = true;

    }

    //쿡드매니저에서 불러옴. 버려지면, 혹은 손님한테 낸다면.
    public void CookedMedicineRemoved()
    {
        isPotCooked = false;
        cookedMedicine = null;
    }

    //CookedMedicineManager에서 약을 트레이로 올릴 때도 사용함. 버튼에서도 사용함.
    //룸이 움직일 때 카메라 옮겨줌
    public void ToCounterButton(bool isMedicineOnTray)
    {
        //버튼에서 호출할때는 false, 트레이에올려서 호출할때는 true
        nowInRoom = false;
        cam.transform.position = cameraCounterPos;
        roomUICanvas.SetActive(false);
        counterUICanvas.SetActive(true);
        if (isPotCooked)
        {
            if (isMedicineOnTray == false)
            {
                medicineObjectPrefab.SetActive(false);
            }
            
        }
    }
    
    //이거 카운터매니저에 넣을려 했는데 카운터매니저에서 변수선언하기 귀찮음.
    //카운터에서 조제실 오는 버튼.
    public void ToRoomButton()
    {
        nowInRoom = true;
        cam.transform.position = cameraRoomPos;
        roomUICanvas.SetActive(true);
        counterUICanvas.SetActive(false);
        if (isPotCooked)
        {
            //이거 손님한테 주고 돌아올 때 새롭게해줘야함.
            medicineObjectPrefab.SetActive(true);
        }
    }

    //증상기록 켜는 버튼, 끄는 버튼에서 여는거.
    public void SymptomChartButton(bool turnOn)
    {
        if (turnOn)
        {
            symptomChartObject.SetActive(true);
        }
        else
        {
            symptomChartObject.SetActive(false);
        }
    }

    //이거 카운터매니저에서 넘겨주는거
    public void VisitorVisits(RandomVisitorClass visitor)
    {
        nowVisitor = visitor;
        for(int i = 0; i < 5; i++)
        {
            symptomMeasuredArray[i] = false;
        }
        ChangeSymptomChartText();
    }

    //차트가 변할 때 얘도 같이 변해야한다. counterManager의 symptomCheckToggle에서 호출
    public void ChangeSymptomChartText()
    {
        if(nowVisitor == null)
        {
            return;
        }
        int[] array = new int[7];
        for(int i = 0; i< 6; i++)
        {
            if (i == 5)
            {
                array[i] = counterManager.symptomCheckArray[i];
            }
            else
            {
                if (symptomMeasuredArray[i] == true)
                {
                    array[i] = nowVisitor.symptomAmountArray[i];
                }
                else
                {
                    array[i] = counterManager.symptomCheckArray[i];
                }
            }

            
        }
        for(int i = 0; i < medicineInPotList.Count; i++)
        {
            int firstSymtpom = (int)medicineInPotList[i].medicineClass.firstSymptom;
            array[firstSymtpom] += medicineInPotList[i].medicineClass.firstNumber;

            int secondSymtpom = (int)medicineInPotList[i].medicineClass.secondSymptom;
            array[secondSymtpom] += medicineInPotList[i].medicineClass.secondNumber;
        }

        for (int i = 0; i < 6; i++)
        {
            if(counterManager.symptomCheckedArray[i])
            {
                symptomChartTextArray[i].text = array[i].ToString();
            }
            else
            {
                symptomChartTextArray[i].text = "???";
            }
            


        }

    }

    //이거 measureTool에서 불러옴. 오버라이드 된 그거.
    public void SymptomMeasured(int index)
    {
        symptomMeasuredArray[index] = true;
        ChangeSymptomChartText();
    }
}
