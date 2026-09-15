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
            ' Register JavaScript -> VB.NET communication
            '-------------------------------------------------
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

            '-------------------------------------------------
            ' Get JSON message
            '-------------------------------------------------
            Dim json As String =
                e.WebMessageAsJson


            '-------------------------------------------------
            ' Parse JSON
            '-------------------------------------------------
            Using document As JsonDocument =
                JsonDocument.Parse(json)

                Dim root As JsonElement =
                    document.RootElement


                '-------------------------------------------------
                ' Get action safely
                '-------------------------------------------------
                Dim actionElement As JsonElement


                If Not root.TryGetProperty(
                    "action",
                    actionElement
                ) Then

                    Return

                End If


                Dim action As String =
                    actionElement.GetString()


                '-------------------------------------------------
                ' Process action
                '-------------------------------------------------
                Select Case action


                    Case "loadProducts"

                        LoadProductsToWeb()


                    Case "loadCategories"

                        LoadCategoriesToWeb()


                    Case "loadSuppliers"

                        LoadSuppliersToWeb()


                    Case "addProduct"

                        AddProduct(root)


                    Case "updateProduct"

                        UpdateProduct(root)


                    Case "deleteProduct"

                        DeleteProduct(root)


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


    '=========================================================
    ' LOAD PRODUCTS
    '=========================================================
    Private Sub LoadProductsToWeb()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

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
                "LEFT JOIN Categories c ON p.CategoryID = c.CategoryID " &
                "LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID " &
                "ORDER BY p.ProductID DESC"

                Using cmd As New MySqlCommand(query, con)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        Dim products As New List(Of Dictionary(Of String, Object))()

                        While reader.Read()

                            ' Create product dictionary
                            Dim product As New Dictionary(Of String, Object)()

                            product.Add(
                            "ProductID",
                            GetDbInt(reader, "ProductID")
                        )

                            product.Add(
                            "ProductCode",
                            GetDbString(reader, "ProductCode")
                        )

                            product.Add(
                            "ProductName",
                            GetDbString(reader, "ProductName")
                        )

                            product.Add(
                            "CategoryID",
                            GetDbNullableInt(reader, "CategoryID")
                        )

                            product.Add(
                            "CategoryName",
                            GetDbString(reader, "CategoryName")
                        )

                            product.Add(
                            "SupplierID",
                            GetDbNullableInt(reader, "SupplierID")
                        )

                            product.Add(
                            "SupplierName",
                            GetDbString(reader, "SupplierName")
                        )

                            product.Add(
                            "PurchasePrice",
                            GetDbDecimal(reader, "PurchasePrice")
                        )

                            product.Add(
                            "SellingPrice",
                            GetDbDecimal(reader, "SellingPrice")
                        )

                            product.Add(
                            "StockQuantity",
                            GetDbInt(reader, "StockQuantity")
                        )

                            product.Add(
                            "MinimumStock",
                            GetDbInt(reader, "MinimumStock", 10)
                        )

                            products.Add(product)

                        End While

                        '-------------------------------------------------
                        ' Send products to JavaScript
                        '-------------------------------------------------
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

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                "SELECT CategoryID, CategoryName " &
                "FROM Categories " &
                "ORDER BY CategoryName"

                Using cmd As New MySqlCommand(query, con)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        Dim categories As New List(Of Dictionary(Of String, Object))()

                        While reader.Read()

                            Dim category As New Dictionary(Of String, Object)()

                            category.Add(
                            "CategoryID",
                            Convert.ToInt32(reader("CategoryID"))
                        )

                            category.Add(
                            "CategoryName",
                            reader("CategoryName").ToString()
                        )

                            categories.Add(category)

                        End While

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
    ' LOAD SUPPLIERS
    '=========================================================
    Private Sub LoadSuppliersToWeb()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                "SELECT SupplierID, SupplierName " &
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


    '=========================================================
    ' SEND DATA TO WEB UI
    '=========================================================
    Private Sub SendToWeb(data As Object)

        Try

            If WebView21.CoreWebView2 Is Nothing Then

                Return

            End If


            Dim json As String =
                JsonSerializer.Serialize(data)


            '-------------------------------------------------
            ' Send message to WebView2
            '-------------------------------------------------
            WebView21.CoreWebView2.PostWebMessageAsJson(
                json
            )


            '-------------------------------------------------
            ' Send message to iframe
            '-------------------------------------------------
            Dim script As String =
                "if (document.getElementById('page-frame')) {" &
                "document.getElementById('page-frame')" &
                ".contentWindow.postMessage(" &
                json &
                ", '*');" &
                "}"


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

        If IsDBNull(reader(columnName)) Then

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

        If IsDBNull(reader(columnName)) Then

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

        If IsDBNull(reader(columnName)) Then

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

        If IsDBNull(reader(columnName)) Then

            Return ""

        End If

        Return reader(columnName).ToString()

    End Function


End Class