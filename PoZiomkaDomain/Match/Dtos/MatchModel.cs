namespace PoZiomkaDomain.Match.Dtos;

public enum MatchStatus
{
    Accepted,
    Rejected
}

public record MatchModel(int Id, int StudentId1, int StudentId2, MatchStatus Status1, MatchStatus Status2)
{
    public bool IsAccepted { get => Status1 == MatchStatus.Accepted && Status2 == MatchStatus.Accepted; }
    public bool IsRejected { get => Status1 == MatchStatus.Rejected || Status2 == MatchStatus.Rejected; }
};
