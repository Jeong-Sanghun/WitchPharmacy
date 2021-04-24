using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//손님이 가지고 있는 질병 클래스
[System.Serializable]
public class DiseaseClass   //SH 
{

    public Symptom firstSymptom;
    public Symptom secondSymptom;

    public int firstSymptomDegree;
    public int secondSymptomDegree;


}
