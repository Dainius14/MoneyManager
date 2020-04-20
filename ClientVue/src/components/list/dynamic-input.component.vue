<template>
    <v-text-field
            v-if="options.type === 'number'"
            v-bind:class="{ required: options.required }"
            v-bind:value="value"
            @input="onInput"
            :label="options.label"

            :prepend-inner-icon="options.prependInnerIcon"
            type="number"
            :rules="options.rules"
    ></v-text-field>

    <date-picker
            v-else-if="options.type === 'date'"
            v-bind:class="{ required: options.required }"
            v-bind:value="value"
            @input="onInput"
            :label="options.label"

            :prepend-inner-icon="options.prependInnerIcon"
            :required="options.required"
            enableValidation>
    </date-picker>

    <v-text-field
            v-else
            v-bind:class="{ required: options.required }"
            v-bind:value="value"
            @input="onInput"
            :label="options.label"

            :prepend-inner-icon="options.prependInnerIcon"
            :rules="options.rules"
            :counter="options.maxLength"
    ></v-text-field>
</template>

<script src="./dynamic-input.component.ts"></script>
