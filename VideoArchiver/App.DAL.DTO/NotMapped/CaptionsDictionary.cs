namespace App.DAL.DTO.NotMapped;

public class CaptionsDictionary : Dictionary<string, List<Caption>>
{
    public bool Equals(CaptionsDictionary other)
    {
        if (Keys.Count != other.Keys.Count) return false;
        foreach (var key in Keys)
        {
            var captions1 = this[key];
            if (other.TryGetValue(key, out var captions2))
            {
                if (!AreCaptionsListsEqual(captions1, captions2)) return false;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private static bool AreCaptionsListsEqual(List<Caption> captions1, List<Caption> captions2)
    {
        if (captions1.Count != captions2.Count) return false;
        foreach (var caption in captions1)
        {
            if (!captions2.Any(caption.Equals)) return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            foreach (var kvp in this.OrderBy(kvp => kvp.Key))
            {
                hash = hash * 23 + kvp.Key.GetHashCode();
                var listHash = kvp.Value
                    .Select(caption => caption.GetCustomHashCode())
                    .OrderBy(h => h)
                    .Aggregate(17, (current, next) => current * 23 + next);
                hash = hash * 23 + listHash;
            }
            return hash;
        }
    }

    public CaptionsDictionary GetSnapShot()
    {
        var snapshot = new CaptionsDictionary();
        foreach (var kvp in this)
        {
            var newList = kvp.Value.Select(caption => caption.Clone()).ToList();
            snapshot.Add(kvp.Key, newList);
        }

        return snapshot;
    } 
}