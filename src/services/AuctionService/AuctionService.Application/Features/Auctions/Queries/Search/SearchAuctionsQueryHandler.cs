using AuctionService.Application.Contracts.Models;
using AuctionService.Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.Results;

namespace AuctionService.Application.Features.Auctions.Queries.Search;

internal sealed class SearchAuctionsQueryHandler
    : IRequestHandler<SearchAuctionsQuery, PagedResult<AuctionModel>>
{
    private readonly IAuctionServiceUnitOfWork _unitOfWork;
    private readonly ILogger<SearchAuctionsQueryHandler> _logger;

    public SearchAuctionsQueryHandler(
        IAuctionServiceUnitOfWork unitOfWork,
        ILogger<SearchAuctionsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResult<AuctionModel>> Handle(SearchAuctionsQuery query, CancellationToken ct)
    {
        _logger.LogInformation("Searching auctions with query: {@query}", query);

        var auctionsQuery = _unitOfWork.Auctions.Query(a =>
           (!query.OwnerId.HasValue || a.OwnerId == query.OwnerId) &&
           (string.IsNullOrEmpty(query.Title) || a.Title!.Contains(query.Title)) &&
           (string.IsNullOrEmpty(query.Description) || a.Description!.Contains(query.Description)) &&
           (!query.MaxDesiredPrice.HasValue || a.DesiredPrice <= query.MaxDesiredPrice) &&
           (string.IsNullOrEmpty(query.Currency) || a.Currency == query.Currency) &&
           (!query.MinStartTime.HasValue || a.StartsAt >= query.MinStartTime) &&
           (!query.MaxEndTime.HasValue || a.EndsAt <= query.MaxEndTime) &&
           (!query.MaxCurrentBidAmount.HasValue || a.LowestBidAmount <= query.MaxCurrentBidAmount) &&
           (!query.Status.HasValue || a.Status == query.Status)
        );

        var totalCount = await auctionsQuery.CountAsync(ct);
        var pagedAuctions = await _unitOfWork.Auctions
            .GetPagedAsync(query.PageNumber, query.PageSize, auctionsQuery, ct);

        _logger.LogInformation("Auctions fetched successfully.");
        return new PagedResult<AuctionModel>(
            items: pagedAuctions.Adapt<List<AuctionModel>>(),
            totalCount: totalCount,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize);
    }
}
