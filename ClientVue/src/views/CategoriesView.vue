<template>
    <list
        title="Categories"
        :headers="headers"
        :items="categoriesState.categories"
        :newItem="newItem"
        :editDialogFields="editDialogFields"
        :isLoading="isLoading"
        @editDialogSaveClicked="onEditDialogSaveClicked($event)"
        @deleteItemButtonClicked="onDeleteItemButtonClicked($event)"
        v-bind:showEditDialog.sync="showEditDialog"
    >
    </list>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { CategoriesModule } from '@/store/modules/categories-module.store';
import { ToastService } from '@/services/snackbar.service';
import { Category } from '@/models/category.model';
import { DataTableHeader } from 'vuetify';
import { List } from '@/components/list';
import { EditDialogField } from '@/components/list/list.component';

@Component({
    components: {
        List
    }
})
export default class CategoriesView extends Vue {
    showEditDialog: boolean = false;

    get categoriesState() {
        return CategoriesModule;
    }

    
    headers: DataTableHeader<Category>[] = [
        {
            text: 'Name',
            value: 'name',
            align: 'start',
            sortable: true,
            filterable: false,
        },
        {
            text: 'Created',
            value: 'createdAt',
            sortable: false,
            filterable: false,
        },
        {
            text: 'Updated',
            value: 'updatedAt',
            sortable: false,
            filterable: false,
        },
        {
            text: 'Actions',
            value: 'actions',
            align: 'end',
            sortable: false,
            filterable: false,
        },
    ];

    editDialogFields: EditDialogField[] = [
        {
            label: 'Name',
            value: 'name'
        },
    ];

    newItem = new Category();
    isLoading = true;

    async created() {
        try {
            await CategoriesModule.getCategories();
        }
        catch (e) {
            ToastService.show(e, { color: 'error' });
        }
        this.isLoading = false;
    }

    async onDeleteItemButtonClicked(item: Category) {
        try {
            await CategoriesModule.removeCategory(item);
        }
        catch (e) {
            ToastService.show(e, { color: 'error' });
        }
    }

    async onEditDialogSaveClicked(item: Category) {
        try {
            if (item.id !== -1) {
                await CategoriesModule.editCategory(item);
            }
            else {
                await CategoriesModule.createCategory(item);
            }
        }
        catch (e) {
            ToastService.show(e, { color: 'error' });
            return;
        }
        this.showEditDialog = false;
    }

    async deleteAll() {
        CategoriesModule.categories.forEach(async t => await CategoriesModule.removeCategory(t));
    }
}
</script>
