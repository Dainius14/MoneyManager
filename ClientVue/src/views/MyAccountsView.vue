<template>
    <account-list
        :title="'My accounts'"
        :items="accountsState.personalAccounts"
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
