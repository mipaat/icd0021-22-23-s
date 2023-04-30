namespace App.DTO;

public class UrlSubmissionResults : List<UrlSubmissionResult>
{
    public bool ContainsNonArchivedPlaylist = false;
}