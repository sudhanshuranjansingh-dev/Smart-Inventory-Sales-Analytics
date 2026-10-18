Imports System.IO
Imports System.Text.Json
Imports Microsoft.Web.WebView2.Core
Imports MySql.Data.MySqlClient
Imports Windows.Win32.System

Public Class FrmMain

    Private Sub KeepMainFormFullScreen()

        Me.FormBorderStyle = FormBorderStyle.None
        Me.WindowState = FormWindowState.Maximized
        Me.StartPosition = FormStartPosition.CenterScreen

    End Sub

    Public Sub RefreshUserSession()

        If Session.CurrentUserID <= 0 OrElse
       String.IsNullOrWhiteSpace(Session.CurrentRole) OrElse
       String.IsNullOrWhiteSpace(Session.CurrentFullName) Then

            Return
        End If

        Try
            ' Request the current user through the existing WebView2 bridge.
            If WebView21 Is Nothing OrElse
           WebView21.CoreWebView2 Is Nothing Then

                Return
            End If

            WebView21.CoreWebView2.PostWebMessageAsJson(
            System.Text.Json.JsonSerializer.Serialize(
                New With {
                    .type = "currentUser",
                    .role = Session.CurrentRole,
                    .fullName = Session.CurrentFullName,
                    .username = Session.CurrentUsername
                }
            )
        )

        Catch ex As Exception
            MessageBox.Show(
            "Unable to refresh user session: " & ex.Message,
            "Session Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )
        End Try

    End Sub


    '=========================================================
    ' FORM LOAD
    '=========================================================
    Private Async Sub FrmMain_Load(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Load

        Me.FormBorderStyle = FormBorderStyle.Sizable

        Me.MinimizeBox = True
        Me.MaximizeBox = True
        Me.ControlBox = True

        Me.Text = "Smart Inventory"

        Try




            '-------------------------------------------------
            ' Initialize WebView2
            '-------------------------------------------------
            Await WebView21.EnsureCoreWebView2Async()


            '-------------------------------------------------
            ' Register WebView2 message handler
            '-------------------------------------------------
            RemoveHandler WebView21.CoreWebView2.WebMessageReceived,
                AddressOf WebMessageReceived

            AddHandler WebView21.CoreWebView2.WebMessageReceived,
                AddressOf WebMessageReceived


            '-------------------------------------------------
            ' Locate WebUI folder
            '-------------------------------------------------
            Dim webUIPath As String =
                Path.Combine(
                    Application.StartupPath,
                    "WebUI"
                )


            '-------------------------------------------------
            ' Locate index.html
            '-------------------------------------------------
            Dim indexPath As String =
                Path.Combine(
                    webUIPath,
                    "index.html"
                )


            '-------------------------------------------------
            ' Check index.html
            '-------------------------------------------------
            If Not File.Exists(indexPath) Then

                MessageBox.Show(
                    "index.html was not found." &
                    Environment.NewLine &
                    Environment.NewLine &
                    indexPath,
                    "WebUI Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                )

                Return

            End If


            '-------------------------------------------------
            ' Load Web UI
            '-------------------------------------------------
            WebView21.Source =
                New Uri(indexPath)


        Catch ex As Exception

            MessageBox.Show(
                "Unable to load Smart Inventory." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Application Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '=========================================================
    ' JAVASCRIPT -> VB.NET MESSAGE
    '=========================================================
    Private Sub WebMessageReceived(
    sender As Object,
    e As CoreWebView2WebMessageReceivedEventArgs
)
        Try
            Dim json As String = e.WebMessageAsJson

            Using document As JsonDocument = JsonDocument.Parse(json)
                Dim root As JsonElement = document.RootElement

                ' Handle string messages
                If root.ValueKind = JsonValueKind.String Then
                    Dim stringAction As String = root.GetString()

                    If String.Equals(stringAction, "logout",
                                 StringComparison.OrdinalIgnoreCase) Then
                        LogoutFromMain()
                    Else
                        Debug.WriteLine("Unknown string message: " & stringAction)
                    End If

                    Return
                End If

                ' Only object messages are supported beyond this point
                If root.ValueKind <> JsonValueKind.Object Then
                    Debug.WriteLine("Ignored non-object WebUI message.")
                    Return
                End If

                ' Read action
                Dim actionElement As JsonElement

                If Not root.TryGetProperty("action", actionElement) OrElse
               actionElement.ValueKind <> JsonValueKind.String Then
                    Debug.WriteLine("WebUI message has no valid action: " & json)
                    Return
                End If

                Dim action As String = actionElement.GetString()

                Debug.WriteLine("WebUI action received: " & action)
                Debug.WriteLine("Session user ID: " & Session.CurrentUserID.ToString())
                Debug.WriteLine("Session role: " & If(Session.CurrentRole, "(empty)"))

                ' Validate session for protected actions
                If action <> "getCurrentUser" AndAlso
               action <> "logout" AndAlso
               (Session.CurrentUserID <= 0 OrElse
                String.IsNullOrWhiteSpace(Session.CurrentRole)) Then

                    SendToWeb(New With {
                    .type = "sessionInvalid",
                    .message = "Your session is no longer active. Please log in again."
                })

                    Debug.WriteLine("Rejected action because session is invalid: " & action)
                    Return
                End If

                ' Staff permission check
                If IsStaffUser() AndAlso IsStaffRestrictedAction(action) Then
                    SendToWeb(New With {
                    .type = "accessDenied",
                    .message = "You do not have permission to access this feature."
                })

                    Debug.WriteLine("Access denied for staff action: " & action)
                    Return
                End If

                ' Process actions
                Select Case action

                    Case "getCurrentUser"
                        Debug.WriteLine("GET CURRENT USER requested")
                        Debug.WriteLine("Session.CurrentUserID: " &
                    Session.CurrentUserID.ToString())
                        Debug.WriteLine("Session.CurrentRole: " &
                    If(Session.CurrentRole, "(empty)"))

                        SendCurrentUserToWeb()

                    Case "loadDashboard"
                        LoadDashboardToWeb()

                    Case "loadProducts"
                        LoadProductsToWeb()

                    Case "loadCategories"
                        LoadCategoriesToWeb()

                    Case "addCategory"
                        AddCategory(root)

                    Case "deleteCategory"
                        DeleteCategory(root)

                    Case "loadSuppliers"
                        LoadSuppliersToWeb()

                    Case "addSupplier"
                        AddSupplier(root)

                    Case "loadPurchases"
                        Debug.WriteLine("Staff/Role requested: loadPurchases")
                        LoadPurchasesToWeb()

                    Case "addPurchase"
                        Debug.WriteLine("Staff/Role requested: addPurchase")
                        AddPurchase(root)

                    Case "loadSales"
                        Debug.WriteLine("TEST 1: loadSales received")
                        LoadSalesToWeb()

                    Case "loadSalesHistory"
                        Debug.WriteLine("TEST 2: loadSalesHistory received")
                        LoadSalesHistoryToWeb()
                        Debug.WriteLine("TEST 3: LoadSalesHistoryToWeb returned")

                    Case "addSale"
                        Debug.WriteLine("Staff/Role requested: addSale")
                        AddSale(root)

                    Case "addProduct"
                        AddProduct(root)

                    Case "loadProductsBySupplier"
                        LoadProductsBySupplier(root)

                    Case "updateProduct"
                        UpdateProduct(root)

                    Case "deleteProduct"
                        DeleteProduct(root)

                    Case "loadInventory"
                        LoadInventoryToWeb()

                    Case "loadProductAnalysis"
                        LoadProductAnalysisToWeb()

                    Case "loadSalesAnalytics"
                        Debug.WriteLine("=== SALES ANALYTICS REQUEST RECEIVED ===")
                        Debug.WriteLine("Session User ID: " & Session.CurrentUserID.ToString())
                        Debug.WriteLine("Session Role: " & If(Session.CurrentRole, "(empty)"))

                        LoadSalesAnalyticsToWeb(root)

                        Debug.WriteLine("=== SALES ANALYTICS REQUEST COMPLETED ===")

                    Case "loadReports"
                        LoadReportsToWeb(root)

                    Case "loadPendingAdmins"
                        LoadPendingAdminsToWeb()

                    Case "approveAdmin"
                        ApproveAdmin(root)

                    Case "rejectAdmin"
                        RejectAdmin(root)

                    ' User Management
                    Case "loadUsers"
                        LoadUsersToWeb()

                    Case "updateUser"
                        UpdateUser(root)

                    Case "deleteUser"
                        DeleteUser(root)

                    Case "logout"
                        LogoutFromMain()

                    Case "logoutConfirmed"
                        ' Logout was already handled by LogoutFromMain().
                        ' Do not initiate logout a second time.
                        Debug.WriteLine("Ignoring redundant logoutConfirmed action.")

                    Case Else
                        Debug.WriteLine("Unknown WebUI action: " & action)

                        SendToWeb(New With {
                        .type = "webUIError",
                        .message = "Unsupported action: " & action
                    })

                End Select
            End Using

        Catch ex As Exception
            Debug.WriteLine("WebUI communication error: " & ex.ToString())

            MessageBox.Show(
            "WebUI communication error:" & Environment.NewLine &
            ex.Message,
            "WebUI Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )
        End Try
    End Sub

    '=========================================================
    ' LOAD SALES
    '=========================================================



    Private Sub LoadSalesToWeb()

        Try
            Dim sales As New List(Of Dictionary(Of String, Object))()

            Using con As MySqlConnection = DBConnection.GetConnection()
                con.Open()

                Dim query As String =
                "SELECT " &
                "s.SaleID, " &
                "s.CustomerName, " &
                "s.CustomerPhone, " &
                "GROUP_CONCAT(DISTINCT p.ProductName ORDER BY p.ProductName SEPARATOR ', ') AS ProductName, " &
               "GROUP_CONCAT(DISTINCT p.ProductCode ORDER BY p.ProductCode SEPARATOR ', ') AS ProductCode, " &
                "SUM(sd.Quantity) AS Quantity, " &
                "CASE WHEN SUM(sd.Quantity) > 0 " &
                "THEN SUM(sd.TotalPrice) / SUM(sd.Quantity) ELSE 0 END AS SellingPrice, " &
                "SUM(sd.TotalPrice) AS TotalAmount, " &
                "s.SaleDate " &
                "FROM Sales s " &
                "INNER JOIN SaleDetails sd ON s.SaleID = sd.SaleID " &
                "INNER JOIN Products p ON sd.ProductID = p.ProductID " &
                "GROUP BY s.SaleID, s.CustomerName, s.CustomerPhone, s.SaleDate " &
                "ORDER BY s.SaleDate DESC, s.SaleID DESC " &
                "LIMIT 10"

                Using cmd As New MySqlCommand(query, con)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        While reader.Read()

                            sales.Add(New Dictionary(Of String, Object) From {
                            {"SaleID", Convert.ToInt32(reader("SaleID"))},
                            {"ProductCode", Convert.ToString(reader("ProductCode"))},
                            {"ProductName", Convert.ToString(reader("ProductName"))},
                            {"CustomerName", If(
                                reader.IsDBNull(reader.GetOrdinal("CustomerName")),
                                "",
                                Convert.ToString(reader("CustomerName"))
                            )},
                            {"CustomerPhone", If(
                                reader.IsDBNull(reader.GetOrdinal("CustomerPhone")),
                                "",
                                Convert.ToString(reader("CustomerPhone"))
                            )},
                            {"Quantity", Convert.ToInt32(reader("Quantity"))},
                            {"SellingPrice", Convert.ToDecimal(reader("SellingPrice"))},
                            {"TotalAmount", Convert.ToDecimal(reader("TotalAmount"))},
                            {"SaleDate", Convert.ToDateTime(reader("SaleDate")).ToString("yyyy-MM-dd HH:mm")}
                        })

                        End While

                    End Using
                End Using
            End Using

            Debug.WriteLine("Sales records loaded: " & sales.Count.ToString())

            SendToWeb(New With {
            .type = "sales",
            .data = sales
        })

        Catch ex As Exception

            Debug.WriteLine("LoadSalesToWeb error: " & ex.ToString())

            SendToWeb(New With {
            .type = "salesError",
            .message = "Unable to load recent sales: " & ex.Message
        })

        End Try

    End Sub







    Private Sub LoadSalesHistoryToWeb()

        Debug.WriteLine("HISTORY 1: Method started")

        Try
            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()
                Debug.WriteLine("HISTORY 2: Database connected")


                Dim query As String =
                        "SELECT " &
                        "s.SaleID, " &
                        "s.CustomerName, " &
                        "s.CustomerPhone, " &
                        "GROUP_CONCAT(DISTINCT p.ProductCode ORDER BY p.ProductCode SEPARATOR ', ') AS ProductCode, " &
                        "GROUP_CONCAT(DISTINCT p.ProductName ORDER BY p.ProductName SEPARATOR ', ') AS ProductName, " &
                        "SUM(sd.Quantity) AS TotalItems, " &
                        "CASE WHEN SUM(sd.Quantity) > 0 " &
                        "THEN SUM(sd.TotalPrice) / SUM(sd.Quantity) ELSE 0 END AS SellingPrice, " &
                        "SUM(sd.TotalPrice) AS TotalAmount, " &
                        "s.SaleDate " &
                        "FROM Sales s " &
                        "INNER JOIN SaleDetails sd ON s.SaleID = sd.SaleID " &
                        "INNER JOIN Products p ON sd.ProductID = p.ProductID " &
                        "GROUP BY s.SaleID, s.CustomerName, s.CustomerPhone, s.SaleDate " &
                        "ORDER BY s.SaleDate DESC, s.SaleID DESC"


                Using cmd As New MySqlCommand(query, con)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        Dim sales As New List(Of Dictionary(Of String, Object))()

                        While reader.Read()


                            sales.Add(New Dictionary(Of String, Object) From {
                                {"CustomerName", If(reader.IsDBNull(reader.GetOrdinal("CustomerName")), "", Convert.ToString(reader("CustomerName")))},
                                {"CustomerPhone", If(reader.IsDBNull(reader.GetOrdinal("CustomerPhone")), "", Convert.ToString(reader("CustomerPhone")))},
                                {"ProductCode", Convert.ToString(reader("ProductCode"))},
                                {"ProductName", Convert.ToString(reader("ProductName"))},
                                {"TotalItems", Convert.ToInt32(reader("TotalItems"))},
                                {"Quantity", Convert.ToInt32(reader("TotalItems"))},
                                {"SellingPrice", Convert.ToDecimal(reader("SellingPrice"))},
                                {"TotalAmount", Convert.ToDecimal(reader("TotalAmount"))},
                                {"SaleDate", Convert.ToDateTime(reader("SaleDate")).ToString("yyyy-MM-dd HH:mm:ss")}
                            })


                        End While

                        Debug.WriteLine("HISTORY 3: Rows loaded = " & sales.Count.ToString())

                        SendToWeb(New With {
                        .type = "salesHistory",
                        .data = sales
                    })

                        Debug.WriteLine("HISTORY 4: Response sent to SendToWeb")

                    End Using
                End Using
            End Using

        Catch ex As Exception

            Debug.WriteLine("HISTORY ERROR: " & ex.ToString())

            SendToWeb(New With {
            .type = "salesHistoryError",
            .message = ex.Message
        })

        End Try

    End Sub







    Private Sub AddPurchase(root As JsonElement)

        Dim transaction As MySqlTransaction = Nothing
        Dim purchaseID As Integer = 0
        Dim totalPrice As Decimal = 0D

        Try
            ' Read values sent by JavaScript
            Dim supplierID As Integer =
            GetIntegerValue(root, "SupplierID")

            Dim productID As Integer =
            GetIntegerValue(root, "ProductID")

            Dim quantity As Integer =
            GetIntegerValue(root, "Quantity")

            ' Validate input
            If supplierID <= 0 Then
                SendToWeb(New With {
                .type = "purchaseError",
                .message = "Please select a supplier."
            })
                Return
            End If

            If productID <= 0 Then
                SendToWeb(New With {
                .type = "purchaseError",
                .message = "Please select a product."
            })
                Return
            End If

            If quantity <= 0 Then
                SendToWeb(New With {
                .type = "purchaseError",
                .message = "Quantity must be greater than zero."
            })
                Return
            End If

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()
                transaction = con.BeginTransaction()

                ' 1. Verify that the supplier exists
                Dim supplierQuery As String =
                "SELECT COUNT(*) FROM Suppliers " &
                "WHERE SupplierID = @SupplierID"

                Using cmd As New MySqlCommand(
                supplierQuery, con, transaction)

                    cmd.Parameters.AddWithValue(
                    "@SupplierID", supplierID)

                    Dim supplierExists As Integer =
                    Convert.ToInt32(cmd.ExecuteScalar())

                    If supplierExists = 0 Then
                        Throw New Exception(
                        "Selected supplier was not found.")
                    End If
                End Using

                ' 2. Verify that the product belongs to this supplier
                ' Lock the product row while updating stock
                Dim previousStock As Integer = 0
                Dim purchasePrice As Decimal = 0D

                Dim productQuery As String =
                "SELECT StockQuantity, PurchasePrice " &
                "FROM Products " &
                "WHERE ProductID = @ProductID " &
                "AND SupplierID = @SupplierID " &
                "FOR UPDATE"

                Using cmd As New MySqlCommand(
                productQuery, con, transaction)

                    cmd.Parameters.AddWithValue(
                    "@ProductID", productID)

                    cmd.Parameters.AddWithValue(
                    "@SupplierID", supplierID)

                    Using reader As MySqlDataReader =
                    cmd.ExecuteReader()

                        If Not reader.Read() Then
                            Throw New Exception(
                            "The selected product does not belong " &
                            "to this supplier, or the product was not found.")
                        End If

                        previousStock =
                        Convert.ToInt32(reader("StockQuantity"))

                        purchasePrice =
                        Convert.ToDecimal(reader("PurchasePrice"))
                    End Using
                End Using

                If purchasePrice <= 0D Then
                    Throw New Exception(
                    "The selected product has an invalid purchase price.")
                End If

                ' 3. Calculate purchase amount and new stock

                totalPrice = Decimal.Round(
                    quantity * purchasePrice,
                    2,
                    MidpointRounding.AwayFromZero)

                Dim newStock As Integer =
                previousStock + quantity

                ' 4. Insert purchase header
                Dim purchaseQuery As String =
                "INSERT INTO Purchases " &
                "(SupplierID, PurchaseDate, TotalAmount) " &
                "VALUES (@SupplierID, NOW(), @TotalAmount)"

                Using cmd As New MySqlCommand(
                purchaseQuery, con, transaction)

                    cmd.Parameters.AddWithValue(
                    "@SupplierID", supplierID)

                    cmd.Parameters.AddWithValue(
                    "@TotalAmount", totalPrice)

                    cmd.ExecuteNonQuery()

                    purchaseID = Convert.ToInt32(cmd.LastInsertedId)
                End Using

                ' 5. Insert purchase details
                Dim detailQuery As String =
                "INSERT INTO PurchaseDetails " &
                "(PurchaseID, ProductID, Quantity, " &
                "PurchasePrice, TotalPrice) " &
                "VALUES (@PurchaseID, @ProductID, @Quantity, " &
                "@PurchasePrice, @TotalPrice)"

                Using cmd As New MySqlCommand(
                detailQuery, con, transaction)

                    cmd.Parameters.AddWithValue(
                    "@PurchaseID", purchaseID)

                    cmd.Parameters.AddWithValue(
                    "@ProductID", productID)

                    cmd.Parameters.AddWithValue(
                    "@Quantity", quantity)

                    cmd.Parameters.AddWithValue(
                    "@PurchasePrice", purchasePrice)

                    cmd.Parameters.AddWithValue(
                    "@TotalPrice", totalPrice)

                    cmd.ExecuteNonQuery()
                End Using

                ' 6. Update product stock
                Dim stockQuery As String =
                "UPDATE Products SET " &
                "StockQuantity = @NewStock " &
                "WHERE ProductID = @ProductID " &
                "AND SupplierID = @SupplierID"

                Using cmd As New MySqlCommand(
                stockQuery, con, transaction)

                    cmd.Parameters.AddWithValue(
                    "@NewStock", newStock)

                    cmd.Parameters.AddWithValue(
                    "@ProductID", productID)

                    cmd.Parameters.AddWithValue(
                    "@SupplierID", supplierID)

                    Dim rowsAffected As Integer =
                    cmd.ExecuteNonQuery()

                    If rowsAffected = 0 Then
                        Throw New Exception(
                        "Product stock could not be updated.")
                    End If
                End Using

                ' 7. Record stock history
                Dim historyQuery As String =
                "INSERT INTO StockHistory " &
                "(ProductID, ChangeType, Quantity, " &
                "PreviousStock, NewStock, ChangeDate) " &
                "VALUES (@ProductID, @ChangeType, @Quantity, " &
                "@PreviousStock, @NewStock, NOW())"

                Using cmd As New MySqlCommand(
                historyQuery, con, transaction)

                    cmd.Parameters.AddWithValue(
                    "@ProductID", productID)

                    cmd.Parameters.AddWithValue(
                    "@ChangeType", "PURCHASE")

                    cmd.Parameters.AddWithValue(
                    "@Quantity", quantity)

                    cmd.Parameters.AddWithValue(
                    "@PreviousStock", previousStock)

                    cmd.Parameters.AddWithValue(
                    "@NewStock", newStock)

                    cmd.ExecuteNonQuery()
                End Using

                ' 8. Commit all database changes
                transaction.Commit()
                transaction = Nothing

            End Using

            ' 9. Notify JavaScript that the purchase succeeded
            SendToWeb(New With {
                    .type = "purchaseAdded",
                    .message = "Purchase added successfully.",
                    .purchaseID = purchaseID,
                    .totalAmount = totalPrice
                })
            ' Refresh purchase history and product/supplier data
            LoadPurchasesToWeb()
            LoadProductsToWeb()
            LoadSuppliersToWeb()

        Catch ex As Exception

            ' Roll back if any database operation failed
            If transaction IsNot Nothing Then
                Try
                    transaction.Rollback()
                Catch rollbackEx As Exception
                    Debug.WriteLine(
                    "Purchase rollback error: " & rollbackEx.Message)
                End Try
            End If

            SendToWeb(New With {
            .type = "purchaseError",
            .message = "Unable to add purchase: " & ex.Message
        })

            Debug.WriteLine(
            "AddPurchase error: " & ex.ToString())

        Finally
            If transaction IsNot Nothing Then
                transaction.Dispose()
            End If
        End Try

    End Sub


    '=============================================
    ' Products By Supplier
    '=====================================

    Private Sub LoadProductsBySupplier(supplierID As Integer)

        Try
            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                "SELECT ProductID, ProductName " &
                "FROM Products " &
                "WHERE SupplierID = @SupplierID " &
                "ORDER BY ProductName"

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue("@SupplierID", supplierID)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        Dim products As New List(Of Dictionary(Of String, Object))

                        While reader.Read()
                            products.Add(New Dictionary(Of String, Object) From {
                            {"ProductID", reader("ProductID")},
                            {"ProductName", reader("ProductName")}
                        })
                        End While

                        SendToWeb(New With {
                        .type = "supplierProducts",
                        .data = products
                    })

                    End Using
                End Using
            End Using

        Catch ex As Exception
            SendToWeb(New With {
            .type = "supplierProductsError",
            .message = ex.Message
        })
        End Try

    End Sub



    '=========================================================
    ' LOAD DASHBOARD DATA TO WEB UI
    '=========================================================
    Private Sub LoadDashboardToWeb()

        Try

            Using conn As MySqlConnection =
            DBConnection.GetConnection()

                conn.Open()


                '=================================================
                ' SUMMARY VALUES
                '=================================================

                Dim totalProducts As Integer = 0

                Dim totalSales As Decimal = 0D

                Dim lowStock As Integer = 0

                Dim totalUnits As Integer = 0

                Dim inStock As Integer = 0

                Dim lowStockItems As Integer = 0

                Dim outOfStock As Integer = 0


                '=================================================
                ' CATEGORY-WISE PRODUCT DATA
                '=================================================

                Dim categoryNames As New List(Of String)
                Dim categoryCounts As New List(Of Integer)

                '=================================================
                ' TOTAL PRODUCTS
                '=================================================

                Dim productQuery As String =
                "SELECT COUNT(*) " &
                "FROM Products"


                Using cmd As New MySqlCommand(
                productQuery,
                conn
            )

                    totalProducts =
                    Convert.ToInt32(
                        cmd.ExecuteScalar()
                    )

                End Using


                '=================================================
                ' TOTAL SALES
                '=================================================

                Dim salesQuery As String =
                "SELECT COALESCE(SUM(TotalAmount), 0) " &
                "FROM Sales"


                Using cmd As New MySqlCommand(
                salesQuery,
                conn
            )

                    totalSales =
                    Convert.ToDecimal(
                        cmd.ExecuteScalar()
                    )

                End Using


                '=================================================
                ' TOTAL UNITS
                '=================================================

                Dim unitsQuery As String =
                "SELECT COALESCE(SUM(StockQuantity), 0) " &
                "FROM Products"


                Using cmd As New MySqlCommand(
                unitsQuery,
                conn
            )

                    totalUnits =
                    Convert.ToInt32(
                        cmd.ExecuteScalar()
                    )

                End Using


                '=================================================
                ' LOW STOCK
                '
                ' Stock > 0 AND Stock <= MinimumStock
                '=================================================

                Dim lowStockQuery As String =
                "SELECT COUNT(*) " &
                "FROM Products " &
                "WHERE StockQuantity > 0 " &
                "AND StockQuantity <= MinimumStock"


                Using cmd As New MySqlCommand(
                lowStockQuery,
                conn
            )

                    lowStock =
                    Convert.ToInt32(
                        cmd.ExecuteScalar()
                    )

                End Using


                lowStockItems =
                lowStock


                '=================================================
                ' IN STOCK
                '
                ' Stock greater than MinimumStock
                '=================================================

                Dim inStockQuery As String =
                "SELECT COUNT(*) " &
                "FROM Products " &
                "WHERE StockQuantity > MinimumStock"


                Using cmd As New MySqlCommand(
                inStockQuery,
                conn
            )

                    inStock =
                    Convert.ToInt32(
                        cmd.ExecuteScalar()
                    )

                End Using


                '=================================================
                ' OUT OF STOCK
                '=================================================

                Dim outOfStockQuery As String =
                "SELECT COUNT(*) " &
                "FROM Products " &
                "WHERE StockQuantity <= 0"


                Using cmd As New MySqlCommand(
                outOfStockQuery,
                conn
            )

                    outOfStock =
                    Convert.ToInt32(
                        cmd.ExecuteScalar()
                    )

                End Using


                '=================================================
                ' MONTHLY SALES
                ' CURRENT YEAR
                '=================================================

                Dim months As New List(Of String)

                Dim monthlySales As New List(Of Decimal)


                For monthNumber As Integer = 1 To 12

                    months.Add(
                    Globalization.CultureInfo.CurrentCulture.
                    DateTimeFormat.
                    GetAbbreviatedMonthName(
                        monthNumber
                    )
                )

                    monthlySales.Add(0D)

                Next


                Dim monthlyQuery As String =
                "SELECT " &
                "MONTH(SaleDate) AS SaleMonth, " &
                "COALESCE(SUM(TotalAmount), 0) AS Amount " &
                "FROM Sales " &
                "WHERE YEAR(SaleDate) = @Year " &
                "GROUP BY MONTH(SaleDate) " &
                "ORDER BY MONTH(SaleDate)"


                Using cmd As New MySqlCommand(
                monthlyQuery,
                conn
            )

                    cmd.Parameters.AddWithValue(
                    "@Year",
                    Date.Today.Year
                )


                    Using reader As MySqlDataReader =
                    cmd.ExecuteReader()

                        While reader.Read()

                            Dim monthNumber As Integer =
                            Convert.ToInt32(
                                reader("SaleMonth")
                            )


                            Dim amount As Decimal =
                            Convert.ToDecimal(
                                reader("Amount")
                            )


                            If monthNumber >= 1 AndAlso
                           monthNumber <= 12 Then

                                monthlySales(
                                monthNumber - 1
                            ) = amount

                            End If

                        End While

                    End Using

                End Using



                '=================================================
                ' CATEGORY-WISE PRODUCT COUNTS
                '=================================================

                Dim categoryQuery As String =
                    "SELECT c.CategoryName, COUNT(p.ProductID) AS ProductCount " &
                    "FROM Categories c " &
                    "LEFT JOIN Products p ON c.CategoryID = p.CategoryID " &
                    "GROUP BY c.CategoryID, c.CategoryName " &
                    "HAVING COUNT(p.ProductID) > 0 " &
                    "ORDER BY c.CategoryName"

                Using cmd As New MySqlCommand(categoryQuery, conn)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        While reader.Read()

                            categoryNames.Add(
                Convert.ToString(reader("CategoryName"))
            )

                            categoryCounts.Add(
                Convert.ToInt32(reader("ProductCount"))
            )

                        End While

                    End Using

                End Using


                '=================================================
                ' BUILD DASHBOARD RESPONSE
                '=================================================

                Dim dashboardData =
                      New With {
                         .type = "dashboard",
                         .totalProducts = totalProducts,
                         .totalSales = totalSales,
                         .lowStock = lowStock,
                         .totalUnits = totalUnits,
                         .inStock = inStock,
                         .lowStockItems = lowStockItems,
                         .outOfStock = outOfStock,
                         .categoryNames = categoryNames,
                         .categoryCounts = categoryCounts,
                         .months = months,
                         .sales = monthlySales
                     }


                '=================================================
                ' SEND DASHBOARD DATA TO WEB UI
                '=================================================

                SendToWeb(dashboardData)


            End Using


        Catch ex As Exception

            '=================================================
            ' SEND ERROR TO WEB UI
            '=================================================

            Dim dashboardError =
    New With {
        .type = "dashboardError",
        .message = "Unable to load dashboard: " & ex.Message
    }

            SendToWeb(dashboardError)


            MessageBox.Show(
            "Unable to load dashboard:" &
            Environment.NewLine &
            Environment.NewLine &
            ex.Message,
            "Dashboard Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

        End Try

    End Sub


    '=========================================================
    ' ADD SALE
    '=========================================================

    Private Sub AddSale(root As JsonElement)

        Dim transaction As MySqlTransaction = Nothing
        Dim saleID As Integer = 0

        Try

            '-------------------------------------------------
            ' GET VALUES FROM WEB UI
            '-------------------------------------------------
            Dim customerName As String =
            GetStringValue(root, "CustomerName").Trim()

            Dim customerPhone As String =
            GetStringValue(root, "CustomerPhone").Trim()

            Dim productID As Integer =
            GetIntegerValue(root, "ProductID")

            Dim quantity As Integer =
            GetIntegerValue(root, "Quantity")

            Dim sellingPrice As Decimal =
            GetDecimalValue(root, "SellingPrice")

            '-------------------------------------------------
            ' VALIDATE CUSTOMER DETAILS
            '-------------------------------------------------
            If String.IsNullOrWhiteSpace(customerName) Then

                SendToWeb(New With {
                .type = "saleError",
                .message = "Customer name is required."
            })

                Return

            End If

            If String.IsNullOrWhiteSpace(customerPhone) OrElse
           Not System.Text.RegularExpressions.Regex.IsMatch(
               customerPhone, "^[0-9]{10}$") Then

                SendToWeb(New With {
                .type = "saleError",
                .message = "Enter a valid 10-digit customer phone number."
            })

                Return

            End If

            '-------------------------------------------------
            ' VALIDATE SALE DETAILS
            '-------------------------------------------------
            If productID <= 0 Then

                SendToWeb(New With {
                .type = "saleError",
                .message = "Please select a product."
            })

                Return

            End If

            If quantity <= 0 Then

                SendToWeb(New With {
                .type = "saleError",
                .message = "Quantity must be greater than zero."
            })

                Return

            End If

            If sellingPrice <= 0D Then

                SendToWeb(New With {
                .type = "saleError",
                .message = "Selling price must be greater than zero."
            })

                Return

            End If

            '-------------------------------------------------
            ' CALCULATE TOTAL
            '-------------------------------------------------
            Dim totalPrice As Decimal = quantity * sellingPrice

            '-------------------------------------------------
            ' DATABASE CONNECTION
            '-------------------------------------------------
            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                transaction = con.BeginTransaction()

                '=================================================
                ' 1. GET CURRENT PRODUCT STOCK AND PRICE
                '=================================================
                Dim previousStock As Integer = 0
                Dim databasePrice As Decimal = 0D

                Dim productQuery As String =
                "SELECT StockQuantity, SellingPrice " &
                "FROM Products " &
                "WHERE ProductID = @ProductID " &
                "FOR UPDATE"

                Using cmd As New MySqlCommand(
                productQuery, con, transaction)

                    cmd.Parameters.AddWithValue("@ProductID", productID)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        If Not reader.Read() Then
                            Throw New Exception("Selected product was not found.")
                        End If

                        previousStock = Convert.ToInt32(reader("StockQuantity"))
                        databasePrice = Convert.ToDecimal(reader("SellingPrice"))

                    End Using

                End Using

                ' Use the current database price, not a client-supplied price.
                sellingPrice = databasePrice
                totalPrice = quantity * sellingPrice

                '=================================================
                ' 2. CHECK STOCK AND PRICE
                '=================================================
                If quantity > previousStock Then

                    Throw New Exception(
                    "Insufficient stock. Available stock: " &
                    previousStock.ToString())

                End If

                If sellingPrice <= 0D Then
                    Throw New Exception("The product selling price is invalid.")
                End If

                Dim newStock As Integer = previousStock - quantity

                '=================================================
                ' 3. INSERT SALE HEADER
                '=================================================
                Dim saleQuery As String =
                "INSERT INTO Sales " &
                "(CustomerName, CustomerPhone, PaymentMethod, " &
                "TotalAmount, Status) " &
                "VALUES " &
                "(@CustomerName, @CustomerPhone, @PaymentMethod, " &
                "@TotalAmount, @Status)"

                Using cmd As New MySqlCommand(
                saleQuery, con, transaction)

                    cmd.Parameters.AddWithValue("@CustomerName", customerName)
                    cmd.Parameters.AddWithValue("@CustomerPhone", customerPhone)
                    cmd.Parameters.AddWithValue("@PaymentMethod", "Cash")
                    cmd.Parameters.AddWithValue("@TotalAmount", totalPrice)
                    cmd.Parameters.AddWithValue("@Status", "Completed")

                    cmd.ExecuteNonQuery()

                    saleID = Convert.ToInt32(cmd.LastInsertedId)

                End Using

                '=================================================
                ' 4. INSERT SALE DETAIL
                '=================================================
                Dim detailQuery As String =
                "INSERT INTO SaleDetails " &
                "(SaleID, ProductID, Quantity, SellingPrice, TotalPrice) " &
                "VALUES " &
                "(@SaleID, @ProductID, @Quantity, @SellingPrice, @TotalPrice)"

                Using cmd As New MySqlCommand(
                detailQuery, con, transaction)

                    cmd.Parameters.AddWithValue("@SaleID", saleID)
                    cmd.Parameters.AddWithValue("@ProductID", productID)
                    cmd.Parameters.AddWithValue("@Quantity", quantity)
                    cmd.Parameters.AddWithValue("@SellingPrice", sellingPrice)
                    cmd.Parameters.AddWithValue("@TotalPrice", totalPrice)

                    cmd.ExecuteNonQuery()

                End Using

                '=================================================
                ' 5. UPDATE PRODUCT STOCK
                '=================================================
                Dim stockQuery As String =
                "UPDATE Products SET StockQuantity = @NewStock " &
                "WHERE ProductID = @ProductID"

                Using cmd As New MySqlCommand(
                stockQuery, con, transaction)

                    cmd.Parameters.AddWithValue("@NewStock", newStock)
                    cmd.Parameters.AddWithValue("@ProductID", productID)

                    If cmd.ExecuteNonQuery() = 0 Then
                        Throw New Exception("Product stock could not be updated.")
                    End If

                End Using

                '=================================================
                ' 6. INSERT STOCK HISTORY
                '=================================================
                Dim historyQuery As String =
                "INSERT INTO StockHistory " &
                "(ProductID, ChangeType, Quantity, PreviousStock, " &
                "NewStock, ChangeDate) " &
                "VALUES " &
                "(@ProductID, @ChangeType, @Quantity, " &
                "@PreviousStock, @NewStock, NOW())"

                Using cmd As New MySqlCommand(
                historyQuery, con, transaction)

                    cmd.Parameters.AddWithValue("@ProductID", productID)
                    cmd.Parameters.AddWithValue("@ChangeType", "SALE")
                    cmd.Parameters.AddWithValue("@Quantity", quantity)
                    cmd.Parameters.AddWithValue("@PreviousStock", previousStock)
                    cmd.Parameters.AddWithValue("@NewStock", newStock)

                    cmd.ExecuteNonQuery()

                End Using

                '=================================================
                ' 7. COMMIT TRANSACTION
                '=================================================
                transaction.Commit()
                transaction = Nothing

            End Using

            '-------------------------------------------------
            ' SUCCESS AND REFRESH
            '-------------------------------------------------
            SendToWeb(New With {
            .type = "saleAdded",
            .saleID = saleID,
            .message = "Sale completed successfully."
        })

            LoadSalesToWeb()
            LoadProductsToWeb()

        Catch ex As Exception

            Try
                If transaction IsNot Nothing Then
                    transaction.Rollback()
                End If
            Catch
                ' Ignore rollback errors.
            End Try

            SendToWeb(New With {
            .type = "saleError",
            .message = "Unable to complete sale: " & ex.Message
        })

        Finally

            If transaction IsNot Nothing Then
                transaction.Dispose()
            End If

        End Try

    End Sub


    '=========================================================
    ' LOAD PRODUCTS
    '=========================================================
    Private Sub LoadProductsToWeb()

        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()


                Dim query As String =
                    "SELECT " &
                    "p.ProductID, " &
                    "p.ProductCode, " &
                    "p.ProductName, " &
                    "p.CategoryID, " &
                    "c.CategoryName, " &
                    "p.SupplierID, " &
                    "s.SupplierName, " &
                    "p.PurchasePrice, " &
                    "p.SellingPrice, " &
                    "p.StockQuantity, " &
                    "p.MinimumStock " &
                    "FROM Products p " &
                    "LEFT JOIN Categories c " &
                    "ON p.CategoryID = c.CategoryID " &
                    "LEFT JOIN Suppliers s " &
                    "ON p.SupplierID = s.SupplierID " &
                    "ORDER BY p.ProductID DESC"


                Using cmd As New MySqlCommand(
                    query,
                    con
                )

                    Using reader As MySqlDataReader =
                        cmd.ExecuteReader()

                        Dim products As New List(
                            Of Dictionary(Of String, Object)
                        )()


                        While reader.Read()

                            Dim product As New Dictionary(
                                Of String, Object
                            )()


                            product.Add(
                                "ProductID",
                                GetDbInt(
                                    reader,
                                    "ProductID"
                                )
                            )


                            product.Add(
                                "ProductCode",
                                GetDbString(
                                    reader,
                                    "ProductCode"
                                )
                            )


                            product.Add(
                                "ProductName",
                                GetDbString(
                                    reader,
                                    "ProductName"
                                )
                            )


                            product.Add(
                                "CategoryID",
                                GetDbNullableInt(
                                    reader,
                                    "CategoryID"
                                )
                            )


                            product.Add(
                                "CategoryName",
                                GetDbString(
                                    reader,
                                    "CategoryName"
                                )
                            )


                            product.Add(
                                "SupplierID",
                                GetDbNullableInt(
                                    reader,
                                    "SupplierID"
                                )
                            )


                            product.Add(
                                "SupplierName",
                                GetDbString(
                                    reader,
                                    "SupplierName"
                                )
                            )


                            product.Add(
                                "PurchasePrice",
                                GetDbDecimal(
                                    reader,
                                    "PurchasePrice"
                                )
                            )


                            product.Add(
                                "SellingPrice",
                                GetDbDecimal(
                                    reader,
                                    "SellingPrice"
                                )
                            )


                            product.Add(
                                "StockQuantity",
                                GetDbInt(
                                    reader,
                                    "StockQuantity"
                                )
                            )


                            product.Add(
                                "MinimumStock",
                                GetDbInt(
                                    reader,
                                    "MinimumStock",
                                    10
                                )
                            )


                            products.Add(product)

                        End While


                        SendToWeb(
                            New With {
                                .type = "products",
                                .data = products
                            }
                        )

                    End Using

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Error loading products:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

    '=========================================================
    ' LOAD PRODUCT ANALYSIS TO WEB UI
    '=========================================================
    Private Sub LoadProductAnalysisToWeb()

        Try

            Using conn As MySqlConnection =
            DBConnection.GetConnection()

                conn.Open()

                '-------------------------------------------------
                ' TEMPORARY PRODUCT ANALYSIS DATA
                '-------------------------------------------------
                Dim temporaryData As New List(Of ProductAnalysisTemp)

                '-------------------------------------------------
                ' PRODUCT ANALYSIS QUERY
                '-------------------------------------------------
                Dim query As String =
                "SELECT " &
                "p.ProductID, " &
                "p.ProductCode, " &
                "p.ProductName, " &
                "p.StockQuantity, " &
                "p.SellingPrice, " &
                "c.CategoryName, " &
                "COALESCE(SUM(sd.Quantity), 0) AS UnitsSold, " &
                "COALESCE(SUM(sd.TotalPrice), 0) AS TotalSales " &
                "FROM Products p " &
                "LEFT JOIN Categories c " &
                "ON p.CategoryID = c.CategoryID " &
                "LEFT JOIN SaleDetails sd " &
                "ON p.ProductID = sd.ProductID " &
                "GROUP BY " &
                "p.ProductID, " &
                "p.ProductCode, " &
                "p.ProductName, " &
                "p.StockQuantity, " &
                "p.SellingPrice, " &
                "c.CategoryName " &
                "ORDER BY TotalSales DESC, p.ProductName ASC"


                '-------------------------------------------------
                ' EXECUTE QUERY
                '-------------------------------------------------
                Using cmd As New MySqlCommand(
                query,
                conn
            )

                    Using reader As MySqlDataReader =
                    cmd.ExecuteReader()

                        While reader.Read()

                            '-------------------------------------------------
                            ' PRODUCT ID
                            '-------------------------------------------------
                            Dim productID As Integer =
                            If(
                                reader("ProductID") Is DBNull.Value,
                                0,
                                Convert.ToInt32(
                                    reader("ProductID")
                                )
                            )


                            '-------------------------------------------------
                            ' PRODUCT CODE
                            '-------------------------------------------------
                            Dim productCode As String =
                            If(
                                reader("ProductCode") Is DBNull.Value,
                                "",
                                reader("ProductCode").ToString()
                            )


                            '-------------------------------------------------
                            ' PRODUCT NAME
                            '-------------------------------------------------
                            Dim productName As String =
                            If(
                                reader("ProductName") Is DBNull.Value,
                                "",
                                reader("ProductName").ToString()
                            )


                            '-------------------------------------------------
                            ' CATEGORY
                            '-------------------------------------------------
                            Dim categoryName As String =
                            If(
                                reader("CategoryName") Is DBNull.Value,
                                "Uncategorized",
                                reader("CategoryName").ToString()
                            )


                            '-------------------------------------------------
                            ' CURRENT STOCK
                            '-------------------------------------------------
                            Dim stockQuantity As Integer =
                            If(
                                reader("StockQuantity") Is DBNull.Value,
                                0,
                                Convert.ToInt32(
                                    reader("StockQuantity")
                                )
                            )


                            '-------------------------------------------------
                            ' SELLING PRICE
                            '-------------------------------------------------
                            Dim sellingPrice As Decimal =
                            If(
                                reader("SellingPrice") Is DBNull.Value,
                                0D,
                                Convert.ToDecimal(
                                    reader("SellingPrice")
                                )
                            )


                            '-------------------------------------------------
                            ' UNITS SOLD
                            '-------------------------------------------------
                            Dim unitsSold As Integer =
                            If(
                                reader("UnitsSold") Is DBNull.Value,
                                0,
                                Convert.ToInt32(
                                    reader("UnitsSold")
                                )
                            )


                            '-------------------------------------------------
                            ' TOTAL SALES
                            '-------------------------------------------------
                            Dim totalSales As Decimal =
                            If(
                                reader("TotalSales") Is DBNull.Value,
                                0D,
                                Convert.ToDecimal(
                                    reader("TotalSales")
                                )
                            )


                            '-------------------------------------------------
                            ' ADD PRODUCT TO TEMPORARY LIST
                            '-------------------------------------------------
                            temporaryData.Add(
                            New ProductAnalysisTemp With {
                                .ProductID = productID,
                                .ProductCode = productCode,
                                .ProductName = productName,
                                .CategoryName = categoryName,
                                .StockQuantity = stockQuantity,
                                .SellingPrice = sellingPrice,
                                .UnitsSold = unitsSold,
                                .TotalSales = totalSales
                            }
                        )

                        End While

                    End Using

                End Using


                '-------------------------------------------------
                ' FIND HIGHEST UNITS SOLD
                '-------------------------------------------------
                Dim highestUnitsSold As Integer = 0


                For Each item As ProductAnalysisTemp In temporaryData

                    If item.UnitsSold >
                   highestUnitsSold Then

                        highestUnitsSold =
                        item.UnitsSold

                    End If

                Next


                '-------------------------------------------------
                ' FINAL ANALYSIS LIST
                '-------------------------------------------------
                Dim analysis As New List(Of Object)


                For Each item As ProductAnalysisTemp In temporaryData

                    Dim performance As String =
                    "never"


                    '-------------------------------------------------
                    ' NO SALES
                    '-------------------------------------------------
                    If item.UnitsSold <= 0 Then

                        performance = "never"


                        '-------------------------------------------------
                        ' ONLY ONE SALE LEVEL
                        '-------------------------------------------------
                    ElseIf highestUnitsSold <= 1 Then

                        performance = "top"


                    Else

                        '-------------------------------------------------
                        ' CALCULATE PERFORMANCE PERCENTAGE
                        '-------------------------------------------------
                        Dim percentage As Double =
                        (
                            CDbl(item.UnitsSold) /
                            CDbl(highestUnitsSold)
                        ) * 100


                        '-------------------------------------------------
                        ' CLASSIFY PRODUCT
                        '-------------------------------------------------
                        If percentage >= 70 Then

                            performance = "top"

                        ElseIf percentage >= 30 Then

                            performance = "medium"

                        Else

                            performance = "low"

                        End If

                    End If


                    '-------------------------------------------------
                    ' ADD FINAL RESULT
                    '-------------------------------------------------
                    analysis.Add(
                    New With {
                        .ProductID = item.ProductID,
                        .ProductCode = item.ProductCode,
                        .ProductName = item.ProductName,
                        .CategoryName = item.CategoryName,
                        .StockQuantity = item.StockQuantity,
                        .SellingPrice = item.SellingPrice,
                        .UnitsSold = item.UnitsSold,
                        .TotalSales = item.TotalSales,
                        .Performance = performance
                    }
                )

                Next


                '-------------------------------------------------
                ' SEND DATA TO WEB UI
                '-------------------------------------------------
                SendToWeb(
                New With {
                    .type = "productAnalysis",
                    .data = analysis
                }
            )

            End Using


        Catch ex As Exception

            '-----------------------------------------------------
            ' SEND ERROR TO WEB UI
            '-----------------------------------------------------
            SendToWeb(
            New With {
                .type = "productAnalysisError",
                .message =
                    "Unable to load product analysis: " &
                    ex.Message
            }
        )

        End Try

    End Sub


    '=========================================================
    ' PRODUCT ANALYSIS TEMPORARY DATA
    '=========================================================
    Private Class ProductAnalysisTemp

        Public Property ProductID As Integer

        Public Property ProductCode As String

        Public Property ProductName As String

        Public Property CategoryName As String

        Public Property StockQuantity As Integer

        Public Property SellingPrice As Decimal

        Public Property UnitsSold As Integer

        Public Property TotalSales As Decimal

    End Class


    '=========================================================
    ' LOAD INVENTORY TO WEB UI
    '=========================================================
    Private Sub LoadInventoryToWeb()

        Try

            Using conn As MySqlConnection =
            DBConnection.GetConnection()

                conn.Open()

                Dim inventory As New List(Of Object)

                Dim query As String =
                "SELECT " &
                "p.ProductID, " &
                "p.ProductCode, " &
                "p.ProductName, " &
                "p.StockQuantity, " &
                "p.MinimumStock, " &
                "p.PurchasePrice, " &
                "p.SellingPrice, " &
                "c.CategoryName " &
                "FROM Products p " &
                "LEFT JOIN Categories c " &
                "ON p.CategoryID = c.CategoryID " &
                "ORDER BY p.ProductName ASC"

                Using cmd As New MySqlCommand(
                query,
                conn
            )

                    Using reader As MySqlDataReader =
                    cmd.ExecuteReader()

                        While reader.Read()

                            Dim productID As Integer =
                            If(
                                reader("ProductID") Is DBNull.Value,
                                0,
                                Convert.ToInt32(
                                    reader("ProductID")
                                )
                            )

                            Dim productCode As String =
                            If(
                                reader("ProductCode") Is DBNull.Value,
                                "",
                                reader("ProductCode").ToString()
                            )

                            Dim productName As String =
                            If(
                                reader("ProductName") Is DBNull.Value,
                                "",
                                reader("ProductName").ToString()
                            )

                            Dim categoryName As String =
                            If(
                                reader("CategoryName") Is DBNull.Value,
                                "Uncategorized",
                                reader("CategoryName").ToString()
                            )

                            Dim stockQuantity As Integer =
                            If(
                                reader("StockQuantity") Is DBNull.Value,
                                0,
                                Convert.ToInt32(
                                    reader("StockQuantity")
                                )
                            )

                            Dim minimumStock As Integer =
                            If(
                                reader("MinimumStock") Is DBNull.Value,
                                0,
                                Convert.ToInt32(
                                    reader("MinimumStock")
                                )
                            )

                            Dim purchasePrice As Decimal =
                            If(
                                reader("PurchasePrice") Is DBNull.Value,
                                0D,
                                Convert.ToDecimal(
                                    reader("PurchasePrice")
                                )
                            )

                            Dim sellingPrice As Decimal =
                            If(
                                reader("SellingPrice") Is DBNull.Value,
                                0D,
                                Convert.ToDecimal(
                                    reader("SellingPrice")
                                )
                            )

                            inventory.Add(
                            New With {
                                .ProductID = productID,
                                .ProductCode = productCode,
                                .ProductName = productName,
                                .CategoryName = categoryName,
                                .StockQuantity = stockQuantity,
                                .MinimumStock = minimumStock,
                                .PurchasePrice = purchasePrice,
                                .SellingPrice = sellingPrice
                            }
                        )

                        End While

                    End Using

                End Using

                SendToWeb(
                New With {
                    .type = "inventory",
                    .data = inventory
                }
            )

            End Using

        Catch ex As Exception

            SendToWeb(
            New With {
                .type = "inventoryError",
                .message =
                    "Unable to load inventory: " &
                    ex.Message
            }
        )

        End Try

    End Sub

    '=========================================================
    ' LOAD CATEGORIES
    '=========================================================
    Private Sub LoadCategoriesToWeb()

        Try

            Using con As MySqlConnection =
            DBConnection.GetConnection()

                con.Open()


                '-------------------------------------------------
                ' LOAD CATEGORIES
                '-------------------------------------------------
                Dim query As String =
                "SELECT CategoryID,
                CategoryName " &
                "FROM Categories " &
                "ORDER BY CategoryID ASC"


                Using cmd As New MySqlCommand(
                query,
                con
            )

                    Using reader As MySqlDataReader =
                    cmd.ExecuteReader()

                        Dim categories As New List(
                        Of Dictionary(Of String, Object)
                    )()


                        While reader.Read()

                            Dim category As New Dictionary(
                            Of String, Object
                        )()


                            category.Add(
                            "CategoryID",
                            Convert.ToInt32(
                                reader("CategoryID")
                            )
                        )


                            category.Add(
                            "CategoryName",
                            reader("CategoryName").ToString()
                        )


                            categories.Add(category)

                        End While


                        '-------------------------------------------------
                        ' SEND TO WEB UI
                        '-------------------------------------------------
                        SendToWeb(
                        New With {
                            .type = "categories",
                            .data = categories
                        }
                    )

                    End Using

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
            "Error loading categories:" &
            Environment.NewLine &
            Environment.NewLine &
            ex.Message,
            "Database Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

        End Try

    End Sub


    '=========================================================
    ' ADD CATEGORY
    '=========================================================
    Private Sub AddCategory(root As JsonElement)

        Try

            '-------------------------------------------------
            ' GET CATEGORY NAME
            '-------------------------------------------------
            Dim categoryName As String =
            GetStringValue(
                root,
                "CategoryName"
            ).Trim()


            '-------------------------------------------------
            ' VALIDATE
            '-------------------------------------------------
            If String.IsNullOrWhiteSpace(categoryName) Then

                SendToWeb(
                New With {
                    .type = "categoryError",
                    .message = "Category name is required."
                }
            )

                Return

            End If


            '-------------------------------------------------
            ' DATABASE
            '-------------------------------------------------
            Using con As MySqlConnection =
            DBConnection.GetConnection()

                con.Open()


                '-------------------------------------------------
                ' CHECK DUPLICATE
                '-------------------------------------------------
                Dim checkQuery As String =
                "SELECT COUNT(*) " &
                "FROM Categories " &
                "WHERE CategoryName = @CategoryName"


                Using checkCmd As New MySqlCommand(
                checkQuery,
                con
            )

                    checkCmd.Parameters.AddWithValue(
                    "@CategoryName",
                    categoryName
                )


                    Dim existingCount As Integer =
                    Convert.ToInt32(
                        checkCmd.ExecuteScalar()
                    )


                    If existingCount > 0 Then

                        SendToWeb(
                        New With {
                            .type = "categoryError",
                            .message =
                                "Category already exists."
                        }
                    )

                        Return

                    End If

                End Using


                '-------------------------------------------------
                ' INSERT CATEGORY
                '-------------------------------------------------
                Dim insertQuery As String =
                "INSERT INTO Categories " &
                "(CategoryName) " &
                "VALUES " &
                "(@CategoryName)"


                Using cmd As New MySqlCommand(
                insertQuery,
                con
            )

                    cmd.Parameters.AddWithValue(
                    "@CategoryName",
                    categoryName
                )


                    cmd.ExecuteNonQuery()

                End Using

            End Using


            '-------------------------------------------------
            ' SUCCESS
            '-------------------------------------------------
            SendToWeb(
            New With {
                .type = "categoryAdded",
                .message =
                    "Category added successfully."
            }
        )


            '-------------------------------------------------
            ' REFRESH CATEGORY LIST
            '-------------------------------------------------
            LoadCategoriesToWeb()


            '-------------------------------------------------
            ' ALSO REFRESH PRODUCT PAGE DATA
            '-------------------------------------------------
            LoadProductsToWeb()


        Catch ex As Exception

            SendToWeb(
            New With {
                .type = "categoryError",
                .message =
                    "Unable to add category: " &
                    ex.Message
            }
        )

        End Try

    End Sub


    '=========================================================
    ' DELETE CATEGORY
    '=========================================================
    Private Sub DeleteCategory(root As JsonElement)

        Try

            '-------------------------------------------------
            ' GET CATEGORY ID
            '-------------------------------------------------
            Dim categoryID As Integer =
            GetIntegerValue(
                root,
                "CategoryID"
            )


            '-------------------------------------------------
            ' VALIDATE
            '-------------------------------------------------
            If categoryID <= 0 Then

                SendToWeb(
                New With {
                    .type = "categoryError",
                    .message = "Invalid category ID."
                }
            )

                Return

            End If


            '-------------------------------------------------
            ' DATABASE
            '-------------------------------------------------
            Using con As MySqlConnection =
            DBConnection.GetConnection()

                con.Open()


                '-------------------------------------------------
                ' CHECK WHETHER CATEGORY IS USED BY PRODUCTS
                '-------------------------------------------------
                Dim checkQuery As String =
                "SELECT COUNT(*) " &
                "FROM Products " &
                "WHERE CategoryID = @CategoryID"


                Using checkCmd As New MySqlCommand(
                checkQuery,
                con
            )

                    checkCmd.Parameters.AddWithValue(
                    "@CategoryID",
                    categoryID
                )


                    Dim productCount As Integer =
                    Convert.ToInt32(
                        checkCmd.ExecuteScalar()
                    )


                    If productCount > 0 Then

                        SendToWeb(
                        New With {
                            .type = "categoryError",
                            .message =
                                "This category is being used by a product and cannot be deleted."
                        }
                    )

                        Return

                    End If

                End Using


                '-------------------------------------------------
                ' DELETE CATEGORY
                '-------------------------------------------------
                Dim deleteQuery As String =
                "DELETE FROM Categories " &
                "WHERE CategoryID = @CategoryID"


                Using cmd As New MySqlCommand(
                deleteQuery,
                con
            )

                    cmd.Parameters.AddWithValue(
                    "@CategoryID",
                    categoryID
                )

                    cmd.ExecuteNonQuery()

                End Using

            End Using


            '-------------------------------------------------
            ' SUCCESS
            '-------------------------------------------------
            SendToWeb(
            New With {
                .type = "categoryDeleted",
                .message =
                    "Category deleted successfully."
            }
        )


            '-------------------------------------------------
            ' REFRESH CATEGORY LIST
            '-------------------------------------------------
            LoadCategoriesToWeb()


            '-------------------------------------------------
            ' REFRESH PRODUCTS
            '-------------------------------------------------
            LoadProductsToWeb()


        Catch ex As Exception

            SendToWeb(
            New With {
                .type = "categoryError",
                .message =
                    "Unable to delete category: " &
                    ex.Message
            }
        )

        End Try

    End Sub


    '=========================================================
    ' LOAD SUPPLIERS
    '=========================================================
    Private Sub LoadSuppliersToWeb()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                "SELECT SupplierID, SupplierName, Phone, Email, Address " &
                "FROM Suppliers " &
                "ORDER BY SupplierName"

                Using cmd As New MySqlCommand(query, con)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        Dim suppliers As New List(Of Dictionary(Of String, Object))()

                        While reader.Read()

                            Dim supplier As New Dictionary(Of String, Object)()

                            supplier.Add(
                            "SupplierID",
                            Convert.ToInt32(reader("SupplierID"))
                        )

                            supplier.Add(
                            "SupplierName",
                            reader("SupplierName").ToString()
                        )

                            supplier.Add(
                            "Phone",
                            If(reader.IsDBNull(reader.GetOrdinal("Phone")),
                               "",
                               reader("Phone").ToString())
                        )

                            supplier.Add(
                            "Email",
                            If(reader.IsDBNull(reader.GetOrdinal("Email")),
                               "",
                               reader("Email").ToString())
                        )

                            supplier.Add(
                            "Address",
                            If(reader.IsDBNull(reader.GetOrdinal("Address")),
                               "",
                               reader("Address").ToString())
                        )

                            suppliers.Add(supplier)

                        End While

                        SendToWeb(
                        New With {
                            .type = "suppliers",
                            .data = suppliers
                        }
                    )

                    End Using

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
            "Error loading suppliers:" &
            Environment.NewLine &
            Environment.NewLine &
            ex.Message,
            "Database Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

        End Try

    End Sub

    Private Sub AddSupplier(root As JsonElement)

        Try

            '-------------------------------------------------
            ' GET DATA FROM WEB
            '-------------------------------------------------
            Dim supplierName As String = GetStringValue(root, "SupplierName")
            Dim phone As String = GetStringValue(root, "Phone")
            Dim email As String = GetStringValue(root, "Email")
            Dim address As String = GetStringValue(root, "Address")

            '-------------------------------------------------
            ' VALIDATION
            '-------------------------------------------------
            If String.IsNullOrWhiteSpace(supplierName) Then

                SendToWeb(New With {
                .type = "supplierError",
                .message = "Supplier name is required."
            })

                Return

            End If

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                '-------------------------------------------------
                ' CHECK DUPLICATE SUPPLIER
                '-------------------------------------------------
                Dim checkQuery As String =
                "SELECT COUNT(*) FROM Suppliers " &
                "WHERE SupplierName = @SupplierName"

                Using checkCmd As New MySqlCommand(checkQuery, con)

                    checkCmd.Parameters.AddWithValue("@SupplierName", supplierName)

                    Dim count As Integer =
                    Convert.ToInt32(checkCmd.ExecuteScalar())

                    If count > 0 Then

                        SendToWeb(New With {
                        .type = "supplierError",
                        .message = "Supplier already exists."
                    })

                        Return

                    End If

                End Using

                '-------------------------------------------------
                ' INSERT SUPPLIER
                '-------------------------------------------------
                Dim insertQuery As String =
                "INSERT INTO Suppliers " &
                "(SupplierName, Phone, Email, Address) " &
                "VALUES " &
                "(@SupplierName, @Phone, @Email, @Address)"

                Using cmd As New MySqlCommand(insertQuery, con)

                    cmd.Parameters.AddWithValue("@SupplierName", supplierName)
                    cmd.Parameters.AddWithValue("@Phone", phone)
                    cmd.Parameters.AddWithValue("@Email", email)
                    cmd.Parameters.AddWithValue("@Address", address)

                    cmd.ExecuteNonQuery()

                End Using

            End Using

            '-------------------------------------------------
            ' SUCCESS
            '-------------------------------------------------
            SendToWeb(New With {
            .type = "supplierAdded",
            .message = "Supplier added successfully."
        })

            ' Refresh supplier list
            LoadSuppliersToWeb()

            ' Refresh product page supplier dropdown
            LoadProductsToWeb()

        Catch ex As Exception

            SendToWeb(New With {
            .type = "supplierError",
            .message = ex.Message
        })

        End Try

    End Sub

    '=========================================================
    ' LOAD PURCHASES
    '=========================================================
    Private Sub LoadPurchasesToWeb()

        Try

            Using con As MySqlConnection =
            DBConnection.GetConnection()

                con.Open()
                Debug.WriteLine("LoadPurchasesToWeb called")
                Debug.WriteLine("Current role: " & If(Session.CurrentRole, "(empty)"))

                Dim query As String =
                "SELECT " &
                "p.PurchaseID, " &
                "p.SupplierID, " &
                "s.SupplierName, " &
                "p.PurchaseDate, " &
                "p.TotalAmount " &
                "FROM Purchases p " &
                "LEFT JOIN Suppliers s " &
                "ON p.SupplierID = s.SupplierID " &
                "ORDER BY p.PurchaseID DESC"

                Using cmd As New MySqlCommand(query, con)

                    Using reader As MySqlDataReader =
                    cmd.ExecuteReader()

                        Dim purchases As New List(
                        Of Dictionary(Of String, Object)
                    )()

                        While reader.Read()

                            Dim purchase As New Dictionary(Of String, Object)()

                            purchase("PurchaseID") = GetDbInt(reader, "PurchaseID")
                            purchase("SupplierID") = GetDbNullableInt(reader, "SupplierID")
                            purchase("SupplierName") = GetDbString(reader, "SupplierName")

                            If IsDBNull(reader("PurchaseDate")) Then
                                purchase("PurchaseDate") = ""
                            Else
                                purchase("PurchaseDate") =
                                Convert.ToDateTime(reader("PurchaseDate")).
                                ToString("yyyy-MM-dd HH:mm:ss")
                            End If

                            purchase("TotalAmount") = GetDbDecimal(reader, "TotalAmount")

                            purchases.Add(purchase)

                        End While

                        ' Debug line: purchases is declared earlier in this same method
                        Debug.WriteLine("Purchases loaded: " & purchases.Count.ToString())

                        SendToWeb(
                                    New With {
                                        .type = "purchases",
                                        .data = purchases
                                    }
                                )

                    End Using

                End Using

            End Using

        Catch ex As Exception

            Debug.WriteLine("Error loading purchases: " & ex.ToString())

            SendToWeb(
        New With {
            .type = "purchaseError",
            .message = "Error loading purchases: " & ex.Message
        }
    )

        End Try

    End Sub

    '=========================================================
    ' ADD PURCHASE
    '=========================================================
    Private Sub LoadProductsBySupplier(root As JsonElement)

        Dim supplierID As Integer = 0
        Dim requestNumber As Integer = 0

        Try
            supplierID = GetIntegerValue(root, "SupplierID")
            requestNumber = GetIntegerValue(root, "RequestNumber")

            If supplierID <= 0 Then
                SendToWeb(New With {
                .type = "supplierProducts",
                .SupplierID = supplierID,
                .RequestNumber = requestNumber,
                .data = New Object() {}
            })
                Return
            End If

            Dim products As New List(Of Dictionary(Of String, Object))()

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                "SELECT ProductID, ProductName, StockQuantity, PurchasePrice " &
                "FROM Products " &
                "WHERE SupplierID = @SupplierID " &
                "ORDER BY ProductName"

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue("@SupplierID", supplierID)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        While reader.Read()

                            products.Add(New Dictionary(Of String, Object) From {
                            {"ProductID", Convert.ToInt32(reader("ProductID"))},
                            {"ProductName", reader("ProductName").ToString()},
                            {"StockQuantity", Convert.ToInt32(reader("StockQuantity"))},
                            {"PurchasePrice", Convert.ToDecimal(reader("PurchasePrice"))}
                        })

                        End While

                    End Using
                End Using
            End Using

            SendToWeb(New With {
            .type = "supplierProducts",
            .SupplierID = supplierID,
            .RequestNumber = requestNumber,
            .data = products
        })

        Catch ex As Exception

            SendToWeb(New With {
            .type = "supplierProductsError",
            .SupplierID = supplierID,
            .RequestNumber = requestNumber,
            .message = ex.Message
        })

        End Try

    End Sub


    '=========================================================
    ' SEND DATA TO WEB UI
    '=========================================================


    Private Sub SendToWeb(data As Object)
        Try
            If WebView21.CoreWebView2 Is Nothing Then
                Debug.WriteLine("SendToWeb: WebView2 is not ready.")
                Return
            End If

            Dim json As String = JsonSerializer.Serialize(data)

            Debug.WriteLine("SendToWeb payload: " & json)

            ' Send the response to the top-level page.
            WebView21.CoreWebView2.PostWebMessageAsJson(json)

            Debug.WriteLine("SendToWeb: response sent to main page.")

        Catch ex As Exception
            Debug.WriteLine("SendToWeb error: " & ex.ToString())
        End Try
    End Sub




    '=========================================================
    ' ADD PRODUCT
    '=========================================================
    Private Sub AddProduct(root As JsonElement)

        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()


                Dim query As String =
                    "INSERT INTO Products " &
                    "(ProductCode, ProductName, CategoryID, " &
                    "SupplierID, PurchasePrice, SellingPrice, " &
                    "StockQuantity, MinimumStock) " &
                    "VALUES " &
                    "(@ProductCode, @ProductName, @CategoryID, " &
                    "@SupplierID, @PurchasePrice, @SellingPrice, " &
                    "@StockQuantity, @MinimumStock)"


                Using cmd As New MySqlCommand(
                    query,
                    con
                )

                    cmd.Parameters.AddWithValue(
                        "@ProductCode",
                        GetStringValue(
                            root,
                            "ProductCode"
                        )
                    )


                    cmd.Parameters.AddWithValue(
                        "@ProductName",
                        GetStringValue(
                            root,
                            "ProductName"
                        )
                    )


                    cmd.Parameters.AddWithValue(
                        "@CategoryID",
                        GetNullableInteger(
                            root,
                            "CategoryID"
                        )
                    )


                    cmd.Parameters.AddWithValue(
                        "@SupplierID",
                        GetNullableInteger(
                            root,
                            "SupplierID"
                        )
                    )


                    cmd.Parameters.AddWithValue(
                        "@PurchasePrice",
                        GetDecimalValue(
                            root,
                            "PurchasePrice"
                        )
                    )


                    cmd.Parameters.AddWithValue(
                        "@SellingPrice",
                        GetDecimalValue(
                            root,
                            "SellingPrice"
                        )
                    )


                    cmd.Parameters.AddWithValue(
                        "@StockQuantity",
                        GetIntegerValue(
                            root,
                            "StockQuantity"
                        )
                    )


                    cmd.Parameters.AddWithValue(
                        "@MinimumStock",
                        GetIntegerValue(
                            root,
                            "MinimumStock",
                            10
                        )
                    )


                    cmd.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Product added successfully.",
                "Smart Inventory",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            RefreshProductPage()


        Catch ex As Exception

            MessageBox.Show(
                "Error adding product:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '=========================================================
    ' UPDATE PRODUCT
    '=========================================================
    Private Sub UpdateProduct(root As JsonElement)

        Try

            Dim productID As Integer =
                GetIntegerValue(
                    root,
                    "ProductID"
                )


            If productID <= 0 Then

                MessageBox.Show(
                    "Invalid Product ID.",
                    "Product",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                )

                Return

            End If


            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()


                Dim query As String =
                    "UPDATE Products SET " &
                    "ProductCode = @ProductCode, " &
                    "ProductName = @ProductName, " &
                    "CategoryID = @CategoryID, " &
                    "SupplierID = @SupplierID, " &
                    "PurchasePrice = @PurchasePrice, " &
                    "SellingPrice = @SellingPrice, " &
                    "StockQuantity = @StockQuantity, " &
                    "MinimumStock = @MinimumStock " &
                    "WHERE ProductID = @ProductID"


                Using cmd As New MySqlCommand(
                    query,
                    con
                )

                    cmd.Parameters.AddWithValue(
                        "@ProductID",
                        productID
                    )


                    cmd.Parameters.AddWithValue(
                        "@ProductCode",
                        GetStringValue(
                            root,
                            "ProductCode"
                        )
                    )


                    cmd.Parameters.AddWithValue(
                        "@ProductName",
                        GetStringValue(
                            root,
                            "ProductName"
                        )
                    )


                    cmd.Parameters.AddWithValue(
                        "@CategoryID",
                        GetNullableInteger(
                            root,
                            "CategoryID"
                        )
                    )


                    cmd.Parameters.AddWithValue(
                        "@SupplierID",
                        GetNullableInteger(
                            root,
                            "SupplierID"
                        )
                    )


                    cmd.Parameters.AddWithValue(
                        "@PurchasePrice",
                        GetDecimalValue(
                            root,
                            "PurchasePrice"
                        )
                    )


                    cmd.Parameters.AddWithValue(
                        "@SellingPrice",
                        GetDecimalValue(
                            root,
                            "SellingPrice"
                        )
                    )


                    cmd.Parameters.AddWithValue(
                        "@StockQuantity",
                        GetIntegerValue(
                            root,
                            "StockQuantity"
                        )
                    )


                    cmd.Parameters.AddWithValue(
                        "@MinimumStock",
                        GetIntegerValue(
                            root,
                            "MinimumStock",
                            10
                        )
                    )


                    cmd.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Product updated successfully.",
                "Smart Inventory",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            RefreshProductPage()


        Catch ex As Exception

            MessageBox.Show(
                "Error updating product:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '=========================================================
    ' DELETE PRODUCT
    '=========================================================
    Private Sub DeleteProduct(root As JsonElement)

        Try

            Dim productID As Integer =
                GetIntegerValue(
                    root,
                    "ProductID"
                )


            If productID <= 0 Then

                MessageBox.Show(
                    "Invalid Product ID.",
                    "Product",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                )

                Return

            End If


            Dim result As DialogResult =
                MessageBox.Show(
                    "Are you sure you want to delete this product?",
                    "Delete Product",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                )


            If result <> DialogResult.Yes Then
                Return
            End If


            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()


                Dim query As String =
                    "DELETE FROM Products " &
                    "WHERE ProductID = @ProductID"


                Using cmd As New MySqlCommand(
                    query,
                    con
                )

                    cmd.Parameters.AddWithValue(
                        "@ProductID",
                        productID
                    )


                    cmd.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Product deleted successfully.",
                "Smart Inventory",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            RefreshProductPage()

        Catch ex As MySqlException When ex.Number = 1451

            MessageBox.Show(
                "This product cannot be deleted because it is already used in purchases, sales, or stock history.",
                "Product Cannot Be Deleted",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

        Catch ex As Exception

            MessageBox.Show(
                "Error deleting product:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '=========================================================
    ' REFRESH PRODUCT PAGE
    '=========================================================
    Private Sub RefreshProductPage()

        LoadProductsToWeb()

        LoadCategoriesToWeb()

        LoadSuppliersToWeb()

    End Sub


    '=========================================================
    ' GET STRING VALUE
    '=========================================================
    Private Function GetStringValue(
        root As JsonElement,
        propertyName As String
    ) As String

        Dim element As JsonElement


        If Not root.TryGetProperty(
            propertyName,
            element
        ) Then

            Return ""

        End If


        If element.ValueKind =
            JsonValueKind.Null Then

            Return ""

        End If


        Return element.ToString()

    End Function


    '=========================================================
    ' GET INTEGER VALUE
    '=========================================================
    Private Function GetIntegerValue(
        root As JsonElement,
        propertyName As String,
        Optional defaultValue As Integer = 0
    ) As Integer

        Dim element As JsonElement


        If Not root.TryGetProperty(
            propertyName,
            element
        ) Then

            Return defaultValue

        End If


        If element.ValueKind =
            JsonValueKind.Null Then

            Return defaultValue

        End If


        Dim value As Integer


        If Integer.TryParse(
            element.ToString(),
            value
        ) Then

            Return value

        End If


        Dim decimalValue As Decimal


        If Decimal.TryParse(
            element.ToString(),
            decimalValue
        ) Then

            Return Convert.ToInt32(
                decimalValue
            )

        End If


        Return defaultValue

    End Function


    '=========================================================
    ' GET DECIMAL VALUE
    '=========================================================
    Private Function GetDecimalValue(
        root As JsonElement,
        propertyName As String,
        Optional defaultValue As Decimal = 0D
    ) As Decimal

        Dim element As JsonElement


        If Not root.TryGetProperty(
            propertyName,
            element
        ) Then

            Return defaultValue

        End If


        If element.ValueKind =
            JsonValueKind.Null Then

            Return defaultValue

        End If


        Dim value As Decimal


        If Decimal.TryParse(
            element.ToString(),
            value
        ) Then

            Return value

        End If


        Return defaultValue

    End Function


    '=========================================================
    ' GET NULLABLE INTEGER
    '=========================================================
    Private Function GetNullableInteger(
        root As JsonElement,
        propertyName As String
    ) As Object

        Dim element As JsonElement


        If Not root.TryGetProperty(
            propertyName,
            element
        ) Then

            Return DBNull.Value

        End If


        If element.ValueKind =
            JsonValueKind.Null Then

            Return DBNull.Value

        End If


        If String.IsNullOrWhiteSpace(
            element.ToString()
        ) Then

            Return DBNull.Value

        End If


        Dim value As Integer


        If Integer.TryParse(
            element.ToString(),
            value
        ) Then

            Return value

        End If


        Return DBNull.Value

    End Function


    '=========================================================
    ' GET DATABASE INTEGER
    '=========================================================
    Private Function GetDbInt(
        reader As MySqlDataReader,
        columnName As String,
        Optional defaultValue As Integer = 0
    ) As Integer

        If IsDBNull(
            reader(columnName)
        ) Then

            Return defaultValue

        End If


        Return Convert.ToInt32(
            reader(columnName)
        )

    End Function


    '=========================================================
    ' GET DATABASE NULLABLE INTEGER
    '=========================================================
    Private Function GetDbNullableInt(
        reader As MySqlDataReader,
        columnName As String
    ) As Object

        If IsDBNull(
            reader(columnName)
        ) Then

            Return Nothing

        End If


        Return Convert.ToInt32(
            reader(columnName)
        )

    End Function


    '=========================================================
    ' GET DATABASE DECIMAL
    '=========================================================
    Private Function GetDbDecimal(
        reader As MySqlDataReader,
        columnName As String,
        Optional defaultValue As Decimal = 0D
    ) As Decimal

        If IsDBNull(
            reader(columnName)
        ) Then

            Return defaultValue

        End If


        Return Convert.ToDecimal(
            reader(columnName)
        )

    End Function


    '=========================================================
    ' GET DATABASE STRING
    '=========================================================
    Private Function GetDbString(
        reader As MySqlDataReader,
        columnName As String
    ) As String

        If IsDBNull(
            reader(columnName)
        ) Then

            Return ""

        End If


        Return reader(columnName).ToString()

    End Function


    '=========================================================
    ' SALES ANALYTICS
    '=========================================================
    Private Sub LoadSalesAnalyticsToWeb(
        root As JsonElement
    )

        Try

            '-------------------------------------------------
            ' Default date range
            '-------------------------------------------------
            Dim fromDate As Date =
                Date.Today.AddDays(-30)

            Dim toDate As Date =
                Date.Today


            Dim fromDateElement As JsonElement
            Dim toDateElement As JsonElement


            If root.TryGetProperty(
                "fromDate",
                fromDateElement
            ) Then

                If fromDateElement.ValueKind =
                    JsonValueKind.String Then

                    Dim tempFrom As Date

                    If Date.TryParse(
                        fromDateElement.GetString(),
                        tempFrom
                    ) Then

                        fromDate = tempFrom

                    End If

                End If

            End If


            If root.TryGetProperty(
                "toDate",
                toDateElement
            ) Then

                If toDateElement.ValueKind =
                    JsonValueKind.String Then

                    Dim tempTo As Date

                    If Date.TryParse(
                        toDateElement.GetString(),
                        tempTo
                    ) Then

                        toDate = tempTo

                    End If

                End If

            End If


            '-------------------------------------------------
            ' Validate date range
            '-------------------------------------------------
            If fromDate > toDate Then

                Dim tempDate As Date =
                    fromDate

                fromDate =
                    toDate

                toDate =
                    tempDate

            End If


            Using conn As MySqlConnection =
                DBConnection.GetConnection()

                conn.Open()


                '=================================================
                ' TOTAL SALES / ORDERS / AVERAGE
                '=================================================
                Dim totalSales As Decimal = 0D

                Dim totalOrders As Integer = 0

                Dim averageSale As Decimal = 0D


                Dim totalQuery As String =
                    "SELECT " &
                    "COUNT(*) AS TotalOrders, " &
                    "COALESCE(SUM(TotalAmount), 0) AS TotalSales, " &
                    "COALESCE(AVG(TotalAmount), 0) AS AverageSale " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < @ToDate"


                Using cmd As New MySqlCommand(
                    totalQuery,
                    conn
                )

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        fromDate.Date
                    )


                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        toDate.Date.AddDays(1)
                    )


                    Using reader As MySqlDataReader =
                        cmd.ExecuteReader()

                        If reader.Read() Then

                            totalOrders =
                                Convert.ToInt32(
                                    reader("TotalOrders")
                                )


                            totalSales =
                                Convert.ToDecimal(
                                    reader("TotalSales")
                                )


                            averageSale =
                                Convert.ToDecimal(
                                    reader("AverageSale")
                                )

                        End If

                    End Using

                End Using


                '=================================================
                ' DAILY SALES
                '=================================================
                Dim dailySales As New List(Of Object)


                Dim dailyQuery As String =
                    "SELECT " &
                    "DATE(SaleDate) AS SaleDay, " &
                    "SUM(TotalAmount) AS Amount " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < @ToDate " &
                    "GROUP BY DATE(SaleDate) " &
                    "ORDER BY DATE(SaleDate)"


                Using cmd As New MySqlCommand(
                    dailyQuery,
                    conn
                )

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        fromDate.Date
                    )


                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        toDate.Date.AddDays(1)
                    )


                    Using reader As MySqlDataReader =
                        cmd.ExecuteReader()

                        While reader.Read()

                            Dim item As New Dictionary(
                                Of String, Object
                            )


                            item.Add(
                                "date",
                                Convert.ToDateTime(
                                    reader("SaleDay")
                                ).ToString("yyyy-MM-dd")
                            )


                            item.Add(
                                "amount",
                                Convert.ToDecimal(
                                    reader("Amount")
                                )
                            )


                            dailySales.Add(item)

                        End While

                    End Using

                End Using


                '=================================================
                ' MONTHLY SALES
                '=================================================
                Dim monthlySales As New List(Of Object)


                Dim monthlyQuery As String =
                    "SELECT " &
                    "DATE_FORMAT(SaleDate, '%Y-%m') AS SaleMonth, " &
                    "SUM(TotalAmount) AS Amount " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < @ToDate " &
                    "GROUP BY DATE_FORMAT(SaleDate, '%Y-%m') " &
                    "ORDER BY SaleMonth"


                Using cmd As New MySqlCommand(
                    monthlyQuery,
                    conn
                )

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        fromDate.Date
                    )


                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        toDate.Date.AddDays(1)
                    )


                    Using reader As MySqlDataReader =
                        cmd.ExecuteReader()

                        While reader.Read()

                            Dim item As New Dictionary(
                                Of String, Object
                            )


                            item.Add(
                                "month",
                                reader("SaleMonth").ToString()
                            )


                            item.Add(
                                "amount",
                                Convert.ToDecimal(
                                    reader("Amount")
                                )
                            )


                            monthlySales.Add(item)

                        End While

                    End Using

                End Using


                '=================================================
                ' PAYMENT METHODS
                '=================================================
                Dim paymentSales As New List(Of Object)


                Dim paymentQuery As String =
                    "SELECT " &
                    "PaymentMethod, " &
                    "SUM(TotalAmount) AS Amount " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < @ToDate " &
                    "GROUP BY PaymentMethod " &
                    "ORDER BY PaymentMethod"


                Using cmd As New MySqlCommand(
                    paymentQuery,
                    conn
                )

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        fromDate.Date
                    )


                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        toDate.Date.AddDays(1)
                    )


                    Using reader As MySqlDataReader =
                        cmd.ExecuteReader()

                        While reader.Read()

                            Dim item As New Dictionary(
                                Of String, Object
                            )


                            item.Add(
                                "method",
                                reader("PaymentMethod").ToString()
                            )


                            item.Add(
                                "amount",
                                Convert.ToDecimal(
                                    reader("Amount")
                                )
                            )


                            paymentSales.Add(item)

                        End While

                    End Using

                End Using


                '=================================================
                ' SALES STATUS
                '=================================================
                Dim statusSales As New List(Of Object)


                Dim statusQuery As String =
                    "SELECT " &
                    "Status, " &
                    "COUNT(*) AS Orders, " &
                    "SUM(TotalAmount) AS Amount " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < @ToDate " &
                    "GROUP BY Status " &
                    "ORDER BY Status"


                Using cmd As New MySqlCommand(
                    statusQuery,
                    conn
                )

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        fromDate.Date
                    )


                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        toDate.Date.AddDays(1)
                    )


                    Using reader As MySqlDataReader =
                        cmd.ExecuteReader()

                        While reader.Read()

                            Dim item As New Dictionary(
                                Of String, Object
                            )


                            item.Add(
                                "status",
                                reader("Status").ToString()
                            )


                            item.Add(
                                "orders",
                                Convert.ToInt32(
                                    reader("Orders")
                                )
                            )


                            item.Add(
                                "amount",
                                Convert.ToDecimal(
                                    reader("Amount")
                                )
                            )


                            statusSales.Add(item)

                        End While

                    End Using

                End Using


                '=================================================
                ' BUILD RESULT
                '=================================================
                Dim result As New Dictionary(
                    Of String, Object
                )


                result.Add(
                    "type",
                    "salesAnalyticsData"
                )


                result.Add(
                    "totalSales",
                    totalSales
                )


                result.Add(
                    "totalOrders",
                    totalOrders
                )


                result.Add(
                    "averageSale",
                    averageSale
                )


                result.Add(
                    "dailySales",
                    dailySales
                )


                result.Add(
                    "monthlySales",
                    monthlySales
                )


                result.Add(
                    "paymentSales",
                    paymentSales
                )


                result.Add(
                    "statusSales",
                    statusSales
                )


                result.Add(
                    "fromDate",
                    fromDate.ToString("yyyy-MM-dd")
                )


                result.Add(
                    "toDate",
                    toDate.ToString("yyyy-MM-dd")
                )

                Debug.WriteLine("========== SALES ANALYTICS ==========")
                Debug.WriteLine("From: " & fromDate.ToString("yyyy-MM-dd"))
                Debug.WriteLine("To: " & toDate.ToString("yyyy-MM-dd"))
                Debug.WriteLine("Total Sales: " & totalSales.ToString())
                Debug.WriteLine("Total Orders: " & totalOrders.ToString())
                Debug.WriteLine("Average Sale: " & averageSale.ToString())
                Debug.WriteLine("Daily Records: " & dailySales.Count.ToString())
                Debug.WriteLine("Monthly Records: " & monthlySales.Count.ToString())
                Debug.WriteLine("Payment Records: " & paymentSales.Count.ToString())
                Debug.WriteLine("Status Records: " & statusSales.Count.ToString())

                SendToWeb(result)


                SendToWeb(result)

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Sales Analytics Error:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Sales Analytics",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    '=========================================================
    ' LOAD PENDING ACCOUNT REQUESTS
    '=========================================================

    Private Sub LoadPendingAdminsToWeb()

        Try
            '-------------------------------------------------
            ' ADMIN AUTHORIZATION
            '-------------------------------------------------
            If Not Session.IsAdmin Then

                Debug.WriteLine(
                "LoadPendingAdmins: Admin access denied."
            )

                SendToWeb(New With {
                .type = "adminApprovalError",
                .message = "Admin access required."
            })

                Return
            End If

            '-------------------------------------------------
            ' DATABASE CONNECTION
            '-------------------------------------------------
            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Debug.WriteLine(
                "Connected to database: " &
                con.Database
            )

                '-------------------------------------------------
                ' LOAD PENDING ACCOUNTS
                '-------------------------------------------------
                Dim query As String =
                "SELECT UserID, Username, FullName, Role, AccountStatus " &
                "FROM users " &
                "WHERE TRIM(AccountStatus) = @Status " &
                "ORDER BY UserID DESC"

                Dim pendingAdmins As New List(
                Of Dictionary(Of String, Object)
            )()

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue(
                    "@Status",
                    "Pending"
                )

                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        While reader.Read()

                            Dim user As New Dictionary(
                            Of String, Object
                        )()

                            user("UserID") =
                            Convert.ToInt32(reader("UserID"))

                            user("Username") =
                            GetDbString(reader, "Username")

                            user("FullName") =
                            GetDbString(reader, "FullName")

                            user("Role") =
                            GetDbString(reader, "Role")

                            user("AccountStatus") =
                            GetDbString(reader, "AccountStatus")

                            pendingAdmins.Add(user)

                        End While

                    End Using

                End Using

                '-------------------------------------------------
                ' SEND DATA TO WEBVIEW2
                '-------------------------------------------------
                Debug.WriteLine(
                "Pending accounts found: " &
                pendingAdmins.Count
            )

                SendToWeb(New With {
                .type = "pendingAdmins",
                .data = pendingAdmins
            })

            End Using

        Catch ex As Exception

            Debug.WriteLine(
            "LoadPendingAdminsToWeb error: " &
            ex.ToString()
        )

            SendToWeb(New With {
            .type = "adminApprovalError",
            .message = "Unable to load pending account requests."
        })

        End Try

    End Sub


    '=========================================================
    ' APPROVE ADMIN REQUEST
    '=========================================================
    Private Sub ApproveAdmin(root As JsonElement)

        Try

            '-------------------------------------------------
            ' ADMIN AUTHORIZATION
            '-------------------------------------------------
            If Not Session.IsAdmin Then

                SendToWeb(
                New With {
                    .type = "adminApprovalError",
                    .message = "Admin access required."
                }
            )

                Return

            End If

            '-------------------------------------------------
            ' GET USER ID
            '-------------------------------------------------
            Dim userID As Integer =
            GetIntegerValue(root, "userID")

            If userID <= 0 Then

                SendToWeb(
                New With {
                    .type = "adminApprovalError",
                    .message = "Invalid user ID."
                }
            )

                Return

            End If

            '-------------------------------------------------
            ' DATABASE
            '-------------------------------------------------
            Using con As MySqlConnection =
            DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                "UPDATE users SET " &
                "AccountStatus = 'Approved', " &
                "ApprovedBy = @ApprovedBy, " &
                "ApprovedAt = NOW() " &
                "WHERE UserID = @UserID " &
                "AND AccountStatus = 'Pending'"

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue(
                    "@ApprovedBy",
                    Session.CurrentUserID
                )

                    cmd.Parameters.AddWithValue(
                    "@UserID",
                    userID
                )

                    Dim rowsAffected As Integer =
                    cmd.ExecuteNonQuery()

                    If rowsAffected = 0 Then

                        SendToWeb(
                        New With {
                            .type = "adminApprovalError",
                            .message =
                                "Request was not found or has already been processed."
                        }
                    )

                        Return

                    End If

                End Using

            End Using

            '-------------------------------------------------
            ' SUCCESS RESPONSE
            '-------------------------------------------------
            SendToWeb(
                New With {
                    .type = "adminApprovalMessage",
                    .message = "User account approved successfully."
                }
            )

        Catch ex As Exception

            SendToWeb(
            New With {
                .type = "adminApprovalError",
                .message =
                    "Unable to approve request: " &
                    ex.Message
            }
        )

        End Try

    End Sub



    Private Sub LoadUsersToWeb()

        Try
            '--------------------------------------------
            ' ADMIN AUTHORIZATION
            '--------------------------------------------
            If Not Session.IsAdmin Then

                SendToWeb(New With {
                .type = "usersError",
                .message = "Admin access required."
            })

                Return
            End If

            '--------------------------------------------
            ' DATABASE CONNECTION
            '--------------------------------------------
            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                '--------------------------------------------
                ' LOAD ALL REGISTERED USERS
                '--------------------------------------------
                Dim query As String =
                "SELECT UserID, Username, FullName, Role, AccountStatus " &
                "FROM users " &
                "ORDER BY UserID DESC"

                Dim users As New List(
                Of Dictionary(Of String, Object)
            )()

                Using cmd As New MySqlCommand(query, con)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        While reader.Read()

                            Dim user As New Dictionary(
                            Of String, Object
                        )()

                            user("UserID") =
                            Convert.ToInt32(reader("UserID"))

                            user("Username") =
                            Convert.ToString(reader("Username"))

                            user("FullName") =
                            If(reader.IsDBNull(reader.GetOrdinal("FullName")),
                               "",
                               Convert.ToString(reader("FullName")))

                            user("Role") =
                            If(reader.IsDBNull(reader.GetOrdinal("Role")),
                               "",
                               Convert.ToString(reader("Role")))

                            user("AccountStatus") =
                            Convert.ToString(reader("AccountStatus"))

                            users.Add(user)

                        End While

                    End Using
                End Using

                '--------------------------------------------
                ' SEND USERS TO USER MANAGEMENT PAGE
                '--------------------------------------------
                Debug.WriteLine(
                "LoadUsersToWeb: Total users = " &
                users.Count.ToString()
            )

                SendToWeb(New With {
                .type = "usersData",
                .data = users
            })

            End Using

        Catch ex As Exception

            Debug.WriteLine(
            "LoadUsersToWeb error: " & ex.ToString()
        )

            SendToWeb(New With {
            .type = "usersError",
            .message = "Unable to load users. Check the application log."
        })

        End Try

    End Sub



    Private Sub UpdateUser(root As JsonElement)
        Try
            If Not Session.IsAdmin Then
                SendToWeb(New With {
                .type = "userManagementError",
                .message = "Admin access required."
            })
                Return
            End If

            Dim userID As Integer
            Dim username As String = ""
            Dim fullName As String = ""
            Dim role As String = ""
            Dim accountStatus As String = ""

            If Not root.TryGetProperty("userID", Nothing) Then
                ' The fields are validated below using TryGetProperty.
            End If

            Dim element As JsonElement

            If Not root.TryGetProperty("userID", element) OrElse
            Not element.TryGetInt32(userID) OrElse userID <= 0 Then
                Throw New ArgumentException("Invalid user ID.")
            End If

            If Not root.TryGetProperty("username", element) OrElse
           element.ValueKind <> JsonValueKind.String Then
                Throw New ArgumentException("Username is required.")
            End If
            username = element.GetString().Trim()

            If Not root.TryGetProperty("fullName", element) OrElse
           element.ValueKind = JsonValueKind.Null Then
                fullName = ""
            ElseIf element.ValueKind = JsonValueKind.String Then
                fullName = element.GetString().Trim()
            Else
                Throw New ArgumentException("Invalid full name.")
            End If

            If Not root.TryGetProperty("role", element) OrElse
           element.ValueKind <> JsonValueKind.String Then
                Throw New ArgumentException("Role is required.")
            End If
            role = element.GetString().Trim()

            If Not root.TryGetProperty("accountStatus", element) OrElse
           element.ValueKind <> JsonValueKind.String Then
                Throw New ArgumentException("Account status is required.")
            End If
            accountStatus = element.GetString().Trim()

            If username.Length = 0 OrElse username.Length > 50 Then
                Throw New ArgumentException("Username must be between 1 and 50 characters.")
            End If

            If fullName.Length > 100 Then
                Throw New ArgumentException("Full name cannot exceed 100 characters.")
            End If

            If role.Length = 0 OrElse role.Length > 30 Then
                Throw New ArgumentException("Role must be between 1 and 30 characters.")
            End If

            If Not {"Pending", "Approved", "Rejected"}.Contains(accountStatus) Then
                Throw New ArgumentException("Invalid account status.")
            End If

            Using con As MySqlConnection = DBConnection.GetConnection()
                con.Open()

                Dim query As String =
                "UPDATE users SET Username = @Username, " &
                "FullName = @FullName, Role = @Role, " &
                "AccountStatus = @AccountStatus " &
                "WHERE UserID = @UserID"

                Using cmd As New MySqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@Username", username)
                    cmd.Parameters.AddWithValue("@FullName",
                    If(String.IsNullOrWhiteSpace(fullName),
                       CType(DBNull.Value, Object), fullName))
                    cmd.Parameters.AddWithValue("@Role", role)
                    cmd.Parameters.AddWithValue("@AccountStatus", accountStatus)
                    cmd.Parameters.AddWithValue("@UserID", userID)

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected = 0 Then
                        SendToWeb(New With {
                        .type = "userManagementError",
                        .message = "User not found or no changes were made."
                    })
                        Return
                    End If
                End Using
            End Using

            SendToWeb(New With {
            .type = "userManagementMessage",
            .message = "User updated successfully."
        })

        Catch ex As MySqlException When ex.Number = 1062
            SendToWeb(New With {
            .type = "userManagementError",
            .message = "That username already exists."
        })

        Catch ex As ArgumentException
            SendToWeb(New With {
            .type = "userManagementError",
            .message = ex.Message
        })

        Catch ex As Exception
            Debug.WriteLine("UpdateUser error: " & ex.ToString())
            SendToWeb(New With {
            .type = "userManagementError",
            .message = "Unable to update user."
        })
        End Try
    End Sub



    Private Sub DeleteUser(root As JsonElement)
        Try
            If Not Session.IsAdmin Then
                SendToWeb(New With {
                .type = "userManagementError",
                .message = "Admin access required."
            })
                Return
            End If

            Dim element As JsonElement
            Dim userID As Integer

            If Not root.TryGetProperty("userID", element) OrElse
           Not element.TryGetInt32(userID) OrElse userID <= 0 Then
                Throw New ArgumentException("Invalid user ID.")
            End If

            If userID = Session.CurrentUserID Then
                SendToWeb(New With {
                .type = "userManagementError",
                .message = "You cannot delete your own account."
            })
                Return
            End If

            Using con As MySqlConnection = DBConnection.GetConnection()
                con.Open()

                Dim query As String =
                "DELETE FROM users WHERE UserID = @UserID"

                Using cmd As New MySqlCommand(query, con)
                    cmd.Parameters.AddWithValue("@UserID", userID)

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    If rowsAffected = 0 Then
                        SendToWeb(New With {
                        .type = "userManagementError",
                        .message = "User not found."
                    })
                        Return
                    End If
                End Using
            End Using

            SendToWeb(New With {
            .type = "userManagementMessage",
            .message = "User deleted successfully."
        })

        Catch ex As ArgumentException
            SendToWeb(New With {
            .type = "userManagementError",
            .message = ex.Message
        })

        Catch ex As MySqlException
            Debug.WriteLine("DeleteUser database error: " & ex.ToString())
            SendToWeb(New With {
            .type = "userManagementError",
            .message = "Unable to delete user. The account may be referenced by other records."
        })

        Catch ex As Exception
            Debug.WriteLine("DeleteUser error: " & ex.ToString())
            SendToWeb(New With {
            .type = "userManagementError",
            .message = "Unable to delete user."
        })
        End Try
    End Sub



    '=========================================================
    ' REJECT PENDING ACCOUNT REQUEST
    '=========================================================
    Private Sub RejectAdmin(root As JsonElement)

        Try

            '-------------------------------------------------
            ' ADMIN AUTHORIZATION
            '-------------------------------------------------
            If Not Session.IsAdmin Then

                SendToWeb(New With {
                    .type = "adminApprovalError",
                    .message = "Admin access required."
                })

                Return

            End If


            '-------------------------------------------------
            ' GET USER ID
            '-------------------------------------------------
            Dim userID As Integer =
                GetIntegerValue(root, "userID")

            If userID <= 0 Then

                SendToWeb(New With {
                    .type = "adminApprovalError",
                    .message = "Invalid user ID."
                })

                Return

            End If


            '-------------------------------------------------
            ' UPDATE DATABASE
            '-------------------------------------------------
            Dim rowsAffected As Integer = 0

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                    "UPDATE users SET " &
                    "AccountStatus = 'Rejected' " &
                    "WHERE UserID = @UserID " &
                    "AND AccountStatus = 'Pending'"

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue(
                        "@UserID",
                        userID
                    )

                    rowsAffected = cmd.ExecuteNonQuery()

                End Using

            End Using


            '-------------------------------------------------
            ' CHECK RESULT
            '-------------------------------------------------
            If rowsAffected = 0 Then

                SendToWeb(New With {
                    .type = "adminApprovalError",
                    .message =
                        "Request was not found or has already been processed."
                })

                Return

            End If


            '-------------------------------------------------
            ' SEND SUCCESS MESSAGE
            '-------------------------------------------------
            SendToWeb(New With {
                .type = "adminApprovalMessage",
                .message = "Account request rejected successfully."
            })


            '-------------------------------------------------
            ' REFRESH PENDING REQUESTS
            '-------------------------------------------------
            LoadPendingAdminsToWeb()


        Catch ex As Exception

            Debug.WriteLine(
                "RejectAdmin error: " & ex.ToString()
            )

            SendToWeb(New With {
                .type = "adminApprovalError",
                .message =
                    "Unable to reject request: " & ex.Message
            })

        End Try

    End Sub

    '=========================================================
    ' LOGOUT
    '=========================================================
    Private Sub LogoutFromMain()

        Try
            Dim result As DialogResult =
            MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            )

            If result <> DialogResult.Yes Then
                ' Preserve the current session and screen.
                SendToWeb(New With {
                .type = "logoutCancelled"
            })
                Return
            End If

            ' Clear the VB.NET session.
            Session.ClearSession()

            ' Tell JavaScript to clear the previous user's state.
            SendToWeb(New With {
            .type = "logoutComplete"
        })

            ' Restore the login form to full-screen.
            If FrmLogin IsNot Nothing Then

                FrmLogin.FormBorderStyle = FormBorderStyle.None
                FrmLogin.StartPosition = FormStartPosition.CenterScreen
                FrmLogin.WindowState = FormWindowState.Maximized

                FrmLogin.Show()

                ' Reapply maximized state after showing.
                FrmLogin.WindowState = FormWindowState.Maximized
                FrmLogin.BringToFront()
                FrmLogin.Activate()

            End If

            ' Hide the main dashboard.
            Me.Hide()

        Catch ex As Exception
            MessageBox.Show(
            "Logout error:" &
            Environment.NewLine & Environment.NewLine &
            ex.Message,
            "Logout Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )
        End Try

    End Sub

    '=========================================================
    ' LOAD REPORTS TO WEB
    '=========================================================
    Private Sub LoadReportsToWeb(root As JsonElement)

        Try

            '=====================================================
            ' GET REPORT TYPE
            '=====================================================
            Dim reportType As String =
            GetStringValue(root, "reportType")

            If String.IsNullOrWhiteSpace(reportType) Then
                reportType = "sales"
            End If

            reportType = reportType.Trim().ToLowerInvariant()


            '=====================================================
            ' GET DATE VALUES
            '=====================================================
            Dim fromDateText As String =
            GetStringValue(root, "fromDate")

            Dim toDateText As String =
            GetStringValue(root, "toDate")

            Dim fromDate As DateTime
            Dim toDate As DateTime

            If String.IsNullOrWhiteSpace(fromDateText) OrElse
           Not DateTime.TryParse(fromDateText, fromDate) Then

                fromDate = New DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                1
            )

            End If

            If String.IsNullOrWhiteSpace(toDateText) OrElse
           Not DateTime.TryParse(toDateText, toDate) Then

                toDate = DateTime.Now.Date

            End If

            fromDate = fromDate.Date
            toDate = toDate.Date

            If fromDate > toDate Then
                Throw New Exception(
                "From date cannot be later than To date."
            )
            End If


            '=====================================================
            ' REPORT VARIABLES
            '=====================================================
            Dim reportData As New List(Of Object)()

            Dim totalRecords As Integer = 0
            Dim totalUnits As Integer = 0
            Dim totalAmount As Decimal = 0D
            Dim totalAlerts As Integer = 0


            '=====================================================
            ' DATABASE CONNECTION
            '=====================================================
            Using conn As MySqlConnection =
            DBConnection.GetConnection()

                conn.Open()


                '=================================================
                ' 1. SALES REPORT
                '=================================================
                If reportType = "sales" Then

                    Dim query As String =
                    "SELECT " &
                    "sd.SaleID, " &
                    "p.ProductName, " &
                    "sd.Quantity, " &
                    "sd.SellingPrice, " &
                    "sd.TotalPrice, " &
                    "s.SaleDate " &
                    "FROM SaleDetails sd " &
                    "INNER JOIN Sales s ON sd.SaleID = s.SaleID " &
                    "INNER JOIN Products p ON sd.ProductID = p.ProductID " &
                    "WHERE s.SaleDate >= @FromDate " &
                    "AND s.SaleDate < DATE_ADD(@ToDate, INTERVAL 1 DAY) " &
                    "ORDER BY s.SaleDate DESC, sd.SaleID DESC"

                    Using cmd As New MySqlCommand(query, conn)

                        cmd.Parameters.AddWithValue(
                        "@FromDate", fromDate
                    )

                        cmd.Parameters.AddWithValue(
                        "@ToDate", toDate
                    )

                        Using reader As MySqlDataReader =
                        cmd.ExecuteReader()

                            While reader.Read()

                                Dim saleID As Integer =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("SaleID")
                                ), 0,
                                Convert.ToInt32(reader("SaleID")))

                                Dim productName As String =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("ProductName")
                                ), "Unknown Product",
                                reader("ProductName").ToString())

                                Dim quantity As Integer =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("Quantity")
                                ), 0,
                                Convert.ToInt32(reader("Quantity")))

                                Dim sellingPrice As Decimal =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("SellingPrice")
                                ), 0D,
                                Convert.ToDecimal(reader("SellingPrice")))

                                Dim totalPrice As Decimal =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("TotalPrice")
                                ), 0D,
                                Convert.ToDecimal(reader("TotalPrice")))

                                Dim saleDate As String = ""

                                If Not reader.IsDBNull(
                                reader.GetOrdinal("SaleDate")
                            ) Then

                                    saleDate =
                                    Convert.ToDateTime(
                                        reader("SaleDate")
                                    ).ToString("yyyy-MM-dd HH:mm")

                                End If

                                reportData.Add(
                                New With {
                                    .SaleID = saleID,
                                    .ProductName = productName,
                                    .Quantity = quantity,
                                    .SellingPrice = sellingPrice,
                                    .TotalAmount = totalPrice,
                                    .SaleDate = saleDate
                                }
                            )

                                totalRecords += 1
                                totalUnits += quantity
                                totalAmount += totalPrice

                            End While

                        End Using

                    End Using


                    '=================================================
                    ' 2. INVENTORY REPORT
                    '=================================================
                ElseIf reportType = "inventory" Then

                    Dim query As String =
                    "SELECT " &
                    "p.ProductID, p.ProductCode, p.ProductName, " &
                    "p.StockQuantity, p.MinimumStock, " &
                    "p.PurchasePrice, p.SellingPrice, " &
                    "c.CategoryName " &
                    "FROM Products p " &
                    "LEFT JOIN Categories c " &
                    "ON p.CategoryID = c.CategoryID " &
                    "ORDER BY p.ProductName ASC"

                    Using cmd As New MySqlCommand(query, conn)

                        Using reader As MySqlDataReader =
                        cmd.ExecuteReader()

                            While reader.Read()

                                Dim productID As Integer =
                                Convert.ToInt32(reader("ProductID"))

                                Dim productCode As String =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("ProductCode")
                                ), "",
                                reader("ProductCode").ToString())

                                Dim productName As String =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("ProductName")
                                ), "",
                                reader("ProductName").ToString())

                                Dim categoryName As String =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("CategoryName")
                                ), "Uncategorized",
                                reader("CategoryName").ToString())

                                Dim stockQuantity As Integer =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("StockQuantity")
                                ), 0,
                                Convert.ToInt32(reader("StockQuantity")))

                                Dim minimumStock As Integer =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("MinimumStock")
                                ), 0,
                                Convert.ToInt32(reader("MinimumStock")))

                                Dim purchasePrice As Decimal =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("PurchasePrice")
                                ), 0D,
                                Convert.ToDecimal(reader("PurchasePrice")))

                                Dim sellingPrice As Decimal =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("SellingPrice")
                                ), 0D,
                                Convert.ToDecimal(reader("SellingPrice")))

                                Dim stockValue As Decimal =
                                stockQuantity * purchasePrice

                                Dim status As String = "In Stock"

                                If stockQuantity <= 0 Then

                                    status = "Out of Stock"

                                ElseIf stockQuantity <= minimumStock Then

                                    status = "Low Stock"

                                End If

                                reportData.Add(
                                New With {
                                    .ProductID = productID,
                                    .ProductCode = productCode,
                                    .ProductName = productName,
                                    .CategoryName = categoryName,
                                    .StockQuantity = stockQuantity,
                                    .MinimumStock = minimumStock,
                                    .PurchasePrice = purchasePrice,
                                    .SellingPrice = sellingPrice,
                                    .StockValue = stockValue,
                                    .Status = status
                                }
                            )

                                totalRecords += 1
                                totalUnits += stockQuantity
                                totalAmount += stockValue

                                If status <> "In Stock" Then
                                    totalAlerts += 1
                                End If

                            End While

                        End Using

                    End Using


                    '=================================================
                    ' 3. LOW STOCK REPORT
                    '=================================================
                ElseIf reportType = "low-stock" Then

                    Dim query As String =
                    "SELECT " &
                    "p.ProductID, p.ProductCode, p.ProductName, " &
                    "p.StockQuantity, p.MinimumStock, " &
                    "c.CategoryName " &
                    "FROM Products p " &
                    "LEFT JOIN Categories c " &
                    "ON p.CategoryID = c.CategoryID " &
                    "WHERE p.StockQuantity <= p.MinimumStock " &
                    "ORDER BY p.StockQuantity ASC, p.ProductName ASC"

                    Using cmd As New MySqlCommand(query, conn)

                        Using reader As MySqlDataReader =
                        cmd.ExecuteReader()

                            While reader.Read()

                                Dim productID As Integer =
                                Convert.ToInt32(reader("ProductID"))

                                Dim productCode As String =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("ProductCode")
                                ), "",
                                reader("ProductCode").ToString())

                                Dim productName As String =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("ProductName")
                                ), "",
                                reader("ProductName").ToString())

                                Dim categoryName As String =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("CategoryName")
                                ), "Uncategorized",
                                reader("CategoryName").ToString())

                                Dim stockQuantity As Integer =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("StockQuantity")
                                ), 0,
                                Convert.ToInt32(reader("StockQuantity")))

                                Dim minimumStock As Integer =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("MinimumStock")
                                ), 0,
                                Convert.ToInt32(reader("MinimumStock")))

                                reportData.Add(
                                New With {
                                    .ProductID = productID,
                                    .ProductCode = productCode,
                                    .ProductName = productName,
                                    .CategoryName = categoryName,
                                    .StockQuantity = stockQuantity,
                                    .MinimumStock = minimumStock
                                }
                            )

                                totalRecords += 1
                                totalUnits += stockQuantity
                                totalAlerts += 1

                            End While

                        End Using

                    End Using


                    '=================================================
                    ' 4. PRODUCT PERFORMANCE REPORT
                    '=================================================
                ElseIf reportType = "product-performance" Then

                    Dim temporaryData As New List(
                    Of ProductAnalysisTemp
                )()

                    Dim query As String =
                    "SELECT " &
                    "p.ProductID, p.ProductCode, p.ProductName, " &
                    "p.StockQuantity, p.SellingPrice, " &
                    "c.CategoryName, " &
                    "COALESCE(SUM(sd.Quantity), 0) AS UnitsSold, " &
                    "COALESCE(SUM(sd.TotalPrice), 0) AS TotalSales " &
                    "FROM Products p " &
                    "LEFT JOIN Categories c " &
                    "ON p.CategoryID = c.CategoryID " &
                    "LEFT JOIN SaleDetails sd " &
                    "ON p.ProductID = sd.ProductID " &
                    "GROUP BY p.ProductID, p.ProductCode, " &
                    "p.ProductName, p.StockQuantity, " &
                    "p.SellingPrice, c.CategoryName " &
                    "ORDER BY TotalSales DESC, p.ProductName ASC"

                    Using cmd As New MySqlCommand(query, conn)

                        Using reader As MySqlDataReader =
                        cmd.ExecuteReader()

                            While reader.Read()

                                Dim item As New ProductAnalysisTemp()

                                item.ProductID =
                                Convert.ToInt32(reader("ProductID"))

                                item.ProductCode =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("ProductCode")
                                ), "",
                                reader("ProductCode").ToString())

                                item.ProductName =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("ProductName")
                                ), "",
                                reader("ProductName").ToString())

                                item.CategoryName =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("CategoryName")
                                ), "Uncategorized",
                                reader("CategoryName").ToString())

                                item.StockQuantity =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("StockQuantity")
                                ), 0,
                                Convert.ToInt32(reader("StockQuantity")))

                                item.SellingPrice =
                                If(reader.IsDBNull(
                                    reader.GetOrdinal("SellingPrice")
                                ), 0D,
                                Convert.ToDecimal(reader("SellingPrice")))

                                item.UnitsSold =
                                Convert.ToInt32(reader("UnitsSold"))

                                item.TotalSales =
                                Convert.ToDecimal(reader("TotalSales"))

                                temporaryData.Add(item)

                            End While

                        End Using

                    End Using


                    '---------------------------------------------
                    ' FIND HIGHEST UNITS SOLD
                    '---------------------------------------------
                    Dim highestUnitsSold As Integer = 0

                    For Each item As ProductAnalysisTemp _
                    In temporaryData

                        If item.UnitsSold > highestUnitsSold Then
                            highestUnitsSold = item.UnitsSold
                        End If

                    Next


                    '---------------------------------------------
                    ' BUILD PERFORMANCE DATA
                    '---------------------------------------------
                    For Each item As ProductAnalysisTemp _
                    In temporaryData

                        Dim performance As String = "never"

                        If item.UnitsSold <= 0 Then

                            performance = "never"

                        ElseIf highestUnitsSold > 0 Then

                            Dim percentage As Double =
                            (CDbl(item.UnitsSold) /
                             CDbl(highestUnitsSold)) * 100

                            If percentage >= 70 Then

                                performance = "top"

                            ElseIf percentage >= 30 Then

                                performance = "medium"

                            Else

                                performance = "low"

                            End If

                        End If

                        reportData.Add(
                        New With {
                            .ProductID = item.ProductID,
                            .ProductCode = item.ProductCode,
                            .ProductName = item.ProductName,
                            .CategoryName = item.CategoryName,
                            .StockQuantity = item.StockQuantity,
                            .SellingPrice = item.SellingPrice,
                            .UnitsSold = item.UnitsSold,
                            .TotalSales = item.TotalSales,
                            .Performance = performance
                        }
                    )

                        totalRecords += 1
                        totalUnits += item.UnitsSold
                        totalAmount += item.TotalSales

                    Next


                Else

                    Throw New Exception(
                    "Invalid report type: " & reportType
                )

                End If


                '=================================================
                ' SEND REPORT TO WEBVIEW2
                '=================================================
                SendToWeb(
                New With {
                    .type = "reports",
                    .reportType = reportType,
                    .data = reportData,
                    .summary = New With {
                        .totalRecords = totalRecords,
                        .totalUnits = totalUnits,
                        .totalAmount = totalAmount,
                        .totalAlerts = totalAlerts
                    }
                }
            )

            End Using


        Catch ex As Exception

            Debug.WriteLine(
            "LoadReportsToWeb error: " & ex.ToString()
        )

            SendToWeb(
            New With {
                .type = "reportsError",
                .message =
                    "Unable to generate report: " &
                    ex.Message
            }
        )

        End Try

    End Sub

    Private Function IsStaffUser() As Boolean

        Return String.Equals(
            Session.CurrentRole,
            "Staff",
            StringComparison.OrdinalIgnoreCase
        )

    End Function



    Private Function IsStaffRestrictedAction(action As String) As Boolean

        Select Case action

            Case "addProduct", "updateProduct", "deleteProduct",
             "loadCategories", "addCategory", "deleteCategory",
             "addSupplier",
             "loadProductAnalysis", "loadSalesAnalytics", "loadReports",
             "loadPendingAdmins", "approveAdmin", "rejectAdmin",
             "loadUsers", "updateUser", "deleteUser"

                Return True

            Case Else
                Return False

        End Select

    End Function


    Private Sub SendCurrentUserToWeb()

        Dim response = New With {
        .type = "currentUser",
        .userID = Session.CurrentUserID,
        .username = If(Session.CurrentUsername, ""),
        .role = If(Session.CurrentRole, ""),
        .fullName = If(Session.CurrentFullName, "")
    }

        Dim json As String =
        System.Text.Json.JsonSerializer.Serialize(response)

        Debug.WriteLine("Sending current-user response:")
        Debug.WriteLine(json)

        SendToWeb(response)

    End Sub

End Class