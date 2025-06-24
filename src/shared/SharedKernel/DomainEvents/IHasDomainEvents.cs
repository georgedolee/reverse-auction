using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.DomainEvents;

public interface IHasDomainEvents
{
    [NotMapped]
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }
}

