import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { Transaction } from '@/models/transaction.model';
import { TransactionsApi } from '@/services/transactions.api';
import store from '@/store';
import { LoadState } from '@/models/common.models';

const name = 'transactions';
if ((store as any).state[name]) {
    store.unregisterModule(name);
}

@Module({ name, store, dynamic: true, namespaced: true })
class Transactions extends VuexModule {
    transactions: Transaction[] = [];
    pagesLoaded: Set<number> = new Set();
    loadState: LoadState = LoadState.NotLoaded;

    @Mutation
    private _addItems(items: Transaction[]) {
        this.transactions = this.transactions.concat(items);
    }
    @Mutation
    private _removeItem(item: Transaction) {
        const index = this.transactions.indexOf(item);
        this.transactions.splice(index, 1);
    }
    @Mutation
    private _replaceItem(newItem: Transaction) {
        const index = this.transactions.findIndex(a => a.id === newItem.id);
        this.transactions.splice(index, 1, newItem);
    }

    @Mutation
    private _clear() {
        this.transactions.splice(0, this.transactions.length);
    }

    @Mutation
    private _setState(state: LoadState) {
        this.loadState = state;
    }

    @Mutation
    private _addLoadedPage(page: number) {
        this.pagesLoaded.add(page);
    }

    @Action({ rawError: true })
    async getTransactions({ page }: { page: number }) {
        this._setState(LoadState.Loading);
        try {
            this._addItems(await TransactionsApi.getTransactions(page));
            this._setState(LoadState.Loaded);
        }
        catch (ex) {
            this._setState(LoadState.NotLoaded);
            throw ex;
        }
    }

    @Action({ rawError: true })
    async removeTransaction(item: Transaction) {
        if (await TransactionsApi.deleteTransaction(item.id)) {
            this._removeItem(item);
        }
    }

    @Action({ rawError: true })
    async editTransaction(item: Transaction) {
        const updated = await TransactionsApi.editTransaction(item);
        this._replaceItem(updated);
    }

    @Action({ rawError: true })
    async createTransaction(item: Transaction) {
        const created = await TransactionsApi.createTransaction(item);
        this._addItems([created]);
    }

}

export const TransactionsModule = getModule(Transactions);
