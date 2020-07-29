using Entygine.Cycles;
using System;

namespace Entygine.Ecs
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SystemGroupAttribute : Attribute
    {
        public PhaseType PhaseType { get; private set; }
        public Type GroupType { get; private set; }

        public SystemGroupAttribute(Type groupType, PhaseType phase = PhaseType.Logic)
        {
            PhaseType = phase;

            if (typeof(IPhaseId).IsAssignableFrom(groupType))
                GroupType = groupType;
            else
                throw new ArgumentException("Type is not an interface inherited from " + typeof(IPhaseId).Name);
        }
    }

    public enum PhaseType { Logic, Render, Physics }
}
