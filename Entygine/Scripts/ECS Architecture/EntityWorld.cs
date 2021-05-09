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

        public EntityManager EntityManager => entityManager;
        public EcsRunner Runner => ecsRunner;
        public static EntityWorld Active => activeWorld;
    }
}
