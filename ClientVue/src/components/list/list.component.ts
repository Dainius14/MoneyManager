
import { Component, Vue, Prop } from 'vue-property-decorator';
import { DataTableHeader } from 'vuetify';
import CreateItemCard from '@/components/create-item-card.component.vue';
import { IListItem } from './list-item.model';

export interface EditDialogField {
    label: string;
    value: string;
    type?: string;
    rules?: ((value: string) => (boolean|string))[];
    maxLength?: number;
    required?: boolean;
}

@Component({
    components: {
        CreateItemCard
    }
})
export default class ListComponent extends Vue {
    @Prop({ type: Array, required: true })
    headers!: DataTableHeader[];

    @Prop({ type: Array, required: true })
    items!: IListItem[];

    @Prop({ type: Array, required: false })
    editDialogFields!: EditDialogField[];

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

    showEditDialog: boolean = false;
    savingItem: boolean = false;
    deletingItem: boolean = false;
    disableDeleteDialogButtons: boolean = false;
    disableEditDialogButtons: boolean = false;

    get formTitle() {
        return this.editedIndex === -1 ? this.newItemText : this.editItemText;
    }

    showDeleteDialog: { [_: string]: boolean } = {};

    editedItem: any = {};
    editedCustomItem: any = {};
    editedIndex = -1;

    field: string = '';

    isFormValid: boolean = false;

    onNewItemButtonClicked() {
        (this.$refs.form as any)?.resetValidation();
        this.editedIndex = -1;
        this.editedItem = {};
        this.showEditDialog = true;
        this.disableEditDialogButtons = false;
    }

    onEditItemClicked(item: IListItem) {
        (this.$refs.form as any)?.resetValidation();
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
        this.$emit('save-item-clicked', {
            item: this.editedCustomItem || this.editedItem,
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
    onSaveSuccess() {
        this.savingItem = false;
        this.disableEditDialogButtons = true;
        this.showEditDialog = false;
    }
    onSaveError() {
        return;
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

}

export interface ListEventArgs<T> {
    item: T;
    onStart: Function;
    onSuccess: Function;
    onError: Function;
}
