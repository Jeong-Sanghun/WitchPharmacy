using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoreToolDataWrapper
{
    public List<StoreToolClass> storeToolDataList;

    public StoreToolDataWrapper()
    {
        storeToolDataList = new List<StoreToolClass>();

        StoreToolClass tool = new StoreToolClass();
        tool.fileName = "pickaxe";
        tool.name = "곡괭이";
        tool.toolTip = "탐험 중 바위 장애물을 파괴할 수 있다. 1회성";
        tool.cost = 10;
        tool.usedForExplore = true;
        tool.usedOnce = true;

        storeToolDataList.Add(tool);

        tool = new StoreToolClass();
        tool.fileName = "shovel";
        tool.name = "삽";
        tool.toolTip = "탐험 중 흙더미 장애물을 파괴할 수 있다. 1회성";
        tool.cost = 10;
        tool.usedForExplore = true;
        tool.usedOnce = true;

        storeToolDataList.Add(tool);


        tool = new StoreToolClass();
        tool.fileName = "lantern";
        tool.name = "손전등";
        tool.toolTip = "바 밝다... 동굴지역을 탐험할 수 있습니다";
        tool.cost = 1000;
        tool.usedForExplore = true;
        tool.usedOnce = false;
        storeToolDataList.Add(tool);


        tool = new StoreToolClass();
        tool.fileName = "friendship";
        tool.name = "우정";
        tool.toolTip = "이게 찐 우정이지 ㅋㅋ";
        tool.cost = 100000;
        tool.usedForExplore = false;
        tool.usedOnce = true;



        storeToolDataList.Add(tool);



    }
}
