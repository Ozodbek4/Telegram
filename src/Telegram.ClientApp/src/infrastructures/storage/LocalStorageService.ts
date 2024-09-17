export class LocalStorageService{
    public get<T>(key: string): T | null{
        const item = localStorage.getItem(key);
        return item ? JSON.parse(item) as T : null;
    }

    public set<T>(key: string, value: T): void {
        localStorage.setItem(key, JSON.stringify(value));
    }

    public remove(key: string): void {
        localStorage.removeItem(key);
    }

    public clear(key: string): void{
        localStorage.clear();
    }
}