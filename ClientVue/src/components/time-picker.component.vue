<template>
    <v-menu
        ref="menu"
        v-model="isOpen"
        :close-on-content-click="false"
        transition="scale-transition"
        offset-y
        min-width="290px"
        @input="value => value && $refs.picker && ($refs.picker.selectingHour = true)"
    >
        <template v-slot:activator="{ on }">
            <v-text-field
                v-bind:class="{ required }"
                :value="value"
                :label="label"
                :rules="enableValidation && timeRules"
                prepend-inner-icon="mdi-clock"
                v-on="on"
                @change="onValueUpdated"
                @keydown.tab="isOpen = false"
            ></v-text-field>
        </template>
        <v-time-picker
            ref="picker"
            :value="value" @input="onValueUpdated"
            full-width
            format="24hr"
            no-title
            @change="$refs.menu.save(value)"
        >
            <v-spacer></v-spacer>
            <v-btn text color="primary" @click="isOpen = false">Close</v-btn>
        </v-time-picker>
    </v-menu>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import { time } from '@/utils/rules';

@Component
export default class TimePickerComponent extends Vue {
    @Prop({ type: Boolean, required: false, default: false })
    required!: boolean;

    @Prop({ type: String, required: true })
    label!: string;

    @Prop({ type: String, required: true })
    value!: string;

    @Prop({ type: Boolean, required: false, default: false })
    enableValidation!: boolean;

    isOpen: boolean = false;

    timeRules = [
        time('This is not a valid time')
    ]

    onValueUpdated(value: string) {
        this.$emit('update:value', value);
    }
}
</script>
