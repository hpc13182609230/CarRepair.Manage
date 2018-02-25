namespace EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class Area : BaseEntity
    {
        public string codeID { get; set; }
        public string name { get; set; }
        public string parentID { get; set; }
    }

    //[MetadataType(typeof(AreaMD))]
    //public partial class Area
    //{

    //}

    //public class AreaMD : BaseEntity
    //{

    //}
}

