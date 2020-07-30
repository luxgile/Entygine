using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Entygine.Ecs
{
    public class EntityWorld
    {
        private static List<EntityWorld> worlds = new List<EntityWorld>();
        private static EntityWorld activeWorld;

        private EntityManager entityManager;
        private EcsRunner ecsRunner;

        private EntityWorld()
        {
            entityManager = new EntityManager();
            ecsRunner = new EcsRunner();
        }

        public static EntityWorld CreateWorld()
        {
            EntityWorld world = new EntityWorld();
            worlds.Add(world);
            return world;
        }

        public static void SetActive(EntityWorld world)
        {
            activeWorld = world;
        }

        public void DEBUG_LOG_INFO()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Entity Manager: \n");
            List<EntityChunk> chunks = entityManager.GetChunks();
            sb.Append("Chunks Count: " + chunks.Count + "\n");
            for (int i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                sb.Append($"Entities: {chunk.Count} / Version: {chunk.ChunkVersion} ");
                sb.Append("\n");
                sb.Append($"Archetype: {chunk.Archetype}");
                sb.Append("\n");
            }
            sb.Append("\n");
            Console.WriteLine(sb);
        }

        public EntityManager EntityManager => entityManager;
        public EcsRunner Runner => ecsRunner;
        public static EntityWorld Active => activeWorld;
    }
}
