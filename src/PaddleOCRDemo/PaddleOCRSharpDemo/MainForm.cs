using PaddleOCRSharp;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PaddleOCRSharpDemo;

/// <summary>
/// PaddleOCRSharp使用示例
/// </summary>
public partial class MainForm : Form
{
    private readonly string[]              bmpFilters = [".bmp", ".jpg", ".jpeg", ".tiff", ".tif", ".png"];
    private const    string                FileFilter = "*.*|*.bmp;*.jpg;*.jpeg;*.tiff;*.tiff;*.png";
    private const    string                Title      = "PaddleOCR";
    private          PaddleOCREngine       engine;
    private          PaddleStructureEngine StructEngine;
    private          Bitmap?               bmp;
    private          OCRResult             lastOCRResult;
    private readonly string                outPath = Path.Combine(Environment.CurrentDirectory, "out");
    private          DateTime              dt1     = DateTime.Now;
    private          DateTime              dt2     = DateTime.Now;

    public MainForm()
    {
        InitializeComponent();
        Text            = Title;
        imageView1.AllowDrop = true;

        //EngineBase.PaddleOCRdllName = Path.Combine(Environment.CurrentDirectory, "x64", "PaddleOCR.dll");
    }

   

    private void MainForm_Load(object sender, EventArgs e)
    {
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }

        //自带轻量版中英文模型V3模型
        OCRModelConfig? config = null;

        //服务器中英文模型
        //OCRModelConfig config = new OCRModelConfig();
        //string root = System.IO.Path.GetDirectoryName(typeof(OCRModelConfig).Assembly.Location);
        //string modelPathroot = root + @"\inferenceserver";
        //config.det_infer = modelPathroot + @"\ch_ppocr_server_v2.0_det_infer";
        //config.cls_infer = modelPathroot + @"\ch_ppocr_mobile_v2.0_cls_infer";
        //config.rec_infer = modelPathroot + @"\ch_ppocr_server_v2.0_rec_infer";
        //config.keys = modelPathroot + @"\ppocr_keys.txt";

        //英文和数字模型V3
        //OCRModelConfig config = new OCRModelConfig();
        //string root = System.IO.Path.GetDirectoryName(typeof(OCRModelConfig).Assembly.Location);
        //string modelPathroot = root + @"\en_v3";
        //config.det_infer = modelPathroot + @"\en_PP-OCRv3_det_infer";
        //config.cls_infer = modelPathroot + @"\ch_ppocr_mobile_v2.0_cls_infer";
        //config.rec_infer = modelPathroot + @"\en_PP-OCRv3_rec_infer";
        //config.keys = modelPathroot + @"\en_dict.txt";

        //OCR参数
        var ocrParameter = new OCRParameter
        {
            cpu_math_library_num_threads = 10,    //预测并发线程数
            enable_mkldnn                = true,  //web部署该值建议设置为0,否则出错，内存如果使用很大，建议该值也设置为0.
            cls                          = false, //是否执行文字方向分类；默认false
            det                          = true,  //是否开启方向检测，用于检测识别180旋转
            use_angle_cls                = false, //是否开启方向检测，用于检测识别180旋转
            det_db_score_mode            = true   //是否使用多段线，即文字区域是用多段线还是用矩形，
        };

        //初始化OCR引擎
        engine = new PaddleOCREngine(config, ocrParameter);

        //模型配置，使用默认值
        StructureModelConfig? structureModelConfig = null;
        //表格识别参数配置，使用默认值
        var structureParameter = new StructureParameter();
        StructEngine = new PaddleStructureEngine(structureModelConfig, structureParameter);
    }

    private Bitmap? GetClipboardImage()
    {
        bmp = (Bitmap?)Clipboard.GetImage();
        if (bmp != null) return bmp;
        var files = Clipboard.GetFileDropList();

        var Filtersarr = new string[files.Count];
        files.CopyTo(Filtersarr, 0);
        Filtersarr = Filtersarr.Where(x => bmpFilters.Contains(Path.GetExtension(x).ToLower())).ToArray();
        if (Filtersarr.Length <= 0) return bmp;
        var image = File.ReadAllBytes(Filtersarr[0]);
        bmp = new Bitmap(new MemoryStream(image));

        return bmp;
    }

    private void imageView1_DragDrop(object sender, DragEventArgs e)
    {
        var data = e.Data;
        if (data == null) return;
        var files = data.GetData(DataFormats.FileDrop, autoConvert: true) as string[];

        var Filtersarr = new string[files!.Length];
        files.CopyTo(Filtersarr, 0);
        Filtersarr = Filtersarr.Where(x => bmpFilters.Contains(Path.GetExtension(x).ToLower())).ToArray();
        if (Filtersarr.Length <= 0) return;
        var image = File.ReadAllBytes(Filtersarr[0]);
        bmp              = new Bitmap(new MemoryStream(image));
        imageView1.Image = bmp;

        richTextBox1.Clear();
        richTextBox1.Show();
        dataGridView1.Hide();
        if (bmp == null) return;
        dt1 = DateTime.Now;
        var ocrResult = engine.DetectText(image);
        dt2 = DateTime.Now;
        ShowOCRResult(ocrResult);
    }


    private void imageView1_DragEnter(object sender, DragEventArgs e)
    {
        e.Effect = DragDropEffects.Move;
    }

    private void imageView1_DragOver(object sender, DragEventArgs e)
    {
        e.Effect = DragDropEffects.Move;
    }

    /// <summary>
    /// 打开本地图片
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void toolStripopenFile_Click(object sender, EventArgs e)
    {
        var ofd = new OpenFileDialog();
        ofd.Filter = FileFilter;
        if (ofd.ShowDialog() != DialogResult.OK) return;
        var image = File.ReadAllBytes(ofd.FileName);
        bmp              = new Bitmap(new MemoryStream(image));
        imageView1.Image = bmp;

        richTextBox1.Clear();
        richTextBox1.Show();
        dataGridView1.Hide();
        if (bmp == null) return;

        dt1 = DateTime.Now;
        var ocrResult = engine.DetectText(image);
        dt2 = DateTime.Now;
        ShowOCRResult(ocrResult);
    }

    /// <summary>
    /// 识别截图文本
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void toolStripLabel2_Click(object sender, EventArgs e)
    {
        this.Hide();
        System.Threading.Thread.Sleep(200);
        Application.DoEvents();
        richTextBox1.Clear();
        richTextBox1.Show();
        dataGridView1.Hide();
        var capture = new ScreenCapturer.ScreenCapturerTool();
        if (capture.ShowDialog() == DialogResult.OK)
        {
            bmp              = (Bitmap)capture.Image;
            imageView1.Image = bmp;
            try
            {
                dt1 = DateTime.Now;
                var ocrResult = engine.DetectText(bmp);
                dt2 = DateTime.Now;
                ShowOCRResult(ocrResult);
            }
            catch (Exception ex)
            {
                //
            }
        }

        this.Show();
    }

    /// <summary>
    /// 剪切板识别
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>       
    private void toolStripsnapocr_Click(object sender, EventArgs e)
    {
        bmp = GetClipboardImage();

        imageView1.Image = bmp;

        try
        {
            dt1 = DateTime.Now;
            var ocrResult = engine.DetectText(bmp);
            dt2 = DateTime.Now;
            ShowOCRResult(ocrResult);
        }
        catch (Exception ex)
        {
            //
        }
    }

    /// <summary>
    /// 本地文件表格
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void toolStripLabel4_Click(object sender, EventArgs e)
    {
        var ofd = new OpenFileDialog();
        ofd.Filter = FileFilter;
        if (ofd.ShowDialog() != DialogResult.OK) return;
        var imagebyte = File.ReadAllBytes(ofd.FileName);
        bmp              = new Bitmap(new MemoryStream(imagebyte));
        imageView1.Image = bmp;
        if (bmp == null) return;
        var result = StructEngine.StructureDetect(bmp);
        ShowOCRResult(result, Path.GetFileNameWithoutExtension(ofd.FileName));
    }

    /// <summary>
    /// 识别截图表格
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void toolStripLabel3_Click(object sender, EventArgs e)
    {
        this.Hide();

        System.Threading.Thread.Sleep(200);
        Application.DoEvents();

        var capture = new ScreenCapturer.ScreenCapturerTool();
        if (capture.ShowDialog() != DialogResult.OK) return;
        bmp              = (Bitmap)capture.Image;
        imageView1.Image = bmp;
        var result = StructEngine.StructureDetect(bmp);
        ShowOCRResult(result, Path.GetRandomFileName());
        this.Show();
    }

    /// <summary>
    /// 识别剪切板表格
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void toolStripsnaptable_Click(object sender, EventArgs e)
    {
        bmp = GetClipboardImage();

        imageView1.Image = bmp;
        if (bmp == null) return;
        var result = StructEngine.StructureDetect(bmp);
        ShowOCRResult(result, Path.GetRandomFileName());
    }

    /// <summary>
    /// 显示结果
    /// </summary>
    private void ShowOCRResult(OCRResult ocrResult)
    {
        lastOCRResult = ocrResult;
        richTextBox1.Clear();
        var bitmap = (Bitmap)imageView1.Image;

        switch (toolStripComboBox1.Text)
        {
            case "简单显示":
            {
                foreach (var item in ocrResult.TextBlocks)
                {
                    richTextBox1.AppendText(item.Text + "\n");
                }

                break;
            }
            case "详细显示":
            {
                foreach (var item in ocrResult.TextBlocks)
                {
                    richTextBox1.AppendText(item + "\n");
                }

                break;
            }
        }

        var bmp = new Bitmap(bitmap.Width, bitmap.Height);
        using (var g = Graphics.FromImage(bmp))
        {
            g.DrawImage(bitmap, 0, 0);
            foreach (var item in ocrResult.TextBlocks)
            {
                g.DrawPolygon(new Pen(Brushes.Red, 2),
                    item.BoxPoints.Select(x => new PointF() { X = x.X, Y = x.Y }).ToArray());
            }
        }

        richTextBox1.AppendText("-----------------------------------\n");
        richTextBox1.AppendText($"耗时：{(dt2 - dt1).TotalMilliseconds}ms\n");
        imageView1.Image = bmp;
    }

    /// <summary>
    /// 显示表格结果
    /// </summary>
    private void ShowOCRResult(string result, string name)
    {
        var css = "<style>table{ border-spacing: 0pt;} td { border: 1px solid black;}</style>";
        result = result.Replace("<html>", "<html>" + css);
        var savefile = $"{Environment.CurrentDirectory}\\out\\{name}.html";
        File.WriteAllText(savefile, result);
        //打开网页查看效果
        Process.Start("explorer.exe", savefile);
    }

    private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowOCRResult(lastOCRResult);
    }
}