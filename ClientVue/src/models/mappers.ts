import { Account, GetAccountDto, PostAccountDto } from './account.model';
import { GetCategoryDto, Category, PostCategoryDto } from './category.model';
import { GetTransactionDto, Transaction, PostTransactionDto, TransactionType } from './transaction.model';
import { format } from 'date-fns';

export class AccountMapper {
    static fromGetAccountDto(dto: GetAccountDto): Account {
        const account = new Account();
        account.id = dto.id;
        account.name = dto.name;
        account.isPersonal = dto.isPersonal;
        account.createdAt = new Date(dto.createdAt);
        account.updatedAt = dto.updatedAt ? new Date(dto.updatedAt!) : undefined;
        account.currentBalance = dto.currentBalance;
        account.openingBalance = dto.openingBalance;
        account.openingDate = dto.openingDate;
        return account;
    }

    static toDto(item: Account): PostAccountDto {
        return {
            ...item
        };
    }
}

export class CategoryMapper {
    static fromGetCategoryDto(dto: GetCategoryDto): Category {
        const category = new Category();
        category.id = dto.id;
        category.name = dto.name;
        category.createdAt = new Date(dto.createdAt);
        category.updatedAt = dto.updatedAt ? new Date(dto.updatedAt!) : undefined;
        category.parent = dto.parent ? this.fromGetCategoryDto(dto.parent) : undefined;
        return category;
    }

    static toDto(item: Category): PostCategoryDto {
        return {
            ...item
        };
    }
}

export class TransactionMapper {
    static fromGetTransactionDto(dto: GetTransactionDto): Transaction {
        const transaction = new Transaction();
        transaction.id = dto.id;
        transaction.description = dto.description;
        transaction.date = new Date(dto.date);
        transaction.createdAt = new Date(dto.createdAt);
        transaction.updatedAt = dto.updatedAt ? new Date(dto.updatedAt!) : undefined;
        transaction.amount = dto.amount;
        transaction.type = dto.type as TransactionType;
        transaction.transactionDetails = dto.transactionDetails.map(detail => ({
            id: detail.id,
            amount: detail.amount,
            fromAccount: AccountMapper.fromGetAccountDto(detail.fromAccount),
            toAccount: AccountMapper.fromGetAccountDto(detail.toAccount),
            category: detail.category && detail.category.id != -1 ? CategoryMapper.fromGetCategoryDto(detail.category) : undefined,
        }));
        return transaction;
    }

    static toDto(item: Transaction): PostTransactionDto {
        return {
            date: format(item.date, "yyyy-MM-dd'T'HH:mm"),
            description: item.description,
            transactionDetails: item.transactionDetails.map(detail => ({
                id: detail.id !== -1 ? detail.id : undefined,
                amount: detail.amount,
                fromAccount: detail.fromAccount!.id,
                toAccount: detail.toAccount!.id,
                Category: (detail.category && detail.category.id !== -1) ? detail.category.id : null,
            }))
        };
    }
}
