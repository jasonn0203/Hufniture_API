using Hufniture_API.Data;
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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HufnitureDbContext _context;
        private IFurnitureCategoryRepository _furnitureCategoryRepository;
        private IFurnitureTypeRepository _furnitureTypeRepository;
        private IFurnitureProductRepository _furnitureProductRepository;
        private IColorRepository _colorRepository;
        private IReviewRepository _reviewRepository;
        private IUserRepository _userRepository;
        private IOrderRepository _orderRepository;
        private IOrderItemRepository _orderItemRepository;

        public UnitOfWork(HufnitureDbContext context)
        {
            _context = context;
        }

        public IFurnitureCategoryRepository FurnitureCategoryRepository
        { get { return _furnitureCategoryRepository ??= new FurnitureCategoryRepository(_context); } }

        public IFurnitureTypeRepository FurnitureTypeRepository
        { get { return _furnitureTypeRepository ??= new FurnitureTypeRepository(_context); } }

        public IFurnitureProductRepository FurnitureProductRepository
        {
            get { return _furnitureProductRepository ??= new FurnitureProductRepository(_context); }
        }
        public IColorRepository ColorRepository
        {
            get { return _colorRepository ??= new ColorRepository(_context); }
        }

        public IReviewRepository ReviewRepository
        {
            get { return _reviewRepository ??= new ReviewRepository(_context); }
        }
        public IUserRepository UserRepository
        {
            get { return _userRepository ??= new UserRepository(_context); }
        }

        public IOrderRepository OrderRepository
        {
            get { return _orderRepository ??= new OrderRepository(_context); }
        }

        public IOrderItemRepository OrderItemRepository
        {
            get { return _orderItemRepository ??= new OrderItemRepository(_context); }
        }



        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
