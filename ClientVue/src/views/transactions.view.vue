<template>
    <list
        title="Transactions"
        newItemText="Create transaction"
        editItemText="Edit transaction"
        :headers="headers"
        :items="transactionsState.transactions"
        :newItem="newItem"
        :isLoading="isLoading"
        :sortBy="'date'"
        :enablePagination="true"
        :show-keep-open-buttons="true"

        @save-item-clicked="onSaveItemClicked"
        @delete-item-clicked="onDeleteItemClicked"
    >
        <template #item.fromAccount="{ item }">
            {{ item.transactionDetails[0].fromAccount.name }}
        </template>
        <template #item.toAccount="{ item }">
            {{ item.transactionDetails[0].toAccount.name }}
        </template>
        <template #item.category="{ item }">
            {{ item.transactionDetails.map(td => td.category && td.category.name ).join(', ') }}
        </template>
        <template #item.amount="{ item }">
            <span :class="item.type">
                {{ item | transactionSign }} {{ item.amount | currency }}
            </span>
        </template>
        <template #item.date="{ item }">
            {{ item.date | properDate }}
        </template>

        <template #edit-dialog-content="{ item, formChanged, isFormValidChanged }">
            <create-transaction
                :transaction="item"
                @update:editedTransaction="formChanged"
                @update:isFormValid="isFormValidChanged"
            ></create-transaction>
        </template>

        <template #delete-dialog-title="{ item }">
            Do you really want to delete this {{ item.amount | currency }} transaction to {{ item.transactionDetails[0].toAccount.name }} at {{ item.date.toLocaleDateString('lt') }}?
        </template>
    </list>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { ToastService } from '@/services/snackbar.service';
import { DataTableHeader } from 'vuetify';
import { List } from '@/components/list';
import { ListEventArgs } from '@/components/list/list.component';
import { TransactionsModule } from '@/store/modules/transactions-module.store';
import { Transaction, TransactionType } from '@/models/transaction.model';
import CreateTransaction from '@/components/create-transaction.component.vue';
import { format, isBefore } from 'date-fns';
import { LoadState } from '@/models/common.models';

@Component({
    components: {
        List,
        CreateTransaction
    },
    filters: {
        transactionSign(transaction: Transaction) {
            if (transaction.type === TransactionType.Expense) {
                return '-';
            }
            return '';
        },
        properDate(date: Date) {
            const dateStr = format(date, 'yyyy-MM-dd');
            const timeStr = format(date, 'HH:mm');
            let str = dateStr;
            if (timeStr !== '00:00') {
                str += ' ' + timeStr;
            }
            return str;
        }
    }
})
export default class CategoriesView extends Vue {
    headers: DataTableHeader<Transaction>[] = [
        {
            text: 'Date',
            value: 'date',
            align: 'start',
            sortable: true,
            filterable: false,
            // should be both dates, vetur is complaining
            sort: (a: any, b: any) => isBefore(a, b) ? 1 : -1
        },
        {
            text: 'From',
            value: 'fromAccount',
            align: 'start',
            sortable: true,
            filterable: false,
        },
        {
            text: 'To',
            value: 'toAccount',
            align: 'start',
            sortable: true,
            filterable: false,
        },
        {
            text: 'Amount',
            value: 'amount',
            align: 'start',
            sortable: true,
            filterable: false,
        },
        {
            text: 'Category',
            value: 'category',
            align: 'start',
            sortable: true,
            filterable: false,
        },
        {
            text: 'Description',
            value: 'description',
            align: 'start',
            sortable: false,
            filterable: false,
        },
        // {
        //     text: 'Created',
        //     value: 'createdAt',
        //     sortable: false,
        //     filterable: false,
        // },
        // {
        //     text: 'Updated',
        //     value: 'updatedAt',
        //     sortable: false,
        //     filterable: false,
        // },
        {
            text: 'Actions',
            value: 'actions',
            align: 'end',
            sortable: false,
            filterable: false,
        },
    ];

    newItem = new Transaction();

    get transactionsState() {
        return TransactionsModule;
    }

    get isLoading() {
        return TransactionsModule.loadState === LoadState.Loading;
    }

    async created() {
        if (TransactionsModule.loadState === LoadState.NotLoaded) {
            try {
                await TransactionsModule.getTransactions({ page: 0 });
            }
            catch (e) {
                ToastService.show(e, { color: 'error' });
            }
        }
    }

    async onDeleteItemClicked({ item, onStart, onSuccess, onError }: ListEventArgs<Transaction>) {
        onStart();
        try {
            await TransactionsModule.removeTransaction(item);
            onSuccess();
        }
        catch (e) {
            ToastService.show(e, { color: 'error' });
            onError();
        }
    }

    async onSaveItemClicked({ item, onStart, onSuccess, onError }: ListEventArgs<Transaction>) {
        onStart();
        try {
            if (item.id !== -1) {
                await TransactionsModule.editTransaction(item);
            }
            else {
                await TransactionsModule.createTransaction(item);
            }
            onSuccess();
        }
        catch (e) {
            ToastService.show(e, { color: 'error' });
            onError();
        }
    }
}
</script>

<style scoped>
    .expense {
        color: darkred;
    }
    .income {
        color: green;
    }
    .transfer {
        color: darkblue;
    }
    
</style>
