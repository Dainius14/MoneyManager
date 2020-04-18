import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import store from '@/store';
import { Category } from '@/models/category.model';
import { CategoriesApi } from '@/services/categories.api';
import { LoadState } from '@/models/common.models';

const name = 'categories';
if ((store as any).state[name]) {
    store.unregisterModule(name);
}

@Module({ name, store, dynamic: true, namespaced: true })
class Categories extends VuexModule {
    loadState: LoadState = LoadState.NotLoaded;
    categories: Category[] = [];

    @Mutation
    private addItems(items: Category[]) {
        this.categories = this.categories.concat(items);
    }
    
    @Mutation
    private _removeItem(item: Category) {
        const index = this.categories.indexOf(item);
        this.categories.splice(index, 1);
    }
    @Mutation
    private _replaceItem(newItem: Category) {
        const index = this.categories.findIndex(a => a.id === newItem.id);
        this.categories.splice(index, 1, newItem);
    }

    @Mutation
    private _clear() {
        this.categories.splice(0, this.categories.length);
    }
    
    @Mutation
    private _setState(state: LoadState) {
        this.loadState = state;
    }

    @Action({ rawError: true })
    async getCategories() {
        this._setState(LoadState.Loading);
        try {
            this.addItems(await CategoriesApi.getCategories());
            this._setState(LoadState.Loaded);
        }
        catch (ex) {
            this._setState(LoadState.NotLoaded);
            throw ex;
        }
    }

    @Action({ rawError: true })
    async removeCategory(item: Category) {
        if (await CategoriesApi.deleteCategory(item.id)) {
            this._removeItem(item);
        }
    }

    @Action({ rawError: true })
    async editCategory(item: Category) {
        const updated = await CategoriesApi.editCategory(item);
        this._replaceItem(updated);
    }

    @Action({ rawError: true })
    async createCategory(item: Category) {
        const created = await CategoriesApi.createCategory(item.name);
        this.addItems([created]);
    }

}

export const CategoriesModule = getModule(Categories);
