using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Entygine.Ecs;
using System;
using System.Diagnostics;

namespace Entygine.Benchmarking
{
    public struct Dummy : IComponent { public int value; }
    public struct A : IComponent { }
    public struct B : IComponent { }
    public struct C : IComponent { }
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

    public class ReadComponent
    {
        private int count = 10000;
        private EntityWorld world;
        private EntityArchetype arch;

        public ReadComponent()
        {
            world = EntityWorld.CreateWorld();
            arch = new EntityArchetype(typeof(Dummy));
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < count; i++)
                world.EntityManager.CreateEntity(arch);
            sw.Stop();
            Console.WriteLine($"Entity Creation: {sw.ElapsedMilliseconds}ms");
        }

        [Benchmark(Description = "Default reading component")]
        public int Read()
        {
            int temp = 0;
            QuerySettings settings = new QuerySettings().With(TypeCache.ReadType<Dummy>());
            Stopwatch sw = new Stopwatch();
            sw.Start();
            new EntityQueryScope(settings, world, (context) =>
            {
                context.Read(out Dummy dummy);
                temp = dummy.value;
            }).Perform();
            sw.Stop();
            Console.WriteLine($"Read: {sw.ElapsedMilliseconds}ms");
            return temp;
        }

        public int Write()
        {
            int temp = 0;
            QuerySettings settings = new QuerySettings().With(TypeCache.ReadType<Dummy>());
            Stopwatch sw = new Stopwatch();
            sw.Start();
            new EntityQueryScope(settings, world, (context) =>
            {
                context.Read(out Dummy dummy);
                dummy.value = 10;
                context.Write(dummy);
            }).Perform();
            sw.Stop();
            Console.WriteLine($"Write: {sw.ElapsedMilliseconds}ms");
            return temp;
        }

        [Benchmark(Description = "Raw reading component")]
        public int RawRead()
        {
            int temp = 0;
            var chunks = world.EntityManager.GetChunks();
            var type = TypeCache.ReadType<Dummy>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < chunks.Count; i++)
            {
                var currchunk = chunks[i];
                if (currchunk.Archetype.HasAnyTypes(type))
                {
                    currchunk.TryGetComponents<Dummy>(out ComponentArray array);
                    for (int t = 0; t < array.Count; t++)
                    {
                        Dummy dummy = (Dummy)array[t];
                        temp = dummy.value;
                    }
                }
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            return temp;
        }
    }

    public class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<TypeComparison>();
            //BenchmarkRunner.Run<ReadComponent>();
            //var rc = new ReadComponent();
            //rc.Read();
            //rc.Write();
            Console.Read();
        }
    }
}
