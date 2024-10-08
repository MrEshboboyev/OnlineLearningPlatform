namespace OnlineLearningPlatform.Application.Requests
{
    public class ResetPasswordRequest
    {
        public string UserName { get; set; }
        public string NewPassword { get; set; }
    }
}
