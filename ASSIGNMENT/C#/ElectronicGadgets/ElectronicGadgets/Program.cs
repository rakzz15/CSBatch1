using System;

namespace ElectronicGadgets
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n--- MAIN MENU ---");
                Console.WriteLine("1. Customer Operations");
                Console.WriteLine("2. Product Operations");
                Console.WriteLine("3. Order Operations");
                Console.WriteLine("4. OrderDetail Operations");
                Console.WriteLine("5. Inventory Operations");
                Console.WriteLine("6. Exit");

                Console.Write("Select an option: ");
                int mainChoice = int.Parse(Console.ReadLine());

                switch (mainChoice)
                {
                    case 1:
                        CustomerOperations();
                        break;
                    case 2:
                        ProductOperations();
                        break;
                    case 3:
                        OrderOperations();
                        break;
                    case 4:
                        OrderDetailOperations();
                        break;
                    case 5:
                        InventoryOperations();
                        break;
                    case 6:
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void CustomerOperations()
        {
            Customer customer = new Customer();

            Console.Write("Enter Customer ID: ");
            customer.CustomerID = int.Parse(Console.ReadLine());

            while (true)
            {
                Console.WriteLine("\n--- Customer Menu ---");
                Console.WriteLine("1. Get Customer Details");
                Console.WriteLine("2. Calculate Total Orders");
                Console.WriteLine("3. Update Customer Info");
                Console.WriteLine("4. Back to Main Menu");

                Console.Write("Select an option: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        customer.GetCustomerDetails();
                        break;
                    case 2:
                        customer.CalculateTotalOrders();
                        break;
                    case 3:
                        Console.Write("Enter new email: ");
                        string email = Console.ReadLine();
                        Console.Write("Enter new phone: ");
                        string phone = Console.ReadLine();
                        Console.Write("Enter new address: ");
                        string address = Console.ReadLine();
                        customer.UpdateCustomerInfo(email, phone, address);
                        break;
                    case 4:
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void ProductOperations()
        {
            Product product = new Product();

            Console.Write("Enter Product ID: ");
            product.ProductID = int.Parse(Console.ReadLine());

            while (true)
            {
                Console.WriteLine("\n--- Product Menu ---");
                Console.WriteLine("1. Get Product Details");
                Console.WriteLine("2. Check Stock");
                Console.WriteLine("3. Update Product Info");
                Console.WriteLine("4. Back to Main Menu");

                Console.Write("Select an option: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        product.GetProductDetails();
                        break;
                    case 2:
                        product.IsProductInStock();
                        break;
                    case 3:
                        Console.Write("Enter new price: ");
                        decimal price = decimal.Parse(Console.ReadLine());
                        Console.Write("Enter new description: ");
                        string desc = Console.ReadLine();
                        product.UpdateProductInfo(price, desc);
                        break;
                    case 4:
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void OrderOperations()
        {
            Order order = new Order();

            Console.Write("Enter Order ID: ");
            order.OrderID = int.Parse(Console.ReadLine());

            while (true)
            {
                Console.WriteLine("\n--- Order Menu ---");
                Console.WriteLine("1. Get Order Details");
                Console.WriteLine("2. Calculate Total Amount");
                Console.WriteLine("3. Update Order Status");
                Console.WriteLine("4. Cancel Order");
                Console.WriteLine("5. Back to Main Menu");

                Console.Write("Select an option: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        order.GetOrderDetails();
                        break;
                    case 2:
                        order.CalculateTotalAmount(order.OrderID);
                        Console.WriteLine("Total Amount: Rs." + order.TotalAmount);
                        break;
                    case 3:
                        Console.Write("Enter new status (Processing/Shipped/Delivered): ");
                        string status = Console.ReadLine();
                        order.UpdateOrderStatus(status);
                        order.CheckOrderStatus(order.OrderID);
                        break;
                    case 4:
                        Console.Write("Enter Order ID to cancel: ");
                        int orderId = Convert.ToInt32(Console.ReadLine());
                        order.CancelOrder(orderId);
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void OrderDetailOperations()
        {
            OrderDetail detail = new OrderDetail();
            detail.Product = new Product();

            Console.Write("Enter OrderDetail ID: ");
            detail.OrderDetailID = int.Parse(Console.ReadLine());

            Console.Write("Enter Product ID: ");
            detail.Product.ProductID = int.Parse(Console.ReadLine());

            while (true)
            {
                Console.WriteLine("\n--- OrderDetail Menu ---");
                Console.WriteLine("1. Get Order Detail Info");
                Console.WriteLine("2. Update Quantity");
                Console.WriteLine("3. Calculate Subtotal");
                Console.WriteLine("4. Apply Discount");
                Console.WriteLine("5. Back to Main Menu");

                Console.Write("Select an option: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        detail.GetOrderDetailInfo();
                        break;
                    case 2:
                        Console.Write("Enter new quantity: ");
                        detail.Quantity = int.Parse(Console.ReadLine());
                        detail.UpdateQuantity(detail.Quantity);
                        break;
                    case 3:
                        Console.Write("Enter Order ID: ");
                        int subtotalOrderId = int.Parse(Console.ReadLine());
                        detail.CalculateSubtotal(subtotalOrderId);
                        break;

                    case 4:
                        Console.Write("Enter discount %: ");
                        decimal discount = decimal.Parse(Console.ReadLine());
                        detail.AddDiscount(discount);
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void InventoryOperations()
        {
            Inventory inventory = new Inventory();
            inventory.Product = new Product();

            Console.Write("Enter Inventory ID: ");
            inventory.InventoryID = int.Parse(Console.ReadLine());

            Console.Write("Enter Product ID: ");
            inventory.Product.ProductID = int.Parse(Console.ReadLine());

            while (true)
            {
                Console.WriteLine("\n--- Inventory Menu ---");
                Console.WriteLine("1. Get Product Info");
                Console.WriteLine("2. Get Quantity in Stock");
                Console.WriteLine("3. Add to Inventory");
                Console.WriteLine("4. Remove from Inventory");
                Console.WriteLine("5. Update Stock Quantity");
                Console.WriteLine("6. Check Product Availability");
                Console.WriteLine("7. Get Inventory Value");
                Console.WriteLine("8. List Low Stock Products");
                Console.WriteLine("9. List Out-of-Stock Products");
                Console.WriteLine("10. List All Products");
                Console.WriteLine("11. Back to Main Menu");

                Console.Write("Select an option: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        inventory.Product.GetProductDetails();
                        break;
                    case 2:
                        Console.WriteLine("Stock: " + inventory.GetQuantityInStock());
                        break;
                    case 3:
                        Console.Write("Quantity to add: ");
                        inventory.AddToInventory(int.Parse(Console.ReadLine()));
                        break;
                    case 4:
                        try
                        {
                            Console.Write("Quantity to remove: ");
                            int quantity = int.Parse(Console.ReadLine());
                            inventory.RemoveFromInventory(quantity);
                        }
                        catch (InsufficientStockException ex)
                        {
                            Console.WriteLine("Inventory Error: " + ex.Message);
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Invalid input. Please enter a numeric quantity.");
                        }
                        break;
                    case 5:
                        Console.Write("New stock quantity: ");
                        inventory.UpdateStockQuantity(int.Parse(Console.ReadLine()));
                        break;
                    case 6:
                        Console.Write("Check availability for quantity: ");
                        bool available = inventory.IsProductAvailable(int.Parse(Console.ReadLine()));
                        Console.WriteLine("Available: " + (available ? "Yes" : "No"));
                        break;
                    case 7:
                        inventory.GetInventoryValue();
                        break;
                    case 8:
                        Console.Write("Enter threshold: ");
                        inventory.ListLowStockProducts(int.Parse(Console.ReadLine()));
                        break;
                    case 9:
                        inventory.ListOutOfStockProducts();
                        break;
                    case 10:
                        inventory.ListAllProducts();
                        break;
                    case 11:
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}



