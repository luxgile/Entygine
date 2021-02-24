namespace Entygine
{
    public static class FrameContext
    {
        public static FrameData Current { get; internal set; }
    }

    public struct FrameData
    {
        public int count;
        public float delta;

        public FrameData(int count, float delta)
        {
            this.count = count;
            this.delta = delta;
        }
    }
}
