using FruitsAppBackEnd.BL.Interfaces;
using FruitsAppBackEnd.Domain;
using FruitsAppBackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;



namespace FruitsAppBackEnd.BL.Classes
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        public HomeService(ApplicationDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Task<bool> IsCategoryWithIdFound(int CategoryId)
        {
           return _context.Categories.AnyAsync(i => i.Id == CategoryId);
        }

        public Task<bool> IsProductWithIdFound(int ProductId)
        {
            return _context.Products.AnyAsync(i => i.Id == ProductId);
        }

        public Task<bool> IsOrderWithIdFound(int OrderId)
        {
            return _context.Orders.AnyAsync(i => i.Id == OrderId);
        }
        public async Task<ApiResponse> AddCategory(CategoryDto dtoCategory)
        {
            try
            {
                var category = new Category
                {
                    Name = dtoCategory.Name,
                };
                await _context.Categories.AddAsync(category);
                _context.SaveChanges();
                return new ApiResponse
                {
                    Message = "Category Added Successfully",
                    data = category,
                    StatusCode = "200",
                };
            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }
        public async Task<ApiResponse> AddProduct(ProductDto dtoProduct)
        {
            try
            {
                int maxId = _context.Categories.Count() == 0 ? 0 : _context.Categories.Max(s => s.Id);
                if (dtoProduct.CategoryId <= 0 || dtoProduct.CategoryId > maxId)
                {
                    return new ApiResponse
                    {
                        Message = "Invalid CategoryId",
                        Errors = new { Messages = new List<string> { "Invalid CategoryId" } },
                    };
                }

                if(!await IsCategoryWithIdFound(dtoProduct.CategoryId))
                {
                    return new ApiResponse
                    {
                        Message = "Category Not Found",
                        Errors = new { Messages = new List<string> { "Category Not Found" } },
                    };
                }
                byte[] imageBytes = null;
                if (dtoProduct.Image != null && dtoProduct.Image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await dtoProduct.Image.CopyToAsync(memoryStream);
                        imageBytes = memoryStream.ToArray();
                    }
                }

                var Product = new Product
                {
                    Image = imageBytes,
                    Title = dtoProduct.Title,
                    Description = dtoProduct.Description,
                    Price = dtoProduct.Price,
                    Quantity = dtoProduct.Quantity,
                    Discount = dtoProduct.Discount,
                    IsFeatured = dtoProduct.IsFeatured,
                    PriceUnit = dtoProduct.PriceUnit,
                    SellingCount = dtoProduct.SellingCount,
                    CategoryId = dtoProduct.CategoryId,
                    ProductCode = dtoProduct.ProductCode,
                    
                };
                await _context.Products.AddAsync(Product);
                _context.SaveChanges();
                return new ApiResponse
                {
                    Message = "Product Added Successfully",
                    data = new { Product.Title, Product.Description, Product.Price, Product.Quantity, Product.CategoryId, Product.Image},
                    StatusCode = "200",
                };
            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }


        public async Task<ApiResponse> MakeOrder(OrderDto dtOrder)
        {
            try
            {
                // Retrieve the user
                var user = await _userManager.FindByIdAsync(dtOrder.UserId);
                if (user == null)
                {
                    return new ApiResponse
                    {
                        Message = "User Not Found",
                        Errors = new { Messages = new List<string> { "User Not Found" } },
                    };
                }

                // Create the Order
                var order = new Order
                {
                    AppUser = user,
                    Data = dtOrder.Data,
                    Price = dtOrder.Price,
                    Quantity = dtOrder.Quantity,
                };

                // Add Order and Save
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                return new ApiResponse
                {
                    Message = "Order Made Successfully",
                    data = new { UserId = order.AppUser.Id, order.Data, order.Price, order.Quantity },
                    StatusCode = "200",
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }


        }

        public async Task<ApiResponse> GetAllCategory()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                if(categories == null)
                {
                    return new ApiResponse
                    {
                        Message = "Categories Not Found",
                        Errors = new { Messages = new List<string> { "Categories Not Found" } },
                    };
                }
                else
                {
                    string Message = categories.Count == 0 ? "Categories List is Empty" : "All Categories Fetched Successfully";
                    return new ApiResponse
                    {
                        Message = Message,
                        data = categories,
                        StatusCode = "200",
                    };
                }
            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }

        }

        public async Task<ApiResponse> GetAllProduct()
        {
            // Determine Specific Attribute
            try
            {
                var products = await _context.Products.ToListAsync();
                if (products == null)
                {
                    return new ApiResponse
                    {
                        Message = "Products Not Found",
                        Errors = new { Messages = new List<string> { "Products Not Found" } },
                    };
                }
                else
                {
                    string Message = products.Count == 0 ? "Products List is Empty" : "All Products Fetched Successfully";
                    return new ApiResponse
                    {
                        Message = Message,
                        data = products,
                        StatusCode = "200",
                    };
                }
            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }
        public async Task<ApiResponse> GetProductById(int Id)
        {
            // Determine Specific Attribute
            try
            {
                int maxId = _context.Products.Count() == 0 ? 0 : _context.Products.Max(s => s.Id);
                if (Id <= 0 || Id > maxId)
                {
                    return new ApiResponse
                    {
                        Message = "InValid ProductId",
                        Errors = new { Messages = new List<string> { "InValid ProductId" } },
                    };
                }

                var product = await _context.Products.FindAsync(Id);
                if(product != null)
                {
                    return new ApiResponse
                    {
                        Message = "Product Got Successfully",
                        data = product,
                        StatusCode = "200",
                    };
                }
                else
                {
                    return new ApiResponse
                    {
                        Message = "Product Not Found",
                        Errors = new { Messages = new List<string> { "Product Not Found" } },
                    };
                }
            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }

        public async Task<ApiResponse> GetProductsByCategoryId(int CategoryId)
        {
            try
            {
                int maxId = _context.Categories.Count() == 0 ? 0 : _context.Categories.Max(s => s.Id);
                if (CategoryId <= 0 || CategoryId > maxId)
                {
                    return new ApiResponse
                    {
                        Message = "InValid CategoryId",
                        Errors = new { Messages = new List<string> { "InValid CategoryId" } },
                    };
                }

                if (!await IsCategoryWithIdFound(CategoryId))
                {
                    return new ApiResponse
                    {
                        Message = "Category Not Found",
                        Errors = new { Messages = new List<string> { "Category Not Found" } },
                    };
                }

                var products = await _context.Products.Join(
                    _context.Categories,
                    (p) => p.CategoryId,
                    (c) => c.Id,
                    (p, c) => new 
                    {
                        p.Title,
                        p.Description,
                        p.Price,
                        p.Quantity,
                        p.Discount,
                        p.IsFeatured,
                        p.PriceUnit,
                        p.SellingCount,
                        p.CategoryId,
                    }
                    ).ToListAsync();
                if (products == null)
                {
                    return new ApiResponse
                    {
                        Message = "Products Not Found",
                        Errors = new { Messages = new List<string> { "Products Not Found" } },
                    };
                }
                else
                {
                    string Message = products.Count == 0 ? "Products List is Empty" : "All Products Fetched Successfully";
                    return new ApiResponse
                    {
                        Message = Message,
                        data = products,
                        StatusCode = "200",
                    };
                }
            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }

        public async Task<ApiResponse> GetProductsByOrderId(int OrderId)
        {
            try
            {
                int maxId = _context.Orders.Count() == 0 ? 0 : _context.Orders.Max(s => s.Id);
                if (OrderId <= 0 || OrderId > maxId)
                {
                    return new ApiResponse
                    {
                        Message = "InValid OrderId",
                        Errors = new { Messages = new List<string> { "InValid OrderId" } },
                    };
                }

                if (!await IsOrderWithIdFound(OrderId))
                {
                    return new ApiResponse
                    {
                        Message = "Order Not Found",
                        Errors = new { Messages = new List<string> { "Order Not Found" } },
                    };
                }

                var products = await _context.OrderProducts.Join(
                    _context.Products,
                    op => op.ProductId,
                    p => p.Id,
                    (op, p) => new {
                        p.Title,
                        p.Description,
                        p.Price,
                        p.Quantity,
                        p.Discount,
                        p.IsFeatured,
                        p.PriceUnit,
                        p.SellingCount,
                        p.CategoryId,
                    }
                ).ToListAsync();
                if (products == null)
                {
                    return new ApiResponse
                    {
                        Message = "Products Not Found",
                        Errors = new { Messages = new List<string> { "Products Not Found" } },
                    };
                }
                else
                {
                    string Message = products.Count == 0 ? "Products List is Empty" : "All Products Fetched Successfully";
                    return new ApiResponse
                    {
                        Message = Message,
                        data = products,
                        StatusCode = "200",
                    };
                }
            }

            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }




        public async Task<ApiResponse> GetOrdersByProductId(int ProductId)
        {
            try
            {
                int maxId = _context.Products.Count() == 0 ? 0 : _context.Products.Max(s => s.Id);
                if (ProductId <= 0 || ProductId > maxId)
                {
                    return new ApiResponse
                    {
                        Message = "InValid ProductId",
                        Errors = new { Messages = new List<string> { "InValid ProductId" } },
                    };
                }

                if (!await IsProductWithIdFound(ProductId))
                {
                    return new ApiResponse
                    {
                        Message = "Product Not Found",
                        Errors = new { Messages = new List<string> { "Product Not Found" } },
                    };
                }

                var orders = await _context.OrderProducts.Join(
                    _context.Orders,
                    op => op.ProductId,
                    o => o.Id,
                    (op, o) => new { UserId = o.AppUser.Id, o.Data, o.Price, o.Quantity }
                ).ToListAsync();
                if (orders == null)
                {
                    return new ApiResponse
                    {
                        Message = "Orders Not Found",
                        Errors = new { Messages = new List<string> { "Orders Not Found" } },
                    };
                }
                else
                {
                    string Message = orders.Count == 0 ? "Orders List is Empty" : "All Orders Fetched Successfully";
                    return new ApiResponse
                    {
                        Message = Message,
                        data = orders,
                        StatusCode = "200",
                    };
                }
            }

            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }
        public async Task<ApiResponse> GetUserOrders(string UserId)
        {
            try
            {
                if (await _userManager.FindByIdAsync(UserId) is null)
                {
                    return new ApiResponse
                    {
                        Message = "User Not Found",
                        Errors = new { Messages = new List<string> { "User Not Found" } },
                    };
                }
                var orders = await _context.Orders.Join(
                    _context.Users,
                    (o) => o.AppUser.Id,
                    (u) => u.Id,
                    (o, u) => new { UserId = o.AppUser.Id, o.Data, o.Price, o.Quantity }
                    ).ToListAsync();
                if (orders == null)
                {
                    return new ApiResponse
                    {
                        Message = "Orders Not Found",
                        Errors = new { Messages = new List<string> { "Orders Not Found" } },
                    };
                }
                else
                {
                    string Message = orders.Count == 0 ? "Orders List is Empty" : "All Orders Fetched Successfully";
                    return new ApiResponse
                    {
                        Message = Message,
                        data = orders,
                        StatusCode = "200",
                    };
                }
            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }


        public async Task<ApiResponse> CancelOrder(int OrderId)
        {
            try
            {
                int maxId = _context.Orders.Count() == 0 ? 0 : _context.Orders.Max(s => s.Id);
                if (OrderId <= 0 || OrderId > maxId)
                {
                    return new ApiResponse
                    {
                        Message = "InValid OrderId",
                        Errors = new { Messages = new List<string> { "InValid OrderId" } },
                    };
                }
                var order = await _context.Orders.FindAsync(OrderId);
                if (order != null)
                {
                    _context.Orders.Remove(order);
                    _context.SaveChanges();
                    return new ApiResponse
                    {
                        Message = "Order Cancelled Successfully",
                        data = order,
                        StatusCode = "200",
                    };
                }
                else
                {
                    return new ApiResponse
                    {
                        Message = "Order Not Found",
                        Errors = new { Messages = new List<string> { "Order Not Found" } },
                    };
                }
            } catch(Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }

        public async Task<ApiResponse> AddOrderProduct(OrderProductDto orderProductDto)
        {
            try
            {
                if (! await IsOrderWithIdFound(orderProductDto.OrderId))
                {
                    return new ApiResponse
                    {
                        Message = "Order Not Found",
                        Errors = new { Messages = new List<string> { "Order Not Found" } },
                    };
                }
                if (!await IsProductWithIdFound(orderProductDto.ProductId))
                {
                    return new ApiResponse
                    {
                        Message = "Product Not Found",
                        Errors = new { Messages = new List<string> { "Product Not Found" } },
                    };
                }
                var orderProduct = new OrderProduct { OrderId = orderProductDto.OrderId, ProductId = orderProductDto.ProductId };
                await _context.AddAsync(orderProduct);
                _context.SaveChanges();

                return new ApiResponse
                {
                    Message = "OrderProduct Added Successfully",
                    data = orderProduct,
                    StatusCode = "200",
                };
            }
            catch (Exception ex)
            {



                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }

        public async Task<ApiResponse> GetBestSellingProducts()
        { 
            // Determine specific attribute
            try
            {
                var products = await _context.Products.OrderByDescending(i => i.SellingCount).Take(10).ToListAsync();

                return new ApiResponse
                {
                    Message = "Rate Average Got Successfully",
                    data = products,
                    StatusCode = "200",
                };


            } catch(Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }

        public async Task<ApiResponse> GetProductAVGRating(int ProductId)
        {
            try
            {
                var product = await _context.Products.FindAsync(ProductId);
                if (product == null)
                {
                    return new ApiResponse
                    {
                        Message = "Product Not Found",
                        Errors = new { Messages = new List<string> { "Product Not Found" } },
                    };
                }
                double avgRate = product.Rates.Average(i => i.RateValue);
                return new ApiResponse
                {
                    Message = "Rate Average Got Successfully",
                    data = new
                    {
                        RageAverage = avgRate
                    },
                    StatusCode = "200",
                };
            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }

        public async Task<ApiResponse> AddRate(RateDto RateDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(RateDto.UserId);
                if (user == null)
                {
                    return new ApiResponse
                    {
                        Message = "User Not Found",
                        Errors = new { Messages = new List<string> { "User Not Found" } },
                    };
                }
                var product = _context.Products.Find(RateDto.ProductId);
                if (product == null)
                {
                    return new ApiResponse
                    {
                        Message = "Product Not Found",
                        Errors = new { Messages = new List<string> { "Product Not Found" } },
                    };
                }

                var rate = new Rate
                {
                    RateValue = RateDto.RateValue,
                    AppUserForeignKey = RateDto.UserId,
                    ProductId = RateDto.ProductId,
                };
                await _context.Rates.AddAsync(rate);
                _context.SaveChanges();

                return new ApiResponse
                {
                    Message = "Rate Added Successfully",
                    data = new
                    {
                        rate.RateValue,
                        UserId = rate.AppUserForeignKey,
                        rate.ProductId,
                    },
                    StatusCode = "200",
                };


            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }


        }

        public async Task<ApiResponse> GetRatesCount(int ProductId)
        {
            try
            {

                var product = await _context.Products.FindAsync(ProductId);
                if (product == null)
                {
                    return new ApiResponse
                    {
                        Message = "Product Not Found",
                        Errors = new { Messages = new List<string> { "Product Not Found" } },
                    };
                }

                int count = product.Rates.Count;
                return new ApiResponse
                {
                    Message = "Rates Count Got Successfully",
                    data = count,
                    StatusCode = "200",
                };
            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }

        public async Task<ApiResponse> AddReview(ReviewDto ReviewDto)
        {
            try
            {


                var review = new Review
                {
                    Date = ReviewDto.Date,
                    ReviewComment = ReviewDto.ReviewComment,
                    AppUserForeignKey = ReviewDto.UserId,
                    ProductId = ReviewDto.ProductId,
                };


                await _context.Reviews.AddAsync(review);
                _context.SaveChanges();

                return new ApiResponse
                {
                    Message = "Review Added Successfully",
                    data = new
                    {
                        review.ReviewComment,
                        review.Date,
                        review.AppUserForeignKey,
                        review.ProductId,
                    },
                    StatusCode = "200",
                };
            }
            catch (Exception ex)
            {

                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }

        public async Task<ApiResponse> GetReviews(int ProductId)
        {
            try
            {
                List<object> data = new List<object>();
                var reviews = await _context.Reviews.Where(i => i.ProductId == ProductId).ToListAsync();
                foreach (var item in reviews)
                {
                    var user = await _userManager.FindByIdAsync(item.AppUserForeignKey);
                    var obj = new
                    {
                        item.ReviewComment,
                        item.Date,
                        item.ProductId,
                        User = new
                        {
                            user!.Id,
                            user.UserName,
                            user.Email,
                        },
                    };
                    data.Add(obj);
                }
                return new ApiResponse
                {
                    Message = data.Count == 0 ? "Reviews List is Empty" : "Reviews Got Successfully",
                    data = data,
                    StatusCode = "200",
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }

    }
}

