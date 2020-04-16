
import { Component, Vue, Prop } from "vue-property-decorator";
import { DataTableHeader } from 'vuetify';
import CreateItemCard from '@/components/create-item-card.component.vue';
import { IListItem } from './list-item.model';

export interface EditDialogField {
    label: string;
    value: string;
    type?: string;
    rules?: ((value: string) => (boolean|string))[];
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

    @Prop({ type: String, required: true })
    title!: string;

    @Prop({ type: Object, required: true })
    newItem!: any;

    @Prop({ type: Array, required: false })
    editDialogFields!: EditDialogField[];

    @Prop({ type: Boolean, required: true })
    isLoading!: boolean;

    @Prop({ type: String, required: false })
    sortBy?: string;
    
    @Prop({ type: Boolean, required: false, default: false })
    sortDesc?: boolean;


    showEditDialog: boolean = false;
    savingItem: boolean = false;
    deletingItem: boolean = false;
    disableDeleteDialogButtons: boolean = false;
    disableEditDialogButtons: boolean = false;

    get formTitle() {
        return this.editedIndex === -1 ? 'New item' : 'Edit item';
    }

    showDeleteDialog: { [_: string]: boolean } = {};

    editedItem: any = {};
    editedCustomItem: any = {};
    editedIndex = -1;

    onNewItemButtonClicked() {
        this.editedIndex = -1;
        this.editedItem = { ...this.newItem };
        this.showEditDialog = true;
        this.disableEditDialogButtons = false;
    }

    onEditItemClicked(item: IListItem) {
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

    onCustomUpdate(item: IListItem) {
        this.editedCustomItem = item;
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
