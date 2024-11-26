namespace btlWEBNC.Models
{
    public class CourseDetailViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double   Price { get; set; }
        public string TeacherName { get; set; }
        public List<LearningMaterialViewModel2> LearningMaterials { get; set; } = new List<LearningMaterialViewModel2>();

    }
}
