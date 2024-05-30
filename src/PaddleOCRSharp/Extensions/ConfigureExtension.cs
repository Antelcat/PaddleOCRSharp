using System.IO;

namespace PaddleOCRSharp.Extensions;

internal static class ConfigureExtension
{
    internal static StructureModelConfig StructureModelConfigDefault
    {
        get
        {
            var root = Path.Combine(NativeExtension.BaseDirectory, @"runtimes\win-x64\native\inference");
            return new StructureModelConfig
            {
                DetInfer          = Path.Combine(root, "ch_PP-OCRv4_det_infer"),
                RecInfer          = Path.Combine(root, "ch_PP-OCRv4_rec_infer"),
                Keys              = Path.Combine(root, "ppocr_keys.txt"),
                TableModelDir     = Path.Combine(root, "ch_ppstructure_mobile_v2.0_SLANet_infer"),
                TableCharDictPath = Path.Combine(root, "table_structure_dict_ch.txt"),
            };
        }
    }

    internal static OCRModelConfig OCRModelConfigDefault
    {
        get
        {
            var root = Path.Combine(NativeExtension.BaseDirectory, @"runtimes\win-x64\native\inference");
            return new OCRModelConfig
            {
                DetInfer = Path.Combine(root, "ch_PP-OCRv4_det_infer"),
                ClsInfer = Path.Combine(root, "ch_ppocr_mobile_v2.0_cls_infer"),
                RecInfer = Path.Combine(root, "ch_PP-OCRv4_rec_infer"),
                Keys     = Path.Combine(root, "ppocr_keys.txt")
            };
        }
    }
}