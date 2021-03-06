<template>
    <div>
        <v-navigation-drawer app class="nav-drawer-bg elevation-3" v-model="showDrawer"
            :permanent="!isMobile">
            <v-layout fill-height column justify-space-between>
                <v-list dense nav>
                    <v-list-item v-for="item in items" :key="item.title" :to="item.path" link>
                        <v-list-item-icon>
                            <v-icon>{{ item.icon }}</v-icon>
                        </v-list-item-icon>

                        <v-list-item-content>
                            <v-list-item-title>{{ item.title }}</v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>
                </v-list>

                <v-list dense nav>
                    <v-divider fill-height></v-divider>

                    <v-list-item v-for="item in bottomItems" :key="item.title"
                                 link
                                 :to="item.path"
                                 justify-end>
                        <v-list-item-icon>
                            <v-icon>{{ item.icon }}</v-icon>
                        </v-list-item-icon>

                        <v-list-item-content>
                            <v-list-item-title>{{ item.title }}</v-list-item-title>
                        </v-list-item-content>
                    </v-list-item>
                </v-list>
            </v-layout>
        </v-navigation-drawer>

         <v-app-bar app v-if="isMobile">
             <v-app-bar-nav-icon @click="showDrawer = true"
             ></v-app-bar-nav-icon>
         </v-app-bar>

        <v-content>
            <v-container style="padding: 0">
                <router-view></router-view>
            </v-container>
        </v-content>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Routes } from '@/router';
import { IconNames } from '@/constants';
import { UserModule } from '@/store/modules/users-module.store';

@Component
export default class DefaultLayout extends Vue {

    showDrawer: boolean = false;

    get isMobile() {
        return this.$vuetify.breakpoint.xsOnly;
    }

    private readonly bottomItems = [
        {
            title: `Profile (${UserModule.currentUser.email})`,
            icon: 'mdi-account',
            path: Routes.Profile.path
        },
        {
            title: 'Logout',
            icon: 'mdi-logout',
            path: Routes.Logout.path
        }
    ];
    private readonly items = [
        {
            title: 'Dashboard',
            icon: IconNames.Dashboard,
            path: Routes.Dashboard.path
        },
        {
            title: 'Transactions',
            icon: IconNames.Transactions,
            path: Routes.Transactions.path
        },
        {
            title: 'My Accounts',
            icon: IconNames.MyAccount,
            path: Routes.MyAccounts.path
        },
        {
            title: 'Other Accounts',
            icon: IconNames.OtherAccount,
            path: Routes.OtherAccounts.path
        },
        {
            title: 'Categories',
            icon: IconNames.Category,
            path: Routes.Categories.path
        },
        {
            title: 'Import transactions',
            icon: IconNames.Import,
            path: Routes.ImportTransactions.path
        },
    ];
}
</script>

<style>
    .nav-drawer-bg {
        background-image: linear-gradient(120deg, #48c6ef 0%, #6f86d6 100%);
    }
</style>
