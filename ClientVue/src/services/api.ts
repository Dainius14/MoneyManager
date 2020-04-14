import axios, { AxiosInstance, AxiosError, AxiosRequestConfig } from 'axios';
import { TokenService } from '@/services/token.service';
import router, { Routes } from '@/router';
// import { UserModule } from '@/store/modules/user';

// export default axios.create({
//     baseURL: 'http://localhost:5500/api'
// });

class ApiService {
    private axios: AxiosInstance;

    private isRefreshing: boolean = false;

    constructor() {
        this.axios = axios.create({
            baseURL: 'https://localhost:5501/api',
            headers: {
                Authorization: 'Bearer ' + TokenService.getAccessToken()
            }
        });

        this.createRefreshTokenResponseInterceptor();
    }

    private createRefreshTokenResponseInterceptor() {
        const interceptor = this.axios.interceptors.response.use(
            (success) => success,
            async (error: AxiosError) => {
                if (error.response?.status !== 401) {
                    return Promise.reject(error);
                }
                if (!error.response?.headers['token-expired']) {
                    router.push(Routes.Login.path);
                    return Promise.reject(error);
                }
                this.axios.interceptors.response.eject(interceptor);

                try {
                    console.log('refreshing token');
                    await this.refreshAccessToken();
                }
                catch (ex) {
                    console.log('refreshing did not succeed', ex);
                    router.push(Routes.Login.path);
                    return Promise.reject(ex);
                }
                finally {
                    this.createRefreshTokenResponseInterceptor();
                }
                this.setAuthorizationHeader(TokenService.getAccessToken()!, error.config);
                return axios(error.config);
            }
        );
    }

    public setAuthorizationHeader(accessToken: string, requestConfig?: AxiosRequestConfig) {
        const header = 'Bearer ' + accessToken;
        if (requestConfig) {
            requestConfig.headers.Authorization = header;
            return
        }
        this.axios.defaults.headers.Authorization = header;
    }

    public resetAuthorizationHeader() {
        this.axios.defaults.headers.Authorization = undefined;
    }

    private async refreshAccessToken() {
        this.isRefreshing = true;
        try {
            const response = await this.axios.post('/users/refreshToken', {
                accessToken: TokenService.getAccessToken(),
                refreshToken: TokenService.getRefreshToken()
            });
            TokenService.saveAccessToken(response.data.accessToken);
            TokenService.saveRefreshToken(response.data.refreshToken);

            this.setAuthorizationHeader(response.data.accessToken);
        }
        catch (ex) {
            console.log(ex)
            throw ex;
        }
        finally {
            this.isRefreshing = false;
        }
    }

    async get<T>(resource: string): Promise<T> {
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
}

export default new ApiService();
