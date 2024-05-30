using System.IO;
using PaddleOCRSharp.Extensions;

namespace PaddleOCRSharp;

/// <summary>
/// Known models
/// </summary>
public static class KnownModels
{
    private static string PredefinedModelDir => Path.Combine(NativeExtension.BaseDirectory, @"models\PaddleOCR");

    /// <summary>
    /// Default OCR Chinese V4
    /// </summary>
    public static OCRModelConfig ChineseV4 => new()
    {
        DetInfer = Path.Combine(PredefinedModelDir, @"ChineseV4\ch_PP-OCRv4_det_infer"),
        RecInfer = Path.Combine(PredefinedModelDir, @"ChineseV4\ch_PP-OCRv4_rec_infer"),
        Keys     = Path.Combine(PredefinedModelDir, @"ChineseV4\ppocr_keys.txt"),
        ClsInfer = Path.Combine(PredefinedModelDir, @"ChineseV4\ch_ppocr_mobile_v2.0_cls_infer"),
    };
    
    /// <summary>
    /// 
    /// </summary>
    public static StructureModelConfig ChineseV4Param => new()
    {
        DetInfer          = Path.Combine(PredefinedModelDir, @"ChineseV4\ch_PP-OCRv4_det_infer"),
        RecInfer          = Path.Combine(PredefinedModelDir, @"ChineseV4\ch_PP-OCRv4_rec_infer"),
        Keys              = Path.Combine(PredefinedModelDir, @"ChineseV4\ppocr_keys.txt"),
        TableModelDir     = Path.Combine(PredefinedModelDir, @"ChineseV4\ch_ppstructure_mobile_v2.0_SLANet_infer"),
        TableCharDictPath = Path.Combine(PredefinedModelDir, @"ChineseV4\table_structure_dict_ch.txt"),
    };
}