namespace Entygine.Ecs
{
    public partial class EntityIterator
    {
      public delegate void RWWWWWWW<C0, C1, C2, C3, C4, C5, C6, C7>(in C0 read0, ref C1 write1, ref C2 write2, ref C3 write3, ref C4 write4, ref C5 write5, ref C6 write6, ref C7 write7); 
      public delegate void RRWWWWWW<C0, C1, C2, C3, C4, C5, C6, C7>(in C0 read0, in C1 read1, ref C2 write2, ref C3 write3, ref C4 write4, ref C5 write5, ref C6 write6, ref C7 write7); 
      public delegate void RRRWWWWW<C0, C1, C2, C3, C4, C5, C6, C7>(in C0 read0, in C1 read1, in C2 read2, ref C3 write3, ref C4 write4, ref C5 write5, ref C6 write6, ref C7 write7); 
      public delegate void RRRRWWWW<C0, C1, C2, C3, C4, C5, C6, C7>(in C0 read0, in C1 read1, in C2 read2, in C3 read3, ref C4 write4, ref C5 write5, ref C6 write6, ref C7 write7); 
      public delegate void RRRRRWWW<C0, C1, C2, C3, C4, C5, C6, C7>(in C0 read0, in C1 read1, in C2 read2, in C3 read3, in C4 read4, ref C5 write5, ref C6 write6, ref C7 write7); 
      public delegate void RRRRRRWW<C0, C1, C2, C3, C4, C5, C6, C7>(in C0 read0, in C1 read1, in C2 read2, in C3 read3, in C4 read4, in C5 read5, ref C6 write6, ref C7 write7); 
      public delegate void RRRRRRRW<C0, C1, C2, C3, C4, C5, C6, C7>(in C0 read0, in C1 read1, in C2 read2, in C3 read3, in C4 read4, in C5 read5, in C6 read6, ref C7 write7); 
      public delegate void RRRRRRRR<C0, C1, C2, C3, C4, C5, C6, C7>(in C0 read0, in C1 read1, in C2 read2, in C3 read3, in C4 read4, in C5 read5, in C6 read6, in C7 read7); 
      public delegate void WWWWWWWW<C0, C1, C2, C3, C4, C5, C6, C7>(ref C0 write0, ref C1 write1, ref C2 write2, ref C3 write3, ref C4 write4, ref C5 write5, ref C6 write6, ref C7 write7); 
    }
}