using AuctionService.Application.Contracts.Models;
using AuctionService.Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Results;

namespace AuctionService.Application.Features.Bids.Queries.Search;

internal sealed class SearchBidsQueryHandler : IRequestHandler<SearchBidsQuery, PagedResult<BidModel>>
{
    private readonly IAuctionServiceUnitOfWork _unitOfWork;
    private readonly ILogger<SearchBidsQueryHandler> _logger;

    public SearchBidsQueryHandler(
        IAuctionServiceUnitOfWork unitOfWork,
        ILogger<SearchBidsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResult<BidModel>> Handle(SearchBidsQuery query, CancellationToken ct)
    {
        _logger.LogInformation("Searching bids with query: {@query}", query);

        var bidsQuery = _unitOfWork.Bids.Query(a =>
           (!query.AuctionId.HasValue || a.AuctionId == query.AuctionId) &&
           (!query.BidderId.HasValue || a.BidderId == query.BidderId) &&
           (!query.MaxAmount.HasValue || a.Amount <= query.MaxAmount) &&
           (!query.MinAmount.HasValue || a.Amount >= query.MinAmount)
        );

        var totalCount = await _unitOfWork.Bids.CountAsync(ct);
        var pagedBids = await _unitOfWork.Bids
            .GetPagedAsync(query.PageNumber, query.PageSize, bidsQuery, ct);

        _logger.LogInformation("Bids fetched successfully.");
        return new PagedResult<BidModel>(
            items: pagedBids.Adapt<List<BidModel>>(),
            totalCount: totalCount,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize);
    }
}
