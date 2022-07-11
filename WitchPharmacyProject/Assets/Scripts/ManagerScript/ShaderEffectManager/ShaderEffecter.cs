using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllIn1SpriteShader;

public enum ShaderEffect
{
    HOLOGRAM_ON
}

public class ShaderEffecter
{
    public static void SetShaderEffectActive(GameObject obj, string effect, bool active)
    {
        AllIn1Shader nowShader = obj.GetComponent<AllIn1Shader>();
        nowShader.SetKeyword(effect, active);
    }


}
