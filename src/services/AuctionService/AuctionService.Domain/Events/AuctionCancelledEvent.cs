using SharedKernel.DomainEvents;

namespace AuctionService.Domain.Events;

public record class AuctionCancelledEvent(Guid id, Guid AuctionId, DateTime CancelledAt) 
    : DomainEvent(id);
