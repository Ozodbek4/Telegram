export class PaginationParameters {
    PageNumber: number = 1;
    PageSize: number = 200;
    SortBy?: string | null;
    SortType?: string | null;
    search?: string | null;

    constructor(init?: Partial<PaginationParameters>) {
        Object.assign(this, init);
    }
}