using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;


//카운터씬 매니저
//여기서 증상까지 만들어서 medicineManager로 넘겨줌
public class TutorialCounterManager : MonoBehaviour //SH
{
    GameManager gameManager;
    SceneManager sceneManager;
    [SerializeField]
    TutorialMedicineManager medicineManager;
    [SerializeField]
    TutorialSymptomChartManager symptomChartManager;
    [SerializeField]
    BlurManager blurManager;
    //SymptomDialog symptomDialog;
    //List<int> ownedMedicineIndexList;
    List<MedicineClass> ownedMedicineList;
    //Dictionary<int,int> owningMedicineDictionary;
    //List<MedicineClass> owningMedicineList;


    //SpecialVisitorClass nowSpecialVisitor;

    GameObject touchedObject;               //터치한 오브젝트
    RaycastHit2D hit;                         //터치를 위한 raycastHit
    public Camera cam;                      //레이캐스트를 위한 카메라.



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
    Text gainedCoinText;
    Vector3 gainedCoinObjectOriginPos;


    [SerializeField]
    Text wholeCoinText;
    [SerializeField]
    bool isDebugMode;

    SpecialVisitorCondition nowSpecialVisitorCondition;

    List<SpecialMedicineClass> specialMedicineDataList;

    int nowCoin;

    void Start()
    {
        gameManager = GameManager.singleton;
        sceneManager = SceneManager.inst;
        nowCoin = 100;
        //ownedMedicineIndexList = saveData.ownedMedicineList;
        specialMedicineDataList = gameManager.specialMedicineDataWrapper.specialMedicineDataList;
        ownedMedicineList = new List<MedicineClass>();
        //owningMedicineDictionary = saveData.owningMedicineDictionary;
        endSales = false;

        gainedCoinObjectOriginPos = gainedCoinText.transform.position;

        wholeCoinText.text = nowCoin.ToString();



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
        RandomVisitorClass.SetStaticData(ownedMedicineList, gameManager.randomVisitorDiseaseBundle);
        TimeTextChange();
    }

    float timer = 0;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            if (counterStarted)
                TimeChange(60);
            timer = 0;
        }
    }


    

    //cookedMedicineManager의 pointerUP에서 호출
    public void OnMedicineDelivery(CookedMedicine medicine)
    {
        //넥다
    }

    //counterDialogManager에서 호출, 스킵버튼에서 호출.



  


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
        nowCoin += coin;
        CoinTextChange();
        gainedCoinText.color = Color.black;
        gainedCoinText.text = "+" + coin.ToString();
        gainedCoinText.transform.position = gainedCoinObjectOriginPos;
        StartCoroutine(sceneManager.FadeModule_Text(gainedCoinText, 1, 0, 2f));
        StartCoroutine(sceneManager.MoveModule_Linear(gainedCoinText.gameObject, gainedCoinObjectOriginPos + new Vector3(0, 2, 0), 1));
    }

    public void PanelCoinChange(int coin)
    {
        nowCoin += coin;
        CoinTextChange();
    }


    public void CoinTextChange()
    {
        wholeCoinText.text = nowCoin.ToString();
    }

}
