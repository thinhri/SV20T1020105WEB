﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020105.DomainModels
{
    /// <summary>
    /// Loai hang
    /// </summary>
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = "";    
        public string Description { get; set; } = "";
        public string Photo { get; set; } = "";
    }
}
