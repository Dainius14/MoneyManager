<template>
    <v-card>
        <v-card-title>
            <span class="headline">{{ title }}</span>
        </v-card-title>

        <v-card-text>
            <slot></slot>
        </v-card-text>

        <v-card-actions>
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
            </v-btn>
        </v-card-actions>
    </v-card>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";

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
}
</script>
