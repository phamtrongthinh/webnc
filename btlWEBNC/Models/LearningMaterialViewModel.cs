using System.ComponentModel.DataAnnotations;

namespace btlWEBNC.Models
{
    public class LearningMaterialViewModel
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tài liệu.")]
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
