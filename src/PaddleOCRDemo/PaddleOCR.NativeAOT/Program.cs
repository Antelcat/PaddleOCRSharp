using System.Diagnostics;
using PaddleOCRSharp;

//初始化OCR引擎
var engine = new PaddleOCREngine(KnownModels.ChineseV4, new OCRParameter
{
    cpu_math_library_num_threads = 10,    
    enable_mkldnn                = true,  
    cls                          = false, 
    det                          = true,  
    use_angle_cls                = false, 
    det_db_score_mode            = true
});

Console.WriteLine("Please input the image path:");
Console.Write("> ");
var path = Console.ReadLine();
if (string.IsNullOrWhiteSpace(path))
{
    path = Path.Combine(AppContext.BaseDirectory, "sample.png");
}
var image = File.ReadAllBytes(path);

var stopWatch = new Stopwatch();
stopWatch.Start();
var ocrResult = engine.DetectText(image);
stopWatch.Stop();
Console.WriteLine(string.Join("\n", ocrResult.TextBlocks.Select(x => x.Text)));
Console.WriteLine("cost: " + stopWatch.ElapsedMilliseconds + "ms");