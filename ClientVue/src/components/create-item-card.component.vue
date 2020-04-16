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
            <v-btn
                text
                color="red darken-1"
                :disabled="savingItem || disableButtons"
                @click="$emit('close-clicked')"
            >
                Cancel
            </v-btn>
            <v-btn
                color="primary"
                :loading="savingItem"
                :disabled="disableButtons"
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
    @Prop({ type: Object, required: true })
    item!: any;

    @Prop({ type: String, required: true })
    title!: string;

    @Prop({ type: Boolean, required: true })
    savingItem!: string;

    @Prop({ type: Boolean, required: true })
    disableButtons!: string;
}
</script>
