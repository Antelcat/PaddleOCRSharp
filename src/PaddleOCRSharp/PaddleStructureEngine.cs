// Copyright (c) 2021 raoyutian Authors. All Rights Reserved.
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

using System.Runtime.InteropServices;
using System;
using System.Drawing;
using System.IO;
using PaddleOCRSharp.Extensions;

namespace PaddleOCRSharp;

/// <summary>
/// PaddleOCR NET帮助类
/// </summary>
public class PaddleStructureEngine : EngineBase
{
    #region PaddleOCR API

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    internal static extern bool StructureInitialize(string detInfer, string recInfer, string Keys,
        string TableModelDir, string TableCharDictPath, StructureParameter parameter);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    internal static extern bool StructureInitializejson(string DetInfer, string RecInfer, string Keys,
        string TableModelDir, string TableCharDictPath, string parameter);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    internal static extern IntPtr GetStructureDetectFile(string imagefile);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    internal static extern IntPtr GetStructureDetectByte(byte[] imagebytedata, long size);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    internal static extern IntPtr GetStructureDetectBase64(string imagebase64);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    internal static extern void FreeStructureEngine();

    #endregion

    /// <summary>
    /// PaddleStructureEngine识别引擎对象初始化
    /// </summary>
    public PaddleStructureEngine() : this(null, new StructureParameter())
    {
    }

    /// <summary>
    /// PaddleStructureEngine识别引擎对象初始化
    /// </summary>
    /// <param name="config">模型配置对象，如果为空则按默认值</param>
    public PaddleStructureEngine(StructureModelConfig config) : this(config, new StructureParameter())
    {
    }

    /// <summary>
    /// PaddleStructureEngine识别引擎对象初始化
    /// </summary>
    /// <param name="config">模型配置对象，如果为空则按默认值</param>
    /// <param name="parameter">识别参数，为空均按缺省值</param>
    public PaddleStructureEngine(StructureModelConfig? config, StructureParameter? parameter)
    {
        parameter ??= new StructureParameter();
        config    ??= ConfigureExtension.StructureModelConfigDefault;
        if (!StructureInitialize(config.DetInfer, 
                config.RecInfer, 
                config.Keys, 
                config.TableModelDir,
                config.TableCharDictPath, parameter)) 
            throw new Exception("Initialize err:" + GetLastError());
    }

    /// <summary>
    /// PaddleStructureEngine识别引擎对象初始化
    /// </summary>
    /// <param name="config">模型配置对象，如果为空则按默认值</param>
    /// <param name="paramJson">识别参数Json格式，为空均按缺省值</param>
    public PaddleStructureEngine(StructureModelConfig? config, string? paramJson) 
    {
        config ??= ConfigureExtension.StructureModelConfigDefault;

        if (string.IsNullOrEmpty(paramJson))
        {
            paramJson = NativeExtension.BaseDirectory + @"\inference\PaddleOCRStructure.config.json";
            if (!File.Exists(paramJson)) throw new FileNotFoundException(paramJson);
            paramJson = File.ReadAllText(paramJson);
        }

        if (!StructureInitializejson(config.DetInfer,
                config.RecInfer,
                config.Keys,
                config.TableModelDir,
                config.TableCharDictPath, paramJson!))
            throw new Exception("Initialize err:" + GetLastError());
    }

    /// <summary>
    /// 对图像文件进行表格文本识别
    /// </summary>
    /// <param name="image">图像文件</param>
    /// <returns>表格识别结果</returns>
    public string? StructureDetectFile(string image)
    {
        if (!File.Exists(image)) throw new FileNotFoundException(image);
        var ptr    = GetStructureDetectFile(image);
        var result = Marshal.PtrToStringUni(ptr);
        Marshal.FreeHGlobal(ptr);
        return result;
    }


    /// <summary>
    ///对图像对象进行表格文本识别
    /// </summary>
    /// <param name="image">图像</param>
    /// <returns>表格识别结果</returns>
    public string? StructureDetect(Image image)
    {
        if (image == null) throw new ArgumentNullException(nameof(image));
        var imageBytes = ImageToBytes(image);
        var result     = StructureDetect(imageBytes);
        return result;
    }

    /// <summary>
    /// 对图像Byte数组进行表格文本识别
    /// </summary>
    /// <param name="image">图像字节数组</param>
    /// <returns>表格识别结果</returns>
    public static string? StructureDetect(byte[] image)
    {
        if (image == null) throw new ArgumentNullException(nameof(image));
        var ptr    = GetStructureDetectByte(image, image.LongLength);
        var result = Marshal.PtrToStringUni(ptr);
        Marshal.FreeHGlobal(ptr);
        return result;
    }

    /// <summary>
    /// 对图像Base64进行表格文本识别
    /// </summary>
    /// <param name="base64">图像Base64</param>
    /// <returns>表格识别结果</returns>
    public string? StructureDetectBase64(string base64)
    {
        if (string.IsNullOrEmpty(base64)) throw new ArgumentNullException(nameof(base64));
        var ptr    = GetStructureDetectBase64(base64);
        var result = Marshal.PtrToStringUni(ptr);
        Marshal.FreeHGlobal(ptr);
        return result;
    }

    #region Dispose

    /// <summary>
    /// 释放对象
    /// </summary>
    public override void Dispose() => FreeStructureEngine();

    #endregion
}