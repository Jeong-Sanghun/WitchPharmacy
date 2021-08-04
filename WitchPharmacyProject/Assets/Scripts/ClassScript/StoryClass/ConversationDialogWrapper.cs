using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CutSceneEffect
{
    None,Blur
}
[System.Serializable]
public class ConversationDialogWrapper
{
    public int wrapperIndex;

    public string[] characterName;

    public string[] ingameName;

    public string[] characterFeeling;

    public List<DialogEffect> startEffectList;
    public List<ConversationDialog> conversationDialogList;
    public bool nextWrapperIsRouter = false;
    public bool isCutscene = false;
    public string cutSceneFileName;
    public CutSceneEffect cutSceneEffect;
    public CutSceneEffect backGroundEffect;
    public string backGroundFileName;


    public ConversationDialogWrapper()
    {
        conversationDialogList = new List<ConversationDialog>();
        startEffectList = new List<DialogEffect>();
        characterName = new string[4];
        ingameName = new string[4];
        characterFeeling = new string[4];
        cutSceneEffect = CutSceneEffect.None;
        backGroundEffect = CutSceneEffect.None;
        for(int i = 0; i < 4; i++)
        {
            characterFeeling[i] = "nothing";
        }
    //    if (index == 0)
    //    {
    //        ConversationDialog dialog = new ConversationDialog();

    //        dialog.leftFade = false;
    //        dialog.rightFade = true;
    //        dialog.dialog = "오늘 날씨가 참 맑을 것 같지? 그래서인지 내 기분도 좋아!";
    //        conversationDialogList.Add(dialog);

    //        dialog = new ConversationDialog();

    //        dialog.leftFade = true;
    //        dialog.rightFade = false;
    //        dialog.dialog = "무슨소리야 밖에 비가 잔뜩오잖아";
    //        conversationDialogList.Add(dialog);

    //        dialog = new ConversationDialog();
    //        dialog.leftCharacterName = "Ruellia";
    //        dialog.rightCharacterName = "Cari";
    //        dialog.leftFade = false;
    //        dialog.rightFade = true;
    //        dialog.dialog = "엇........진짜네 미안해!!";
    //        conversationDialogList.Add(dialog);

    //        dialog = new ConversationDialog();
    //        dialog.leftCharacterName = "Ruellia";
    //        dialog.rightCharacterName = "Cari";
    //        dialog.leftFade = true;
    //        dialog.rightFade = false;
    //        dialog.dialog = "다음부턴 창문을 잘 보고다니라구";
    //        conversationDialogList.Add(dialog);
    //    }
    //    else if (index == 1)
    //    {
    //        ConversationDialog dialog = new ConversationDialog();
    //        dialog.leftCharacterName = "Ruellia";
    //        dialog.rightCharacterName = "Cari";
    //        dialog.leftFade = false;
    //        dialog.rightFade = true;
    //        dialog.dialog = "뭐야 말이 너무 심한거 아니야? 사과해!";
    //        conversationDialogList.Add(dialog);

    //        dialog = new ConversationDialog();
    //        dialog.leftCharacterName = "Ruellia";
    //        dialog.rightCharacterName = "Cari";
    //        dialog.leftFade = true;
    //        dialog.rightFade = false;
    //        dialog.dialog = "??? 니가 먼저 이상한 소리를 한거잖아";
    //        conversationDialogList.Add(dialog);

    //        dialog = new ConversationDialog();
    //        dialog.leftCharacterName = "Ruellia";
    //        dialog.rightCharacterName = "Cari";
    //        dialog.leftFade = false;
    //        dialog.rightFade = true;
    //        dialog.dialog = "지금 날 만만하게 보는거야????";
    //        conversationDialogList.Add(dialog);

    //        dialog = new ConversationDialog();
    //        dialog.leftCharacterName = "Ruellia";
    //        dialog.rightCharacterName = "Cari";
    //        dialog.leftFade = true;
    //        dialog.rightFade = false;
    //        dialog.dialog = "아 아니 그게 아니라...";
    //        conversationDialogList.Add(dialog);
    //    }
    //    else
    //    {
    //        ConversationDialog dialog = new ConversationDialog();
    //        dialog.leftCharacterName = "Ruellia";
    //        dialog.rightCharacterName = "Cari";
    //        dialog.leftFade = false;
    //        dialog.rightFade = true;
    //        dialog.dialog = "....카리야...우리집에는 창문 없는거 알잖아";
    //        conversationDialogList.Add(dialog);

    //        dialog = new ConversationDialog();
    //        dialog.leftCharacterName = "Ruellia";
    //        dialog.rightCharacterName = "Cari";
    //        dialog.leftFade = true;
    //        dialog.rightFade =false;
    //        dialog.dialog = "뭐야 루엘리아 너 이상해. 우리집에 창문 있어! 저기... 어 어디갔지......";
    //        conversationDialogList.Add(dialog);

    //        dialog = new ConversationDialog();
    //        dialog.leftCharacterName = "Ruellia";
    //        dialog.rightCharacterName = "Cari";
    //        dialog.leftFade = false;
    //        dialog.rightFade = true;
    //        dialog.dialog = "흑흑....이러지마 제발 나 너무 슬퍼";
    //        conversationDialogList.Add(dialog);

    //        dialog = new ConversationDialog();
    //        dialog.leftCharacterName = "Ruellia";
    //        dialog.rightCharacterName = "Cari";
    //        dialog.leftFade = true;
    //        dialog.rightFade = false;
    //        dialog.dialog = ".....미안....병세가 좀 심해진거같아.....";
    //        conversationDialogList.Add(dialog);
    //    }
        
    }
}
