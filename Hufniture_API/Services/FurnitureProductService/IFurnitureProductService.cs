using Hufniture_API.Models;
using Hufniture_API.ViewModel;

namespace Hufniture_API.Services.FurnitureProductService
{
    public interface IFurnitureProductService
    {
        Task<IEnumerable<FurniturePrProductVM>> GetAll();
        Task<FurniturePrProductVM?> GetByIdAsync(Guid id);

        Task<IEnumerable<FurniturePrProductVM>> GetRandomProductsAsync(int take);

        Task<IEnumerable<FurniturePrProductVM>> GetFilteredProductsAsync(
        decimal? minPrice, decimal? maxPrice, IEnumerable<Guid> categoryIds, IEnumerable<Guid> colorIds);

        Task AddProduct(FurnitureProductVM model);
        Task DeleteProduct(Guid id);
        Task UpdateProduct(Guid id, FurnitureProductVM model);


        Task<IEnumerable<Color>> GetAllColors();
        Task AddProductColor(ProductColorVM model);
        Task DeleteProductColor(Guid id);
        Task UpdateProductColor(Guid id, ProductColorVM model);


        Task<IEnumerable<Review>> GetReviewByProdId(Guid id);

        Task<FurniturePrProductVM?> GetBestSellingProductAsync();


    }
}
