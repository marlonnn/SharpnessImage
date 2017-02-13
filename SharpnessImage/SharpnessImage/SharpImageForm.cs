using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpnessImage
{
    public partial class SharpImageForm : Form
    {
        [DllImport("SharpnessImageLib.dll", EntryPoint = "Tenengrad")]
        extern static double Tenengrad(string fileName);

        [DllImport("SharpnessImageLib.dll", EntryPoint = "Laplacian")]
        extern static double Laplacian(string fileName);

        [DllImport("SharpnessImageLib.dll", EntryPoint = "Variance")]
        extern static double Variance(string fileName);

        private List<Frame> frames;
        private int currentIndex;
        private bool play;
        public SharpImageForm()
        {
            InitializeComponent();
            frames = new List<Frame>();
            this.startToolStripMenuItem.Enabled = false;
        }

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
            this.startToolStripMenuItem.Image = start ?
                Properties.Resources.pause : Properties.Resources.Run;
            this.startToolStripMenuItem.Text = !start ? "Start" : "Pause";
            this.timer.Enabled = start;
            this.openToolStripMenuItem.Enabled = !start;
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        private void StopPlay()
        {
            this.timer.Enabled = false;
            this.currentIndex = 0;
            this.startToolStripMenuItem.Enabled = false;
            this.openToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// 更新进度条
        /// </summary>
        /// <param name="value"></param>
        private void UpdateProgressBar(int value)
        {

            this.progressBar.Value = value;
        }

        private double CalculateSharpnessImage(Frame frame)
        {
            return 0.0d;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.frames != null && this.frames.Count > 0)
            {
                try
                {
                    this.pictureBox.ImageLocation = frames[currentIndex].FileFullName;
                    double d = Tenengrad(frames[currentIndex].FileFullName);
                    UpdateProgressBar(currentIndex);
                    currentIndex++;
                    if (currentIndex == frames.Count)
                    {
                        StopPlay();
                    }
                }
                catch (Exception ee)
                {
                }
            }
        }
    }
}
