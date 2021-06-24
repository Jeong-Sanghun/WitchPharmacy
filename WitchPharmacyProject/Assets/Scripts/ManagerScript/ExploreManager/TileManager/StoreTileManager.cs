using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTileManager : StoreToolManager
{
    protected override void Start()
    {
        exploreManager = ExploreManager.inst;
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
        storeToolDataList = gameManager.storeToolDataWrapper.storeToolDataList;
        owningToolList = saveData.owningToolList;
        appearingStoreToolList = new List<StoreToolClass>();
        for (int i = 0; i < storeToolDataList.Count; i++)
        {
            if (storeToolDataList[i].usedForExplore)
            {
                appearingStoreToolList.Add(storeToolDataList[i]);
            }
        }
        StoreStart();
    }

    public void OnBackButton()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
