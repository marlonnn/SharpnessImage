using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpnessImage
{
    public partial class SharpImageForm : Form
    {
        private List<Frame> frames;
        private bool play;
        private List<SharpnessImage> sharpnessImages;
        private SharpnessImage finaleIamge;

        public SharpImageForm()
        {
            InitializeComponent();
            frames = new List<Frame>();
            sharpnessImages = new List<SharpnessImage>();
            this.startToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog ofd = new FolderBrowserDialog())
            {
                ofd.Description = "请选择将要播放帧图的文件夹";
                ofd.RootFolder = Environment.SpecialFolder.Desktop;
                ofd.SelectedPath = System.Environment.CurrentDirectory + "\\Images";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    frames.Clear();
                    string fileFolder = ofd.SelectedPath;
                    DirectoryInfo folder = new DirectoryInfo(fileFolder);
                    try
                    {
                        FileInfo[] fileInfos = folder.GetFiles();
                        //Array.Sort(fileInfos, delegate (FileInfo x, FileInfo y)
                        //{
                        //    return Int32.Parse(Path.GetFileNameWithoutExtension(x.Name)).CompareTo
                        //    (Int32.Parse(Path.GetFileNameWithoutExtension(y.Name)));
                        //});
                        foreach (FileInfo info in fileInfos)
                        {
                            var v = Path.GetFileNameWithoutExtension(info.Name);
                            Frame frame = new Frame(info.FullName, fileFolder);
                            frames.Add(frame);
                        }
                        this.startToolStripMenuItem.Enabled = true;
                    }
                    catch (Exception ee)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// start calculate sharpness of every image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            play = !play;
            PlayFrames(play);
        }

        private void PlayFrames(bool start)
        {
            Thread thread = new Thread(CalculateSharpnessImage);
            thread.Start();
        }

        /// <summary>
        /// 更新进度条
        /// </summary>
        /// <param name="value"></param>
        private void UpdateProgressBar(int value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int>(UpdateProgressBar), value);
            }
            else
            {
                this.progressBar.Value = value;
            }
        }

        /// <summary>
        /// 显示最终的焦点图片
        /// </summary>
        /// <param name="fileName"></param>
        private void ShowSharpnessImage(string fileName)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(ShowSharpnessImage), fileName);
            }
            else
            {
                this.pictureBox.ImageLocation = fileName;
                this.linkLblName.Text = Path.GetFileName(fileName);
            }
        }

        /// <summary>
        /// 计算耗时 ms
        /// </summary>
        /// <param name="milliseconds"></param>
        private void TotalTime(long milliseconds)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<long>(TotalTime), milliseconds);
            }
            else
            {
                this.lblTime.Text = string.Format("Total:{0}ms", milliseconds);
            }
        }

        /// <summary>
        /// 计算焦点图片
        /// </summary>
        private void CalculateSharpnessImage()
        {
            sharpnessImages = new List<SharpnessImage>();
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
            sw.Start();
            if (this.frames != null && this.frames.Count > 0)
            {
                for (int i=0; i<frames.Count; i++)
                {
                    try
                    {
                        sw1.Start();
                        double sharpnessValue = SharpnessAlgorithm.Tenengrad(frames[i].FileFullName);
                        System.Diagnostics.Trace.WriteLine("Tenengrad : " + sw1.ElapsedMilliseconds);
                        sw1.Reset();
                        var sharpnessImage = new SharpnessImage(frames[i].FileFullName, sharpnessValue);
                        sharpnessImages.Add(sharpnessImage);
                        UpdateProgressBar(i);
                    }
                    catch (Exception ee)
                    {
                    }
                }
            }

            System.Diagnostics.Trace.WriteLine("Calculate sharpness image : " + sw.ElapsedMilliseconds);
            TotalTime(sw.ElapsedMilliseconds);
            sw1.Stop();
            sw.Stop();
            var maxValue = sharpnessImages.Select(sharpnessImage => sharpnessImage.SharpnessValue).Max();
            finaleIamge = sharpnessImages.Find(sharpnessImage => sharpnessImage.SharpnessValue == maxValue);
            ShowSharpnessImage(finaleIamge.FileFullName);
        }

        /// <summary>
        /// 打开最终焦点图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLblName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(finaleIamge.FileFullName);
        }
    }
}
