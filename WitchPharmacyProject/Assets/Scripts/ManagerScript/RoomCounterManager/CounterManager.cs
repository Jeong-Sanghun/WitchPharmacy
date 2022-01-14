using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

public enum VisitorType
{
    Random,Odd,Special,RuelliaStart
}
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
    [SerializeField]
    BlurManager blurManager;
    SaveDataClass saveData;
    //SymptomDialog symptomDialog;
    //List<int> ownedMedicineIndexList;
    List<MedicineClass> ownedMedicineList;
    //Dictionary<int,int> owningMedicineDictionary;
    //List<MedicineClass> owningMedicineList;
    List<MedicineClass> medicineDataList;


    //SpecialVisitorClass nowSpecialVisitor;
    [SerializeField]
    List<VisitorClass> visitorList;
    VisitorClass nowVisitor;
    VisitorType nowVisitorType;
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
    //bool isSpecialVisitor = false;
    bool lastVisitor = false;
    bool counterStarted = false;

    [SerializeField]
    GameObject measureToolOpenButton;
    [SerializeField]
    GameObject symptomChartOpenButton;
    [SerializeField]
    GameObject specialVisitorPrefab;

    [SerializeField]
    Text gainedCoinText;
    Vector3 gainedCoinObjectOriginPos;

    [SerializeField]
    Image gainedMedicineImage;
    Vector3 gainedMedicineObjectOriginPos;

    [SerializeField]
    Text wholeCoinText;
    [SerializeField]
    bool isDebugMode;

    SpecialVisitorCondition nowSpecialVisitorCondition;

    List<SpecialMedicineClass> specialMedicineDataList;

    void Start()
    {
        gameManager = GameManager.singleton;
        sceneManager = SceneManager.inst;
        saveData = gameManager.saveData;
        medicineDataList = gameManager.medicineDataWrapper.medicineDataList;
        //ownedMedicineIndexList = saveData.ownedMedicineList;
        measureToolArray = measureToolManager.measureToolArray;
        specialMedicineDataList = gameManager.specialMedicineDataWrapper.specialMedicineDataList;
        ownedMedicineList = new List<MedicineClass>();
        for(int i = 0; i < saveData.owningMedicineList.Count; i++)
        {
            ownedMedicineList.Add(medicineDataList[saveData.owningMedicineList[i].medicineIndex]);
        }
        //owningMedicineDictionary = saveData.owningMedicineDictionary;
        endSales = false;

        visitorAppearPos = new Vector3(-7.06f, 0.88f, 2f);
        visitorDisappearPos = new Vector3(-7.06f, -12, 2f);
        gainedCoinObjectOriginPos = gainedCoinText.transform.position;
        gainedMedicineObjectOriginPos = gainedMedicineImage.transform.position;

        visitorList = new List<VisitorClass>();

        wholeCoinText.text = saveData.coin.ToString();

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

        //for(int i = 0; i < 4; i++)
        //{
        //    measureToolOriginPosArray[i] = measureToolIconArray[i].transform.position;
        //    EventTrigger buttonEvent = measureToolIconArray[i].GetComponent<EventTrigger>();


        //    EventTrigger.Entry entry = new EventTrigger.Entry();
        //    int delegateIndex = i;
        //    entry.eventID = EventTriggerType.Drag;
        //    entry.callback.AddListener((data) => { OnButtonDrag((PointerEventData)data, delegateIndex); });
        //    buttonEvent.triggers.Add(entry);

        //    EventTrigger.Entry entry1 = new EventTrigger.Entry();
        //    entry1.eventID = EventTriggerType.PointerUp;
        //    entry1.callback.AddListener((data) => { OnButtonUp((PointerEventData)data, delegateIndex); });
        //    buttonEvent.triggers.Add(entry1);
        //}
        counterStarted = false;
        //스태틱으로 만들어버려
        RandomVisitorClass.SetStaticData(ownedMedicineList,gameManager.randomVisitorDiseaseBundle);
        TimeTextChange();
    }

    float timer = 0;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            if(counterStarted)
                TimeChange(60);
            timer = 0;
        }
    }

    //트리거매니저에서 불러옴.
    public void CounterStart()
    {
        counterStarted = true;
        nowVisitorType = VisitorType.Random;
        SpawnRandomVisitor();
    }

    public void CounterSpecialStart(SpecialVisitorCondition condition)
    {
        counterStarted = true;
        if (condition.isOdd)
        {
            nowVisitorType = VisitorType.Odd;
        }
        else
        {
            nowVisitorType = VisitorType.Special;
        }
        
        nowSpecialVisitorCondition = condition;
        SpawnRandomVisitor();
    }

    public VisitorClass GetVisitor()
    {
        return nowVisitor;
    }

    ////씨발...트리거매니저...
    //public void CounterSecondStart(SpecialVisitorDialogBundle bundle)
    //{
    //    nowVisitorType = VisitorType.SecondSpecial;
    //    specialVisitorDialogBundle = bundle;
    //    SpawnSpecialVisitor(true);
    //}
    ////좃리거 메니저~
    //public void CounterFirstStart(SpecialVisitorDialogBundle bundle)
    //{
    //    nowVisitorType = VisitorType.FirstSpecial;
    //    specialVisitorDialogBundle = bundle;
    //    SpawnSpecialVisitor(false);
    //}
    //whtflrj~~
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

    void SpawnSpecialVisitor(bool isSecond)
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
        //if(nowSpecialVisitor != null)
        //{
        //    nowSpecialVisitor.visitorObject.SetActive(false);
        //}
        counterDialogManager.nowTalking = true;
        medicineManager.SpecialVisitorVisits();
        //symptomChartManager.SpecialVisitorVisits(specialVisitorDialogBundle.symptomNumberArray);
        //measureToolManager.OnNewVisitor(specialVisitorDialogBundle.symptomNumberArray);
        //nowSpecialVisitor = new SpecialVisitorClass(visitorParent,specialVisitorPrefab, specialVisitorDialogBundle.characterName,CharacterFeeling.nothing);
        
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

    //카운터 다이얼로그 매니저에서 스프라이트 바꿔줘야돼서. 앵그리로.
    //public void ChangeSpecialVisitorSprite(Sprite sprite)
    //{
    //    nowSpecialVisitor.spriteRenderer.sprite = sprite;
    //}



    void SpawnRandomVisitor()
    {
        if (endSales)
        {
            medicineManager.ToCounterButton(false);
            return;
        }

        counterDialogManager.nowTalking = true;
        

        
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
        int[] medicineIndexArray = medicine.medicineArray;
        int[] medicineSymptomArray = new int[6];
        int[] finalSymptomArray = new int[6];
        int[] visitorSymptomArray = nowVisitor.symptomAmountArray;

        List<Symptom> badSymptomList = new List<Symptom>();
        for (int i = 0; i < 5; i++)
        {
            finalSymptomArray[i] = 0;
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
        for (int i = 0; i < 5; i++)
        {
            finalSymptomArray[i] = medicineSymptomArray[i] + visitorSymptomArray[i];
            if (finalSymptomArray[i] != 0)
            {
                wrongMedicine = true;
                badSymptomList.Add((Symptom)i);
            }
        }
        if (wrongMedicine)
        {
            counterDialogManager.OnVisitorEnd(VisitorEndState.Wrong);
        }
        else
        {
            counterDialogManager.OnVisitorEnd(VisitorEndState.Right);
        }

        if (nowVisitor.visitorType == VisitorType.Random)
        {

            ((RandomVisitorClass)nowVisitor).FinalSymptomSpriteUpdate(finalSymptomArray);
            StartCoroutine(nowVisitor.FinalDissolve());
        }
        else
        {

            nowVisitor.FinalSymptomSpriteUpdate(finalSymptomArray);
            StartCoroutine(nowVisitor.FinalDissolve());
        }
    }

    //counterDialogManager에서 호출, 스킵버튼에서 호출.
    public void VisitorDisappear(bool skip)
    {
        //TimeChange(3600);
        VisitorTalkStart();
        if (skip)
        {
            counterDialogManager.OnVisitorEnd(VisitorEndState.Skip);
        }
        else
        {
            StartCoroutine(VisitorDisapperCoroutine());
        }

        
    }

    public void SetSpecialVisitor(string characterName, string feeling)
    {

        if (nowVisitor != null && nowVisitor.visitorType == VisitorType.Special)
        {
            ((SpecialVisitorClass)nowVisitor).SetObjectImage(characterName, feeling);
        }
        else
        {
            if (nowVisitor != null)
            {
                nowVisitor.visitorObject.SetActive(false);
            }
            SpecialVisitorClass visitor = new SpecialVisitorClass(visitorParent, specialVisitorPrefab, characterName, feeling);
            nowVisitor = visitor;
        }
    }

    //카운터 다이얼로그 매니저에서 불러옴.
    public void VisitorUpFx()
    {
        StartCoroutine(sceneManager.MoveModule_Linear(visitorParent, visitorAppearPos, 0.5f));
    }

    public void VisitorDownFx()
    {
        StartCoroutine(sceneManager.MoveModule_Linear(visitorParent, visitorDisappearPos, 0.5f));
    }

    
    IEnumerator VisitorAppearCoroutine()
    {
        if(visitorParent.transform.position != visitorDisappearPos)
        {
            VisitorDownFx();
            yield return new WaitForSeconds(2);
        }
        if (nowVisitor != null)
        {
            nowVisitor.visitorObject.SetActive(false);
        }
        if(nowVisitorType == VisitorType.Random)
        {
            nowVisitor = new RandomVisitorClass(visitorParent, saveData.nowRegion);
        }
        else if(nowVisitorType== VisitorType.Special)
        {
            VisitorDialogBundle bundle = counterDialogManager.LoadSpecialOddBundle(nowSpecialVisitorCondition,nowVisitorType);
            nowVisitor = new SpecialVisitorClass(visitorParent, specialVisitorPrefab, bundle);
            VisitorDialogWrapper wrapper = bundle.startWrapperList[0];
            ((SpecialVisitorClass)nowVisitor).SetObjectImage(wrapper.characterName,wrapper.characterFeeling);
        }
        else if (nowVisitorType == VisitorType.Odd)
        {
            VisitorDialogBundle bundle = counterDialogManager.LoadSpecialOddBundle(nowSpecialVisitorCondition, nowVisitorType);
            nowVisitor = new OddVisitorClass(visitorParent, specialVisitorPrefab, bundle);
        }
        
        //쫙 뿌려준다
        visitorList.Add(nowVisitor);
        symptomChartManager.VisitorVisits(nowVisitor);
        medicineManager.VisitorVisits(nowVisitor);
        measureToolManager.OnNewVisitor(nowVisitor.symptomAmountArray);

        blurManager.OnBlur(true);
        StartCoroutine(sceneManager.MoveModule_Linear(visitorParent, visitorAppearPos, 0.5f));

        yield return new WaitForSeconds(1.5f);
        counterDialogManager.OnVisitorVisit(nowVisitor);

        //switch (nowVisitorType){
        //    case VisitorType.Random:
        //        counterDialogManager.OnVisitorVisit(nowVisitor);
        //        break;
        //    case VisitorType.Special:

        //        break;
        //        //case VisitorType.SecondSpecial:
        //        //    counterDialogManager.OnSpecialVisitorVisit(specialVisitorDialogBundle,true);
        //        //    break;
        //        //case VisitorType.FirstSpecial:
        //        //    counterDialogManager.OnSpecialVisitorVisit(specialVisitorDialogBundle, false);
        //        //    break;
        //        //case VisitorType.Odd:
        //        //    counterDialogManager.OnOddVisitorVisit(oddVisitorDialogBundle,nowVisitor);
        //        //break;
        //}


    }

    IEnumerator VisitorDisapperCoroutine()
    {
        blurManager.OnBlur(false);
        StartCoroutine(sceneManager.MoveModule_Linear(visitorParent, visitorDisappearPos, 0.5f));

        yield return new WaitForSeconds(1.5f);
        if(isDebugMode == true)
        {
            lastVisitor = true;
        }
        if (lastVisitor)
        {
            endSales = true;

            //gameManager.AutoSave("StoryScene");
            gameManager.ForceSaveButtonActive("StoryScene",SaveTime.ExploreStart);
            //toNextSceneButton.SetActive(true);

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

    ////메저툴 드래그
    //void OnButtonDrag(PointerEventData data, int index)
    //{
    //    Vector2 mousePos =Input.mousePosition;
    //    measureToolIconArray[index].transform.position = Input.mousePosition;
    //}

    ////메저툴 엔드,드래그하고서 클릭 뗐을 때
    //void OnButtonUp(PointerEventData data, int index)
    //{
        
    //    measureToolIconArray[index].transform.position = measureToolOriginPosArray[index];

    //    Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    //    if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
    //    {
    //        touchedObject = hit.collider.gameObject;
    //        //Ray에 맞은 콜라이더를 터치된 오브젝트로 설정
    //        if (touchedObject.CompareTag("Visitor"))
    //        {
    //            measureToolManager.ToolOpenButton(index);
    //        }
    //    }
    //}

    int symptomToggleIndex = 0;
    //시발 파라미터가 1개밖에 안들어가서 어쩔 수 없이 두 개의 함수를 파야했다.
    //위에거에서는 단순히 증상인덱스만 넘겨주고, 아래거에서는 -2~2의 값을 넘겨준다
    public void ToggleIndexChecker(int symptom)
    {
        symptomToggleIndex = symptom;
    }


    public void SymptomCheckToggle(int amount)
    {
        if(symptomToggleIndex != 4)
        {
            if (measureToolArray[symptomToggleIndex].measureEnd)
            {
                int fixedToggle;
                //if (nowVisitorType == VisitorType.SecondSpecial)
                //{
                //    fixedToggle = symptomToggleIndex * 5 + 2 + specialVisitorDialogBundle.symptomNumberArray[symptomToggleIndex];

                //}
                //else
                //{
                    fixedToggle = symptomToggleIndex * 5 + 2 + nowVisitor.symptomAmountArray[symptomToggleIndex];
                //}
                

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

                //if (nowVisitorType== VisitorType.SecondSpecial)
                //{
                //    symptomCheckArray[symptomToggleIndex] = specialVisitorDialogBundle.symptomNumberArray[symptomToggleIndex];

                //}
                //else
                //{
                    symptomCheckArray[symptomToggleIndex] = nowVisitor.symptomAmountArray[symptomToggleIndex];
                //}



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
        //if (nowVisitorType== VisitorType.SecondSpecial)
        //{
        //    fixedToggle = symptomToggleIndex * 5 + 2 + specialVisitorDialogBundle.symptomNumberArray[symptomToggleIndex];
        //}
        //else
        //{
            fixedToggle = symptomToggleIndex * 5 + 2 + nowVisitor.symptomAmountArray[symptomToggleIndex];
        //}
         
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
        if (hour >= 12)
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

    //카운터다이얼로그매니저
    public void CoinGain(int coin)
    {
        saveData.coin += coin;
        CoinTextChange();
        gainedCoinText.color = Color.black;
        gainedCoinText.text = "+" + coin.ToString();
        gainedCoinText.transform.position = gainedCoinObjectOriginPos;
        TabletManager.inst.UpdateBill(BillReason.medicineSell, true, coin);
        StartCoroutine(sceneManager.FadeModule_Text(gainedCoinText, 1, 0, 2f));
        StartCoroutine(sceneManager.MoveModule_Linear(gainedCoinText.gameObject, gainedCoinObjectOriginPos + new Vector3(0, 2, 0), 1));
    }

    void SpecialMedicineGain(string medicineName)
    {
        
        int index = -1;
        SpecialMedicineClass medicine = null;
        for (int i = 0; i < specialMedicineDataList.Count; i++)
        {
            if (specialMedicineDataList[i].fileName == medicineName)
            {
                medicine = specialMedicineDataList[i];
                index = i;
                break;
            }
        }
        if (index == -1)
        {
            Debug.LogError("좆됐따");
            return;
        }
        bool find = false;
        for (int i = 0; i < saveData.owningSpecialMedicineList.Count; i++)
        {
            if (saveData.owningSpecialMedicineList[i].medicineIndex == index)
            {
                //if(saveData.owningSpecialMedicineList[i].medicineQuantity == 0)
                //{
                //    medicineManager.AddSpecialMedicineOnOdd(saveData.owningSpecialMedicineList[i]);
                //}
                //saveData.owningSpecialMedicineList[i].medicineQuantity++;
                find = true;
                break;
            }
        }
        if (find == false)
        {
            OwningMedicineClass med = new OwningMedicineClass();
            med.medicineIndex = index;
            med.medicineCost = medicine.cost;
            saveData.owningSpecialMedicineList.Add(med);
            medicineManager.AddSpecialMedicineOnOdd(med);
        }
        
        gainedMedicineImage.sprite = medicine.LoadImage();
        gainedMedicineImage.transform.position = gainedMedicineObjectOriginPos;
        StartCoroutine(sceneManager.FadeModule_Image(gainedMedicineImage.gameObject, 1, 0, 2f));
        StartCoroutine(sceneManager.MoveModule_Linear(gainedMedicineImage.gameObject, gainedMedicineObjectOriginPos + new Vector3(0, 2, 0), 1));


    }

    public void ToNextSceneButton()
    {
        
        sceneManager.LoadScene("StoryScene");
    }

    public void CoinTextChange()
    {
        wholeCoinText.text = saveData.coin.ToString();
    }

}
