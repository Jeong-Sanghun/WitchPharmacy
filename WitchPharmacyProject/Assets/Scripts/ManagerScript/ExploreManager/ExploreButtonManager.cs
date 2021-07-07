using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
public class ExploreButtonManager : MonoBehaviour
{
    ExploreManager exploreManager;
    GameManager gameManager;
    SaveDataClass saveData;
    List<MedicineClass> medicineDataList;
    [SerializeField]
    Button[] regionOpenButtonArray;
    RegionProperty[] regionPropertyArray;
    [SerializeField]
    GameObject infoParent;
    [SerializeField]
    Image[] appearingMedicineImageArray;
    [SerializeField]
    Image nowMedicineImage;
    [SerializeField]
    Text medicineNameText;
    [SerializeField]
    Text medicineSymptomText;
    [SerializeField]
    Text medicineToolTipText;
    [SerializeField]
    Text exploreStartButtonText;
    [SerializeField]
    Text regionNameText;
    [SerializeField]
    Text regionMedicineTypeText;
    [SerializeField]
    Text timeText;
    int nowRegionIndex;
    RegionProperty nowProperty;

    // Start is called before the first frame update
    void Start()
    {
        exploreManager = ExploreManager.inst;
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        medicineDataList = gameManager.medicineDataWrapper.medicineDataList;

        regionPropertyArray = gameManager.regionPropertyWrapper.regionPropertyArray;

        for(int i = 0; i < regionOpenButtonArray.Length; i++)
        {
            regionOpenButtonArray[i].interactable = false;
        }

        for(int i = 0; i < saveData.unlockedRegionIndex.Count; i++)
        {
            regionOpenButtonArray[saveData.unlockedRegionIndex[i]].interactable = true;
        }
    }

    public void OnInfoButton(int index)
    {

        nowRegionIndex = index;
        nowProperty = regionPropertyArray[index];
        exploreStartButtonText.text = "탐험시작 " + (nowProperty.entranceTime / 60).ToString() + "분";
        for(int i = 0; i < appearingMedicineImageArray.Length; i++)
        {
            //여기 조심해야함. property랑 메디슨 어레이랑 인덱스 개수가 맞아야함.
            appearingMedicineImageArray[i].sprite = medicineDataList[nowProperty.regionAvailableMedicine[i]].LoadImage();
        }

        int hour = (int)gameManager.nowTime / 3600;
        int minute = ((int)gameManager.nowTime % 3600) / 60;
        StringBuilder builder = new StringBuilder(hour.ToString());
        builder.Append("시");
        builder.Append(minute.ToString());
        builder.Append("분");
        timeText.text = builder.ToString();

        regionNameText.text = nowProperty.regionName;
        regionMedicineTypeText.text = nowProperty.regionMedicineType;

        infoParent.SetActive(true);
        OnMedicineInfoButton(0);

    }

    public void OnInfoBackButton()
    {
        infoParent.SetActive(false);
    }

    //약재 하나를 눌렀을 때 뜨는 거.
    public void OnMedicineInfoButton(int index)
    {
        MedicineClass nowMedicine = medicineDataList[nowProperty.regionAvailableMedicine[index]];
        StringBuilder text = new StringBuilder(nowMedicine.firstName);
        text.Append(" ");
        text.Append(nowMedicine.secondName);
        medicineNameText.text = text.ToString();

        //여기서부터 더미. UI나오면 바꿔야함.
        text = new StringBuilder(nowMedicine.firstSymptomText);
        
        if(nowMedicine.firstNumber == 1)
        {
            text.Append("+");
        }
        else
        {
            text.Append("-");
        }
        text.Append(nowMedicine.secondSymptomText);
        if (nowMedicine.secondNumber == 2)
        {
            text.Append("++");
        }
        else
        {
            text.Append("--");
        }
        medicineSymptomText.text = text.ToString();

        medicineToolTipText.text = nowMedicine.toolTip;

        nowMedicineImage.sprite = nowMedicine.LoadImage();

    }

    //탐험시작 버튼 누르면 들어옴.
    public void OnExploreStart()
    {
        exploreManager = ExploreManager.inst;
        
        exploreManager.OnRegionLoad(nowRegionIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
