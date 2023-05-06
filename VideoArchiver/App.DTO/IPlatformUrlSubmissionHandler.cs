namespace App.DTO;

public interface IPlatformUrlSubmissionHandler
{
    public bool IsPlatformUrl(string url);
    public Task<UrlSubmissionResults> SubmitUrl(string url, Guid submitterId, bool autoSubmit, bool alsoSubmitPlaylist = false);
}