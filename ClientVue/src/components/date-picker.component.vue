<template>
    <v-menu
        ref="menu"
        v-model="isPickerOpen"
        :close-on-content-click="true"
        transition="scale-transition"
        offset-y
        min-width="290px"
    >
        <template v-slot:activator="{ on }">
            <v-text-field
                v-bind:class="{ required }"
                :value="valueProxy"
                :label="label"
                :rules="enableValidation && dateRules"
                :prepend-inner-icon="prependInnerIconValue"
                v-on="on"
                @change="onInput"
                @keydown.tab="isPickerOpen = false"
            ></v-text-field>
        </template>
        <v-date-picker v-bind:value="value" @input="onInput" no-title scrollable first-day-of-week="1">
            <v-spacer></v-spacer>
            <v-btn text color="primary" @click="isPickerOpen = false">Close</v-btn>
        </v-date-picker>
    </v-menu>
</template>

<script lang="ts">
import { Component, Vue, Prop, Emit, Watch } from 'vue-property-decorator';
import { date } from '@/utils/rules';

@Component
export default class DatePickerComponent extends Vue {
    @Prop({ type: Boolean, required: false, default: false })
    required!: boolean;

    @Prop({ type: String, required: true })
    label!: string;

    @Prop({ type: String, required: true })
    value!: string;

    @Prop({ type: Boolean, required: false, default: false })
    enableValidation!: boolean;

    @Prop({ type: [Boolean, String], required: false, default: false })
    prependInnerIcon!: boolean|string;

    private valueProxy: string = '';

    private isPickerOpen: boolean = false;
    private readonly dateRules = [
        date('This is not a valid date')
    ]

    get prependInnerIconValue() {
        if (this.prependInnerIcon === true) {
            return 'mdi-calendar';
        }
        if (this.prependInnerIcon) {
            return this.prependInnerIcon;
        }
        return undefined;
    }

    @Watch('value', { immediate: true })
    onValueChange(newValue: string) {
        this.valueProxy = newValue;
    }

    @Emit('input')
    private onInput(value: string) {
        this.valueProxy = value;
        return value;
    }
}
</script>
