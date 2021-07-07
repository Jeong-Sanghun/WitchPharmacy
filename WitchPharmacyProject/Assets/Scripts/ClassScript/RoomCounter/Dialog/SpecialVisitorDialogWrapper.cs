using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialVisitorDialogWrapper
{
    public string wrapperName;
    public List<SpecialVisitorDialog> specialVisitorDialogList;

    public SpecialVisitorDialogWrapper(int index)
    {
        specialVisitorDialogList = new List<SpecialVisitorDialog>();
        wrapperName = index.ToString();
        SpecialVisitorDialog dialog;
        for(int i = 0; i < 4; i++)
        {
            dialog = new SpecialVisitorDialog();
            specialVisitorDialogList.Add(dialog);
        }
    }
}
