using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorTriggerManager : MonoBehaviour
{
    [SerializeField]
    CounterManager counterManager;
    [SerializeField]
    CounterDialogManager counterDialogManager;
    GameManager gameManager;
    SaveDataClass saveData;
    // Start is called before the first frame update
    //bool nowCheckingTrigger;

    
    List<SpecialVisitorCondition> specialVisitorConditionDataList;

    //List<string> todaySpecialVisitorList;
    int nowVisitorIndex;
    //bool isSecondVisit = false;
    //bool specialVisitorVisited;
    //int oddProgressingQuestIndex = 0;
    List<int> specialVisitorAppearingIndex;
    SpecialVisitorCondition nowSpecialVisitorCondition;

    void Start()
    {
        //nowCheckingTrigger = false;
        gameManager = GameManager.singleton;
        saveData = gameManager.saveData;

        specialVisitorConditionDataList = gameManager.specialVisitorConditionWrapper.specialVisitorConditionDataList;
        //todaySpecialVisitorList = new List<string>();
        specialVisitorAppearingIndex = new List<int>();
        nowVisitorIndex = 0;
        for(int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, 7);
            if (specialVisitorAppearingIndex.Contains(index))
            {
                i--;
                continue;
            }
            Debug.Log(index);
            specialVisitorAppearingIndex.Add(index);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (nowCheckingTrigger)
        //{
        //    if (Input.GetKeyDown(KeyCode.A))
        //    {
        //        nowCheckingTrigger = false;
        //        counterManager.CounterStart();
        //    }
        //    else if (Input.GetKeyDown(KeyCode.B) || Input.GetMouseButtonDown(0))
        //    {
        //        nowCheckingTrigger = false;

        //    }
           
        //}
    }

    //트루면 트리거체크, 폴스면 그냥 일반손님 혹은 띨빵손님.
    bool SpecialConditionCheck()
    {
        //if(specialVisitorVisited == true)
        //{
        //    return false;
        //}
        //if (specialVisitorAppearingIndex.Contains(nowVisitorIndex))
        //{
        //    return false;
        //}
        //float chaosMeter = gameManager.saveData.chaosMeter;
        //List<string> solvedQuest = gameManager.saveData.solvedQuestBundleName;
        //List<string> calledQuest = gameManager.saveData.calledQuestBundleName;
        //호출때렸으면 무조건 얘부터 나옴.
        //if(calledQuest.Count > 0)
        //{
        //    isSecondVisit = true;
        //    specialVisitorDialogBundle = gameManager.LoadVisitorBundle(calledQuest[0]);
        //    calledQuest.RemoveAt(0);
        //    return true;

        //}

        for (int i = 0; i < specialVisitorConditionDataList.Count; i++)
        {

            //    if (solvedQuest.Contains(specialVisitorConditionDataList[i].bundleName))
            //    {
            //        Debug.Log("해결한 퀘스트");
            //        continue;
            //    }
            //    if (saveData.progressingQuestBundleName.Contains(specialVisitorConditionDataList[i].bundleName))
            //    {
            //        Debug.Log("해결한 퀘스트");
            //        continue;
            //    }
            //    //if (todaySpecialVisitorList.Contains(specialVisitorConditionDataList[i].bundleName))
            //    //{
            //    //    Debug.Log("오늘 이미 방문함");
            //    //    continue;
            //    //}
            //    if (chaosMeter < specialVisitorConditionDataList[i].appearingChaosMeter)
            //    {
            //        Debug.Log("카오스미터가 안됨");
            //        continue;
            //    }
            bool read = false;
            for(int j = 0;j< gameManager.saveData.readSpecialVisitorBundleList.Count; j++)
            {
                if (gameManager.saveData.readSpecialVisitorBundleList[j].Contains(specialVisitorConditionDataList[i].bundleName))
                {
                    read = true;
                    break;
                }
            }
            if (read)
            {
                continue;
            }

            if (gameManager.saveData.nowDay != specialVisitorConditionDataList[i].appearingDay)
            {
                Debug.Log("데이가 안됨");
                continue;
            }
            if(specialVisitorConditionDataList[i].appearingSequence != nowVisitorIndex)
            {
                continue;
            }
            saveData.readSpecialVisitorBundleList.Add(specialVisitorConditionDataList[i].bundleName);
            nowSpecialVisitorCondition = specialVisitorConditionDataList[i];
            //specialVisitorFileName = specialVisitorConditionDataList[i].bundleName;
            return true;
            //    bool notContaining = false;
            //    for (int j = 0; j < specialVisitorConditionDataList[i].appearingSolvedQuestBundleList.Count; j++)
            //    {
            //        if (!gameManager.saveData.solvedQuestBundleName.
            //            Contains(specialVisitorConditionDataList[i].appearingSolvedQuestBundleList[j]))
            //        {
            //            notContaining = true;
            //            break;
            //        }
            //    }
            //    if (notContaining == true)
            //    {
            //        Debug.Log("솔브드퀘스트가 안됨");
            //        continue;
            //    }

            //    for (int j = 0; j < specialVisitorConditionDataList[i].appearingProgressingQuestBundleList.Count; j++)
            //    {
            //        if (!gameManager.saveData.progressingQuestBundleName.
            //            Contains(specialVisitorConditionDataList[i].appearingProgressingQuestBundleList[j]))
            //        {
            //            notContaining = true;
            //            break;
            //        }
            //    }
            //    if (notContaining == true)
            //    {
            //        Debug.Log("프로그레싱퀘스트 조건이 안됨");
            //        continue;
            //    }
            //    List<SpecialMedicineClass> medicineList = gameManager.specialMedicineDataWrapper.specialMedicineDataList;
            //    List<OwningMedicineClass> owningList = gameManager.saveData.owningSpecialMedicineList;
            //    for (int j = 0; j < specialVisitorConditionDataList[i].appearingSpecialMedicineList.Count; j++)
            //    {

            //        for (int k = 0; k < owningList.Count; k++)
            //        {
            //            if (medicineList[owningList[i].medicineIndex].fileName
            //                == specialVisitorConditionDataList[i].appearingSpecialMedicineList[j])
            //            {
            //                notContaining = false;
            //                break;
            //            }
            //            else
            //            {
            //                notContaining = true;
            //            }
            //        }
            //        if (notContaining == true)
            //        {
            //            break;
            //        }
            //    }
            //    if (notContaining == true)
            //    {
            //        Debug.Log("스페셜메디슨이 안됨");
            //        continue;
            //    }
            //    for (int j = 0; j < specialVisitorConditionDataList[i].appearingStoryBundleList.Count; j++)
            //    {
            //        if (!gameManager.saveData.readStoryList.
            //            Contains(specialVisitorConditionDataList[i].appearingStoryBundleList[j]))
            //        {
            //            notContaining = true;
            //            break;
            //        }
            //    }
            //    if (notContaining == true)
            //    {
            //        Debug.Log("스토리가 안됨");
            //        continue;
            //    }
            //    if (!notContaining)
            //    {

            //        specialVisitorDialogBundle = gameManager.LoadVisitorBundle(specialVisitorConditionDataList[i].bundleName);
            //        if (saveData.progressingQuestBundleName.Contains(specialVisitorDialogBundle.bundleName))
            //        {
            //            isSecondVisit = true;
            //        }
            //        else
            //        {
            //            isSecondVisit = false;
            //        }
            //        return true;
            //    }
        }
        return false;
    }

    bool OddConditionCheck()
    {
        //if(saveData.progressingQuestBundleName.Count == 0)
        //{
        //    return false;
        //}
        //if(oddProgressingQuestIndex >= saveData.progressingQuestBundleName.Count)
        //{
        //    return false;
        //}
        //if (!specialVisitorAppearingIndex.Contains(nowVisitorIndex))
        //{
        //    return false;
        //}
        //List<SpecialMedicineClass> specialMedicineList = gameManager.specialMedicineDataWrapper.specialMedicineDataList;
        //if (saveData.visitedOddVisitorName.Count == specialMedicineList.Count)
        //{
        //    return false;
        //}
        //SpecialVisitorCondition condition = null;

        //for(int i = 0; i < specialVisitorConditionDataList.Count; i++)
        //{
        //    if(saveData.progressingQuestBundleName[oddProgressingQuestIndex] == specialVisitorConditionDataList[i].bundleName)
        //    {
        //        condition = specialVisitorConditionDataList[i];
        //        break;
        //    }
        //}
        //if(condition == null)
        //{
        //    Debug.LogError("좆됐음");
        //    return false;
        //}
        //nowOddVisitorDialogBundle = gameManager.LoadOddBundle(condition.specialMedicine);

        //int randIndex = Random.Range(0, specialMedicineList.Count);


        //while (true)
        //{
        //    bool breaking = false;
        //    for (; randIndex < specialMedicineList.Count; randIndex++)
        //    {
        //        if (!saveData.visitedOddVisitorName.Contains(specialMedicineList[randIndex].fileName))
        //        {
        //            //nowOddVisitorDialogBundle = gameManager.LoadOddBundle(specialMedicineList[randIndex].fileName);
        //            breaking = true;
        //            break;
        //        }
        //    }

        //    if (breaking)
        //    {
        //        break;
        //    }
        //    randIndex = 0;
        //}
        //nowOddVisitorDialogBundle = gameManager.LoadOddBundle("meltfire");
        //saveData.visitedOddVisitorName.Add(nowOddVisitorDialogBundle.rewardSpecialMedicine);

        return false;
    }



    //카운터매니저에서 일 다 보고 이거 불러옴. Disappear하면은 이거 불러옴. 다음 손님 나와라~
    public void TriggerCheck()
    {
        if (SpecialConditionCheck())
        {
            //specialVisitorVisited = true;
            counterManager.CounterSpecialStart(nowSpecialVisitorCondition);
            //todaySpecialVisitorList.Add(specialVisitorDialogBundle.bundleName);
            //if (isSecondVisit)
            //{
            //    counterManager.CounterSecondStart(specialVisitorDialogBundle);
            //}
            //else
            //{
            //    counterManager.CounterFirstStart(specialVisitorDialogBundle);
            //}
            
        }
        else
        {
            counterManager.CounterStart();
        }
        nowVisitorIndex++;

    }
}
