<template>

    <v-data-table
        :headers="headers"
        :items="items"
        :disable-pagination="!enablePagination"
        :loading="isLoading"
        :sort-by="sortBy"
        :sort-desc="sortDesc"
        :items-per-page="25"
        :footer-props="{ disableItemsPerPage: true }"
        
    >

        <template v-slot:top>
            <v-toolbar flat color="white">
                <v-toolbar-title>{{ title }}</v-toolbar-title>
                <v-spacer></v-spacer>
                <v-btn color="primary" dark class="mb-2" @click="onNewItemButtonClicked">{{ newItemText }}</v-btn>

                <v-dialog v-model="showEditDialog" max-width="500px"
                          @click:outside="onEditDialogCloseClicked">

                    <create-item-card
                        :title="formTitle"
                        :savingItem="savingItem"
                        :disableButtons="disableEditDialogButtons"
                        :enableSave="isFormValid"
                        :show-keep-open-buttons="isNewItem"
                        :keep-open.sync="keepDialogOpen"
                        :keep-values.sync="keepDialogValues"
                        @close-clicked="onEditDialogCloseClicked"
                        @save-clicked="onEditDialogSaveClicked"
                    >
                        <slot name="edit-dialog-content" v-bind="{ item: editedItem, formChanged: onCustomFormChanged, isFormValidChanged: onCustomIsFormValidChanged }">
                            <v-form v-model="isFormValid" ref="form">
                                <dynamic-input
                                    v-for="field in editDialogFields"
                                    :key="field.value"
                                    :options="field"
                                    v-model="editedItem[field.key]"
                                ></dynamic-input>
                            </v-form>
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
            
            <v-dialog v-model="showDeleteDialog[item.id]" width="400px">
                <template v-slot:activator="{ on }">
                    <v-icon v-on="on" small
                        @click="disableDeleteDialogButtons = false">
                        mdi-delete
                    </v-icon>
                </template>
                <v-card>
                    <v-card-title style="word-break: unset">
                        <slot name="delete-dialog-title" v-bind="{ item }">
                            Do you really want to delete this item?
                        </slot>
                    </v-card-title>

                    <v-card-text>
                        <slot name="delete-dialog-text" v-bind="{ item }">
                            This action is irreversible
                        </slot>
                    </v-card-text>

                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn
                            text
                            :disabled="deletingItem || disableDeleteDialogButtons"
                            @click="showDeleteDialog[item.id] = false"
                        >
                            No, cancel
                        </v-btn>
                        <v-btn
                            color="red"
                            :loading="deletingItem"
                            :disabled="disableDeleteDialogButtons"
                            @click="onDeleteItemClicked(item)"
                        >
                            Yes, delete
                        </v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>



        </template>
        
        <template v-for="(_, slot) of $scopedSlots" v-slot:[slot]="scope">
            <slot :name="slot" v-bind="scope"/>
        </template>
    </v-data-table>
</template>

<script src="./list.component.ts"></script>
