using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class PackageGameTypeTime : IDomainEntityId
{
    public string PackageGtName { get; set; } = default!;
    
    public Guid GameTypeId { get; set; }
    public GameType? GameType { get; set; }
    
    public decimal Times { get; set; }
    public Guid Id { get; set; }
}