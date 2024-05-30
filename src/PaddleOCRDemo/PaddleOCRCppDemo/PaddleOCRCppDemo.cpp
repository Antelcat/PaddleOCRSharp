#include <iostream>
#include <Windows.h>
#include <tchar.h>
#include "string"
#include <string.h>
#include <include/yt_Parameter.h>
#include <io.h> 
#include <chrono>
using namespace std;
using namespace chrono;
#pragma comment (lib,"PaddleOCR.lib")
extern "C" {
	
	/// <summary>
	/// 是否使用单字节编码（适用于go,rust）,C#,Python不用打开此开关
	/// </summary>
	/// <param name="useANSI"></param>
	__declspec(dllimport) void EnableANSIResult(bool useANSI);
	/// <summary>
	/// 文字识别引擎初始化
	/// </summary>
	/// <param name="det_infer">det模型全路径</param>
	/// <param name="cls_infer">cls模型全路径</param>
	/// <param name="rec_infer">rec模型全路径</param>
	/// <param name="keys">字典全路径</param>
	/// <param name="parameter">识别参数对象</param>
	/// <returns></returns>
	__declspec(dllimport) void Initialize(char* det_infer, char* cls_infer, char* rec_infer, char* keys, OCRParameter parameter);
	/// <summary>
	/// PaddleOCREngine引擎初始化
	/// </summary>
	/// <param name="det_infer">det模型全路径</param>
	/// <param name="cls_infer">cls模型全路径</param>
	/// <param name="rec_infer">rec模型全路径</param>
	/// <param name="keys">字典全路径</param>
	/// <param name="parameterjson">识别参数对象json字符串</param>
	/// <returns></returns>
	__declspec(dllimport) void Initializejson(char* modelPath_det_infer, char* modelPath_cls_infer, char* modelPath_rec_infer, char* keys, char* parameterjson);
	/// <summary>
	/// 文本检测识别-图像文件路径
	/// </summary>
	/// <param name="imagebytedata">图像文件路径</param>
	/// <returns></returns>
	__declspec(dllimport) char* Detect(char* imagefile);

	/*/// <summary>
	/// 文本检测识别-OpenCV Mat
	/// </summary>
	/// <param name="cvmat">Mat对象</param>
	/// <returns></returns>
	__declspec(dllimport)char* DetectMat(cv::Mat& cvmat);*/
	/// <summary>
	/// 文本检测识别-图像字节流
	/// </summary>
	/// <param name="imagebytedata">图像字节流</param>
	/// <param name="size">图像字节流长度</param>
	/// <returns></returns>
	__declspec(dllimport) char* DetectByte( char* imagebytedata, size_t* size);
	
	/// <summary>
	/// 文本检测识别-图像字节流
	/// </summary>
	/// <param name="img">图像地址</param>
	/// <param name="nWidth">图像宽度</param>
	///  <param name="nHeight">图像高度</param>
	///  <param name="nChannel">图像通道数</param>
	/// <returns></returns>
	__declspec(dllimport) char* DetectByteData(const char* img, int nWidth, int nHeight, int nChannel);

	/// <summary>
	/// 文本检测识别-图像base64
	/// </summary>
	/// <param name="imagebase64">图像base64</param>
	/// <returns></returns>
	__declspec(dllimport) char* DetectBase64( char* imagebase64);

	/// <summary>
	/// 释放引擎对象
	/// </summary>
	__declspec(dllimport) void FreeEngine();
	






	/// <summary>
	/// 表格识别引擎初始化
	/// </summary>
	/// <param name="modelPath_det_infer">det模型全路径</param>
	/// <param name="modelPath_rec_infer">cls模型全路径</param>
	/// <param name="keys">字典全路径</param>
	/// <param name="table_model_dir">表格模型全路径</param>
	/// <param name="table_char_dict_path">表格字典全路径</param>
	/// <param name="parameter">参数对象</param>
	/// <returns></returns>
	__declspec(dllimport) void StructureInitialize(char* modelPath_det_infer, char* modelPath_rec_infer, char* keys, char* table_model_dir, char* table_char_dict_path, StructureParameter parameter);
	/// <summary>
	/// 表格识别引擎初始化json格式
	/// </summary>
	/// <param name="modelPath_det_infer">det模型全路径</param>
	/// <param name="modelPath_rec_infer">cls模型全路径</param>
	/// <param name="keys">字典全路径</param>
	/// <param name="table_model_dir">表格模型全路径</param>
	/// <param name="table_char_dict_path">表格字典全路径</param>
	/// <param name="parameterjson">参数对象json格式</param>
	/// <returns></returns>
	__declspec(dllimport) void StructureInitializejson(char* modelPath_det_infer, char* modelPath_rec_infer, char* keys, char* table_model_dir, char* table_char_dict_path, char* parameterjson);
	/// <summary>
	///表格识别
	/// </summary>
	/// <param name="imagefile">文件路径</param>
	/// <returns></returns>
	__declspec(dllimport) char* GetStructureDetectFile(char* imagefile);
	///// <summary>
	/////表格识别
	///// </summary>
	///// <param name="cvmat">opencv Mat对象</param>
	///// <returns></returns>
	//__declspec(dllimport) char* GetStructureDetectMat(cv::Mat& cvmat);
	/// <summary>
	///表格识别
	/// </summary>
	/// <param name="imagebytedata">图像字节流</param>
	/// <param name="size">图像字节流长度</param>
	/// <returns></returns>
	__declspec(dllimport) char* GetStructureDetectByte(char* imagebytedata, size_t* size);
	/// <summary>
	/// 表格识别
	/// </summary>
	/// <param name="imagebase64">图像base64</param>
	/// <returns></returns>
	__declspec(dllimport) char* GetStructureDetectBase64(char* imagebase64);
	/// <summary>
	/// 释放引擎对象
	/// </summary>
	__declspec(dllimport) void FreeStructureEngine();






};

void getFiles(string path, vector<string>& files)
{
	intptr_t   hFile = 0;//文件句柄，过会儿用来查找
	struct _finddata_t fileinfo;//文件信息
	string p;
	if ((hFile = _findfirst(p.assign(path).append("\\*").c_str(), &fileinfo)) != -1)
		//如果查找到第一个文件
	{
		do
		{
			if ((fileinfo.attrib & _A_SUBDIR))//如果是文件夹
			{
				if (strcmp(fileinfo.name, ".") != 0 && strcmp(fileinfo.name, "..") != 0)
					getFiles(p.assign(path).append("\\").append(fileinfo.name), files);
			}
			else//如果是文件
			{
				files.push_back(p.assign(path).append("\\").append(fileinfo.name));
			}
		} while (_findnext(hFile, &fileinfo) == 0);	//能寻找到其他文件

		_findclose(hFile);	//结束查找，关闭句柄
	}
}

int main()
{  
	////0：不支持，1：AVX，2：AVX2
	/*int cpus = IsCPUSupport();*/
	// 
	OCRParameter parameter;
	parameter.enable_mkldnn = true;
	parameter.cpu_math_library_num_threads = 24;
	parameter.max_side_len = 960;
	char path[MAX_PATH];
	 
	GetCurrentDirectoryA(MAX_PATH, path);

	string cls_infer(path);

	
	//V3
	cls_infer += "\\inference\\ch_ppocr_mobile_v2.0_cls_infer";
	string rec_infer(path);
	rec_infer += "\\inference\\ch_PP-OCRv3_rec_infer";
	string det_infer(path);
	det_infer += "\\inference\\ch_PP-OCRv3_det_infer";
	string ocrkeys(path);
	ocrkeys += "\\inference\\ppocr_keys.txt";


	string imagepath(path);
	imagepath += "\\image"; 
	vector<string> images;
	getFiles(imagepath, images);

	Initialize(const_cast<char*>(det_infer.c_str()),
							 const_cast<char*>(cls_infer.c_str()), 
						     const_cast<char*>(rec_infer.c_str()),
							 const_cast<char*>(ocrkeys.c_str()),
		                     parameter);
	
	std::wcout.imbue(std::locale("chs"));
	if (images.size() > 0)
	{
		for (size_t i = 0; i < images.size(); i++)
		{ 
		    auto	start = std::chrono::steady_clock::now();
			  wstring  result = (WCHAR*)Detect( const_cast<char*>(images[i].c_str()));
	      	std::wcout << result << endl;
			auto	end = std::chrono::steady_clock::now();
			auto duration=	duration_cast<milliseconds>(end - start);
		 
			std::cout << duration.count() <<"ms"<< endl;
		}
	}
	try
	{
		FreeEngine();
	}
	catch (const std::exception& e)
	{
		std::wcout << e.what();
	}
	
	std::cin.get();
}

