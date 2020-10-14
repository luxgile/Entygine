using Entygine.Cycles;
using System;

namespace Entygine.Ecs
{
    /// <summary>
    /// Used to assign a system to a defined logic group. Avaliable ones are inside <see cref="MainPhases"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SystemGroupAttribute : Attribute
    {
        public PhaseType PhaseType { get; private set; }
        public Type GroupType { get; private set; }

        public SystemGroupAttribute(Type phaseType, PhaseType phase = PhaseType.Logic)
        {
            PhaseType = phase;

            if (typeof(IPhaseId).IsAssignableFrom(phaseType))
                GroupType = phaseType;
            else
                throw new ArgumentException("Type is not an interface inherited from " + typeof(IPhaseId).Name);
        }
    }

    public enum PhaseType { Logic, Render }
}
