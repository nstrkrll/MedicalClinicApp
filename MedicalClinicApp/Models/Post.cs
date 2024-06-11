using System.ComponentModel.DataAnnotations;

namespace MedicalClinicApp.Models
{
    public class Post
    {
        [Key]
        public int? PostId { get; set; }
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
