import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { Transaction } from '@/models/transaction.model';
import { TransactionsApi } from '@/services/transactions.api';
import store from '@/store';

const name = 'transactions';
if ((store as any).state[name]) {
    store.unregisterModule(name);
}

@Module({ name, store, dynamic: true, namespaced: true })
class Transactions extends VuexModule {
    transactions: Transaction[] = [];
    pagesLoaded: Set<number> = new Set();

    @Mutation
    addItems(items: Transaction[]) {
        this.transactions = this.transactions.concat(items);
    }
    @Mutation
    removeItem(item: Transaction) {
        const index = this.transactions.indexOf(item);
        this.transactions.splice(index, 1);
    }
    @Mutation
    replaceItem(newItem: Transaction) {
        const index = this.transactions.findIndex(a => a.id === newItem.id);
        this.transactions.splice(index, 1, newItem);
    }

    @Mutation
    clear() {
        this.transactions.splice(0, this.transactions.length);
    }

    @Mutation
    addLoadedPage(page: number) {
        this.pagesLoaded.add(page);
    }

    @Action({ rawError: true })
    async getTransactions({ page }: { page: number }) {
        this.addItems(await TransactionsApi.getTransactions(page));
    }

    @Action({ rawError: true })
    async removeTransaction(item: Transaction) {
        if (await TransactionsApi.deleteTransaction(item.id)) {
            this.removeItem(item);
        }
    }

    @Action({ rawError: true })
    async editTransaction(item: Transaction) {
        const updated = await TransactionsApi.editTransaction(item);
        this.replaceItem(updated);
    }

    @Action({ rawError: true })
    async createTransaction(item: Transaction) {
        const created = await TransactionsApi.createTransaction(item);
        this.addItems([created]);
    }

}

export const TransactionsModule = getModule(Transactions);
