using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurPass : ScriptableRenderPass
{
    private RenderTargetHandle _destination;

    private Material _blurMaterial;

    private List<ShaderTagId> _shaderTagIdList = new List<ShaderTagId>() { new ShaderTagId("UniversalForward") };
    private FilteringSettings _filteringSettings;
    private RenderStateBlock _renderStateBlock;

    public BlurPass(RenderTargetHandle destination, int layerMask, Material blurMaterial)
    {
        _destination = destination;

        _blurMaterial = blurMaterial;

        _filteringSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask);
        _renderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
    }
    

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        cmd.GetTemporaryRT(_destination.id, cameraTextureDescriptor);
        ConfigureTarget(_destination.Identifier());
        ConfigureClear(ClearFlag.Color, Color.clear);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        SortingCriteria sortingCriteria = renderingData.cameraData.defaultOpaqueSortFlags;
        DrawingSettings drawingSettings = CreateDrawingSettings(_shaderTagIdList, ref renderingData, sortingCriteria);
        drawingSettings.overrideMaterial = _blurMaterial;

        context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref _filteringSettings, ref _renderStateBlock);
    }
}


