# Project Documentation: OOP, CRUD, MVC and Database

UMak Coop Store system, a C# Windows Forms app (.NET, `Login.csproj`) with a SQL Server LocalDB database (`SQLQuerycoopstore.sql`).
Namespaces used: `Login`, `Login.Models`, `Login.Database` and `UMakCoop.*`.

## 1. OOP Concepts

### 1.1 Inheritance (Parent and Child)

| Parent class (base) | File | Child class (derived) | File | What the child changes |
|---|---|---|---|---|
| `Discount` (abstract) | `Payment/Discount.cs` | `NoDiscount` | `Payment/NoDiscount.cs` | `Name` = "None", `Rate` = 0 |
| | | `SeniorDiscount` | `Payment/SeniorDiscount.cs` | `Name`, `Rate` = 20%, `RequiresIdNumber` = true, `IdLabel` |
| | | `PwdDiscount` | `Payment/PwdDiscount.cs` | `Name`, `Rate` = 20%, `RequiresIdNumber` = true, `IdLabel` |
| | | `RecordedDiscount` (private nested class) | `Payment/ReceiptStore.cs` | Rebuilds a saved discount when old receipts are loaded |
| `Payment` (abstract) | `Payment/Payment.cs` | `CashPayment` | `Payment/CashPayment.cs` | `Method`, `Change`, `TryValidate()` (cash rules), `ReceiptLine` |
| | | `GCashPayment` | `Payment/GCashPayment.cs` | `Method`, `Change` = 0, `TryValidate()` (13-digit reference), `ReceiptLine` |
| `Form` (WinForms) | .NET library | `LoginPage`, `AdminDB`, `AdminInventory`, `AdminSettings`, `AdminReports`, `AdminTransactions`, `Form2`, `CustomerInfoForm`, `OrderReceiptForm`, `ReceiptForm`, `SummaryForm`, `CoopDialogForm` | `Login/`, `Admin/`, `Dashboard/`, `Payment/`, `CoopDialog.cs` | Each screen is a child of `Form` |
| `Control` (WinForms) | .NET library | `StampControl`, `ReceiptPaper` | `StampControl.cs`, `Payment/ReceiptPaper.cs` | Custom drawing in `OnPaint` |
| `UserControl` (WinForms) | .NET library | `CheckoutPanel`, `TransactionsView`, `CartItemControl` | `Payment/CheckoutPanel.cs`, `Dashboard/TransactionsView.cs`, `Cart/Controls/CartItemControl.cs` | Reusable panels |
| `Panel` / `Button` (WinForms) | .NET library | `RoundedPanel`, `RoundedButton` | `UiKit.cs` | Rounded look, hover and press effects |
| `ApplicationContext` (WinForms) | .NET library | `CoopAppContext` | `Program.cs` | Switches between the login screen and the main forms |

### 1.2 Abstraction

| Where | File | Explanation |
|---|---|---|
| `abstract class Discount` | `Payment/Discount.cs` | `Name` and `Rate` are abstract, so every child must define them. |
| `abstract class Payment` | `Payment/Payment.cs` | `Method`, `Change`, `TryValidate()` and `ReceiptLine` are abstract. |
| `interface IStockService` | `Cart/IStockService.cs` | Declares `HasStock`, `Deduct` and `Restore` without saying how stock is stored. |

Interface implementer: `InMemoryStockService` (`Cart/InMemoryStockService.cs`) implements `IStockService`. `TransactionController` (`Dashboard/TransactionController.cs`) depends only on the interface.

### 1.3 Polymorphism

| Type | File | Explanation |
|---|---|---|
| Method overriding (`Discount`) | `Payment/*Discount.cs` | `PaymentController` keeps a `List<Discount>` (No, Senior, PWD). Calling `SelectedDiscount.Compute(subtotal)` runs the right behavior for the chosen child. |
| Method overriding (`Payment`) | `Payment/CashPayment.cs`, `Payment/GCashPayment.cs` | `Receipt` holds a `Payment`. `Payment.TryValidate()` and `Payment.ReceiptLine` behave differently for Cash and GCash. |
| `virtual` members | `Payment/Discount.cs` | `RequiresIdNumber`, `IdLabel` and `Compute()` have defaults that children can override. |
| `ToString()` override | `Payment/Discount.cs` | Shows "Senior Citizen 20%". |
| Framework overrides | `UiKit.cs`, `StampControl.cs`, `Payment/ReceiptPaper.cs`, `CoopDialog.cs`, `LoginPage.cs`, `Admin*.cs` | `OnPaint`, `OnResize`, `OnMouseEnter`, etc. |
| Constructor overloading | `Cart/Models/Order.cs` | `Order()` generates an ID, `Order(string customId)` uses a given ID. |

### 1.4 Encapsulation

| Class | File | Explanation |
|---|---|---|
| `OrderItem` | `Cart/Models/OrderItem.cs` | `Quantity` has a `private set`. It only changes through `Increment()`, `Decrement()` and `SetQuantity()`. |
| `Order` | `Cart/Models/Order.cs` | `OrderId`, `Items` and `CreatedAt` are read-only. `Subtotal`, `DiscountAmount`, `Total` and `Change` are computed properties. |
| `Receipt` | `Payment/Receipt.cs` | Properties use `private set`. The `_lines` list is private and exposed read-only. Voiding uses `internal` methods. |
| `ReceiptLine` | `Payment/ReceiptLine.cs` | `private set` and validation in the constructor. |
| `Payment` | `Payment/Payment.cs` | `AmountDue` has `private set` and is validated in the constructor. |
| `Product` | `Cart/Models/Product.cs` | Properties and a `DisplayDetails` helper. |
| `CoopDatabase` | `Database/CoopDatabase.cs` | `OpenConnection()` and `AddDecimal()` are private, and the connection string is hidden. |
| `CartController` | `Cart/Controllers/CartController.cs` | `_catalog`, `_heldOrders` and `_barcodeParser` are private. They are exposed as `IReadOnlyList`. |

### 1.5 Composition (has-a)

- `Order` has many `OrderItem`, and each `OrderItem` has a `Product` (`Cart/Models/`).
- `Receipt` has a `Discount`, a `Payment` and many `ReceiptLine` (`Payment/Receipt.cs`).
- `CheckoutPanel` has a `PaymentController` (`Payment/CheckoutPanel.cs`).
- `Form2` has a `CartController` (`Dashboard/Form2.cs`).

### 1.6 Other OOP features

| Feature | File | Explanation |
|---|---|---|
| Events (loose coupling) | `Cart/Controllers/CartController.cs` | `OrderUpdated`, `OrderHeld`, `OrderRecalled`, `OrderCleared`, `Notification`. The view subscribes and reacts. |
| Partial classes | `Dashboard/Form2.cs` + `Form2.TransactionLogs.cs` + `Form2.Designer.cs`, and each `*.Designer.cs` | One class split across files. |
| Static utility classes | `Money`, `Theme`, `CoopTheme`, `AppFonts`, `Gfx`, `NumericInputHelper`, `PromptDialog`, `CoopMessageBox`, `CsvExporter`, `JsonLite`, `CoopDatabase` | Helpers that need no object. |
| Records (immutable data) | `Database/CoopDatabase.cs` | `DatabaseUser`, `DatabaseStaff`, `DatabaseProduct`, `DatabaseTransaction`. |
| Enums | `Enums.cs`, `Cart/Models/OrderStatus.cs` | `PaymentMethod`, `ReceiptStatus`, `OrderStatus`. |
| Exception handling | `CoopDatabase.SaveOrder()`, `Program.cs` | `try/catch` with a transaction rollback, plus global error handlers. |

## 2. CRUD

| Entity | Create | Read | Update | Delete |
|---|---|---|---|---|
| **Staff / Users** | `AdminSettings.cs` (add staff modal) calls `CoopDatabase.SaveStaff(null, ...)` | `AdminSettings.LoadStaffData()` calls `CoopDatabase.LoadStaff()`. Login uses `CoopDatabase.TryAuthenticate()` | `AdminSettings.cs` (edit modal) calls `CoopDatabase.SaveStaff(id, ...)` | `AdminSettings.DeleteStaff()` calls `CoopDatabase.DeleteStaff()` (admins cannot be deleted) |
| **Products / Inventory** | `AdminInventory.BtnSaveItem_Click` (new item, **list in memory only**) | `CoopDatabase.LoadInventory()` (admin) and `LoadProducts()` (cashier catalog in `CartController`) | `CoopDatabase.SetStock()` (restock) | `AdminInventory.BtnDeleteItem_Click` (**removes from the list only, not from the database**) |
| **Orders / Transactions** | `CoopDatabase.SaveOrder()` (called from `Form2` at checkout; inserts `Orders` and `OrderItems` and deducts stock in one transaction) | `CoopDatabase.LoadOrders()` and `LoadTransactionRecords()` (`Form2`, `AdminTransactions`, `AdminReports`) | None (receipts are permanent records) | None |
| **Cart (in memory)** | `CartController.AddItem()` | `CartController.CurrentOrder` | `IncrementItem()`, `DecrementItem()`, `SetItemQuantity()` | `RemoveItem()`, `ClearCart()` |

## 3. MVC (Model, View, Controller)

| Layer | Files | Role |
|---|---|---|
| **Model** | `Cart/Models/` (`Product`, `Order`, `OrderItem`, `OrderStatus`, `BarcodeParser`), `Payment/` (`Receipt`, `ReceiptLine`, `Discount` + children, `Payment` + children), `Dashboard/TransactionModels.cs`, `Dashboard/DailySummary.cs` | Data and business rules |
| **View** | `Login/LoginPage.cs`, `Dashboard/Form2*.cs` (cashier screen), `Admin/Admin*.cs` (admin screens), `Cart/Controls/CartItemControl.cs`, `Payment/CheckoutPanel.cs`, `Payment/OrderReceiptForm.cs`, `Payment/ReceiptForm.cs`, `Payment/CustomerInfoForm.cs`, `Dashboard/TransactionsView.cs`, `Dashboard/SummaryForm.cs` | What the user sees and clicks |
| **Controller** | `Cart/Controllers/CartController.cs`, `Payment/PaymentController.cs`, `Dashboard/TransactionController.cs` | Takes actions from the view, updates the models, and raises events back to the view |
| **Data access** | `Database/CoopDatabase.cs` | All SQL lives here |

Flow of a sale: `Form2` (View) calls `CartController` (Controller), which updates `Order` and `OrderItem` (Model). The controller raises `OrderUpdated`, and `Form2` redraws. At checkout, `Form2` calls `CoopDatabase.SaveOrder()` and shows `OrderReceiptForm`.

Note: the Admin forms call `CoopDatabase` directly and have no controller of their own.

## 4. Database (SQL Server, `SQLQuerycoopstore.sql`)

| Table | Primary key (PK) | Foreign key (FK) | Other keys and constraints |
|---|---|---|---|
| `Users` | `UserId` (identity) | None | `Username` UNIQUE, `Role` CHECK (Admin or Staff) |
| `UserArchive` | `UserArchiveId` (identity) | None. `UserId` is only a copy of the deleted user's ID | Holds archived staff |
| `Products` | `ProductId` (identity) | None | `Sku` UNIQUE, `Category` CHECK (Books or Merchandise), `Price >= 0`, `StockQuantity >= 0` |
| `Archive` | `ArchiveId` (identity) | None. `ProductId` is only a copy | Filled by the trigger on `Products` |
| `Orders` | `OrderId` (NVARCHAR) | None | `PaymentMethod` CHECK (Cash or GCash) |
| `OrderItems` | `OrderItemId` (identity) | **`OrderId` references `Orders(OrderId)` ON DELETE CASCADE** | `Quantity > 0`, `LineTotal` is a computed column (`UnitPrice * Quantity`) |

Relationships:

- `Orders` (parent, 1) to `OrderItems` (child, many). This is the only true foreign key in the database. Deleting an order deletes its items.
- `OrderItems.ProductId` points to `Products.ProductId` logically, but it is **not** declared as a foreign key. It is nullable so old orders survive when a product is deleted.
- `Users` and `Products` are standalone parent tables, and their archive tables are copies.

Triggers and procedures:

- `trg_Products_ArchiveOnDelete` runs after a delete on `Products` and copies the deleted row into `Archive`.
- `usp_ArchiveStaff` copies a Staff user into `UserArchive`, then deletes it from `Users`, in one transaction. Admins cannot be archived.
- `usp_DeleteArchivedUser` removes a row from `UserArchive`.

Seed data: default accounts `admin`, `staff1` and `staff2`, and 90 products (SKU `UM-001` to `UM-090`).

### Known gaps

- The C# code (`CoopDatabase.LoadStaff/SaveStaff`) uses a `Users.FullName` column that is **not in** `SQLQuerycoopstore.sql`. Add `FullName NVARCHAR(100) NULL` to `Users` or staff pages will fail on a fresh database.
- The `Payment` module classes `Receipt`, `ReceiptStore`, `TransactionController` and `IStockService` are not yet wired into `Form2`. The live checkout uses `CartController` and `CoopDatabase.SaveOrder()`.
- `usp_ArchiveStaff` and the `Archive` and `UserArchive` tables exist in SQL, but the C# code does not call them yet. `DeleteStaff` runs a plain `DELETE`.
- Passwords are stored as plain text.