using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//이거 모든 타일의 상속클래스.
//상당히 실험적임. json으로 받아올 수 없으면 타일마다 다 따로 변수리스트 만들어야함. 
//json으로는 index만 받아온다.
[System.Serializable]
public class Tile
{
    public int index;
    public TileType tileType;
    public bool opened;

    public Tile(int index)
    {
        this.index = index;
        opened = false;
    }

    public virtual void Open()
    {
        opened = true;
    }

}
