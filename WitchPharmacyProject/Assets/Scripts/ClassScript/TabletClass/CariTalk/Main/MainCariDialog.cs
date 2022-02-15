using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainCariDialog : CariDialog
{
    public string firstButtonText;
    public string secondButtonText;
    public string thirdButtonText;

    //[System.NonSerialized]
    public List<string> buttonTextList;

    public MainCariDialog()
    {
        firstButtonText = null;
        secondButtonText = null;
        thirdButtonText = null;
        buttonTextList = new List<string>();
    }

    public override void Parse()
    {
        base.Parse();
        if(firstButtonText != null)
        {
            buttonTextList.Add(firstButtonText);
        }
        if (secondButtonText != null)
        {
            buttonTextList.Add(secondButtonText);
        }
        if (thirdButtonText != null)
        {
            buttonTextList.Add(thirdButtonText);
        }
    }


}
