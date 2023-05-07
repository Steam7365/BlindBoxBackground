using System.ComponentModel.DataAnnotations;

namespace BilndBox.Dto.Entity
{
    /// <summary>
    /// 盲盒商品等级表
    /// </summary>
    public class GradeDto
    {
        public int GradeId { get; set; }

        [StringLength(6, MinimumLength = 1, ErrorMessage = "分类名称长度不能小于1")]
        public string GradeName { get; set; }
    }
}
