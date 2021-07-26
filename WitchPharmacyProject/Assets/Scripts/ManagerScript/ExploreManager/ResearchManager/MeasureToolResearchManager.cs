using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class MeasureToolResearchManager : MonoBehaviour
{


    GameManager gameManager;
    SaveDataClass saveData;
    ResearchSaveData researchSaveData;
    UILanguagePack languagePack;
    ExploreManager exploreManager;
    
    MeasureToolResearchDataWrapper dataWrapper;
    [SerializeField]
    Transform buttonContent;
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    Text prefabButtonText;

    [SerializeField]
    Transform canvasParent;
    [SerializeField]
    GameObject canvasPrefab;
    [SerializeField]
    Image canvasImage;
    [SerializeField]
    Text canvasTitleText;
    [SerializeField]
    Text explainTitleText;
    [SerializeField]
    Text explainText;
    [SerializeField]
    Text neededResearchTitleText;
    [SerializeField]
    Text neededResearchText;

    int openedButtonIndex;
    List<GameObject> wholeCanvasList;
    List<ResearchButtonClass> wholeButtonList;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        languagePack = gameManager.languagePack;
        exploreManager = ExploreManager.inst;
        dataWrapper = gameManager.jsonManager.ResourceDataLoad<MeasureToolResearchDataWrapper>("MeasureToolResearchDataWrapper");
        saveData = gameManager.saveData;
        researchSaveData = saveData.researchSaveData;
        openedButtonIndex = -1;
        wholeCanvasList = new List<GameObject>();
        wholeButtonList = new List<ResearchButtonClass>();
        MakeButtonCanvas();

    }

    void MakeButtonCanvas()
    {
        List<string> researchEndTool = researchSaveData.endMeasureToolResearchList;
        List<MeasureToolResearchData> dataList = dataWrapper.measureToolResearchDataList;
        int nowButtonIndex = 0;
        for (int i = 0; i < dataList.Count; i++)
        {
            
            if (researchEndTool.Contains(dataList[i].fileName))
            {
                continue;
            }
            bool contain = true;
            for(int j = 0; j < dataList[i].neededResearchList.Count;j++)
            {
                if (!researchEndTool.Contains(dataList[i].neededResearchList[j]))
                {
                    contain = false;
                    break;
                }
            }

            prefabButtonText.text = dataList[i].ingameName;
            GameObject inst = Instantiate(buttonPrefab, buttonContent);
            inst.SetActive(true);
 
            inst.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -90 - nowButtonIndex * 180);

            int dele = nowButtonIndex;
            Button menuButton = inst.GetComponent<Button>();
            menuButton.onClick.AddListener(() => OnButtonClick(dele));
            if (!contain)
            {
                menuButton.interactable = false;
                inst.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                inst.transform.GetChild(0).gameObject.SetActive(false);
            }
            canvasImage.sprite = dataList[i].LoadImage();
            canvasTitleText.text = dataList[i].ingameName;
            explainTitleText.text = languagePack.explain;
            explainText.text = dataList[i].explain;
            neededResearchTitleText.text = languagePack.neededResearch;
            StringBuilder builder = new StringBuilder();
            for (int j = 0; j < dataList[i].neededResearchList.Count; j++)
            {
                builder.Append(dataList[i].neededResearchList[j]);
                if(j != dataList[i].neededResearchList.Count - 1)
                {
                    builder.Append(",");
                }
                
            }
            neededResearchText.text = builder.ToString();
            GameObject canvasInst = Instantiate(canvasPrefab, canvasParent);

            Button researchButton = canvasInst.transform.GetChild(0).GetComponent<Button>();
            researchButton.onClick.AddListener(() => ResearchButonClick(dele));
            wholeCanvasList.Add(canvasInst);
            canvasInst.SetActive(false);

            ResearchButtonClass buttonClass = new ResearchButtonClass();
            buttonClass.buttonComponent = researchButton;
            buttonClass.data = dataList[i];
            buttonClass.researchProgressText = researchButton.GetComponentInChildren<Text>();
            buttonClass.canvas = canvasInst;
            wholeButtonList.Add(buttonClass);
            nowButtonIndex++;

            
        }
        buttonContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 180 * nowButtonIndex);
    }

    string FindIngmaeTitleByFileName(string fileName)
    {
        List<MeasureToolResearchData> dataList = dataWrapper.measureToolResearchDataList;
        for (int i = 0; i < dataList.Count; i++)
        {
            if(dataList[i].fileName == fileName)
            {
                return dataList[i].ingameName;
            }
        }
        return null;
    }

    void ResearchButonClick(int index)
    {
        ResearchData data = wholeButtonList[index].data;
        OneMeasureToolResearch research = null;
        for(int i = 0; i < researchSaveData.progressingMeasureToolReaserchList.Count; i++)
        {
            if(data.fileName == researchSaveData.progressingMeasureToolReaserchList[i].fileName)
            {
                research = researchSaveData.progressingMeasureToolReaserchList[i];
                break;
            }
        }
        if(research == null)
        {
            research = new OneMeasureToolResearch();
            research.fileName = data.fileName;
            research.researchedTime = 1;
            researchSaveData.progressingMeasureToolReaserchList.Add(research);
        }
        else
        {
            research.researchedTime++;
        }

        if (research.researchedTime >= data.researchEndTime)
        {
            researchSaveData.progressingMeasureToolReaserchList.Remove(research);
            researchSaveData.endMeasureToolResearchList.Add(research.fileName);
            wholeButtonList[index].buttonComponent.interactable = false;
        }
        exploreManager.TimeChange(1200);
        wholeButtonList[index].researchProgressText.text = research.researchedTime + "/" + data.researchEndTime;
        
    }

    void OnButtonClick(int index)
    {
        if(openedButtonIndex != -1)
        {
            wholeCanvasList[openedButtonIndex].SetActive(false);
        }
        openedButtonIndex = index;
        wholeCanvasList[openedButtonIndex].SetActive(true);
    }

    public void BackButton()
    {
        SceneManager.inst.LoadScene("ExploreScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
