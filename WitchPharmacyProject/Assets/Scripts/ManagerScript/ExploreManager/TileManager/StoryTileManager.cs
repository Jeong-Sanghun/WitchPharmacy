using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTileManager : StoryManager
{
    bool isExploring;
    //여기서 세팅만 해주면 됨. 어느 번들을 쓸지.
    protected override void Start()
    {
        base.Start();
        isExploring = true;
    }

    public override void TileOpen(TileButtonClass tile)
    {
        base.TileOpen(tile);
        isExploring = false;

    }

    public void OnBackButton()
    {
        isExploring = true;
        ResetOnTile();
    }
    protected virtual void ResetOnTile()
    {
        nowBundle = gameManager.LoadBundle("testBundle");
        nowWrapper = nowBundle.dialogWrapperList[0];
        //routingTime = nowBundle.conversationRouter.routingTime;
        checkingRouter = false;
        nowConversationIndex = 0;
        nowWrapperIndex = 0;
        //nowBundleIndex = 0;
        PrintConversation();
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!checkingRouter && !isExploring)
            {
                PrintConversation();
            }
        }
    }
}
