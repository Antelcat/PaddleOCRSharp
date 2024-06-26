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

#pragma once
#pragma pack(push,1)
#include <vector>
using namespace std;


#pragma pack(push,1)
/// <summary>
/// OCR识别参数
/// </summary>
struct OCRParameter
{
	//通用参数
	bool use_gpu;//是否使用GPU；默认false
	int gpu_id;//GPU id，使用GPU时有效；默认0;
	int gpu_mem;//申请的GPU内存;默认4000
	int cpu_math_library_num_threads;//CPU预测时的线程数，在机器核数充足的情况下，该值越大，预测速度越快；默认10
	bool enable_mkldnn;//是否使用mkldnn库；默认true
    

	//前向相关
	bool det;//是否执行文字检测；默认true
	bool rec;//是否执行文字识别；默认true
	bool cls;//是否执行文字方向分类；默认false

	//检测模型相关
	int    max_side_len;//输入图像长宽大于960时，等比例缩放图像，使得图像最长边为960,；默认960
	float  det_db_thresh;//用于过滤DB预测的二值化图像，设置为0.-0.3对结果影响不明显；默认0.3
	float   det_db_box_thresh;//DB后处理过滤box的阈值，如果检测存在漏框情况，可酌情减小；默认0.5
	float   det_db_unclip_ratio;//表示文本框的紧致程度，越小则文本框更靠近文本;默认1.6
	bool use_dilation;//是否在输出映射上使用膨胀,默认false
	bool det_db_score_mode;//1:使用多边形框计算bbox score，0:使用矩形框计算。矩形框计算速度更快，多边形框对弯曲文本区域计算更准确。
	bool visualize;//是否对结果进行可视化，为1时，预测结果会保存在output字段指定的文件夹下和输入图像同名的图像上。默认false
	 
	
	//方向分类器相关
	bool use_angle_cls;//是否使用方向分类器,默认false
	float   cls_thresh;//方向分类器的得分阈值，默认0.9
	int cls_batch_num;//方向分类器batchsize，默认1

	//识别模型相关
	int rec_batch_num;//识别模型batchsize，默认6
	int   rec_img_h;//识别模型输入图像高度，默认48
	int rec_img_w;//识别模型输入图像宽度，默认320
	
	bool show_img_vis;//是否显示预测结果，默认false

	OCRParameter()
	{
		//通用参数
		use_gpu = false;
		gpu_id = 0;
		gpu_mem = 4000;
		cpu_math_library_num_threads = 10;
		enable_mkldnn = true;
		
		//前向相关
		 det=true;
		 rec=true;
		 cls=false;



		//检测模型相关
		
		 max_side_len = 960;
		 det_db_thresh = 0.3f;
		 det_db_box_thresh = 0.618f;
		 det_db_unclip_ratio = 1.6f;
		 use_dilation = false;
		 det_db_score_mode = 1;
		 visualize = false;

		//方向分类器相关
		use_angle_cls = false;
		cls_thresh = 0.9f;
		cls_batch_num = 1;

		//识别模型相关
		rec_batch_num = 6;
		rec_img_h = 48;
		rec_img_w = 320;

		show_img_vis = false;
	}
};

/// <summary>
/// 文本区域
/// </summary>
struct Textblock {

	std::wstring textblock;
	std::vector<std::vector<int>> box;
	float score;
	Textblock(wstring textblock, std::vector<std::vector<int>> box, float score) {
		this->textblock = textblock;
		this->box = box;
		this->score = score;
	}
};

/// <summary>
/// OCR文本区域四周的点
/// </summary>
struct _OCRTextPoint {
	int x;
	int y;
	_OCRTextPoint() :x(0), y(0) {
	}
};

/// <summary>
/// OCR文本
/// </summary>
struct _OCRText {
	//textblock文本
	int textLen;
	char* ptext;
	//一个textblock四个点
	_OCRTextPoint points[4];
	//得分
	float score;
	_OCRText() {
		textLen = 0;
		ptext = nullptr;
		score = 0.0f;
	}
};
/// <summary>
/// OCR识别结果
/// </summary>
typedef struct _OCRResult {
	//textblock文本个数
	int textCount;
	_OCRText* pOCRText;
}OCRResult, * LpOCRResult;

#pragma pack(pop) 
#pragma pack(pop) 
