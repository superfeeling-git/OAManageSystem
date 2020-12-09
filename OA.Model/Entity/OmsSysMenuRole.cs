using System;
using System.Collections.Generic;
using System.Text;

namespace OA.Model.Entity
{
    public class OmsSysMenuRole
    {
        public int ID { get; set; }
        public OmsSysMenu OmsSysMenu { get; set; }
        public OmsRoles OmsRoles { get; set; }
    }
}
