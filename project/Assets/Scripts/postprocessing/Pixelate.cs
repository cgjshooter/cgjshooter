using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PixelateRenderer), PostProcessEvent.AfterStack, "Custom/Pixelate")]
public sealed class Pixelate : PostProcessEffectSettings
{
    public Vector2Parameter pixelate = new Vector2Parameter { value = new Vector2(Screen.width, Screen.height) };
}

public sealed class PixelateRenderer : PostProcessEffectRenderer<Pixelate>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Pixelate"));
        sheet.properties.SetVector("_Pixelate", settings.pixelate);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}