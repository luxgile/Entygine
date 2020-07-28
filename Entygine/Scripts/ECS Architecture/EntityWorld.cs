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
        private List<LogicSystem> logicSystems;
        private List<RenderSystem> renderSystems;

        private EntityWorld()
        {
            entityManager = new EntityManager();
            logicSystems = new List<LogicSystem>();
            renderSystems = new List<RenderSystem>();
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

        public void PerformLogic()
        {
            for (int i = 0; i < logicSystems.Count; i++)
                logicSystems[i].PerformFrame();
        }

        public void PerformRender()
        {
            for (int i = 0; i < renderSystems.Count; i++)
                renderSystems[i].PerformFrame();
        }

        public T0 CreateLogicSystem<T0>() where T0 : LogicSystem, new()
        {
            //TODO: Check for system duplication
            T0 system = new T0();
            system.SetWorld(this);
            logicSystems.Add(system);
            return system;
        }
        public T0 CreateRenderSystem<T0>() where T0 : RenderSystem, new()
        {
            //TODO: Check for system duplication
            T0 system = new T0();
            system.SetWorld(this);
            renderSystems.Add(system);
            return system;
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
        public static EntityWorld Active => activeWorld;
    }
}
