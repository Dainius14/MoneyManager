import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import store from '@/store';
import { Category } from '@/models/category.model';
import { CategoriesApi } from '@/services/categories.api';

const name = 'categories';
if ((store as any).state[name]) {
    store.unregisterModule(name);
}

@Module({ name, store, dynamic: true, namespaced: true })
class Categories extends VuexModule {
    loaded: boolean = false;
    categories: Category[] = [];

    @Mutation
    addItems(items: Category[]) {
        this.categories = this.categories.concat(items);
        this.loaded = true;
    }
    
    @Mutation
    removeItem(item: Category) {
        const index = this.categories.indexOf(item);
        this.categories.splice(index, 1);
    }
    @Mutation
    replaceItem(newItem: Category) {
        const index = this.categories.findIndex(a => a.id === newItem.id);
        this.categories.splice(index, 1, newItem);
    }

    @Mutation
    clear() {
        this.categories.splice(0, this.categories.length);
    }

    @Action({ rawError: true })
    async getCategories() {
        this.clear();
        this.addItems(await CategoriesApi.getCategories());
    }

    @Action({ rawError: true })
    async removeCategory(item: Category) {
        if (await CategoriesApi.deleteCategory(item.id)) {
            this.removeItem(item);
        }
    }

    @Action({ rawError: true })
    async editCategory(item: Category) {
        const updated = await CategoriesApi.editCategory(item);
        this.replaceItem(updated);
    }

    @Action({ rawError: true })
    async createCategory(item: Category) {
        const created = await CategoriesApi.createCategory(item.name);
        this.addItems([created]);
    }

}

export const CategoriesModule = getModule(Categories);
