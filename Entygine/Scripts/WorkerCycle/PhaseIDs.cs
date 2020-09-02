namespace Entygine.Cycles
{
    public class MainPhases
    {
        public struct EarlyPhaseId : IPhaseId { public static EarlyPhaseId Default => new EarlyPhaseId(); }
        public struct DefaultPhaseId : IPhaseId { public static DefaultPhaseId Default => new DefaultPhaseId(); }
        public struct LatePhaseId : IPhaseId { public static LatePhaseId Default => new LatePhaseId(); }
    }
}
