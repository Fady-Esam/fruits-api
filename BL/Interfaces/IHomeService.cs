using FruitsAppBackEnd.Domain;
using FruitsAppBackEnd.Models;

namespace FruitsAppBackEnd.BL.Interfaces
{
    public interface IHomeService
    {
        Task<ApiResponse> AddProduct(ProductDto ProductDto);
        Task<ApiResponse> AddCategory(CategoryDto CategoryDto);
        Task<ApiResponse> MakeOrder(OrderDto OrderDto);
        Task<bool> IsCategoryWithIdFound(int CategoryId);
        Task<bool> IsProductWithIdFound(int ProductId);
        Task<bool> IsOrderWithIdFound(int OrderId);
        Task<ApiResponse> GetAllProduct();
        Task<ApiResponse> GetAllCategory();
        Task<ApiResponse> GetProductsByCategoryId(int CategoryId);
        Task<ApiResponse> GetProductsByOrderId(int OrderId);
        Task<ApiResponse> GetOrdersByProductId(int ProductId);
        Task<ApiResponse> AddOrderProduct(OrderProductDto orderProductDto);
        Task<ApiResponse> GetProductById(int Id);
        Task<ApiResponse> GetUserOrders(string UserId);
        Task<ApiResponse> CancelOrder(int OrderId);
        Task<ApiResponse> GetBestSellingProducts();
        Task<ApiResponse> GetProductAVGRating(int ProductId);
        Task<ApiResponse> AddRate(RateDto RateDto);
        Task<ApiResponse> AddReview(ReviewDto ReviewDto);
        Task<ApiResponse> GetReviews(int ProductId);
        Task<ApiResponse> GetRatesCount(int ProductId);

    }
}
