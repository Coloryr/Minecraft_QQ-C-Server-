using System;
using System.Runtime.InteropServices;

namespace Native.Csharp.Sdk.Cqp.Core
{
    internal class Kernel32
    {
        [DllImport("kernel32.dll", EntryPoint = "lstrlenA", CharSet = CharSet.Ansi)]
        public extern static int LstrlenA(IntPtr ptr);
    }
}
