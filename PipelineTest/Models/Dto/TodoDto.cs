namespace PipelineTest.Models.Dto
{
    public class TodoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
