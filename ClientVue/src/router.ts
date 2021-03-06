import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import DefaultLayout from '@/layouts/DefaultLayout.vue';
import OtherAccountsView from '@/views/OtherAccountsView.vue';
import MyAccountsView from '@/views/MyAccountsView.vue';
import CategoriesView from '@/views/CategoriesView.vue';
import LogoutComponent from '@/components/logout.component.vue';
import TransactionsView from '@/views/transactions.view.vue';
import DashboardView from '@/views/dashboard.view.vue';
import ImportTransactionsView from '@/views/import-transactions.view.vue';
import { UserModule } from '@/store/modules/users-module.store';
import ProfileView from '@/views/profile/profile.view.vue';

Vue.use(VueRouter);

export const Routes = {
    Root: { name: 'Root', path: '/' },
    Login: { name: 'Login', path: '/login' },
    Logout: { name: 'Logout', path: '/logout' },
    Dashboard: { name: 'Dashboard', path: '/dashboard' },
    OtherAccounts: { name: 'OtherAccounts', path: '/accounts/other' },
    MyAccounts: { name: 'MyAccounts', path: '/accounts/my' },
    Categories: { name: 'Categories', path: '/categories' },
    Transactions: { name: 'Transactions', path: '/transactions' },
    ImportTransactions: { name: 'ImportTransactions', path: '/transactions/import' },
    Profile: { name: 'Profile', path: '/profile' },
};

const routes: RouteConfig[] = [
    {
        path: Routes.Login.path,
        name: Routes.Login.name,

        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "login" */ '@/views/Login/login.view.vue')
    },
    {
        path: Routes.Logout.path,
        name: Routes.Logout.name,
        component: LogoutComponent
    },

    {
        path: '/',
        component: DefaultLayout,
        children: [
            {
                path: '/',
                redirect: Routes.Transactions.path,
            },
            {
                path: Routes.Transactions.path,
                name: Routes.Transactions.name,
                component: TransactionsView,
            },
            {
                path: Routes.Dashboard.path,
                name: Routes.Dashboard.name,
                component: DashboardView,
            },
            {
                path: Routes.OtherAccounts.path,
                name: Routes.OtherAccounts.name,
                component: OtherAccountsView
            },
            {
                path: Routes.MyAccounts.path,
                name: Routes.MyAccounts.name,
                component: MyAccountsView
            },
            {
                path: Routes.Categories.path,
                name: Routes.Categories.name,
                component: CategoriesView
            },
            {
                path: Routes.ImportTransactions.path,
                name: Routes.ImportTransactions.name,
                component: ImportTransactionsView,
            },
            {
                path: Routes.Profile.path,
                name: Routes.Profile.name,
                component: ProfileView,
            }
        ]
    },

];

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
});

router.beforeEach(async (to, from, next) => {
    if (to.name !== Routes.Login.name && !UserModule.isLoggedIn) {
        await UserModule.getCurrentUser();
        if (UserModule.isLoggedIn) {
            return next();
        }
        else {
            return next(Routes.Login.path);
        }
    }
    else if (to.name === Routes.Login.name && UserModule.isLoggedIn) {
        next(Routes.Root.path);
    }
    else {
        next();
    }
});


export default router;
