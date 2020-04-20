<template>
    <v-snackbar
        :timeout="0"
        :color="options.color"
        :multi-line="options.multiline"
        v-model="active"
        class="application"
        @mouseover="onMouseOver"
        @mouseout="onMouseOut"
    >
        <v-icon v-if="options.icon" left dark >{{ options.icon }}</v-icon>

        <div class="toast-text">{{ text }}</div>

        <v-icon v-if="options.closable" dark @click="close"
                class="close-button">mdi-close</v-icon>
    </v-snackbar>
</template>
<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { ToastOptions } from '@/services/snackbar.service';

@Component
export default class SnackbarComponent extends Vue {
    private text: string = '';
    private active: boolean = false;
    private options: ToastOptions = {};
    private timeout: number = -1;

    show(text: string, options: ToastOptions) {
        this.active = true;
        this.text = text;
        this.options = options;
        this.startCloseTimeout();
    }

    close() {
        this.active = false;
        this.clearCloseTimeout();
    }

    private onMouseOver() {
        this.clearCloseTimeout();
    }

    private onMouseOut() {
        this.startCloseTimeout();
    }

    private startCloseTimeout() {
        this.timeout = setTimeout(() => this.close(), this.options.timeout);
    }

    private clearCloseTimeout() {
        clearTimeout(this.timeout);
    }
}
</script>

<style>
    .close-button {
        font-size: 16px !important;
        opacity: 90%;
    }

    .toast-text {
        white-space: pre-line;
        width: 100%;
    }
</style>