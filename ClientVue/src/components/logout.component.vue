<template>
    <div></div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import router, { Routes } from '@/router';
import { UserModule } from '@/store/modules/users-module.store';

@Component({})
export default class LogoutComponent extends Vue {
    created() {
        UserModule.logout();
        router.push(Routes.Login);
    }
}
</script>
