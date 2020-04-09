import api from '@/services/api';
import { DashboardData } from '@/models/dashboard-data.model';
import { toIsoDate } from '@/utils/utils';

export class DashboardApi {
    static async getData(fromDate: Date, toDate: Date): Promise<DashboardData> {
        return await api.get<DashboardData>(`/dashboard?fromDate=${toIsoDate(fromDate)}&toDate=${toIsoDate(toDate)}`);
    }
}
