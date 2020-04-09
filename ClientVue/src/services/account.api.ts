import api from '@/services/api';
import { Account, GetAccountDto } from '@/models/account.model';
import { AccountMapper } from '@/models/mappers';

export class AccountApi {
    static async getAccounts(): Promise<Account[]> {
        const dtos = await api.get<GetAccountDto[]>('/accounts');
        return dtos.map((dto) => {
            return AccountMapper.fromGetAccountDto(dto);
        });
    }
    
    static async createAccount(account: Account): Promise<Account> {
        const sendDto = AccountMapper.toDto(account);
        const dto = await api.post<GetAccountDto>('/accounts', sendDto);
        return AccountMapper.fromGetAccountDto(dto);
    }
    
    static async editAccount(account: Account): Promise<Account> {
        const sendDto = AccountMapper.toDto(account);
        const getDto = await api.put<GetAccountDto>('/accounts/' + account.id, sendDto);
        return AccountMapper.fromGetAccountDto(getDto);
    }
    
    static async deleteAccount(id: number): Promise<boolean> {
        return await api.delete('/accounts/' + id);
    }
}
