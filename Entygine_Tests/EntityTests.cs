using Entygine;
using Entygine.Ecs;
using NUnit.Framework;

namespace EntygineTests
{
    public class EntityTests
    {
        [Test]
        public void CreateEntity()
        {
            EntityArchetype arch = new EntityArchetype(C_Transform.Identifier);
            EntityManager em = new EntityManager();
            em.CreateEntity(arch);

            Assert.AreEqual(1, em.GetEntityCount());
        }


        [Test]
        public void CreateEntities()
        {
            EntityArchetype arch = new EntityArchetype(C_Transform.Identifier);
            EntityManager em = new EntityManager();
            em.CreateEntities(arch, 100);

            Assert.AreEqual(100, em.GetEntityCount());
        }
    }
}