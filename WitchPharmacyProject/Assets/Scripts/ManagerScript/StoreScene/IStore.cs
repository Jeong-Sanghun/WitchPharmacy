using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStore
{

    //버튼 클릭했을 때. 인덱스는 wholeButtonList의 index임. 거기안에 medicineClass들어있음.
    //팝업올라옴
    void OnButtonDown(int index);

    void OnSliderValueChange();


    //그 버튼눌러서 1개씩 올라가느넉
    void OnQuantityChangeButton(bool plus);

    //살게요버튼
    void OnBuyButton();

    void OnNotEnoughCoinPopupButton();

    void OnPopupBackButton();

}
