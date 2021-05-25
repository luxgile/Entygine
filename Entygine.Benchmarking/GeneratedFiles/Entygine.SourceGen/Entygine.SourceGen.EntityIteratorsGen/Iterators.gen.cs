using System.Runtime.CompilerServices;
using System.Collections.Generic;
namespace Entygine.Ecs
{
internal partial class BenchmarkIterationGenerated : IIteratorPhase1, IIteratorPhase2, IIteratorPhase3
{

        private List<TypeId> anyTypes = new();
        private List<TypeId> withTypes = new();
        private List<TypeId> noneTypes = new();

        private EntityWorld world;
        private QuerySettings settings;
        private IteratorAction iteration;

        public uint Version { get; private set; }

        public BenchmarkIterationGenerated()
        {
            settings = new();
        }

        public void SetWorld(EntityWorld world) => this.world = world;

        private void AddType(List<TypeId> list, TypeId type)
        {
            if (!list.Contains(type))
                list.Add(type);
        }

        public IIteratorPhase3 SetVersion(uint version)
        {
            Version = version;
            return this;
        }

        IIteratorPhase1 IIteratorPhase1.Any(params TypeId[] types) => Any(types);
        public IIteratorPhase1 Any(params TypeId[] types)
        {
            foreach (var type in types)
                AddType(anyTypes, type);

            return this;
        }

        IIteratorPhase1 IIteratorPhase1.None(params TypeId[] types) => None(types);
        public IIteratorPhase1 None(params TypeId[] types)
        {
            foreach (var type in types)
                AddType(noneTypes, type);
            return this;
        }

        IIteratorPhase1 IIteratorPhase1.With(params TypeId[] types) => With(types);
        public BenchmarkIterationGenerated With(params TypeId[] types)
        {
            foreach (var type in types)
                AddType(withTypes, type);
            return this;
        }

        private void BakeSettings()
        {
            settings.With(withTypes.ToArray());
            settings.Any(anyTypes.ToArray());
            settings.None(noneTypes.ToArray());
        }

        public void Synchronous()
        {
            iteration();
        }

}
}
