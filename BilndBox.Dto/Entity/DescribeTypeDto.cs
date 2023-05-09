namespace BilndBox.Dto.Entity
{
    /// <summary>
    /// 商品描述
    /// </summary>
    public class DescribeTypeDto
    {
        public int DescribeTypeId { get; set; }
        /// <summary>
        /// 描述标题
        /// </summary>
        public string DescTitle { get; set; }
        /// <summary>
        /// 描述内容
        /// </summary>
        public string DescContent { get; set; }
    }
}
