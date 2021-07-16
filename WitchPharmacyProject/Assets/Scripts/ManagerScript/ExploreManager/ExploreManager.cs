using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

//지역탐색에 들어간 후 지역을 고를 때 나옴.
public class ExploreManager : MonoBehaviour
{
    const int regionQuantity = 10;
    public static ExploreManager inst;
    GameManager gameManager;
    SceneManager sceneManager;
    RegionMaker regionMaker;
    RegionPropertyWrapper regionPropertyWrapper;
    RegionIngame[] regionIngameArray;
    List<MedicineClass> medicineDataList;
    List<StoreToolClass> storeToolDataList;
    List<SpecialMedicineClass> specialMedicienDataList;
    //bool[] visitedRegionArray;
    int nowIndex;

    List<OwningMedicineClass> gainedMedicineList;
    List<OwningMedicineClass> gainedSpecialMedicineList;
    List<OwningToolClass> gainedToolList;
    List<string> gainedDocumentNameList;
    int gainedCoin = 0;
    

    //이거 타일매니저로 넘겨줘야되는데 regionManager에서 타일매니저로 넘겨줌
    public RegionProperty nowProperty;
    public RegionIngame nowRegionIngame;

    
    //이거 단위 초임.
    public float nowTime;

    //GameObject.find씀
    Text timeText;

    [SerializeField]
    Sprite coinSprite;

    [SerializeField]
    GameObject gainedItemCanvas;

    [SerializeField]
    GameObject itemPrefab;
    [SerializeField]
    Image prefabItemImage;
    [SerializeField]
    Text prefabItemText;
    
    // Start is called before the first frame update
    void Awake()
    {
        if(inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(gainedItemCanvas);
        }
        else
        {
            Destroy(gainedItemCanvas);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sceneManager = SceneManager.inst;
        gameManager = GameManager.singleTon;
        regionPropertyWrapper = gameManager.regionPropertyWrapper;

        medicineDataList = gameManager.medicineDataWrapper.medicineDataList;
        storeToolDataList = gameManager.storeToolDataWrapper.storeToolDataList;
        specialMedicienDataList = gameManager.specialMedicineDataWrapper.specialMedicineDataList;

        gainedMedicineList = new List<OwningMedicineClass>();
        gainedToolList = new List<OwningToolClass>();
        gainedSpecialMedicineList = new List<OwningMedicineClass>();
        gainedDocumentNameList = new List<string>();



        //이거도 더미. 나중에는 제이슨으로 받아올건데 지금은 임시로 램덤.
        //int roundRobin;

        //for (int j = 0; j < regionQuantity; j++)
        //{
        //    regionPropertyWrapper.regionPropertyArray[j].tileTypeArray[0] = 1;
        //    for (int k = 1; k < 8; k++)
        //    {
        //        regionPropertyWrapper.regionPropertyArray[j].tileTypeArray[k] = 0;
        //    }
        //}


        //for (int j = 0; j < regionQuantity; j++)
        //{
        //    for (int i = 1; i < RegionIngame.tileNumber; i++)
        //    {
        //        roundRobin = Random.Range(1, 8);
        //        regionPropertyWrapper.regionPropertyArray[j].tileTypeArray[roundRobin]++;
        //    }
        //}
        
        regionIngameArray = new RegionIngame[regionQuantity];
        //이거는 굳이 필요없을지도..
        //visitedRegionArray = new bool[regionQuantity];
        //for(int i = 0; i < regionQuantity; i++)
        //{
        //    visitedRegionArray[i] = false;
        //}

    }

    //버튼 눌렸을 때 ExploreButtonManager에서 불러옴.
    public void OnRegionLoad(int index)
    {
        nowIndex = index;
        nowProperty = regionPropertyWrapper.regionPropertyArray[nowIndex];
        nowRegionIngame = regionIngameArray[nowIndex];
        sceneManager.LoadScene("RegionScene");

    }

    //지역 로드하면 만들어줘야댐. 이거 RegionMaker에서 Start로 불러옴.
    public void OnRegionLoaded(RegionMaker _regionMaker)
    {
        regionMaker = _regionMaker;
        timeText = GameObject.Find("TimeText").GetComponent<Text>();
        if (regionIngameArray[nowIndex] == null)
        {
            regionIngameArray[nowIndex] = regionMaker.EdgeMaker(null, regionPropertyWrapper.regionPropertyArray[nowIndex]);
        }
        else
        {
            regionMaker.EdgeMaker(regionIngameArray[nowIndex], regionPropertyWrapper.regionPropertyArray[nowIndex]);
        }

    }

    //MedicineTileManager에서 불러옴
    public void OnBuyMedicine(int index, int quantity)
    {
        OwningMedicineClass gainedMedicine = null;
        for(int i = 0; i < gainedMedicineList.Count; i++)
        {
            if(gainedMedicineList[i].medicineIndex == index)
            {
                gainedMedicine = gainedMedicineList[i];
                break;
            }
        }

        if (gainedMedicine == null)
        {
            gainedMedicine = new OwningMedicineClass();
            gainedMedicine.medicineIndex = index;
            gainedMedicine.medicineQuantity = quantity;
            gainedMedicineList.Add(gainedMedicine);
        }
        else
        {
            gainedMedicine.medicineQuantity += quantity;
        }
    }

    public void OnBuySpecialMedicine(int index, int quantity)
    {
        OwningMedicineClass gainedSpecialMedicine = null;
        for (int i = 0; i < gainedSpecialMedicineList.Count; i++)
        {
            if (gainedSpecialMedicineList[i].medicineIndex == index)
            {
                gainedSpecialMedicine = gainedMedicineList[i];
                break;
            }
        }

        if (gainedSpecialMedicine == null)
        {
            gainedSpecialMedicine = new OwningMedicineClass();
            gainedSpecialMedicine.medicineIndex = index;
            gainedSpecialMedicine.medicineQuantity = quantity;
            gainedMedicineList.Add(gainedSpecialMedicine);
        }
        else
        {
            gainedSpecialMedicine.medicineQuantity += quantity;
        }
    }

    //StoreTileManager에서불러옴
    public void OnBuyTool(int index, int quantity)
    {
        OwningToolClass gainedTool = null;
        for (int i = 0; i < gainedToolList.Count; i++)
        {
            if (gainedToolList[i].index == index)
            {
                gainedTool = gainedToolList[i];
                break;
            }
        }

        if (gainedTool == null)
        {
            gainedTool = new OwningToolClass();
            gainedTool.index = index;
            gainedTool.quantity = quantity;
            gainedToolList.Add(gainedTool);
        }
        else
        {
            gainedTool.quantity += quantity;
        }
    }

    public void OnCoinGain(int quantity)
    {
        gainedCoin += quantity;
    }

    public void OnDocumentGain(string fileName)
    {
        if (gainedDocumentNameList.Contains(fileName))
        {
            Debug.LogError("진짜졷됐음 다큐먼트가 두개 겹쳐뜸");
            return;
        }
        gainedDocumentNameList.Add(fileName);
    }

    public void OnTimeOut()
    {
        sceneManager.LoadScene("ExploreScene");
        int nowItemIndex = 0;
        for(int i = 0; i < gainedMedicineList.Count; i++)
        {
            prefabItemImage.sprite = medicineDataList[gainedMedicineList[i].medicineIndex].LoadImage();
            prefabItemText.text = gainedMedicineList[i].medicineQuantity.ToString();
            GameObject obj = Instantiate(itemPrefab, gainedItemCanvas.transform);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-800 + (nowItemIndex % 6) * 300, 300 - 300 * (nowItemIndex / 6));
            nowItemIndex++;
        }
        for (int i = 0; i < gainedToolList.Count; i++)
        {
            prefabItemImage.sprite = storeToolDataList[gainedToolList[i].index].LoadImage();
            prefabItemText.text = gainedToolList[i].quantity.ToString();
            GameObject obj = Instantiate(itemPrefab, gainedItemCanvas.transform);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-800 + (nowItemIndex % 6) * 300, 300 - 300 * (nowItemIndex / 6));
            nowItemIndex++;
        }

        for (int i = 0; i < gainedSpecialMedicineList.Count; i++)
        {
            prefabItemImage.sprite = medicineDataList[gainedSpecialMedicineList[i].medicineIndex].LoadImage();
            prefabItemText.text = gainedSpecialMedicineList[i].medicineQuantity.ToString();
            GameObject obj = Instantiate(itemPrefab, gainedItemCanvas.transform);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-800 + (nowItemIndex % 6) * 300, 300 - 300 * (nowItemIndex / 6));
            nowItemIndex++;
        }

        for (int i = 0; i < gainedDocumentNameList.Count; i++)
        {
            DocumentCondition condition = null;
            for(int j = 0; j < gameManager.documentConditionWrapper.documentConditionList.Count; j++)
            {
                if (gameManager.documentConditionWrapper.documentConditionList[j].fileName == gainedDocumentNameList[i])
                {
                    condition = gameManager.documentConditionWrapper.documentConditionList[j];
                    break;
                }
            }
            prefabItemImage.sprite = condition.LoadSprite();
            prefabItemText.text = null;
            GameObject obj = Instantiate(itemPrefab, gainedItemCanvas.transform);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-800 + (nowItemIndex % 6) * 300, 300 - 300 * (nowItemIndex / 6));
            nowItemIndex++;
        }
        prefabItemImage.sprite = coinSprite;
        prefabItemText.text = gainedCoin.ToString();
        GameObject coinObj = Instantiate(itemPrefab, gainedItemCanvas.transform);
        coinObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-800 + (nowItemIndex % 6) * 300, 300 - 300 * (nowItemIndex / 6));
        nowItemIndex++;

        itemPrefab.SetActive(false);

        gainedItemCanvas.SetActive(true);


    }

    //이거 버튼누를때마다 불러옴.
    //이거 단위 초다.
    public void TimeChange(float plusTime)
    {
        gameManager.TimeChange(plusTime);
        int hour = (int)gameManager.nowTime / 3600;
        int minute = ((int)gameManager.nowTime % 3600) / 60;
        StringBuilder builder = new StringBuilder(hour.ToString());
        builder.Append("시");
        builder.Append(minute.ToString());
        builder.Append("분");
        timeText.text = builder.ToString();
        if (hour >= 24)
        {
            OnTimeOut();
            gameManager.NextDay();
        }

    }

    //이거 RegionManager에서 불러옴.
    public RegionIngame GetRegionIngame()
    {
        return regionIngameArray[nowIndex];
    }

    public void DestroyOnEnd()
    {
        Destroy(gainedItemCanvas);
        Destroy(gameObject);
    }

    public void ToNextSceneButton()
    {
        gameManager.SaveJson();
        DestroyOnEnd();
        sceneManager.LoadScene("StoryScene");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnTimeOut();
        }
    }

}
