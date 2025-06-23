using MediatR;

namespace SharedKernel.DomainEvents;

public record DomainEvent(Guid Id) : INotification;