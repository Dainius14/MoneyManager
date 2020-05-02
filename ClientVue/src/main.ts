import Vue from 'vue';
import App from '@/App.vue';
import router from '@/router';
import store from '@/store';
import vuetify from '@/plugins/vuetify';
import { formatDistanceToNow, formatRelative } from 'date-fns';
// import toast from '@/plugins/toast';

Vue.config.productionTip = false;

Vue.filter('dateToDistanceFromNow', function (value: Date) {
    if (!value) {
        return '';
    }
    return formatDistanceToNow(value, { addSuffix: true });
});

Vue.filter('dateToRelativeDateFromNow', function (value: Date) {
    if (!value) {
        return '';
    }
    return formatRelative(value, new Date());
});

Vue.filter('currency', function(value: number) {
    return `${value.toFixed(2)}\xa0€`;  // contains non-breaking space
});

// Vue.use(toast);
new Vue({
    router,
    store,
    vuetify,
    render: h => h(App)
}).$mount('#app');
