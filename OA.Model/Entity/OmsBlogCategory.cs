using System;
using System.Collections.Generic;
using System.Text;

namespace OA.Model.Entity
{
    /// <summary>
    /// 博客分类实体
    /// </summary>
    public class OmsBlogCategory
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 分类排序
        /// </summary>
        public int CategoryOrder { get; set; }
    }
}
