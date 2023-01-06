namespace InventoryManagementSystem.Models; 

public class OrderQuantity 
{
	public Guid ProductId { get; set; }

	public int Quantity { get; set; }

	public DateTime LastOrderedAt { get; set; }
}