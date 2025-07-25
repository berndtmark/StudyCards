﻿namespace StudyCards.Api.Models.Response;

public class CardResponse
{
    public Guid Id { get; init; } 
    public Guid DeckId { get; init; }
    public string CardFront { get; init; } = string.Empty;
    public string CardBack { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; init; } = DateTime.UtcNow;
    public DateTime? NextReviewDate { get; init; } = null;
    public int ReviewCount { get; init; } = 0;
    public string? ReviewPhase { get; init; }
}
