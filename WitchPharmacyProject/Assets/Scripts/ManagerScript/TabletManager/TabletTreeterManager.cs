using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class TabletTreeterManager : MonoBehaviour
{
    GameManager gameManager;
    SaveDataClass saveData;
    TreeterConditionWrapper wrapper;
    // Start is called before the first frame update
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

    List<TreeterButtonClass> wholeTreeterButton;
    int nowButtonIndex;
    
    void Start()
    {
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        wrapper = gameManager.treeterConditionWrapper;


        wholeTreeterButton = new List<TreeterButtonClass>();
        MakeButtons();

    }

    void MakeButtons()
    {
        List<TreeterCondition> conditionList = wrapper.treeterConditionList;
        for (int i = 0; i < conditionList.Count; i++)
        {
            if (saveData.nowDay < conditionList[i].dayCondition)
            {
                continue;
                
            }
            MakeOneButton(conditionList[i]);

        }
    }

    void MakeOneButton(TreeterCondition condition)
    {
        TreeterButtonClass buttonClass = new TreeterButtonClass();
        wholeTreeterButton.Add(buttonClass);
        
        TreeterData data = gameManager.jsonManager.ResourceDataLoad<TreeterData>("TreeterData/" + condition.fileName);
        buttonClass.SetTreeterButton(condition, data, oneTreeterCanvasPrefab, oneTreeterCanvasParent, openedCanvasPrefab, openedCanvasParent, onePostPrefab, oneCommentPrefab);
        prefabProfileImage.sprite = data.LoadSprite(true);
        prefabProfileNameText.text = data.profileIngameName;
        prefabTitleText.text = data.titleIngameText;
        GameObject buttonObj = Instantiate(oneTitleButtonPrefab, contentRect);
        buttonObj.SetActive(true);
        Button button = buttonObj.transform.GetChild(1).GetComponent<Button>();
        int dele = wholeTreeterButton.Count - 1;
        button.onClick.AddListener(() => TreeterButtonActive(dele));
    }

    public void OnNextDay()
    {
        List<TreeterCondition> conditionList = wrapper.treeterConditionList;
        saveData = gameManager.saveData;
        for (int i = 0; i < conditionList.Count; i++)
        {
            if (1 < 1)
            {

            }
            if (saveData.nowDay < conditionList[i].dayCondition)
            {
                Debug.Log(saveData.nowDay + " 세이브데이터 데이랑  컨디션 데이 " + conditionList[i].dayCondition);
                Debug.Log(conditionList[i].fileName + " 데이 안충족");
                continue;

            }
            bool contain = false;
            for(int j = 0; j < wholeTreeterButton.Count; j++)
            {
                if(wholeTreeterButton[j].condition == conditionList[i])
                {
                    Debug.Log(conditionList[i].fileName + " 컨테인");
                    contain = true;
                    break;
                }
            }
            if (contain)
            {
                continue;
            }
            
            MakeOneButton(conditionList[i]);

        }
    }

    void TreeterButtonActive(int index)
    {
        nowButtonIndex = index;
        wholeTreeterButton[index].wholeCanvasObject.SetActive(true);
    }

    public void OneTreeterGetOutButton()
    {
        if(nowButtonIndex == -1)
        {
            return;
        }
        wholeTreeterButton[nowButtonIndex].wholeCanvasObject.SetActive(false);
        nowButtonIndex = -1;
    }

    public void WholeTreeterActive(bool active)
    {
        wholeTreeterCanvas.SetActive(active);
    }

    public void WholeButtonOff()
    {
        OneTreeterGetOutButton();
        WholeTreeterActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
