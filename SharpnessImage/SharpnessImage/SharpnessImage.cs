using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpnessImage
{
    public class SharpnessImage
    {
        private string fileFullName;
        public string FileFullName
        {
            get
            {
                return this.fileFullName;
            }
            set
            {
                this.fileFullName = value;
            }
        }

        private double sharpnessValue;
        public double SharpnessValue
        {
            get
            {
                return this.sharpnessValue;
            }
            set
            {
                this.sharpnessValue = value;
            }
        }

        public SharpnessImage(string fileFullName, double sharpnessValue)
        {
            this.fileFullName = fileFullName;
            this.sharpnessValue = sharpnessValue;
        }
    }
}
