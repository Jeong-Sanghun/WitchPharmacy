using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//roomManager가 너무 길어져가지고 쪼갬
//다 만든 약을 가지고 할 수 있는 것들을 만듦.
public class CookedMedicineManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    List<MedicineClass> medicineDataList;
    
    [SerializeField]
    RoomManager roomManager;
    [SerializeField]
    CounterManager counterManager;
    CookedMedicine cookedMedicine;

    GameObject touchedObject;               //터치한 오브젝트
    RaycastHit2D hit;                         //터치를 위한 raycastHit
    public Camera cam;                      //레이캐스트를 위한 카메라.

    //쓰레기통
    [SerializeField]
    GameObject binObject;
    [SerializeField]
    GameObject trayObject;
    [SerializeField]
    GameObject cookedMedicineObject;

    Vector3 medicineOriginPos;
    Vector3 medicineOriginCounterPos;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        medicineDataList = gameManager.medicineDataWrapper.medicineDataList;
        binObject.SetActive(false);//쓰레기통 꺼줌


        EventTrigger medicineEvent = cookedMedicineObject.GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnMedicineDrag((PointerEventData)data); });
        medicineEvent.triggers.Add(entry);


        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.PointerUp;
        entry1.callback.AddListener((data) => { OnMedicinePointerUp((PointerEventData)data); });
        medicineEvent.triggers.Add(entry1);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerDown;
        entry2.callback.AddListener((data) => { OnMedicinePointerDown((PointerEventData)data); });
        medicineEvent.triggers.Add(entry2);
    }

    // Update is called once per frame
    void Update()
    {
        //약을 다 만들고 버리면 약재는 안돌아옴.    
    }
    
    //룸매니저에서 부름. cooked다하고 나서
    public void CookedMedicineManagerSetting(CookedMedicine medicine)
    {
        cookedMedicine = medicine;
        medicineOriginPos = medicine.medicineObject.transform.position;
        medicineOriginCounterPos = new Vector3(-100, -555, 0);
        //약병오브젝트를 만들고 거기다가 이벤트들을 심어줌
        //약병오브젝트는 UI이고 쓰레기통은 월드오브젝트임


    }

    void OnMedicineDrag(PointerEventData data)
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
        cookedMedicine.medicineObject.transform.position = Input.mousePosition;
    }

    void OnMedicinePointerDown(PointerEventData data)
    {
        if (!roomManager.nowInRoom)
        {
            return;
        }
        binObject.SetActive(true);
        //약병을 누르면 쓰레기통이 켜짐.
    }

    void OnMedicinePointerUp(PointerEventData data)
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
        cookedMedicine.medicineObject.transform.position = medicineOriginPos;



        if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
        {
            touchedObject = hit.collider.gameObject;
            if (touchedObject==binObject)
            {
                roomManager.CookedMedicineRemoved();
                cookedMedicine.medicineObject.SetActive(false);
            }
            if (touchedObject== trayObject)
            {
                roomManager.ToCounterButton(true);
                cookedMedicine.medicineObject.GetComponent<RectTransform>().anchoredPosition = medicineOriginCounterPos;
            }
            if (touchedObject.CompareTag("Visitor"))
            {
                counterManager.OnMedicineDelivery(cookedMedicine);
                roomManager.CookedMedicineRemoved();
                cookedMedicine.medicineObject.SetActive(false);
            }
        }
        binObject.SetActive(false);


    }
}
