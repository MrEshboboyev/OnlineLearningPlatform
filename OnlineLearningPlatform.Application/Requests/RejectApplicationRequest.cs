namespace OnlineLearningPlatform.Application.Requests;

public class RejectApplicationRequest
{
    public Guid ApplicationId { get; set; }
    public DateTime ReapplyDate { get; set; }
}
