import api from '@/services/api';
import { TransactionMapper } from '@/models/mappers';
import { Transaction, GetTransactionDto } from '@/models/transaction.model';
import { ImportTransactionsResults } from '@/models/import-transactions-results.model';

export class TransactionsApi {
    static async getTransactions(page: number): Promise<Transaction[]> {
        const dtos = await api.get<GetTransactionDto[]>('/transactions', { page });
        return dtos.map((dto) => {
            return TransactionMapper.fromGetTransactionDto(dto);
        });
    }
    
    static async createTransaction(transaction: Transaction): Promise<Transaction> {
        const sendDto = TransactionMapper.toDto(transaction);
        const dto = await api.post<GetTransactionDto>('/transactions', sendDto);
        return TransactionMapper.fromGetTransactionDto(dto);
    }
    
    static async editTransaction(transaction: Transaction): Promise<Transaction> {
        const sendDto = TransactionMapper.toDto(transaction);
        const getDto = await api.put<GetTransactionDto>('/transactions/' + transaction.id, sendDto);
        return TransactionMapper.fromGetTransactionDto(getDto);
    }
    
    static async deleteTransaction(id: number): Promise<boolean> {
        return await api.delete('/transactions/' + id);
    }

    static async uploadFile(file: File): Promise<ImportTransactionsResults> {
        const formData = new FormData();
        formData.append('file', file);
        return await api.post<ImportTransactionsResults>('/transactions/import', formData);
    }
}
