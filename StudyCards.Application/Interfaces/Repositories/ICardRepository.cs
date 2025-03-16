﻿using StudyCards.Domain.Entities;

namespace StudyCards.Application.Interfaces.Repositories;

public interface ICardRepository
{
    Task<Card?> Get(Guid id);
    Task<IEnumerable<Card>> GetByDeck(Guid deckId);
    Task<Card> Add(Card card);
    Task<Card> Update(Card card);
}
