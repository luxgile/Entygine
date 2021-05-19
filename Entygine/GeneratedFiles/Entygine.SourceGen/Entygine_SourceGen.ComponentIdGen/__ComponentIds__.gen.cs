#region USINGS
using Entygine.Ecs;
using System;
using System.Collections.Generic;
using Entygine.Ecs.Components;
using Entygine.Ecs;
using Entygine.Ecs.Systems;
using Entygine.UI;
#endregion

#region COMPONENTS
namespace Entygine.Ecs.Components
{
	public partial struct C_EditorCamera { public static TypeId Identifier { get; } = new (0); }
	public partial struct C_Camera { public static TypeId Identifier { get; } = new (1); }
	public partial struct SC_RenderMesh { public static TypeId Identifier { get; } = new (2); }
}
namespace Entygine.Ecs
{
	public partial struct C_Position { public static TypeId Identifier { get; } = new (3); }
	public partial struct C_Rotation { public static TypeId Identifier { get; } = new (4); }
	public partial struct C_Transform { public static TypeId Identifier { get; } = new (5); }
	public partial struct C_UniformScale { public static TypeId Identifier { get; } = new (6); }
	public partial struct C_PhysicsBody { public static TypeId Identifier { get; } = new (7); }
}
namespace Entygine.Ecs.Systems
{
	public partial struct C_BoxTag { public static TypeId Identifier { get; } = new (8); }
}
namespace Entygine.UI
{
	public partial struct C_UICanvas { public static TypeId Identifier { get; } = new (9); }
}
#endregion
#region TYPE MANAGER
namespace Entygine.Ecs
{
	public static partial class TypeManager 
	{
		private static readonly Type[] idToType = Array.Empty<Type>();
		
		private static readonly Dictionary<Type, TypeId> typeToId = new Dictionary<Type, TypeId>();
		
		static TypeManager() {
			idToType = new Type[10];
			 idToType[0] = typeof(C_EditorCamera);
			 typeToId.Add(typeof(C_EditorCamera), new TypeId(0));
			 idToType[1] = typeof(C_Camera);
			 typeToId.Add(typeof(C_Camera), new TypeId(1));
			 idToType[2] = typeof(SC_RenderMesh);
			 typeToId.Add(typeof(SC_RenderMesh), new TypeId(2));
			 idToType[3] = typeof(C_Position);
			 typeToId.Add(typeof(C_Position), new TypeId(3));
			 idToType[4] = typeof(C_Rotation);
			 typeToId.Add(typeof(C_Rotation), new TypeId(4));
			 idToType[5] = typeof(C_Transform);
			 typeToId.Add(typeof(C_Transform), new TypeId(5));
			 idToType[6] = typeof(C_UniformScale);
			 typeToId.Add(typeof(C_UniformScale), new TypeId(6));
			 idToType[7] = typeof(C_PhysicsBody);
			 typeToId.Add(typeof(C_PhysicsBody), new TypeId(7));
			 idToType[8] = typeof(C_BoxTag);
			 typeToId.Add(typeof(C_BoxTag), new TypeId(8));
			 idToType[9] = typeof(C_UICanvas);
			 typeToId.Add(typeof(C_UICanvas), new TypeId(9));
		}
	}
}
#endregion