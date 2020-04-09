<template>
    <v-menu
        ref="menu"
        v-model="isOpen"
        :close-on-content-click="true"
        transition="scale-transition"
        offset-y
        min-width="290px"
    >
        <template v-slot:activator="{ on }">
            <v-text-field
                v-bind:class="{ required }"
                :value="value"
                @change="onValueUpdated"
                :label="label"
                prepend-inner-icon="mdi-calendar"
                v-on="on"
            ></v-text-field>
        </template>
        <v-date-picker :value="value" @input="onValueUpdated" no-title scrollable>
            <v-spacer></v-spacer>
            <v-btn text color="primary" @click="isOpen = false">Close</v-btn>
            <!-- <v-btn text color="primary" @click="$refs.menu.save(date)">OK</v-btn> -->
        </v-date-picker>
    </v-menu>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";

@Component
export default class DatePickerComponent extends Vue {
    @Prop({ type: Boolean, required: false, default: false })
    required!: boolean;

    @Prop({ type: String, required: true })
    label!: string;

    @Prop({ type: String, required: true })
    value!: string;

    isOpen: boolean = false;

    onValueUpdated(value: string) {
        this.$emit('update:value', value);
    }
}
</script>
