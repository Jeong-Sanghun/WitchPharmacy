using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialStoreTile : Tile
{
    public List<OwningMedicineClass> selledSpecialMedicineList;

    public SpecialStoreTile(int i) : base(i)
    {
        index = i;
        tileType = TileType.SpecialStoreTile;
        selledSpecialMedicineList = new List<OwningMedicineClass>();
    }
}
