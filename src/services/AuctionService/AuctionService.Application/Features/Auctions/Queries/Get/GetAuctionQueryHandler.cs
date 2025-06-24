using AuctionService.Application.Contracts.Models;
using AuctionService.Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Guards;
using SharedKernel.Interfaces;

namespace AuctionService.Application.Features.Auctions.Queries.Get;

internal sealed class GetAuctionQueryHandler : IRequestHandler<GetAuctionQuery, AuctionModel>
{
    private readonly IAuctionServiceUnitOfWork _unitOfWork;
    private readonly ILogger<GetAuctionQueryHandler> _logger;
    public GetAuctionQueryHandler(
        IAuctionServiceUnitOfWork unitOfWork,
        ILogger<GetAuctionQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<AuctionModel> Handle(GetAuctionQuery query, CancellationToken ct)
    {
        _logger.LogInformation("Trying to fetch auction with id: {id}", query.Id);
        var auction = await _unitOfWork.Auctions.GetAsync(query.Id, ct);

        Guard.EnsureFound(auction, nameof(auction), query.Id, _logger);

        _logger.LogInformation("Auction with id: {id} fetched successfully", query.Id);
        return auction.Adapt<AuctionModel>();
    }
}
