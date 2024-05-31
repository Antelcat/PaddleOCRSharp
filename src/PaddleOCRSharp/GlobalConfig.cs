using PaddleOCRSharp.Extensions;

namespace PaddleOCRSharp;

/// <summary>
/// 全局配置
/// </summary>
public static class GlobalConfig
{
    /// <summary>
    /// 基路径
    /// </summary>
    public static string BaseDirectory
    {
        get => NativeExtension.BaseDirectory;
        set => NativeExtension.BaseDirectory = value;
    }
}