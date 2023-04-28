namespace App.DTO;

public abstract record BaseEntitySubmissionData(Guid SubmitterId, bool AutoSubmit);

public record EntitySubmissionDataWithUrl(string Url, Guid SubmitterId, bool AutoSubmit) :
    BaseEntitySubmissionData(SubmitterId, AutoSubmit);

public record EntitySubmissionDataWithId(string Id, Guid SubmitterId, bool AutoSubmit) :
    BaseEntitySubmissionData(SubmitterId, AutoSubmit);

public record VideoSubmissionDataWithId(string Id, Guid SubmitterId, bool AutoSubmit) :
    EntitySubmissionDataWithId(Id, SubmitterId, AutoSubmit);

public record AuthorSubmissionDataWithId(string Id, Guid SubmitterId, bool AutoSubmit) :
    EntitySubmissionDataWithId(Id, SubmitterId, AutoSubmit);

public record PlaylistSubmissionDataWithId(string Id, Guid SubmitterId, bool AutoSubmit) :
    EntitySubmissionDataWithId(Id, SubmitterId, AutoSubmit);