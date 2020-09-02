using System;

namespace Entygine.Ecs
{
    public class AfterSystemAttribute : Attribute
    {
        public Type SystemType { get; }

        public AfterSystemAttribute(Type systemType)
        {
            SystemType = systemType; 
        }
    }

    public class BeforeSystemAttribute : Attribute
    {
        public Type SystemType { get; }

        public BeforeSystemAttribute(Type systemType)
        {
            SystemType = systemType;
        }
    }
}
