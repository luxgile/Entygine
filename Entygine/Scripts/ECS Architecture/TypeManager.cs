using System;
using System.Linq;

namespace Entygine.Ecs
{
    public static partial class TypeManager
    {
        public static Type GetTypeFromId(TypeId id)
        {
            return idToType[id.Id];
        }

        public static TypeId GetIdFromType(Type type)
        {
            typeToId.TryGetValue(type, out TypeId id);
            return id;
        }
    }
}
//namespace Entygine.Ecs
//{
//    public static partial class TypeManager
//    {
//        static TypeManager()
//        {
//            idToType = new Type[10];
//        }
//    }
//}
