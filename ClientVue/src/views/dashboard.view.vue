<template>
    <div>
        <v-btn v-for="btn in buttons" v-bind:key="btn.label"
            @click="btn.onClick"
        >
            {{ btn.label }}
        </v-btn>
        <highcharts :options="chartOptions" ref="hcInstance"></highcharts>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";
import Highcharts, { SeriesPieOptions } from 'highcharts';
import { Chart } from 'highcharts-vue';
import { DashboardApi } from '@/services/dashboard.api';
import { startOfMonth, sub, endOfMonth } from 'date-fns';

@Component({
    components: {
        Highcharts: Chart
    }
})
export default class DashboardView extends Vue {
    chartOptions: Highcharts.Options = {
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
    hcInstance: any = {};

    buttons: any = [];

    data: any = {};

    @Watch('data')
    onDataChanged() {
        const chart = this.getChart(this.$refs.hcInstance);
        chart.series[0].setData(this.data, true);
    }

    async created() {
        this.createButtons();
        this.buttons[0].onClick();
    }

    async mounted() {
        this.$nextTick(() => this.getChart(this.$refs.hcInstance).reflow());
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
        const receivedData = await DashboardApi.getData(from, to);
        this.data = receivedData.amountsPerCategory
            .filter(x => x.amount !== 0)
            .map(x => ({ name: x.category?.name ?? 'n/a', y: x.amount }));
    }

    private getChart(ref: any): Highcharts.Chart {
        return ref.chart as Highcharts.Chart;
    }
}
</script>
