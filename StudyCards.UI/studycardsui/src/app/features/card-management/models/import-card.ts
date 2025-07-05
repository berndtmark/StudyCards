export class ImportCard {
    cardFront?: string
    cardBack?: string;

    static isValid(data: any): data is ImportCard[] {
        return (
        Array.isArray(data) &&
        data.every(
            (item) =>
            typeof item === 'object' &&
            typeof item.cardFront === 'string' &&
            typeof item.cardBack === 'string'
        )
        );
    }
}
