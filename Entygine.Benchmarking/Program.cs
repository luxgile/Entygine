using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Entygine.Async;
using Entygine.Ecs;
using Entygine.Ecs.Components;
using Entygine.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Entygine.Benchmarking
{
    public struct Dummy : IComponent { public int value; }
    public struct A : IComponent { }
    public struct B : IComponent { }
    public struct C : IComponent { }
    public class IdToTypeComparison
    {
        public Dictionary<int, Type> intToType = new();
        public Dictionary<TypeId, Type> idToType = new();
        public Dictionary<Type, TypeId> typeToId = new();
        public const int count = 100;

        public IdToTypeComparison()
        {
            intToType.Add(0, typeof(A));
            intToType.Add(1, typeof(A));
            intToType.Add(2, typeof(A));
            intToType.Add(3, typeof(A));
            intToType.Add(4, typeof(A));
            intToType.Add(5, typeof(A));
            intToType.Add(6, typeof(A));
            intToType.Add(7, typeof(A));
            intToType.Add(8, typeof(A));
            intToType.Add(9, typeof(A));
            intToType.Add(10, typeof(A));

            idToType.Add(new TypeId(0), typeof(A));
            idToType.Add(new TypeId(1), typeof(A));
            idToType.Add(new TypeId(2), typeof(A));
            idToType.Add(new TypeId(3), typeof(A));
            idToType.Add(new TypeId(4), typeof(A));
            idToType.Add(new TypeId(5), typeof(A));
            idToType.Add(new TypeId(6), typeof(A));
            idToType.Add(new TypeId(7), typeof(A));
            idToType.Add(new TypeId(8), typeof(A));
            idToType.Add(new TypeId(9), typeof(A));
            idToType.Add(new TypeId(10), typeof(A));

            typeToId.Add(typeof(A), new TypeId(0));
            typeToId.Add(typeof(B), new TypeId(0));
            typeToId.Add(typeof(C), new TypeId(0));
            typeToId.Add(typeof(C_PhysicsBody), new TypeId(0));
            typeToId.Add(typeof(C_Position), new TypeId(0));
            typeToId.Add(typeof(C_Rotation), new TypeId(0));
            typeToId.Add(typeof(C_UniformScale), new TypeId(0));
            typeToId.Add(typeof(C_Transform), new TypeId(0));
            typeToId.Add(typeof(C_Camera), new TypeId(0));
        }
        [Benchmark]
        public Type IntToType()
        {
            int typeInt = 50;
            Type type = null;
            bool found = false;
            for (int i = 0; i < count; i++)
            {
                found = intToType.TryGetValue(typeInt, out type);
            }
            return type;
        }
        [Benchmark]
        public Type IdToType()
        {
            TypeId typeId = new TypeId(50);
            Type type = null;
            bool found = false;
            for (int i = 0; i < count; i++)
            {
                found = idToType.TryGetValue(typeId, out type);
            }
            return type;
        }
        [Benchmark]
        public TypeId TypeToId()
        {
            TypeId typeId = default;
            Type type = typeof(A);
            bool found = false;
            for (int i = 0; i < count; i++)
            {
                found = typeToId.TryGetValue(type, out typeId);
            }
            return typeId;
        }
    }
    public class TypeComparison
    {
        private int a = 5;
        private int b = 3;

        [Benchmark]
        public bool TypeCompare()
        {
            return typeof(A).Equals(typeof(B));
        }
        [Benchmark]
        public bool IntCompare()
        {
            return a.Equals(b);
        }
        [Benchmark]
        public bool TypeCacheCompare()
        {
            return TypeCache.ReadType<A>().Equals(TypeCache.ReadType<B>());
        }
    }

    //public class ReadComponent
    //{
    //    private int count = 10000;
    //    private EntityWorld world;
    //    private EntityArchetype arch;

    //    public ReadComponent()
    //    {
    //        world = EntityWorld.CreateWorld();
    //        arch = new EntityArchetype(typeof(Dummy));
    //        Stopwatch sw = new Stopwatch();
    //        sw.Start();
    //        for (int i = 0; i < count; i++)
    //            world.EntityManager.CreateEntity(arch);
    //        sw.Stop();
    //        Console.WriteLine($"Entity Creation: {sw.ElapsedMilliseconds}ms");
    //    }

    //    [Benchmark(Description = "Default reading component")]
    //    public int Read()
    //    {
    //        int temp = 0;
    //        QuerySettings settings = new QuerySettings().With(TypeCache.ReadType<Dummy>());
    //        Stopwatch sw = new Stopwatch();
    //        sw.Start();
    //        new EntityQueryScope(settings, world, (context) =>
    //        {
    //            context.Read(out Dummy dummy);
    //            temp = dummy.value;
    //        }).Perform();
    //        sw.Stop();
    //        Console.WriteLine($"Read: {sw.ElapsedMilliseconds}ms");
    //        return temp;
    //    }

    //    public int Write()
    //    {
    //        int temp = 0;
    //        QuerySettings settings = new QuerySettings().With(TypeCache.ReadType<Dummy>());
    //        Stopwatch sw = new Stopwatch();
    //        sw.Start();
    //        new EntityQueryScope(settings, world, (context) =>
    //        {
    //            context.Read(out Dummy dummy);
    //            dummy.value = 10;
    //            context.Write(dummy);
    //        }).Perform();
    //        sw.Stop();
    //        Console.WriteLine($"Write: {sw.ElapsedMilliseconds}ms");
    //        return temp;
    //    }

    //    [Benchmark(Description = "Raw reading component")]
    //    public int RawRead()
    //    {
    //        int temp = 0;
    //        var chunks = world.EntityManager.GetChunks();
    //        var type = TypeCache.ReadType<Dummy>();
    //        Stopwatch sw = new Stopwatch();
    //        sw.Start();
    //        for (int i = 0; i < chunks.Count; i++)
    //        {
    //            var currchunk = chunks[i];
    //            if (currchunk.Archetype.HasAnyTypes(type))
    //            {
    //                currchunk.TryGetComponents<Dummy>(out ComponentArray array);
    //                for (int t = 0; t < array.Count; t++)
    //                {
    //                    Dummy dummy = (Dummy)array[t];
    //                    temp = dummy.value;
    //                }
    //            }
    //        }
    //        sw.Stop();
    //        Console.WriteLine(sw.ElapsedMilliseconds);
    //        return temp;
    //    }
    //}
    [GenerateIteratorsTarget]
    internal partial class BenchmarkIterationGenerated { }
    public class IterationDiff
    {
        private EntityWorld world;
        private EntityQueryScope query;
        private BenchmarkIterationGenerated iterator;

        public IterationDiff()
        {
            world = EntityWorld.CreateWorld();
            world.EntityManager.CreateEntities(new EntityArchetype(C_Position.Identifier), 10000);
            QuerySettings settings = new QuerySettings().With(C_Position.Identifier);
            iterator = new BenchmarkIterationGenerated();
            iterator.SetWorld(world);
            query = new EntityQueryScope(settings, world, (ref EntityQueryContext context) =>
            {
                context.Read(C_Position.Identifier, out C_Position position);
                position.value = Vec3f.Zero;
                context.Write(C_Position.Identifier, position);
            });
        }

        [Benchmark]
        public void OldIterate()
        {
            query.Perform();
        }

        [Benchmark]
        public void NewIterate()
        {
            iterator.Iterate((ref C_Position position) =>
            {
                position.value = Vec3f.Zero;
            }).SetVersion(0).Synchronous();
        }
    }

    public class Program
    {
        private static void Main(string[] args)
        {
            //BenchmarkRunner.Run<IterationDiff>();
            //BenchmarkRunner.Run<ReadComponent>();
            //var rc = new ReadComponent();
            //rc.Read();
            //rc.Write();

            WorkAsyncHandle lastWork = null;
            for (int i = 0; i < 100; i++)
            {
                int index = i;
                lastWork = new WorkAsyncHandle(() => Wait(index), lastWork);
                lastWork.Async();
            }

            lastWork.FinishWork();
            Console.WriteLine("Finished.");
            Console.Read();
        }

        private static void Wait(int i)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {i}");
        }
    }
}
