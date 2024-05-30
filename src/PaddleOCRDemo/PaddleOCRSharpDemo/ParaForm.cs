using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using PaddleOCRSharp;
 
namespace PaddleOCRSharpDemo;

public partial class ParaForm : Form
{
    public ParaForm()
    {
        InitializeComponent();
    }

    private string imageFilePath = "";
    private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        imageFilePath = "";
        var ofd = new OpenFileDialog();
        ofd.Filter = "*.*|*.bmp;*.jpg;*.jpeg;*.tiff;*.tiff;*.png";
        if (ofd.ShowDialog() != DialogResult.OK) return;
        imageFilePath = ofd.FileName;
        var imageBytes = File.ReadAllBytes(imageFilePath);
        pictureBox1.BackgroundImage = new Bitmap(new MemoryStream(imageBytes));
        ParaChanged();
    }

    private void ParaChanged()
    {
        //OCR参数
        var ocrParameter = new OCRParameter
        {
            max_side_len        = trackBar2.Value,
            det_db_thresh       = Convert.ToSingle(Math.Round(trackBar3.Value * 1.0 / 100, 2)),
            det_db_box_thresh   = Convert.ToSingle(Math.Round(trackBar4.Value * 1.0 / 100, 2)),
            det_db_unclip_ratio = Convert.ToSingle(Math.Round(trackBar5.Value * 1.0 / 10, 2)),
            det_db_score_mode   = use_polygon_score.Checked,
            use_angle_cls       = use_angle_cls.Checked,
            cls_thresh          = Convert.ToSingle(Math.Round(trackBar1.Value * 1.0 / 100, 2))
        };

        var image = File.ReadAllBytes(imageFilePath);
        var bitmap         = new Bitmap(new MemoryStream(image));

        var scale = Math.Round(trackBar6.Value * 1.0 / 10, 1);

        bitmap = new Bitmap(bitmap, new Size(Convert.ToInt32(bitmap.Width * scale), Convert.ToInt32(bitmap.Height * scale)));
        var tmpFile = Path.GetTempPath() + Guid.NewGuid() + ".bmp";
        bitmap.Save(tmpFile);
        bitmap.Dispose();
        GC.Collect();

        // PaddleOCREngine.Detect(null, tempfile, oCRParameter);
        File.Delete(tmpFile);
        var file      = Environment.CurrentDirectory + "\\ocr_vis.png";
        var imageBytes = File.ReadAllBytes(file);
        pictureBox1.BackgroundImage = new Bitmap(new MemoryStream(imageBytes));
    }
    private void trackBar1_Scroll(object sender, EventArgs e)
    {
        label1.Text = Math.Round(trackBar1.Value * 1.0 / 100, 2).ToString();

        ParaChanged();

    }

    private void trackBar2_Scroll(object sender, EventArgs e)
    {
        label2.Text = trackBar2.Value.ToString();

        ParaChanged();
    }

    private void ParaForm_Load(object sender, EventArgs e)
    {
        if (File.Exists(Environment.CurrentDirectory + "\\ocr_vis.png"))

        {
            File.Delete(Environment.CurrentDirectory + "\\ocr_vis.png");
        }
    }

    private void trackBar3_Scroll(object sender, EventArgs e)
    {
        label3.Text = Math.Round(trackBar3.Value * 1.0 / 100, 2).ToString();

        ParaChanged();
    }

    private void trackBar4_Scroll(object sender, EventArgs e)
    {
        label4.Text = Math.Round(trackBar4.Value * 1.0 / 100, 2).ToString();

        ParaChanged();

    }

    private void trackBar5_Scroll(object sender, EventArgs e)
    {
        label5.Text = Math.Round(trackBar5.Value * 1.0 / 10, 2).ToString();

        ParaChanged();

    }

    private void use_polygon_score_CheckedChanged(object sender, EventArgs e)
    {
        ParaChanged();

    }

    private void use_angle_cls_CheckedChanged(object sender, EventArgs e)
    {
        ParaChanged();
    }

    private void trackBar6_Scroll(object sender, EventArgs e)
    {
        label12.Text = Math.Round(trackBar6.Value * 1.0 / 10, 1).ToString();

        ParaChanged();
    }
}