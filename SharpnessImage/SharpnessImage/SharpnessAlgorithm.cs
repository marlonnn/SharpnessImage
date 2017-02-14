using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;

namespace SharpnessImage
{
    public class SharpnessAlgorithm
    {
        /// <summary>
        /// Tenengrad梯度方法利用Sobel算子分别计算水平和垂直方向的梯度，同一场景下梯度值越高，图像越清晰。
        /// 以下是具体实现，这里衡量的指标是经过Sobel算子处理后的图像的平均灰度值，值越大，代表图像越清晰。
        /// </summary>
        /// <param name="fileName">包含文件路径的文件名</param>
        /// <returns></returns>
        public static double Tenengrad(string fileName)
        {
            Mat imageSource = CvInvoke.Imread(fileName, Emgu.CV.CvEnum.LoadImageType.Color);

            Mat imageGrey = new Mat();
            CvInvoke.CvtColor(imageSource, imageGrey, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray);

            Mat imageSobel = new Mat();
            CvInvoke.Sobel(imageGrey, imageSobel, Emgu.CV.CvEnum.DepthType.Cv16U, 1, 1);

            //图像的平均灰度
            MCvScalar scalar = CvInvoke.Mean(imageSobel);
            return scalar.ToArray()[0];
        }

        /// <summary>
        /// Laplacian梯度是另一种求图像梯度的方法
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static double Laplacian(string fileName)
        {
            Mat imageSource = CvInvoke.Imread(fileName, Emgu.CV.CvEnum.LoadImageType.Color);

            Mat imageGrey = new Mat();
            CvInvoke.CvtColor(imageSource, imageGrey, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray);

            Mat imageSobel = new Mat();
            CvInvoke.Laplacian(imageGrey, imageSobel, Emgu.CV.CvEnum.DepthType.Cv16U);
            //CvInvoke.Sobel(imageGrey, imageSobel, Emgu.CV.CvEnum.DepthType.Cv16U, 1, 1);

            //图像的平均灰度
            MCvScalar scalar = CvInvoke.Mean(imageSobel);
            return scalar.ToArray()[0];
        }

        /// <summary>
        /// 方差是概率论中用来考察一组离散数据和其期望（即数据的均值）之间的离散（偏离）成都的度量方法。
        /// 方差较大，表示这一组数据之间的偏差就较大，组内的数据有的较大，有的较小，分布不均衡；
        /// 方差较小，表示这一组数据之间的偏差较小，组内的数据之间分布平均，大小相近。
        /// 对焦清晰的图像相比对焦模糊的图像，它的数据之间的灰度差异应该更大，即它的方差应该较大，可以通过图像灰度数据的方差来衡量图像的清晰度，方差越大，表示清晰度越好。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        //public static double Variance(string fileName)
        //{
        //    Mat imageSource = CvInvoke.Imread(fileName, Emgu.CV.CvEnum.LoadImageType.Color);

        //    Mat imageGrey = new Mat();
        //    CvInvoke.CvtColor(imageSource, imageGrey, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray);

        //    Mat meanValueImage = new Mat();
        //    Mat meanStdValueImage = new Mat();
        //    //求灰度图像的标准差  
        //    CvInvoke.MeanStdDev(imageGrey, meanValueImage, meanStdValueImage);
        //    double meanValue = 0.0;
        //    meanValue = CvInvoke.mea.at<double>(0, 0);

        //    //图像的平均灰度
        //    return scalar.ToArray()[0];
        //}
    }
}
