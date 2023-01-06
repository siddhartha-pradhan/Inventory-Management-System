namespace InventoryManagementSystem.Models;

public class Order
{
	public Guid Id { get; set; } = Guid.NewGuid();

	public Guid ProductId { get; set; }

	public int Quantity { get; set; }

	public DateTime OrderedAt { get; set; } = DateTime.Now;
	
	public Guid OrderedBy { get; set; }

	public Status OrderStatus { get; set; } = Status.Pending;

	public bool IsApproved { get; set; } = false;

	public Guid ActivityBy { get; set; }

	public DateTime ActivityAt { get; set; }
}
