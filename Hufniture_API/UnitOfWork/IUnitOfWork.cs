using Hufniture_API.Models;
using Hufniture_API.Repositories;
using Hufniture_API.Repositories.ColorRepository;
using Hufniture_API.Repositories.FurnitureCategoryRepository;
using Hufniture_API.Repositories.FurnitureProductRepository;
using Hufniture_API.Repositories.FurnitureTypeRepository;
using Hufniture_API.Repositories.OrderItemRepository;
using Hufniture_API.Repositories.OrderRepository;
using Hufniture_API.Repositories.ReviewRepository;
using Hufniture_API.Repositories.UserRepository;

namespace Hufniture_API.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        //IRepositories
        IFurnitureCategoryRepository FurnitureCategoryRepository { get; }
        IFurnitureTypeRepository FurnitureTypeRepository { get; }
        IFurnitureProductRepository FurnitureProductRepository { get; }
        IColorRepository ColorRepository { get; }
        IReviewRepository ReviewRepository { get; }
        IUserRepository UserRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
