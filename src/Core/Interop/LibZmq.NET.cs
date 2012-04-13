#if DOTNET

namespace CrossroadsIO.Interop
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Compatibility with native headers.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Compatibility with native headers.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Compatibility with native headers.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Compatibility with native headers.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Reviewed. Suppression is OK here.")]
    internal static class LibXs
    {
        public const string LibraryName = "libxs";

        // From xs.h:
        // typedef struct {unsigned char _ [32];} xs_msg_t;
        public const int XsMsgTSize = 32;

        public static readonly int MajorVersion;
        public static readonly int MinorVersion;
        public static readonly int PatchVersion;

        private static readonly UnmanagedLibrary NativeLib;

        static LibXs()
        {
            NativeLib = new UnmanagedLibrary(LibraryName);

            AssignDelegates();
            AssignCurrentVersion(out MajorVersion, out MinorVersion, out PatchVersion);

        }

        private static void AssignDelegates()
        {
            xs_init = NativeLib.GetUnmanagedFunction<XsInitProc>("xs_init");
            xs_term = NativeLib.GetUnmanagedFunction<XsTermProc>("xs_term");
            xs_close = NativeLib.GetUnmanagedFunction<XsCloseProc>("xs_close");
            xs_setsockopt = NativeLib.GetUnmanagedFunction<XsSetSockOptProc>("xs_setsockopt");
            xs_getsockopt = NativeLib.GetUnmanagedFunction<XsGetSockOptProc>("xs_getsockopt");
            xs_bind = NativeLib.GetUnmanagedFunction<XsBindProc>("xs_bind");
            xs_connect = NativeLib.GetUnmanagedFunction<XsConnectProc>("xs_connect");
            xs_socket = NativeLib.GetUnmanagedFunction<XsSocketProc>("xs_socket");
            xs_getmsgopt = NativeLib.GetUnmanagedFunction<XsGetMsgOptProc>("xs_getmsgopt");
            xs_msg_close = NativeLib.GetUnmanagedFunction<XsMsgCloseProc>("xs_msg_close");
            xs_msg_copy = NativeLib.GetUnmanagedFunction<XsMsgCopyProc>("xs_msg_copy");
            xs_msg_data = NativeLib.GetUnmanagedFunction<XsMsgDataProc>("xs_msg_data");
            xs_msg_init = NativeLib.GetUnmanagedFunction<XsMsgInitProc>("xs_msg_init");
            xs_msg_init_size = NativeLib.GetUnmanagedFunction<XsMsgInitSizeProc>("xs_msg_init_size");
            xs_msg_size = NativeLib.GetUnmanagedFunction<XsMsgSizeProc>("xs_msg_size");
            xs_msg_init_data = NativeLib.GetUnmanagedFunction<XsMsgInitDataProc>("xs_msg_init_data");
            xs_msg_move = NativeLib.GetUnmanagedFunction<XsMsgMoveProc>("xs_msg_move");
            xs_errno = NativeLib.GetUnmanagedFunction<XsErrnoProc>("xs_errno");
            xs_strerror = NativeLib.GetUnmanagedFunction<XsStrErrorProc>("xs_strerror");
            xs_version = NativeLib.GetUnmanagedFunction<XsVersionProc>("xs_version");
            xs_poll = NativeLib.GetUnmanagedFunction<XsPollProc>("xs_poll");
            xs_recvmsg = NativeLib.GetUnmanagedFunction<XsRecvMsgProc>("xs_recvmsg");
            xs_sendmsg = NativeLib.GetUnmanagedFunction<XsSendMsgProc>("xs_sendmsg");
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

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr XsInitProc();
        public static XsInitProc xs_init;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsTermProc(IntPtr context);
        public static XsTermProc xs_term;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsSetCtxOptProc(IntPtr ctx, int option, IntPtr optval, int optvallen);
        public static XsSetCtxOptProc xs_setctxopt;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsSetSockOptProc(IntPtr socket, int option, IntPtr optval, int optvallen);
        public static XsSetSockOptProc xs_setsockopt;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsGetSockOptProc(IntPtr socket, int option, IntPtr optval, IntPtr optvallen);
        public static XsGetSockOptProc xs_getsockopt;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsGetMsgOptProc(IntPtr message, int option, IntPtr optval, IntPtr optvallen);
        public static XsGetMsgOptProc xs_getmsgopt;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int XsBindProc(IntPtr socket, string addr);
        public static XsBindProc xs_bind;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int XsConnectProc(IntPtr socket, string addr);
        public static XsConnectProc xs_connect;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsCloseProc(IntPtr socket);
        public static XsCloseProc xs_close;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsRecvMsgProc(IntPtr socket, IntPtr msg, int flags);
        public static XsRecvMsgProc xs_recvmsg;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsSendMsgProc(IntPtr socket, IntPtr msg, int flags);
        public static XsSendMsgProc xs_sendmsg;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr XsSocketProc(IntPtr context, int type);
        public static XsSocketProc xs_socket;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsMsgCloseProc(IntPtr msg);
        public static XsMsgCloseProc xs_msg_close;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsMsgCopyProc(IntPtr destmsg, IntPtr srcmsg);
        public static XsMsgCopyProc xs_msg_copy;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr XsMsgDataProc(IntPtr msg);
        public static XsMsgDataProc xs_msg_data;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsMsgInitProc(IntPtr msg);
        public static XsMsgInitProc xs_msg_init;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsMsgInitSizeProc(IntPtr msg, int size);
        public static XsMsgInitSizeProc xs_msg_init_size;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsMsgInitDataProc(IntPtr msg, IntPtr data, int size, FreeMessageDataCallback ffn, IntPtr hint);
        public static XsMsgInitDataProc xs_msg_init_data;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsMsgMoveProc(IntPtr destmsg, IntPtr srcmsg);
        public static XsMsgMoveProc xs_msg_move;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsMsgSizeProc(IntPtr msg);
        public static XsMsgSizeProc xs_msg_size;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsErrnoProc();
        public static XsErrnoProc xs_errno;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate IntPtr XsStrErrorProc(int errnum);
        public static XsStrErrorProc xs_strerror;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void XsVersionProc(IntPtr major, IntPtr minor, IntPtr patch);
        public static XsVersionProc xs_version;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int XsPollProc([In] [Out] PollItem[] items, int numItems, long timeoutMsec);
        public static XsPollProc xs_poll;
    }
    // ReSharper restore InconsistentNaming
}

#endif