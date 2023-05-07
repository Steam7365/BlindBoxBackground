
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BilndBox.Dto.Entity
{
    /// <summary>
    /// 员工信息
    /// </summary>
    public class StaffDto
    {
        /// <summary>
        /// 员工编号
        /// </summary>
        public int StaffId { get; set; }

        [StringLength(20, MinimumLength = 2,ErrorMessage ="姓名长度不能小于2")]
        /// <summary>
        /// 姓名
        /// </summary>
        public string StaffName { get; set; }

        [RegularExpression(@"^(?:[1-9]\d*|0)(?:\.\d+)?$",ErrorMessage ="请输入合理的工资")]
        /// <summary>
        /// 工资
        /// </summary>
        public decimal StaffWages { get; set; }

        [RegularExpression(@"^(?:男|女)$",ErrorMessage ="只能为男或者女！")]
        [Required(ErrorMessage ="不能为空")]
        /// <summary>
        /// 性别
        /// </summary>
        public string StaffGender { get; set; }

        [Phone(ErrorMessage ="请输入正确的手机号")]
        [Required(ErrorMessage = "不能为空")]
        /// <summary>
        /// 手机号
        /// </summary>
        public string StaffPhone { get; set; }

        [RegularExpression(@"^(?:\d{15}|\d{17}[\dXx])$",ErrorMessage ="请输入正确的身份证")]
        [Required(ErrorMessage = "不能为空")]
        /// <summary>
        /// 身份证
        /// </summary>
        public string StaffCode { get; set; }
        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime StaffEntryTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 职位
        /// </summary>
        public string StaffPosition { get; set; } = "员工";
        /// <summary>
        /// 状态
        /// </summary>
        public string? StaffState { get; set; } = "在职";
        /// <summary>
        /// 头像
        /// </summary>
        public string? Image { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string? Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string? City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public string? Area { get; set; }
        /// <summary>
        /// 详情地址
        /// </summary>
        public string? Details { get; set; }
    }
}
