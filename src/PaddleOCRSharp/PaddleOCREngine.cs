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

using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System;
using System.Linq;
using System.IO;
using PaddleOCRSharp.Extensions;

namespace PaddleOCRSharp;

/// <summary>
/// PaddleOCR识别引擎对象
/// </summary>
public class PaddleOCREngine : EngineBase
{
    #region PaddleOCR API

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern bool Initialize(string detInfer, string clsInfer, string recInfer, string keys,
        OCRParameter parameter);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern bool Initializejson(string detInfer, string clsInfer, string recInfer, string keys,
        string parameterJson);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern IntPtr Detect(string imageFile);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern IntPtr DetectByte(byte[] imageByteData, long size);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern IntPtr DetectBase64(string base64);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern int FreeEngine();

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern bool libModifyParameter(ModifyParameter parameter);

    #endregion

    #region 文本识别

    /// <summary>
    /// 使用默认参数初始化OCR引擎对象
    /// </summary>
    public PaddleOCREngine() : this(null, new OCRParameter())
    {
    }

    /// <summary>
    /// 使用默认参数初始化OCR引擎对象
    /// </summary>
    /// <param name="config">模型配置对象，如果为空则按默认值</param>
    public PaddleOCREngine(OCRModelConfig config) : this(config, new OCRParameter())
    {
    }

    /// <summary>
    /// PaddleOCR识别引擎对象初始化
    /// </summary>
    /// <param name="config">模型配置对象，如果为空则按默认值</param>
    /// <param name="parameter">识别参数，为空均按缺省值</param>
    public PaddleOCREngine(OCRModelConfig? config, OCRParameter? parameter)
    {
        parameter ??= new OCRParameter();
        config    ??= ConfigureExtension.OCRModelConfigDefault;

        if (!Directory.Exists(config.DetInfer) && parameter.det)
            throw new DirectoryNotFoundException(config.DetInfer);
        if (!Directory.Exists(config.ClsInfer) && parameter.cls)
            throw new DirectoryNotFoundException(config.ClsInfer);
        if (!Directory.Exists(config.RecInfer)) throw new DirectoryNotFoundException(config.RecInfer);
        if (!File.Exists(config.Keys)) throw new FileNotFoundException(config.Keys);
        if (!Initialize(config.DetInfer, config.ClsInfer, config.RecInfer, config.Keys, parameter))
            throw new Exception("Initialize err:" + GetLastError());
    }

    /// <summary>
    /// PaddleOCR识别引擎对象初始化
    /// </summary>
    /// <param name="config">模型配置对象，如果为空则按默认值</param>
    /// <param name="jsonParam">识别参数json字符串</param>
    public PaddleOCREngine(OCRModelConfig? config, string? jsonParam)
    {
        config ??= ConfigureExtension.OCRModelConfigDefault;

        if (string.IsNullOrEmpty(jsonParam))
        {
            jsonParam =  NativeExtension.BaseDirectory.TrimEnd('\\');
            jsonParam += @"\inference\PaddleOCR.config.json";
            if (!File.Exists(jsonParam)) throw new FileNotFoundException(jsonParam);
            jsonParam = File.ReadAllText(jsonParam);
        }

        var parameter = jsonParam!.DeserializeObject<OCRParameter>();

        if (!Directory.Exists(config.DetInfer) && parameter.det)
            throw new DirectoryNotFoundException(config.DetInfer);
        if (!Directory.Exists(config.ClsInfer) && parameter.cls)
            throw new DirectoryNotFoundException(config.ClsInfer);
        if (!Directory.Exists(config.RecInfer)) throw new DirectoryNotFoundException(config.RecInfer);
        if (!File.Exists(config.Keys)) throw new FileNotFoundException(config.Keys);
        if (!Initializejson(config.DetInfer, config.ClsInfer, config.RecInfer, config.Keys, jsonParam!))
            throw new Exception("Initialize err:" + GetLastError());
    }

    /// <summary>
    /// 对图像文件进行文本识别
    /// </summary>
    /// <param name="image">图像文件</param>
    /// <returns>OCR识别结果</returns>
    public OCRResult DetectText(string image) =>
        !File.Exists(image) 
            ? throw new FileNotFoundException($"{image}") 
            : ConvertResult(Detect(image));


    /// <summary>
    ///对图像对象进行文本识别
    /// </summary>
    /// <param name="image">图像</param>
    /// <returns>OCR识别结果</returns>
    public OCRResult DetectText(Image image) => image == null
        ? throw new ArgumentNullException(nameof(image))
        : DetectText(ImageToBytes(image));

    /// <summary>
    ///文本识别
    /// </summary>
    /// <param name="image">图像内存流</param>
    /// <returns>OCR识别结果</returns>
    public OCRResult DetectText(byte[] image) =>
        image == null
            ? throw new ArgumentNullException(nameof(image))
            : ConvertResult(DetectByte(image, image.LongLength));

    /// <summary>
    ///文本识别
    /// </summary>
    /// <param name="base64">图像base64</param>
    /// <returns>OCR识别结果</returns>
    public OCRResult DetectTextBase64(string base64) =>
        string.IsNullOrEmpty(base64)
            ? throw new ArgumentNullException(nameof(base64))
            : ConvertResult(DetectBase64(base64));

    /// <summary>
    /// 结果解析
    /// </summary>
    /// <param name="ptrResult"></param>
    /// <returns></returns>
    private OCRResult ConvertResult(IntPtr ptrResult)
    {
        var result = new OCRResult();
        if (ptrResult == IntPtr.Zero)
        {
            var err = GetLastError();
            if (!string.IsNullOrEmpty(err))
            {
                throw new Exception("内部遇到错误：" + err);
            }

            return result;
        }

        try
        {
            var json       = Marshal.PtrToStringUni(ptrResult);
            var textBlocks = json!.DeserializeObject<List<TextBlock>>();
            result.JsonText   = json!;
            result.TextBlocks = textBlocks;
            Marshal.FreeHGlobal(ptrResult);
        }
        catch (Exception ex)
        {
            throw new Exception("OCR结果Json反序列化失败。", ex);
        }

        return result;
    }

    #endregion

    #region 表格识别

    /// <summary>
    ///结构化文本识别
    /// </summary>
    /// <param name="image">图像</param>
    /// <returns>表格识别结果</returns>
    public OCRStructureResult DetectStructure(Image image)
    {
        if (image == null) throw new ArgumentNullException(nameof(image));
        var imageBytes = ImageToBytes(image);
        var result     = DetectText(imageBytes);
        var blocks     = result.TextBlocks;
        if (blocks.Count == 0) return new ();
            
        var orderedXBox = blocks.OrderBy(static x => x.BoxPoints[0].X).ToList();
        var orderedYBox = blocks.OrderBy(static x => x.BoxPoints[0].Y).ToList();
            
        var yList = GetZeroIndex(orderedYBox
                .Select(static x => x.BoxPoints[0].Y)
                .ToArray(),
            10);
        var xList = GetZeroIndex(orderedXBox
                .Select(static x => x.BoxPoints[0].X)
                .ToArray(),
            10);
            
        var rowcount = yList.Count;
        var colcount = xList.Count;
        var structureResult = new OCRStructureResult
        {
            TextBlocks = blocks,
            RowCount   = rowcount,
            ColCount   = colcount,
        };
        for (var i = 0; i < rowcount; i++)
        {
            var yMin                   = orderedYBox[yList[i]].BoxPoints[0].Y;
            var yMax                   = 99999;
            if (i < rowcount - 1) yMax = orderedYBox[yList[i + 1]].BoxPoints[0].Y;

            for (var j = 0; j < colcount; j++)
            {
                var xMin                   = orderedXBox[xList[j]].BoxPoints[0].X;
                var xMax                   = 99999;
                if (j < colcount - 1) xMax = orderedXBox[xList[j + 1]].BoxPoints[0].X;

                var textBlocks = blocks
                    .Where(x => x.BoxPoints[0].X    < xMax
                                && x.BoxPoints[0].X >= xMin
                                && x.BoxPoints[0].Y < yMax
                                && x.BoxPoints[0].Y >= yMin)
                    .OrderBy(static u => u.BoxPoints[0].X)
                    .ToList();
                var texts = textBlocks.Select(static x => x.Text).ToArray();

                var cell = new StructureCells
                {
                    Row = i,
                    Col = j
                };

#if NET35
                cell.Text = string.Join(string.Empty, texts);
#else
                    cell.Text = string.Join<string>(string.Empty, texts);
#endif


                cell.TextBlocks = textBlocks.ToList();
                structureResult.Cells.Add(cell);
            }
        }

        return structureResult;
    }

    /// <summary>
    /// 计算表格分割
    /// </summary>
    /// <param name="pixels"></param>
    /// <param name="threshold2Zero"></param>
    /// <returns></returns>
    private static List<int> GetZeroIndex(int[] pixels, int threshold2Zero = 10)
    {
        var zeroList = new List<int> { 0 };
        for (var i = 0; i < pixels.Length; i++)
        {
            if (i < pixels.Length - 1 && Math.Abs(pixels[i + 1] - pixels[i]) > threshold2Zero)
            {
                //突增点
                zeroList.Add(i + 1);
            }
        }

        return zeroList;
    }

    #endregion

    /// <summary>
    /// 在初始化后动态修改参数
    /// </summary>
    /// <param name="parameter">可修改参数对象</param>
    /// <returns>是否成功，在初始化前调用会导致失败</returns>
    public bool ModifyParameter(ModifyParameter parameter) => libModifyParameter(parameter);

    #region Dispose

    /// <summary>
    /// 释放对象
    /// </summary>
    public override void Dispose() => FreeEngine();

    #endregion
}