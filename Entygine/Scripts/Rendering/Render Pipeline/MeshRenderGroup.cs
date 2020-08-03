using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;

namespace Entygine.Rendering
{
    public class MeshRenderGroup
    {
        public MeshRender MeshRender { get; internal set; }
        public List<Matrix4> Transforms { get; internal set; }

        public MeshRenderGroup(MeshRender meshRender)
        {
            MeshRender = meshRender;
            Transforms = new List<Matrix4>();
        }

        public bool HasMeshRender(MeshRender meshRender)
        {
            return MeshRender.Equals(meshRender);
        }

        public void AddTransform(Matrix4 transform)
        {
            Transforms.Add(transform);
        }
    }
}
