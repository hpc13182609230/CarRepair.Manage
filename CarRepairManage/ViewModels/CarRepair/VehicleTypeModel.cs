using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CarRepair
{
    public class VehicleTypeModel : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Name_PY
        /// </summary>		
        private string _name_py;
        public string Name_PY
        {
            get { return _name_py; }
            set { _name_py = value; }
        }
        /// <summary>
        /// Name_FC
        /// </summary>		
        private string _name_fc;
        public string Name_FC
        {
            get { return _name_fc; }
            set { _name_fc = value; }
        }
        /// <summary>
        /// Remark
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
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
    }
}
