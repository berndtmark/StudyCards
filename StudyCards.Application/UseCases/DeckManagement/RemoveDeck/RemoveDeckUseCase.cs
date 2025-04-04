﻿using Microsoft.Extensions.Logging;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;

namespace StudyCards.Application.UseCases.DeckManagement.RemoveDeck;

public class RemoveDeckUseCaseRequest
{
    public Guid DeckId { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
}

public class RemoveDeckUseCase(IDeckRepository deckRepository, ILogger<RemoveDeckUseCase> logger) : IUseCase<RemoveDeckUseCaseRequest, bool>
{
    public async Task<bool> Handle(RemoveDeckUseCaseRequest request)
    {
        try
        {
            await deckRepository.Remove(request.DeckId, request.EmailAddress);
            return true;
        }
        catch (Exception)
        {
            logger.LogError("Failed to remove deck {DeckId}", request.DeckId);
            return false;
        }
    }
}
