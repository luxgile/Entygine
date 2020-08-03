using Entygine.Rendering.Pipeline;
using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;

namespace Entygine.Rendering
{
    public struct RenderContext
    {
        private List<RenderContextData> datas;

        public RenderContext(RenderCommandBuffer commandBuffer)
        {
            CommandBuffer = commandBuffer ?? throw new ArgumentNullException(nameof(commandBuffer));
            datas = new List<RenderContextData>();
        }

        public void ClearBuffer()
        {
            CommandBuffer.Clear();
        }

        public bool TryGetData<T0>(out T0 data) where T0 : RenderContextData
        {
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i] is T0 result)
                {
                    data = result;
                    return true;
                }
            }

            data = null;
            return false;
        }

        public void AddData(RenderContextData data)
        {
            datas.Add(data);
        }

        public bool HasData<T0>() where T0 : RenderContextData
        {
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i] is T0)
                    return true;
            }

            return false;
        }

        public RenderCommandBuffer CommandBuffer { get; }
    }
}
