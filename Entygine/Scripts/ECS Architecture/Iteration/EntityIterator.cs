using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Entygine.Ecs
{
    public interface IIteratorPhase1
    {
        IIteratorPhase1 With(params TypeId[] types);
        IIteratorPhase1 Any(params TypeId[] types);
        IIteratorPhase1 None(params TypeId[] types);
    }

    public interface IIteratorPhase2
    {
        uint Version { get; }
        void SetWorld(EntityWorld world);
        IIteratorPhase3 SetVersion(uint version);
    }
    public interface IIteratorPhase3
    {
        void Synchronous();
    }

    public delegate void IteratorAction();

    //public partial class EntityIterator : IIteratorPhase1, IIteratorPhase2, IIteratorPhase3
    //{
    //    private List<TypeId> anyTypes = new();
    //    private List<TypeId> withTypes = new();
    //    private List<TypeId> noneTypes = new();

    //    private EntityWorld world;
    //    private QuerySettings settings;
    //    private IteratorAction iteration;

    //    public uint Version { get; private set; }

    //    public EntityIterator()
    //    {
    //        settings = new();
    //    }

    //    public void SetWorld(EntityWorld world) => this.world = world;

    //    private void AddType(List<TypeId> list, TypeId type)
    //    {
    //        if (!list.Contains(type))
    //            list.Add(type);
    //    }

    //    public IIteratorPhase3 SetVersion(uint version)
    //    {
    //        Version = version;
    //        return this;
    //    }

    //    IIteratorPhase1 IIteratorPhase1.Any(params TypeId[] types) => Any(types);
    //    public IIteratorPhase1 Any(params TypeId[] types)
    //    {
    //        foreach (var type in types)
    //            AddType(anyTypes, type);

    //        return this;
    //    }

    //    IIteratorPhase1 IIteratorPhase1.None(params TypeId[] types) => None(types);
    //    public IIteratorPhase1 None(params TypeId[] types)
    //    {
    //        foreach (var type in types)
    //            AddType(noneTypes, type);
    //        return this;
    //    }

    //    IIteratorPhase1 IIteratorPhase1.With(params TypeId[] types) => With(types);
    //    public EntityIterator With(params TypeId[] types)
    //    {
    //        foreach (var type in types)
    //            AddType(withTypes, type);
    //        return this;
    //    }

    //    private void BakeSettings()
    //    {
    //        settings.With(withTypes.ToArray());
    //        settings.Any(anyTypes.ToArray());
    //        settings.None(noneTypes.ToArray());
    //    }

    //    public void Synchronous()
    //    {
    //        iteration();
    //    }

    //    //private void SetDelegate(Action<EntityChunk> act)
    //    //{
    //    //    iteration = () =>
    //    //    {
    //    //        world.EntityManager.GetChunks(settings, out int start, out int count);
    //    //        for (int i = 0; i < count; i++)
    //    //        {
    //    //            int index = i + start;
    //    //            EntityChunk chunk = world.EntityManager.GetChunk(index);

    //    //            if (!chunk.HasChanged(Version))
    //    //                continue;

    //    //            act(chunk);

    //    //            chunk.UpdateVersion(world.EntityManager.Version);
    //    //        }
    //    //    };
    //    //}

    //    //public delegate void Optional<C0, C1, C2, C3>(ref C0 c, ref C1? c1, ref C2? c2, ref C3? c3) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent;
    //    //public void Iterate<C0, C1, C2, C3>(Optional<C0, C1, C2, C3> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent
    //    //{
    //    //    TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
    //    //    TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
    //    //    TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
    //    //    TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
    //    //    AddType(withTypes, id0);
    //    //    AddType(anyTypes, id1);
    //    //    AddType(anyTypes, id2);
    //    //    AddType(anyTypes, id3);
    //    //    BakeSettings();
    //    //    SetDelegate((chunk) =>
    //    //    {
    //    //        chunk.TryGetComponents(id0, out ComponentArray collection0);
    //    //        bool flag1 = chunk.TryGetComponents(id1, out ComponentArray collection1);
    //    //        bool flag2 = chunk.TryGetComponents(id2, out ComponentArray collection2);
    //    //        bool flag3 = chunk.TryGetComponents(id3, out ComponentArray collection3);
    //    //        for (int e = 0; e < chunk.Count; e++)
    //    //        {
    //    //            ref C0 c0 = ref collection0.GetRef<C0>(e);
    //    //            C1? c1 = collection1?.Get<C1>(e);
    //    //            C2? c2 = collection2?.Get<C2>(e);
    //    //            C3? c3 = collection3?.Get<C3>(e);

    //    //            iterator(ref c0, ref c1, ref c2, ref c3);

    //    //            //This should only be done if the parameter is "ref", "in" variables won't be modified.
    //    //            if (c1 != null) collection1[e] = c1;
    //    //            if (c2 != null) collection2[e] = c2;
    //    //            if (c3 != null) collection3[e] = c3;
    //    //        }
    //    //    });
    //    //}
    //}

    public class ChunkIterator : IIteratorPhase1, IIteratorPhase2, IIteratorPhase3
    {
        private List<TypeId> anyTypes = new();
        private List<TypeId> withTypes = new();
        private List<TypeId> noneTypes = new();
        private List<EntityChunk> cachedChunks = new();

        private EntityWorld world;
        private QuerySettings settings;
        private IteratorAction iteration;

        public uint Version { get; private set; }

        public ChunkIterator()
        {
            settings = new();
        }

        public void SetWorld(EntityWorld world) => this.world = world;

        public IIteratorPhase3 SetVersion(uint version)
        {
            Version = version;
            return this;
        }

        private void AddType(List<TypeId> list, TypeId type)
        {
            if (!list.Contains(type))
                list.Add(type);
        }

        public IIteratorPhase1 Any(params TypeId[] types)
        {
            foreach (var type in types)
                AddType(anyTypes, type);

            return this;
        }

        public IIteratorPhase1 None(params TypeId[] types)
        {
            foreach (var type in types)
                AddType(noneTypes, type);
            return this;
        }

        IIteratorPhase1 IIteratorPhase1.With(params TypeId[] types) => With(types);
        public ChunkIterator With(params TypeId[] types)
        {
            foreach (var type in types)
                AddType(withTypes, type);
            return this;
        }

        public void Synchronous()
        {
            iteration();
        }

        private void SetDelegate(Action<EntityChunk> act)
        {
            iteration = () =>
            {
                world.EntityManager.GetChunks(settings, cachedChunks);
                for (int i = 0; i < cachedChunks.Count; i++)
                {
                    EntityChunk chunk = cachedChunks[i];

                    if (!chunk.HasChanged(Version))
                        continue;

                    act(chunk);

                    chunk.UpdateVersion(world.EntityManager.Version);
                }
            };
        }

        private void BakeSettings()
        {
            settings.With(withTypes.ToArray());
            settings.Any(anyTypes.ToArray());
            settings.None(noneTypes.ToArray());
        }

        public delegate void ChunkIterationDelegate(EntityChunk chunk);
        public unsafe IIteratorPhase2 Iterate(ChunkIterationDelegate iteration)
        {
            BakeSettings();
            SetDelegate((chunk) =>
            {
                iteration(chunk);
            });
            return this;
        }
    }
}
