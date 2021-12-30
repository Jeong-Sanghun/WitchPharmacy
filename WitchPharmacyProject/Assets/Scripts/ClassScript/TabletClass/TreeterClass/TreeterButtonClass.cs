using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TreeterButtonClass
{
    public GameObject wholeCanvasObject;
    public RectTransform wholeContentRect;
    public Text titleText;
    public Text dialogText;
    public RectTransform bgRect;
    public RectTransform mainPostRect;
    public Image treeterImage;
    public Image profileImage;
    public Text profileNameText;
    public Button likeButton;
    public Text likeText;
    public GameObject oneButtonObj;

    public GameObject openedImageCanvas;
    public Image openedImage;
    public Text openedImageText;

    public TreeterCondition condition;
    public TreeterData data;

    public List<TreeterIngameComment> commentList;

    public bool liked;

    public TreeterButtonClass()
    {
        liked = false;
        commentList = new List<TreeterIngameComment>();
    }

    public void SetTreeterButton(TreeterCondition cond, TreeterData dat,GameObject canvasPrefab, Transform prefabParent, GameObject openedImagePrefab, Transform openedImageParent
        ,GameObject mainPostPrefab,GameObject commentPrefab)
    {
        condition = cond;
        data = dat;
        wholeCanvasObject = GameObject.Instantiate(canvasPrefab, prefabParent);
        //wholeCanvasObject.SetActive(true);
        wholeContentRect = wholeCanvasObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<RectTransform>();
        GameObject mainPost = GameObject.Instantiate(mainPostPrefab, wholeContentRect);
        mainPost.SetActive(true);
        profileNameText = mainPost.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        profileImage = mainPost.transform.GetChild(0).GetComponent<Image>();
        titleText = mainPost.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        treeterImage = mainPost.transform.GetChild(1).GetChild(1).GetComponent<Image>();
        dialogText = mainPost.transform.GetChild(1).GetChild(2).GetComponent<Text>();
        likeText = mainPost.transform.GetChild(1).GetChild(3).GetChild(1).GetComponent<Text>();
        likeButton = mainPost.transform.GetChild(1).GetChild(3).GetComponent<Button>();
        likeButton.onClick.AddListener(() => LikeButton());

        profileNameText.text = TabletTreeterManager.profileWrapper.LoadIngameName(data.profileFileName);
            //data.profileIngameName;
        profileImage.sprite = TabletTreeterManager.profileWrapper.LoadSprite(data.profileFileName);
        titleText.text = data.titleIngameText;
        dialogText.text = data.dialog;

        if (GameManager.singleTon.saveData.likedTreeterIndexList.Contains(data.index))
        {
            likeText.text = (data.likeNumber + 1).ToString();
        }
        else
        {
            likeText.text = data.likeNumber.ToString();
        }

        if (cond.printSprite)
        {
            treeterImage.sprite = data.LoadSprite();
            treeterImage.rectTransform.sizeDelta = new Vector2(treeterImage.sprite.rect.width, treeterImage.rectTransform.sizeDelta.y);
            openedImageCanvas = GameObject.Instantiate(openedImagePrefab, openedImageParent);
            openedImage = openedImageCanvas.transform.GetChild(1).GetComponent<Image>();
            openedImageText = openedImageCanvas.transform.GetChild(2).GetComponent<Text>();
            openedImage.sprite = data.LoadSprite();
            openedImageText.text = data.titleIngameText;
            EventTrigger trigger1 = treeterImage.GetComponent<EventTrigger>();

            EventTrigger.Entry entry1 = new EventTrigger.Entry();
            entry1.eventID = EventTriggerType.PointerUp;
            entry1.callback.AddListener((data) => { ActiveImage(true); });
            trigger1.triggers.Add(entry1);

            EventTrigger trigger = openedImageCanvas.transform.GetChild(3).GetComponent<EventTrigger>();
            
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) => {ActiveImage(false); });
            trigger.triggers.Add(entry);
        }
        else
        {
            treeterImage.gameObject.SetActive(false);
        }

        for(int i = 0; i < data.commentDataList.Count; i++)
        {
            TreeterIngameComment comment = new TreeterIngameComment();
            comment.SetButton(data.commentDataList[i], commentPrefab, wholeContentRect);
            commentList.Add(comment);
        }
    }
    

    void LikeButton()
    {
        SaveDataClass saveData = GameManager.singleTon.saveData;
        if (saveData.likedTreeterIndexList.Contains(data.index))
        {
            saveData.likedTreeterIndexList.Remove(data.index);
            likeText.text = data.likeNumber.ToString();
        }
        else
        {
            saveData.likedTreeterIndexList.Add(data.index);
            likeText.text = (data.likeNumber+1).ToString();
        }
        
    }

    void ActiveImage(bool active)
    {
        openedImageCanvas.SetActive(active);
    }





}
