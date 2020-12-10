using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace OA.Model.Entity
{
    public class OmsRoles : IdentityRole<long>
    {
        public IEnumerable<OmsSysMenuRole> OmsSysMenuRole { get; set; }
    }
}
