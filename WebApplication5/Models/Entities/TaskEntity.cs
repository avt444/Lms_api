namespace WebApplication5.Models.Entities
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string? Courseid { get; set; }
        public string? Coursename { get; set; }
        public string? Courseduration { get; set; }
        public string? Faculty { get; set; }
        public string? Amount { get; set; }
        public string? Description { get; set; }
        public DateTime? startdate { get; set; }
        
    }
}
