using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//roomManager가 너무 길어져가지고 쪼갬
//다 만든 약을 가지고 할 수 있는 것들을 만듦.
public class TutorialCookedMedicineManager : MonoBehaviour
{
    GameManager gameManager;
    List<MedicineClass> medicineDataList;

    [SerializeField]
    TutorialMedicineManager medicineManager;
    [SerializeField]
    TutorialCounterManager counterManager;
    [SerializeField]
    TutorialRoomManager roomManager;
    CookedMedicine cookedMedicine;

    GameObject touchedObject;               //터치한 오브젝트
    RaycastHit2D hit;                         //터치를 위한 raycastHit
    public Camera cam;                      //레이캐스트를 위한 카메라.

    //쓰레기통
    [SerializeField]
    GameObject trayObject;
    [SerializeField]
    GameObject cookedMedicineObject;

    Vector3 medicineOriginPos;
    Vector3 medicineOriginCounterPos;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleton;
        medicineDataList = gameManager.medicineDataWrapper.medicineDataList;


        EventTrigger medicineEvent = cookedMedicineObject.GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnMedicineDrag((PointerEventData)data); });
        medicineEvent.triggers.Add(entry);


        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.PointerUp;
        entry1.callback.AddListener((data) => { OnMedicinePointerUp((PointerEventData)data); });
        medicineEvent.triggers.Add(entry1);

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

    public void ToRoomButton()
    {
        if (cookedMedicine == null)
        {
            return;
        }
        cookedMedicine.medicineObject.SetActive(!cookedMedicine.medicineObject.activeSelf);
        cookedMedicine.medicineObject.transform.position = medicineOriginPos;
    }

    public void ToCounterButton()
    {
        if (cookedMedicine == null)
        {
            return;
        }
        cookedMedicine.medicineObject.SetActive(false);
        //cookedMedicine.medicineObject.GetComponent<RectTransform>().anchoredPosition = medicineOriginCounterPos;
    }


    void OnMedicineDrag(PointerEventData data)
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
        cookedMedicine.medicineObject.transform.position = Input.mousePosition;
    }

 
    void OnMedicinePointerUp(PointerEventData data)
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
        cookedMedicine.medicineObject.transform.position = medicineOriginPos;




        if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
        {
            touchedObject = hit.collider.gameObject;

            if (touchedObject == trayObject)
            {
                roomManager.ToCounterButton(true);
                //cookedMedicine.medicineObject.GetComponent<RectTransform>().anchoredPosition = medicineOriginCounterPos;
                counterManager.OnMedicineDelivery(cookedMedicine);
                //cookedMedicine.medicineObject.transform.position = medicineOriginPos;
                medicineManager.CookedMedicineRemoved();
                cookedMedicine.medicineObject.SetActive(false);
                cookedMedicine = null;
            }
        }

    }
}
