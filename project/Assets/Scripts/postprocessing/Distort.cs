﻿using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(DistortRenderer), PostProcessEvent.AfterStack, "Custom/Distort")]
public sealed class Distort : PostProcessEffectSettings
{
    [Range(0f, 5f), Tooltip("Distort intensity")]
    public FloatParameter intensity = new FloatParameter { value = 0.1f };
    [Range(0f, 4096f), Tooltip("Distort line height")]
    public FloatParameter height = new FloatParameter { value = Screen.height*2f };
}

public sealed class DistortRenderer : PostProcessEffectRenderer<Distort>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Distort"));
        sheet.properties.SetFloat("_Intensity", settings.intensity/100.0f);
        sheet.properties.SetFloat("_Height", settings.height);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}