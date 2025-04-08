using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicGadgets
{
    public class Customer
    {
        private int customerID;
        private string firstName = "";
        private string lastName = "";
        private string email = "";
        private string phone = "";
        private string address = "";

        private string connectionString = "Data Source=RAKS\\SQL2022;Initial Catalog=TechShop;Integrated Security=True";

        public int CustomerID
        {
            get => customerID;
            set => customerID = value;
        }
        public string FirstName
        {
            get => firstName;
            set => firstName = value;
        }
        public string LastName
        {
            get => lastName;
            set => lastName = value;
        }
        public string Email
        {
            get => email;
            set => email = value;
        }
        public string Phone
        {
            get => phone;
            set => phone = value;
        }
        public string Address
        {
            get => address;
            set => address = value;
        }

        public Customer() { }

        public Customer(int id, string firstName, string lastName, string email, string phone, string address)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new InvalidDataException("First Name and Last Name cannot be empty.");
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new InvalidDataException("Invalid email address format.");
            if (string.IsNullOrWhiteSpace(phone) || phone.Length < 10)
                throw new InvalidDataException("Phone number must be at least 10 digits.");
            if (string.IsNullOrWhiteSpace(address))
                throw new InvalidDataException("Address cannot be empty.");
            CustomerID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Address = address;
        }


        public void CalculateTotalOrders()
        {
            try { 
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Orders WHERE CustomerID = @CustomerID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

                int totalOrders = (int)cmd.ExecuteScalar();
                Console.WriteLine($"{FirstName} {LastName} has placed {totalOrders} order(s).");
            }
        }
            catch (SqlException ex)
{
                Console.WriteLine("Database error: " + ex.Message);
            }
}

        public void GetCustomerDetails()
        {
            try { 
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Customers WHERE CustomerID = @CustomerID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    FirstName = reader["FirstName"].ToString();
                    LastName = reader["LastName"].ToString();
                    Email = reader["Email"].ToString();
                    Phone = reader["Phone"].ToString();
                    Address = reader["Address"].ToString();

                    Console.WriteLine("Customer Details:");
                    Console.WriteLine($"ID: {CustomerID}");
                    Console.WriteLine($"Name: {FirstName} {LastName}");
                    Console.WriteLine($"Email: {Email}");
                    Console.WriteLine($"Phone: {Phone}");
                    Console.WriteLine($"Address: {Address}");
                }
                else
                {
                    Console.WriteLine("Customer not found.");
                }
            }
        }
            catch (SqlException ex)
{
                Console.WriteLine("Database error: " + ex.Message);
            }
}
        public void UpdateCustomerInfo(string newEmail, string newPhone, string newAddress)
        {
            try {
                if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains("@"))
                    throw new InvalidDataException("Invalid email address format.");
                if (string.IsNullOrWhiteSpace(newPhone) || newPhone.Length < 10)
                    throw new InvalidDataException("Phone number must be at least 10 digits.");
                if (string.IsNullOrWhiteSpace(newAddress))
                    throw new InvalidDataException("Address cannot be empty.");
                using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Customers SET Email = @Email, Phone = @Phone, Address = @Address WHERE CustomerID = @CustomerID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", newEmail);
                cmd.Parameters.AddWithValue("@Phone", newPhone);
                cmd.Parameters.AddWithValue("@Address", newAddress);
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Customer information updated successfully.");
                    Email = newEmail;
                    Phone = newPhone;
                    Address = newAddress;
                }
                else
                {
                    Console.WriteLine("Update failed. Customer not found.");
                }
            }
        }
            catch (SqlException ex)
{
                Console.WriteLine("Database error: " + ex.Message);
            }
}
    }
    public class Product
    {
        private int productID;
        private string productName = "";
        private string description = "";
        private decimal price;
        private string category = "";

        private string connectionString = "Data Source=RAKS\\SQL2022;Initial Catalog=TechShop;Integrated Security=True";

        public int ProductID
        {
            get => productID;
            set => productID = value;
        }
        public string ProductName
        {
            get => productName;
            set => productName = value;
        }
        public string Description
        {
            get => description;
            set => description = value;
        }
        public decimal Price
        {
            get => price;
            set
            {
                if (value < 0)
                    throw new InvalidDataException("Price cannot be negative.");
                price = value;
            }
        }
        public string Category
        {
            get => category;
            set => category = value;
        }

        public Product() { }

        public Product(int id, string name, string desc, decimal price,string category)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidDataException("Product name cannot be empty.");
            if (string.IsNullOrWhiteSpace(desc))
                throw new InvalidDataException("Description cannot be empty.");
            if (price < 0)
                throw new InvalidDataException("Price cannot be negative.");

            ProductID = id;
            ProductName = name;
            Description = desc;
            Price = price;
            Category=category;
               
        }

        public void GetProductDetails()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Products WHERE ProductID = @ProductID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProductID", ProductID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        ProductName = reader["ProductName"].ToString();
                        Description = reader["Description"].ToString();
                        Price = Convert.ToDecimal(reader["Price"]);

                        Console.WriteLine("Product Details:");
                        Console.WriteLine($"ID: {ProductID}");
                        Console.WriteLine($"Name: {ProductName}");
                        Console.WriteLine($"Description: {Description}");
                        Console.WriteLine($"Price: {Price:F2}");
                    }
                    else
                    {
                        Console.WriteLine("Product not found.");
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
            }
        }

        public void UpdateProductInfo(decimal newPrice, string newDescription)
        {
            try
            {
                if (newPrice < 0)
                    throw new InvalidDataException("Price cannot be negative.");
                if (string.IsNullOrWhiteSpace(newDescription))
                    throw new InvalidDataException("Description cannot be empty.");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Products SET Price = @Price, Description = @Description WHERE ProductID = @ProductID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Price", newPrice);
                    cmd.Parameters.AddWithValue("@Description", newDescription);
                    cmd.Parameters.AddWithValue("@ProductID", ProductID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Product information updated successfully.");
                        Price = newPrice;
                        Description = newDescription;
                    }
                    else
                    {
                        Console.WriteLine("Update failed. Product not found.");
                    }
                }
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine("Validation Error: " + ex.Message);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
            }
        }

        public void IsProductInStock()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT QuantityInStock FROM Inventory WHERE ProductID = @ProductID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProductID", ProductID);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        int quantity = Convert.ToInt32(result);
                        if (quantity > 0)
                        {
                            Console.WriteLine($"Product '{ProductName}' is in stock. (Quantity: {quantity})");
                        }
                        else
                        {
                            Console.WriteLine($"Product '{ProductName}' is out of stock.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No inventory record found for this product.");
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
            }
        }
    }
    public class Order
    {
        public int OrderID { get; set; }
        public Customer Customer { get; set; } // Composition
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        private string connectionString = "Data Source=RAKS\\SQL2022;Initial Catalog=TechShop;Integrated Security=True";

        public void CalculateTotalAmount(int orderId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                    SELECT od.ProductID, od.Quantity, p.Price
                    FROM OrderDetails od
                    JOIN Products p ON od.ProductID = p.ProductID
                    WHERE od.OrderID = @OrderID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    decimal totalAmount = 0;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No order details found.");
                            return;
                        }

                        while (reader.Read())
                        {
                            int productId = reader.GetInt32(0);
                            int quantity = reader.GetInt32(1);
                            decimal price = reader.GetDecimal(2);
                            decimal subtotal = quantity * price;
                            totalAmount += subtotal;

                            Console.WriteLine($"Product ID: {productId}, Quantity: {quantity}, Price: Rs.{price}, Subtotal: Rs.{subtotal}");
                        }
                    }

                    Console.WriteLine($"Total Amount for Order ID {orderId}: Rs.{totalAmount}");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while calculating total amount: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }

        public void GetOrderDetails()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT o.OrderID, c.FirstName, c.LastName, o.OrderDate, p.ProductName, od.Quantity, p.Price 
                                 FROM Orders o 
                                 JOIN Customers c ON o.CustomerID = c.CustomerID
                                 JOIN OrderDetails od ON o.OrderID = od.OrderID 
                                 JOIN Products p ON od.ProductID = p.ProductID 
                                 WHERE o.OrderID = @OrderID";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        Console.WriteLine("No order details found.");
                        return;
                    }

                    while (reader.Read())
                    {
                        Console.WriteLine($"OrderID: {reader["OrderID"]}, Customer: {reader["FirstName"]} {reader["LastName"]}, Date: {reader["OrderDate"]}, Product: {reader["ProductName"]}, Quantity: {reader["Quantity"]}, Price: {reader["Price"]}");
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while retrieving order details: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }

        public void UpdateOrderStatus(string newStatus)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newStatus))
                {
                    Console.WriteLine("Invalid status provided.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Orders SET Status = @Status WHERE OrderID = @OrderID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);

                    connection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Order status updated." : "Order not found.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while updating status: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }

        public void CheckOrderStatus(int orderId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Status FROM Orders WHERE OrderID = @OrderID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        Console.WriteLine($"Order ID {orderId} Status: {result}");
                    }
                    else
                    {
                        Console.WriteLine("Order not found.");
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while checking status: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }

        public void CancelOrder(int orderId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // Step 1: Get Order Details
                        string getOrderDetailsQuery = @"
                        SELECT ProductID, Quantity
                        FROM OrderDetails
                        WHERE OrderID = @OrderID";

                        SqlCommand getOrderDetailsCmd = new SqlCommand(getOrderDetailsQuery, conn, transaction);
                        getOrderDetailsCmd.Parameters.AddWithValue("@OrderID", orderId);

                        List<(int ProductID, int Quantity)> orderDetails = new List<(int, int)>();
                        using (SqlDataReader reader = getOrderDetailsCmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("No products found for the order.");
                                return;
                            }

                            while (reader.Read())
                            {
                                int productId = reader.GetInt32(0);
                                int quantity = reader.GetInt32(1);
                                orderDetails.Add((productId, quantity));
                            }
                        }

                        // Step 2: Update Inventory
                        foreach (var item in orderDetails)
                        {
                            string updateInventoryQuery = @"
                            UPDATE Inventory
                            SET QuantityInStock = QuantityInStock + @Quantity
                            WHERE ProductID = @ProductID";

                            SqlCommand updateInventoryCmd = new SqlCommand(updateInventoryQuery, conn, transaction);
                            updateInventoryCmd.Parameters.AddWithValue("@ProductID", item.ProductID);
                            updateInventoryCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                            updateInventoryCmd.ExecuteNonQuery();
                        }

                        // Step 3: Delete Order Details
                        string deleteOrderDetailsQuery = "DELETE FROM OrderDetails WHERE OrderID = @OrderID";
                        SqlCommand deleteOrderDetailsCmd = new SqlCommand(deleteOrderDetailsQuery, conn, transaction);
                        deleteOrderDetailsCmd.Parameters.AddWithValue("@OrderID", orderId);
                        deleteOrderDetailsCmd.ExecuteNonQuery();

                        // Step 4: Delete Order or Update Status
                        string deleteOrderQuery = "DELETE FROM Orders WHERE OrderID = @OrderID";
                        SqlCommand deleteOrderCmd = new SqlCommand(deleteOrderQuery, conn, transaction);
                        deleteOrderCmd.Parameters.AddWithValue("@OrderID", orderId);
                        deleteOrderCmd.ExecuteNonQuery();

                        // If you prefer status update instead of delete:
                        // string updateOrderStatusQuery = "UPDATE Orders SET Status = 'Cancelled' WHERE OrderID = @OrderID";
                        // SqlCommand updateOrderStatusCmd = new SqlCommand(updateOrderStatusQuery, conn, transaction);
                        // updateOrderStatusCmd.Parameters.AddWithValue("@OrderID", orderId);
                        // updateOrderStatusCmd.ExecuteNonQuery();

                        transaction.Commit();
                        Console.WriteLine("Order cancelled successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to error: " + ex.Message);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database connection error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error during order cancellation: " + ex.Message);
            }
        }
    }

    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public Order Order { get; set; }     // Composition
        public Product Product { get; set; } // Composition
        public int Quantity { get; set; }

        private string connectionString = "Data Source=RAKS\\SQL2022;Initial Catalog=TechShop;Integrated Security=True";

        public void CalculateSubtotal(int orderId)
        {
            try { 
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
            SELECT od.ProductID, od.Quantity, p.Price
            FROM OrderDetails od
            INNER JOIN Products p ON od.ProductID = p.ProductID
            WHERE od.OrderID = @OrderID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int productId = reader.GetInt32(0);
                        int quantity = reader.GetInt32(1);
                        decimal price = reader.GetDecimal(2);

                        // ❗ Order processing check
                        if (productId == 0)
                        {
                            throw new IncompleteOrderException("Order detail lacks a valid product reference.");
                        }

                        if (quantity <= 0)
                        {
                            throw new IncompleteOrderException("Order detail has invalid quantity.");
                        }

                        decimal subtotal = price * quantity;
                        Console.WriteLine($"Product ID: {productId}, Quantity: {quantity}, Price: Rs.{price}, Subtotal: Rs.{subtotal}");
                    }
}
                }
            catch (SqlException ex)
{
                Console.WriteLine("Database error: " + ex.Message);
            }
}

        public void GetOrderDetailInfo()
        {
            try { 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT od.OrderDetailID, o.OrderID, p.ProductName, od.Quantity, p.Price 
                         FROM OrderDetails od
                         JOIN Orders o ON od.OrderID = o.OrderID
                         JOIN Products p ON od.ProductID = p.ProductID
                         WHERE od.OrderDetailID = @OrderDetailID";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@OrderDetailID", OrderDetailID);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Console.WriteLine($"OrderDetail ID: {reader["OrderDetailID"]}");
                    Console.WriteLine($"Order ID: {reader["OrderID"]}");
                    Console.WriteLine($"Product Name: {reader["ProductName"]}");
                    Console.WriteLine($"Quantity: {reader["Quantity"]}");
                    Console.WriteLine($"Price: {reader["Price"]}");
                }
                else
                {
                    Console.WriteLine("Order detail not found.");
                }
            }
        }
            catch (SqlException ex)
{
                Console.WriteLine("Database error: " + ex.Message);
            }
}



        public void UpdateQuantity(int newQty)
        {
            try
            {
                if (newQty <= 0)
                {
                    throw new InvalidDataException("Quantity must be greater than 0.");
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE OrderDetails SET Quantity = @Quantity WHERE OrderDetailID = @OrderDetailID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Quantity", newQty);
                    cmd.Parameters.AddWithValue("@OrderDetailID", OrderDetailID);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Order detail quantity updated.");
                }
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine("Validation error: " + ex.Message);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
            }
        }


        public void AddDiscount(decimal discountPercentage)
        {
            try { 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Products
                             SET Price = Price - (Price * @Discount)
                             WHERE ProductID = @ProductID";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Discount", discountPercentage / 100);
                cmd.Parameters.AddWithValue("@ProductID", Product.ProductID);

                connection.Open();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Discount applied to product.");
            }
        }
            catch (SqlException ex)
{
                Console.WriteLine("Database error: " + ex.Message);
            }
}
    }

    public class Inventory
    {
        public int InventoryID { get; set; }
        public Product Product { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime LastStockUpdate { get; set; }

        private string connectionString = "Data Source=RAKS\\SQL2022;Initial Catalog=TechShop;Integrated Security=True";
        public Inventory(int productId, int quantity)
        {
            Product = new Product { ProductID = productId }; 
            QuantityInStock = quantity;
            LastStockUpdate = DateTime.Now;
        }

        public Inventory()
        {
        }

        public void GetProduct()
        {
            Console.WriteLine($"Product in Inventory: {Product.ProductName}");
        }

        public int GetQuantityInStock()
        {
            return QuantityInStock;
        }

        public void AddToInventory(int quantity)
        {
            QuantityInStock += quantity;
            LastStockUpdate = DateTime.Now;
            UpdateStockInDatabase();
            Console.WriteLine($"{quantity} units added to inventory.");
        }

        public void RemoveFromInventory(int quantity)
        {
            if (QuantityInStock >= quantity)
            {
                QuantityInStock -= quantity;
                LastStockUpdate = DateTime.Now;
                UpdateStockInDatabase();
                Console.WriteLine($"{quantity} units removed from inventory.");
            }
            else
            {
                throw new InsufficientStockException($"Cannot remove {quantity} units. Only {QuantityInStock} units are available in stock.");
            }
        }

        public void UpdateStockQuantity(int newQuantity)
        {
            QuantityInStock = newQuantity;
            LastStockUpdate = DateTime.Now;
            UpdateStockInDatabase();
            Console.WriteLine("Inventory quantity updated.");
        }

        public bool IsProductAvailable(int quantityToCheck)
        {
            return QuantityInStock >= quantityToCheck;
        }

        public void GetInventoryValue()
        {
            try { 
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
            SELECT p.ProductID, p.ProductName, p.Price, i.QuantityInStock
            FROM Products p
            INNER JOIN Inventory i ON p.ProductID = i.ProductID";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                decimal totalValue = 0;

                while (reader.Read())
                {
                    int productId = reader.GetInt32(0);
                    string productName = reader.GetString(1);
                    decimal price = reader.GetDecimal(2);
                    int quantity = reader.GetInt32(3);

                    decimal productValue = price * quantity;
                    totalValue += productValue;

                    Console.WriteLine($"Product ID: {productId}, Name: {productName}, Price: rs.{price}, Quantity: {quantity}, Value: rs.{productValue}");
                }

                reader.Close();

                Console.WriteLine($"\nTotal Inventory Value: rs.{totalValue}");
            } }
            catch (SqlException ex)
{
                Console.WriteLine("Database error: " + ex.Message);
            }

        }


        public void ListLowStockProducts(int threshold)
        {
            try { 
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT p.ProductName, i.QuantityInStock FROM Inventory i JOIN Products p ON i.ProductID = p.ProductID WHERE i.QuantityInStock < @Threshold";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Threshold", threshold);

                SqlDataReader reader = cmd.ExecuteReader();
                Console.WriteLine("Low Stock Products:");
                while (reader.Read())
                {
                    Console.WriteLine($"- {reader["ProductName"]} (Stock: {reader["QuantityInStock"]})");
                }
            }}
            catch (SqlException ex)
{
                Console.WriteLine("Database error: " + ex.Message);
            }

        }

        public void ListOutOfStockProducts()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT p.ProductName FROM Inventory i JOIN Products p ON i.ProductID = p.ProductID WHERE i.QuantityInStock = 0";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    Console.WriteLine("Out of Stock Products:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"- {reader["ProductName"]}");
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
            }

        }

        public void ListAllProducts()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT p.ProductName, i.QuantityInStock FROM Inventory i JOIN Products p ON i.ProductID = p.ProductID";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    Console.WriteLine("All Products in Inventory:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"- {reader["ProductName"]}: {reader["QuantityInStock"]} units");
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
            }

        }

        private void UpdateStockInDatabase()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Inventory SET QuantityInStock = @QuantityInStock, LastStockUpdate = @LastStockUpdate WHERE InventoryID = @InventoryID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@QuantityInStock", QuantityInStock);
                    cmd.Parameters.AddWithValue("@LastStockUpdate", LastStockUpdate);
                    cmd.Parameters.AddWithValue("@InventoryID", InventoryID);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error: " + ex.Message);
            }

        }
    }
}

