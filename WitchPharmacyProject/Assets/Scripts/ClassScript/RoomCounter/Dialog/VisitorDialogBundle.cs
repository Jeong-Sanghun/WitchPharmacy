using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorDialogBundle
{
    public string bundleName;
    public VisitorType visitorType;
    public List<VisitorDialogWrapper> startWrapperList;
    public List<VisitorDialogWrapper> rightWrapperList;
    public List<VisitorDialogWrapper> wrongWrapperList;

    public VisitorDialogBundle()
    {
        visitorType = VisitorType.Random;
        bundleName = null;
        startWrapperList = new List<VisitorDialogWrapper>();
        rightWrapperList = new List<VisitorDialogWrapper>();
        wrongWrapperList = new List<VisitorDialogWrapper>();
    }
}
