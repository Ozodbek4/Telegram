export class PaginationParameters {
    PageNumber: number = 1;
    PageSize: number = 200;
    SortBy?: string;
    SortType?: string;
    search?: string;

    constructor(init?: Partial<PaginationParameters>) {
        Object.assign(this, init);
    }
}