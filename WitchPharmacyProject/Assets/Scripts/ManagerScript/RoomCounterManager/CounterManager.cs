using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;
//카운터씬 매니저
//여기서 증상까지 만들어서 RoomManager로 넘겨줌
public class CounterManager : MonoBehaviour //SH
{
    GameManager gameManager;
    SceneManager sceneManager;
    [SerializeField]
    RoomManager roomManager;
    [SerializeField]
    MeasureToolManager measureToolManager;
    SaveDataClass saveData;
    SymptomDialog symptomDialog;
    List<int> ownedMedicineIndexList;
    List<MedicineClass> ownedMedicineList;
    //Dictionary<int,int> owningMedicineDictionary;
    //List<MedicineClass> owningMedicineList;
    List<MedicineClass> medicineDataList;
   

    [SerializeField]
    List<RandomVisitorClass> randomVisitorList;
    RandomVisitorClass nowVisitor;
    [SerializeField]
    Text visitorText;

    [SerializeField]
    GameObject visitorParent;

    

    Vector3 visitorAppearPos;
    Vector3 visitorDisappearPos;

    [SerializeField]
    GameObject dialogPanelObject;

    [SerializeField]
    GameObject[] measureToolIconArray ;
    Vector3[] measureToolOriginPosArray;

    GameObject touchedObject;               //터치한 오브젝트
    RaycastHit2D hit;                         //터치를 위한 raycastHit
    public Camera cam;                      //레이캐스트를 위한 카메라.


    //내가 기록한 값. 룸매니저의 증상확인에서 가져옴.
    //바로바로 가져와야돼서 퍼블릭 쓸수박에 없엄슴
    [HideInInspector]
    public int[] symptomCheckArray;
    [HideInInspector]
    public bool[] symptomCheckedArray;  //체크를 했는지 안했는지

    [SerializeField]
    GameObject symptomChartObject;
    [SerializeField]
    ToggleGroup[] toggleGroupArray;
    MeasureTool[] measureToolArray;
    
    //2차원배열은 serialize안되네;
    [SerializeField]
    Toggle[] toggleArray;

    [SerializeField]
    GameObject toNextSceneButton;

    [SerializeField]
    Text timeText;

    public bool endSales;
    


    void Start()
    {
        gameManager = GameManager.singleTon;
        sceneManager = SceneManager.inst;
        symptomDialog = gameManager.symptomDialog;
        saveData = gameManager.saveData;
        medicineDataList = gameManager.medicineDataWrapper.medicineDataList;
        ownedMedicineIndexList = saveData.ownedMedicineList;
        measureToolArray = measureToolManager.measureToolArray;
        ownedMedicineList = new List<MedicineClass>();
        for(int i = 0; i < ownedMedicineIndexList.Count; i++)
        {
            ownedMedicineList.Add(medicineDataList[ownedMedicineIndexList[i]]);
        }
        //owningMedicineDictionary = saveData.owningMedicineDictionary;
        endSales = false;

        visitorAppearPos = new Vector3(-7.06f, 0.88f, -1);
        visitorDisappearPos = new Vector3(-7.06f, -12, -1);

        randomVisitorList = new List<RandomVisitorClass>();

        measureToolOriginPosArray = new Vector3[5];
        symptomCheckArray = new int[6];
        symptomCheckedArray = new bool[6];
        symptomChartObject.SetActive(false);
       
        //기록안했을 떄가 -10임
        for(int i = 0; i < symptomCheckArray.Length; i++)
        {
            symptomCheckArray[i] = 0;
            symptomCheckedArray[i] = false;
        } 

        for(int i = 0; i < 5; i++)
        {
            measureToolOriginPosArray[i] = measureToolIconArray[i].transform.position;
            EventTrigger buttonEvent = measureToolIconArray[i].GetComponent<EventTrigger>();


            EventTrigger.Entry entry = new EventTrigger.Entry();
            int delegateIndex = i;
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { OnButtonDrag((PointerEventData)data, delegateIndex); });
            buttonEvent.triggers.Add(entry);

            EventTrigger.Entry entry1 = new EventTrigger.Entry();
            entry1.eventID = EventTriggerType.PointerUp;
            entry1.callback.AddListener((data) => { OnButtonUp((PointerEventData)data, delegateIndex); });
            buttonEvent.triggers.Add(entry1);
        }

        //스태틱으로 만들어버려
        RandomVisitorClass.SetOwnedMedicineList(ownedMedicineList);
        TimeTextChange();
        SpawnRandomVisitor();   //이거 나중에 지울거임.
    }

    int index = 0;
    void SpawnRandomVisitor()
    {


        if (endSales)
        {
            roomManager.ToCounterButton(false);
            return;
        }
        if (nowVisitor != null)
        {
            nowVisitor.visitorObject.SetActive(false);
        }
        
        nowVisitor = new RandomVisitorClass(symptomDialog,visitorParent);
        randomVisitorList.Add(nowVisitor);
        roomManager.VisitorVisits(nowVisitor);
        measureToolManager.OnNewVisitor(nowVisitor.symptomAmountArray);
        StartCoroutine(VisitorAppearCoroutine());
        index++;
        //기록안했을 떄가 -3임
        for(int i = 0; i < toggleGroupArray.Length; i++)
        {
            for (int j = i * 5; j < i * 5 + 5; j++)
            {
                toggleArray[j].isOn = false;
                toggleArray[j].group = toggleGroupArray[i];
            }
            //toggleGroupArray[i].SetAllTogglesOff();

            
        }
        for (int i = 0; i < symptomCheckArray.Length; i++)
        {
            symptomCheckedArray[i] = false;
            symptomCheckArray[i] = 0;
        }
    }

    //cookedMedicineManager의 pointerUP에서 호출
    public void OnMedicineDelivery(CookedMedicine medicine)
    {
        int[] medicineIndexArray = medicine.medicineArray;
        int[] medicineSymptomArray = new int[6];
        int[] visitorSymptomArray = nowVisitor.symptomAmountArray;
        bool goodMedicine = true;
        List<Symptom> badSymptomList = new List<Symptom>();
        for(int i =0; i < 6; i++)
        {
            medicineSymptomArray[i] = 0;
        }
        for(int i = 0; i < 3; i++)
        {
            MedicineClass med = medicineDataList[medicineIndexArray[i]];
            if (med.firstSymptom == Symptom.none)
            {
                continue;
            }
            Debug.Log("메디슨 첫번째 넘버 " + med.firstNumber);
            Debug.Log("메디슨 두번째 넘버 " + med.secondNumber);
            medicineSymptomArray[(int)med.firstSymptom] += med.firstNumber;
            medicineSymptomArray[(int)med.secondSymptom] += med.secondNumber;
        }
        for(int i = 0; i < 6; i++)
        {
            if(medicineSymptomArray[i] + visitorSymptomArray[i] != 0)
            {
                goodMedicine = false;
                badSymptomList.Add((Symptom)i);
            }
        }
        StringBuilder builder;
        if (goodMedicine)
        {
            builder = new StringBuilder("아주 좋은 약이에요!!! 감사합니다!!");
        }
        else
        {
            builder = new StringBuilder("윽.....어딘가 이상해요......특히 ");
            for(int i = 0; i < badSymptomList.Count; i++)
            {
                builder.Append(badSymptomList[i]);
                if(i+1 != badSymptomList.Count)
                    builder.Append("하고 ");
            }
            builder.Append(" 쪽이 이상해요...");
        }

        StartCoroutine(VisitorDisapperCoroutine(builder.ToString()));
    }

    
    IEnumerator VisitorAppearCoroutine()
    {

        StartCoroutine(sceneManager.MoveModule_Accel2(visitorParent, visitorAppearPos, 2f));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(sceneManager.LoadTextOneByOne(randomVisitorList[index-1].fullDialog, visitorText));


    }

    IEnumerator VisitorDisapperCoroutine(string dialog)
    {

        StartCoroutine(sceneManager.LoadTextOneByOne(dialog, visitorText));
        yield return new WaitForSeconds(4f);
        StartCoroutine(sceneManager.MoveModule_Accel2(visitorParent, visitorDisappearPos, 2f));
        yield return new WaitForSeconds(1.5f);
        TimeChange(3600);
        if (!endSales)
        {
            SpawnRandomVisitor();
        }
        
    }

    //측정도구 켜질 때 대화창 꺼주기 위한거
    public void DialogActive(bool active)
    {
        dialogPanelObject.SetActive(active);
    }

    void OnButtonDrag(PointerEventData data, int index)
    {
        Vector2 mousePos =Input.mousePosition;
        measureToolIconArray[index].transform.position = Input.mousePosition;
    }

    //드래그하고서 클릭 뗐을 때
    void OnButtonUp(PointerEventData data, int index)
    {
        measureToolIconArray[index].transform.position = measureToolOriginPosArray[index];

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
        {
            touchedObject = hit.collider.gameObject;
            //Ray에 맞은 콜라이더를 터치된 오브젝트로 설정
            if (touchedObject.CompareTag("Visitor"))
            {
                measureToolManager.ToolOpenButton(index);
            }
        }
    }

    int symptomToggleIndex = 0;
    //시발 파라미터가 1개밖에 안들어가서 어쩔 수 없이 두 개의 함수를 파야했다.
    //위에거에서는 단순히 증상인덱스만 넘겨주고, 아래거에서는 -2~2의 값을 넘겨준다
    public void ToggleIndexChecker(int symptom)
    {
        symptomToggleIndex = symptom;
    }


    public void SymptomCheckToggle(int amount)
    {
        if(symptomToggleIndex != 5)
        {
            if (measureToolArray[symptomToggleIndex].measureEnd)
            {
                int fixedToggle = symptomToggleIndex * 5 + 2 + nowVisitor.symptomAmountArray[symptomToggleIndex];

                for (int i = symptomToggleIndex * 5; i < symptomToggleIndex*5 + 5; i++)
                {
                    if (i == fixedToggle)
                    {
                        if(toggleArray[i].isOn == false)
                        {
                            toggleArray[i].isOn = true;
                        }
                    }
                    else
                    {
                        toggleArray[i].isOn = false;
                    }
                }

                symptomCheckedArray[symptomToggleIndex] = true;
                symptomCheckArray[symptomToggleIndex] = nowVisitor.symptomAmountArray[symptomToggleIndex];

            }
            else
            {
                if (!toggleGroupArray[symptomToggleIndex].AnyTogglesOn())
                {
                    symptomCheckedArray[symptomToggleIndex] = false;
                    symptomCheckArray[symptomToggleIndex] = 0;
                }
                else
                {
                    symptomCheckedArray[symptomToggleIndex] = true;
                    symptomCheckArray[symptomToggleIndex] = amount;
                }
            }
        }
        else
        {
            if (!toggleGroupArray[symptomToggleIndex].AnyTogglesOn())
            {
                symptomCheckedArray[symptomToggleIndex] = false;
                symptomCheckArray[symptomToggleIndex] = 0;
            }
            else
            {
                symptomCheckedArray[symptomToggleIndex] = true;
                symptomCheckArray[symptomToggleIndex] = amount;
            }
        }

       
        roomManager.ChangeSymptomChartText();
    }

    //측정이 끝나면 토글창을 고정시켜줘야돼.
    public void OnMeasureEnd(int symptom)
    {
        ToggleIndexChecker(symptom);
        SymptomCheckToggle(0);

        Debug.Log("메저 엔드");
        //int pushedToggle = symptomToggleIndex * 5 + 2 + amount;
        int fixedToggle = symptomToggleIndex * 5 + 2 + nowVisitor.symptomAmountArray[symptomToggleIndex];
        toggleGroupArray[symptomToggleIndex].SetAllTogglesOff();

        for (int i = symptomToggleIndex * 5; i < symptomToggleIndex*5 + 5; i++)
        {
            toggleArray[i].group = null;
            if (i == fixedToggle)
            {
                toggleArray[i].isOn = true;
            }
            else
            {
                toggleArray[i].isOn = false;
            }

        }

    }

    //증상기록 버튼 닫기버튼
    public void SymptomChartActive(bool active)
    {
        symptomChartObject.SetActive(active);
    }

    public void TimeChange(float plusTime)
    {
        gameManager.TimeChange(plusTime);
        int hour = (int)gameManager.nowTime / 3600;
        int minute = ((int)gameManager.nowTime % 3600) / 60;
        TimeTextChange();
        if (hour >= 16)
        {
            endSales = true;
            toNextSceneButton.SetActive(true);
        }
    }

    void TimeTextChange()
    {
        int hour = (int)gameManager.nowTime / 3600;
        int minute = ((int)gameManager.nowTime % 3600) / 60;
        StringBuilder builder = new StringBuilder();
        if (hour / 10 < 1)
        {
            builder.Append("0");
        }
        builder.Append(hour.ToString());
        builder.Append(":");
        if (minute / 10 < 1)
        {
            builder.Append("0");
        }
        builder.Append(minute.ToString());
        timeText.text = builder.ToString();
    }

    public void ToNextSceneButton()
    {;
        gameManager.SaveJson();
        sceneManager.LoadScene("StoryScene");
    }


}
