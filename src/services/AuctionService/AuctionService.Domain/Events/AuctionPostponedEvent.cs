using SharedKernel.DomainEvents;

namespace AuctionService.Domain.Events;

public record AuctionPostponedEvent(Guid Id, Guid AuctionId, DateTime NewStartTime, DateTime NewEndTime) : DomainEvent(Id);