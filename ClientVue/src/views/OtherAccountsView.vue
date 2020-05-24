<template>
    <account-list
        :title="'Other accounts'"
        :items="accountsState.otherAccounts"
        :isPersonal="false"
    ></account-list>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { AccountsModule } from '@/store/modules/accounts-module.store';
import AccountList from '@/components/account-list.component.vue';

@Component({
    components: {
        AccountList
    }
})
export default class OtherAccountsView extends Vue {

    get accountsState() {
        return AccountsModule;
    }

}
</script>
