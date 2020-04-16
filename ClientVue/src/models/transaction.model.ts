import { Account, GetAccountDto } from './account.model';
import { Category, GetCategoryDto } from './category.model';
import { IListItem } from '@/components/list/list-item.model';

export enum TransactionType {
    Expense = 'expense',
    Income = 'income',
    Transfer = 'transfer',
    Null = 'null',
}

export class TransactionDetails {
    id: number = -1;
    amount: number = 0;
    fromAccount: Account | null = null;
    toAccount: Account | null = null;
    category?: Category;
}

export class Transaction implements IListItem {
    id: number = -1;
    description: string = '';
    date: Date = new Date();
    amount: number = 0;
    type: TransactionType = TransactionType.Null;
    transactionDetails: TransactionDetails[] = [new TransactionDetails()];
    createdAt: Date = new Date();
    updatedAt?: Date;

    toString() {
        return this.date.toDateString();
    }
}

export interface GetTransactionDto {
    id: number;
    description: string;
    date: string;
    createdAt: Date;
    updatedAt?: Date;
    type: string;
    amount: number;

    transactionDetails: {
        id: number;
        amount: number;
        fromAccount: GetAccountDto;
        toAccount: GetAccountDto;
        category?: GetCategoryDto;
    }[];
}


export interface PostTransactionDto {
    description: string;
    date: string;

    transactionDetails: {
        id?: number;
        amount: number;
        fromAccount: number;
        toAccount: number;
        category: number | null;
    }[];
}
