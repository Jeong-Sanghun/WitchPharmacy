using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//이거 모든 타일의 상속클래스.
//상당히 실험적임. json으로 받아올 수 없으면 타일마다 다 따로 변수리스트 만들어야함. 
[System.Serializable]
public class Tile
{
    public TileType tileType;

}
