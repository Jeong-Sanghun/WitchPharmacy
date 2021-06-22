using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineTile : Tile
{
    //등장하는 약들
    public int[] appearingMedicineIndex;
    //8개중에 뭘 눌렀냐.
    public bool[] clickedArray;

    public MedicineTile(int i) : base(i)
    {
        index = i;
        tileType = TileType.MedicineTile;
    }

    public override void Open()
    {
        base.Open();
    }

    public void MedicineDataSetting(int[] medicineIndex, float[] probability)
    {
        int nowIndex = 0;
        appearingMedicineIndex = new int[MedicineTileManager.appearingMedicineTypes];
        clickedArray = new bool[MedicineTileManager.appearingMedicine];
        for(int i = 0; i < MedicineTileManager.appearingMedicine; i++)
        {
            clickedArray[i] = false;
        }
        while (nowIndex < MedicineTileManager.appearingMedicineTypes)
        {
            //확률계산해주는거. 총 약 종류로 해야겠지?
            for(int i = 0; i < MedicineTileManager.appearingMedicine; i++)
            {
                if(Random.Range(0,100) < probability[i])
                {
                    appearingMedicineIndex[nowIndex] = medicineIndex[i];
                    nowIndex++;
                    if (nowIndex > MedicineTileManager.appearingMedicineTypes-1)
                    {
                        break;
                    }

                }
            }
            
        }
    }

}
