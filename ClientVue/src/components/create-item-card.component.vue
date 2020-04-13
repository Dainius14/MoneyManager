<template>
    <v-card>
        <v-card-title>
            <span class="headline">{{ title }}</span>
        </v-card-title>

        <v-card-text>
            <v-container>
                <slot></slot>
            </v-container>
        </v-card-text>

        <v-card-actions>
            <v-spacer></v-spacer>

            <v-btn color="blue darken-1" text @click="$emit('close-clicked')" :disabled="savingItem">Cancel</v-btn>
            <v-btn color="blue darken-1" text @click="$emit('save-clicked')" :loading="savingItem">Save</v-btn>
        </v-card-actions>
    </v-card>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";

@Component
export default class CreateItemCard extends Vue {
    @Prop({ type: Object, required: true })
    item!: any;

    @Prop({ type: String, required: true })
    title!: string;

    @Prop({ type: Boolean, required: true })
    savingItem!: string;
}
</script>
