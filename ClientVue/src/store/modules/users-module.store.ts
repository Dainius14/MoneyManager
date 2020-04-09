import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import store from '@/store';
import { TokenService } from '@/services/token.service';
import api from '@/services/api';
import { Authentication } from '@/models/user.models';

@Module({ name: 'user', dynamic: true, store, namespaced: true })
class User extends VuexModule {
    public accessToken: string = TokenService.getAccessToken() ?? '';
    public refreshToken: string = TokenService.getRefreshToken() ?? '';

    @Mutation
    private setAccessToken(token: string) {
        this.accessToken = token;
    }

    @Mutation
    private setRefreshToken(token: string) {
        this.refreshToken = token;
    }

    @Action({ rawError: true })
    public async login({ email, password }: { email: string; password: string }) {
        const response = await api.post<Authentication>('/users/authenticate', { email, password });
        this.setAccessToken(response.accessToken);
        this.setRefreshToken(response.refreshToken);

        api.setAuthorizationHeader(this.accessToken);

        TokenService.saveAccessToken(response.accessToken);
        TokenService.saveRefreshToken(response.refreshToken);
    }

    @Action({ rawError: true })
    public async logout() {
        this.setAccessToken('');
        this.setRefreshToken('');

        api.resetAuthorizationHeader();

        TokenService.removeAccessToken();
        TokenService.removeRefreshToken();
    }
}

export const UserModule = getModule(User);
