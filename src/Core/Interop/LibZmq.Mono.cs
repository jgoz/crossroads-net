#if MONO

namespace CrossroadsIO.Interop
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    internal static class LibXs
    {
        public const string LibraryName = "libxs";

        // From xs.h:
        // typedef struct {unsigned char _ [32];} xs_msg_t;
        public const int XsMsgTSize = 32;

        public static readonly int MajorVersion;
        public static readonly int MinorVersion;
        public static readonly int PatchVersion;

        static LibXs()
        {
            AssignCurrentVersion(out MajorVersion, out MinorVersion, out PatchVersion);
        }

        private static void AssignCurrentVersion(out int majorVersion, out int minorVersion, out int patchVersion)
        {
            int sizeofInt32 = Marshal.SizeOf(typeof(int));

            IntPtr majorPointer = Marshal.AllocHGlobal(sizeofInt32);
            IntPtr minorPointer = Marshal.AllocHGlobal(sizeofInt32);
            IntPtr patchPointer = Marshal.AllocHGlobal(sizeofInt32);

            xs_version(majorPointer, minorPointer, patchPointer);

            majorVersion = Marshal.ReadInt32(majorPointer);
            minorVersion = Marshal.ReadInt32(minorPointer);
            patchVersion = Marshal.ReadInt32(patchPointer);

            Marshal.FreeHGlobal(majorPointer);
            Marshal.FreeHGlobal(minorPointer);
            Marshal.FreeHGlobal(patchPointer);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FreeMessageDataCallback(IntPtr data, IntPtr hint);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr xs_init();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_term(IntPtr context);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_close(IntPtr socket);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_setctxopt(IntPtr ctx, int option, IntPtr optval, int optvallen);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_setsockopt(IntPtr socket, int option, IntPtr optval, int optvallen);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_getsockopt(IntPtr socket, int option, IntPtr optval, IntPtr optvallen);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_getmsgopt(IntPtr message, int option, IntPtr optval, IntPtr optvallen);

        [DllImport(LibraryName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_bind(IntPtr socket, string addr);

        [DllImport(LibraryName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_connect(IntPtr socket, string addr);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_recvmsg(IntPtr socket, IntPtr msg, int flags);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_sendmsg(IntPtr socket, IntPtr msg, int flags);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr xs_socket(IntPtr context, int type);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_msg_close(IntPtr msg);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_msg_copy(IntPtr destmsg, IntPtr srcmsg);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr xs_msg_data(IntPtr msg);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_msg_init(IntPtr msg);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_msg_init_data(IntPtr msg, IntPtr data, int size, FreeMessageDataCallback ffn, IntPtr hint);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_msg_init_size(IntPtr msg, int size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_msg_size(IntPtr msg);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_msg_move(IntPtr destmsg, IntPtr srcmsg);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_errno();

        [DllImport(LibraryName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr xs_strerror(int errnum);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void xs_version(IntPtr major, IntPtr minor, IntPtr patch);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int xs_poll([In, Out] PollItem[] items, int numItems, int timeout);
    }
}

#endif