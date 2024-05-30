#pragma once
// Copyright (c) 2021 饶玉田 Authors. All Rights Reserved.
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


#include <include/yt_Parameter.h>
#include <vector>
#include <codecvt>
using namespace std;

//导出C函数
extern "C" {
#ifdef _WIN64
	__declspec(dllexport) void EnableANSIResult(bool useANSI);
	__declspec(dllexport) void Initialize(char* det_infer, char* cls_infer, char* rec_infer, char* keys, OCRParameter parameter);
	__declspec(dllexport) void Initializejson(char* modelPath_det_infer, char* modelPath_cls_infer, char* modelPath_rec_infer, char* keys, char* parameterjson);
	__declspec(dllexport) char* Detect(char* imagefile);
	//__declspec(dllexport) char* DetectMat(cv::Mat& cvmat);
	__declspec(dllexport) char* DetectByte(char* imagebytedata, size_t* size);
	__declspec(dllexport) char* DetectBase64(char* imagebase64);
	__declspec(dllexport) char* DetectByteData(const char* img, int nWidth, int nHeight, int nChannel);
	__declspec(dllexport) void FreeEngine();
	
	__declspec(dllexport) void StructureInitialize(char* modelPath_det_infer, char* modelPath_rec_infer, char* keys, char* table_model_dir, char* table_char_dict_path, StructureParameter parameter);
	__declspec(dllexport) void StructureInitializejson(char* modelPath_det_infer, char* modelPath_rec_infer, char* keys, char* table_model_dir, char* table_char_dict_path, char* parameterjson);
	__declspec(dllexport) char* GetStructureDetectFile(char* imagefile);
	/*__declspec(dllexport) char* GetStructureDetectMat(cv::Mat& cvmat);*/
	__declspec(dllexport) char* GetStructureDetectByte(char* imagebytedata, size_t* size);
	__declspec(dllexport) char* GetStructureDetectBase64(char* imagebase64);
	__declspec(dllexport) void FreeStructureEngine();
	 

#else
	 void EnableANSIResult(bool useANSI);
	 void Initialize(char* det_infer, char* cls_infer, char* rec_infer, char* keys, OCRParameter parameter);
	 void Initializejson(char* modelPath_det_infer, char* modelPath_cls_infer, char* modelPath_rec_infer, char* keys, char* parameterjson);
	 char* Detect(char* imagefile);
	/* char* DetectMat(cv::Mat& cvmat);*/
	 char* DetectByte(char* imagebytedata, size_t* size);
	 char* DetectBase64(char* imagebase64);
	 char* DetectByteData(const char* img, int nWidth, int nHeight, int nChannel);
	 void FreeEngine();
	
	  void StructureInitialize(char* modelPath_det_infer, char* modelPath_rec_infer, char* keys, char* table_model_dir, char* table_char_dict_path, StructureParameter parameter);
	 void StructureInitializejson(char* modelPath_det_infer, char* modelPath_rec_infer, char* keys, char* table_model_dir, char* table_char_dict_path, char* parameterjson);
	 char* GetStructureDetectFile(char* imagefile);
	/*  char* GetStructureDetectMat(cv::Mat& cvmat);*/
	  char* GetStructureDetectByte(char* imagebytedata, size_t* size);
	 char* GetStructureDetectBase64(char* imagebase64);
	  void FreeStructureEngine();


#endif 
};
