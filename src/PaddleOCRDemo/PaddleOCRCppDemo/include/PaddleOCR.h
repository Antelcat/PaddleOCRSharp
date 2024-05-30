#pragma once
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

////OCR识别返回数据结构
//OCRResult
//-Text   识别的文本（所有TextBlocks内的文本拼接）
//-TextBlocks
//--Text   该分割区域下的识别文本
//--BoxPoints   该分割区域的四个点坐标，围成一个范围
//---Point   点   
//----X     X坐标
//----Y     Y坐标

#include <include/OCREngine.h>
#include <include/Parameter.h>
#include <vector>
using namespace std;

//导出C函数
extern "C" {
#ifdef _WIN64
	__declspec(dllexport) OCREngine* Initialize(char* det_infer, char* cls_infer, char* rec_infer, char* keys, OCRParameter  parameter);
	__declspec(dllexport) int  Detect(OCREngine* engine, char* imagefile, LpOCRResult* pOCRResult);
	__declspec(dllexport) int  DetectMat(OCREngine* engine, cv::Mat& cvmat, LpOCRResult* pOCRResult);
	__declspec(dllexport) int DetectByte(OCREngine* engine, char* imagebytedata, size_t* size, LpOCRResult* OCRResult);
	__declspec(dllexport) int DetectBase64(OCREngine* engine, char* imagebase64, LpOCRResult* OCRResult);
	__declspec(dllexport) void FreeEngine(OCREngine* engine);
	__declspec(dllexport) void FreeDetectResult(LpOCRResult pOCRResult);
	__declspec(dllexport) void DetectImage(char* modelPath_det_infer, char* imagefile, OCRParameter parameter);
	__declspec(dllexport) int IsCPUSupport();
#else
	 OCREngine* Initialize(char* det_infer, char* cls_infer, char* rec_infer, char* keys, OCRParameter  parameter);
	int  Detect(OCREngine* engine, char* imagefile, LpOCRResult* pOCRResult);
	 int  DetectMat(OCREngine* engine, cv::Mat& cvmat, LpOCRResult* pOCRResult);
	int DetectByte(OCREngine* engine, char* imagebytedata, size_t* size, LpOCRResult* OCRResult);
	 int DetectBase64(OCREngine* engine, char* imagebase64, LpOCRResult* OCRResult);
	void FreeEngine(OCREngine* engine);
	void FreeDetectResult(LpOCRResult pOCRResult);
	void DetectImage(char* modelPath_det_infer, char* imagefile, OCRParameter parameter);
	 int IsCPUSupport();
#endif 
	

};
