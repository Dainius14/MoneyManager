<template>
    <v-card>
        <v-card-title>
            <span class="headline">{{ title }}</span>
        </v-card-title>

        <v-card-text>
            <slot></slot>
        </v-card-text>

        <v-card-actions style="flex-direction: column">
            <div v-if="showKeepOpenButtons"
                 style="display: flex; flex-direction: row; width: 100%"
            >
                <v-spacer></v-spacer>
                <v-checkbox v-model="syncedKeepOpen"
                            class="small mr-2"
                            label="Keep dialog open after saving"
                            :ripple="false"
                ></v-checkbox>
                <v-checkbox v-model="syncedKeepValues"
                            :disabled="!syncedKeepOpen"
                            class="small"
                            label="Don't reset values"
                            :ripple="false"
                ></v-checkbox>
            </div>
            <div style="display: flex; width: 100%">
                <v-spacer></v-spacer>
                <v-btn
                    text
                    :disabled="savingItem || disableButtons"
                    @click="$emit('close-clicked')"
                >
                    Cancel
                </v-btn>
                <v-btn
                    color="primary"
                    :loading="savingItem"
                    :disabled="disableButtons || !enableSave"
                    @click="$emit('save-clicked')"
                >
                    Save
                </v-btn></div>
        </v-card-actions>
    </v-card>
</template>

<script lang="ts">
    import { Component, Vue, Prop, PropSync } from 'vue-property-decorator';

@Component
export default class CreateItemCard extends Vue {
    @Prop({ type: String, required: true })
    title!: string;

    @Prop({ type: Boolean, required: true })
    savingItem!: boolean;

    @Prop({ type: Boolean, required: true })
    disableButtons!: boolean;

    @Prop({ type: Boolean, required: true })
    enableSave!: boolean;

    @Prop({ type: Boolean, required: false, default: false })
    showKeepOpenButtons!: boolean;

    @PropSync('keepOpen', { type: Boolean, required: false, default: false })
    syncedKeepOpen!: boolean;

    @PropSync('keepValues', { type: Boolean, required: false, default: false })
    syncedKeepValues!: boolean;
}
</script>

<style scoped>
    /deep/ .small .v-label {
        font-size: 12px;
    }
    /deep/ .small .v-icon {
        width: 80% !important;
    }
    /deep/ .small .v-input--selection-controls__input {
        margin-right: 0 !important;
    }
    /deep/ .small.v-input {
        margin-top: 0;
        padding-top: 0;
    }
    /deep/ .small .v-messages {
        display: none;
    }
</style>