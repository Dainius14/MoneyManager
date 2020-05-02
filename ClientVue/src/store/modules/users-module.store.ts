import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import store from '@/store';
import api from '@/services/api';
import { Authentication, User } from '@/models/user.models';

@Module({ name: 'user', dynamic: true, store, namespaced: true })
class UserStore extends VuexModule {

    public currentUser: User = new User();

    get isLoggedIn() {
        return this.currentUser.id !== -1;
    }

    @Mutation
    private _setCurrentUser(user: User) {
        this.currentUser = user;
    }

    @Action({ rawError: true })
    public async login({ email, password }: { email: string; password: string }) {
        const response = await api.post<Authentication>('/users/authenticate', { email, password });
        this._setCurrentUser(response.user);
        api.setAuth(response.accessToken, response.refreshToken);
    }

    @Action({ rawError: true })
    public logout() {
        this._setCurrentUser(new User());
        api.resetAuth();
    }

    @Action({ rawError: true })
    public async changeEmail(email: string) {
        await api.post<void>('/users/email', { email });
        this.logout();
    }

    @Action({ rawError: true })
    public async changePassword(data: object) {
        await api.post<void>('/users/password', data);
        this.logout();
    }

    @Action({ rawError: true })
    public async getCurrentUser() {
        if (!api.hasAccessToken()) {
            return;
        }

        try {
            const response = await api.get<User>('/users/current');
            this._setCurrentUser(response);
        }
        catch {
            return;
        }
    }
}

export const UserModule = getModule(UserStore);
