namespace Entygine.Rendering
{
    public abstract class Light
    {
        private int depthMapHandle;

        public Light()
        {
            depthMapHandle = Ogl.GenFramebuffer();
        }

        public abstract int DepthMapHandle { get; }
    }
}
