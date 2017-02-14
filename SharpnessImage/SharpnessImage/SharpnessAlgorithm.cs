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
    }
}
