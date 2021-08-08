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

    public GameObject openedImageCanvas;
    public Image openedImage;
    public Text openedImageText;

    public TreeterCondition condition;
    public TreeterData data;

    public List<TreeterIngameComment> commentList;

    public bool liked;
    public bool printSprite;

    public TreeterButtonClass()
    {
        liked = false;
        printSprite = false;
    }

    public void SetTreeterButton(TreeterCondition cond, TreeterData dat,GameObject canvasPrefab, Transform prefabParent, GameObject openedImagePrefab, Transform openedImageParent
        ,GameObject mainPostPrefab,GameObject commentPrefab)
    {
        condition = cond;
        data = dat;
        wholeCanvasObject = GameObject.Instantiate(canvasPrefab, prefabParent);
        wholeContentRect = wholeCanvasObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<RectTransform>();
        GameObject mainPost = GameObject.Instantiate(mainPostPrefab, wholeContentRect);
        titleText = mainPost.transform.GetChild(1).GetComponent<Text>();
        treeterImage = mainPost.transform.GetChild(2).GetComponent<Image>();
        profileNameText  = mainPost.transform.GetChild(3).GetComponent<Text>();
        profileImage = mainPost.transform.GetChild(4).GetComponent<Image>();
        dialogText = mainPost.transform.GetChild(5).GetComponent<Text>();

        mainPostRect = mainPost.GetComponent<RectTransform>();
        bgRect = mainPost.transform.GetChild(0).GetComponent<RectTransform>();



        if (cond.printSprite)
        {
            openedImageCanvas = GameObject.Instantiate(openedImagePrefab, openedImageParent);
            openedImage = openedImageCanvas.transform.GetChild(1).GetComponent<Image>();
            openedImageText = openedImageCanvas.transform.GetChild(2).GetComponent<Text>();
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
    }

    void ActiveImage(bool active)
    {
        openedImageCanvas.SetActive(active);
    }





}
