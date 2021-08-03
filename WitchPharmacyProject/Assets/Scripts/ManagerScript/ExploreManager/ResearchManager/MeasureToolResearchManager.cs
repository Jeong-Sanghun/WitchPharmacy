using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class MeasureToolResearchManager : ResearchManagerParent
{


    MeasureToolResearchDataWrapper dataWrapper;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        dataWrapper = gameManager.jsonManager.ResourceDataLoad<MeasureToolResearchDataWrapper>("MeasureToolResearchDataWrapper");
        MakeButtonCanvas();
    }

    //버
    void MakeButtonCanvas()
    {
        //끝난 연구
        List<string> researchEndTool = researchSaveData.endMeasureToolResearchList;
        //제이슨 데이터
        List<MeasureToolResearchData> dataList = dataWrapper.measureToolResearchDataList;
        int nowButtonIndex = 0;
        for (int i = 0; i < dataList.Count; i++)
        {
            //끝났으면 안만들어줌.
            if (researchEndTool.Contains(dataList[i].fileName))
            {
                continue;
            }
            //만약 요구사항을 충족하지 않았으면 락걸어줌.
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
            inst.transform.GetChild(0).gameObject.SetActive(!contain);
            //컨테인 안하면은 락 오브젝트 활성화.
            canvasImage.sprite = dataList[i].LoadImage();
            canvasTitleText.text = dataList[i].ingameName;
            explainTitleText.text = languagePack.explain;
            explainText.text = dataList[i].explain;
            neededResearchTitleText.text = languagePack.neededResearch;
            StringBuilder builder = new StringBuilder();
            for (int j = 0; j < dataList[i].neededResearchList.Count; j++)
            {
                builder.Append(FindIngmaeTitleByFileName(dataList[i].neededResearchList[j]));
                if(j != dataList[i].neededResearchList.Count - 1)
                {
                    builder.Append(",");
                }
                
            }
            neededResearchText.text = builder.ToString();
            GameObject canvasInst = Instantiate(canvasPrefab, canvasParent);

            Button researchButton = canvasInst.transform.GetChild(2).GetComponent<Button>();
            researchButton.onClick.AddListener(() => ResearchButonClick(dele));
            wholeCanvasList.Add(canvasInst);
            canvasInst.SetActive(false);

            ResearchButtonClass buttonClass = new ResearchButtonClass();
            buttonClass.filledImage = canvasInst.transform.GetChild(3).GetChild(0).GetComponent<Image>();
            buttonClass.menuButtonObj = inst;
            buttonClass.menuButtonComponent = menuButton;
            buttonClass.researchButtonComponent = researchButton;
            buttonClass.researchButtonText = researchButton.transform.GetChild(0).GetComponent<Text>();
            buttonClass.data = dataList[i];
            buttonClass.locked = !contain;
            buttonClass.researchProgressText = canvasInst.transform.GetChild(3).GetChild(1).GetComponent<Text>();
            buttonClass.canvas = canvasInst;
            wholeButtonList.Add(buttonClass);

            buttonClass.researchButtonComponent.interactable = contain;
            if (contain)
            {
                buttonClass.researchButtonText.text = languagePack.doResearch;
            }
            else
            {
                buttonClass.researchButtonText.text = languagePack.priorResearchNeeded;
            }
            

            ResearchData data = buttonClass.data;
            OneResearch research = null;
            for (int j = 0; j < researchSaveData.progressingMeasureToolResearchList.Count; j++)
            {
                if (data.fileName == researchSaveData.progressingMeasureToolResearchList[j].fileName)
                {
                    research = researchSaveData.progressingMeasureToolResearchList[j];
                    break;
                }
            }
            if (research == null)
            {
                buttonClass.researchProgressText.text = "0 / " + data.researchEndTime;
                buttonClass.filledImage.fillAmount = 0;
            }
            else
            {
                if (research.researchedTime >= data.researchEndTime)
                {
                    researchSaveData.progressingMeasureToolResearchList.Remove(research);
                    researchSaveData.endMeasureToolResearchList.Add(research.fileName);
                    buttonClass.researchButtonComponent.interactable = false;
                }
                buttonClass.researchProgressText.text = research.researchedTime + "/" + data.researchEndTime;
                buttonClass.filledImage.fillAmount = (float)research.researchedTime / data.researchEndTime;
            }
            nowButtonIndex++;

            
        }
        buttonContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 180 * nowButtonIndex);
    }


    //파일네임을 통해서 인게임네임을 가져옴.
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

    //연구버튼 클릭.
    void ResearchButonClick(int index)
    {
        MeasureToolResearchData data = (MeasureToolResearchData)wholeButtonList[index].data;
        OneResearch research = null;
        for(int i = 0; i < researchSaveData.progressingMeasureToolResearchList.Count; i++)
        {
            if(data.fileName == researchSaveData.progressingMeasureToolResearchList[i].fileName)
            {
                research = researchSaveData.progressingMeasureToolResearchList[i];
                break;
            }
        }
        if(research == null)
        {
            research = new OneResearch();
            research.fileName = data.fileName;
            research.researchedTime = researchCount;
            researchSaveData.progressingMeasureToolResearchList.Add(research);
        }
        else
        {
            research.researchedTime+= researchCount;
        }

        if (research.researchedTime >= data.researchEndTime)
        {
            research.researchedTime = data.researchEndTime;
            researchSaveData.progressingMeasureToolResearchList.Remove(research);
            researchSaveData.endMeasureToolResearchList.Add(research.fileName);
            wholeButtonList[index].researchButtonComponent.interactable = false;

            int containingIndex = -1;
            for(int i = 0; i < wholeButtonList.Count; i++)
            {
                MeasureToolResearchData researchData  = (MeasureToolResearchData)wholeButtonList[i].data; ;
                for(int j =0;j< researchData.neededResearchList.Count; j++)
                {
                    if (researchData.neededResearchList[j].Contains(data.fileName))
                    {
                        containingIndex = i;
                        break;
                    }
                }
            }
            if(containingIndex != -1)
            {
                wholeButtonList[containingIndex].locked = false;
                wholeButtonList[containingIndex].researchButtonComponent.interactable = true;
                wholeButtonList[containingIndex].researchButtonText.text = languagePack.doResearch;
                wholeButtonList[containingIndex].menuButtonObj.transform.GetChild(0).gameObject.SetActive(false);
            }

        }
        //exploreManager.TimeChange(1200);
        exploreManager.NextTime();
        wholeButtonList[index].researchProgressText.text = research.researchedTime + " / " + data.researchEndTime;
        wholeButtonList[index].filledImage.fillAmount = (float)research.researchedTime / data.researchEndTime;
        
    }

    //메뉴버튼 클릭
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
