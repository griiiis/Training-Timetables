using Base.Contracts.Domain;

namespace Base.Test.DAL;

public class TestUser : IDomainEntityId
{
    public string Name { get; set; } = default!;
    public Guid Id { get; set; }
}

