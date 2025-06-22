export interface Deck {
    id?: string;
    cardCount?: number | null;
    createdDate?: string;
    deckName?: string;
    deckReviewStatus?: DeckReviewStatus;
    deckSettings?: DeckSettings;
    description?: string | null;
    hasReviewsToday?: boolean;
    updatedDate?: string;
    userEmail?: string;
}

export interface DeckSettings {
  newCardsPerDay?: number;
  reviewsPerDay?: number;
}

export interface DeckReviewStatus {
  lastReview?: string;
  reviewCount?: number;
}