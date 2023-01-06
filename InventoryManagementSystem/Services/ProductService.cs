using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Services; 

public class ProductService : GenericService<Product>
{
	/// <summary>
	/// Defining a static path for all the required files for the service and domain logic
	/// </summary>
	public static string appDataDirectoryPath = UtilityService.GetAppDirectoryPath();
	public static string appProductsFilePath = UtilityService.GetAppProductsFilePath();

	/// <summary>
	/// Defining a method to retrieve a product when an ID is passed to it
	/// </summary>
	/// <param name="Id">Valid Product Id</param>
	/// <returns>Instance of product</returns>
	/// <exception cref="Exception">Error for a non-matching product</exception>
	public static Product GetById(Guid Id)
	{
		var product = GetAll(appProductsFilePath).FirstOrDefault(x => x.Id == Id);

		if(product == null)
		{
			throw new Exception("Product not found.");
		}

		return product;
	}

	/// <summary>
	/// Defining a method to create a new instance of product
	/// </summary>
	/// <param name="userId">Admin responsible for creation of a product</param>
	/// <param name="title">The title of the product</param>
	/// <param name="quantity">The quantity of the added product</param>
	/// <returns>A new updated list after addition of product</returns>
	/// <exception cref="Exception">Errors when invalid values are places</exception>
	public static List<Product> Create(Guid userId, string title, int quantity)
	{
		if(title == "")
		{
			throw new Exception("Please insert coorect and valid input for each of the fields.");
		}

		if (quantity <= 0)
		{
			throw new Exception("Add a positive integer value to add a product item.");
		}

		title = title.Trim().ToLower();

		var products = GetAll(appProductsFilePath);

		var productExists = products.Any(x => x.Title.ToLower() == title);

		if (productExists)
		{
			throw new Exception("Product with the same already exists, Please add any other title.");
		}

		var product = new Product()
		{
			Title = title,
			Quantity = quantity,
			CreatedBy = userId
		};

		products.Add(product);

		SaveAll(products, appDataDirectoryPath, appProductsFilePath);

		return products;
	}

	/// <summary>
	/// Defining a method to update an existing product
	/// </summary>
	/// <param name="productId">Valid existing product's identifier</param>
	/// <param name="title">The title of the product to be updated</param>
	/// <param name="quantity">The quantity with which it is updated</param>
	/// <returns>A list of updating products</returns>
	/// <exception cref="Exception">Errors thrown when it runs to some dysfunctioning</exception>
	public static List<Product> Update(Guid productId, string title, int quantity)
	{
		if (quantity < 0)
		{
			throw new Exception("Add a positive integer value to update the product item.");
		}

		var products = GetAll(appProductsFilePath);

		var product = products.FirstOrDefault(x => x.Id == productId);

		if (product == null)
		{
			throw new Exception("Product not found.");
		}

		product.Title = title;
		product.Quantity = quantity;
		product.LastModifiedAt = DateTime.Now;

		SaveAll(products, appDataDirectoryPath, appProductsFilePath);

		return products;
	}

	/// <summary>
	/// Defining a method to delete any existing product from the list
	/// </summary>
	/// <param name="id">ID of an existing product</param>
	/// <returns>An updated list without the deleted product</returns>
	/// <exception cref="Exception">Exception thrown for a non-existent product deletion</exception>
	public static List<Product> Delete(Guid id)
	{
		var products = GetAll(appProductsFilePath);

		var product = products.FirstOrDefault(x => x.Id == id);

		if (product == null)
		{
			throw new Exception("Product not found.");
		}

		products.Remove(product);

		SaveAll(products, appDataDirectoryPath, appProductsFilePath);

		return products;
	}

	/// <summary>
	/// Defining a sorting algorithm to sort a unsorted list of products 
	/// </summary>
	/// <param name="unsorted">Accepts an unsorted list of products</param>
	/// <returns>Returns a sorted list of products as per the item count or quantity</returns>
	public static List<Product> MergeSort(List<Product> unsorted)
	{
		if (unsorted.Count <= 1)
			return unsorted;

		var left = new List<Product>();
		var right = new List<Product>();

		int middle = unsorted.Count / 2;

		for (int i = 0; i < middle; i++)  
		{
			left.Add(unsorted[i]);
		}
		
		for (int i = middle; i < unsorted.Count; i++)
		{
			right.Add(unsorted[i]);
		}

		left = MergeSort(left);
		right = MergeSort(right);
		return Merge(left, right);
	}

	/// <summary>
	/// The divide algorithm to divide a single list into two different states of left and right side
	/// </summary>
	/// <param name="left">The left hand side of the unsorted list</param>
	/// <param name="right">The right hand side of the unsorted list</param>
	/// <returns>Sorted list as per the side of the list</returns>
	public static List<Product> Merge(List<Product> left, List<Product> right)
	{
		var result = new List<Product>();

		while (left.Count > 0 || right.Count > 0)
		{
			if (left.Count > 0 && right.Count > 0)
			{
				if (left.First().Quantity <= right.First().Quantity)
				{
					result.Add(left.First());
					left.Remove(left.First());
				}
				else
				{
					result.Add(right.First());
					right.Remove(right.First());
				}
			}
			else if (left.Count > 0)
			{
				result.Add(left.First());
				left.Remove(left.First());
			}
			else if (right.Count > 0)
			{
				result.Add(right.First());

				right.Remove(right.First());
			}
		}
		return result;
	}
}