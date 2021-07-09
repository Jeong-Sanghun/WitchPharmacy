using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SpecialVisitorClass
{
    public GameObject visitorObject;
    public SpriteRenderer spriteRenderer;

    public SpecialVisitorClass(GameObject parent,GameObject prefab,string characterName)
    {
        string path = "RandomCharacter/";


        StringBuilder builder = new StringBuilder(path);
        builder.Append(characterName);
        GameObject part = GameObject.Instantiate(prefab, parent.transform);
        visitorObject = part;
        spriteRenderer = part.GetComponent<SpriteRenderer>();
        CharacterIndexToName loader = new CharacterIndexToName();

        spriteRenderer.sprite = loader.GetSprite(characterName, "nothing");
        //part.transform.position = new Vector3(0, -11.91f, -1);
        part.SetActive(true);
        part.transform.localPosition = Vector3.zero;
    }
}
