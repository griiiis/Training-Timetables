using Base.Contracts.Domain;

namespace Base.Test.BLL;

public class TestUser : IDomainEntityId
{
    public string Name { get; set; } = default!;
    public Guid Id { get; set; }
}

