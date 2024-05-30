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

using PaddleOCRSharp.Extensions;

namespace PaddleOCRSharp;

/// <summary>
/// Shared configs
/// </summary>
public abstract class SharedConfig
{
    /// <summary>
    /// det_infer model directory
    /// </summary>
    public string DetInfer { get; set; }

    /// <summary>
    /// rec_infer model directory
    /// </summary>
    public string RecInfer { get; set; }

    /// <summary>
    /// ppocr_keys file path
    /// </summary>
    public string Keys { get; set; }
}

/// <summary>
/// 模型配置对象
/// </summary>
public class OCRModelConfig : SharedConfig
{
    
    /// <summary>
    /// cls_infer model directory
    /// </summary>
    public string ClsInfer { get; set; }
}



/// <summary>
/// 表格模型配置对象
/// </summary>
public class StructureModelConfig : SharedConfig
{
    /// <summary>
    /// table_model_dir模型路径
    /// </summary>
    public string TableModelDir { get; set; }

    /// <summary>
    /// 表格识别字典
    /// </summary>
    public string TableCharDictPath { get; set; }
}