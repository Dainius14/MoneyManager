import axios, { AxiosInstance, AxiosError } from 'axios';
import { TokenService } from '@/services/token.service';
// import { UserModule } from '@/store/modules/user';

// export default axios.create({
//     baseURL: 'http://localhost:5500/api'
// });

class ApiService {
    private axios: AxiosInstance;

    private isRefreshing: boolean = false;

    constructor() {
        this.axios = axios.create({
            baseURL: 'http://localhost:5500/api',
            headers: {
                Authorization: 'Bearer ' + TokenService.getAccessToken()
            }
        });

        // this.axios.interceptors.response.use(
        //     (success) => success,
        //     async (error: AxiosError) => {
        //         if (error.response?.status === 401 && error.response?.headers['token-expired']
        //             && !this.isRefreshing) {
        //             const refreshed = await this.refreshAccessToken();
        //             if (refreshed) {
        //                 error.config.headers.Authorization = 'Bearer ' + TokenService.getAccessToken();
        //                 return axios(error.config);
        //             }
        //         }

        //         return Promise.reject(error);
        //     }
        // );
    }

    public setAuthorizationHeader(accessToken: string) {
        this.axios.defaults.headers.Authorization = 'Bearer ' + accessToken;
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
            TokenService.saveAccessToken(response.data.refreshToken);

            this.axios.defaults.headers.Authorization = 'Bearer ' + response.data.accessToken;
            return true;
        }
        catch (e) {
            return false;
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
