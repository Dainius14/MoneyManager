import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { Account } from '@/models/account.model';
import { AccountApi } from '@/services/account.api';
import store from '@/store';

const name = 'accounts';
if ((store as any).state[name]) {
    store.unregisterModule(name);
}

@Module({ name, store, dynamic: true, namespaced: true })
class Accounts extends VuexModule {
    loaded: boolean = false;
    accounts: Account[] = [];

    get accountsSorted() {
        return [...this.accounts].sort((a, b) => a.name.localeCompare(b.name));
    }

    get personalAccounts() {
        return this.accountsSorted.filter(account => account.isPersonal);
    }

    get otherAccounts() {
        return this.accountsSorted.filter(account => !account.isPersonal);
    }

    @Mutation
    addItems(items: Account[]) {
        this.accounts = this.accounts.concat(items);
        this.loaded = true;
    }
    @Mutation
    removeItem(item: Account) {
        const index = this.accounts.indexOf(item);
        this.accounts.splice(index, 1);
    }
    @Mutation
    replaceItem(newItem: Account) {
        const index = this.accounts.findIndex(a => a.id === newItem.id);
        this.accounts.splice(index, 1, newItem);
    }

    @Mutation
    clear() {
        this.accounts.splice(0, this.accounts.length);
    }

    @Action({ rawError: true })
    async getAccounts() {
        this.clear();
        this.addItems(await AccountApi.getAccounts());
    }

    @Action({ rawError: true })
    async removeAccount(item: Account) {
        if (await AccountApi.deleteAccount(item.id)) {
            this.removeItem(item);
        }
    }

    @Action({ rawError: true })
    async editAccount(item: Account) {
        const updated = await AccountApi.editAccount(item);
        this.replaceItem(updated);
    }

    @Action({ rawError: true })
    async createAccount(item: Account) {
        const created = await AccountApi.createAccount(item);
        this.addItems([created]);
    }

}

export const AccountsModule = getModule(Accounts);
