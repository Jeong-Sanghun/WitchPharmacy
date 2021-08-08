using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeterIngameComment
{
    public RectTransform commentRect;
    public RectTransform bgRect;
    public GameObject commentObject;

    public TreeterIngameComment()
    {

    }

    public void SetButton(TreeterCommentData data, GameObject prefab,Transform prefabParent)
    {
        commentObject = GameObject.Instantiate(prefab, prefabParent);
        Text profileNameText = commentObject.transform.GetChild(1).GetComponent<Text>();
        Image profileImage = commentObject.transform.GetChild(2).GetComponent<Image>();
        Text dialogText = commentObject.transform.GetChild(3).GetComponent<Text>();

        profileNameText.text = data.profileIngameName;
        profileImage.sprite = data.LoadSprite();
        dialogText.text = data.dialog;

        commentRect = commentObject.GetComponent<RectTransform>();
        bgRect = commentObject.transform.GetChild(0).GetComponent<RectTransform>();

    }
}
