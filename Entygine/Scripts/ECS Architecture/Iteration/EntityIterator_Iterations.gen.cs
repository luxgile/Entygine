using System.Runtime.CompilerServices;

namespace Entygine.Ecs
{
    public static class GeneratedIterators
    {
          public delegate void R<C0>(in C0 read0)
            where C0 : struct, IComponent ;
          public delegate void W<C0>(ref C0 write0)
            where C0 : struct, IComponent ;
          public delegate void RW<C0, C1>(in C0 read0, ref C1 write1)
            where C0 : struct, IComponent where C1 : struct, IComponent ;
          public delegate void RR<C0, C1>(in C0 read0, in C1 read1)
            where C0 : struct, IComponent where C1 : struct, IComponent ;
          public delegate void WW<C0, C1>(ref C0 write0, ref C1 write1)
            where C0 : struct, IComponent where C1 : struct, IComponent ;
          public delegate void RWW<C0, C1, C2>(in C0 read0, ref C1 write1, ref C2 write2)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent ;
          public delegate void RRW<C0, C1, C2>(in C0 read0, in C1 read1, ref C2 write2)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent ;
          public delegate void RRR<C0, C1, C2>(in C0 read0, in C1 read1, in C2 read2)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent ;
          public delegate void WWW<C0, C1, C2>(ref C0 write0, ref C1 write1, ref C2 write2)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent ;
          public delegate void RWWW<C0, C1, C2, C3>(in C0 read0, ref C1 write1, ref C2 write2, ref C3 write3)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent ;
          public delegate void RRWW<C0, C1, C2, C3>(in C0 read0, in C1 read1, ref C2 write2, ref C3 write3)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent ;
          public delegate void RRRW<C0, C1, C2, C3>(in C0 read0, in C1 read1, in C2 read2, ref C3 write3)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent ;
          public delegate void RRRR<C0, C1, C2, C3>(in C0 read0, in C1 read1, in C2 read2, in C3 read3)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent ;
          public delegate void WWWW<C0, C1, C2, C3>(ref C0 write0, ref C1 write1, ref C2 write2, ref C3 write3)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent ;
          public delegate void RWWWW<C0, C1, C2, C3, C4>(in C0 read0, ref C1 write1, ref C2 write2, ref C3 write3, ref C4 write4)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent ;
          public delegate void RRWWW<C0, C1, C2, C3, C4>(in C0 read0, in C1 read1, ref C2 write2, ref C3 write3, ref C4 write4)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent ;
          public delegate void RRRWW<C0, C1, C2, C3, C4>(in C0 read0, in C1 read1, in C2 read2, ref C3 write3, ref C4 write4)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent ;
          public delegate void RRRRW<C0, C1, C2, C3, C4>(in C0 read0, in C1 read1, in C2 read2, in C3 read3, ref C4 write4)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent ;
          public delegate void RRRRR<C0, C1, C2, C3, C4>(in C0 read0, in C1 read1, in C2 read2, in C3 read3, in C4 read4)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent ;
          public delegate void WWWWW<C0, C1, C2, C3, C4>(ref C0 write0, ref C1 write1, ref C2 write2, ref C3 write3, ref C4 write4)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent ;
          public delegate void RWWWWW<C0, C1, C2, C3, C4, C5>(in C0 read0, ref C1 write1, ref C2 write2, ref C3 write3, ref C4 write4, ref C5 write5)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent ;
          public delegate void RRWWWW<C0, C1, C2, C3, C4, C5>(in C0 read0, in C1 read1, ref C2 write2, ref C3 write3, ref C4 write4, ref C5 write5)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent ;
          public delegate void RRRWWW<C0, C1, C2, C3, C4, C5>(in C0 read0, in C1 read1, in C2 read2, ref C3 write3, ref C4 write4, ref C5 write5)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent ;
          public delegate void RRRRWW<C0, C1, C2, C3, C4, C5>(in C0 read0, in C1 read1, in C2 read2, in C3 read3, ref C4 write4, ref C5 write5)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent ;
          public delegate void RRRRRW<C0, C1, C2, C3, C4, C5>(in C0 read0, in C1 read1, in C2 read2, in C3 read3, in C4 read4, ref C5 write5)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent ;
          public delegate void RRRRRR<C0, C1, C2, C3, C4, C5>(in C0 read0, in C1 read1, in C2 read2, in C3 read3, in C4 read4, in C5 read5)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent ;
          public delegate void WWWWWW<C0, C1, C2, C3, C4, C5>(ref C0 write0, ref C1 write1, ref C2 write2, ref C3 write3, ref C4 write4, ref C5 write5)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent ;
          public delegate void RWWWWWW<C0, C1, C2, C3, C4, C5, C6>(in C0 read0, ref C1 write1, ref C2 write2, ref C3 write3, ref C4 write4, ref C5 write5, ref C6 write6)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent ;
          public delegate void RRWWWWW<C0, C1, C2, C3, C4, C5, C6>(in C0 read0, in C1 read1, ref C2 write2, ref C3 write3, ref C4 write4, ref C5 write5, ref C6 write6)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent ;
          public delegate void RRRWWWW<C0, C1, C2, C3, C4, C5, C6>(in C0 read0, in C1 read1, in C2 read2, ref C3 write3, ref C4 write4, ref C5 write5, ref C6 write6)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent ;
          public delegate void RRRRWWW<C0, C1, C2, C3, C4, C5, C6>(in C0 read0, in C1 read1, in C2 read2, in C3 read3, ref C4 write4, ref C5 write5, ref C6 write6)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent ;
          public delegate void RRRRRWW<C0, C1, C2, C3, C4, C5, C6>(in C0 read0, in C1 read1, in C2 read2, in C3 read3, in C4 read4, ref C5 write5, ref C6 write6)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent ;
          public delegate void RRRRRRW<C0, C1, C2, C3, C4, C5, C6>(in C0 read0, in C1 read1, in C2 read2, in C3 read3, in C4 read4, in C5 read5, ref C6 write6)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent ;
          public delegate void RRRRRRR<C0, C1, C2, C3, C4, C5, C6>(in C0 read0, in C1 read1, in C2 read2, in C3 read3, in C4 read4, in C5 read5, in C6 read6)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent ;
          public delegate void WWWWWWW<C0, C1, C2, C3, C4, C5, C6>(ref C0 write0, ref C1 write1, ref C2 write2, ref C3 write3, ref C4 write4, ref C5 write5, ref C6 write6)
            where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent ;
    }

    public partial class EntityIterator
    {
          public IIteratorPhase2 Iterate<C0> (GeneratedIterators.R<C0> iterator) where C0 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0> (GeneratedIterators.W<C0> iterator) where C0 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(ref Unsafe.Unbox<C0>(collection0[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1> (GeneratedIterators.RW<C0, C1> iterator) where C0 : struct, IComponent where C1 : struct, IComponent 
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
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), ref Unsafe.Unbox<C1>(collection1[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1> (GeneratedIterators.RR<C0, C1> iterator) where C0 : struct, IComponent where C1 : struct, IComponent 
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
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1> (GeneratedIterators.WW<C0, C1> iterator) where C0 : struct, IComponent where C1 : struct, IComponent 
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
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(ref Unsafe.Unbox<C0>(collection0[e]), ref Unsafe.Unbox<C1>(collection1[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2> (GeneratedIterators.RWW<C0, C1, C2> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), ref Unsafe.Unbox<C1>(collection1[e]), ref Unsafe.Unbox<C2>(collection2[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2> (GeneratedIterators.RRW<C0, C1, C2> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), ref Unsafe.Unbox<C2>(collection2[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2> (GeneratedIterators.RRR<C0, C1, C2> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), in Unsafe.Unbox<C2>(collection2[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2> (GeneratedIterators.WWW<C0, C1, C2> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(ref Unsafe.Unbox<C0>(collection0[e]), ref Unsafe.Unbox<C1>(collection1[e]), ref Unsafe.Unbox<C2>(collection2[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3> (GeneratedIterators.RWWW<C0, C1, C2, C3> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent 
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
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), ref Unsafe.Unbox<C1>(collection1[e]), ref Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3> (GeneratedIterators.RRWW<C0, C1, C2, C3> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent 
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
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), ref Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3> (GeneratedIterators.RRRW<C0, C1, C2, C3> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent 
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
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), in Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3> (GeneratedIterators.RRRR<C0, C1, C2, C3> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent 
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
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), in Unsafe.Unbox<C2>(collection2[e]), in Unsafe.Unbox<C3>(collection3[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3> (GeneratedIterators.WWWW<C0, C1, C2, C3> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent 
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
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(ref Unsafe.Unbox<C0>(collection0[e]), ref Unsafe.Unbox<C1>(collection1[e]), ref Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4> (GeneratedIterators.RWWWW<C0, C1, C2, C3, C4> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), ref Unsafe.Unbox<C1>(collection1[e]), ref Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]), ref Unsafe.Unbox<C4>(collection4[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4> (GeneratedIterators.RRWWW<C0, C1, C2, C3, C4> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), ref Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]), ref Unsafe.Unbox<C4>(collection4[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4> (GeneratedIterators.RRRWW<C0, C1, C2, C3, C4> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), in Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]), ref Unsafe.Unbox<C4>(collection4[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4> (GeneratedIterators.RRRRW<C0, C1, C2, C3, C4> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), in Unsafe.Unbox<C2>(collection2[e]), in Unsafe.Unbox<C3>(collection3[e]), ref Unsafe.Unbox<C4>(collection4[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4> (GeneratedIterators.RRRRR<C0, C1, C2, C3, C4> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), in Unsafe.Unbox<C2>(collection2[e]), in Unsafe.Unbox<C3>(collection3[e]), in Unsafe.Unbox<C4>(collection4[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4> (GeneratedIterators.WWWWW<C0, C1, C2, C3, C4> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(ref Unsafe.Unbox<C0>(collection0[e]), ref Unsafe.Unbox<C1>(collection1[e]), ref Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]), ref Unsafe.Unbox<C4>(collection4[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4, C5> (GeneratedIterators.RWWWWW<C0, C1, C2, C3, C4, C5> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            TypeId id5 = TypeManager.GetIdFromType(typeof(C5));
            AddType(withTypes, id5);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
              chunk.TryGetComponents(id5, out ComponentArray collection5);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), ref Unsafe.Unbox<C1>(collection1[e]), ref Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]), ref Unsafe.Unbox<C4>(collection4[e]), ref Unsafe.Unbox<C5>(collection5[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4, C5> (GeneratedIterators.RRWWWW<C0, C1, C2, C3, C4, C5> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            TypeId id5 = TypeManager.GetIdFromType(typeof(C5));
            AddType(withTypes, id5);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
              chunk.TryGetComponents(id5, out ComponentArray collection5);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), ref Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]), ref Unsafe.Unbox<C4>(collection4[e]), ref Unsafe.Unbox<C5>(collection5[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4, C5> (GeneratedIterators.RRRWWW<C0, C1, C2, C3, C4, C5> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            TypeId id5 = TypeManager.GetIdFromType(typeof(C5));
            AddType(withTypes, id5);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
              chunk.TryGetComponents(id5, out ComponentArray collection5);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), in Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]), ref Unsafe.Unbox<C4>(collection4[e]), ref Unsafe.Unbox<C5>(collection5[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4, C5> (GeneratedIterators.RRRRWW<C0, C1, C2, C3, C4, C5> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            TypeId id5 = TypeManager.GetIdFromType(typeof(C5));
            AddType(withTypes, id5);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
              chunk.TryGetComponents(id5, out ComponentArray collection5);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), in Unsafe.Unbox<C2>(collection2[e]), in Unsafe.Unbox<C3>(collection3[e]), ref Unsafe.Unbox<C4>(collection4[e]), ref Unsafe.Unbox<C5>(collection5[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4, C5> (GeneratedIterators.RRRRRW<C0, C1, C2, C3, C4, C5> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            TypeId id5 = TypeManager.GetIdFromType(typeof(C5));
            AddType(withTypes, id5);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
              chunk.TryGetComponents(id5, out ComponentArray collection5);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), in Unsafe.Unbox<C2>(collection2[e]), in Unsafe.Unbox<C3>(collection3[e]), in Unsafe.Unbox<C4>(collection4[e]), ref Unsafe.Unbox<C5>(collection5[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4, C5> (GeneratedIterators.RRRRRR<C0, C1, C2, C3, C4, C5> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            TypeId id5 = TypeManager.GetIdFromType(typeof(C5));
            AddType(withTypes, id5);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
              chunk.TryGetComponents(id5, out ComponentArray collection5);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), in Unsafe.Unbox<C2>(collection2[e]), in Unsafe.Unbox<C3>(collection3[e]), in Unsafe.Unbox<C4>(collection4[e]), in Unsafe.Unbox<C5>(collection5[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4, C5> (GeneratedIterators.WWWWWW<C0, C1, C2, C3, C4, C5> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            TypeId id5 = TypeManager.GetIdFromType(typeof(C5));
            AddType(withTypes, id5);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
              chunk.TryGetComponents(id5, out ComponentArray collection5);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(ref Unsafe.Unbox<C0>(collection0[e]), ref Unsafe.Unbox<C1>(collection1[e]), ref Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]), ref Unsafe.Unbox<C4>(collection4[e]), ref Unsafe.Unbox<C5>(collection5[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4, C5, C6> (GeneratedIterators.RWWWWWW<C0, C1, C2, C3, C4, C5, C6> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            TypeId id5 = TypeManager.GetIdFromType(typeof(C5));
            AddType(withTypes, id5);
            TypeId id6 = TypeManager.GetIdFromType(typeof(C6));
            AddType(withTypes, id6);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
              chunk.TryGetComponents(id5, out ComponentArray collection5);
              chunk.TryGetComponents(id6, out ComponentArray collection6);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), ref Unsafe.Unbox<C1>(collection1[e]), ref Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]), ref Unsafe.Unbox<C4>(collection4[e]), ref Unsafe.Unbox<C5>(collection5[e]), ref Unsafe.Unbox<C6>(collection6[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4, C5, C6> (GeneratedIterators.RRWWWWW<C0, C1, C2, C3, C4, C5, C6> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            TypeId id5 = TypeManager.GetIdFromType(typeof(C5));
            AddType(withTypes, id5);
            TypeId id6 = TypeManager.GetIdFromType(typeof(C6));
            AddType(withTypes, id6);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
              chunk.TryGetComponents(id5, out ComponentArray collection5);
              chunk.TryGetComponents(id6, out ComponentArray collection6);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), ref Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]), ref Unsafe.Unbox<C4>(collection4[e]), ref Unsafe.Unbox<C5>(collection5[e]), ref Unsafe.Unbox<C6>(collection6[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4, C5, C6> (GeneratedIterators.RRRWWWW<C0, C1, C2, C3, C4, C5, C6> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            TypeId id5 = TypeManager.GetIdFromType(typeof(C5));
            AddType(withTypes, id5);
            TypeId id6 = TypeManager.GetIdFromType(typeof(C6));
            AddType(withTypes, id6);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
              chunk.TryGetComponents(id5, out ComponentArray collection5);
              chunk.TryGetComponents(id6, out ComponentArray collection6);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), in Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]), ref Unsafe.Unbox<C4>(collection4[e]), ref Unsafe.Unbox<C5>(collection5[e]), ref Unsafe.Unbox<C6>(collection6[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4, C5, C6> (GeneratedIterators.RRRRWWW<C0, C1, C2, C3, C4, C5, C6> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            TypeId id5 = TypeManager.GetIdFromType(typeof(C5));
            AddType(withTypes, id5);
            TypeId id6 = TypeManager.GetIdFromType(typeof(C6));
            AddType(withTypes, id6);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
              chunk.TryGetComponents(id5, out ComponentArray collection5);
              chunk.TryGetComponents(id6, out ComponentArray collection6);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), in Unsafe.Unbox<C2>(collection2[e]), in Unsafe.Unbox<C3>(collection3[e]), ref Unsafe.Unbox<C4>(collection4[e]), ref Unsafe.Unbox<C5>(collection5[e]), ref Unsafe.Unbox<C6>(collection6[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4, C5, C6> (GeneratedIterators.RRRRRWW<C0, C1, C2, C3, C4, C5, C6> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            TypeId id5 = TypeManager.GetIdFromType(typeof(C5));
            AddType(withTypes, id5);
            TypeId id6 = TypeManager.GetIdFromType(typeof(C6));
            AddType(withTypes, id6);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
              chunk.TryGetComponents(id5, out ComponentArray collection5);
              chunk.TryGetComponents(id6, out ComponentArray collection6);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), in Unsafe.Unbox<C2>(collection2[e]), in Unsafe.Unbox<C3>(collection3[e]), in Unsafe.Unbox<C4>(collection4[e]), ref Unsafe.Unbox<C5>(collection5[e]), ref Unsafe.Unbox<C6>(collection6[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4, C5, C6> (GeneratedIterators.RRRRRRW<C0, C1, C2, C3, C4, C5, C6> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            TypeId id5 = TypeManager.GetIdFromType(typeof(C5));
            AddType(withTypes, id5);
            TypeId id6 = TypeManager.GetIdFromType(typeof(C6));
            AddType(withTypes, id6);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
              chunk.TryGetComponents(id5, out ComponentArray collection5);
              chunk.TryGetComponents(id6, out ComponentArray collection6);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), in Unsafe.Unbox<C2>(collection2[e]), in Unsafe.Unbox<C3>(collection3[e]), in Unsafe.Unbox<C4>(collection4[e]), in Unsafe.Unbox<C5>(collection5[e]), ref Unsafe.Unbox<C6>(collection6[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4, C5, C6> (GeneratedIterators.RRRRRRR<C0, C1, C2, C3, C4, C5, C6> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            TypeId id5 = TypeManager.GetIdFromType(typeof(C5));
            AddType(withTypes, id5);
            TypeId id6 = TypeManager.GetIdFromType(typeof(C6));
            AddType(withTypes, id6);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
              chunk.TryGetComponents(id5, out ComponentArray collection5);
              chunk.TryGetComponents(id6, out ComponentArray collection6);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(in Unsafe.Unbox<C0>(collection0[e]), in Unsafe.Unbox<C1>(collection1[e]), in Unsafe.Unbox<C2>(collection2[e]), in Unsafe.Unbox<C3>(collection3[e]), in Unsafe.Unbox<C4>(collection4[e]), in Unsafe.Unbox<C5>(collection5[e]), in Unsafe.Unbox<C6>(collection6[e]));
                }
            });
            
            return this;
            }
          public IIteratorPhase2 Iterate<C0, C1, C2, C3, C4, C5, C6> (GeneratedIterators.WWWWWWW<C0, C1, C2, C3, C4, C5, C6> iterator) where C0 : struct, IComponent where C1 : struct, IComponent where C2 : struct, IComponent where C3 : struct, IComponent where C4 : struct, IComponent where C5 : struct, IComponent where C6 : struct, IComponent 
            {
            TypeId id0 = TypeManager.GetIdFromType(typeof(C0));
            AddType(withTypes, id0);
            TypeId id1 = TypeManager.GetIdFromType(typeof(C1));
            AddType(withTypes, id1);
            TypeId id2 = TypeManager.GetIdFromType(typeof(C2));
            AddType(withTypes, id2);
            TypeId id3 = TypeManager.GetIdFromType(typeof(C3));
            AddType(withTypes, id3);
            TypeId id4 = TypeManager.GetIdFromType(typeof(C4));
            AddType(withTypes, id4);
            TypeId id5 = TypeManager.GetIdFromType(typeof(C5));
            AddType(withTypes, id5);
            TypeId id6 = TypeManager.GetIdFromType(typeof(C6));
            AddType(withTypes, id6);
            BakeSettings();
            SetDelegate((chunk) =>
            {
              chunk.TryGetComponents(id0, out ComponentArray collection0);
              chunk.TryGetComponents(id1, out ComponentArray collection1);
              chunk.TryGetComponents(id2, out ComponentArray collection2);
              chunk.TryGetComponents(id3, out ComponentArray collection3);
              chunk.TryGetComponents(id4, out ComponentArray collection4);
              chunk.TryGetComponents(id5, out ComponentArray collection5);
              chunk.TryGetComponents(id6, out ComponentArray collection6);
                for (int e = 0; e < chunk.Count; e++)
                {
                    iterator(ref Unsafe.Unbox<C0>(collection0[e]), ref Unsafe.Unbox<C1>(collection1[e]), ref Unsafe.Unbox<C2>(collection2[e]), ref Unsafe.Unbox<C3>(collection3[e]), ref Unsafe.Unbox<C4>(collection4[e]), ref Unsafe.Unbox<C5>(collection5[e]), ref Unsafe.Unbox<C6>(collection6[e]));
                }
            });
            
            return this;
            }
    }
}