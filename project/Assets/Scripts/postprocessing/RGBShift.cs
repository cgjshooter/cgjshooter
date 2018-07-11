using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(RGBShiftRenderer), PostProcessEvent.AfterStack, "Custom/RGBShift")]
public sealed class RGBShift : PostProcessEffectSettings
{
    [Range(-1f, 1f), Tooltip("Red shift")]
    public FloatParameter rShift = new FloatParameter { value = 0.0f };
    [Range(-1f, 1f), Tooltip("Green shift")]
    public FloatParameter gShift = new FloatParameter { value = 0.0f };
    [Range(-1f, 1f), Tooltip("Blue shift")]
    public FloatParameter bShift = new FloatParameter { value = 0.0f };
}

public sealed class RGBShiftRenderer : PostProcessEffectRenderer<RGBShift>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/RGBShift"));
        sheet.properties.SetFloat("_Rshift", settings.rShift);
        sheet.properties.SetFloat("_Gshift", settings.gShift);
        sheet.properties.SetFloat("_Bshift", settings.bShift);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}