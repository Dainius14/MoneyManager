<template>
    <list
        title="Categories"
        :headers="headers"
        :items="categoriesState.categories"
        :newItem="newItem"
        :editDialogFields="editDialogFields"
        :isLoading="isLoading"
        @save-item-clicked="onSaveItemClicked"
        @delete-item-clicked="onDeleteItemClicked"
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
import { EditDialogField, ListEventArgs } from '@/components/list/list.component';

@Component({
    components: {
        List
    }
})
export default class CategoriesView extends Vue {
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

    get categoriesState() {
        return CategoriesModule;
    }


    async created() {
        try {
            await CategoriesModule.getCategories();
        }
        catch (e) {
            ToastService.show(e, { color: 'error' });
        }
        this.isLoading = false;
    }

    async onDeleteItemClicked({ item, onStart, onSuccess, onError }: ListEventArgs<Category>) {
        onStart();
        try {
            await CategoriesModule.removeCategory(item);
            onSuccess();
        }
        catch (e) {
            ToastService.show(e, { color: 'error' });
            onError();
        }
    }

    async onSaveItemClicked({ item, onStart, onSuccess, onError }: ListEventArgs<Category>) {
        onStart();
        try {
            if (item.id !== -1) {
                await CategoriesModule.editCategory(item);
            }
            else {
                await CategoriesModule.createCategory(item);
            }
            onSuccess();
        }
        catch (e) {
            ToastService.show(e, { color: 'error' });
            onError();
        }
    }

    async deleteAll() {
        CategoriesModule.categories.forEach(async t => await CategoriesModule.removeCategory(t));
    }
}
</script>
