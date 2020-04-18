export function toIsoDate(date: Date): string {
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${date.getFullYear()}-${month}-${day}`;
}

export function isEmpty(obj: object): boolean {
    return obj == null || Object.keys(obj).length === 0;
}