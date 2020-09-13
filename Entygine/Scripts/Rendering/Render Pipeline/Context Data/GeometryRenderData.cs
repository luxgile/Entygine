using Entygine.Rendering.Pipeline;
using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entygine.Rendering
{
    public class GeometryRenderData : RenderContextData
    {
        private Dictionary<int, MeshRenderGroup> renderGroups = new Dictionary<int, MeshRenderGroup>();

        public int CreateRenderMeshGroup(RenderMesh renderMesh, out MeshRenderGroup group)
        {
            renderMesh.mat.LoadMaterial();

            int id = new Random().Next();
            group = new MeshRenderGroup(renderMesh);
            renderGroups.Add(id, group);

            return id;
        }

        public bool TryGetRenderMeshGroup(int id, out MeshRenderGroup group)
        {
            return renderGroups.TryGetValue(id, out group);
        }

        public List<MeshRenderGroup> GetRenderGroups() => renderGroups.Values.ToList();
    }
}
