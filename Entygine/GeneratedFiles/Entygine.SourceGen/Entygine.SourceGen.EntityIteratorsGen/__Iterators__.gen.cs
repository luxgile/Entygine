using System.Runtime.CompilerServices;
namespace Entygine.Ecs
{
public partial class EntityIterator
{
//From file: D:\Development\VS 2019\Entygine\Entygine\Scripts\Common\Ecs\Systems\BoxHoverSystem.cs
public delegate void W<C0>(ref C0 c0) where C0 : struct, IComponent;
public IIteratorPhase2 Iterate<C0>(W<C0> iterator) where C0 : struct, IComponent
{
TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
AddType(withTypes, id0);
BakeSettings();
SetDelegate((chunk) => 
{
chunk.TryGetComponents(id0, out ComponentArray collection0);
for (int i = 0; i < chunk.Count; i++)
{
ref C0 c0 = ref collection0.GetRef<C0>(i);
iterator(ref c0);
}
});
return this;
}

//From file: D:\Development\VS 2019\Entygine\Entygine\Scripts\Common\Ecs\Systems\EditorCameraControlSystem.cs
public delegate void W_W<C0, C1>(ref C0 c0, ref C1 c1) where C0 : struct, IComponent where C1 : struct, IComponent;
public IIteratorPhase2 Iterate<C0, C1>(W_W<C0, C1> iterator) where C0 : struct, IComponent where C1 : struct, IComponent
{
TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
AddType(withTypes, id0);
TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
AddType(withTypes, id1);
BakeSettings();
SetDelegate((chunk) => 
{
chunk.TryGetComponents(id0, out ComponentArray collection0);
chunk.TryGetComponents(id1, out ComponentArray collection1);
for (int i = 0; i < chunk.Count; i++)
{
ref C0 c0 = ref collection0.GetRef<C0>(i);
ref C1 c1 = ref collection1.GetRef<C1>(i);
iterator(ref c0, ref c1);
}
});
return this;
}

//From file: D:\Development\VS 2019\Entygine\Entygine\Scripts\Common\Ecs\Systems\TransformConverterSystem.cs
public delegate void W_WO_WO_WO<C0, C1, C2, C3>(ref C0 c0, ref C1? c1, ref C2? c2, ref C3? c3) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent;
public IIteratorPhase2 Iterate<C0, C1, C2, C3>(W_WO_WO_WO<C0, C1, C2, C3> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent
{
TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
AddType(withTypes, id0);
TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
AddType(withTypes, id1);
TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
AddType(withTypes, id2);
TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
AddType(withTypes, id3);
BakeSettings();
SetDelegate((chunk) => 
{
chunk.TryGetComponents(id0, out ComponentArray collection0);
bool flag1 = chunk.TryGetComponents(id1, out ComponentArray collection1);
bool flag2 = chunk.TryGetComponents(id2, out ComponentArray collection2);
bool flag3 = chunk.TryGetComponents(id3, out ComponentArray collection3);
for (int i = 0; i < chunk.Count; i++)
{
ref C0 c0 = ref collection0.GetRef<C0>(i);
C1? c1 = flag1 ? collection1.Get<C1>(i) : null;
C2? c2 = flag2 ? collection2.Get<C2>(i) : null;
C3? c3 = flag3 ? collection3.Get<C3>(i) : null;
iterator(ref c0, ref c1, ref c2, ref c3);
if(flag1) collection1[i] = c1;
if(flag2) collection2[i] = c2;
if(flag3) collection3[i] = c3;
}
});
return this;
}

}
}
