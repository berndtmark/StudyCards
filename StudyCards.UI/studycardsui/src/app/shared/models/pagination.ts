export class Pagination {
    totalCount: number;
    pageNumber: number;
    pageSize: number;

    constructor(totalCount: number, pageNumber: number, pageSize: number) {
        this.totalCount = totalCount;
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
    }
}