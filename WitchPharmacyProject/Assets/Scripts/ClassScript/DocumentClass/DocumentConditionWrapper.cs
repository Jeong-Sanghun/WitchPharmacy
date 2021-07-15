using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DocumentConditionWrapper
{
    public List<DocumentCondition> documentConditionList;

    public DocumentConditionWrapper()
    {
        documentConditionList = new List<DocumentCondition>();

        documentConditionList.Add(new DocumentCondition());
        documentConditionList.Add(new DocumentCondition());
        documentConditionList.Add(new DocumentCondition());

    }
}
