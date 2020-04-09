import api from '@/services/api';
import { CategoryMapper } from '@/models/mappers';
import { GetCategoryDto, Category } from '@/models/category.model';

export class CategoriesApi {
    static async getCategories(): Promise<Category[]> {
        const dtos = await api.get<GetCategoryDto[]>('/categories');
        return dtos.map((dto) => {
            return CategoryMapper.fromGetCategoryDto(dto);
        });
    }
    
    static async createCategory(name: string): Promise<Category> {
        const dto = await api.post<GetCategoryDto>('/categories', { name });
        return CategoryMapper.fromGetCategoryDto(dto);
    }
    
    static async editCategory(Category: Category): Promise<Category> {
        const sendDto = CategoryMapper.toDto(Category);
        const getDto = await api.put<GetCategoryDto>('/categories/' + Category.id, sendDto);
        return CategoryMapper.fromGetCategoryDto(getDto);
    }
    
    static async deleteCategory(id: number): Promise<boolean> {
        return await api.delete('/categories/' + id);
    }
}
