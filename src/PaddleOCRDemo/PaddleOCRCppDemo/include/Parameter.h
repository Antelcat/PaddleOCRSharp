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
/// OCRʶ�����
/// </summary>
struct OCRParameter
{
	//ͨ�ò���
	bool use_gpu;//�Ƿ�ʹ��GPU��Ĭ��false
	int gpu_id;//GPU id��ʹ��GPUʱ��Ч��Ĭ��0;
	int gpu_mem;//�����GPU�ڴ�;Ĭ��4000
	int cpu_math_library_num_threads;//CPUԤ��ʱ���߳������ڻ����������������£���ֵԽ��Ԥ���ٶ�Խ�죻Ĭ��10
	bool enable_mkldnn;//�Ƿ�ʹ��mkldnn�⣻Ĭ��true
    

	//ǰ�����
	bool det;//�Ƿ�ִ�����ּ�⣻Ĭ��true
	bool rec;//�Ƿ�ִ������ʶ��Ĭ��true
	bool cls;//�Ƿ�ִ�����ַ�����ࣻĬ��false

	//���ģ�����
	int    max_side_len;//����ͼ�񳤿����960ʱ���ȱ�������ͼ��ʹ��ͼ�����Ϊ960,��Ĭ��960
	float  det_db_thresh;//���ڹ���DBԤ��Ķ�ֵ��ͼ������Ϊ0.-0.3�Խ��Ӱ�첻���ԣ�Ĭ��0.3
	float   det_db_box_thresh;//DB�������box����ֵ�����������©��������������С��Ĭ��0.5
	float   det_db_unclip_ratio;//��ʾ�ı���Ľ��³̶ȣ�ԽС���ı���������ı�;Ĭ��1.6
	bool use_dilation;//�Ƿ������ӳ����ʹ������,Ĭ��false
	bool det_db_score_mode;//1:ʹ�ö���ο����bbox score��0:ʹ�þ��ο���㡣���ο�����ٶȸ��죬����ο�������ı���������׼ȷ��
	bool visualize;//�Ƿ�Խ�����п��ӻ���Ϊ1ʱ��Ԥ�����ᱣ����output�ֶ�ָ�����ļ����º�����ͼ��ͬ����ͼ���ϡ�Ĭ��false
	 
	
	//������������
	bool use_angle_cls;//�Ƿ�ʹ�÷��������,Ĭ��false
	float   cls_thresh;//����������ĵ÷���ֵ��Ĭ��0.9
	int cls_batch_num;//���������batchsize��Ĭ��1

	//ʶ��ģ�����
	int rec_batch_num;//ʶ��ģ��batchsize��Ĭ��6
	int   rec_img_h;//ʶ��ģ������ͼ��߶ȣ�Ĭ��48
	int rec_img_w;//ʶ��ģ������ͼ���ȣ�Ĭ��320
	
	bool show_img_vis;//�Ƿ���ʾԤ������Ĭ��false

	OCRParameter()
	{
		//ͨ�ò���
		use_gpu = false;
		gpu_id = 0;
		gpu_mem = 4000;
		cpu_math_library_num_threads = 10;
		enable_mkldnn = true;
		
		//ǰ�����
		 det=true;
		 rec=true;
		 cls=false;



		//���ģ�����
		
		 max_side_len = 960;
		 det_db_thresh = 0.3f;
		 det_db_box_thresh = 0.618f;
		 det_db_unclip_ratio = 1.6f;
		 use_dilation = false;
		 det_db_score_mode = 1;
		 visualize = false;

		//������������
		use_angle_cls = false;
		cls_thresh = 0.9f;
		cls_batch_num = 1;

		//ʶ��ģ�����
		rec_batch_num = 6;
		rec_img_h = 48;
		rec_img_w = 320;

		show_img_vis = false;
	}
};

/// <summary>
/// �ı�����
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
/// OCR�ı��������ܵĵ�
/// </summary>
struct _OCRTextPoint {
	int x;
	int y;
	_OCRTextPoint() :x(0), y(0) {
	}
};

/// <summary>
/// OCR�ı�
/// </summary>
struct _OCRText {
	//textblock�ı�
	int textLen;
	char* ptext;
	//һ��textblock�ĸ���
	_OCRTextPoint points[4];
	//�÷�
	float score;
	_OCRText() {
		textLen = 0;
		ptext = nullptr;
		score = 0.0f;
	}
};
/// <summary>
/// OCRʶ����
/// </summary>
typedef struct _OCRResult {
	//textblock�ı�����
	int textCount;
	_OCRText* pOCRText;
}OCRResult, * LpOCRResult;

#pragma pack(pop) 
#pragma pack(pop) 
