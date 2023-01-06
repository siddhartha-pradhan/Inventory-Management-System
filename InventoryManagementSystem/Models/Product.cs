namespace InventoryManagementSystem.Models;

public class Product
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public string Title { get; set; }

	public int Quantity { get; set; }

	public Guid CreatedBy { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.Now;

	public DateTime? LastModifiedAt { get; set; }
}
