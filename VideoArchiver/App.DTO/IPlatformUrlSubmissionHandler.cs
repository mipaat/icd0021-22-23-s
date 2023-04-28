namespace App.DTO;

public interface IPlatformUrlSubmissionHandler
{
    public bool IsPlatformUrl(string url);
    public Task<EntityOrQueueItem> SubmitUrl(string url, Guid submitterId, bool autoSubmit);
}