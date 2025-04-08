using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ElectronicGadgets.OrderManager;

namespace ElectronicGadgets
{
    public class DatabaseConnector
    {
        public readonly string connectionString;
        public SqlConnection Connection { get; private set; }

        public DatabaseConnector()
        {
            connectionString = @"Server=RAKS\SQL2022;Database=TechShop;Trusted_Connection=True;";
            Connection = new SqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            if (Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();
        }

        public void CloseConnection()
        {
            if (Connection.State == System.Data.ConnectionState.Open)
                Connection.Close();
        }

    }

    public class CustomerManager
    {
        public DatabaseConnector Db { get; set; }

        public CustomerManager()
        {
            Db = new DatabaseConnector();
        }

        public void AddCustomer(Customer customer)
        {
            try
            {
                Db.OpenConnection();

                string query = @"INSERT INTO Customers 
                             (CustomerID, FirstName, LastName, Email, Phone, Address) 
                             VALUES (@CustomerID, @FirstName, @LastName, @Email, @Phone, @Address)";

                using (SqlCommand cmd = new SqlCommand(query, Db.Connection))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                    cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                    cmd.Parameters.AddWithValue("@Address", customer.Address);

                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine(rows > 0 ? "Customer added successfully." : "Failed to add customer.");
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Duplicate key (e.g., Email or ID already exists)
                    Console.WriteLine("Error: Duplicate entry (ID or Email already exists).");
                else
                    Console.WriteLine("Database Error: " + ex.Message);
            }
            finally
            {
                Db.CloseConnection();
            }
        }
    }

    public class ProductManager
    {
        private readonly DatabaseConnector db;

        public ProductManager()
        {
            db = new DatabaseConnector();
        }

        public void UpdateProduct(int productId, decimal newPrice, string newDescription)
        {
            try
            {
                db.OpenConnection();

                // Check if product exists
                string checkQuery = "SELECT COUNT(*) FROM Products WHERE ProductID = @ProductID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, db.Connection))
                {
                    checkCmd.Parameters.AddWithValue("@ProductID", productId);
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count == 0)
                    {
                        Console.WriteLine("Product not found. Update aborted.");
                        return;
                    }
                }

                // Update query
                string query = @"UPDATE Products 
                             SET Price = @Price, Description = @Description 
                             WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, db.Connection))
                {
                    cmd.Parameters.AddWithValue("@Price", newPrice);
                    cmd.Parameters.AddWithValue("@Description", newDescription);
                    cmd.Parameters.AddWithValue("@ProductID", productId);

                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine(rows > 0 ? "Product updated successfully." : "Product update failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database Error: " + ex.Message);
            }
            finally
            {
                db.CloseConnection();
            }
        }
    }
    public class OrderManager
    {
        private readonly DatabaseConnector db;

        public OrderManager()
        {
            db = new DatabaseConnector();
        }

        public void PlaceOrder(Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                db.OpenConnection();

                // Insert Order
                string orderQuery = @"INSERT INTO Orders (OrderID, CustomerID, OrderDate, Status, TotalAmount)
                                  VALUES (@OrderID, @CustomerID, @OrderDate, @Status, @TotalAmount)";
                using (SqlCommand cmd = new SqlCommand(orderQuery, db.Connection))
                {
                    cmd.Parameters.AddWithValue("@OrderID", order.OrderID);
                    cmd.Parameters.AddWithValue("@CustomerID", order.Customer.CustomerID);
                    cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    cmd.Parameters.AddWithValue("@Status", order.Status);
                    cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                    cmd.ExecuteNonQuery();
                }

                // Insert Order Details
                foreach (var detail in orderDetails)
                {
                    string detailQuery = @"INSERT INTO OrderDetails (OrderID, ProductID, Quantity, OrderDetailID)
                                       VALUES (@OrderID, @ProductID, @Quantity, @OrderDetailID)";
                    using (SqlCommand cmd = new SqlCommand(detailQuery, db.Connection))
                    {
                        cmd.Parameters.AddWithValue("@OrderID", order.OrderID);
                        cmd.Parameters.AddWithValue("@ProductID", detail.Product.ProductID);
                        cmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                        cmd.Parameters.AddWithValue("@OrderDetailID", detail.OrderDetailID);
                        cmd.ExecuteNonQuery();
                    }

                    // Update Inventory
                    string inventoryUpdate = @"UPDATE Inventory SET QuantityInStock = QuantityInStock - @Quantity 
                                           WHERE ProductID = @ProductID";
                    using (SqlCommand cmd = new SqlCommand(inventoryUpdate, db.Connection))
                    {
                        cmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                        cmd.Parameters.AddWithValue("@ProductID", detail.Product.ProductID);
                        cmd.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Order placed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error placing order: " + ex.Message);
            }
            finally
            {
                db.CloseConnection();
            }
        }
        public void PlaceOrder()
        {
            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            string insertOrderQuery = "INSERT INTO Orders (CustomerID, OrderDate, TotalAmount) VALUES (@CustomerID, @OrderDate, 0); SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = new SqlConnection(db.connectionString))
            {
                conn.Open();

                int orderId;
                // Step 1: Insert into Orders table and get auto-generated OrderID
                using (SqlCommand cmd = new SqlCommand(insertOrderQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                    object result = cmd.ExecuteScalar();
                    orderId = Convert.ToInt32(result);
                }

                decimal totalAmount = 0;

                // Step 2: Loop to add products
                while (true)
                {
                    Console.Write("Enter Product ID (or 0 to finish): ");
                    int productId = int.Parse(Console.ReadLine());
                    if (productId == 0) break;

                    Console.Write("Enter Quantity: ");
                    int quantity = int.Parse(Console.ReadLine());

                    // Step 3: Get price and stock
                    string getDetailsQuery = @"
                SELECT P.Price, I.QuantityInStock
                FROM Products P
                INNER JOIN Inventory I ON P.ProductID = I.ProductID
                WHERE P.ProductID = @ProductID";

                    using (SqlCommand getCmd = new SqlCommand(getDetailsQuery, conn))
                    {
                        getCmd.Parameters.AddWithValue("@ProductID", productId);
                        using (SqlDataReader reader = getCmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                Console.WriteLine("Product not found in inventory.");
                                continue;
                            }

                            decimal price = reader.GetDecimal(0);
                            int stock = reader.GetInt32(1);
                            reader.Close();

                            if (quantity > stock)
                            {
                                Console.WriteLine("Insufficient stock.");
                                continue;
                            }

                            decimal subtotal = price * quantity;
                            totalAmount += subtotal;

                            // Step 4: Insert into OrderDetails (OrderDetailID auto-generated)
                            string insertDetailQuery = @"
                        INSERT INTO OrderDetails (OrderID, ProductID, Quantity)
                        VALUES (@OrderID, @ProductID, @Quantity)";

                            using (SqlCommand detailCmd = new SqlCommand(insertDetailQuery, conn))
                            {
                                detailCmd.Parameters.AddWithValue("@OrderID", orderId);
                                detailCmd.Parameters.AddWithValue("@ProductID", productId);
                                detailCmd.Parameters.AddWithValue("@Quantity", quantity);
                                detailCmd.ExecuteNonQuery();
                            }

                            // Step 5: Update Inventory
                            string updateInventoryQuery = @"
                        UPDATE Inventory
                        SET QuantityInStock = QuantityInStock - @Quantity
                        WHERE ProductID = @ProductID";

                            using (SqlCommand inventoryCmd = new SqlCommand(updateInventoryQuery, conn))
                            {
                                inventoryCmd.Parameters.AddWithValue("@Quantity", quantity);
                                inventoryCmd.Parameters.AddWithValue("@ProductID", productId);
                                inventoryCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                // Step 6: Update TotalAmount in Orders table
                string updateOrderQuery = "UPDATE Orders SET TotalAmount = @TotalAmount WHERE OrderID = @OrderID";
                using (SqlCommand updateCmd = new SqlCommand(updateOrderQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    updateCmd.Parameters.AddWithValue("@OrderID", orderId);
                    updateCmd.ExecuteNonQuery();
                }

                Console.WriteLine($"Order placed successfully! Order ID: {orderId}, Total: Rs.{totalAmount}");
            }
        } }

    public class OrderStatusManager
    {
        private readonly DatabaseConnector db;

        public OrderStatusManager()
        {
            db = new DatabaseConnector();
        }

        public void GetOrderStatus(int orderId)
        {
            try
            {
                db.OpenConnection();

                string query = "SELECT Status FROM Orders WHERE OrderID = @OrderID";
                using (SqlCommand cmd = new SqlCommand(query, db.Connection))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    var status = cmd.ExecuteScalar();
                    Console.WriteLine(status != null ? $"Order Status: {status}" : "Order not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving status: " + ex.Message);
            }
            finally
            {
                db.CloseConnection();
            }
        }
        public void ViewOrderStatus()
        {
            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            string query = @"
        SELECT O.OrderID, O.OrderDate, O.TotalAmount,
               OD.ProductID, OD.Quantity,
               P.Price AS UnitPrice
        FROM Orders O
        INNER JOIN OrderDetails OD ON O.OrderID = OD.OrderID
        INNER JOIN Products P ON OD.ProductID = P.ProductID
        WHERE O.CustomerID = @CustomerID";

            using (SqlConnection conn = new SqlConnection(db.connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    Console.WriteLine("No orders found for this customer.");
                    return;
                }

                Console.WriteLine("Order Details:");
                while (reader.Read())
                {
                    Console.WriteLine($"Order ID: {reader["OrderID"]}, Date: {reader["OrderDate"]}, Product ID: {reader["ProductID"]}, Qty: {reader["Quantity"]}, Unit Price: rs.{reader["UnitPrice"]}, Total: rs.{reader["TotalAmount"]}");
                }
            }
        }
    }

            public class InventoryManager
            {
                private readonly DatabaseConnector db;

                public InventoryManager()
                {
                    db = new DatabaseConnector();
                }

                public void ManageInventory()
                {
                    try
                    {
                        db.OpenConnection();

                        Console.Write("Enter Product ID to update stock: ");
                        int productId = int.Parse(Console.ReadLine());

                        Console.Write("Enter new quantity in stock: ");
                        int quantity = int.Parse(Console.ReadLine());

                        string query = "UPDATE Inventory SET QuantityInStock = @Quantity WHERE ProductID = @ProductID";

                        using (SqlCommand cmd = new SqlCommand(query, db.Connection))
                        {
                            cmd.Parameters.AddWithValue("@Quantity", quantity);
                            cmd.Parameters.AddWithValue("@ProductID", productId);

                            int rows = cmd.ExecuteNonQuery();
                            Console.WriteLine(rows > 0 ? "Inventory updated successfully." : "Product not found or update failed.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error managing inventory: " + ex.Message);
                    }
                    finally
                    {
                        db.CloseConnection();
                    }
                }
            }
            public class ReportManager
            {
                private readonly DatabaseConnector db;

                public ReportManager()
                {
                    db = new DatabaseConnector();
                }

                public void GenerateSalesReport()
                {
                    try
                    {
                        db.OpenConnection();

                        string query = @"SELECT O.OrderID, O.OrderDate, C.FirstName + ' ' + C.LastName AS Customer, 
                            O.TotalAmount 
                            FROM Orders O 
                            JOIN Customers C ON O.CustomerID = C.CustomerID";

                        using (SqlCommand cmd = new SqlCommand(query, db.Connection))
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("\n--- Sales Report ---");
                            while (reader.Read())
                            {
                                Console.WriteLine($"OrderID: {reader["OrderID"]}, Date: {reader["OrderDate"]}, " +
                                                  $"Customer: {reader["Customer"]}, Amount: ₹{reader["TotalAmount"]}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error generating report: " + ex.Message);
                    }
                    finally
                    {
                        db.CloseConnection();
                    }
                }
            }
            public class CustomerManager2
            {
                public DatabaseConnector Db { get; set; }

                public CustomerManager2()
                {
                    Db = new DatabaseConnector();
                }

                // Already implemented: AddCustomer()

                public void UpdateCustomerAccount()
                {
                    try
                    {
                        Db.OpenConnection();

                        Console.Write("Enter Customer ID to update: ");
                        int customerId = int.Parse(Console.ReadLine());

                        Console.Write("Enter new Email: ");
                        string newEmail = Console.ReadLine();

                        Console.Write("Enter new Phone: ");
                        string newPhone = Console.ReadLine();

                        string query = @"UPDATE Customers 
                             SET Email = @Email, Phone = @Phone 
                             WHERE CustomerID = @CustomerID";

                        using (SqlCommand cmd = new SqlCommand(query, Db.Connection))
                        {
                            cmd.Parameters.AddWithValue("@Email", newEmail);
                            cmd.Parameters.AddWithValue("@Phone", newPhone);
                            cmd.Parameters.AddWithValue("@CustomerID", customerId);

                            int rows = cmd.ExecuteNonQuery();
                            Console.WriteLine(rows > 0 ? "Customer account updated." : "Customer not found.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating customer account: " + ex.Message);
                    }
                    finally
                    {
                        Db.CloseConnection();
                    }
                }
            }

            public class PaymentManager
            {
                public void ProcessPayment()
                {
                    Console.WriteLine("Processing payment...");
                    Console.WriteLine("Payment successful. (placeholder)");
                }
            }
            public class ProductManager2
            {
                private readonly DatabaseConnector db;

                public ProductManager2()
                {
                    db = new DatabaseConnector();
                }

                // Already implemented: UpdateProduct()

                public void SearchAndRecommendProducts()
                {
                    try
                    {
                        db.OpenConnection();

                        Console.Write("Enter product keyword to search: ");
                        string keyword = Console.ReadLine();

                        string query = @"SELECT ProductID, ProductName, Price, Description 
                             FROM Products 
                             WHERE ProductName LIKE @Keyword OR Description LIKE @Keyword";

                        using (SqlCommand cmd = new SqlCommand(query, db.Connection))
                        {
                            cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                Console.WriteLine("\n--- Search Results ---");
                                while (reader.Read())
                                {
                                    Console.WriteLine($"ID: {reader["ProductID"]}, Name: {reader["ProductName"]}, " +
                                                      $"Price: ₹{reader["Price"]}, Desc: {reader["Description"]}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in product search: " + ex.Message);
                    }
                    finally
                    {
                        db.CloseConnection();
                    }
                }
            }
            class Program1
            {
                static void Main(string[] args)
                {
                    CustomerManager customerManager = new CustomerManager();
                    CustomerManager2 customerManager2 = new CustomerManager2();
                    ProductManager productManager = new ProductManager();
                    ProductManager2 productManager2 = new ProductManager2();
                    OrderManager orderManager = new OrderManager();
                    OrderStatusManager orderStatusManager = new OrderStatusManager();
                    InventoryManager inventoryManager = new InventoryManager();
                    ReportManager reportManager = new ReportManager();
                    PaymentManager paymentManager = new PaymentManager();

                    bool exit = false;

                    while (!exit)
                    {
                        Console.WriteLine("\n--- TECHSHOP MAIN MENU ---");
                        Console.WriteLine("1. Customer Registration");
                        Console.WriteLine("2. Manage Product Catalog");
                        Console.WriteLine("3. Place Customer Order");
                        Console.WriteLine("4. Track Order Status");
                        Console.WriteLine("5. Manage Inventory");
                        Console.WriteLine("6. Sales Reporting");
                        Console.WriteLine("7. Update Customer Account");
                        Console.WriteLine("8. Process Payment");
                        Console.WriteLine("9. Product Search & Recommendations");
                        Console.WriteLine("10. Exit");
                        Console.Write("Choose an option: ");
                        string choice = Console.ReadLine();

                        switch (choice)
                        {
                            case "1":
                                Console.Write("Enter Customer ID: ");
                                int customerId = int.Parse(Console.ReadLine());

                                Console.Write("Enter First Name: ");
                                string firstName = Console.ReadLine();

                                Console.Write("Enter Last Name: ");
                                string lastName = Console.ReadLine();

                                Console.Write("Enter Email: ");
                                string email = Console.ReadLine();

                                Console.Write("Enter Phone: ");
                                string phone = Console.ReadLine();

                                Console.Write("Enter Address: ");
                                string address = Console.ReadLine();

                                Customer newCustomer = new Customer(customerId, firstName, lastName, email, phone, address);
                                customerManager.AddCustomer(newCustomer);
                                break;

                            case "2":
                                Console.Write("Enter Product ID to update: ");
                                int prodId = int.Parse(Console.ReadLine());

                                Console.Write("Enter new Price: ");
                                decimal price = decimal.Parse(Console.ReadLine());

                                Console.Write("Enter new Description: ");
                                string desc = Console.ReadLine();

                                productManager.UpdateProduct(prodId, price, desc);
                                break;

                            case "3":
                                orderManager.PlaceOrder();
                                break;

                            case "4":
                                orderStatusManager.ViewOrderStatus();
                                break;

                            case "5":
                                inventoryManager.ManageInventory();
                                break;

                            case "6":
                                reportManager.GenerateSalesReport();
                                break;

                            case "7":
                                customerManager2.UpdateCustomerAccount();
                                break;

                            case "8":
                                paymentManager.ProcessPayment();
                                break;

                            case "9":
                                productManager2.SearchAndRecommendProducts();
                                break;

                            case "10":
                                Console.WriteLine("Exiting the program. Thank you for using TechShop!");
                                exit = true;
                                break;

                            default:
                                Console.WriteLine("Invalid option. Please try again.");
                                break;
                        }
                    }
                }
            }
        }
    
