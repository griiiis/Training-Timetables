using Base.Contracts.Domain;
using Base.Domain;
using TestUser = Base.Test.DAL.TestUser;

namespace Base.Test.BLL;

public class TestEntity : BaseEntityId, IDomainAppUserId
{
    public string Value { get; set; } = default!;
    public Guid AppUserId { get; set; }
    public TestUser? AppUser { get; set; }
    
    public override bool Equals(object? obj)
    {
        var other = obj as TestEntity;
        
        return other != null && Id == other.Id && Value == other.Value && AppUserId == other.AppUserId;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Value, AppUserId);
    }
}

