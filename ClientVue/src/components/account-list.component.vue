<template>
    <list
        :title="title"
        :headers="headers"
        :items="items"
        :newItem="newItem"
        :editDialogFields="editDialogFields"
        :isLoading="isLoading"
        @save-item-clicked="onSaveItemClicked"
        @delete-item-clicked="onDeleteItemClicked"
    >
        <template v-slot:item.currentBalance="{ item }">
            {{ item.currentBalance | currency }}
        </template>
    </list>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Account } from '@/models/account.model';
import { AccountsModule } from '@/store/modules/accounts-module.store';
import { DataTableHeader } from 'vuetify';
import { ToastService } from '@/services/snackbar.service';
import CreateItemCard from '@/components/create-item-card.component.vue';
import { List } from '@/components/list';
import { EditDialogField, ListEventArgs } from './list/list.component';
import { LoadState } from '@/models/common.models';

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

    newItem: Account = new Account();    
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
    ];

    get isLoading() {
        return AccountsModule.loadState === LoadState.Loading;
    }


    async created() {
        if (AccountsModule.loadState === LoadState.NotLoaded) {
            try {
                await AccountsModule.getAccounts();
            }
            catch (e) {
                ToastService.show(e, { color: 'error' });
            }
        }
    }

    async onDeleteItemClicked({ item, onStart, onSuccess, onError }: ListEventArgs<Account>) {
        onStart();
        try {
            await AccountsModule.removeAccount(item);
            onSuccess();
        }
        catch (e) {
            onError();
            ToastService.show(e, { color: 'error' });
        }
    }
    

    async onSaveItemClicked({ item, onStart, onSuccess, onError }: ListEventArgs<Account>) {
        onStart();
        try {
            if (item.id !== -1) {
                await AccountsModule.editAccount(item);
            }
            else {
                await AccountsModule.createAccount(item);
            }
            onSuccess();
        }
        catch (e) {
            onError();
            ToastService.show(e, { color: 'error' });
            return;
        }
    }
}
</script>
