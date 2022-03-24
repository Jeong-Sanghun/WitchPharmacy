using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using Obsolete;

public enum ResearchType
{
    Medicine,MeasureTool,OtherTool
}

public class RegionManager : MonoBehaviour
{
    [SerializeField] BlurManager blurManager;
    protected SceneManager sceneManager;
    GameManager gameManager;
    SaveDataClass saveData;

    //protected List<ConversationDialogBundle> conversationDialogBundleList;
    
    ConversationDialogBundle nowBundle;
    ConversationDialogWrapper nowWrapper;
    ConversationRouter nowRouter;
    StoryParser storyParser;

    CharacterIndexToName characterIndexToName;

    [SerializeField]
    SpriteRenderer[] characterSprite;
    [SerializeField]
    SpriteRenderer cutSceneBGSprite;

    [SerializeField]
    Text conversationText;
    [SerializeField]
    Text nameText;

    [SerializeField]
    Text[] routingTextArray;
    [SerializeField]
    GameObject[] routingButtonArray;


    float downYpos = -10;
    float upYpos = 0;

    //어느 번들인지.
    //int nowBundleIndex;
    //어디에서 분기해서 어디 래퍼인지.
    protected int nowWrapperIndex;
    protected int nowConversationIndex;
    protected bool checkingRouter;
    bool[] faded;
    bool blurred;
    protected int nowRouterIndex;
    bool nowInRouterWrapper;
    int leftRouterWrapper;
    int nowRouterWrapperIndex;

    RegionDataWrapper regionDataWrapper;
    List<RegionSaveData> regionSaveDataList;
    ExploreManager exploreManager;
    RegionName nowRegion;
    RegionSaveData nowRegionSaveData;
    RegionEvent nowRegionEvent;
    DocumentConditionWrapper documentConditionWrapper;
    SpecialEventCondition nowSpecialEvent;
    string nowDocument;
    int nowMedicineIndex;
    bool isFirstDiscount;
    string nowStoryBundleName;
    string nowResearch;
    string nowUnhiddenResearch;
    int nowProgressNumber;
    ResearchType nowResearchType;

    [SerializeField]
    Image[] iconArray;
    [SerializeField]
    Text[] bigTextArray;
    [SerializeField]
    Text[] smallTextArray;

    [SerializeField]
    GameObject popupParent;
    [SerializeField]
    Sprite coinSprite;


    const int progressingResearchTime = 3;
    const float discountRate = 0.7f;


    // Start is called before the first frame update
    void Start()
    {
        exploreManager = ExploreManager.inst;
        gameManager = GameManager.singleton;
        saveData = gameManager.saveData;
        sceneManager = SceneManager.inst;
        regionDataWrapper = exploreManager.regionDataWrapper;
        documentConditionWrapper = gameManager.documentConditionWrapper;
        regionSaveDataList = saveData.regionSaveDataList;

        nowRegion = exploreManager.nowRegion;
        for (int i = 0; i < regionSaveDataList.Count; i++)
        {
            if (nowRegion == regionSaveDataList[i].regionName)
            {
                nowRegionSaveData = regionSaveDataList[i];
                break;
            }
        }
        if (nowRegionSaveData == null)
        {
            nowRegionSaveData = new RegionSaveData();
            regionSaveDataList.Add(nowRegionSaveData);
            nowRegionSaveData.regionName = nowRegion;
        }

        //여기서 조건체크 다 해주고 아래에서 그거에 맞는 스토리 표출해야함
        if (CheckSpecialEvent())
        {
            Debug.Log("조건체크 트루");
            nowStoryBundleName = nowSpecialEvent.fileName;
        }
        else
        {
            Debug.Log("조건체크 폴스");
            GenerateRegularReward();
        }
        saveData.forcedRegion = null;
        characterIndexToName = new CharacterIndexToName();
        checkingRouter = false;
        blurred = false;
        faded = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            faded[i] = false;
            characterSprite[i].color = new Color(1, 1, 1, 0.2f);
        }
        nowConversationIndex = 0;
        nowWrapperIndex = 0;
        nowRouterIndex = 0;
        storyParser = new StoryParser(characterIndexToName, gameManager.languagePack);
        nowBundle = storyParser.LoadBundle(nowStoryBundleName, gameManager.saveDataTimeWrapper.nowLanguageDirectory,true,nowRegion,saveData.nowRegion);
        nowWrapper = nowBundle.dialogWrapperList[0];
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(nowWrapper.characterName[i]);
            if (nowWrapper.characterName[i] != null)
                characterSprite[i].sprite = characterIndexToName.GetSprite(nowWrapper.characterName[i], nowWrapper.characterFeeling[i]);
            else
                characterSprite[i].sprite = null;
        }
        for (int i = 0; i < nowWrapper.startEffectList.Count; i++)
        {
            DialogEffect effect = nowWrapper.startEffectList[i];
            GameObject obj = characterSprite[(int)effect.characterPosition].gameObject;
            if (effect.effect == DialogFX.Up)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, downYpos, 0);
                StartCoroutine(sceneManager.MoveModule_Linear(obj, new Vector3(obj.transform.position.x, upYpos, 0), 1));
            }
            else if (effect.effect == DialogFX.Down)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, upYpos, 0);
                StartCoroutine(sceneManager.MoveModule_Linear(obj, new Vector3(obj.transform.position.x, downYpos, 0), 1));
            }
        }
        if (nowWrapper.isCutscene)
        {
            cutSceneBGSprite.sprite = characterIndexToName.GetBackGroundSprite(nowWrapper.cutSceneFileName, true);
            if (nowWrapper.cutSceneEffect == CutSceneEffect.Blur)
            {
                blurred = true;
                blurManager.OnBlur(true);
            }
        }
        else
        {
            cutSceneBGSprite.sprite = characterIndexToName.GetBackGroundSprite(nowWrapper.backGroundFileName, false);
            if (nowWrapper.backGroundEffect == CutSceneEffect.Blur)
            {
                blurred = true;
                blurManager.OnBlur(true);
            }
        }
        PrintConversation();

    }

    bool CheckSpecialEvent()
    {
        RegionData data = regionDataWrapper.regionDataList[(int)nowRegion];
        nowSpecialEvent = null;
        
        for(int  i = 0; i < data.specialEventConditionList.Count; i++)
        {
            SpecialEventCondition condition = data.specialEventConditionList[i];
            bool contains = false;
            for (int j = 0;j < nowRegionSaveData.seenSpecialEventList.Count; j++)
            {
                if (nowRegionSaveData.seenSpecialEventList[j].Contains(condition.fileName))
                {
                    contains = true;
                    break;
                }
            }
            if (contains)
            {
                continue;
            }


            bool cont = false;
            if(saveData.nowDay< condition.leastDayCondition)
            {
                continue;
            }
            for (int k = 0; k < condition.questConditionList.Count; k++)
            {
                bool contain = false;
                for (int j = 0; j < saveData.endQuestList.Count; j++)
                {
                    if (saveData.endQuestList[j].Contains(condition.questConditionList[k]))
                    {
                        contain = true;
                        break;
                    }
                }
                if (contain == false)
                {
                    cont = true;
                    break;
                }
            }
            if (cont)
            {
                continue;
            }

            for (int k = 0; k < condition.routeConditionList.Count; k++)
            {
                bool contain = false;
                for (int j = 0; j < saveData.routePairList.Count; j++)
                {
                    if (saveData.routePairList[j].storyName.Contains(condition.routeConditionList[k].storyName))
                    {
                        bool pick = true;
                        for(int m = 0; m < saveData.routePairList[j].pickedRouteList.Count; m++)
                        {
                            if(saveData.routePairList[j].pickedRouteList[m] != condition.routeConditionList[k].pickedRouteList[m])
                            {
                                pick = false;
                                break;
                            }
                        }
                        if(pick == false)
                        {
                            contain = false;
                            break;
                        }
                        else
                        {
                            contain = true;
                        }
                    }
                }
                if (contain == false)
                {
                    cont = true;
                    break;
                }
            }
            if (cont)
            {
                continue;
            }

            ResearchSaveData researchSaveData = saveData.researchSaveData;
            nowResearch = null;
            nowUnhiddenResearch = null;
            nowMedicineIndex = -1;

            if (condition.eventType == RegionEvent.DocumentMedicine || condition.eventType == RegionEvent.DocumentResearch || condition.eventType == RegionEvent.DocumentUnhiddenResearch)
            {
                bool notContain = true;
                for (int j = 0; j < saveData.owningDocumentList.Count; j++)
                {
                    if (saveData.owningDocumentList[j].name.Contains(condition.rewardDocument))
                    {
                        notContain = false;
                        break;
                    }
                }
                if (notContain)
                {
                    nowDocument = condition.rewardDocument;
                }
                else
                {
                    continue;
                }
            }

            if (condition.eventType == RegionEvent.ResearchProgress || condition.eventType == RegionEvent.DocumentResearch)
            {
                bool find = false;
                for (int j = 0; j < researchSaveData.endMeasureToolResearchList.Count; j++)
                {
                    if (researchSaveData.endMeasureToolResearchList[j].Contains(condition.reward))
                    {
                        find = true;
                        break;
                    }

                }
                if (!find)
                {

                    for (int j = 0; j < researchSaveData.endMedicineResearchList.Count; j++)
                    {
                        if (researchSaveData.endMedicineResearchList[j].Contains(condition.reward))
                        {
                            find = true;
                            break;
                        }

                    }
                }
                if (!find)
                {
                    for (int j = 0; j < researchSaveData.endOtherToolResearchList.Count; j++)
                    {
                        if (researchSaveData.endOtherToolResearchList[j].Contains(condition.reward))
                        {
                            find = true;
                            break;
                        }

                    }
                }
                if(find == true)
                {
                    continue;
                }
                find = false;
                ResearchData researchData = null;
                for (int j = 0; j < gameManager.otherToolResearchDataWrapper.otherToolResearchDataList.Count; j++)
                {
                    if (gameManager.otherToolResearchDataWrapper.otherToolResearchDataList[j].fileName.Contains(condition.reward))
                    {
                        researchData = gameManager.otherToolResearchDataWrapper.otherToolResearchDataList[j];
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    for (int j = 0; j < gameManager.measureToolResearchDataWrapper.measureToolResearchDataList.Count; j++)
                    {
                        if (gameManager.measureToolResearchDataWrapper.measureToolResearchDataList[j].fileName.Contains(condition.reward))
                        {
                            researchData = gameManager.measureToolResearchDataWrapper.measureToolResearchDataList[j];
                            find = true;
                            break;
                        }
                    }
                }

                if (!find)
                {
                    for (int j = 0; j < gameManager.medicineResearchDataWrapper.medicineResearchDataList.Count; j++)
                    {
                        if (gameManager.medicineResearchDataWrapper.medicineResearchDataList[j].fileName.Contains(condition.reward))
                        {
                            researchData = gameManager.medicineResearchDataWrapper.medicineResearchDataList[j];
                            find = true;
                            break;
                        }
                    }
                }

                if (researchData.hidden == true)
                {
                    find = false;
                    for (int j = 0; j < researchSaveData.unHiddenResearchList.Count; j++)
                    {
                        if (researchSaveData.unHiddenResearchList[j].Contains(condition.reward))
                        {
                            find = true;
                            break;
                        }
                    }
                    if (find == false)
                    {
                        continue;
                    }
                }

                nowResearch = condition.reward;
                nowProgressNumber = condition.researchProgress;
            }
            else if(condition.eventType == RegionEvent.UnhiddenResearch || condition.eventType == RegionEvent.DocumentUnhiddenResearch)
            {
                bool find = false;
                ResearchData researchData = null;
                for (int j = 0; j < gameManager.otherToolResearchDataWrapper.otherToolResearchDataList.Count; j++)
                {
                    if (gameManager.otherToolResearchDataWrapper.otherToolResearchDataList[j].fileName.Contains(condition.reward))
                    {
                        researchData = gameManager.otherToolResearchDataWrapper.otherToolResearchDataList[j];
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    for (int j = 0; j < gameManager.measureToolResearchDataWrapper.measureToolResearchDataList.Count; j++)
                    {
                        if (gameManager.measureToolResearchDataWrapper.measureToolResearchDataList[j].fileName.Contains(condition.reward))
                        {
                            researchData = gameManager.measureToolResearchDataWrapper.measureToolResearchDataList[j];
                            find = true;
                            break;
                        }
                    }
                }

                if (!find)
                {
                    for (int j = 0; j < gameManager.medicineResearchDataWrapper.medicineResearchDataList.Count; j++)
                    {
                        if (gameManager.medicineResearchDataWrapper.medicineResearchDataList[j].fileName.Contains(condition.reward))
                        {
                            researchData = gameManager.medicineResearchDataWrapper.medicineResearchDataList[j];
                            find = true;
                            break;
                        }
                    }
                }
                find = false;
                if (researchData.hidden == true)
                {
                    for (int j = 0; j < researchSaveData.unHiddenResearchList.Count; j++)
                    {
                        if (researchSaveData.unHiddenResearchList[j].Contains(condition.reward))
                        {
                            find = true;
                            break;
                        }
                    }
                }
                else
                {
                    //심각한 오류
                    Debug.Log("찾은 데이터가 히든이 아님");
                    continue;
                }
                if (find == true)
                {
                    continue;
                }
                nowUnhiddenResearch = condition.reward;
            }
            else if(condition.eventType == RegionEvent.DocumentMedicine || condition.eventType == RegionEvent.MedicineDiscount)
            {
                bool disable = false;
                for (int j = 0; j < saveData.owningMedicineList.Count; j++)
                {
                    if (saveData.owningMedicineList[j].medicineIndex == condition.rewardMedicineIndex)
                    {
                        if (nowRegionSaveData.secondDiscountedMedicineIndex.Contains(condition.rewardMedicineIndex))
                        {
                            disable = true;
                            break;
                        }
                    }
                }
                if (disable)
                {
                    continue;
                }
                nowMedicineIndex = condition.rewardMedicineIndex;
            }
            nowSpecialEvent = condition;
            nowRegionEvent = condition.eventType;
            nowRegionSaveData.seenSpecialEventList.Add(nowSpecialEvent.fileName);
            return true;
        }
        return false;
    }

    //여기서 조건 체크해주고 보상이 뭔지 다 보내버림.
    void GenerateRegularReward()
    {
        RegionData data = regionDataWrapper.regionDataList[(int)nowRegion];
        List<int> rewardNumPool = new List<int>();
        int rewardIndex = -1;
        List<int> discountableMedicine = new List<int>();
        List<string> progressableResearchList = new List<string>();
        //List<string> unHiddenableReserachList = new List<string>();

        nowMedicineIndex = -1;
        for(int i = 0; i < data.appearingMedicineArray.Length; i++)
        {
            for(int j = 0; j < saveData.owningMedicineList.Count; j++)
            {
                if(saveData.owningMedicineList[j].medicineIndex == data.appearingMedicineArray[i])
                {
                    if (!nowRegionSaveData.secondDiscountedMedicineIndex.Contains(data.appearingMedicineArray[i]))
                    {
                        discountableMedicine.Add(data.appearingMedicineArray[i]);
                    }
                }
            }
        }
        if (discountableMedicine.Count > 0)
        {
            nowMedicineIndex = discountableMedicine[Random.Range(0, discountableMedicine.Count)];
        }

        ResearchSaveData researchSaveData = saveData.researchSaveData;
        nowResearch = null;
        nowUnhiddenResearch = null;

        for (int i = 0; i < data.appearingResearchArray.Length; i++)
        {
            bool find = false;
            for (int j = 0; j < researchSaveData.endMeasureToolResearchList.Count; j++)
            {
                if (researchSaveData.endMeasureToolResearchList[j].Contains(data.appearingResearchArray[i]))
                {
                    find = true;
                    break;
                }
                    
            }
            if (!find)
            {

                for (int j = 0; j < researchSaveData.endMedicineResearchList.Count; j++)
                {
                    if (researchSaveData.endMedicineResearchList[j].Contains(data.appearingResearchArray[i]))
                    {
                        find = true;
                        break;
                    }
                        
                }
            }
            if (!find)
            {
                for (int j = 0; j < researchSaveData.endOtherToolResearchList.Count; j++)
                {
                    if (researchSaveData.endOtherToolResearchList[j].Contains(data.appearingResearchArray[i]))
                    {
                        find = true;
                        break;
                    }
                        
                }
            }
            if (find == true)
            {
                continue;
            }
            find = false;
            ResearchData researchData = null;
            for (int j = 0; j < gameManager.otherToolResearchDataWrapper.otherToolResearchDataList.Count; j++)
            {
                if (gameManager.otherToolResearchDataWrapper.otherToolResearchDataList[j].fileName.Contains(data.appearingResearchArray[i]))
                {
                    researchData = gameManager.otherToolResearchDataWrapper.otherToolResearchDataList[j];
                    find = true;
                    break;
                }
            }
            if (!find)
            {
                for (int j = 0; j < gameManager.measureToolResearchDataWrapper.measureToolResearchDataList.Count; j++)
                {
                    if (gameManager.measureToolResearchDataWrapper.measureToolResearchDataList[j].fileName.Contains(data.appearingResearchArray[i]))
                    {
                        researchData = gameManager.measureToolResearchDataWrapper.measureToolResearchDataList[j];
                        find = true;
                        break;
                    }
                }
            }

            if (!find)
            {
                for (int j = 0; j < gameManager.medicineResearchDataWrapper.medicineResearchDataList.Count; j++)
                {
                    if (gameManager.medicineResearchDataWrapper.medicineResearchDataList[j].fileName.Contains(data.appearingResearchArray[i]))
                    {
                        researchData = gameManager.medicineResearchDataWrapper.medicineResearchDataList[j];
                        find = true;
                        break;
                    }
                }
            }

            if(researchData.hidden == true)
            {
                find = false;
                for (int j = 0; j < researchSaveData.unHiddenResearchList.Count; j++)
                {
                    if (researchSaveData.unHiddenResearchList[j].Contains(data.appearingResearchArray[i]))
                    {
                        find = true;
                        break;
                    }
                }
                if (find == false)
                {
                    continue;
                }
            }

            nowProgressNumber = 3;
            progressableResearchList.Add(data.appearingResearchArray[i]);
        }

        //for(int i = 0; i < data.unHiddenResearchArray.Length; i++)
        //{
        //    bool find = false;
        //    for(int j = 0; j < researchSaveData.unHiddenResearchList.Count; j++)
        //    {
        //        if (researchSaveData.unHiddenResearchList[j].Contains(data.unHiddenResearchArray[i]))
        //        {
        //            find = true;
        //            break;
        //        }
        //    }
        //    if (find == false)
        //    {
        //        unHiddenableReserachList.Add(data.unHiddenResearchArray[i]);
        //    }
            
        //}
        if (progressableResearchList.Count > 0)
        {
            nowResearch = progressableResearchList[Random.Range(0, progressableResearchList.Count)];
        }
        //if(unHiddenableReserachList.Count > 0)
        //{
        //    nowUnhiddenResearch = unHiddenableReserachList[Random.Range(0, unHiddenableReserachList.Count)];
        //}



        nowDocument = null;
        for (int j = 0; j < data.appearingDocumentArray.Length; j++)
        {
            bool notContain = true;
            for (int i = 0; i < saveData.owningDocumentList.Count; i++)
            {
                if (saveData.owningDocumentList[i].name.Contains(data.appearingDocumentArray[j]))
                {
                    notContain = false;
                    break;
                }
            }
            if (notContain)
            {
                nowDocument = data.appearingDocumentArray[j];
                break;
            }
        }


        for (int i = 0; i < data.eventTimeArray.Length; i++)
        {
            if(nowDocument == null && ( (RegionEvent)i == RegionEvent.DocumentResearch || (RegionEvent)i == RegionEvent.DocumentMedicine))
            {
                continue;
            }
            if(nowMedicineIndex == -1 && ((RegionEvent)i == RegionEvent.MedicineDiscount || (RegionEvent)i == RegionEvent.DocumentMedicine))
            {
                continue;
            }
            if (nowResearch == null && ((RegionEvent)i == RegionEvent.DocumentResearch || (RegionEvent)i == RegionEvent.ResearchProgress))
            {
                continue;
            }
            //if (nowUnhiddenResearch == null && ((RegionEvent)i == RegionEvent.UnhiddenResearch || (RegionEvent)i == RegionEvent.DocumentUnhiddenResearch))
            //{
            //    continue;
            //}
            if (data.eventTimeArray[i] - nowRegionSaveData.eventTimeArray[i] > 0)
            {
                rewardNumPool.Add(i);
            }
        }
        if(rewardNumPool.Count > 0)
        {
            rewardIndex = rewardNumPool[Random.Range(0, rewardNumPool.Count)];
        }
        if (rewardIndex == -1)
        {
            nowRegionEvent = RegionEvent.RandomCoin;
        }
        else
        {
            nowRegionEvent = (RegionEvent)rewardIndex;
        }
        StringBuilder builder = new StringBuilder(nowRegionEvent.ToString());
        builder.Append(Random.Range(0, 3).ToString());
        nowStoryBundleName = builder.ToString();
    }

    bool popupOnce = false;
    void RegionEndPopup()
    {
        if(popupOnce == true)
        {
            return;
        }
        popupOnce = true;
        switch (nowRegionEvent)
        {
            case RegionEvent.DocumentMedicine:
            case RegionEvent.MedicineDiscount:
                for(int i = 0; i < saveData.owningMedicineList.Count; i++)
                {
                    if(saveData.owningMedicineList[i].medicineIndex == nowMedicineIndex)
                    {
                        saveData.owningMedicineList[i].medicineCost = (int)(discountRate * saveData.owningMedicineList[i].medicineCost);
                        break;
                    }
                }
                if (nowRegionSaveData.firstDiscountedMedicineIndex.Contains(nowMedicineIndex))
                {
                    nowRegionSaveData.firstDiscountedMedicineIndex.Remove(nowMedicineIndex);
                    nowRegionSaveData.secondDiscountedMedicineIndex.Add(nowMedicineIndex);
                }
                else
                {
                    nowRegionSaveData.firstDiscountedMedicineIndex.Add(nowMedicineIndex);
                }
                MedicineClass nowMedicineData = gameManager.medicineDataWrapper.medicineDataList[nowMedicineIndex];
                iconArray[0].sprite = nowMedicineData.LoadImage();
                StringBuilder name = new StringBuilder(nowMedicineData.firstName);
                name.Append(" ");
                name.Append(nowMedicineData.secondName);
                name.Append("(");
                name.Append(gameManager.languagePack.symptomArray[(int)nowMedicineData.GetFirstSymptom()]);
                if(nowMedicineData.firstNumber == 1)
                {
                    name.Append("+");
                }
                else if (nowMedicineData.firstNumber == 2)
                {
                    name.Append("++");
                }
                else if (nowMedicineData.firstNumber == -1)
                {
                    name.Append("-");
                }
                else if (nowMedicineData.firstNumber == -2)
                {
                    name.Append("--");
                }
                name.Append(gameManager.languagePack.symptomArray[(int)nowMedicineData.GetSecondSymptom()]);
                if (nowMedicineData.secondNumber == 1)
                {
                    name.Append("+");
                }
                else if (nowMedicineData.secondNumber == 2)
                {
                    name.Append("++");
                }
                else if (nowMedicineData.secondNumber == -1)
                {
                    name.Append("-");
                }
                else if (nowMedicineData.secondNumber == -2)
                {
                    name.Append("--");
                }
                name.Append(")");
                bigTextArray[0].text = gameManager.languagePack.Insert(gameManager.languagePack.whatMedicineDiscount, name.ToString());
                smallTextArray[0].text = gameManager.languagePack.medicineDiscount;
                if(nowRegionEvent == RegionEvent.DocumentMedicine)
                {
                    DocumentCondition nowCondition = null;
                    OwningDocumentClass doc = new OwningDocumentClass();
                    doc.name = nowDocument;
                    doc.gainedDay = saveData.nowDay;
                    doc.gainedTime = saveData.nowTime;
                    doc.gainedRegion = nowRegion;
                    saveData.owningDocumentList.Add(doc);
                    TabletManager.inst.UpdateDocument(doc);
                    for(int  i =0;i< gameManager.documentConditionWrapper.documentConditionList.Count; i++)
                    {
                        if (gameManager.documentConditionWrapper.documentConditionList[i].fileName.Contains(doc.name))
                        {
                            nowCondition = documentConditionWrapper.documentConditionList[i];
                        }
                    }

                    iconArray[1].sprite = nowCondition.LoadSprite();
                    bigTextArray[1].text = gameManager.languagePack.Insert(gameManager.languagePack.whatDocumentGained, nowCondition.ingameName);
                    smallTextArray[1].text = gameManager.languagePack.documentGained;
                }
                else
                {
                    iconArray[1].gameObject.SetActive(false);
                    bigTextArray[1].gameObject.SetActive(false);
                    smallTextArray[1].gameObject.SetActive(false);
                }
                break;

            case RegionEvent.DocumentResearch:
            case RegionEvent.ResearchProgress:
                bool find = false;
                ResearchData researchData = null;
                for (int i = 0; i < gameManager.otherToolResearchDataWrapper.otherToolResearchDataList.Count; i++)
                {
                    if (gameManager.otherToolResearchDataWrapper.otherToolResearchDataList[i].fileName.Contains(nowResearch))
                    {
                        nowResearchType = ResearchType.OtherTool;
                        researchData = gameManager.otherToolResearchDataWrapper.otherToolResearchDataList[i];
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    for (int i = 0; i < gameManager.measureToolResearchDataWrapper.measureToolResearchDataList.Count; i++)
                    {
                        if (gameManager.measureToolResearchDataWrapper.measureToolResearchDataList[i].fileName.Contains(nowResearch))
                        {
                            nowResearchType = ResearchType.MeasureTool;
                            researchData = gameManager.measureToolResearchDataWrapper.measureToolResearchDataList[i];
                            find = true;
                            break;
                        }
                    }
                }

                if (!find)
                {
                    for (int i = 0; i < gameManager.medicineResearchDataWrapper.medicineResearchDataList.Count; i++)
                    {
                        if (gameManager.medicineResearchDataWrapper.medicineResearchDataList[i].fileName.Contains(nowResearch))
                        {
                            nowResearchType = ResearchType.Medicine;
                            researchData = gameManager.medicineResearchDataWrapper.medicineResearchDataList[i];
                            find = true;
                            break;
                        }
                    }
                }
                ResearchSaveData researchSaveData = saveData.researchSaveData;
                
                OneResearch research = null;
                List<OneResearch> oneResearchList = null;
                List<string> endReserachList = null;
                switch (nowResearchType)
                {
                    case ResearchType.Medicine:
                        iconArray[0].sprite = ((MedicineResearchData)researchData).LoadImage();
                        oneResearchList = researchSaveData.progressingMedicineResearchList;
                        endReserachList = researchSaveData.endMedicineResearchList;
                        break;
                    case ResearchType.MeasureTool:
                        iconArray[0].sprite = ((MeasureToolResearchData)researchData).LoadImage();
                        oneResearchList = researchSaveData.progressingMeasureToolResearchList;
                        endReserachList = researchSaveData.endMeasureToolResearchList;
                        break;
                    case ResearchType.OtherTool:
                        iconArray[0].sprite = ((OtherToolResearchData)researchData).LoadImage();
                        oneResearchList = researchSaveData.progressingOtherToolResearchList;
                        endReserachList = researchSaveData.endOtherToolResearchList;
                        break;
                }
                for (int i = 0; i < oneResearchList.Count; i++)
                {
                    if (oneResearchList[i].fileName.Contains(nowResearch))
                    {
                        research = oneResearchList[i];
                        break;
                    }
                }
                
                if (research == null)
                {
                    research = new OneResearch();
                    research.fileName = nowResearch;
                    research.researchedTime = nowProgressNumber;
                    if(research.researchedTime >= researchData.researchEndTime)
                    {
                        endReserachList.Add(nowResearch);
                    }
                    else
                    {
                        oneResearchList.Add(research);
                    }
                    
                }
                else
                {
                    research.researchedTime += nowProgressNumber;
                    if (research.researchedTime >= researchData.researchEndTime)
                    {
                        research.researchedTime = researchData.researchEndTime;
                        oneResearchList.Remove(research);
                        endReserachList.Add(nowResearch);
                    }
                }
                bigTextArray[0].text = gameManager.languagePack.Insert(gameManager.languagePack.whatResearchProgressed, researchData.ingameName);
                smallTextArray[0].text = gameManager.languagePack.researchProgressed;

                if (nowRegionEvent == RegionEvent.DocumentResearch)
                {
                    DocumentCondition nowCondition = null;
                    OwningDocumentClass doc = new OwningDocumentClass();
                    doc.name = nowDocument;
                    doc.gainedDay = saveData.nowDay;
                    doc.gainedTime = saveData.nowTime;
                    doc.gainedRegion = nowRegion;
                    saveData.owningDocumentList.Add(doc);
                    TabletManager.inst.UpdateDocument(doc);
                    for (int i = 0; i < gameManager.documentConditionWrapper.documentConditionList.Count; i++)
                    {
                        if (gameManager.documentConditionWrapper.documentConditionList[i].fileName.Contains(doc.name))
                        {
                            nowCondition = documentConditionWrapper.documentConditionList[i];
                        }
                    }

                    iconArray[1].sprite = nowCondition.LoadSprite();
                    bigTextArray[1].text = gameManager.languagePack.Insert(gameManager.languagePack.whatDocumentGained, nowCondition.ingameName);
                    smallTextArray[1].text = gameManager.languagePack.documentGained;
                }
                else
                {
                    iconArray[1].gameObject.SetActive(false);
                    bigTextArray[1].gameObject.SetActive(false);
                    smallTextArray[1].gameObject.SetActive(false);
                }


                break;
            case RegionEvent.UnhiddenResearch:
            case RegionEvent.DocumentUnhiddenResearch:
                researchSaveData = saveData.researchSaveData;
                find = false;
                researchSaveData.unHiddenResearchList.Add(nowUnhiddenResearch);

                researchData = null;
                for (int i = 0; i < gameManager.otherToolResearchDataWrapper.otherToolResearchDataList.Count; i++)
                {
                    if (gameManager.otherToolResearchDataWrapper.otherToolResearchDataList[i].fileName.Contains(nowUnhiddenResearch))
                    {
                        nowResearchType = ResearchType.OtherTool;
                        researchData = gameManager.otherToolResearchDataWrapper.otherToolResearchDataList[i];
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    for (int i = 0; i < gameManager.measureToolResearchDataWrapper.measureToolResearchDataList.Count; i++)
                    {
                        if (gameManager.measureToolResearchDataWrapper.measureToolResearchDataList[i].fileName.Contains(nowUnhiddenResearch))
                        {
                            nowResearchType = ResearchType.MeasureTool;
                            researchData = gameManager.measureToolResearchDataWrapper.measureToolResearchDataList[i];
                            find = true;
                            break;
                        }
                    }
                }

                if (!find)
                {
                    for (int i = 0; i < gameManager.medicineResearchDataWrapper.medicineResearchDataList.Count; i++)
                    {
                        if (gameManager.medicineResearchDataWrapper.medicineResearchDataList[i].fileName.Contains(nowUnhiddenResearch))
                        {
                            nowResearchType = ResearchType.Medicine;
                            researchData = gameManager.medicineResearchDataWrapper.medicineResearchDataList[i];
                            find = true;
                            break;
                        }
                    }
                }
                Debug.Log(nowUnhiddenResearch);
                bigTextArray[0].text = gameManager.languagePack.Insert(gameManager.languagePack.whatResearchUnhidden, researchData.ingameName);
                smallTextArray[0].text = gameManager.languagePack.researchUnhidden;

                if (nowRegionEvent == RegionEvent.DocumentUnhiddenResearch)
                {
                    DocumentCondition nowCondition = null;
                    OwningDocumentClass doc = new OwningDocumentClass();
                    doc.name = nowDocument;
                    doc.gainedDay = saveData.nowDay;
                    doc.gainedTime = saveData.nowTime;
                    doc.gainedRegion = nowRegion;
                    saveData.owningDocumentList.Add(doc);
                    TabletManager.inst.UpdateDocument(doc);
                    for (int i = 0; i < gameManager.documentConditionWrapper.documentConditionList.Count; i++)
                    {
                        if (gameManager.documentConditionWrapper.documentConditionList[i].fileName.Contains(doc.name))
                        {
                            nowCondition = documentConditionWrapper.documentConditionList[i];
                        }
                    }

                    iconArray[1].sprite = nowCondition.LoadSprite();
                    bigTextArray[1].text = gameManager.languagePack.Insert(gameManager.languagePack.whatDocumentGained, nowCondition.ingameName);
                    smallTextArray[1].text = gameManager.languagePack.documentGained;
                }
                else
                {
                    iconArray[1].gameObject.SetActive(false);
                    bigTextArray[1].gameObject.SetActive(false);
                    smallTextArray[1].gameObject.SetActive(false);
                }


                break;


            case RegionEvent.RandomCoin:
                int coin = Random.Range(100, 200);
                saveData.coin += coin;
                TabletManager.inst.UpdateBill(BillReason.exploreBoxGain, true, coin);
                bigTextArray[0].text = gameManager.languagePack.Insert(gameManager.languagePack.boxCoinGained, coin);
                smallTextArray[0].text = gameManager.languagePack.coinGained;
                iconArray[0].sprite = coinSprite;
                iconArray[1].gameObject.SetActive(false);
                bigTextArray[1].gameObject.SetActive(false);
                smallTextArray[1].gameObject.SetActive(false);
                break;

            //case RegionEvent.SpecialEvent:
            //    OwningMedicineClass medicine = new OwningMedicineClass();
            //    SpecialMedicineClass med = gameManager.specialMedicineDataWrapper.specialMedicineDataList[nowMedicineIndex];
            //    medicine.medicineIndex = nowMedicineIndex;
            //    medicine.medicineCost = med.cost;
            //    saveData.owningSpecialMedicineList.Add(medicine);
                
            //    StringBuilder medName = new StringBuilder(med.firstName);
            //    medName.Append(" ");
            //    medName.Append(med.secondName);
            //    if (gameManager.saveDataTimeWrapper.nowLanguageDirectory.Contains("Korean"))
            //    {
            //        string josa = gameManager.languagePack.GetCompleteWord(medName.ToString(), "을", "를");
            //        medName.Append(josa);
            //    }
            //    bigTextArray[0].text = gameManager.languagePack.Insert(gameManager.languagePack.boxGained, medName.ToString());
            //    smallTextArray[0].gameObject.SetActive(false);
            //    iconArray[0].sprite = med.LoadImage();
            //    iconArray[1].gameObject.SetActive(false);
            //    bigTextArray[1].gameObject.SetActive(false);
            //    smallTextArray[1].gameObject.SetActive(false);
            //    break;

        }

        popupParent.SetActive(true);
    }



    //그 버튼이 뜨는거임. 라우팅 버튼
    void RouteCheck()
    {
        //if (routingTime <= 0)
        //{
        //    conversationText.text = "이야기끝";
        //    if (!isTile)
        //    {
        //        toNextSceneButton.SetActive(true);
        //    }

        //    return;
        //}
        if (nowRouterIndex >= nowBundle.conversationRouterList.Count)
        {
            return;
        }
        ConversationRouter router = nowBundle.conversationRouterList[nowRouterIndex];
        nowRouter = router;
        if (router == null)
        {
            nowWrapperIndex++;
            nowWrapper = nowBundle.dialogWrapperList[nowWrapperIndex];
            return;
        }
        else if (router.routeButtonText.Count == 0)
        {
            if (nowWrapperIndex + 1 < nowBundle.dialogWrapperList.Count)
            {
                nowWrapperIndex++;
                nowWrapper = nowBundle.dialogWrapperList[nowWrapperIndex];
            }
            return;
        }
        checkingRouter = true;
        nowInRouterWrapper = true;
        for (int i = 0; i < routingButtonArray.Length; i++)
        {
            routingButtonArray[i].SetActive(false);
        }

        for (int i = 0; i < router.routeButtonText.Count; i++)
        {
            routingButtonArray[i].SetActive(true);
            routingTextArray[i].text = router.routeButtonText[i];
        }
        nowRouterIndex++;
    }

    public void NextWrapper()
    {
        Debug.Log("넥스트래퍼");
        if (nowInRouterWrapper)
        {
            leftRouterWrapper--;
            if (leftRouterWrapper <= 0)
            {
                nowInRouterWrapper = false;
                if (nowWrapperIndex >= nowBundle.dialogWrapperList.Count)
                {
                    RegionEndPopup();
                    return;
                }
                nowWrapper = nowBundle.dialogWrapperList[nowWrapperIndex];


            }
            else
            {
                nowRouterWrapperIndex++;
                nowWrapper = nowRouter.routingWrapperList[nowRouterWrapperIndex];

            }
        }
        else
        {
            nowWrapperIndex++;
            if (nowWrapper.nextWrapperIsRouter)
            {
                RouteCheck();
                return;
            }
            if (nowWrapperIndex >= nowBundle.dialogWrapperList.Count)
            {
                RegionEndPopup();
                return;
            }

            nowWrapper = nowBundle.dialogWrapperList[nowWrapperIndex];
        }

        nowConversationIndex = 0;
        for (int i = 0; i < 4; i++)
        {
            if (nowWrapper.characterName[i] != null)
                characterSprite[i].sprite = characterIndexToName.GetSprite(nowWrapper.characterName[i], nowWrapper.characterFeeling[i]);
            else
            {
                characterSprite[i].sprite = null;
            }
        }
        for (int i = 0; i < nowWrapper.startEffectList.Count; i++)
        {
            DialogEffect effect = nowWrapper.startEffectList[i];
            GameObject obj = characterSprite[(int)effect.characterPosition].gameObject;
            if (effect.effect == DialogFX.Up)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, downYpos, 0);
                StartCoroutine(sceneManager.MoveModule_Linear(obj, new Vector3(obj.transform.position.x, upYpos, 0), 1));
            }
            else if (effect.effect == DialogFX.Down)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, upYpos, 0);
                StartCoroutine(sceneManager.MoveModule_Linear(obj, new Vector3(obj.transform.position.x, downYpos, 0), 1));

            }
        }
        if (nowWrapper.isCutscene)
        {
            cutSceneBGSprite.sprite = characterIndexToName.GetBackGroundSprite(nowWrapper.cutSceneFileName, true);
            if (nowWrapper.cutSceneEffect == CutSceneEffect.Blur && blurred == false)
            {
                blurManager.OnBlur(true);
                blurred = true;
            }
            else if (nowWrapper.cutSceneEffect == CutSceneEffect.None && blurred == true)
            {
                blurManager.OnBlur(false);
                blurred = false;
            }
        }
        else
        {
            cutSceneBGSprite.sprite = characterIndexToName.GetBackGroundSprite(nowWrapper.backGroundFileName, false);
            if (nowWrapper.backGroundEffect == CutSceneEffect.Blur && blurred == false)
            {
                blurManager.OnBlur(true);
                blurred = true;
            }
            else if (nowWrapper.backGroundEffect == CutSceneEffect.None && blurred == true)
            {
                blurManager.OnBlur(false);
                blurred = false;
            }
        }

        PrintConversation();



    }

    //한 줄 띄우는거.
    void PrintConversation()
    {
        //이제 끝나면 RouteCheck가 뜸. 그 wrapper에 있는거 다 쓰면은.
        if (nowConversationIndex >= nowWrapper.conversationDialogList.Count)
        {
            NextWrapper();
            return;
        }
        ConversationDialog nowConversation = nowWrapper.conversationDialogList[nowConversationIndex];
        conversationText.text = nowConversation.dialog;
        StartCoroutine(sceneManager.LoadTextOneByOne(nowConversation.dialog, conversationText));
        nameText.text = nowConversation.ingameName;
        for (int i = 0; i < 4; i++)
        {
            if (faded[i] == nowConversation.fade[i])
            {
                continue;
            }
            else if (!faded[i] && nowConversation.fade[i])
            {
                faded[i] = true;
                StartCoroutine(sceneManager.FadeModule_Sprite(characterSprite[i].gameObject, 0.2f, 1, 0.5f));

            }
            else if (faded[i] && !nowConversation.fade[i])
            {
                faded[i] = false;
                StartCoroutine(sceneManager.FadeModule_Sprite(characterSprite[i].gameObject, 1f, 0.2f, 0.5f));


            }
        }
        nowConversationIndex++;

    }

    //라우터 버튼 눌릴 때
    public void OnRouterButton(int index)
    {
        checkingRouter = false;
        for (int i = 0; i < routingButtonArray.Length; i++)
        {
            routingButtonArray[i].SetActive(false);
        }
        for (int i = 0; i < nowRouter.routingWrapperIndex.Count; i++)
        {
            Debug.Log(nowRouter.routingWrapperIndex[i]);
        }
        if (nowRouter.routingWrapperIndex.Count - 1 == index)
        {
            leftRouterWrapper = nowRouter.routingWrapperIndex.Count - nowRouter.routingWrapperIndex[index];
        }
        else
        {
            leftRouterWrapper = nowRouter.routingWrapperIndex[index + 1] - nowRouter.routingWrapperIndex[index];
        }
        RoutePair routePair = null;
        for (int i = 0; i < saveData.routePairList.Count; i++)
        {
            if (saveData.routePairList[i].storyName.Contains(nowBundle.bundleName))
            {

                if (routePair.pickedRouteList.Count >= nowBundle.conversationRouterList.Count)
                {
                    saveData.routePairList.RemoveAt(i);
                }
                else
                {
                    routePair = saveData.routePairList[i];
                }
                break;
            }
        }
        if (routePair == null)
        {
            routePair = new RoutePair();
            routePair.storyName = nowBundle.bundleName;
        }
        routePair.pickedRouteList.Add(index);

        nowRouterWrapperIndex = nowRouter.routingWrapperIndex[index];
        nowWrapper = nowRouter.routingWrapperList[nowRouterWrapperIndex];
        nowConversationIndex = 0;
        PrintConversation();

    }

    public void EndButton()
    {
        exploreManager.NextTime();
    }

    public void OnTouch()
    {
        if (!checkingRouter && !sceneManager.nowTexting)
        {
            PrintConversation();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            nowConversationIndex = nowWrapper.conversationDialogList.Count;
        }

    }


}
