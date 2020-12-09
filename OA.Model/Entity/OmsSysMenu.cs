using System;
using System.Collections.Generic;
using System.Text;

namespace OA.Model.Entity
{
    public class OmsSysMenu
    {
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public int ParentID { get; set; }
        public int Depth { get; set; }
        public string ParentPath { get; set; }
    }
}
