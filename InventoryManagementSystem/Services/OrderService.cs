using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Services;

public class OrderService : GenericService<Order>
{
	/// <summary>
	/// Defining a static path for all the required files for the service and domain logic
	/// </summary>
	public static string appDataDirectoryPath = UtilityService.GetAppDirectoryPath();
	public static string appOrdersFilePath = UtilityService.GetAppOrdersFilePath();
	public static string appProductsFilePath = UtilityService.GetAppProductsFilePath();

	/// <summary>
	/// Defines a method when an order is placed by a staff
	/// </summary>
	/// <param name="userId">Staff who placed the order</param>
	/// <param name="productId">The product ordered by the staff</param>
	/// <param name="quantity">The quantity of the product being ordered</param>
	/// <returns></returns>
	/// <exception cref="Exception">Thrown when there's an error while placing an order</exception>
	public static List<Order> Create(Guid userId, Guid productId, int quantity)
	{
		var orders = GetAll(appOrdersFilePath);

		var products = ProductService.GetAll(appProductsFilePath);

		var product = products.FirstOrDefault(x => x.Id == productId);

		if (quantity > product.Quantity)
		{
			throw new Exception("Can not order more items than the product's actual quantity.");
		}

		if (quantity < 0 || quantity == 0)
		{
			throw new Exception("Add a positive integer value for the product to be ordered.");
		}

		var order = new Order()
		{
			ProductId = productId,
			OrderedBy = userId,
			Quantity = quantity
		};

		orders.Add(order);

		SaveAll(orders, appDataDirectoryPath, appOrdersFilePath);

		return orders;
	}

	/// <summary>
	/// Defines a method to update any existing pending order
	/// </summary>
	/// <param name="orderId">The ID of the order that is to be updated</param>
	/// <param name="userId">The admin responsible for handling the order</param>
	/// <param name="status">The status or activity of the order after update</param>
	/// <returns></returns>
	/// <exception cref="Exception">When an order mishandles any operation</exception>
	public static List<Order> Update(Guid orderId, Guid userId, Status status)
	{
		var orders = GetAll(appOrdersFilePath);

		var order = orders.FirstOrDefault(x => x.Id == orderId);

		var products = ProductService.GetAll(appProductsFilePath);

		var product = products.FirstOrDefault(x => x.Id == order.ProductId);

		if (order == null)
		{
			throw new Exception("Order not found.");
		}

		if (status == Status.Approved)
		{
			var day = (int)DateTime.Now.DayOfWeek + 1;

			if (day < 2 || day > 6)
			{
				throw new Exception("Can approve only between Monday and Friday.");
			}

			var time = (int)DateTime.Now.Hour;

			if (time < 9 || time > 16)
			{
				throw new Exception("Can approve only between 9AM and 6PM.");
			}

			if(order.Quantity > product.Quantity)
			{
                throw new Exception("Can't approve products more than the existing product count.");
            }

            order.IsApproved = true;
			order.OrderStatus = status;
			product.Quantity -= order.Quantity;
		}

		else if (status == Status.Rejected)
		{
			order.IsApproved = false;
            order.OrderStatus = status;
        }

        order.ActivityBy = userId;
		order.ActivityAt = DateTime.Now;

		SaveAll(orders, appDataDirectoryPath, appOrdersFilePath);

		ProductService.SaveAll(products, appDataDirectoryPath, appProductsFilePath);

		return orders;
	}

	/// <summary>
	/// Defines a method to retrieve the list of approved products and their total sales
	/// </summary>
	/// <returns>A list of approved orders and their sales</returns>
	public static List<OrderQuantity> GetOrderedQuantity()
	{
		var orders = GetAll(appOrdersFilePath).Where(x => x.IsApproved);

		var result = (from order in orders
					  group order by order.ProductId
					  into item
					  select new OrderQuantity
					  {
						  ProductId = item.Key,
						  Quantity = item.Sum(x => x.Quantity),
						  LastOrderedAt = item.Max(x => x.OrderedAt)
					  }).ToList();

		return result;
	}
}
