using Entygine.UI;
using OpenToolkit.Graphics.OpenGL4;
using OpenToolkit.Mathematics;
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
                GraphicsAPI.UseMeshMaterial(canvasRenderData.Mesh, canvasRenderData.Material);

                Matrix4 projection = canvasRenderData.Camera.CalculateProjection();
                canvasRenderData.Material.SetMatrix("projection", projection);

                List<UICanvas> canvases = canvasRenderData.GetCanvases();
                for (int i = 0; i < canvases.Count; i++)
                {
                    UICanvas canvas = canvases[i];
                    canvas.UpdateRenderers();
                    var renderables = canvas.GetRenderables();
                    for (int m = 0; m < renderables.Count; m++)
                    {
                        UI_IRenderable currRenderable = renderables[m];
                        GraphicsAPI.UseMeshMaterial(canvasRenderData.Mesh, currRenderable.Material);
                        canvasRenderData.Material.SetMatrix("model", currRenderable.Rect.GetModelMatrix());
                        canvasRenderData.Material.SetColor("color", currRenderable.Color);
                        GraphicsAPI.DrawTriangles(canvasRenderData.Mesh.GetIndiceCount());
                    }
                    //DrawElement(canvasRenderData, canvas.Root, canvas.GetModelMatrix());
                }
                GraphicsAPI.FreeMeshMaterial(canvasRenderData.Mesh, canvasRenderData.Material);
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
