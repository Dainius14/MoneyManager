<template>
    <div>
        Test buildbot2
        <v-btn v-for="btn in buttons" v-bind:key="btn.label"
            @click="btn.onClick"
        >
            {{ btn.label }}
        </v-btn>
        <highcharts :options="expensesPerCategoryOptions" :ref="refs.expensesPerCategory"></highcharts>
        <highcharts :options="amountsPerMonthOptions" :ref="refs.amountsPerMonth"></highcharts>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator';
import Highcharts, { SeriesPieOptions } from 'highcharts';
import { Chart } from 'highcharts-vue';
import { DashboardApi } from '@/services/dashboard.api';
import { startOfMonth, sub, endOfMonth, getMonth, format } from 'date-fns';

@Component({
    components: {
        Highcharts: Chart
    }
})
export default class DashboardView extends Vue {
    refs = {
        expensesPerCategory: 'expensesPerCategory',
        amountsPerMonth: 'amountsPerMonth'
    };

    expensesPerCategoryOptions: Highcharts.Options = {
        credits: {
            enabled: false
        },
        title: {
            text: 'Expenses per category'
        },
        plotOptions: {
            series: {
                dataLabels: {
                    enabled: true,
                    useHTML: true,
                    format: '<b>{point.name}</b>: {point.y:.0f} €'
                }
            }
        },
        series: [
            {
                name: 'Categories',
                type: 'pie'
            } as SeriesPieOptions
        ]
    };

    amountsPerMonthOptions: Highcharts.Options = {
        credits: {
            enabled: false
        },
        title: {
            text: 'Expenses and income per month'
        },
        plotOptions: {
            series: {
                dataLabels: {
                    enabled: true,
                    format: '{y:.0f} €'
                }
            }
        },
        series: [
            {
                name: 'Expenses',
                type: 'column',
                color: 'darkred'
            },
            {
                name: 'Income',
                type: 'column',
                color: 'darkgreen'
            },
        ]
    };

    hcInstance: any = {};

    buttons: any = [];

    async created() {
        this.createButtons();
        this.buttons[0].onClick();
    }

    async mounted() {
        this.$nextTick(() => {
            this.getChart(this.refs.expensesPerCategory).reflow();
            this.getChart(this.refs.amountsPerMonth).reflow();
        });
    }

    private createButtons() {
        this.buttons = [
            {
                label: 'Current month',
                onClick: () => this.getNewData(startOfMonth(new Date()), endOfMonth(new Date()))
            },
            {
                label: 'Previous month',
                onClick: () => this.getNewData(startOfMonth(sub(new Date(), { months: 1 })), endOfMonth(sub(new Date(), { months: 1 })))
            }
        ];
    }

    private async getNewData(from: Date, to: Date) {
        const data = await DashboardApi.getData(from, to);

        const expensesPerCategory = data.expensesPerCategory
            .filter(x => x.amount !== 0)
            .map(x => ({ name: x.category?.name ?? 'n/a', y: x.amount }));
        this.getChart(this.refs.expensesPerCategory).series[0].setData(expensesPerCategory);


        const amountsPerMonth = ['expenses', 'income'].map(seriesName => 
            data.amountsPerMonth.map(x => (x as any)[seriesName])
        );
        this.getChart(this.refs.amountsPerMonth).xAxis[0].setCategories(
            data.amountsPerMonth.map(x => format(new Date(x.month), 'MMMM'))
        );
        this.getChart(this.refs.amountsPerMonth).series[0].setData(amountsPerMonth[0]);
        this.getChart(this.refs.amountsPerMonth).series[1].setData(amountsPerMonth[1]);
    }

    private getChart(chartName: string): Highcharts.Chart {
        return (this.$refs[chartName] as any).chart as Highcharts.Chart;
    }


}
</script>
