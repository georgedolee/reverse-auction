using AuctionService.Application.Contracts.Models;
using AuctionService.Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Results;

namespace AuctionService.Application.Features.Auctions.Queries.GetPaginated;

internal sealed class GetAuctionsPaginatedQueryHandler : IRequestHandler<GetAuctionsPaginatedQuery, PagedResult<AuctionModel>>
{
    private readonly IAuctionServiceUnitOfWork _unitOfWork;
    private readonly ILogger<GetAuctionsPaginatedQueryHandler> _logger;
    public GetAuctionsPaginatedQueryHandler(
        IAuctionServiceUnitOfWork unitOfWork,
        ILogger<GetAuctionsPaginatedQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResult<AuctionModel>> Handle(GetAuctionsPaginatedQuery query, CancellationToken ct)
    {
        _logger.LogInformation("trying to fetch auctions on page {pageNumber} with size {pageSize}.",
                    query.PageNumber, query.PageSize);

        var totalCount = await _unitOfWork.Auctions.CountAsync(ct);
        var pagedAuctions = await _unitOfWork.Auctions
            .GetPagedAsync(query.PageNumber, query.PageSize, ct);

        _logger.LogInformation("Auctions on page {pageNumber} with size {pageSize} fetched successfully.",
            query.PageNumber, query.PageSize);

        return new PagedResult<AuctionModel>(
            items: pagedAuctions.Adapt<List<AuctionModel>>(),
            totalCount: totalCount,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize);
    }
}
