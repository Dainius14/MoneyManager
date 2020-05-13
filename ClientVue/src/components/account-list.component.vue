<template>
    <list
        :title="title"
        :headers="headers"
        :items="items"
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
import { List } from '@/components/list';
import { ListEventArgs } from './list/list.component';
import { LoadState } from '@/models/common.models';
import { maxLength, notEmpty, number } from '@/utils/rules';
import { InputOptions } from '@/components/list/dynamic-input.component';
import { toIsoDate } from '@/utils/utils';
import { IconNames } from '@/constants';

@Component({
    components: {
        List
    }
})
export default class AccountList extends Vue {
    @Prop({ type: Array, required: true })
    items!: Account[];

    @Prop({ type: String, required: true })
    title!: string;

    @Prop({ type: Boolean, required: true })
    isPersonal!: boolean;

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

    editDialogFields: InputOptions[] = [
        {
            label: 'Account name',
            key: 'name',
            required: true,
            maxLength: 30,
            rules: [
                notEmpty('Name is required'),
                maxLength(30, "Name can't be that long")
            ],
            prependInnerIcon: 'mdi-account-circle'
        },
        {
            label: 'Opening balance',
            key: 'openingBalance',
            type: 'number',
            required: true,
            prependInnerIcon: IconNames.Cash,
            rules: [
                number('Amount must be a number')
            ],
            defaultValue: 0,
        },
        {
            label: 'Opening date',
            key: 'openingDate',
            type: 'date',
            required: true,
            prependInnerIcon: true,
            defaultValue: toIsoDate(new Date())
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
                ToastService.error(e);
            }
        }
    }

    async onDeleteItemClicked({ item, onStart, onSuccess, onError }: ListEventArgs<Account>) {
        onStart();
        try {
            await AccountsModule.removeAccount(item);
            onSuccess('Account deleted successfully');
        }
        catch (e) {
            onError(e, 'There was en error deleting the account');
        }
    }
    

    async onSaveItemClicked({ item, onStart, onSuccess, onError }: ListEventArgs<Account>) {
        onStart();
        try {
            if (item.id != null) {
                await AccountsModule.editAccount(item);
            }
            else {
                item.isPersonal = this.isPersonal;
                await AccountsModule.createAccount(item);
            }
            onSuccess(`Account ${item.name} saved successfully`);
        }
        catch (e) {
            onError(e, 'There was an error saving the account');
            return;
        }
    }
}
</script>
