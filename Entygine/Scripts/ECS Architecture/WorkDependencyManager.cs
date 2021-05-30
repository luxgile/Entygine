using Entygine.Async;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entygine.Ecs
{
    public class WorkDependencyManager
    {
        public class DependencyNode
        {
            public WorkAsyncHandle write;
            public List<WorkAsyncHandle> read = new();
        }

        private Dictionary<TypeId, DependencyNode> tree = new();

        /// <summary>
        /// Adds the iteration work into the dependency collection. 
        /// </summary>
        /// <param name="desc">What's going to be queried.</param>
        /// <param name="handle">Handle with the work that's going to be done.</param>
        /// <returns>Handle with dependencies inserted and ready to be started.</returns>
        public WorkAsyncHandle InsertDependencies(QueryDesc desc, WorkAsyncHandle handle, bool async)
        {
            List<WorkAsyncHandle> tasksToWait = new();
            IEnumerable<TypeId> types = desc.writeWith.Concat(desc.writeAny);
            foreach (TypeId writeId in types)
            {
                if (tree.TryGetValue(writeId, out DependencyNode node))
                {
                    if (node.read.Count > 0)
                    {
                        tasksToWait.AddRange(node.read);
                        node.read.Clear();
                    }
                    else if (node.write != null)
                    {
                        tasksToWait.Add(node.write);
                    }

                    node.write = handle;
                }
                else
                    tree.Add(writeId, new DependencyNode() { write = handle });
            }

            types = desc.readWith.Concat(desc.readAny);
            foreach (TypeId readId in types)
            {
                if (tree.TryGetValue(readId, out DependencyNode node))
                {
                    if (node.write != null)
                        tasksToWait.Add(node.write);

                    node.read.Add(handle);
                }
                else
                    tree.Add(readId, new DependencyNode() { read = { handle } });
            }

            return new WorkAsyncHandle(() =>
            {
                if (async)
                    handle.Start();
                else
                    handle.RunSync();
            }, tasksToWait.ToArray());
        }
    }
}
