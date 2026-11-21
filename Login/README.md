# Module 6: Checkout & Transactions (Windows Forms, C#)

## 1. Add to your Visual Studio project
1. Copy the Models, Controllers, Services and Views folders into your project folder, then in Solution Explorer click "Show All Files", right-click each folder > Include In Project.
2. No extra references are needed. The JSON save uses a small built-in reader/writer (`JsonLite`), so it works as-is on .NET Framework 4.8 and .NET 6/8.
3. Namespace: files use `UMakCoop`. Use Ctrl+Shift+H (Replace in Files) to change `UMakCoop` to your project's namespace.
4. The code avoids newer C# features, so it works on .NET Framework 4.8 and on .NET 6/8. If you see nullable warnings on .NET 6+, they are harmless.
5. To test alone: in Program.cs use `Application.Run(new UMakCoop.Views.DemoForm());`. Delete DemoForm.cs and Services/InMemoryStockService.cs before the final merge.

## 2. Touch points with the other modules
| Who | What |
|---|---|
| Module 5 (Cart) | Each time the cart changes: `checkoutPanel.SetOrder(order.Items.Select(i => new ReceiptLine(i.ProductId, i.ProductName, i.Size, i.UnitPrice, i.Quantity)).ToList(), student?.Name);` In `checkoutPanel.SaleCompleted`: clear the cart and set order status to Completed. |
| Module 3 (Products) | Make ProductController (or inventory class) implement `IStockService`: HasStock, Deduct, Restore. Refresh product cards in `SaleCompleted`. Low-stock Notification can be raised there too. |
| Module 2 (Login) | After login: `checkoutPanel.CashierName = user.Name; transactionsView.SetUser(user.Name, user is Admin);` |
| MainForm | Create ONE shared `TransactionController(stockService)`. It already saves to disk and loads on start. (`TransactionController.CreateInMemory(stockService)` keeps everything in memory only; `new TransactionController(stockService, new ReceiptStore(path))` saves to another file.) Then `checkoutPanel.Transactions = controller; transactionsView.Attach(controller, user.Name, isAdmin);` |

Layout: put `CheckoutPanel` at the bottom of the Order Scroll panel (Dock = Bottom). Put `TransactionsView` (Dock = Fill) on the Transaction Logs page.
The views are built in code (no .Designer.cs), so adjust them in the .cs files, not in the Designer.

## 3. Class map
Models: PaymentMethod, ReceiptStatus (enums), Payment (abstract), CashPayment, GCashPayment, Discount (abstract), NoDiscount, SeniorDiscount, PwdDiscount, ReceiptLine, Receipt
Models (extra): DailySummary, DiscountSummaryLine
Controllers: PaymentController, TransactionController
Services: Money, IStockService (+ InMemoryStockService for testing), ReceiptStore (JSON save/load, uses JsonLite), CsvExporter
Views: CheckoutPanel, ReceiptForm (+ ReceiptPaper, StampControl), TransactionsView, SummaryForm, PromptDialog, Theme, DemoForm

## 4. Rules built in (for your defense / documentation)
- Money uses `decimal` and rounds discounts to 2 decimals (away from zero).
- Only two discounts: Senior Citizen 20% and PWD 20%. Both need the ID number typed in at checkout (`Discount.RequiresIdNumber`); the number is printed on the receipt.
- Cash must cover the total, has max 2 decimals, and is capped at 100,000 to catch typos.
- GCash needs a 13-digit reference, and the same reference cannot be used for two sales.
- Stock for ALL items is checked before any is deducted, so a failed sale never leaves half-deducted stock.
- Receipts are snapshots (prices frozen at the time of sale). Receipt numbers look like UC-20261121-0001 and restart each day.
- Void is Admin only, needs a reason, returns the items to stock, and keeps who/when/why. Voided receipts stay in the list.
- The receipt is drawn like a bill pad (ReceiptPaper): Sold to / Receipt No. fields, No. / Description / Pcs. / Rate / Amount table, payment details, totals box, signature lines. Re-opening an old receipt shows "*** REPRINT ***". Voided receipts show "*** VOIDED ***" and a VOID stamp.
- Seal button disables itself on click to block double sales.
- Saving: every sale and void is written to `%AppData%\UMakCoop\transactions.json` (readable JSON, saved through a temp file so a crash cannot wipe it) and loaded when the app starts. Receipt numbers continue from the last one of the day, and a GCash reference used before the restart is still blocked. If the file is locked or the disk is full, the sale still goes through and the cashier sees a warning (`TransactionController.StorageWarning`). A damaged file is renamed to `transactions.json.corrupt-<time>` instead of being overwritten.
- Search (Transactions) matches receipt no, student, cashier, Senior/PWD ID number, discount name and GCash reference.
- Export CSV saves exactly what the list shows (search, status and Today filters apply), one row per receipt, opens in Excel; text that starts with = + - @ is defused so it cannot run as a formula.
- Daily summary: pick a date to see completed orders, items, voids, gross, discounts, net, Cash vs GCash, and discounts given per type. Counted by the date of the sale.
- Stock is NOT saved by this module. Saved receipts only restore the transaction list; the product stock must be saved by Module 3.
- VAT is not included: it only applies if the coop is VAT-registered. Ask your adviser before adding it.
