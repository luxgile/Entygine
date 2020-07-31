using Entygine.Ecs;
using NUnit.Framework;

namespace Entygine.Tests
{
    public class StructArray_Tests
    {
        [Test]
        public void Adding_1()
        {
            StructArray<Entity> entities = new StructArray<Entity>();
            Entity entity = new Entity() { id = 2, version = 1 };
            entities.Add(entity);

            Assert.AreEqual(entities.Count, 1);
        }

        [Test]
        public void Adding_2()
        {
            StructArray<Entity> entities = new StructArray<Entity>();
            Entity entity = new Entity() { id = 2, version = 1 };
            entities.Add(entity);

            entity.id = 3;
            entities.Add(entity);

            Assert.AreEqual(entities.Count, 2);
        }

        [Test]
        public void Insert_1()
        {
            StructArray<Entity> entities = new StructArray<Entity>();
            Entity entity = new Entity() { id = 2, version = 1 };
            entities.Add(entity);

            entity.id = 3;
            entities.Insert(0, entity);

            Assert.AreEqual(entities[0].id, 3);
        }

        [Test]
        public void Removing_1()
        {
            StructArray<Entity> entities = new StructArray<Entity>();
            Entity entity = new Entity() { id = 2, version = 1 };
            entities.Add(entity);

            entity.id = 3;
            entities.Add(entity);

            entity.id = 2;
            entities.SwapForLast(entity);

            Assert.AreEqual(entities.Count, 1);
        }

        [Test]
        public void Removing_2()
        {
            StructArray<Entity> entities = new StructArray<Entity>();
            Entity entity = new Entity();
            entities.Add(entity);

            entity.id = 1;
            entities.Add(entity);

            entity.id = 2;
            entities.Add(entity);

            entity.id = 3;
            entities.Add(entity);

            entities.SwapForLast(0);

            Assert.AreEqual(entities[0].id, 3);
        }

        [Test]
        public void Referencing()
        {
            StructArray<Entity> entities = new StructArray<Entity>(8, true);
            ref Entity entity = ref entities[0];
            entity.id = 1;

            Assert.AreEqual(entities[0].id, 1);
        }
    }
}