<template>
    <v-form ref="form" @input="$emit('update:isFormValid', $event)">
        <v-row>
            <v-col>
                <date-picker
                    required
                    enableValidation
                    label="Date"
                    :value.sync="date"
                    @update="onFormChanged"
                ></date-picker>
            </v-col>
            <v-col>
                <time-picker
                    required
                    enableValidation
                    label="Time"
                    :value.sync="time"
                    @update="onFormChanged"
                ></time-picker>
            </v-col>
        </v-row>

        <v-autocomplete
            class="required"
            v-model="fromAccount"
            :items="accountItems"
            :loading="isLoadingAccounts"
            :search-input.sync="fromAccountSearchTerm"
            :filter="filterAccount"
            :rules="accountRules"
            auto-select-first
            clearable
            item-text="name"
            item-value="id"
            label="From account"
            prepend-inner-icon="mdi-account-arrow-right"
            return-object
            @change="onFormChanged"
        ></v-autocomplete>

        <v-autocomplete
            class="required"
            v-model="toAccount"
            :items="accountItems"
            :loading="isLoadingAccounts"
            :search-input.sync="toAccountSearchTerm"
            :filter="filterAccount"
            :rules="accountRules"
            auto-select-first
            clearable
            item-text="name"
            item-value="id"
            label="To account"
            prepend-inner-icon="mdi-account-arrow-left"
            return-object
            @change="onFormChanged"
        ></v-autocomplete>

        <p class="font-weight-light" style="font-size: 12px">{{ transactionType }}<br/></p>
        
        <v-text-field
            class="required"
            label="Amount"
            prepend-inner-icon="mdi-cash"
            prefix="€"
            type="number"
            v-model.number="amount"
            :rules="amountRules"
            @input="onFormChanged"
        ></v-text-field>

        <v-autocomplete
            v-model="category"
            :items="categories"
            :loading="isLoadingCategories"
            :search-input.sync="categorySearchTerm"
            auto-select-first
            item-text="name"
            item-value="id"
            label="Category"
            return-object
            clearable
            @change="onFormChanged"
            prepend-inner-icon="mdi-format-list-bulleted-square"
        ></v-autocomplete>

        <v-text-field
            label="Description"
            v-model="description"
            @input="onFormChanged"
            prepend-inner-icon="mdi-message-text"
            :counter="descriptionMaxLength"
            :rules="descriptionRules"
        ></v-text-field>
    </v-form>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import { Transaction } from '@/models/transaction.model';
import { CategoriesModule } from '../store/modules/categories-module.store';
import { AccountsModule } from '../store/modules/accounts-module.store';
import { Category } from '@/models/category.model';
import { Account } from '@/models/account.model';
import { number, positiveNumber, notEmpty, maxLength } from '@/utils/rules';
import DatePicker from '@/components/date-picker.component.vue';
import TimePicker from '@/components/time-picker.component.vue';
import { toIsoDate } from '../utils/utils';
import { format } from 'date-fns';


@Component({
    components: {
        DatePicker,
        TimePicker
    }
})
export default class CreateTransactionComponent extends Vue {

    @Prop({ type: Object, required: true })
    transaction!: Transaction;

    private isLoadingCategories: boolean = false;
    private isLoadingAccounts: boolean = false;

    private editedTransaction: Transaction = new Transaction();
    private date: string = '';
    private time: string = '';
    private amount: number = 0;
    private description: string = '';
    private fromAccount: Account = new Account();
    private fromAccountSearchTerm: string = '';
    private toAccount: Account = new Account();
    private toAccountSearchTerm: string = '';
    private category?: Category = new Category();
    private categorySearchTerm: string = '';
    private readonly descriptionMaxLength = 100;
    

    private readonly amountRules = [
        number('Amount must be a number'),
        positiveNumber('Amount must be a positive number')
    ];

    private readonly descriptionRules = [
        maxLength(this.descriptionMaxLength, "Description can't be that long")
    ];
    
    private get accountRules() {
        return [
            (item: Account) => notEmpty('Account must be selected')(item?.name),
            () => !this.bothAccountsNotPersonal || "Both accounts must can't be non personal"
        ];
    }

    private get bothAccountsNotPersonal() {
        if (this.fromAccount?.id === -1 || this.toAccount?.id === -1) {
            return false;
        }
        return !this.fromAccount?.isPersonal && !this.toAccount?.isPersonal;
    }
    

    private get categories() {
        return CategoriesModule.categories;
    }

    private get accountItems() {
        return [
            { header: 'My accounts' },
            ...AccountsModule.personalAccounts,
            { header: 'Other accounts' },
            ...AccountsModule.otherAccounts,
        ];
    }

    private get transactionType() {
        let str = 'Creating ';
        if (this.fromAccount?.isPersonal && this.toAccount?.isPersonal) {
            str += 'a transfer';
        }
        else if (this.fromAccount?.isPersonal && !this.toAccount?.isPersonal) {
            str += 'an expense';
        }
        else if (!this.fromAccount?.isPersonal && this.toAccount?.isPersonal) {
            str += 'an income';
        }
        else {
            str += 'some kind of';
        }
        str += ' transaction';
        return str;
    }

    created() {        
        if (!CategoriesModule.loaded) {
            this.isLoadingCategories = true;
            CategoriesModule.getCategories().then(() => {
                this.isLoadingCategories = false;
                if (this.transaction.id !== -1) {
                    this.setCategoryFromStore();
                    this.onFormChanged();
                }

            });
        }

        if (!AccountsModule.loaded) {
            this.isLoadingAccounts = true;
            AccountsModule.getAccounts().then(() => {
                this.isLoadingAccounts = false;
                if (this.transaction.id !== -1) {
                    this.setAccountsFromStore();
                    this.onFormChanged();
                }
            });
        }
    }

    private onFormChanged() {
        console.log(this.description.toString());
        this.editedTransaction.date = new Date(this.date + 'T' + this.time);
        this.editedTransaction.description = this.description;
        this.editedTransaction.transactionDetails[0].amount = this.amount;
        this.editedTransaction.transactionDetails[0].fromAccount = this.fromAccount;
        this.editedTransaction.transactionDetails[0].toAccount = this.toAccount;
        this.editedTransaction.transactionDetails[0].category = this.category;
        this.$emit('update:editedTransaction', this.editedTransaction);
    }


    @Watch('transaction', { immediate: true })
    private onTransactionChanged(transaction: Transaction) {
        if (transaction.id === -1) {
            this.resetFields();
            return;
        }
        this.editedTransaction.id = transaction.id;
        this.editedTransaction.transactionDetails[0].id = transaction.transactionDetails[0].id;
        this.date = format(transaction.date, 'yyyy-MM-dd');
        this.time = format(transaction.date, 'HH:mm');
        this.description = transaction.description;
        this.amount = transaction.transactionDetails[0].amount;
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
        const form = this.$refs.form as any;
        if (!form) {
            return;
        }
        
        this.date = toIsoDate(new Date());
        this.time = '00:00';
        this.amount = 0;
        this.description = '';
        this.fromAccount = new Account();
        this.toAccount = new Account();
        this.category = new Category();
        this.editedTransaction = new Transaction();
    }

    private filterAccount(item: Account&{ header: string }, queryText: string, itemText: string): boolean {
        return !!item.header || itemText.toLowerCase().indexOf(queryText.toLowerCase()) !== -1;
    }
}
</script>

<style>
.required .v-label::after {
    content: ' *';
}
</style>
