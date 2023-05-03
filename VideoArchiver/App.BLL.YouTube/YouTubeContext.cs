namespace App.BLL.YouTube;

public class YouTubeContext
{
    public event EventHandler<string>? NewCommentsQueued;
    public void QueueNewComments(string videoId) => Task.Run(() => NewCommentsQueued?.Invoke(null, videoId));
}