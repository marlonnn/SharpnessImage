namespace SharpnessImage
{
    public class Frame
    {
        private string _fileFullName;

        private string _folder;

        public double SharpValue { get; set; }

        public string FileFullName
        {
            get
            {
                return this._fileFullName;
            }
            set
            {
                this._fileFullName = value;
            }
        }

        public string Folder
        {
            get
            {
                return this._folder;
            }
            set
            {
                this._folder = value;
            }
        }

        public Frame(string fileFullName, string folder)
        {
            this._fileFullName = fileFullName;
            this._folder = folder;
        }
    }
}
