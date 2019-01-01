using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace D3.Blog.Application.ViewModels
{
    /// <summary>
    /// Product 视图模型
    /// </summary>
    public class CustomerViewModel
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// customer名称
        /// </summary>
        [Required(ErrorMessage = "名称是必需的")]
        [MinLength(2)]
        [MaxLength(100)]
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// customer邮箱
        /// </summary>
        [Required(ErrorMessage = "电子邮件是必需的")]
        [EmailAddress]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        /// <summary>
        /// customer出生日期
        /// </summary>
        [Required(ErrorMessage                = "出生日期是必需的")]
        [DisplayFormat(ApplyFormatInEditMode  = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date, ErrorMessage = "日期格式无效")]
        [DisplayName("Birth Date")]
        public DateTime BirthDate { get; set; }
    }
}
