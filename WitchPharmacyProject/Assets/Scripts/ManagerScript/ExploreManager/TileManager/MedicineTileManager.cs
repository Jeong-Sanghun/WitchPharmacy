using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MedicineTileManager : TileManager
{
    public static int appearingMedicineTypes = 4;
    public static int appearingMedicine = 8;
    //매 약재 하나마다 이걸 저장해준다.
    class MedicineStruct
    {
        public bool harvested;
        public MedicineClass medicine;
        public GameObject clickerObject;
        public GameObject medicineParent;
    }

    //매 타일마다 요걸 하나씩 저장해준다.
    class MedicineTileStruct
    {
        public int tileIndex;
        public MedicineTile tile;
        public MedicineStruct[] medicineArray;
        //public GameObject[] medicineParentArray;
        public GameObject parentCanvas;
    }

    int[] medicineIndexArray;
    float[] medicineProbabilityArray;
    
    List<MedicineClass> medicineDataList;
    List<MedicineTileStruct> medicineTileList;
    MedicineTileStruct nowMedicineTileStruct;

    MedicineTile nowMedicineTile;

    [SerializeField]
    Transform medicineCanvas;
    [SerializeField]
    GameObject clickerUI;

    [SerializeField]
    GameObject medicineParentPrefab;
    [SerializeField]
    GameObject medicineObjectPrefab;

    //타일 열 때 실행됨
    public override void TileOpen(TileButtonClass tile)
    {
        base.TileOpen( tile);
        nowMedicineTile = (MedicineTile)tile.tileClass;


        medicineIndexArray = regionProperty.regionAvailableMedicine;
        medicineProbabilityArray = regionProperty.medicineProbability;
        //만약 열었던거면 SetActive(true)만 해주면 되고, 아니라면 새로 만들어줘야해
        if(nowMedicineTile.opened == true)
        {
            for(int i = 0; i < medicineTileList.Count; i++)
            {
                if(tile.tileClass == medicineTileList[i].tile)
                {
                    nowMedicineTileStruct = medicineTileList[i];
                }
            }
            //여기는 오픈드인데 잘못들어온거지.
            if(nowMedicineTileStruct == null)
            {
                RejoinedMedicineTileInit();
            }
            else
            {
                nowMedicineTileStruct.parentCanvas.SetActive(true);
            }
                

        }
        else
        {
            nowMedicineTile.Open();
            nowMedicineTile.MedicineDataSetting(medicineIndexArray, medicineProbabilityArray);

            MedicineTileStructInitialize();

        }

    }

    public override void Initialize(RegionProperty property)
    {
        base.Initialize(property);
        medicineTileList = new List<MedicineTileStruct>();
        
    }

    void RejoinedMedicineTileInit()
    {
        MedicineTileStructInitialize();

        for(int i = 0; i < appearingMedicineTypes; i++)
        {
            bool bothClicked = true;
            for(int j =0;j<appearingMedicine / appearingMedicineTypes; j++)
            {
                if (nowMedicineTile.clickedArray[2*i+j] == true)
                {
                    nowMedicineTileStruct.medicineArray[2 * i + j].clickerObject.SetActive(false);
                    nowMedicineTileStruct.medicineArray[2 * i + j].harvested = true;
                }
                else
                {
                    bothClicked = false;
                }
            }
            if (bothClicked)
            {
                nowMedicineTileStruct.medicineArray[2 * i].medicineParent.SetActive(false);
            }
            
        }
        

    }

    void MedicineTileStructInitialize()
    {
        medicineDataList = gameManager.medicineDataWrapper.medicineDataList;
        MedicineTileStruct tileStruct = new MedicineTileStruct();


        tileStruct.tileIndex = nowMedicineTile.index;
        tileStruct.tile = nowMedicineTile;
        tileStruct.medicineArray = new MedicineStruct[appearingMedicine];
        //tileStruct.medicineParentArray = new GameObject[appearingMedicineTypes];
        tileStruct.parentCanvas = Instantiate(clickerUI, medicineCanvas);

        //tileStruct.parentCanvas.transform.SetParent(medicineCanvas);

        //이부분은 어떻게 못하겠다;; 상수로 할 수 가 없음.
        for (int i = 0; i < appearingMedicineTypes; i++)
        {
            GameObject parentObj = Instantiate(medicineParentPrefab, tileStruct.parentCanvas.transform);
            Vector3 randomPos = new Vector3((i%2-1) * 1280+Random.Range(440, 840), (-1 * i/2)*620 +Random.Range(260, 360), 0);

            parentObj.GetComponent<RectTransform>().anchoredPosition = randomPos;
            MedicineClass medicineClass = medicineDataList[nowMedicineTile.appearingMedicineIndex[i]];
            medicineClass.LoadImage();

            EventTrigger buttonEvent = parentObj.GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            int delegateIndex = i;
            entry.callback.AddListener((data) => { OnMedicineClick((PointerEventData)data, delegateIndex); });
            buttonEvent.triggers.Add(entry);


            for (int j = 0; j < appearingMedicine/appearingMedicineTypes; j++)
            {
                MedicineStruct med = new MedicineStruct();
                med.medicine = medicineClass;
                med.harvested = false;
                med.medicineParent = parentObj;
                GameObject clickObj = Instantiate(medicineObjectPrefab, parentObj.transform);
                Vector3 pos = new Vector3(j * 100 - 50, j * 100 - 50, 0);
                clickObj.GetComponent<RectTransform>().anchoredPosition = pos; 
                med.clickerObject = clickObj;
                clickObj.GetComponent<Image>().sprite = med.medicine.LoadImage();
                Debug.Log(2 * i + j);
                tileStruct.medicineArray[2 * i + j] = med;
                
            }
        }
        nowMedicineTileStruct = tileStruct;
        medicineTileList.Add(tileStruct);
    }

    //이거 인덱스는 0 1 2 3으로 들어오는데 실제 참조해야되는 인덱스는 0 : 01, 1 : 23, 이렇게 들어감.
    public void OnMedicineClick(PointerEventData data,int index)
    {
        MedicineStruct nowMedicineStruct;
        bool second = false;
        int nowIndex;
        if (nowMedicineTileStruct.medicineArray[index*2].harvested == false)
        {
            nowMedicineTileStruct.medicineArray[index * 2].harvested = true;
            nowIndex = index * 2;
            nowMedicineStruct = nowMedicineTileStruct.medicineArray[index*2];
        }
        else if(nowMedicineTileStruct.medicineArray[index*2+1].harvested== false)
        {
            second = true;
            nowMedicineStruct = nowMedicineTileStruct.medicineArray[index * 2+1];
            nowIndex = index * 2 + 1;
            nowMedicineStruct.harvested = true;
        }
        else
        {
            return;
        }
        OwningMedicineClass owningMedicine = null;
        for(int i = 0; i < saveData.owningMedicineList.Count; i++)
        {
            if(nowMedicineStruct.medicine.GetIndex() == saveData.owningMedicineList[i].medicineIndex)
            {
                owningMedicine = saveData.owningMedicineList[i];
                break;
            }
        }
        //bool notOwned = true;
        //for (int i = 0; i < saveData.ownedMedicineList.Count; i++)
        //{
        //    if (nowMedicineStruct.medicine.GetIndex() == saveData.ownedMedicineList[i])
        //    {
        //        notOwned = false;
        //        break;
        //    }
        //}
        //if (notOwned)
        //{
        //    saveData.ownedMedicineList.Add(nowMedicineStruct.medicine.GetIndex());
        //}

        if (owningMedicine == null)
        {
            owningMedicine = new OwningMedicineClass();
            saveData.owningMedicineList.Add(owningMedicine);
            owningMedicine.medicineIndex = nowMedicineStruct.medicine.GetIndex();
            //owningMedicine.medicineQuantity = 1;
            Debug.Log("오우닝 메디슨 추가.");
        }
        else
        {
            Debug.Log("오우닝 메디슨 인덱스 : " +owningMedicine.medicineIndex);
            //owningMedicine.medicineQuantity++;
        }
        

        nowMedicineStruct.clickerObject.SetActive(false);
        nowMedicineTile.clickedArray[nowIndex] = true;
        if (second)
        {
            nowMedicineStruct.medicineParent.SetActive(false);
        }
        exploreManager.OnBuyMedicine(nowMedicineStruct.medicine.GetIndex(), 1);
        exploreManager.TimeChange(60);

    }

    public void OnBackButton()
    {
        nowMedicineTileStruct.parentCanvas.SetActive(false);
    }


}
