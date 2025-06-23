using SharedKernel.DomainEvents;

namespace AuctionService.Domain.Events;

public record AuctionEndedEvent(Guid Id, Guid AuctionId, DateTime EndedAt) : DomainEvent(Id);