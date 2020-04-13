<template>
<div>
        <v-btn @click="deleteAll">Delete all</v-btn>
    <list
        :title="title"
        :headers="headers"
        :items="items"
        :newItem="newItem"
        :editDialogFields="editDialogFields"
        :isLoading="isLoading"
        @editDialogSaveClicked="onEditDialogSaveClicked($event)"
        @deleteItemButtonClicked="onDeleteItemButtonClicked($event)"
        v-bind:showEditDialog.sync="showEditDialog"
    >
        <template v-slot:item.currentBalance="{ item }">
            {{ item.currentBalance | currency }}
        </template>
    </list>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import { Account } from "@/models/account.model";
import { AccountsModule } from '@/store/modules/accounts-module.store';
import { DataTableHeader } from 'vuetify';
import { ToastService } from '@/services/snackbar.service';
import CreateItemCard from '@/components/create-item-card.component.vue';
import { List } from '@/components/list';
import { EditDialogField } from './list/list.component';

@Component({
    components: {
        CreateItemCard,
        List
    }
})
export default class AccountList extends Vue {
    @Prop({ type: Array, required: true })
    items!: Account[];

    @Prop({ type: String, required: true })
    title!: string;

    inputRules = {
        positiveNumber: (value: string) => parseFloat(value) >= 0 || 'Must be a positive number'
    };

    showEditDialog = false;

    newItem: Account = new Account();
    isLoading = true;
    
    headers: DataTableHeader<Account>[] = [
        {
            text: 'Name',
            value: 'name',
            align: 'start',
            sortable: true,
            filterable: false,
        },
        {
            text: 'Current balance',
            value: 'currentBalance',
            sortable: false,
            filterable: false,
        },
        {
            text: 'Created',
            value: 'createdAt',
            sortable: false,
            filterable: false,
        },
        {
            text: 'Updated',
            value: 'updatedAt',
            sortable: false,
            filterable: false,
        },
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
            label: 'Name',
            value: 'name',
        },
        {
            label: 'Opening balance',
            value: 'openingBalance',
            type: 'number'
        },
        {
            label: 'Opening date',
            value: 'openingDate',
        },
    ]


    async created() {
        try {
            await AccountsModule.getAccounts();
        }
        catch (e) {
            ToastService.show(e, { color: 'error' });
        }
        this.isLoading = false;
    }

    async onDeleteItemButtonClicked(item: Account) {
        try {
            await AccountsModule.removeAccount(item);
        }
        catch (e) {
            ToastService.show(e, { color: 'error' });
        }
    }
    

    async onEditDialogSaveClicked(item: Account) {
        try {
            if (item.id !== -1) {
                await AccountsModule.editAccount(item);
            }
            else {
                await AccountsModule.createAccount(item);
            }
        }
        catch (e) {
            ToastService.show(e, { color: 'error' });
            return;
        }
        
        this.showEditDialog = false;
    }
    async deleteAll() {
        AccountsModule.accounts.forEach(async t => await AccountsModule.removeAccount(t));
    }
}
</script>
