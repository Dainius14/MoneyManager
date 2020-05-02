import { isValid } from 'date-fns';

export function positiveNumber(msg: string) {
    return (value: string) => parseFloat(value) > 0 || msg;
}

export function number(msg: string) {
    return (value: string) => (value !== '' && !isNaN(value as any)) || msg;
}

export function date(msg: string) {
    return (value: string) => isValid(new Date(value)) || msg;
}

export function time(msg: string) {
    return (value: string) => isValid(new Date('2020-01-01 ' + value)) || msg;
}

export function maxLength(length: number, msg: string) {
    return (value: string) => value?.length <= length || msg;
}

export function notEmpty(msg: string) {
    return (value: string) => !!value || msg;
}

const emailRegex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/i;
export function email(msg: string) {
    return (value: string) => emailRegex.test(value) || msg;
}