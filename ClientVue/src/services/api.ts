import axios, { AxiosInstance, AxiosError } from 'axios';
import { TokenService } from '@/services/token.service';

class ApiService {
    private axios: AxiosInstance;

    private isRefreshing: boolean = false;

    constructor() {
        this.axios = axios.create({
            baseURL: process.env.VUE_APP_BASE_API_URL,
            headers: {
                Authorization: 'Bearer ' + TokenService.getAccessToken()
            }
        });

        this.createRefreshTokenResponseInterceptor();
    }

    private createRefreshTokenResponseInterceptor() {
        const interceptor = this.axios.interceptors.response.use(
            (success) => success,
            (error: AxiosError) => {
                if (error.response == null
                    || error.response.status !== 401
                    || !error.response.headers['token-expired']) {
                    return Promise.reject(error);
                }

                this.axios.interceptors.response.eject(interceptor);

                return this.refreshAccessToken()
                    .then((accessToken) => {
                        error.config.headers.Authorization = this.getAuthHeaderValue(accessToken);
                        return axios(error.config);
                    })
                    .catch((ex) => {
                        return Promise.reject(ex);
                    })
                    .finally(() => {
                        this.createRefreshTokenResponseInterceptor();
                    });

            }
        );
    }

    private getAuthHeaderValue(accessToken: string) {
        return 'Bearer ' + accessToken;
    }

    public setAuth(accessToken: string, refreshToken: string) {
        this.axios.defaults.headers.Authorization = this.getAuthHeaderValue(accessToken);
        TokenService.saveAccessToken(accessToken);
        TokenService.saveRefreshToken(refreshToken);
    }

    public resetAuth() {
        this.axios.defaults.headers.Authorization = undefined;
        TokenService.removeAccessToken();
        TokenService.removeRefreshToken();
    }

    public hasAccessToken() {
        return !!TokenService.getAccessToken();
    }

    private async refreshAccessToken(): Promise<string> {
        this.isRefreshing = true;
        try {
            const response = await this.axios.post('/users/refreshToken', {
                accessToken: TokenService.getAccessToken(),
                refreshToken: TokenService.getRefreshToken()
            });

            this.setAuth(response.data.accessToken, response.data.refreshToken);
            return response.data.accessToken;
        }
        finally {
            this.isRefreshing = false;
        }
    }

    async get<T>(resource: string, params?: object): Promise<T> {
        if (params) {
            resource += '?' + this.getQueryString(params);
        }
        const response = await this.axios.get(resource);
        return response.data;
    }

    async post<T>(resource: string, data: object): Promise<T> {
        const response = await this.axios.post(resource, data);
        return response.data;
    }

    async put<T>(resource: string, data: object): Promise<T> {
        const response = await this.axios.put(resource, data);
        return response.data;
    }

    async delete(resource: string): Promise<boolean> {
        const response = await this.axios.delete(resource);
        return response.status >= 200 && response.status < 300;
    }

    private getQueryString(params: any): string {
        return new URLSearchParams(params).toString();
    }
}

export default new ApiService();
