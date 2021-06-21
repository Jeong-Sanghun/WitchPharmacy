using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//region Maker에서 씀.
[System.Serializable]   //더미
public class RegionIngame
{
    public RegionProperty regionProperty;
    public List<TileButtonClass> tileButtonList;
    public static int tileNumber = 10;

    ////dumy생성자. 나중에 제이슨으로 받아올거임.
    //public RegionIngame(GameObject button,Transform canvas)
    //{
    //    //여기서 타일을 tileNumber만큼 만들어줌.
    //    regionProperty = new RegionProperty(0);
    //    tileButtonList = new List<TileButtonClass>();
    //    GameObject inst = GameObject.Instantiate(button,canvas);
    //    Tile tile = new StartTile(0);
    //    tileButtonList.Add(new TileButtonClass(inst,tile,true));

    //    //int nowTypeIndex = 1;
    //    //int nowArrayIndex = 0;
    //    int nowFullArrayIndex = 1;
    //    bool end = false;
    //    int[] typeArray = new int[8];
    //    for(int i = 0; i < typeArray.Length; i++)
    //    {
    //        typeArray[i] = regionProperty.tileTypeArray[i];
    //    }
    //    while (!end)
    //    {
    //        inst = GameObject.Instantiate(button, canvas);
    //        bool isStoryTile = true;
    //        end = true;
    //        int nowTypeIndex = Random.Range(1,8);
    //        for(int i = 1; i < 8; i++)
    //        {
    //            if(typeArray[nowTypeIndex] > 0)
    //            {
    //                if (nowTypeIndex == 3)
    //                {
    //                    end = false;
    //                    nowTypeIndex++;
    //                    continue;
    //                }
    //                isStoryTile = false;
    //                end = false;
    //                break;
    //            }
    //            else
    //            {
    //                nowTypeIndex++;
    //                if (nowTypeIndex >= 8)
    //                {
    //                    nowTypeIndex = 1;
    //                }
    //            }
    //        }
    //        if (isStoryTile)
    //        {
    //            nowTypeIndex = 3;
    //        }
    //        typeArray[nowTypeIndex]--;
    //        Debug.Log(nowTypeIndex);
    //        switch (nowTypeIndex)
    //        {
    //            //StartTile, MedicineTile, StoreTile, StoryTile,
    //            //TreasureTile, TrapTile, BarrierTile, SpecialStoreTile
    //            case 1:
    //                tile = new MedicineTile(nowFullArrayIndex);
    //                break;
    //            case 2:
    //                tile = new StoreTile(nowFullArrayIndex);
    //                break;
    //            case 3:
    //                tile = new StoryTile(nowFullArrayIndex);
    //                break;
    //            case 4:
    //                tile = new TreasureTile(nowFullArrayIndex);
    //                break;
    //            case 5:
    //                tile = new TrapTile(nowFullArrayIndex);
    //                break;
    //            case 6:
    //                tile = new BarrierTile(nowFullArrayIndex);
    //                break;
    //            case 7:
    //                tile = new SpecialStoreTile(nowFullArrayIndex);
    //                break;
    //            default:
    //                tile = new MedicineTile(nowFullArrayIndex);
    //                break;
    //        }
    //        tileButtonList.Add(new TileButtonClass(inst, tile, false));
    //        nowFullArrayIndex++;
    //        //nowArrayIndex++;
    //        //if(regionProperty.tileTypeArray[nowTypeIndex] < nowArrayIndex)
    //        //{
    //        //    nowArrayIndex = 0;
    //        //    nowTypeIndex++;
    //        //}
    //    }

    //}


    //나중엔이거로 받아올거.
    public RegionIngame(RegionProperty json,GameObject button, Transform canvas)
    {
        regionProperty = json;
        tileButtonList = new List<TileButtonClass>();
        GameObject inst = GameObject.Instantiate(button, canvas);
        Tile tile = new StartTile(0);
        tileButtonList.Add(new TileButtonClass(inst, tile, true));

        int nowFullArrayIndex = 1;
        bool end = false;
        //typeArray는 property에서 받아옴
        int[] typeArray = new int[8];
        for (int i = 0; i < typeArray.Length; i++)
        {
            typeArray[i] = regionProperty.tileTypeArray[i];
        }
        //특수상점은 뒤로 몰아야해.
        //이거는 랜덤으로 받아서 쓸 수 있게 하는거.
        while (!end)
        {
            bool isSpecialStoreTile = true;
            end = true;
            int nowTypeIndex = Random.Range(1, 8);
            for (int i = 1; i < 8; i++)
            {
                //7번까지 다 돌아보는 중에 있으면 break로 나간다.
                if (typeArray[nowTypeIndex] > 0)
                {
                    end = false;
                    if (nowTypeIndex == 7)
                    {
                        nowTypeIndex = 1;
                        continue;
                    }
                    isSpecialStoreTile = false;
                    break;
                }
                else
                {
                    nowTypeIndex++;
                    if (nowTypeIndex >= 8)
                    {
                        nowTypeIndex = 1;
                    }
                }
            }
            if (isSpecialStoreTile)
            {
                nowTypeIndex = 7;
            }
            //end면 끝내준다.
            if(end == true)
            {
                break;
            }
            else
            {
                inst = GameObject.Instantiate(button, canvas);
            }
            
            typeArray[nowTypeIndex]--;

            switch (nowTypeIndex)
            {
                //StartTile, MedicineTile, StoreTile, StoryTile,
                //TreasureTile, TrapTile, BarrierTile, SpecialStoreTile
                case 1:
                    tile = new MedicineTile(nowFullArrayIndex);
                    break;
                case 2:
                    tile = new StoreTile(nowFullArrayIndex);
                    break;
                case 3:
                    tile = new StoryTile(nowFullArrayIndex);
                    break;
                case 4:
                    tile = new TreasureTile(nowFullArrayIndex);
                    break;
                case 5:
                    tile = new TrapTile(nowFullArrayIndex);
                    break;
                case 6:
                    tile = new BarrierTile(nowFullArrayIndex);
                    break;
                case 7:
                    tile = new SpecialStoreTile(nowFullArrayIndex);
                    break;
                default:
                    tile = new MedicineTile(nowFullArrayIndex);
                    break;
            }
            tileButtonList.Add(new TileButtonClass(inst, tile, false));
            nowFullArrayIndex++;
            //nowArrayIndex++;
            //if(regionProperty.tileTypeArray[nowTypeIndex] < nowArrayIndex)
            //{
            //    nowArrayIndex = 0;
            //    nowTypeIndex++;
            //}
        }

    }
}
