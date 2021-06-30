using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomVisitorEndDialog
{
    public bool wrongMedicine;
    public StringWrapper[] visitorDialog;
    public StringWrapper[] ruelliaDialog;

    public RandomVisitorEndDialog(int index)
    {
        if(index == 0)
        {
            wrongMedicine = true;
            visitorDialog = new StringWrapper[4];
            ruelliaDialog = new StringWrapper[4];
            visitorDialog[0] = new StringWrapper("이거 금속 들어있는거 아니죠?");
            ruelliaDialog[0] = new StringWrapper("그럼요 안심하세요");

            visitorDialog[1] = new StringWrapper("그럼 한번 마셔보겠습니다...");
            ruelliaDialog[1] = new StringWrapper("저어 드십쇼...");

            visitorDialog[2] = new StringWrapper("윽 뭔가 이상한데...!");
            ruelliaDialog[2] = new StringWrapper("헉 무슨일이죠");

            visitorDialog[3] = new StringWrapper("낫는거 같은 기분이 안들어요......다음에 또 올게요");
            ruelliaDialog[3] = new StringWrapper("앗..죄송합니다..ㅠㅠ");
        }
        else if(index == 1)
        {
            wrongMedicine = false;
            visitorDialog = new StringWrapper[3];
            ruelliaDialog = new StringWrapper[3];

            visitorDialog[0] = new StringWrapper("감사합니다. 지금 바로 먹어도 되죠?");
            ruelliaDialog[0] = new StringWrapper("바로 드셔도 됩니다. 저어 드십쇼...");

            visitorDialog[1] = new StringWrapper("음...몸에 활력이 돋는것 같아요");
            ruelliaDialog[1] = new StringWrapper("앗 다행이네요!");

            visitorDialog[2] = new StringWrapper("감사합니다. 다음에 또 올게요");
            ruelliaDialog[2] = new StringWrapper("또오세요~~");
        }
        else if (index == 2)
        {
            wrongMedicine = true;
            visitorDialog = new StringWrapper[4];
            ruelliaDialog = new StringWrapper[4];
            visitorDialog[0] = new StringWrapper("이 약을 먹고 제가 괜찮아지면 번호를 물어봐도 될까요");
            ruelliaDialog[0] = new StringWrapper("그럼요! 얼른 마셔보세요!");

            visitorDialog[1] = new StringWrapper("그럼 한번 마셔보겠습니다...");
            ruelliaDialog[1] = new StringWrapper("저어 드십쇼...");

            visitorDialog[2] = new StringWrapper("윽 뭔가 이상한데...!");
            ruelliaDialog[2] = new StringWrapper("헉..(앗 다행이다)");

            visitorDialog[3] = new StringWrapper("번호는 다음번에 물어보겠습니다");
            ruelliaDialog[3] = new StringWrapper("앗....죄송합니다....");
        }
        else
        {
            wrongMedicine = false;
            visitorDialog = new StringWrapper[3];
            ruelliaDialog = new StringWrapper[3];

            visitorDialog[0] = new StringWrapper("약에서 이상한 냄새가 나는거 같아요..");
            ruelliaDialog[0] = new StringWrapper("원래 몸에 좋은 약은 쓴 법입니다.");

            visitorDialog[1] = new StringWrapper("(벌컥벌컥)윽...너무 써요!");
            ruelliaDialog[1] = new StringWrapper("몸은 좀 어떠세요");

            visitorDialog[2] = new StringWrapper("헉 바로 나은것 같아요. 감사합니다. 다음에 또 올게요");
            ruelliaDialog[2] = new StringWrapper("또오세요~~");
        }
    }
}
