using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OutlineFeature : ScriptableRendererFeature
{
    [Serializable]
    public class RenderSettings
    {
        public Material OverrideMaterial = null;
        public int OverrideMaterialPassIndex = 0;
        [Space]
        public LayerMask LayerMask = 0;
    }
    

    [SerializeField] public RenderPassEvent _renderPassEvent;

    [SerializeField] private Material _outlineMaterial;
    [SerializeField] private string _renderTextureName;

    [SerializeField] private RenderSettings _renderSettings;
    
    private RenderTargetHandle _renderTexture;

    private MyRenderObjectsPass _renderPass;
    private OutlinePass _outlinePass;

    public override void Create()
    {
        _renderTexture.Init(_renderTextureName);

        _renderPass = new MyRenderObjectsPass(_renderTexture, _renderSettings.LayerMask, _renderSettings.OverrideMaterial);
        _outlinePass = new OutlinePass(_outlineMaterial);

        _renderPass.renderPassEvent = _renderPassEvent;
        _outlinePass.renderPassEvent = _renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {

        renderer.EnqueuePass(_renderPass);
        renderer.EnqueuePass(_outlinePass);
    }
}
