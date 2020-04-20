import { Component, Prop, Ref, Vue } from 'vue-property-decorator';
import { DataTableHeader } from 'vuetify';
import CreateItemCard from '@/components/create-item-card.component.vue';
import { IListItem } from './list-item.model';
import DynamicInput from '@/components/list/dynamic-input.component.vue';
import { InputOptions } from '@/components/list/dynamic-input.component';
import { isEmpty } from '@/utils/utils';
import { ToastService } from '@/services/snackbar.service';

@Component({
    components: {
        CreateItemCard,
        DynamicInput
    }
})
export default class ListComponent extends Vue {
    @Ref() private readonly form?: any;

    @Prop({ type: Array, required: true })
    headers!: DataTableHeader[];

    @Prop({ type: Array, required: true })
    items!: IListItem[];

    @Prop({ type: Array, required: false })
    editDialogFields!: InputOptions[];

    @Prop({ type: Boolean, required: true })
    isLoading!: boolean;

    @Prop({ type: String, required: false })
    sortBy?: string;
    
    @Prop({ type: Boolean, required: false, default: false })
    sortDesc?: boolean;

    @Prop({ type: Boolean, required: false, default: false })
    enablePagination?: boolean;


    @Prop({ type: String, required: true })
    title!: string;

    @Prop({ type: String, required: false, default: 'New item' })
    newItemText!: string;

    @Prop({ type: String, required: false, default: 'Edit item' })
    editItemText!: string;

    @Prop({ type: Boolean, required: false, default: false })
    showKeepOpenButtons!: boolean;

    showEditDialog: boolean = false;
    savingItem: boolean = false;
    deletingItem: boolean = false;
    disableDeleteDialogButtons: boolean = false;
    disableEditDialogButtons: boolean = false;
    keepDialogOpen: boolean = false;
    keepDialogValues: boolean = false;

    get isNewItem() {
        return this.editedIndex === -1;
    }

    get formTitle() {
        return this.isNewItem ? this.newItemText : this.editItemText;
    }

    showDeleteDialog: { [_: string]: boolean } = {};

    editedItem: any = {};
    editedCustomItem: any = {};
    editedIndex = -1;

    isFormValid: boolean = false;

    onNewItemButtonClicked() {
        this.resetFormForNewItem();
    }

    onEditItemClicked(item: IListItem) {
        this.form?.resetValidation();
        this.editedIndex = this.items.indexOf(item);
        this.editedItem = { ...item };
        this.showEditDialog = true;
        this.disableEditDialogButtons = false;
    }

    onDeleteItemClicked(item: IListItem) {
        this.$emit('delete-item-clicked', {
            item,
            onStart: this.onDeleteStart,
            onSuccess: () => this.onDeleteSuccess(item.id),
            onError: this.onDeleteError
        } as ListEventArgs<any>);
    }
    onEditDialogCloseClicked() {
        this.showEditDialog = false;
    }

    onEditDialogSaveClicked() {
        const item = isEmpty(this.editedCustomItem) ? this.editedItem : this.editedCustomItem;
        this.$emit('save-item-clicked', {
            item,
            onStart: this.onSaveStart,
            onSuccess: this.onSaveSuccess,
            onError: this.onSaveError
        } as ListEventArgs<any>);
    }

    onCustomFormChanged(item: IListItem) {
        this.editedCustomItem = item;
    }

    onCustomIsFormValidChanged(isValid: boolean) {
        this.isFormValid = isValid;
    }


    onSaveStart() {
        this.savingItem = true;
    }
    onSaveSuccess(toastText?: string) {
        this.savingItem = false;
        if (this.keepDialogOpen) {
            this.disableEditDialogButtons = false;
            this.showEditDialog = true;
            if (!this.keepDialogValues) {
                this.resetFormForNewItem();
            }
        }
        else {
            this.disableEditDialogButtons = true;
            this.showEditDialog = false;
        }
        if (toastText) {
            ToastService.success(toastText);
        }
    }
    onSaveError(ex: Error, toastText?: string) {
        console.error(ex);
        if (toastText) {
            ToastService.error(toastText + ':\n' + ex.message);
        }
    }

    onDeleteStart() {
        this.deletingItem = true;
    }
    onDeleteSuccess(itemId: number) {
        this.deletingItem = false;
        this.disableDeleteDialogButtons = true;
        this.showDeleteDialog[itemId] = false;
    }
    onDeleteError() {
        console.log('onSaveError');
    }

    private resetFormForNewItem() {
        this.form?.resetValidation();
        this.editedIndex = -1;
        this.editedItem = {};
        this.showEditDialog = true;
        this.disableEditDialogButtons = false;

        if (this.editDialogFields) {
            for (const field of this.editDialogFields) {
                if (field.defaultValue != null) {
                    this.editedItem[field.key] = field.defaultValue;
                }
            }
        }
    }
}

export interface ListEventArgs<T> {
    item: T;
    onStart: Function;
    onSuccess: (toastText?: string) => void;
    onError: (ex: Error, toastText?: string) => void;
}
