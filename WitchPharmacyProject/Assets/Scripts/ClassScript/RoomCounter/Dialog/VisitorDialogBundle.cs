using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorDialogBundle
{
    public string bundleName;
    public VisitorType visitorType;
    public int[] oddVisitorSetArray;
    public int[] symptomNumberArray;
    public List<string> diseaseNameList;
    public List<VisitorDialogWrapper> startWrapperList;
    public List<VisitorDialogWrapper> rightWrapperList;
    public List<VisitorDialogWrapper> wrongWrapperList;
    public List<VisitorDialogWrapper> skipWrapperList;

    public VisitorDialogBundle()
    {
        visitorType = VisitorType.Random;
        bundleName = null;
        symptomNumberArray = new int[5];
        diseaseNameList = new List<string>();
        oddVisitorSetArray = new int[5];
        startWrapperList = new List<VisitorDialogWrapper>();
        rightWrapperList = new List<VisitorDialogWrapper>();
        wrongWrapperList = new List<VisitorDialogWrapper>();
        skipWrapperList = new List<VisitorDialogWrapper>();
    }
}
