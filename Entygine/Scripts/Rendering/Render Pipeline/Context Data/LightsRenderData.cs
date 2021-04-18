using Entygine.Rendering.Pipeline;
using OpenTK.Graphics.GL;
using System;
using System.Collections.Generic;

namespace Entygine.Rendering
{
    public class LightsRenderData : RenderContextData
    {
        public Material depthMat;
        public List<Light> lights = new List<Light>();

        public LightsRenderData()
        {
            Shader shader = Shader.CreateShaderWithPath(AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\simpleDepth.vert")
                , AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\simpleDepth.frag"), "Depth Shader");
            depthMat = new Material(shader, null);
            depthMat.LoadMaterial();
        }
    }
}
