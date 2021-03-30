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
    CookedMedicine cookedMedicine;

    GameObject touchedObject;               //터치한 오브젝트
    RaycastHit2D hit;                         //터치를 위한 raycastHit
    public Camera cam;                      //레이캐스트를 위한 카메라.

    [SerializeField]
    GameObject binObject;

    Vector3 medicineOriginPos;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        medicineDataList = gameManager.medicineDataWrapper.medicineDataList;
        binObject.SetActive(false);
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
        Debug.Log("왜안돼");
        EventTrigger medicineEvent = medicine.medicineObject.GetComponent<EventTrigger>();

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

    public void OnMedicineDrag(PointerEventData data)
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
        cookedMedicine.medicineObject.transform.position = Input.mousePosition;
    }

    public void OnMedicinePointerDown(PointerEventData data)
    {
        binObject.SetActive(true);

    }

    public void OnMedicinePointerUp(PointerEventData data)
    {


        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
        if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
        {
            Debug.Log("왜안돼");
            touchedObject = hit.collider.gameObject;
            if (touchedObject.CompareTag("RecycleBin"))
            {
                Debug.Log("이건 왜안돼");
                roomManager.CookedMedicineRemoved();
                cookedMedicine.medicineObject.SetActive(false);
            }
        }
        binObject.SetActive(false);
        cookedMedicine.medicineObject.transform.position = medicineOriginPos;

    }
}
