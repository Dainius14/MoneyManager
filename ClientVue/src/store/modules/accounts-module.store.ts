import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { Account } from '@/models/account.model';
import { AccountApi } from '@/services/account.api';
import store from '@/store';
import { LoadState } from '@/models/common.models';

const name = 'accounts';
if ((store as any).state[name]) {
    store.unregisterModule(name);
}

@Module({ name, store, dynamic: true, namespaced: true })
class Accounts extends VuexModule {
    loadState: LoadState = LoadState.NotLoaded;
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
    private _addItems(items: Account[]) {
        this.accounts = this.accounts.concat(items);
    }

    @Mutation
    private _removeItem(item: Account) {
        const index = this.accounts.indexOf(item);
        this.accounts.splice(index, 1);
    }
    @Mutation
    private _replaceItem(newItem: Account) {
        const index = this.accounts.findIndex(a => a.id === newItem.id);
        this.accounts.splice(index, 1, newItem);
    }

    @Mutation
    private _clear() {
        this.accounts.splice(0, this.accounts.length);
    }
    @Mutation
    private _setState(state: LoadState) {
        this.loadState = state;
    }


    @Action({ rawError: true })
    async getAccounts() {
        this._setState(LoadState.Loading);
        try {
            this._addItems(await AccountApi.getAccounts());
            this._setState(LoadState.Loaded);
        }
        catch (ex) {
            this._setState(LoadState.NotLoaded);
            throw ex;
        }
    }

    @Action({ rawError: true })
    async removeAccount(item: Account) {
        if (await AccountApi.deleteAccount(item.id)) {
            this._removeItem(item);
        }
    }

    @Action({ rawError: true })
    async editAccount(item: Account) {
        const updated = await AccountApi.editAccount(item);
        this._replaceItem(updated);
    }

    @Action({ rawError: true })
    async createAccount(item: Account) {
        const created = await AccountApi.createAccount(item);
        this._addItems([created]);
    }

}

export const AccountsModule = getModule(Accounts);
