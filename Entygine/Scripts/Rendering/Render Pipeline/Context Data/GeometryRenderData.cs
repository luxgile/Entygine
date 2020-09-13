using Entygine.Rendering.Pipeline;
using OpenToolkit.Mathematics;
using System.Collections.Generic;

namespace Entygine.Rendering
{
    public class GeometryRenderData : RenderContextData
    {
        private List<MeshRenderGroup> renderGroups = new List<MeshRenderGroup>();

        public void AddMesh(MeshRender meshRender, Matrix4 transform)
        {
            for (int i = 0; i < renderGroups.Count; i++)
            {
                MeshRenderGroup currentGroup = renderGroups[i];
                if (currentGroup.HasMeshRender(meshRender))
                {
                    currentGroup.AddTransform(transform);
                    return;
                }
            }

            MeshRenderGroup newGroup = new MeshRenderGroup(meshRender);
            newGroup.AddTransform(transform);
            renderGroups.Add(newGroup);
        }

        public List<MeshRenderGroup> GetRenderGroups() => renderGroups;
    }
}
