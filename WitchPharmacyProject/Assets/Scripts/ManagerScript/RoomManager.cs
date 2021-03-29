using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;


//조제실에서 약재버튼 생성해주고 그걸로 조합해서 카운터매니저로 넘길거임.
public class RoomManager : MonoBehaviour    //SH
{
    class MedicineButton
    {

        public MedicineClass medicineClass; //아이템의 약 속성이 뭔지
        public GameObject buttonObject;     //스크롤뷰에 들어가있는 버튼 오브젝트
        public GameObject propertyObject;   //클릭하면 속성창이 떠야되는데 그 속성창 오브젝트
        public RectTransform buttonRect;    //버튼 rect 계속 getComponent해주기 귀찮아서
        public GameObject medicineObject;   //드래그앤드롭시 약재 오브젝트
        public int medicineIndex;           //약재 딕셔너리의 인덱스
        public int medicineQuant;           //약재 몇개인지
        public bool isActive;               //지금 activeSelf상태. 속성을 껐다켰다 해줘야해서.

        public MedicineButton(GameObject obj, int index, int quant, MedicineClass medicine, GameObject property, GameObject medicineObj)
        {
            medicineClass = medicine;
            buttonObject = obj;
            propertyObject = property;
            medicineObject = medicineObj;
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
    Dictionary<int, int> owningMedicineDictionary;
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


    List<MedicineButton> wholeMedicineButtonList;
    //int[] contentButtonQuantityArray;
    bool[] isButtonOn;


    //이거 드래그앤드롭할떄 필요함
    [SerializeField]
    GameObject medicineObjectPrefab;        //약재 오브젝트 프리팹
    GameObject touchedObject;               //터치한 오브젝트
    RaycastHit2D hit;                         //터치를 위한 raycastHit
    public Camera cam;                      //레이캐스트를 위한 카메라.


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
        for (int i = 0; i < isButtonOn.Length; i++)
        {
            isButtonOn[i] = false;
        }
        wholeMedicineButtonList = new List<MedicineButton>();

        int buttonIndex = 0;
        for (int i = 0; i < ownedMedicineList.Count; i++)
        {
            //내가 가졌던거중에 없으면 컨티뉴
            int index = ownedMedicineList[i];
            if (!owningMedicineDictionary.ContainsKey(index))
            {
                continue;
            }
            int quantity = owningMedicineDictionary[index];
            MedicineClass medicine = medicineDataList[index];
            if (medicine.medicineImage == null)
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
            GameObject propertyObject = Instantiate(medicineButtonPrefab, propertyObjectParent);
            propertyObject.SetActive(false);
            propertyObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-450, 250);
            propertyObject.GetComponent<Image>().color = Color.grey;
            GameObject medicineObj = Instantiate(medicineObjectPrefab);
            medicineObj.SetActive(false);
            medicineObj.GetComponent<SpriteRenderer>().sprite = medicine.medicineImage;
            MedicineButton buttonClass = new MedicineButton(buttonObject, index, quantity, medicine, propertyObject,medicineObj);
            wholeMedicineButtonList.Add(buttonClass);
            //이 위까지가 프리팹들 다 설정해서 whoelMedicineButtonList에 buttonClass를 추가하는거임.
            //wholeMedicineButtonList의 index는 바로 아랫줄과 onButtonUp Down Drag함수에서 사용하니 medicineDictionary와 혼동하지 않도록 유의

            //scrollView의 각 버튼마다 index에 맞는 eventTrigger를 심어줘야함.
            EventTrigger buttonEvent = buttonObject.GetComponent<EventTrigger>();

            int delegateIndex = buttonIndex;
            EventTrigger.Entry entry1 = new EventTrigger.Entry();
            entry1.eventID = EventTriggerType.PointerDown;
            entry1.callback.AddListener((data) => { OnButtonDown((PointerEventData)data,delegateIndex); });
            buttonEvent.triggers.Add(entry1);

            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.Drag;
            entry2.callback.AddListener((data) => { OnButtonDrag((PointerEventData)data, delegateIndex); });
            buttonEvent.triggers.Add(entry2);

            //wholeMedicienButtonList의 index임.
            buttonIndex++;
        }

        /*
        for(int i = 0; i < 6; i++)
        {
            scrollContentArray[i].sizeDelta = new Vector2(0, 180 * contentButtonQuantityArray[i]);
        }*/

    }

    //약재 떨어뜨릴 때 약재를 꺼줘야해서.
    public void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            OnButtonUp();
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
                        if (isButtonOn[j] == true && wholeMedicineButtonList[i].isActive == false)
                        {
                            if ((int)wholeMedicineButtonList[i].medicineClass.firstSymptom == j
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

    int nowButtonIndex = -1;
    bool dragged = false;
    //여기서 인덱스는 버튼의 인덱스다. wholeButton의 인덱스 메디슨의 인덱스가아님
    //버튼을 드래그해서 팟에 넣는거.
    public void OnButtonDrag(PointerEventData data, int index)
    {
        Debug.Log("Drag " + index);
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
        wholeMedicineButtonList[index].medicineObject.SetActive(true);
        wholeMedicineButtonList[index].medicineObject.transform.position = new Vector3(mousePos.x, mousePos.y, -8);
        nowButtonIndex = index;
        dragged = true;

    }

    //드래그하고서 클릭 뗐을 때
    public void OnButtonUp()
    {

        if (nowButtonIndex != -1 && dragged == true)
        {
            wholeMedicineButtonList[nowButtonIndex].medicineObject.SetActive(false);
        }
    }

    //버튼 클릭했을 때
    public void OnButtonDown(PointerEventData data, int index)
    {
        Debug.Log("Down " + index);
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
}
