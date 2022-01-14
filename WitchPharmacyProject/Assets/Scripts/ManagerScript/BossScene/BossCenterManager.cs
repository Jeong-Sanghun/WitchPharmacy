using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCenterManager : MonoBehaviour
{
    BossDataWrapper bossDataWrapper;
    List<MedicineClass> ownedMedicineList;
    
    //다른데서 존나 가져다 쓸거.
    public BossData bossData;
    GameManager gameManager;
    SaveDataClass saveData;
    public bool isGameEnd;
    // Start is called before the first frame update
    void Start()
    {
        JsonManager json = new JsonManager();
        bossDataWrapper = json.ResourceDataLoad<BossDataWrapper>("BossDataWrapper");
        gameManager = GameManager.singleton;
        saveData = gameManager.saveData;
        ownedMedicineList = new List<MedicineClass>();
        
        for (int i = 0; i < saveData.owningMedicineList.Count; i++)
        {
            ownedMedicineList.Add(gameManager.medicineDataWrapper.medicineDataList[saveData.owningMedicineList[i].medicineIndex]);
        }
        BossSymptom.SetStaticData(ownedMedicineList);
        string nowCharacter = saveData.nowBossFile;

        for(int i = 0; i < bossDataWrapper.bossDataList.Count; i++)
        {
            if(bossDataWrapper.bossDataList[i].fileName == nowCharacter)
            {
                bossData = bossDataWrapper.bossDataList[i];
                break;
            }
        }
        bossData.SetUp();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
