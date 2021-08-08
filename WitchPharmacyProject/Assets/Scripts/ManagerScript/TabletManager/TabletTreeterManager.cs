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


    }

    void MakeButtons()
    {
        int buttonIndex = 0;
        List<TreeterCondition> conditionList = wrapper.treeterConditionList;
        for (int i = 0; i < conditionList.Count; i++)
        {
            if (saveData.nowDay < conditionList[i].dayCondition)
            {
                continue;
            }
            TreeterButtonClass buttonClass = new TreeterButtonClass();
            wholeTreeterButton.Add(buttonClass);
            TreeterData data = gameManager.jsonManager.ResourceDataLoad<TreeterData>("TreeterData/" + conditionList[i].fileName);
            buttonClass.SetTreeterButton(conditionList[i], data, oneTreeterCanvasPrefab, oneTreeterCanvasParent, openedCanvasPrefab, openedCanvasParent,onePostPrefab,oneCommentPrefab);
            prefabProfileImage.sprite = data.LoadSprite(true);
            prefabProfileNameText.text = data.profileIngameName;
            prefabTitleText.text = data.titleIngameText;
            GameObject buttonObj = Instantiate(oneTitleButtonPrefab, contentRect);
            Button button = buttonObj.transform.GetChild(0).GetComponent<Button>();
            int dele = buttonIndex;
            button.onClick.AddListener(() => TreeterButtonActive(dele));
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
