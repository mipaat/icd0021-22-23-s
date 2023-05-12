namespace App.BLL.DTO.Entities;

public class UrlSubmissionResults : List<UrlSubmissionResult>
{
    public bool ContainsNonArchivedPlaylist = false;
}