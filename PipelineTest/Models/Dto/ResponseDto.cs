namespace PipelineTest.Models.Dto
{
    public class ResponseDto
    {
        public int StatusCode { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public object? Result { get; set; }
    }
}
