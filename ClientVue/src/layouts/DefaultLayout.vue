<template>
    <div>
        <v-navigation-drawer app :permanent="true" class="nav-drawer-bg elevation-3">
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
        </v-navigation-drawer>

        <!-- <v-app-bar app> -->
        <!-- -->
        <!-- </v-app-bar> -->

        <v-content>
            <v-container fluid>
                <router-view></router-view>
            </v-container>
        </v-content>
    </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { Routes } from '../router';

@Component
export default class DefaultLayout extends Vue {
    items = [
        {
            title: "Logout",
            icon: "mdi-logout",
            path: Routes.Logout.path
        },
        {
            title: "Dashboard",
            icon: "mdi-monitor-dashboard",
            path: Routes.Dashboard.path
        },
        {
            title: "Transactions",
            icon: "mdi-bank-transfer",
            path: Routes.Transactions.path
        },
        {
            title: "Other Accounts",
            icon: "mdi-account",
            path: Routes.OtherAccounts.path
        },
        {
            title: "My Accounts",
            icon: "mdi-account",
            path: Routes.MyAccounts.path
        },
        {
            title: "Categories",
            icon: "mdi-view-list",
            path: Routes.Categories.path
        },
        {
            title: "Import transactions",
            icon: "mdi-import",
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
