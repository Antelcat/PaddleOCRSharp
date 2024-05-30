using System;
using System.Runtime.InteropServices;

namespace PaddleOCRSharp.Extensions;

internal static class NativeExtension
{
#if  !NET
    public static bool Load(string path) => !string.IsNullOrEmpty(path) && LoadLibrary(path) != IntPtr.Zero;

    [DllImport("kernel32.dll")]
    private static extern IntPtr LoadLibrary(string path);
#else
    public static bool Load(string path) => !string.IsNullOrEmpty(path) && NativeLibrary.TryLoad(path, out var _);

#endif
    
    /// <summary>
    /// 获取程序的当前路径;
    /// </summary>
    /// <returns></returns>
    internal static string BaseDirectory =>
#if !NETFRAMEWORK
         AppContext.BaseDirectory;
#else
         Environment.CurrentDirectory;
#endif

    /// <summary>
    /// 环境监测
    /// </summary>
    internal static void ThrowIfNotSupportEnv()
    {
#if !NETFRAMEWORK
        if (!Environment.Is64BitProcess) throw new NotSupportedException("Not support 32bit process.");
#endif
    }
}