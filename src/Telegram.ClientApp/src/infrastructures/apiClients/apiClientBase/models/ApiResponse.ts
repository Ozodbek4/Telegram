import type { ProblemDetails } from "./ProblemDetails";

export class ApiResponse<T> {
    public response: T | null;
    public problem: ProblemDetails | null;
    public status: number;

    constructor(response: T | null, problem: ProblemDetails | null, status: number) {
        this.response = response;
        this.problem = problem;
        this.status = status;
    }

    public get isSuccess(): boolean {
        return this.status >= 200 && this.status < 300;
    }
}