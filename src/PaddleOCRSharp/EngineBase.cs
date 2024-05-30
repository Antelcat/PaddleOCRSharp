// Copyright (c) 2021 raoyutian  All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
 
using System.Drawing;
using System.Runtime.InteropServices;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using PaddleOCRSharp.Extensions;

namespace PaddleOCRSharp;

/// <summary>
/// PaddleOCR识别引擎对象
/// </summary>
public abstract class EngineBase
{
    /// <summary>
    /// PaddleOCR.dll自定义加载路径，默认为空，如果指定则需在引擎实例化前赋值。
    /// </summary>
    public static string? PaddleOCRDllPath { get; set; }

    internal const string DllName = "PaddleOCR.dll";

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern IntPtr GetError();

    /// <summary>
    /// 初始化
    /// </summary>
    protected EngineBase()
    {

        if (string.IsNullOrEmpty(PaddleOCRDllPath))
            PaddleOCRDllPath = Path.Combine(NativeExtension.BaseDirectory, @"runtimes\win-x64\native");
        if (new DirectoryInfo(PaddleOCRDllPath).GetFiles("*.dll").Any(dll => !NativeExtension.Load(dll.FullName)))
        {
            throw new Exception();
        }
    }

    #region private



    /// <summary>
    /// Convert Image to Byte[]
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    protected byte[] ImageToBytes(Image image)
    {
        var       format = image.RawFormat;
        using var ms     = new MemoryStream();
        if (format.Guid      == ImageFormat.Jpeg.Guid) image.Save(ms, ImageFormat.Jpeg);
        else if (format.Guid == ImageFormat.Png.Guid) image.Save(ms, ImageFormat.Png);
        else if (format.Guid == ImageFormat.Bmp.Guid) image.Save(ms, ImageFormat.Bmp);
        else if (format.Guid == ImageFormat.Gif.Guid) image.Save(ms, ImageFormat.Gif);
        else if (format.Guid == ImageFormat.Icon.Guid) image.Save(ms, ImageFormat.Icon);
        else image.Save(ms, ImageFormat.Png);
        var buffer = new byte[ms.Length];
        ms.Seek(0, SeekOrigin.Begin);
        _ = ms.Read(buffer, 0, buffer.Length);
        return buffer;
    }

    #endregion
    /// <summary>
    /// 释放内存
    /// </summary>
    public virtual void Dispose()
    {
    }
        
        
    /// <summary>
    /// 获取底层错误信息
    /// </summary>
    /// <returns></returns>
    protected static string? GetLastError()
    {
        var err = string.Empty;
        try
        {
            var ptr = GetError();
            if (ptr != IntPtr.Zero)
            {
                err = Marshal.PtrToStringAnsi(ptr);
                Marshal.FreeHGlobal(ptr);
            }
        }
        catch (Exception e)
        {
            err = e.Message;
        }
        return err;
    }

}