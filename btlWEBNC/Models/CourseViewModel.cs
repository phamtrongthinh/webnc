using System.ComponentModel.DataAnnotations;

namespace btlWEBNC.Models
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Tên khóa học là bắt buộc.")]
        [Display(Name = "Tên khóa học")]
        public string Title { get; set; } = null!;

        [Display(Name = "Mô tả khóa học")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá khóa học là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0.")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Display(Name = "Giá (VNĐ)")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Giảng viên là bắt buộc.")]
        [Display(Name = "Giảng viên")]
        public int? TeacherId { get; set; }
    }
}
