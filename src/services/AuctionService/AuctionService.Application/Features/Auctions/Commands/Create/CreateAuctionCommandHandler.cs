using AuctionService.Application.Contracts.Models;
using AuctionService.Domain.Entities;
using AuctionService.Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuctionService.Application.Features.Auctions.Commands.Create;

internal sealed class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, AuctionModel>
{
    private readonly IAuctionServiceUnitOfWork _unitOfWork;
    private readonly ILogger<CreateAuctionCommandHandler> _logger;
    public CreateAuctionCommandHandler(
        IAuctionServiceUnitOfWork unitOfWork,
        ILogger<CreateAuctionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<AuctionModel> Handle(CreateAuctionCommand command, CancellationToken ct)
    {
        _logger.LogInformation("Initiating auction creation with properties: {@auction}", command);

        var newAuction = command.Adapt<Auction>();

        await _unitOfWork.Auctions.AddAsync(newAuction, ct);
        await _unitOfWork.CommitAsync(ct);

        _logger.LogInformation("New auction created successfully.");
        return newAuction.Adapt<AuctionModel>();
    }
}
