using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.EventSystems;

public class BossCookedMedicineManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    List<MedicineClass> medicineDataList;

    [SerializeField]
    BossCharacterManager bossCharacterManager;
    [SerializeField]
    BossMedicineManager bossMedicineManager;
    CookedMedicine cookedMedicine;

    GameObject touchedObject;               //터치한 오브젝트
    RaycastHit2D hit;                         //터치를 위한 raycastHit
    public Camera cam;                      //레이캐스트를 위한 카메라.

    //쓰레기통
    [SerializeField]
    GameObject binObject;
    [SerializeField]
    GameObject cookedMedicineObject;

    Vector3 medicineOriginPos;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleton;
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

    //룸매니저에서 부름. cooked다하고 나서
    public void CookedMedicineManagerSetting(CookedMedicine medicine)
    {
        cookedMedicine = medicine;
        medicineOriginPos = medicine.medicineObject.transform.position;
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

    void OnMedicinePointerDown(PointerEventData data)
    {
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
            if (touchedObject == binObject)
            {
                bossMedicineManager.CookedMedicineRemoved();
                cookedMedicine.medicineObject.SetActive(false);
                cookedMedicine = null;
            }

            //여기다가 보스꺼에 집어넣으면 어케되는지 넣어야함.
            if (touchedObject.CompareTag("BossSymptom"))
            {
                bossCharacterManager.OnMedicineDelivery(cookedMedicine, touchedObject);
                bossMedicineManager.CookedMedicineRemoved();
                cookedMedicine.medicineObject.SetActive(false);
                cookedMedicine = null;
            }
        }
        binObject.SetActive(false);


    }
}
