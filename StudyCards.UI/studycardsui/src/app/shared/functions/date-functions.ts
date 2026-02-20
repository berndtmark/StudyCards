export class DateFuctions {
  static isToday(date: Date): boolean {
    const today = new Date();
    return (
      date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear()
    );
  }

  static today(): Date {
    const now = new Date();
    return new Date(now.getFullYear(), now.getMonth(), now.getDate());
  }

  static tomorrow(): Date {
    const now = new Date();
    return new Date(now.getFullYear(), now.getMonth(), now.getDate() + 1);
  }

  static oneYearAgo(): Date {
    const now = new Date();
    return new Date(now.getFullYear() - 1, now.getMonth(), now.getDate());
  }
}