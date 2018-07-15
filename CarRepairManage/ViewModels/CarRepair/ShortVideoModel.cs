using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class ShortVideoModel : BaseModel
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _creator;
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }

        private string _filekey;
        public string FileKey
        {
            get { return _filekey; }
            set { _filekey = value; }
        }
        private string _filehash;
        public string FileHash
        {
            get { return _filehash; }
            set { _filehash = value; }
        }

        private string _url;
        public string URL
        {
            get { return _url; }
            set { _url = value; }
        }

        private int _size;
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        private int _hitcount;
        public int HitCount
        {
            get { return _hitcount; }
            set { _hitcount = value; }
        }

        private int _collectcount;
        public int CollectCount
        {
            get { return _collectcount; }
            set { _collectcount = value; }
        }

        private int _sharecount;
        public int ShareCount
        {
            get { return _sharecount; }
            set { _sharecount = value; }
        }

        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}
