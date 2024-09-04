using Base.Contracts.Domain;
using Base.Domain;
using Microsoft.AspNetCore.Identity;

namespace Base.Test.Domain;

public class TestUser : IDomainEntityId
{
    public string Name { get; set; } = default!;
    public Guid Id { get; set; }
}

