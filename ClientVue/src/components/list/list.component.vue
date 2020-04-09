<template>

    <v-data-table
        :headers="headers"
        :items="items"
        :disable-pagination="true"
        :loading="isLoading"
        :sort-by="sortBy"
        :sort-desc="sortDesc"
        class="elevation-1"
    >

        <template v-slot:top>
            <v-toolbar flat color="white">
                <v-toolbar-title>{{ title }}</v-toolbar-title>
                <v-divider
                    class="mx-4"
                    inset
                    vertical
                ></v-divider>
                <v-spacer></v-spacer>
                <v-btn color="primary" dark class="mb-2" v-on:click="onNewItemButtonClicked">New Item</v-btn>

                <v-dialog v-model="showEditDialog" max-width="500px"
                    :persistent="true">

                    <create-item-card
                        :title="formTitle"
                        :item="editedItem"
                        @close-clicked="onEditDialogCloseClicked"
                        @save-clicked="onEditDialogSaveClicked"
                    >
                        <slot name="edit-dialog-content" v-bind="editedItem">
                            <div v-for="field in editDialogFields" :key="field.value">
                                <v-text-field
                                    :label="field.label"
                                    v-model="editedItem[field.value]"
                                ></v-text-field>
                            </div>
                        </slot>
                    </create-item-card>

                </v-dialog>
            </v-toolbar>
        </template>

        <template v-slot:item.createdAt="{ item }">
            <v-tooltip top>
                <template v-slot:activator="{ on }">
                    <span v-on="on">
                        <!-- TODO: called multiple times, find why -->
                        {{ item.createdAt | dateToDistanceFromNow }}
                    </span>
                </template>
                <span>{{ item.createdAt | dateToRelativeDateFromNow}}</span>
            </v-tooltip>
        </template>

        <template v-slot:item.updatedAt="{ item }">
            <div v-if="!item.updatedAt"><i>Never</i></div>
            <div v-else>
                <v-tooltip top>
                    <template v-slot:activator="{ on }">
                        <span v-on="on">
                            {{ item.updatedAt | dateToDistanceFromNow }}
                        </span>
                    </template>
                    <span>{{ item.updatedAt | dateToRelativeDateFromNow }}</span>
                </v-tooltip>
            </div>
        </template>

        <template v-slot:item.actions="{ item }">
            <v-icon
                small
                class="mr-2"
                @click="onEditItemClicked(item)"
            >
                mdi-pencil
            </v-icon>
            <v-icon
                small
                @click="onDeleteItemClicked(item)"
            >
                mdi-delete
            </v-icon>
        </template>
        
        <template v-for="(_, slot) of $scopedSlots" v-slot:[slot]="scope">
            <slot :name="slot" v-bind="scope"/>
        </template>
    </v-data-table>
</template>

<script src="./list.component"></script>
