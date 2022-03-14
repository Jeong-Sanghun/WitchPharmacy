using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTreeterManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField]
    TutorialTabletManager tabletManager;
    [SerializeField]
    GameObject wholeTreeterCanvas;
    [SerializeField]
    GameObject oneTitleButtonPrefab;
    [SerializeField]
    Image prefabProfileImage;
    [SerializeField]
    Text prefabProfileNameText;
    [SerializeField]
    Text prefabTitleText;
    [SerializeField]
    RectTransform contentRect;

    [SerializeField]
    GameObject openedCanvasPrefab;
    [SerializeField]
    Transform openedCanvasParent;

    [SerializeField]
    GameObject oneTreeterCanvasPrefab;
    [SerializeField]
    Transform oneTreeterCanvasParent;

    [SerializeField]
    GameObject oneCommentPrefab;

    [SerializeField]
    GameObject onePostPrefab;

    public TreeterData tutorialTreeterData;

    public static TreeterProfileWrapper profileWrapper;
    List<TreeterButtonClass> wholeTreeterButton;
    int nowButtonIndex;
    bool isInit = false;

    void Start()
    {
        gameManager = GameManager.singleton;
        if (profileWrapper == null)
        {
            profileWrapper = gameManager.jsonManager.ResourceDataLoad<TreeterProfileWrapper>("TreeterProfileWrapper");

        }

        wholeTreeterButton = new List<TreeterButtonClass>();
        tutorialTreeterData = gameManager.jsonManager.ResourceDataLoad<TreeterData>("TreeterData/TutorialTreeterData");
        MakeOneButton();


    }

    //불러오기할때 초기화해야돼서;
    public void InitializeButtons()
    {

        //MakeOneButton(conditionList[i]);

    }

    void MakeOneButton()
    {
        TreeterButtonClass buttonClass = new TreeterButtonClass();
        wholeTreeterButton.Add(buttonClass);
        buttonClass.SetTutorialTreeterButton(tutorialTreeterData, oneTreeterCanvasPrefab, oneTreeterCanvasParent, openedCanvasPrefab, openedCanvasParent, onePostPrefab, oneCommentPrefab);
        prefabProfileImage.sprite = profileWrapper.LoadSprite(tutorialTreeterData.profileFileName);
        //data.LoadSprite(true);
        prefabProfileNameText.text = profileWrapper.LoadIngameName(tutorialTreeterData.profileFileName);
        //data.profileIngameName;
        prefabTitleText.text = tutorialTreeterData.titleIngameText;
        GameObject buttonObj = Instantiate(oneTitleButtonPrefab, contentRect);
        buttonObj.SetActive(true);
        buttonClass.oneButtonObj = buttonObj;
        Button button = buttonObj.transform.GetChild(1).GetComponent<Button>();
        int dele = wholeTreeterButton.Count - 1;
        button.onClick.AddListener(() => TreeterButtonActive(dele));
        button.onClick.AddListener(() => tabletManager.GlowNextDialog(ActionKeyword.TreeterPostButtonGlow.ToString()));

        
    }

    void TreeterButtonActive(int index)
    {
        if (tabletManager.isGlowing[(int)ActionKeyword.TreeterPostButtonGlow] == false)
        {
            return;
        }
        nowButtonIndex = index;
        wholeTreeterButton[index].wholeCanvasObject.SetActive(true);
    }

    public void OneTreeterGetOutButton()
    {
        if (nowButtonIndex == -1)
        {
            return;
        }
        wholeTreeterButton[nowButtonIndex].wholeCanvasObject.SetActive(false);
        nowButtonIndex = -1;
    }

}


