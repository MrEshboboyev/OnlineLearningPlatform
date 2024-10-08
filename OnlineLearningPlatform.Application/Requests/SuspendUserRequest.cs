namespace OnlineLearningPlatform.Application.Requests
{
    public class SuspendUserRequest
    {
        public string UserName { get; set; }
        public string Reason { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
