using SharedKernel.DomainEvents;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Entities;

public class EntityWithDomainEvents : BaseEntity, IHasDomainEvents
{
    [NotMapped]
    private readonly List<DomainEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
     
    protected void RaiseDomainEvent(DomainEvent domainEvent) 
        => _domainEvents.Add(domainEvent);
}
