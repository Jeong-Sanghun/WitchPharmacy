using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CutSceneEffect
{
    None,Blur
}
namespace Obsolete
{

    [System.Serializable]
    public class ConversationDialogWrapper
    {
        public int wrapperIndex;

        public string[] characterName;

        public string[] ingameName;

        public string[] characterFeeling;
        public string nextStory;
        public string effect;
        public string sfx;
        public List<string> concealedCharacter;
        public string bgm;
        public List<DialogEffect> startEffectList;
        public List<ConversationDialog> conversationDialogList;
        public bool nextWrapperIsRouter = false;
        public bool isCutscene = false;
        public string cutSceneFileName;
        public CutSceneEffect cutSceneEffect;
        public CutSceneEffect backGroundEffect;
        public string backGroundFileName;
        public string nextRegion;
        public string popUp;


        public ConversationDialogWrapper()
        {
            conversationDialogList = new List<ConversationDialog>();
            startEffectList = new List<DialogEffect>();
            concealedCharacter = new List<string>();
            characterName = new string[4];
            ingameName = new string[4];
            characterFeeling = new string[4];
            cutSceneEffect = CutSceneEffect.None;
            backGroundEffect = CutSceneEffect.None;
            bgm = null;
            nextStory = null;
            popUp = null;

            for (int i = 0; i < 4; i++)
            {
                characterFeeling[i] = "nothing";
            }
            nextRegion = null;
        }

        public void Copy(ConversationDialogWrapper wrapper)
        {
            for (int j = 0; j < 3; j++)
            {
                characterFeeling[j] = wrapper.characterFeeling[j];
                characterName[j] = wrapper.characterName[j];
                ingameName[j] = wrapper.ingameName[j];
            }
            bgm = wrapper.bgm;
            for (int i = 0; i < wrapper.concealedCharacter.Count; i++)
            {
                concealedCharacter.Add(wrapper.concealedCharacter[i]);
            }
        }

        public void CopyExceptCharacter(ConversationDialogWrapper wrapper)
        {
            bgm = wrapper.bgm;
            for (int i = 0; i < wrapper.concealedCharacter.Count; i++)
            {
                concealedCharacter.Add(wrapper.concealedCharacter[i]);
            }
        }
    }

}