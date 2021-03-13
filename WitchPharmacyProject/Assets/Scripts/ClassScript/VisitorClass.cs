using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public enum Symptom
{
    water, fire, dirt, wood, metal, light
}

//이거 제이슨으로 저장하는게 아님
public class VisitorClass
{
    public string name;
    public string fullDialog;
    //물 불 흙 나무 금속 빛

    public Symptom[] symptomArray;
    public int[] symptomAmountArray;
    //증상은 무조건 두 개

    public int earIndex;
    public int hornIndex;


    public VisitorClass(SymptomDialog dialog)
    {
        symptomArray = new Symptom[2];
        symptomAmountArray = new int[2];
        for(int i = 0; i < symptomAmountArray.Length; i++)
        {
            symptomAmountArray[i] = Random.Range(0, 4);
            //이거 사실 -2~2인데 index로 써야돼서 0~3으로 만듦
        }

        earIndex = Random.Range(0, 5);
        hornIndex = Random.Range(0, 5);
        //동일한거 없애도 된대 ㅎㅎㅎ
        //제외하는 속성이 같아도 됨

        int lastSymptom = -1;
        for(int i = 0; i < symptomArray.Length; i++)
        {
            int random = Random.Range(0, 6);
            while(random == earIndex || random == hornIndex || random == lastSymptom)
            {
                random = Random.Range(0, 6);
            }
            symptomArray[i] = (Symptom)random;
            lastSymptom = random;
        }

        string symptomDialog = "저는 " + symptomArray[0].ToString() + ", 하고 "
            + symptomArray[1].ToString() + " 에 문제가 있는거같아요. \n";
        string firstDialog = dialog.symptomDialogArray[(int)symptomArray[0]]
            .dialogArray[symptomAmountArray[0]];
        string middleDialog = dialog.middleDialog[Random.Range(0, 6)];
        string secondDialog = dialog.symptomDialogArray[(int)symptomArray[1]]
            .dialogArray[symptomAmountArray[1]];

        StringBuilder build = new StringBuilder(symptomDialog);
        build.Append(firstDialog);
        build.Append(middleDialog);
        build.Append(secondDialog);

        fullDialog = build.ToString();

    }


}
