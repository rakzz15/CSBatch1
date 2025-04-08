using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicGadgets;
using static ElectronicGadgets.OrderCollectionManager;


namespace ElectronicGadgets
{
    public class ProductCollectionManager
    {
        private List<Product> products = new List<Product>();

        // 1. Add Product
    public void AddProduct(Product newProduct)
{
    if (newProduct == null)
        throw new ArgumentNullException(nameof(newProduct), "Product cannot be null.");

        bool duplicate = products.Any(p =>
            p.ProductID == newProduct.ProductID ||
            string.Equals(p.ProductName, newProduct.ProductName, StringComparison.OrdinalIgnoreCase)
        );

    if (duplicate)
        throw new DuplicateProductException($"A product with the same name or ID already exists: {newProduct.ProductName}");

        products.Add(newProduct);
    Console.WriteLine("Product added successfully.");
}


        // 2. Update Product
        public void UpdateProduct(int productId, string newName, string newCategory, string newDescription, int newPrice)
        {
            var product = products.FirstOrDefault(p => p.ProductID == productId);
            if (product == null)
            {
                Console.WriteLine($"Product with ID {productId} not found.");
                return; // Exit the method without throwing an exception
            }
            // throw new Exception("Product not found.");

            product.ProductName = newName;
            product.Category = newCategory;
            product.Description = newDescription;
            product.Price = newPrice;

            Console.WriteLine(" Product updated successfully.\n");
        }

        // 3. Remove Product
        public void RemoveProduct(int productId)
        {
            var product = products.FirstOrDefault(p => p.ProductID == productId);
            if (product == null)
                throw new ProductNotFoundException("Product not found to remove");

            products.Remove(product);
            Console.WriteLine("Product removed successfully.\n");
        }

        // 4. Search Product by Name, Category, or Description
        public void SearchProduct(string keyword)
        {
            var results = products.Where(p =>
            p.ProductName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
            p.Category.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
            p.Description.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0).ToList();


            if (results.Count == 0)
            {
                Console.WriteLine(" No matching products found.\n");
            }
            else
            {
                Console.WriteLine(" Matching Products:");
                foreach (var p in results)
                {
                    DisplayProduct(p);
                }
            }
        }

        // 5. Display All Products
        public void DisplayAllProducts()
        {
            if (products.Count == 0)
            {
                Console.WriteLine(" No products available.\n");
                return;
            }

            Console.WriteLine(" Product List:");
            foreach (var p in products)
            {
                DisplayProduct(p);
            }
        }

        // 6. Sort Products by Price (Ascending/Descending)
        public void SortProductsByPrice(bool ascending = true)
        {
            var sorted = ascending
                ? products.OrderBy(p => p.Price).ToList()
                : products.OrderByDescending(p => p.Price).ToList();

            Console.WriteLine($" Products sorted by price ({(ascending ? "Low to High" : "High to Low")}):");
            foreach (var p in sorted)
            {
                DisplayProduct(p);
            }
        }

        // Helper Method to Display a Product
        private void DisplayProduct(Product p)
        {
            Console.WriteLine($"ID: {p.ProductID}, Name: {p.ProductName}, Category: {p.Category}, Description: {p.Description}, Price: rs.{p.Price:F2}");
        }
    }
    public class OrderCollectionManager
    {
        private List<Order> orders;

        public OrderCollectionManager()
        {
            orders = new List<Order>();
        }

        // Add a new order
        public void AddOrder(Order order)
        {
            if (orders.Exists(o => o.OrderID == order.OrderID))
            {
                throw new InvalidOperationException("Order with the same ID already exists.");
            }
            orders.Add(order);
            Console.WriteLine("Order added successfully.");
        }

        // Update order status
        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            Order existingOrder = orders.Find(o => o.OrderID == orderId);
            if (existingOrder != null)
            {
                existingOrder.Status = newStatus;
                Console.WriteLine($"Order {orderId} status updated to {newStatus}.");
            }
            else
            {
                Console.WriteLine("Order not found.");
            }
        }

        // Remove canceled order
        public void RemoveOrder(int orderId)
        {
            Order orderToRemove = orders.Find(o => o.OrderID == orderId);
            if (orderToRemove != null)
            {
                if (orderToRemove.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
                {
                    orders.Remove(orderToRemove);
                    Console.WriteLine("Cancelled order removed.");
                }
                else
                {
                    Console.WriteLine("Only cancelled orders can be removed.");
                }
            }
            else
            {
                Console.WriteLine("Order not found.");
            }
        }

        // Optional: Display orders (for testing)
        public void DisplayAllOrders()
        {
            foreach (var order in orders)
            {
                Console.WriteLine($"Order ID: {order.OrderID}, Customer ID: {order.Customer.CustomerID}, Date: {order.OrderDate}, Status: {order.Status}, Total: rs.{order.TotalAmount}");
            }
        }

        // Sort orders by date (ascending)
        public void SortOrdersByDateAscending()
        {
            var sortedOrders = orders.OrderBy(o => o.OrderDate).ToList();
            Console.WriteLine("\n--- Orders Sorted by Date (Ascending) ---");
            foreach (var order in sortedOrders)
            {
                Console.WriteLine($"Order ID: {order.OrderID}, Date: {order.OrderDate}, Status: {order.Status}, Total: rs.{order.TotalAmount}");
            }
        }

        // Sort orders by date (descending)
        public void SortOrdersByDateDescending()
        {
            var sortedOrders = orders.OrderByDescending(o => o.OrderDate).ToList();
            Console.WriteLine("\n--- Orders Sorted by Date (Descending) ---");
            foreach (var order in sortedOrders)
            {
                Console.WriteLine($"Order ID: {order.OrderID}, Date: {order.OrderDate}, Status: {order.Status}, Total: rs.{order.TotalAmount}");
            }
        }

        // Display orders within a date range
        public void DisplayOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            var filteredOrders = orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .OrderBy(o => o.OrderDate)
                .ToList();

            Console.WriteLine($"\n--- Orders between {startDate.ToShortDateString()} and {endDate.ToShortDateString()} ---");
            foreach (var order in filteredOrders)
            {
                Console.WriteLine($"Order ID: {order.OrderID}, Date: {order.OrderDate}, Status: {order.Status}, Total: rs.{order.TotalAmount}");
            }

            if (!filteredOrders.Any())
            {
                Console.WriteLine("No orders found in the given date range.");
            }
        }
    }

        public class InventoryCollectionManager
        {
            private SortedList<int, Inventory> inventoryList;

            public InventoryCollectionManager()
            {
                inventoryList = new SortedList<int, Inventory>();
            }

        // Add or update inventory
        public void AddOrUpdateInventory(int productId, int quantity)
        {
            if (inventoryList.ContainsKey(productId))
            {
                inventoryList[productId].QuantityInStock += quantity;
                Console.WriteLine($"Updated inventory for Product ID {productId}. New quantity: {inventoryList[productId].QuantityInStock}");
            }
            else
            {
                inventoryList.Add(productId, new Inventory(productId, quantity));
                Console.WriteLine($"Added new inventory for Product ID {productId} with quantity: {quantity}");
            }
        }

        // Remove inventory item
        public void RemoveInventory(int productId)
            {
                if (inventoryList.ContainsKey(productId))
                {
                    inventoryList.Remove(productId);
                    Console.WriteLine($"Inventory for Product ID {productId} removed.");
                }
                else
                {
                    Console.WriteLine($"Product ID {productId} not found in inventory.");
                }
            }

            // Get inventory details
            public void DisplayInventory()
            {
                Console.WriteLine("\n--- Inventory List ---");
                foreach (var item in inventoryList)
                {
                    Console.WriteLine($"Product ID: {item.Key}, Quantity In Stock: {item.Value.QuantityInStock}");
                }

                if (inventoryList.Count == 0)
                {
                    Console.WriteLine("Inventory is empty.");
                }
            }

            // Check stock before processing order
            public bool IsStockAvailable(int productId, int requestedQty)
            {
                return inventoryList.ContainsKey(productId) && inventoryList[productId].QuantityInStock >= requestedQty;
            }

            // Decrease stock after order placement
            public void DecreaseStock(int productId, int qty)
            {
                if (IsStockAvailable(productId, qty))
                {
                    inventoryList[productId].QuantityInStock -= qty;
                    Console.WriteLine($"Stock decreased for Product ID {productId}. Remaining: {inventoryList[productId].QuantityInStock}");
                }
                else
                {
                    Console.WriteLine($"Not enough stock for Product ID {productId}.");
                }
            }
        }

    }
    public class OrderDetailManager
    {
        public void AddOrderDetailWithInventoryCheck(OrderDetail newDetail, List<Inventory> inventoryList)
        {
            if (newDetail == null)
                throw new ArgumentNullException(nameof(newDetail), "OrderDetail cannot be null.");

            var inventoryItem = inventoryList.FirstOrDefault(i => i.Product.ProductID == newDetail.Product.ProductID);

            if (inventoryItem == null)
                throw new ProductUnavailableException("Product not found in inventory.");

            if (inventoryItem.QuantityInStock < newDetail.Quantity)
                throw new ProductUnavailableException($"Not enough stock for {newDetail.Product.ProductName}. Available: {inventoryItem.QuantityInStock}");

            // Update inventory
            inventoryItem.QuantityInStock -= newDetail.Quantity;

            // Add the OrderDetail to the order
            newDetail.Order.OrderDetails.Add(newDetail);

            Console.WriteLine("Order detail added successfully and inventory updated.");
        }

    }

    class Program3
    {
        static void Main(string[] args)
        {
            // Initialize managers for products, orders, and inventory
            var productCollectionManager = new ProductCollectionManager();
            var orderCollectionManager = new OrderCollectionManager();
            var inventoryCollectionManager = new InventoryCollectionManager();
            var orderDetailManager = new OrderDetailManager();

            // Test data for the demonstration
            var product1 = new Product { ProductID = 1, ProductName = "Smartphone", Category = "Electronics", Description = "Latest model smartphone", Price = 15000 };
            var product2 = new Product { ProductID = 2, ProductName = "Laptop", Category = "Electronics", Description = "High-end laptop", Price = 50000 };
            var customer1 = new Customer { CustomerID = 1, FirstName = "John",LastName=" Doe", Email = "john@example.com" };
            var inventoryItem1 = new Inventory { Product = product1, QuantityInStock = 20 };
            var inventoryItem2 = new Inventory { Product = product2, QuantityInStock = 10 };
            inventoryCollectionManager.AddOrUpdateInventory(1, 20);
            inventoryCollectionManager.AddOrUpdateInventory(2, 10);

            // Display main menu
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n--- Main Menu ---");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. Update Product");
                Console.WriteLine("3. Remove Product");
                Console.WriteLine("4. Display All Products");
                Console.WriteLine("5. Search Product");
                Console.WriteLine("6. Sort Products by Price");
                Console.WriteLine("7. Add Order");
                Console.WriteLine("8. Display Orders");
                Console.WriteLine("9. Add Inventory");
                Console.WriteLine("10. Display Inventory");
                Console.WriteLine("11. Exit");
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // Add Product
                        var newProduct = new Product { ProductID = 14, ProductName = "SmartProjector", Category = "Electronics", Description = "New projectorl", Price = 20000 };
                        productCollectionManager.AddProduct(newProduct);
                    var newProduct2 = new Product { ProductID = 15, ProductName = "SmartScreen", Category = "Electronics", Description = "New screen", Price = 2000 };
                    productCollectionManager.AddProduct(newProduct2);
                    break;

                    case "2":
                        // Update Product
                        productCollectionManager.UpdateProduct(1, "Updated Smartphone", "Electronics", "Updated description", 16000);
                        break;

                    case "3":
                        // Remove Product
                        productCollectionManager.RemoveProduct(2);
                        break;

                    case "4":
                        // Display All Products
                        productCollectionManager.DisplayAllProducts();
                        break;

                    case "5":
                        // Search Product
                        productCollectionManager.SearchProduct("Laptop");
                        break;

                    case "6":
                        // Sort Products by Price
                        productCollectionManager.SortProductsByPrice(true); // Ascending
                        break;

                    case "7":
                        // Add Order
                        var newOrder = new Order { OrderID = 15, Customer = customer1, OrderDate = DateTime.Now, Status = "Pending", TotalAmount = 15000 };
                        orderCollectionManager.AddOrder(newOrder);
                        break;

                    case "8":
                        // Display Orders
                        orderCollectionManager.DisplayAllOrders();
                        break;

                    case "9":
                        // Add Inventory
                        inventoryCollectionManager.AddOrUpdateInventory(1, 5);
                        break;

                    case "10":
                        // Display Inventory
                        inventoryCollectionManager.DisplayInventory();
                        break;

                    case "11":
                        // Exit the program
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }

            Console.WriteLine("Exiting the program...");
        }
    }



