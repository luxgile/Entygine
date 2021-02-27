using Entygine.UI;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Entygine.Rendering.Pipeline
{
    public static partial class RenderCommandsLibrary
    {
        public static RenderCommand DrawUI()
        {
            return new RenderCommand("Draw UI", (ref RenderContext context) =>
            {
                if (!context.TryGetData(out UICanvasRenderData canvasRenderData))
                    return;

                Ogl.Disable(EnableCap.CullFace);
                Ogl.Disable(EnableCap.DepthTest);

                Ogl.Enable(EnableCap.Blend);
                Ogl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                Matrix4 projection = canvasRenderData.Camera.CalculateProjection();

                List<UICanvas> canvases = canvasRenderData.GetCanvases();
                for (int i = 0; i < canvases.Count; i++)
                {
                    UICanvas canvas = canvases[i];
                    canvas.deltaTimeText.Text = $"{FrameContext.Current.count} frame - {FrameContext.Current.delta * 1000:F1}ms - {1 / FrameContext.Current.delta:F1}fps";

                    canvas.UpdateRenderers();
                    var renderables = canvas.GetRenderables();
                    for (int m = 0; m < renderables.Count; m++)
                    {
                        UI_IRenderable currRenderable = renderables[m];
                        currRenderable.Material.SetMatrix("projection", projection);
                        currRenderable.DrawUI(canvasRenderData.Mesh);
                    }
                }

                Ogl.Enable(EnableCap.CullFace);
                Ogl.Enable(EnableCap.DepthTest);
                Ogl.Disable(EnableCap.Blend);
            });

            //static void DrawElement(UICanvasRenderData canvasRenderData, UIElement element, Matrix4 parentMatrix)
            //{
            //    Matrix4 model = element.GetModelMatrix(parentMatrix);
            //    canvasRenderData.Material.SetMatrix("model", model);
            //    canvasRenderData.Material.SetColor("color", element.Color);
            //    GraphicsAPI.DrawTriangles(canvasRenderData.Mesh.GetIndiceCount());
            //}
        }
    }
}
