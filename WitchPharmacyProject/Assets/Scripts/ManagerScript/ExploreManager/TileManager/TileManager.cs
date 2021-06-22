using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    protected SaveDataClass saveData;
    protected GameManager gameManager;
    protected ExploreManager exploreManager;
    protected TileButtonClass nowTileButton;
    protected RegionProperty regionProperty;
    // Start is called before the first frame update
    void Start()
    {
        exploreManager = ExploreManager.inst;
        gameManager = GameManager.singleTon;
        saveData = gameManager.saveData;
    }

    virtual public void TileOpen(TileButtonClass tile)
    {
        nowTileButton = tile;
    }

    virtual public void Initialize(RegionProperty property)
    {
        regionProperty = property;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
