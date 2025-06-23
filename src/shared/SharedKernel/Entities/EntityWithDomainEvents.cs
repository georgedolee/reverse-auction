using SharedKernel.DomainEvents;

namespace SharedKernel.Entities;

public class EntityWithDomainEvents : BaseEntity, IHasDomainEvents
{
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
     
    protected void RaiseDomainEvent(DomainEvent domainEvent) 
        => _domainEvents.Add(domainEvent);
}
