<template>
    <div>
        <date-picker
            required
            label="Date"
            :value="date"
            @update:value="onDateChanged"
        ></date-picker>
        
        <v-text-field
            class="required"
            label="Amount"
            prepend-inner-icon="mdi-cash"
            prefix="€"
            type="number"
            v-model="amount"
            mask="0.00"
            :rules="amountRules"
            @change="onFormChanged"
        ></v-text-field>

        <v-autocomplete
            class="required"
            v-model="fromAccount"
            :items="fromAccountItems"
            :loading="isLoadingAccounts"
            :search-input.sync="fromAccountSearchTerm"
            item-text="name"
            item-value="id"
            label="From account"
            no-filter
            return-object
            hide-no-data
            @change="onFormChanged"
            prepend-inner-icon="mdi-account-arrow-right"
        ></v-autocomplete>

        <v-autocomplete
            class="required"
            v-model="toAccount"
            :items="toAccountItems"
            :loading="isLoadingAccounts"
            :search-input.sync="toAccountSearchTerm"
            item-text="name"
            item-value="id"
            label="To account"
            no-filter
            return-object
            @change="onFormChanged"
            prepend-inner-icon="mdi-account-arrow-left"
        ></v-autocomplete>

        <v-autocomplete
            v-model="category"
            :items="categories"
            :loading="isLoadingCategories"
            :search-input.sync="categorySearchTerm"
            hide-no-data
            item-text="name"
            item-value="id"
            label="Category"
            return-object
            @change="onFormChanged"
            prepend-inner-icon="mdi-format-list-bulleted-square"
        ></v-autocomplete>

        <v-text-field
            label="Description"
            v-model="description"
            @change="onFormChanged"
            prepend-inner-icon="mdi-message-text"
        ></v-text-field>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from "vue-property-decorator";
import { Transaction } from '@/models/transaction.model';
import { CategoriesModule } from '../store/modules/categories-module.store';
import { AccountsModule } from '../store/modules/accounts-module.store';
import { Category } from '@/models/category.model';
import { Account } from '@/models/account.model';
import { number, positiveNumber } from '@/utils/rules';
import DatePicker from '@/components/date-picker.component.vue';
import { toIsoDate } from '../utils/utils';


@Component({
    components: {
        DatePicker
    }
})
export default class CreateTransactionComponent extends Vue {
    @Prop({ type: Object, required: true })
    transaction!: Transaction;

    editedTransaction: Transaction = new Transaction();

    get categories() {
        return CategoriesModule.categories;
    }

    get accountStore() {
        return AccountsModule;
    }

    get fromAccountItems() {
        let items: any[];
        if (this.toAccount?.id === -1) {
            items = [
                { header: 'My accounts' },
                ...AccountsModule.personalAccounts,
                { header: 'Other accounts' },
                ...AccountsModule.otherAccounts,
            ];
        }
        else if (this.toAccount?.isPersonal) {
            items = [    
                { header: 'Other accounts' },
                ...AccountsModule.otherAccounts
            ];
        }
        else {
            items = [
                { header: 'My accounts' },
                ...AccountsModule.personalAccounts
            ];
        }
        return items;
        // return items.filter((item: Account & { header: string }) =>
        //     item.header || !this.fromAccountSearchTerm
        //     || item.name.toLowerCase().includes(this.fromAccountSearchTerm)
        // );
    }

    get toAccountItems() {
        let items: any[];
        if (this.fromAccount?.id === -1 || this.fromAccount?.isPersonal) {
            items = [
                { header: 'Other accounts' },
                ...AccountsModule.otherAccounts,
                { header: 'My accounts' },
                ...AccountsModule.personalAccounts,
            ];
        }
        else {
            items = [
                { header: 'My accounts' },
                ...AccountsModule.personalAccounts,
            ];
        }
        
        return items;
        // return items.filter((item: Account & { header: string }) =>
        //     item.header || !this.toAccountSearchTerm
        //     || item.name.toLowerCase().includes(this.toAccountSearchTerm)
        // );
    }

    isLoadingCategories: boolean = false;
    isLoadingAccounts: boolean = false;

    date: string = '';
    amount: string = '0';
    amountRules = [
        number('Amount must be a number'),
        positiveNumber('Amount must be a non-negative number')
    ];

    description: string = '';

    fromAccount: Account = new Account();
    fromAccountSearchTerm: string = '';
    
    toAccount: Account = new Account();
    toAccountSearchTerm: string = '';

    category?: Category = new Category();
    categorySearchTerm: string = '';

    created() {        
        if (!CategoriesModule.loaded) {
            this.isLoadingCategories = true;
            CategoriesModule.getCategories().then(() => {
                this.isLoadingCategories = false;
                if (this.transaction.id !== -1) {
                    this.setCategoryFromStore();
                    this.onFormChanged();
                }

            })
        }

        if (!AccountsModule.loaded) {
            this.isLoadingAccounts = true;
            AccountsModule.getAccounts().then(() => {
                this.isLoadingAccounts = false;
                if (this.transaction.id !== -1) {
                    this.setAccountsFromStore();
                    this.onFormChanged();
                }
            })
        }
    }

    onFormChanged() {
        this.editedTransaction.date = this.date;
        this.editedTransaction.description = this.description;
        this.editedTransaction.transactionDetails[0].amount = parseFloat(this.amount);
        this.editedTransaction.transactionDetails[0].fromAccount = this.fromAccount;
        this.editedTransaction.transactionDetails[0].toAccount = this.toAccount;
        this.editedTransaction.transactionDetails[0].category = this.category;
        this.$emit('update:editedTransaction', this.editedTransaction);
    }


    @Watch('transaction', { immediate: true })
    onTransactionChanged(transaction: Transaction) {
        if (transaction.id === -1) {
            this.resetFields();
            return;
        }
        this.editedTransaction.id = transaction.id;
        this.editedTransaction.transactionDetails[0].id = transaction.transactionDetails[0].id;
        this.date = transaction.date;
        this.description = transaction.description;
        this.amount = transaction.transactionDetails[0].amount.toString() + '';
        this.setCategoryFromStore();
        this.setAccountsFromStore();
        this.onFormChanged();
    }

    private setCategoryFromStore() {
        this.category = CategoriesModule.categories.find(cat =>
            cat.id === this.transaction.transactionDetails[0].category?.id ?? null);
    }

    private setAccountsFromStore() {
        this.fromAccount = AccountsModule.accounts.find(acc =>
            acc.id === this.transaction.transactionDetails[0].fromAccount!.id)!;
        this.toAccount = AccountsModule.accounts.find(acc =>
            acc.id === this.transaction.transactionDetails[0].toAccount!.id)!;
    }

    private resetFields() {
        // TODO when closing new form you can see how fields dissapear
        this.date = toIsoDate(new Date());
        this.amount = '0';
        this.description = '';
        this.fromAccount = new Account();
        this.toAccount = new Account();
        this.category = new Category();
        this.editedTransaction = new Transaction();
    }

    onDateChanged(value: string) {
        this.date = value;
        this.onFormChanged();
    }
}
</script>

<style>
.required .v-label::after {
    content: ' *';
}
</style>