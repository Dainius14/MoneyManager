
import { Component, Vue, Prop } from "vue-property-decorator";
import { DataTableHeader } from 'vuetify';
import CreateItemCard from '@/components/create-item-card.component.vue';

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
    items!: any[];

    @Prop({ type: String, required: true })
    title!: string;

    @Prop({ type: Object, required: true })
    newItem!: any;

    @Prop({ type: Array, required: false })
    editDialogFields!: EditDialogField[];

    @Prop({ type: Boolean, required: true })
    showEditDialog!: boolean;

    @Prop({ type: Boolean, required: true })
    isLoading!: boolean;

    @Prop({ type: String, required: false })
    sortBy?: string;
    
    @Prop({ type: Boolean, required: false, default: false })
    sortDesc?: boolean;
    
    @Prop({ type: Boolean, required: true })
    savingItem!: boolean;

    get formTitle() {
        return this.editedIndex === -1 ? 'New item' : 'Edit item';
    }

    editedItem: any = {};
    editedIndex = -1;

    onNewItemButtonClicked() {
        this.editedIndex = -1;
        this.editedItem = { ...this.newItem };
        this.emitShowEditDialog(true);
    }

    onEditItemClicked(item: any) {
        this.editedIndex = this.items.indexOf(item);
        this.editedItem = { ...item };
        this.emitShowEditDialog(true);
    }

    onDeleteItemClicked(item: any) {
        this.$emit('deleteItemButtonClicked', item);
    }
    onEditDialogCloseClicked() {
        this.emitShowEditDialog(false);
    }

    onEditDialogSaveClicked() {
        this.$emit('editDialogSaveClicked', this.editedItem);
    }

    
    private emitShowEditDialog(value: boolean) {
        this.$emit('update:showEditDialog', value);
    }
}
