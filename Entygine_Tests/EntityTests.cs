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
            EntityArchetype arch = new EntityArchetype(typeof(C_Transform));
            EntityManager em = new EntityManager();
            em.CreateEntity(arch);

            Assert.AreEqual(1, em.GetEntityCount());
        }
    }
}