using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchManagerParent : MonoBehaviour
{

    protected GameManager gameManager;
    protected SaveDataClass saveData;
    protected ResearchSaveData researchSaveData;
    protected UILanguagePack languagePack;
    protected ExploreManager exploreManager;


    [SerializeField]
    protected Transform buttonContent;
    [SerializeField]
    protected GameObject buttonPrefab;
    [SerializeField]
    protected Text prefabButtonText;

    [SerializeField]
    protected Transform canvasParent;
    [SerializeField]
    protected GameObject canvasPrefab;
    [SerializeField]
    protected Image canvasImage;
    [SerializeField]
    protected Text canvasTitleText;
    [SerializeField]
    protected Text explainTitleText;
    [SerializeField]
    protected Text explainText;
    [SerializeField]
    protected Text neededResearchTitleText;
    [SerializeField]
    protected Text neededResearchText;

    protected int openedButtonIndex;
    protected List<GameObject> wholeCanvasList;
    protected List<ResearchButtonClass> wholeButtonList;
    protected int researchCount;

    protected virtual void Start()
    {
        gameManager = GameManager.singleTon;
        languagePack = gameManager.languagePack;
        exploreManager = ExploreManager.inst;

        saveData = gameManager.saveData;
        researchSaveData = saveData.researchSaveData;
        openedButtonIndex = -1;
        wholeCanvasList = new List<GameObject>();
        wholeButtonList = new List<ResearchButtonClass>();
        researchCount = 10;
        for(int i = 0; i < saveData.owningOtherToolList.Count; i++)
        {
            if (saveData.owningOtherToolList[i].Contains("improvement"))
            {
                researchCount = 2;
                break;
            }
        }
        
    }
}
