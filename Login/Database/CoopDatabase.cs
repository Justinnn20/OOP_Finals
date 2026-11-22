using System.Data;
using Login;
using Login.Models;
using Microsoft.Data.SqlClient;

namespace Login.Database;

public sealed record DatabaseUser(string Username, string Password, string Role);
public sealed record DatabaseStaff(int Id, string FullName, string Username, string Password, string Role, bool IsActive);
public sealed record DatabaseProduct(string Sku, string Name, string Category, decimal Price, int Stock);
public sealed record DatabaseTransaction(string OrderId, DateTime CreatedAt, string CashierUsername,
    string PurchaserName, string YearLevel, string College, decimal Subtotal, string DiscountName,
    decimal DiscountAmount, decimal Total, string PaymentMethod, decimal CashReceived, decimal ChangeAmount,
    List<TransactionItem> Items);

public static class CoopDatabase
{
    private static readonly Lazy<string> ConnectionString = new(BuildConnectionString);

    public static string ConnectionStringValue => ConnectionString.Value;

    private static string BuildConnectionString()
    {
        string? configured = Environment.GetEnvironmentVariable("COOPSTORE_CONNECTION_STRING");
        if (!string.IsNullOrWhiteSpace(configured))
        {
            return configured;
        }

        string? mdfPath = Environment.GetEnvironmentVariable("COOPSTORE_MDF_PATH");
        if (string.IsNullOrWhiteSpace(mdfPath))
        {
            mdfPath = FindDatabaseFile();
        }

        return $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={mdfPath};Integrated Security=True;Connect Timeout=10;TrustServerCertificate=True";
    }

    public static List<DatabaseStaff> LoadStaff()
    {
        using SqlConnection connection = OpenConnection();
        using SqlCommand command = new("SELECT UserId, FullName, Username, Password, Role FROM dbo.Users ORDER BY UserId", connection);
        using SqlDataReader reader = command.ExecuteReader();
        List<DatabaseStaff> staff = new();
        while (reader.Read())
        {
            staff.Add(new DatabaseStaff(reader.GetInt32(0), reader.IsDBNull(1) ? reader.GetString(2) : reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), true));
        }

        return staff;
    }

    public static void SaveStaff(int? userId, string fullName, string username, string password, string role)
    {
        using SqlConnection connection = OpenConnection();
        using SqlCommand command = userId.HasValue
            ? new SqlCommand("UPDATE dbo.Users SET FullName = @FullName, Username = @Username, Password = @Password, Role = @Role WHERE UserId = @UserId", connection)
            : new SqlCommand("IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = @Username) INSERT dbo.Users (FullName, Username, Password, Role, CreatedAt) VALUES (@FullName, @Username, @Password, @Role, SYSUTCDATETIME())", connection);
        command.Parameters.AddWithValue("@FullName", fullName.Trim());
        command.Parameters.AddWithValue("@Username", username.Trim());
        command.Parameters.AddWithValue("@Password", password);
        command.Parameters.AddWithValue("@Role", role);
        if (userId.HasValue) command.Parameters.AddWithValue("@UserId", userId.Value);
        if (command.ExecuteNonQuery() == 0 && !userId.HasValue)
        {
            throw new InvalidOperationException("That username already exists.");
        }
    }

    public static void DeleteStaff(int userId)
    {
        using SqlConnection connection = OpenConnection();
        using SqlCommand command = new("DELETE FROM dbo.Users WHERE UserId = @UserId AND Role <> 'Admin'", connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.ExecuteNonQuery();
    }

    public static List<DatabaseTransaction> LoadOrders()
    {
        List<DatabaseTransaction> orders = new();
        using SqlConnection connection = OpenConnection();
        using SqlCommand command = new(@"
SELECT o.OrderId, o.CreatedAt, o.CashierUsername, o.PurchaserName, o.YearLevel, o.College,
       o.Subtotal, o.DiscountName, o.DiscountAmount, o.Total, o.PaymentMethod, o.CashReceived, o.ChangeAmount,
       i.ProductName, i.UnitPrice, i.Quantity, i.LineTotal
FROM dbo.Orders o
LEFT JOIN dbo.OrderItems i ON i.OrderId = o.OrderId
ORDER BY o.CreatedAt DESC;", connection);
        using SqlDataReader reader = command.ExecuteReader();
        Dictionary<string, DatabaseTransaction> byId = new(StringComparer.OrdinalIgnoreCase);
        while (reader.Read())
        {
            string orderId = reader.GetString(0);
            if (!byId.TryGetValue(orderId, out DatabaseTransaction? order))
            {
                order = new DatabaseTransaction(orderId, reader.GetDateTime(1), reader.IsDBNull(2) ? "" : reader.GetString(2),
                    reader.IsDBNull(3) ? "" : reader.GetString(3), reader.IsDBNull(4) ? "" : reader.GetString(4),
                    reader.IsDBNull(5) ? "" : reader.GetString(5), reader.GetDecimal(6), reader.GetString(7),
                    reader.GetDecimal(8), reader.GetDecimal(9), reader.GetString(10), reader.GetDecimal(11),
                    reader.GetDecimal(12), new List<TransactionItem>());
                byId.Add(orderId, order);
            }

            if (!reader.IsDBNull(13))
            {
                order.Items.Add(new TransactionItem
                {
                    Name = reader.GetString(13),
                    UnitPrice = reader.GetDecimal(14),
                    Qty = reader.GetInt32(15),
                    LineTotal = reader.GetDecimal(16)
                });
            }
        }

        orders.AddRange(byId.Values);
        return orders;
    }

    public static List<TransactionRecord> LoadTransactionRecords()
    {
        return LoadOrders().Select(order => new TransactionRecord
        {
            OrderNo = order.OrderId,
            OrderDate = order.CreatedAt,
            CashierName = order.CashierUsername,
            StudentId = order.PurchaserName,
            DiscountType = order.DiscountName,
            PaymentMethod = order.PaymentMethod,
            Subtotal = order.Subtotal,
            DiscountAmount = order.DiscountAmount,
            Total = order.Total,
            CashReceived = order.CashReceived,
            Change = order.ChangeAmount,
            Items = order.Items
        }).ToList();
    }

    private static string FindDatabaseFile()
    {
        string fileName = "UmCoopStoreDatabase.mdf";
        DirectoryInfo? directory = new(AppContext.BaseDirectory);
        while (directory != null)
        {
            string directPath = Path.Combine(directory.FullName, fileName);
            if (File.Exists(directPath))
            {
                return directPath;
            }

            string loginPath = Path.Combine(directory.FullName, "Login", fileName);
            if (File.Exists(loginPath))
            {
                return loginPath;
            }

            directory = directory.Parent;
        }

        throw new FileNotFoundException(
            "UmCoopStoreDatabase.mdf was not found. Set COOPSTORE_MDF_PATH or COOPSTORE_CONNECTION_STRING.",
            fileName);
    }

    public static bool TryAuthenticate(string username, string password, out DatabaseUser? user, out string error)
    {
        user = null;
        error = string.Empty;

        try
        {
            using SqlConnection connection = OpenConnection();
            using SqlCommand command = new(
                "SELECT Username, Password, Role FROM dbo.Users WHERE Username = @Username AND Password = @Password",
                connection);
            command.Parameters.AddWithValue("@Username", username.Trim());
            command.Parameters.AddWithValue("@Password", password);

            using SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                user = new DatabaseUser(reader.GetString(0), reader.GetString(1), reader.GetString(2));
                return true;
            }

            error = "Invalid username or password.";
            return false;
        }
        catch (Exception ex) when (ex is SqlException or IOException)
        {
            error = $"Database connection failed: {ex.Message}";
            return false;
        }
    }

    public static List<Product> LoadProducts()
    {
        List<Product> products = new();
        using SqlConnection connection = OpenConnection();
        using SqlCommand command = new(
            "SELECT Sku, ProductName, Category, Price, Variant, StockQuantity FROM dbo.Products WHERE IsActive = 1 ORDER BY ProductId",
            connection);
        using SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            products.Add(new Product(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetDecimal(3),
                reader.IsDBNull(4) ? null : reader.GetString(4),
                reader.GetInt32(5)));
        }

        return products;
    }

    public static List<DatabaseProduct> LoadInventory()
    {
        using SqlConnection connection = OpenConnection();
        using SqlCommand command = new("SELECT Sku, ProductName, Category, Price, StockQuantity FROM dbo.Products WHERE IsActive = 1 ORDER BY ProductId", connection);
        using SqlDataReader reader = command.ExecuteReader();
        List<DatabaseProduct> products = new();
        while (reader.Read())
        {
            products.Add(new DatabaseProduct(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetDecimal(3), reader.GetInt32(4)));
        }

        return products;
    }

    public static void SetStock(string sku, int stock)
    {
        using SqlConnection connection = OpenConnection();
        using SqlCommand command = new("UPDATE dbo.Products SET StockQuantity = @Stock WHERE Sku = @Sku", connection);
        command.Parameters.AddWithValue("@Sku", sku);
        command.Parameters.AddWithValue("@Stock", Math.Max(0, stock));
        command.ExecuteNonQuery();
    }

    public static void SaveOrder(Order order, string cashierUsername, string purchaserName, string yearLevel, string college)
    {
        if (order.Items.Count == 0)
        {
            throw new InvalidOperationException("Cannot save an empty order.");
        }

        using SqlConnection connection = OpenConnection();
        using SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);
        try
        {
            using SqlCommand orderCommand = new(@"
INSERT INTO dbo.Orders
    (OrderId, CreatedAt, CashierUsername, PurchaserName, YearLevel, College, Subtotal, DiscountName, DiscountAmount, Total, PaymentMethod, CashReceived, ChangeAmount)
VALUES
    (@OrderId, @CreatedAt, @CashierUsername, @PurchaserName, @YearLevel, @College, @Subtotal, @DiscountName, @DiscountAmount, @Total, @PaymentMethod, @CashReceived, @ChangeAmount);", connection, transaction);
            orderCommand.Parameters.AddWithValue("@OrderId", order.OrderId);
            orderCommand.Parameters.AddWithValue("@CreatedAt", order.CreatedAt);
            orderCommand.Parameters.AddWithValue("@CashierUsername", cashierUsername ?? string.Empty);
            orderCommand.Parameters.AddWithValue("@PurchaserName", purchaserName ?? string.Empty);
            orderCommand.Parameters.AddWithValue("@YearLevel", yearLevel ?? string.Empty);
            orderCommand.Parameters.AddWithValue("@College", college ?? string.Empty);
            AddDecimal(orderCommand, "@Subtotal", order.Subtotal);
            orderCommand.Parameters.AddWithValue("@DiscountName", order.DiscountName);
            AddDecimal(orderCommand, "@DiscountAmount", order.DiscountAmount);
            AddDecimal(orderCommand, "@Total", order.Total);
            orderCommand.Parameters.AddWithValue("@PaymentMethod", order.PaymentMethod);
            AddDecimal(orderCommand, "@CashReceived", order.CashReceived);
            AddDecimal(orderCommand, "@ChangeAmount", order.Change);
            orderCommand.ExecuteNonQuery();

            foreach (OrderItem item in order.Items)
            {
                using SqlCommand itemCommand = new(@"
INSERT INTO dbo.OrderItems
    (OrderId, ProductId, ProductName, Sku, UnitPrice, Quantity, LineTotal)
SELECT @OrderId, ProductId, @ProductName, Sku, @UnitPrice, @Quantity, @LineTotal
FROM dbo.Products
WHERE Sku = @Sku;", connection, transaction);
                itemCommand.Parameters.AddWithValue("@OrderId", order.OrderId);
                itemCommand.Parameters.AddWithValue("@ProductName", item.Product.Name);
                itemCommand.Parameters.AddWithValue("@Sku", item.Product.Sku);
                AddDecimal(itemCommand, "@UnitPrice", item.UnitPrice);
                itemCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                AddDecimal(itemCommand, "@LineTotal", item.TotalPrice);
                if (itemCommand.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException($"Product '{item.Product.Sku}' was not found in the database.");
                }

                using SqlCommand stockCommand = new(
                    "UPDATE dbo.Products SET StockQuantity = StockQuantity - @Quantity WHERE Sku = @Sku AND StockQuantity >= @Quantity",
                    connection, transaction);
                stockCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                stockCommand.Parameters.AddWithValue("@Sku", item.Product.Sku);
                if (stockCommand.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException($"Not enough stock for '{item.Product.Name}'.");
                }
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private static SqlConnection OpenConnection()
    {
        SqlConnection connection = new(ConnectionString.Value);
        connection.Open();
        return connection;
    }

    private static void AddDecimal(SqlCommand command, string name, decimal value)
    {
        SqlParameter parameter = command.Parameters.Add(name, SqlDbType.Decimal);
        parameter.Precision = 10;
        parameter.Scale = 2;
        parameter.Value = value;
    }
}
