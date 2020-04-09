<template>
<div>
        <v-btn @click="deleteAll">Delete all</v-btn>
    <list
        title="Transactions"
        :headers="headers"
        :items="transactionsState.transactions"
        :newItem="newItem"
        :isLoading="isLoading"
        :sortBy="'date'"
        :sortDesc="true"
        @editDialogSaveClicked="onEditDialogSaveClicked"
        @deleteItemButtonClicked="onDeleteItemButtonClicked($event)"
        v-bind:showEditDialog.sync="showEditDialog"
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
            {{ item | getTransactionSign }} {{ item.transactionDetails.reduce((sum, td) => sum + td.amount, 0) | currency }}
        </template>

        <template #edit-dialog-content="item">
            <create-transaction :transaction="item"
                @update:editedTransaction="editedTransaction = $event"
            ></create-transaction>
        </template>
    </list>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { ToastService } from '@/services/snackbar.service';
import { DataTableHeader } from 'vuetify';
import { List } from '@/components/list';
import { EditDialogField } from '@/components/list/list.component';
import { TransactionsModule } from '@/store/modules/transactions-module.store';
import { Transaction } from '../models/transaction.model';
import CreateTransaction from '@/components/create-transaction.component.vue';

@Component({
    components: {
        List,
        CreateTransaction
    },
    filters: {
        getTransactionSign(transaction: Transaction) {
            if (transaction.transactionDetails[0].toAccount!.isPersonal) {
                return '';
            }
            return '-';
        },
    }
})
export default class CategoriesView extends Vue {
    showEditDialog: boolean = false;

    get transactionsState() {
        return TransactionsModule;
    }
    
    headers: DataTableHeader<Transaction>[] = [
        {
            text: 'Date',
            value: 'date',
            align: 'start',
            sortable: true,
            filterable: false,
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

    editDialogFields: EditDialogField[] = [
        {
            label: 'Description',
            value: 'description'
        },
    ];

    newItem = new Transaction();
    editedTransaction = new Transaction();
    isLoading = true;

    async created() {
        try {
            await TransactionsModule.getTransactions();
        }
        catch (e) {
            ToastService.show(e, { color: 'error' });
        }
        this.isLoading = false;
    }

    async onDeleteItemButtonClicked(item: Transaction) {
        try {
            await TransactionsModule.removeTransaction(item);
        }
        catch (e) {
            ToastService.show(e, { color: 'error' });
        }
    }

    async onEditDialogSaveClicked() {
        try {
            if (this.editedTransaction.id !== -1) {
                await TransactionsModule.editTransaction(this.editedTransaction);
            }
            else {
                await TransactionsModule.createTransaction(this.editedTransaction);
            }
        }
        catch (e) {
            ToastService.show(e, { color: 'error' });
            return;
        }
        this.showEditDialog = false;
    }

    async deleteAll() {
        TransactionsModule.transactions.forEach(async t => await TransactionsModule.removeTransaction(t));
    }

}
</script>