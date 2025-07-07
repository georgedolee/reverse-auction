using AuctionService.Application.Contracts.Models;
using AuctionService.Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Guards;

namespace AuctionService.Application.Features.Bids.Queries.Get;

internal sealed class GetBidQueryHandler : IRequestHandler<GetBidQuery, BidModel>
{
    private readonly IAuctionServiceUnitOfWork _unitOfWork;
    private readonly ILogger<GetBidQueryHandler> _logger;
    public GetBidQueryHandler(
        IAuctionServiceUnitOfWork unitOfWork,
        ILogger<GetBidQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<BidModel> Handle(GetBidQuery query, CancellationToken ct)
    {
        _logger.LogInformation("Trying to get bid by id: {id}", query.Id);
        var bid = await _unitOfWork.Bids.GetAsync(query.Id, ct);

        Guard.EnsureFound(bid, nameof(bid), query.Id, _logger);

        _logger.LogInformation("Bid by id: {id} fetched successfully.", query.Id);
        return bid.Adapt<BidModel>();
    }
}
