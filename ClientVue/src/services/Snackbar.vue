<template>
    <v-snackbar
        :timeout="options.timeout"
        :color="options.color"
        v-model="active"
        class="application"
        @click="dismiss"
    >
        <v-icon
            dark
            left
            v-if="options.icon"
        >
            {{ options.icon }}
        </v-icon>

        {{ text }}
    </v-snackbar>
</template>
<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { ToastOptions } from "./snackbar.service";

@Component
export default class Snackbar extends Vue {
    text: string = "";
    active: boolean = false;
    options: ToastOptions = {};

    mounted() {
        this.$nextTick(() => (this.active = true));
    }

    show(text: string, options: ToastOptions) {
        this.active = true;
        this.text = text;
        this.options = options;
    }

    close() {
        this.active = false;
    }

    dismiss() {
        if (this.options.dismissable) {
            this.close();
        }
    }
}
</script>
