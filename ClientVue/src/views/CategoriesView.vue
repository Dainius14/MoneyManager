<template>
    <list
        title="Categories"
        newItemText="New category"
        editItemText="Edit category"
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
import { Component, Vue } from 'vue-property-decorator';
import { CategoriesModule } from '@/store/modules/categories-module.store';
import { ToastService } from '@/services/snackbar.service';
import { Category } from '@/models/category.model';
import { DataTableHeader } from 'vuetify';
import { List } from '@/components/list';
import { ListEventArgs } from '@/components/list/list.component';
import { notEmpty, maxLength } from '@/utils/rules';
import { LoadState } from '@/models/common.models';
import { InputOptions } from '@/components/list/dynamic-input.component';
import { IconNames } from '@/constants';

@Component({
    components: {
        List
    }
})
export default class CategoriesView extends Vue {
    readonly headers: DataTableHeader<Category>[] = [
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

    readonly editDialogFields: InputOptions[] = [
        {
            label: 'Name',
            key: 'name',
            maxLength: 30,
            required: true,
            rules: [
                notEmpty('Category must have a name'),
                maxLength(30, "Category name can't be that long")
            ],
            prependInnerIcon: IconNames.Category
        },
    ];

    newItem = new Category();

    get categoriesState() {
        return CategoriesModule;
    }

    get isLoading() {
        return CategoriesModule.loadState === LoadState.Loading;
    }


    async created() {
        if (CategoriesModule.loadState === LoadState.NotLoaded) {
            try {
            await CategoriesModule.getCategories();
            }
            catch (e) {
                ToastService.error(e);
            }
        }
    }

    async onDeleteItemClicked({ item, onStart, onSuccess, onError }: ListEventArgs<Category>) {
        onStart();
        try {
            await CategoriesModule.removeCategory(item);
            onSuccess('Category deleted successfully');
        }
        catch (e) {
            onError(e, 'There was an error deleting the category');
        }
    }

    async onSaveItemClicked({ item, onStart, onSuccess, onError }: ListEventArgs<Category>) {
        onStart();
        try {
            if (item.id != null) {
                await CategoriesModule.editCategory(item);
            }
            else {
                await CategoriesModule.createCategory(item);
            }
            onSuccess(`Category ${item.name} saved successfully`);
        }
        catch (e) {
            onError(e, 'There was an error saving the category');
        }
    }
}
</script>
