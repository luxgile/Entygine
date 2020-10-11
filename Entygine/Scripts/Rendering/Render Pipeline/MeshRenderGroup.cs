using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace Entygine.Rendering
{
    public class MeshRenderGroup
    {
        public RenderMesh MeshRender { get; internal set; }
        public List<Matrix4> Transforms { get; internal set; }

        public MeshRenderGroup(RenderMesh meshRender)
        {
            MeshRender = meshRender;
            Transforms = new List<Matrix4>();
        }

        public bool HasMeshRender(RenderMesh meshRender)
        {
            return MeshRender.Equals(meshRender);
        }

        public void AddTransform(Matrix4 transform)
        {
            Transforms.Add(transform);
        }

        public void ClearTransforms()
        {
            Transforms.Clear();
        }
    }
}
