using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BookResearchDataWrapper
{

    public List<BookResearchData> bookResearchDataList;

    public BookResearchDataWrapper()
    {
        bookResearchDataList = new List<BookResearchData>();
        bookResearchDataList.Add(new BookResearchData());
        bookResearchDataList.Add(new BookResearchData());
        bookResearchDataList.Add(new BookResearchData());
        bookResearchDataList.Add(new BookResearchData());
        bookResearchDataList.Add(new BookResearchData());
    }
}
