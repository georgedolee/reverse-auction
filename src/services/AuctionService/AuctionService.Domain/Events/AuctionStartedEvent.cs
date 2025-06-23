using SharedKernel.DomainEvents;

namespace AuctionService.Domain.Events;

public record AuctionStartedEvent(Guid Id, Guid AuctionId, DateTime StartedAt) : DomainEvent(Id);
