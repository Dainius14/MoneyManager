export function positiveNumber(msg: string) {
    return (value: string) => parseFloat(value) >= 0 || msg;
}

export function number(msg: string) {
    return (value: string) => (value !== '' && !isNaN(value as any)) || msg;
}
