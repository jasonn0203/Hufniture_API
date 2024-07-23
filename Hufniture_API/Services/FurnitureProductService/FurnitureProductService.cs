using Hufniture_API.Models;
using Hufniture_API.UnitOfWork;
using Hufniture_API.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hufniture_API.Services.FurnitureProductService
{
    public class FurnitureProductService : IFurnitureProductService
    {

        public IUnitOfWork _unitOfWork;
        public FurnitureProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FurniturePrProductVM>> GetAll()
        {
            return await _unitOfWork.FurnitureProductRepository.GetAllAsync();
        }

        public async Task<IEnumerable<FurniturePrProductVM>> GetRandomProductsAsync(int take)
        {
            // Get all products
            var allProducts = await _unitOfWork.FurnitureProductRepository.GetAllAsync();

            // Shuffle the list and take the specified number of products
            var random = new Random();
            var randomProducts = allProducts.OrderBy(x => random.Next()).Take(take);

            return randomProducts;
        }


        public async Task AddProduct(FurnitureProductVM model)
        {
            //Check if exists color
            var color = await _unitOfWork.ColorRepository.GetByIdAsync(model.ColorId);
            if (color == null)
            {
                throw new ArgumentException($"Màu sắc với ID {model.ColorId} không tồn tại.");

            }

            //Check if exists color
            var category = await _unitOfWork.FurnitureCategoryRepository.GetByIdAsync(model.FurnitureCategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Danh mục nội thất với ID {model.FurnitureCategoryId} không tồn tại.");

            }
            //Check if exists type
            var type = await _unitOfWork.FurnitureTypeRepository.GetByIdAsync(model.FurnitureTypeId);
            if (type == null)
            {
                throw new ArgumentException($"Loại nội thất với ID {model.FurnitureTypeId} không tồn tại.");

            }


            var product = new FurniturePrProductVM
            {
                Name = model.Name,
                Description = model.Description,
                ImageURL = model.ImageURL,
                Price = model.Price,

                ColorId = model.ColorId,
                FurnitureCategoryId = model.FurnitureCategoryId,
                FurnitureTypeId = model.FurnitureTypeId
            };
            await _unitOfWork.FurnitureProductRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<FurniturePrProductVM?> GetByIdAsync(Guid id)
        {
            return await _unitOfWork.FurnitureProductRepository.GetByIdAsync(id);
        }


        public async Task UpdateProduct(Guid id, FurnitureProductVM model)
        {
            var existingProduct = await _unitOfWork.FurnitureProductRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                throw new ArgumentException($"Sản phẩm không tồn tại với ID {id}");
            }

            //Map objects
            existingProduct.Name = model.Name;
            existingProduct.Description = model.Description;
            existingProduct.ImageURL = model.ImageURL;
            existingProduct.Price = model.Price;
            existingProduct.ColorId = model.ColorId;
            existingProduct.FurnitureCategoryId = model.FurnitureCategoryId;
            existingProduct.FurnitureTypeId = model.FurnitureTypeId;

            await _unitOfWork.FurnitureProductRepository.UpdateAsync(existingProduct);
            await _unitOfWork.SaveChangesAsync();
        }



        public async Task DeleteProduct(Guid id)
        {
            var prod = await _unitOfWork.FurnitureProductRepository.GetByIdAsync(id);
            if (prod == null)
            {
                throw new Exception($"Không tồn tại SP với id : {id} này.");
            }

            await _unitOfWork.FurnitureProductRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }


        //COLOR

        public async Task AddProductColor(ProductColorVM model)
        {

            var color = new Color
            {
                Name = model.Name,
                FurnitureProducts = new List<FurniturePrProductVM>()
            };
            await _unitOfWork.ColorRepository.AddAsync(color);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteProductColor(Guid id)
        {
            var color = await _unitOfWork.ColorRepository.GetByIdAsync(id);
            if (color == null)
            {
                throw new Exception($"Không tồn tại màu sắc với ID {id} này.");
            }

            await _unitOfWork.ColorRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task UpdateProductColor(Guid id, ProductColorVM model)
        {
            var existingColor = await _unitOfWork.ColorRepository.GetByIdAsync(id);
            if (existingColor == null)
            {
                throw new ArgumentException($"Màu sắc không tồn tại với ID {id}");
            }

            existingColor.Name = model.Name;

            await _unitOfWork.ColorRepository.UpdateAsync(existingColor);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Color>> GetAllColors()
        {
            return await _unitOfWork.ColorRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewByProdId(Guid id)
        {
            return await _unitOfWork.ReviewRepository.FindByCondition(ft => ft.FurnitureProductId == id).AsNoTracking().ToListAsync();
        }

    

        public async Task<FurniturePrProductVM?> GetBestSellingProductAsync()
        {
            var bestSellingProduct = await _unitOfWork.OrderItemRepository
                .FindByCondition(oi => true)
                .GroupBy(oi => oi.FurnitureProductId)
                .OrderByDescending(g => g.Count())
                .Select(g => new { ProductId = g.Key, Count = g.Count() })
                .FirstOrDefaultAsync();

            if (bestSellingProduct == null)
            {
                return null;
            }

            var product = await _unitOfWork.FurnitureProductRepository.GetByIdAsync(bestSellingProduct.ProductId);

            return product;
        }

        public async Task<IEnumerable<FurniturePrProductVM>> GetFilteredProductsAsync(decimal? minPrice, decimal? maxPrice, IEnumerable<Guid> categoryIds, IEnumerable<Guid> colorIds)
        {
            var productsQuery = _unitOfWork.FurnitureProductRepository.FindByCondition(p => true)
        .AsQueryable(); // Ensure it's IQueryable before including

            // Correctly use Include with navigation properties
            productsQuery = productsQuery
                .Include(p => p.FurnitureCategory) // Include related FurnitureCategory
                .Include(p => p.Color);

            if (minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price <= maxPrice.Value);
            }

            if (categoryIds != null && categoryIds.Any())
            {
                productsQuery = productsQuery.Where(p => categoryIds.Contains(p.FurnitureCategoryId));
            }

            if (colorIds != null && colorIds.Any())
            {
                productsQuery = productsQuery.Where(p => colorIds.Contains(p.ColorId));
            }

            var products = await productsQuery.ToListAsync();
            return products;
        }
    }
}
