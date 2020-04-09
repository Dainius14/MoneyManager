import { Category } from './category.model';

export class DashboardData {
    expensesPerCategory: { amount: number; category: Category }[] = [];
    amountsPerMonth: { expenses: number; income: number; month: string }[] = [];
}
