using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//json
[System.Serializable]
public class RegionProperty
{
    public int index;
    public string regionName;
    public int[] tileTypeArray;
    //public enum TileType
    //{
    //    StartTile, MedicineTile, StoreTile, StoryTile,
    //    TreasureTile, TrapTile, BarrierTile, SpecialStoreTile
    //}
    public int[] regionAvailableMedicine;
    public float[] medicineProbability;
    
    //더미 생성자
    public RegionProperty(int _index)
    {
        //타일개수만큼 타일 타입을 만들어주는 프로퍼티
        //나중에 제이슨 파일로 받아올거임.
        index = _index;
        regionName = "testRegion";
        tileTypeArray = new int[8];
        tileTypeArray[0] = 1;
        for(int i = 1; i < 8; i++)
        {
            tileTypeArray[i] = 0;
        }
        int roundRobin;
        for (int i = 1; i< RegionIngame.tileNumber; i++)
        {
            roundRobin = i % 8;
            if(roundRobin == 0)
            {
                roundRobin++;
            }
            tileTypeArray[roundRobin]++;
        }
        regionAvailableMedicine = new int[MedicineTileManager.appearingMedicine];
        medicineProbability = new float[MedicineTileManager.appearingMedicine];
        for (int i = 0; i < MedicineTileManager.appearingMedicine; i++)
        {
            regionAvailableMedicine[i] = Random.Range(0,20);
            medicineProbability[i] = 100.0f/ MedicineTileManager.appearingMedicine;
        }
        //regionAvailableMedicine = new RegionAvailableMedicine();
    }


}
