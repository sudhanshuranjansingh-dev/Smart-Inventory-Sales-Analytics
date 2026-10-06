Imports Microsoft.Web.WebView2.Core
Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Text.Json

Public Class FrmMain

    '=========================================================
    ' FORM LOAD
    '=========================================================
    Private Async Sub FrmMain_Load(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Load

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

            Dim json As String =
            e.WebMessageAsJson


            Using document As JsonDocument =
            JsonDocument.Parse(json)

                Dim root As JsonElement =
                document.RootElement


                '=================================================
                ' STRING MESSAGE
                '=================================================
                If root.ValueKind =
                JsonValueKind.String Then

                    Dim stringAction As String =
                    root.GetString()

                    Select Case stringAction

                        Case "logout"

                            LogoutFromMain()

                    End Select

                    Return

                End If


                '=================================================
                ' OBJECT MESSAGE
                '=================================================
                If root.ValueKind <>
                JsonValueKind.Object Then

                    Return

                End If


                '-------------------------------------------------
                ' GET ACTION
                '-------------------------------------------------
                Dim actionElement As JsonElement

                If Not root.TryGetProperty(
                "action",
                actionElement
            ) Then

                    Return

                End If


                If actionElement.ValueKind <>
                JsonValueKind.String Then

                    Return

                End If


                Dim action As String =
                actionElement.GetString()


                '=================================================
                ' PROCESS ACTION
                '=================================================
                Select Case action

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

                        LoadPurchasesToWeb()


                    Case "addPurchase"

                        AddPurchase(root)

                    Case "loadSales"
                        LoadSalesToWeb()

                    Case "addSale"
                        AddSale(root)


                    Case "addProduct"

                        AddProduct(root)


                    Case "updateProduct"

                        UpdateProduct(root)


                    Case "deleteProduct"

                        DeleteProduct(root)


                    Case "loadSalesAnalytics"

                        LoadSalesAnalyticsToWeb(root)


                    Case "loadPendingAdmins"

                        LoadPendingAdminsToWeb()


                    Case "approveAdmin"

                        ApproveAdmin(root)


                    Case "rejectAdmin"

                        RejectAdmin(root)


                    Case "logout"

                        LogoutFromMain()


                    Case Else

                        MessageBox.Show(
                        "Unknown WebUI action: " & action,
                        "WebUI",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    )

                End Select

            End Using


        Catch ex As Exception

            MessageBox.Show(
            "WebUI communication error:" &
            Environment.NewLine &
            Environment.NewLine &
            ex.Message,
            "WebUI Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

        End Try

    End Sub


    Private Sub LoadSalesToWeb()

        Try

            Using conn As MySqlConnection = DBConnection.GetConnection()

                conn.Open()

                Dim sales As New List(Of Object)

                Dim query As String =
                "SELECT " &
                "sd.SaleID, " &
                "p.ProductName, " &
                "sd.Quantity, " &
                "sd.SellingPrice, " &
                "sd.TotalPrice, " &
                "s.SaleDate " &
                "FROM saledetails sd " &
                "INNER JOIN sales s ON sd.SaleID = s.SaleID " &
                "INNER JOIN products p ON sd.ProductID = p.ProductID " &
                "ORDER BY s.SaleDate DESC"

                Using cmd As New MySqlCommand(query, conn)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        While reader.Read()

                            sales.Add(New With {
                            .SaleID = Convert.ToInt32(reader("SaleID")),
                            .ProductName = reader("ProductName").ToString(),
                            .Quantity = Convert.ToInt32(reader("Quantity")),
                            .SellingPrice = Convert.ToDecimal(reader("SellingPrice")),
                            .TotalAmount = Convert.ToDecimal(reader("TotalPrice")),
                            .SaleDate = Convert.ToDateTime(reader("SaleDate")).ToString("yyyy-MM-dd HH:mm")
                        })

                        End While

                    End Using

                End Using

                SendToWeb(New With {
                .type = "sales",
                .data = sales
            })

            End Using

        Catch ex As Exception

            SendToWeb(New With {
            .type = "saleError",
            .message = ex.Message
        })

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
            Dim productID As Integer =
            GetIntegerValue(
                root,
                "ProductID"
            )

            Dim quantity As Integer =
            GetIntegerValue(
                root,
                "Quantity"
            )

            Dim sellingPrice As Decimal =
            GetDecimalValue(
                root,
                "SellingPrice"
            )


            '-------------------------------------------------
            ' VALIDATION
            '-------------------------------------------------
            If productID <= 0 Then

                SendToWeb(
                New With {
                    .type = "saleError",
                    .message = "Please select a product."
                }
            )

                Return

            End If


            If quantity <= 0 Then

                SendToWeb(
                New With {
                    .type = "saleError",
                    .message =
                        "Quantity must be greater than zero."
                }
            )

                Return

            End If


            If sellingPrice <= 0D Then

                SendToWeb(
                New With {
                    .type = "saleError",
                    .message =
                        "Selling price must be greater than zero."
                }
            )

                Return

            End If


            '-------------------------------------------------
            ' CALCULATE TOTAL
            '-------------------------------------------------
            Dim totalPrice As Decimal =
            quantity * sellingPrice


            '-------------------------------------------------
            ' DATABASE CONNECTION
            '-------------------------------------------------
            Using con As MySqlConnection =
            DBConnection.GetConnection()

                con.Open()


                '-------------------------------------------------
                ' START TRANSACTION
                '-------------------------------------------------
                transaction =
                con.BeginTransaction()


                '=================================================
                ' 1. GET CURRENT PRODUCT STOCK
                '=================================================
                Dim previousStock As Integer = 0


                Dim productQuery As String =
                "SELECT StockQuantity " &
                "FROM Products " &
                "WHERE ProductID = @ProductID " &
                "FOR UPDATE"


                Using cmd As New MySqlCommand(
                productQuery,
                con,
                transaction
            )

                    cmd.Parameters.AddWithValue(
                    "@ProductID",
                    productID
                )


                    Dim result As Object =
                    cmd.ExecuteScalar()


                    If result Is Nothing OrElse
                   result Is DBNull.Value Then

                        Throw New Exception(
                        "Selected product was not found."
                    )

                    End If


                    previousStock =
                    Convert.ToInt32(result)

                End Using


                '=================================================
                ' 2. CHECK AVAILABLE STOCK
                '=================================================
                If quantity > previousStock Then

                    Throw New Exception(
                    "Insufficient stock. Available stock: " &
                    previousStock.ToString()
                )

                End If


                '=================================================
                ' 3. CALCULATE NEW STOCK
                '=================================================
                Dim newStock As Integer =
                previousStock - quantity


                '=================================================
                ' 4. INSERT SALE HEADER
                '=================================================



                Dim saleQuery As String =
                "INSERT INTO Sales " &
                "(CustomerName, PaymentMethod, TotalAmount, Status) " &
                "VALUES " &
                "(@CustomerName, @PaymentMethod, " &
                "@TotalAmount, @Status)"


                Using cmd As New MySqlCommand(
                saleQuery,
                con,
                transaction
            )

                    cmd.Parameters.AddWithValue(
                    "@CustomerName",
                    DBNull.Value
                )

                    cmd.Parameters.AddWithValue(
                    "@PaymentMethod",
                    "Cash"
                )

                    cmd.Parameters.AddWithValue(
                    "@TotalAmount",
                    totalPrice
                )

                    cmd.Parameters.AddWithValue(
                    "@Status",
                    "Completed"
                )


                    cmd.ExecuteNonQuery()


                    saleID =
                    Convert.ToInt32(
                        cmd.LastInsertedId
                    )

                End Using


                '=================================================
                ' 5. INSERT SALE DETAIL
                '=================================================
                Dim detailQuery As String =
                "INSERT INTO SaleDetails " &
                "(SaleID, ProductID, Quantity, " &
                "SellingPrice, TotalPrice) " &
                "VALUES " &
                "(@SaleID, @ProductID, @Quantity, " &
                "@SellingPrice, @TotalPrice)"


                Using cmd As New MySqlCommand(
                detailQuery,
                con,
                transaction
            )

                    cmd.Parameters.AddWithValue(
                    "@SaleID",
                    saleID
                )

                    cmd.Parameters.AddWithValue(
                    "@ProductID",
                    productID
                )

                    cmd.Parameters.AddWithValue(
                    "@Quantity",
                    quantity
                )

                    cmd.Parameters.AddWithValue(
                    "@SellingPrice",
                    sellingPrice
                )

                    cmd.Parameters.AddWithValue(
                    "@TotalPrice",
                    totalPrice
                )


                    cmd.ExecuteNonQuery()

                End Using


                '=================================================
                ' 6. UPDATE PRODUCT STOCK
                '=================================================
                Dim stockQuery As String =
                "UPDATE Products SET " &
                "StockQuantity = @NewStock " &
                "WHERE ProductID = @ProductID"


                Using cmd As New MySqlCommand(
                stockQuery,
                con,
                transaction
            )

                    cmd.Parameters.AddWithValue(
                    "@NewStock",
                    newStock
                )

                    cmd.Parameters.AddWithValue(
                    "@ProductID",
                    productID
                )


                    Dim rowsAffected As Integer =
                    cmd.ExecuteNonQuery()


                    If rowsAffected = 0 Then

                        Throw New Exception(
                        "Product stock could not be updated."
                    )

                    End If

                End Using


                '=================================================
                ' 7. INSERT STOCK HISTORY
                '=================================================
                Dim historyQuery As String =
                "INSERT INTO StockHistory " &
                "(ProductID, ChangeType, Quantity, " &
                "PreviousStock, NewStock, ChangeDate) " &
                "VALUES " &
                "(@ProductID, @ChangeType, @Quantity, " &
                "@PreviousStock, @NewStock, NOW())"


                Using cmd As New MySqlCommand(
                historyQuery,
                con,
                transaction
            )

                    cmd.Parameters.AddWithValue(
                    "@ProductID",
                    productID
                )

                    cmd.Parameters.AddWithValue(
                    "@ChangeType",
                    "SALE"
                )

                    cmd.Parameters.AddWithValue(
                    "@Quantity",
                    quantity
                )

                    cmd.Parameters.AddWithValue(
                    "@PreviousStock",
                    previousStock
                )

                    cmd.Parameters.AddWithValue(
                    "@NewStock",
                    newStock
                )


                    cmd.ExecuteNonQuery()

                End Using


                '=================================================
                ' 8. COMMIT TRANSACTION
                '=================================================
                transaction.Commit()

                transaction = Nothing

            End Using


            '=================================================
            ' SUCCESS MESSAGE
            '=================================================
            SendToWeb(
            New With {
                .type = "saleAdded",
                .saleID = saleID,
                .message =
                    "Sale completed successfully."
            }
        )


            '=================================================
            ' REFRESH SALES
            '=================================================
            LoadSalesToWeb()


            '=================================================
            ' REFRESH PRODUCTS / STOCK
            '=================================================
            LoadProductsToWeb()


        Catch ex As Exception

            '-------------------------------------------------
            ' ROLLBACK
            '-------------------------------------------------
            Try

                If transaction IsNot Nothing Then

                    transaction.Rollback()

                End If

            Catch

                ' Ignore rollback errors

            End Try


            '-------------------------------------------------
            ' SEND ERROR TO WEB UI
            '-------------------------------------------------
            SendToWeb(
            New With {
                .type = "saleError",
                .message =
                    "Unable to complete sale: " &
                    ex.Message
            }
        )

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
                "SELECT CategoryID, CategoryName " &
                "FROM Categories " &
                "ORDER BY CategoryName"


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

                            Dim purchase As New Dictionary(
                            Of String, Object
                        )()

                            purchase("PurchaseID") =
                            GetDbInt(
                                reader,
                                "PurchaseID"
                            )

                            purchase("SupplierID") =
                            GetDbNullableInt(
                                reader,
                                "SupplierID"
                            )

                            purchase("SupplierName") =
                            GetDbString(
                                reader,
                                "SupplierName"
                            )

                            If IsDBNull(
                            reader("PurchaseDate")
                        ) Then

                                purchase("PurchaseDate") = ""

                            Else

                                purchase("PurchaseDate") =
                                Convert.ToDateTime(
                                    reader("PurchaseDate")
                                ).ToString(
                                    "yyyy-MM-dd HH:mm:ss"
                                )

                            End If

                            purchase("TotalAmount") =
                            GetDbDecimal(
                                reader,
                                "TotalAmount"
                            )

                            purchases.Add(purchase)

                        End While

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

            SendToWeb(
            New With {
                .type = "purchaseError",
                .message =
                    "Error loading purchases: " &
                    ex.Message
            }
        )

        End Try

    End Sub

    '=========================================================
    ' ADD PURCHASE
    '=========================================================
    Private Sub AddPurchase(root As JsonElement)

        Dim transaction As MySqlTransaction = Nothing
        Dim purchaseID As Integer = 0

        Try

            '-------------------------------------------------
            ' GET VALUES
            '-------------------------------------------------

            Dim supplierID As Integer =
            GetIntegerValue(
                root,
                "SupplierID"
            )

            Dim productID As Integer =
            GetIntegerValue(
                root,
                "ProductID"
            )

            Dim quantity As Integer =
            GetIntegerValue(
                root,
                "Quantity"
            )

            Dim purchasePrice As Decimal =
            GetDecimalValue(
                root,
                "PurchasePrice"
            )


            '-------------------------------------------------
            ' VALIDATION
            '-------------------------------------------------

            If supplierID <= 0 Then

                SendToWeb(
                New With {
                    .type = "purchaseError",
                    .message = "Please select a supplier."
                }
            )

                Return

            End If


            If productID <= 0 Then

                SendToWeb(
                New With {
                    .type = "purchaseError",
                    .message = "Please select a product."
                }
            )

                Return

            End If


            If quantity <= 0 Then

                SendToWeb(
                New With {
                    .type = "purchaseError",
                    .message =
                        "Quantity must be greater than zero."
                }
            )

                Return

            End If


            If purchasePrice <= 0D Then

                SendToWeb(
                New With {
                    .type = "purchaseError",
                    .message =
                        "Purchase price must be greater than zero."
                }
            )

                Return

            End If


            '-------------------------------------------------
            ' CALCULATE TOTAL
            '-------------------------------------------------

            Dim totalPrice As Decimal =
            quantity * purchasePrice


            '-------------------------------------------------
            ' DATABASE CONNECTION
            '-------------------------------------------------

            Using con As MySqlConnection =
            DBConnection.GetConnection()

                con.Open()


                '-------------------------------------------------
                ' START TRANSACTION
                '-------------------------------------------------

                transaction =
                con.BeginTransaction()


                '=================================================
                ' 1. VERIFY SUPPLIER
                '=================================================

                Dim supplierQuery As String =
                "SELECT COUNT(*) " &
                "FROM Suppliers " &
                "WHERE SupplierID = @SupplierID"

                Using cmd As New MySqlCommand(
                supplierQuery,
                con,
                transaction
            )

                    cmd.Parameters.AddWithValue(
                    "@SupplierID",
                    supplierID
                )

                    Dim supplierExists As Integer =
                    Convert.ToInt32(
                        cmd.ExecuteScalar()
                    )

                    If supplierExists = 0 Then

                        Throw New Exception(
                        "Selected supplier was not found."
                    )

                    End If

                End Using


                '=================================================
                ' 2. GET PRODUCT AND CURRENT STOCK
                '=================================================

                Dim previousStock As Integer = 0

                Dim productQuery As String =
                "SELECT StockQuantity " &
                "FROM Products " &
                "WHERE ProductID = @ProductID " &
                "FOR UPDATE"

                Using cmd As New MySqlCommand(
                productQuery,
                con,
                transaction
            )

                    cmd.Parameters.AddWithValue(
                    "@ProductID",
                    productID
                )

                    Dim result As Object =
                    cmd.ExecuteScalar()

                    If result Is Nothing OrElse
                   result Is DBNull.Value Then

                        Throw New Exception(
                        "Selected product was not found."
                    )

                    End If

                    previousStock =
                    Convert.ToInt32(result)

                End Using


                '=================================================
                ' 3. CALCULATE NEW STOCK
                '=================================================

                Dim newStock As Integer =
                previousStock + quantity


                '=================================================
                ' 4. INSERT PURCHASE HEADER
                '=================================================

                Dim purchaseQuery As String =
                "INSERT INTO Purchases " &
                "(SupplierID, PurchaseDate, TotalAmount) " &
                "VALUES " &
                "(@SupplierID, NOW(), @TotalAmount)"

                Using cmd As New MySqlCommand(
                purchaseQuery,
                con,
                transaction
            )

                    cmd.Parameters.AddWithValue(
                    "@SupplierID",
                    supplierID
                )

                    cmd.Parameters.AddWithValue(
                    "@TotalAmount",
                    totalPrice
                )

                    cmd.ExecuteNonQuery()

                    purchaseID =
                    Convert.ToInt32(
                        cmd.LastInsertedId
                    )

                End Using


                '=================================================
                ' 5. INSERT PURCHASE DETAIL
                '=================================================

                Dim detailQuery As String =
                "INSERT INTO PurchaseDetails " &
                "(PurchaseID, ProductID, Quantity, " &
                "PurchasePrice, TotalPrice) " &
                "VALUES " &
                "(@PurchaseID, @ProductID, @Quantity, " &
                "@PurchasePrice, @TotalPrice)"

                Using cmd As New MySqlCommand(
                detailQuery,
                con,
                transaction
            )

                    cmd.Parameters.AddWithValue(
                    "@PurchaseID",
                    purchaseID
                )

                    cmd.Parameters.AddWithValue(
                    "@ProductID",
                    productID
                )

                    cmd.Parameters.AddWithValue(
                    "@Quantity",
                    quantity
                )

                    cmd.Parameters.AddWithValue(
                    "@PurchasePrice",
                    purchasePrice
                )

                    cmd.Parameters.AddWithValue(
                    "@TotalPrice",
                    totalPrice
                )

                    cmd.ExecuteNonQuery()

                End Using


                '=================================================
                ' 6. UPDATE PRODUCT STOCK
                '=================================================

                Dim stockQuery As String =
                "UPDATE Products SET " &
                "StockQuantity = @NewStock, " &
                "PurchasePrice = @PurchasePrice " &
                "WHERE ProductID = @ProductID"

                Using cmd As New MySqlCommand(
                stockQuery,
                con,
                transaction
            )

                    cmd.Parameters.AddWithValue(
                    "@NewStock",
                    newStock
                )

                    cmd.Parameters.AddWithValue(
                    "@PurchasePrice",
                    purchasePrice
                )

                    cmd.Parameters.AddWithValue(
                    "@ProductID",
                    productID
                )

                    Dim rowsAffected As Integer =
                    cmd.ExecuteNonQuery()

                    If rowsAffected = 0 Then

                        Throw New Exception(
                        "Product stock could not be updated."
                    )

                    End If

                End Using


                '=================================================
                ' 7. INSERT STOCK HISTORY
                '=================================================

                Dim historyQuery As String =
                "INSERT INTO StockHistory " &
                "(ProductID, ChangeType, Quantity, " &
                "PreviousStock, NewStock, ChangeDate) " &
                "VALUES " &
                "(@ProductID, @ChangeType, @Quantity, " &
                "@PreviousStock, @NewStock, NOW())"

                Using cmd As New MySqlCommand(
                historyQuery,
                con,
                transaction
            )

                    cmd.Parameters.AddWithValue(
                    "@ProductID",
                    productID
                )

                    cmd.Parameters.AddWithValue(
                    "@ChangeType",
                    "PURCHASE"
                )

                    cmd.Parameters.AddWithValue(
                    "@Quantity",
                    quantity
                )

                    cmd.Parameters.AddWithValue(
                    "@PreviousStock",
                    previousStock
                )

                    cmd.Parameters.AddWithValue(
                    "@NewStock",
                    newStock
                )

                    cmd.ExecuteNonQuery()

                End Using


                '=================================================
                ' 8. COMMIT TRANSACTION
                '=================================================

                transaction.Commit()

                transaction = Nothing

            End Using


            '=================================================
            ' SUCCESS RESPONSE
            '=================================================

            SendToWeb(
            New With {
                .type = "purchaseAdded",
                .message =
                    "Purchase added successfully.",
                .purchaseID = purchaseID,
                .totalAmount = totalPrice
            }
        )


            '=================================================
            ' REFRESH WEB UI DATA
            '=================================================

            LoadPurchasesToWeb()

            LoadProductsToWeb()

            LoadSuppliersToWeb()


        Catch ex As Exception

            '=================================================
            ' ROLLBACK TRANSACTION
            '=================================================

            Try

                If transaction IsNot Nothing Then

                    transaction.Rollback()

                End If

            Catch

                ' Ignore rollback errors

            End Try


            '=================================================
            ' SEND ERROR TO WEB UI
            '=================================================

            SendToWeb(
            New With {
                .type = "purchaseError",
                .message =
                    "Unable to add purchase: " &
                    ex.Message
            }
        )

        End Try

    End Sub


    '=========================================================
    ' SEND DATA TO WEB UI
    '=========================================================
    Private Sub SendToWeb(data As Object)

        Try

            '-------------------------------------------------
            ' Make sure WebView2 is ready
            '-------------------------------------------------
            If WebView21.CoreWebView2 Is Nothing Then
                Return
            End If


            '-------------------------------------------------
            ' Convert VB.NET object to JSON
            '-------------------------------------------------
            Dim json As String =
            JsonSerializer.Serialize(data)


            '-------------------------------------------------
            ' Send message to top-level WebUI
            '-------------------------------------------------
            WebView21.CoreWebView2.PostWebMessageAsJson(
            json
        )


            '-------------------------------------------------
            ' Send message to current iframe
            '-------------------------------------------------
            Dim script As String =
            "(() => {" &
            "const frame = document.getElementById('page-frame');" &
            "if (frame && frame.contentWindow) {" &
            "frame.contentWindow.postMessage(" &
            json &
            ", '*');" &
            "}" &
            "})();"


            WebView21.CoreWebView2.ExecuteScriptAsync(
            script
        )


        Catch ex As Exception

            MessageBox.Show(
            "Error sending data to WebUI:" &
            Environment.NewLine &
            Environment.NewLine &
            ex.Message,
            "WebUI Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

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
                    "action",
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
    ' LOAD PENDING ADMIN REQUESTS
    '=========================================================
    Private Sub LoadPendingAdminsToWeb()

        Try

            '-------------------------------------------------
            ' Check Admin authorization
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
            ' Open database connection
            '-------------------------------------------------
            Using con As MySqlConnection =
            DBConnection.GetConnection()

                con.Open()


                '-------------------------------------------------
                ' Query pending Admin requests
                '-------------------------------------------------
                Dim query As String =
                "SELECT " &
                "UserID, " &
                "Username, " &
                "FullName, " &
                "Role, " &
                "AccountStatus " &
                "FROM Users " &
                "WHERE Role = 'Admin' " &
                "AND AccountStatus = 'Pending' " &
                "ORDER BY UserID DESC"


                Using cmd As New MySqlCommand(
                query,
                con
            )

                    Using reader As MySqlDataReader =
                    cmd.ExecuteReader()

                        Dim pendingAdmins As New List(
                        Of Dictionary(Of String, Object)
                    )()


                        '-------------------------------------------------
                        ' Read database records
                        '-------------------------------------------------
                        While reader.Read()

                            Dim user As New Dictionary(
                            Of String, Object
                        )()


                            user("UserID") =
                            Convert.ToInt32(
                                reader("UserID")
                            )


                            user("Username") =
                            reader("Username").ToString()


                            user("FullName") =
                            reader("FullName").ToString()


                            user("Role") =
                            reader("Role").ToString()


                            user("AccountStatus") =
                            reader("AccountStatus").ToString()


                            pendingAdmins.Add(user)

                        End While


                        '-------------------------------------------------
                        ' Send result to Admin Approval WebUI
                        '-------------------------------------------------
                        SendToWeb(
                        New With {
                            .type = "pendingAdmins",
                            .data = pendingAdmins
                        }
                    )

                    End Using

                End Using

            End Using


        Catch ex As Exception

            '-------------------------------------------------
            ' Send error to WebUI
            '-------------------------------------------------
            SendToWeb(
            New With {
                .type = "adminApprovalError",
                .message =
                    "Unable to load pending Admin requests: " &
                    ex.Message
            }
        )

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
            GetIntegerValue(
                root,
                "userID"
            )


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
                "UPDATE Users SET " &
                "AccountStatus = 'Approved', " &
                "ApprovedBy = @ApprovedBy, " &
                "ApprovedAt = NOW() " &
                "WHERE UserID = @UserID " &
                "AND Role = 'Admin' " &
                "AND AccountStatus = 'Pending'"


                Using cmd As New MySqlCommand(
                query,
                con
            )

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


            SendToWeb(
            New With {
                .type = "adminApprovalMessage",
                .message =
                    "Admin request approved successfully."
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

    '=========================================================
    ' REJECT ADMIN REQUEST
    '=========================================================
    Private Sub RejectAdmin(root As JsonElement)

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
            GetIntegerValue(
                root,
                "userID"
            )


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
                "UPDATE Users SET " &
                "AccountStatus = 'Rejected' " &
                "WHERE UserID = @UserID " &
                "AND Role = 'Admin' " &
                "AND AccountStatus = 'Pending'"


                Using cmd As New MySqlCommand(
                query,
                con
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


            SendToWeb(
            New With {
                .type = "adminApprovalMessage",
                .message =
                    "Admin request rejected."
            }
        )


        Catch ex As Exception

            SendToWeb(
            New With {
                .type = "adminApprovalError",
                .message =
                    "Unable to reject request: " &
                    ex.Message
            }
        )

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
                Return
            End If


            '-------------------------------------------------
            ' Clear current user session
            '-------------------------------------------------
            Session.ClearSession()


            '-------------------------------------------------
            ' Hide Main Form
            '-------------------------------------------------
            Me.Hide()


            '-------------------------------------------------
            ' Show Login Form
            '-------------------------------------------------
            If FrmLogin IsNot Nothing Then

                FrmLogin.Show()

                FrmLogin.WindowState =
                    FormWindowState.Normal

                FrmLogin.BringToFront()

                FrmLogin.Activate()

            End If


        Catch ex As Exception

            MessageBox.Show(
                "Logout error:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Logout Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

End Class