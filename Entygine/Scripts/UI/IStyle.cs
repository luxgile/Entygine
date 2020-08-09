using NUnit.Framework;

namespace Entygine.UI
{
    public interface IStyle
    {
        int topPadding { get; }
        int bottomPadding { get; }
        int rightPadding { get; }
        int leftPadding { get; }
    }
}
