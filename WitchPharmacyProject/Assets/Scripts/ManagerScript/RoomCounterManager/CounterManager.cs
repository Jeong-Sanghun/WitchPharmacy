using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;
//카운터씬 매니저
//여기서 증상까지 만들어서 medicineManager로 넘겨줌
public class CounterManager : MonoBehaviour //SH
{
    GameManager gameManager;
    SceneManager sceneManager;
    [SerializeField]
    MedicineManager medicineManager;
    [SerializeField]
    CounterDialogManager counterDialogManager;
    [SerializeField]
    MeasureToolManager measureToolManager;
    [SerializeField]
    SymptomChartManager symptomChartManager;
    [SerializeField]
    VisitorTriggerManager visitorTriggerManager;
    SaveDataClass saveData;
    SymptomDialog symptomDialog;
    List<int> ownedMedicineIndexList;
    List<MedicineClass> ownedMedicineList;
    //Dictionary<int,int> owningMedicineDictionary;
    //List<MedicineClass> owningMedicineList;
    List<MedicineClass> medicineDataList;


    SpecialVisitorClass nowSpecialVisitor;
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
    [HideInInspector]
    public bool endSales;
    //[HideInInspector]
    //public bool nowTalking;
    int index = 0;
    bool isSpecialVisitor = false;
    bool lastVisitor = false;

    [SerializeField]
    GameObject measureToolOpenButton;
    [SerializeField]
    GameObject symptomChartOpenButton;
    [SerializeField]
    GameObject specialVisitorPrefab;

    [SerializeField]
    Text gainedCoinText;
    Vector3 gainedCoinObjectOriginPos;

    SpecialVisitorDialogBundle specialVisitorDialogBundle;
    List<SpecialMedicineClass> specialMedicineDataList;

    void Start()
    {
        gameManager = GameManager.singleTon;
        sceneManager = SceneManager.inst;
        symptomDialog = gameManager.symptomDialog;
        saveData = gameManager.saveData;
        medicineDataList = gameManager.medicineDataWrapper.medicineDataList;
        ownedMedicineIndexList = saveData.ownedMedicineList;
        measureToolArray = measureToolManager.measureToolArray;
        specialMedicineDataList = gameManager.specialMedicineDataWrapper.specialMedicineDataList;
        ownedMedicineList = new List<MedicineClass>();
        for(int i = 0; i < ownedMedicineIndexList.Count; i++)
        {
            ownedMedicineList.Add(medicineDataList[ownedMedicineIndexList[i]]);
        }
        //owningMedicineDictionary = saveData.owningMedicineDictionary;
        endSales = false;

        visitorAppearPos = new Vector3(-7.06f, 0.88f, -1);
        visitorDisappearPos = new Vector3(-7.06f, -12, -1);
        gainedCoinObjectOriginPos = gainedCoinText.transform.position;

        randomVisitorList = new List<RandomVisitorClass>();


        measureToolOriginPosArray = new Vector3[5];
        symptomCheckArray = new int[6];
        symptomCheckedArray = new bool[6];
        symptomChartObject.SetActive(false);
        measureToolOpenButton.SetActive(false);
        symptomChartOpenButton.SetActive(false);
       
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
    }

    //트리거매니저에서 불러옴.
    public void CounterStart()
    {
        isSpecialVisitor = false;
        SpawnRandomVisitor();
    }
    //씨발...트리거매니저...
    public void CounterStart(string characterName)
    {
        isSpecialVisitor = true;
        specialVisitorDialogBundle = counterDialogManager.specialVisitorDialogBundle;
        SpawnSpecialVisitor(characterName);
    }

    //카운터 다이얼로그 매니저에서 불러옴.
    public void VisitorTalkEnd()
    {
        symptomChartOpenButton.SetActive(true);
        measureToolOpenButton.SetActive(true);
    }

    //카운터 다이얼로그 매니저에서 불러옴
    public void VisitorTalkStart()
    {
        symptomChartOpenButton.SetActive(false);
        measureToolOpenButton.SetActive(false);
    }

    void SpawnSpecialVisitor(string characterName)
    {
        if (endSales)
        {
            medicineManager.ToCounterButton(false);
            return;
        }
        if (nowVisitor != null)
        {
            if(nowVisitor.visitorObject != null)
                nowVisitor.visitorObject.SetActive(false);
        }
        if(nowSpecialVisitor != null)
        {
            nowSpecialVisitor.visitorObject.SetActive(false);
        }
        counterDialogManager.nowTalking = true;
        medicineManager.SpecialVisitorVisits();
        measureToolManager.OnNewVisitor(specialVisitorDialogBundle.symptomNumberArray);
        nowSpecialVisitor = new SpecialVisitorClass(visitorParent,specialVisitorPrefab, characterName);
        symptomChartManager.SpecialVisitorVisits(specialVisitorDialogBundle.symptomNumberArray);
        //쫙 뿌려준다
        for (int i = 0; i < toggleGroupArray.Length; i++)
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
        StartCoroutine(VisitorAppearCoroutine());
        symptomChartManager.ChangeSymptomChartText();
    }

    public void ChangeSpecialVisitorSprite(Sprite sprite)
    {
        nowSpecialVisitor.spriteRenderer.sprite = sprite;
    }



    void SpawnRandomVisitor()
    {
        if (endSales)
        {
            medicineManager.ToCounterButton(false);
            return;
        }
        if (nowVisitor != null)
        {
            nowVisitor.visitorObject.SetActive(false);
        }
        if (nowSpecialVisitor != null)
        {
            nowSpecialVisitor.visitorObject.SetActive(false);
        }
        counterDialogManager.nowTalking = true;
        
        nowVisitor = new RandomVisitorClass(symptomDialog,visitorParent);
        //쫙 뿌려준다
        randomVisitorList.Add(nowVisitor);
        medicineManager.VisitorVisits(nowVisitor);
        measureToolManager.OnNewVisitor(nowVisitor.symptomAmountArray);
        symptomChartManager.VisitorVisits(nowVisitor);
        index++;
        StartCoroutine(VisitorAppearCoroutine());

        //기록안했을 떄가 -3임
        for(int i = 0; i < toggleGroupArray.Length; i++)
        {
            for (int j = i * 5; j < i * 5 + 5; j++)
            {
                toggleArray[j].isOn = false;
                toggleArray[j].group = toggleGroupArray[i];
            }
        }
        for (int i = 0; i < symptomCheckArray.Length; i++)
        {
            symptomCheckedArray[i] = false;
            symptomCheckArray[i] = 0;
        }
        symptomChartManager.ChangeSymptomChartText();
    }

    //cookedMedicineManager의 pointerUP에서 호출
    public void OnMedicineDelivery(CookedMedicine medicine)
    {
        bool wrongMedicine = false;
        if (isSpecialVisitor)
        {
            bool specialWrong = true;
            for (int i = 0; i < medicine.medicineCount; i++)
            {
                if (medicine.specialArray[i] == false)
                {
                    continue;
                }
                if (specialVisitorDialogBundle.answerSpecialMedicineName == specialMedicineDataList[medicine.medicineArray[i]].fileName)
                {
                    specialWrong = false;
                    break;
                }
            }
            if(specialWrong == false)
            {

                int[] medicineIndexArray = medicine.medicineArray;
                int[] medicineSymptomArray = new int[5];
                int[] visitorSymptomArray = specialVisitorDialogBundle.symptomNumberArray;

                for (int i = 0; i < 5; i++)
                {
                    medicineSymptomArray[i] = 0;
                }
                for (int i = 0; i < medicine.medicineCount; i++)
                {
                    if (medicine.specialArray[i] == true)
                    {
                        continue;
                    }
                    MedicineClass med = medicineDataList[medicineIndexArray[i]];

                    medicineSymptomArray[(int)med.GetFirstSymptom()] += med.firstNumber;
                    medicineSymptomArray[(int)med.GetSecondSymptom()] += med.secondNumber;
                }
                for (int i = 0; i < 5; i++)
                {
                    if (medicineSymptomArray[i] + visitorSymptomArray[i] != 0)
                    {
                        wrongMedicine = true;
                        break;
                    }
                }
                if (!specialWrong && !wrongMedicine)
                {
                    wrongMedicine = false;
                }
                else
                {
                    wrongMedicine = true;
                }
                counterDialogManager.OnSpecialVisitorEnd(wrongMedicine);
            }
            else
            {
                Debug.Log(specialWrong);
                wrongMedicine = true;
                counterDialogManager.OnSpecialVisitorEnd(specialWrong);
            }

           

        }
        else
        {
            int[] medicineIndexArray = medicine.medicineArray;
            int[] medicineSymptomArray = new int[6];
            int[] visitorSymptomArray = nowVisitor.symptomAmountArray;

            List<Symptom> badSymptomList = new List<Symptom>();
            for (int i = 0; i < 6; i++)
            {
                medicineSymptomArray[i] = 0;
            }
            for (int i = 0; i < medicine.medicineCount; i++)
            {
                MedicineClass med = medicineDataList[medicineIndexArray[i]];
                //if (med.firstSymptom == Symptom.none)
                //{
                //    continue;
                //}
                medicineSymptomArray[(int)med.GetFirstSymptom()] += med.firstNumber;
                medicineSymptomArray[(int)med.GetSecondSymptom()] += med.secondNumber;
            }
            for (int i = 0; i < 6; i++)
            {
                if (medicineSymptomArray[i] + visitorSymptomArray[i] != 0)
                {
                    wrongMedicine = true;
                    badSymptomList.Add((Symptom)i);
                }
            }
            counterDialogManager.OnVisitorEnd(wrongMedicine);
        }
        
        if (!wrongMedicine)
        {
            CoinGain();
        }


    }

    //counterDialogManager에서 호출, 스킵버튼에서 호출.
    public void VisitorDisappear(bool skip)
    {
        TimeChange(3600);
        VisitorTalkStart();
        StartCoroutine(VisitorDisapperCoroutine());
    }

    
    IEnumerator VisitorAppearCoroutine()
    {

        StartCoroutine(sceneManager.MoveModule_Accel2(visitorParent, visitorAppearPos, 2f));
        yield return new WaitForSeconds(1.5f);
        if (!isSpecialVisitor)
        {
            counterDialogManager.OnVisitorVisit(nowVisitor);
        }
        else
        {
            counterDialogManager.OnSpecialVisitorVisit();
        }
        

    }

    IEnumerator VisitorDisapperCoroutine()
    {
        StartCoroutine(sceneManager.MoveModule_Accel2(visitorParent, visitorDisappearPos, 2f));
        yield return new WaitForSeconds(1.5f);
        if (lastVisitor)
        {
            endSales = true;
            toNextSceneButton.SetActive(true);

        }
        if (!endSales)
        {
            visitorTriggerManager.TriggerCheck();
        }
        
    }

    //측정도구 켜질 때 대화창 꺼주기 위한거
    public void DialogActive(bool active)
    {
        dialogPanelObject.SetActive(active);
    }

    //메저툴 드래그
    void OnButtonDrag(PointerEventData data, int index)
    {
        Vector2 mousePos =Input.mousePosition;
        measureToolIconArray[index].transform.position = Input.mousePosition;
    }

    //메저툴 엔드,드래그하고서 클릭 뗐을 때
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
                int fixedToggle;
                if (isSpecialVisitor)
                {
                    fixedToggle = symptomToggleIndex * 5 + 2 + specialVisitorDialogBundle.symptomNumberArray[symptomToggleIndex];

                }
                else
                {
                    fixedToggle = symptomToggleIndex * 5 + 2 + nowVisitor.symptomAmountArray[symptomToggleIndex];
                }
                

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

                if (isSpecialVisitor)
                {
                    symptomCheckArray[symptomToggleIndex] = specialVisitorDialogBundle.symptomNumberArray[symptomToggleIndex];

                }
                else
                {
                    symptomCheckArray[symptomToggleIndex] = nowVisitor.symptomAmountArray[symptomToggleIndex];
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

       
        symptomChartManager.ChangeSymptomChartText();
    }

    //측정이 끝나면 토글창을 고정시켜줘야돼.
    public void OnMeasureEnd(int symptom)
    {
        ToggleIndexChecker(symptom);
        SymptomCheckToggle(0);
        TimeChange(1800);
        Debug.Log("메저 엔드");
        //int pushedToggle = symptomToggleIndex * 5 + 2 + amount;
        int fixedToggle;
        if (isSpecialVisitor)
        {
            fixedToggle = symptomToggleIndex * 5 + 2 + specialVisitorDialogBundle.symptomNumberArray[symptomToggleIndex];
        }
        else
        {
            fixedToggle = symptomToggleIndex * 5 + 2 + nowVisitor.symptomAmountArray[symptomToggleIndex];
        }
         
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
        int hour = (int)gameManager.saveData.nowTime / 3600;
        int minute = ((int)gameManager.saveData.nowTime % 3600) / 60;
        TimeTextChange();
        if (hour >= 18)
        {
            lastVisitor = true;
        }
    }

    void TimeTextChange()
    {
        int hour = (int)gameManager.saveData.nowTime / 3600;
        int minute = ((int)gameManager.saveData.nowTime % 3600) / 60;
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

    void CoinGain()
    {
        saveData.coin += RandomVisitorClass.gainCoin;
        gainedCoinText.color = Color.black;
        gainedCoinText.text = "+" + RandomVisitorClass.gainCoin.ToString();
        gainedCoinText.transform.position = gainedCoinObjectOriginPos;
        TabletManager.inst.UpdateBill(BillReason.medicineSell, true, RandomVisitorClass.gainCoin);
        StartCoroutine(sceneManager.FadeModule_Text(gainedCoinText, 1, 0, 3f));
        

    }

    public void ToNextSceneButton()
    {
        gameManager.AutoSave();
        sceneManager.LoadScene("StoryScene");
    }


}
