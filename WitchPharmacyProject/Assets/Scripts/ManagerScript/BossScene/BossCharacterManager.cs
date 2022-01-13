using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharacterManager : MonoBehaviour
{
    BossData bossData;
    BossCharacter bossCharacter;


    [SerializeField]
    BossCenterManager bossCenterManager;
    [SerializeField]
    GameObject bossObject;
    [SerializeField]
    GameObject symptomObjectPrefab;
    [SerializeField]
    GameObject[] symptomPositionObjectArray;
    CharacterIndexToName characterIndexToName;

    // Start is called before the first frame update
    void Start()
    {
        characterIndexToName = new CharacterIndexToName();
        bossCharacter = new BossCharacter(bossObject);

        for(int i = 0; i < 3; i++)
        {
            GameObject obj = Instantiate(symptomObjectPrefab, symptomPositionObjectArray[i].transform);
            bossCharacter.bossSymptomList.Add(new BossSymptom(obj));
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
