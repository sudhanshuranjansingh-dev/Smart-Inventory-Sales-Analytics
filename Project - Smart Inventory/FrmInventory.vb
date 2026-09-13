Imports MySql.Data.MySqlClient

Public Class FrmInventory


    ' FORM LOAD

    Private Sub FrmInventoryManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ConfigureDataGridView()

        LoadSummary()

        LoadInventory()

        LoadStockFilter()

    End Sub



    ' DATAGRIDVIEW SETTINGS

    Private Sub ConfigureDataGridView()

        With dgvInventory

            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .RowHeadersVisible = False

        End With

    End Sub



    ' LOAD SUMMARY CARDS

    Private Sub LoadSummary()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()


                '-------------------------------
                ' TOTAL PRODUCTS
                '-------------------------------
                Dim queryTotalProducts As String =
                    "SELECT COUNT(*) FROM Products"

                Using cmd As New MySqlCommand(queryTotalProducts, con)

                    Dim result = cmd.ExecuteScalar()

                    txtTotalProducts.Text =
                        Convert.ToInt32(result).ToString()

                End Using


                '-------------------------------
                ' LOW STOCK
                '-------------------------------
                Dim queryLowStock As String =
                    "SELECT COUNT(*) FROM Products
                     WHERE StockQuantity > 0
                     AND StockQuantity <= MinimumStock"

                Using cmd As New MySqlCommand(queryLowStock, con)

                    Dim result = cmd.ExecuteScalar()

                    txtLowStocks.Text =
                        Convert.ToInt32(result).ToString()

                End Using



                ' OUT OF STOCK

                Dim queryOutOfStock As String =
                    "SELECT COUNT(*) FROM Products
                     WHERE StockQuantity = 0"

                Using cmd As New MySqlCommand(queryOutOfStock, con)

                    Dim result = cmd.ExecuteScalar()

                    txtOutOfStocks.Text =
                        Convert.ToInt32(result).ToString()

                End Using



                ' TOTAL UNITS

                Dim queryTotalUnits As String =
                    "SELECT COALESCE(SUM(StockQuantity), 0)
                     FROM Products"

                Using cmd As New MySqlCommand(queryTotalUnits, con)

                    Dim result = cmd.ExecuteScalar()

                    txtTotalUnits.Text =
                        Convert.ToInt32(result).ToString()

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Error loading inventory summary:" &
                Environment.NewLine &
                ex.Message,
                "Inventory Management",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' LOAD INVENTORY TABLE

    Private Sub LoadInventory()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                    "SELECT
                        p.ProductID AS 'Product ID',
                        p.ProductCode AS 'Product Code',
                        p.ProductName AS 'Product Name',
                        c.CategoryName AS 'Category',
                        s.SupplierName AS 'Supplier',
                        p.PurchasePrice AS 'Purchase Price',
                        p.SellingPrice AS 'Selling Price',
                        p.StockQuantity AS 'Stock',
                        p.MinimumStock AS 'Minimum Stock',

                        CASE
                            WHEN p.StockQuantity = 0
                                THEN 'Out of Stock'

                            WHEN p.StockQuantity <= p.MinimumStock
                                THEN 'Low Stock'

                            ELSE 'In Stock'
                        END AS 'Stock Status'

                    FROM Products p

                    LEFT JOIN Categories c
                        ON p.CategoryID = c.CategoryID

                    LEFT JOIN Suppliers s
                        ON p.SupplierID = s.SupplierID

                    ORDER BY p.ProductName"

                Using cmd As New MySqlCommand(query, con)

                    Using adapter As New MySqlDataAdapter(cmd)

                        Dim dt As New DataTable()

                        adapter.Fill(dt)

                        dgvInventory.DataSource = dt

                    End Using

                End Using

            End Using



            ' FORMAT PRICE COLUMNS

            If dgvInventory.Columns.Contains("Purchase Price") Then

                dgvInventory.Columns("Purchase Price").
                    DefaultCellStyle.Format = "₹#,##0.00"

            End If


            If dgvInventory.Columns.Contains("Selling Price") Then

                dgvInventory.Columns("Selling Price").
                    DefaultCellStyle.Format = "₹#,##0.00"

            End If


        Catch ex As Exception

            MessageBox.Show(
                "Error loading inventory:" &
                Environment.NewLine &
                ex.Message,
                "Inventory Management",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' LOAD STOCK FILTER

    Private Sub LoadStockFilter()

        cmbStockFilter.Items.Clear()

        cmbStockFilter.Items.Add("All Stock")
        cmbStockFilter.Items.Add("In Stock")
        cmbStockFilter.Items.Add("Low Stock")
        cmbStockFilter.Items.Add("Out of Stock")

        cmbStockFilter.SelectedIndex = 0

    End Sub



    ' SEARCH BUTTON

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        SearchInventory()

    End Sub



    ' SEARCH INVENTORY

    Private Sub SearchInventory()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                    "SELECT
                        p.ProductID AS 'Product ID',
                        p.ProductCode AS 'Product Code',
                        p.ProductName AS 'Product Name',
                        c.CategoryName AS 'Category',
                        s.SupplierName AS 'Supplier',
                        p.PurchasePrice AS 'Purchase Price',
                        p.SellingPrice AS 'Selling Price',
                        p.StockQuantity AS 'Stock',
                        p.MinimumStock AS 'Minimum Stock',

                        CASE
                            WHEN p.StockQuantity = 0
                                THEN 'Out of Stock'

                            WHEN p.StockQuantity <= p.MinimumStock
                                THEN 'Low Stock'

                            ELSE 'In Stock'
                        END AS 'Stock Status'

                    FROM Products p

                    LEFT JOIN Categories c
                        ON p.CategoryID = c.CategoryID

                    LEFT JOIN Suppliers s
                        ON p.SupplierID = s.SupplierID

                    WHERE p.ProductName LIKE @Search
                       OR p.ProductCode LIKE @Search

                    ORDER BY p.ProductName"

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue(
                        "@Search",
                        "%" & txtSearchProducts.Text.Trim() & "%"
                    )

                    Using adapter As New MySqlDataAdapter(cmd)

                        Dim dt As New DataTable()

                        adapter.Fill(dt)

                        dgvInventory.DataSource = dt

                    End Using

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "Error searching inventory:" &
                Environment.NewLine &
                ex.Message,
                "Inventory Management",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' STOCK FILTER

    Private Sub cmbStockFilter_SelectedIndexChanged(
        sender As Object,
        e As EventArgs
    ) Handles cmbStockFilter.SelectedIndexChanged

        FilterInventory()

    End Sub



    ' FILTER INVENTORY

    Private Sub FilterInventory()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                    "SELECT
                        p.ProductID AS 'Product ID',
                        p.ProductCode AS 'Product Code',
                        p.ProductName AS 'Product Name',
                        c.CategoryName AS 'Category',
                        s.SupplierName AS 'Supplier',
                        p.PurchasePrice AS 'Purchase Price',
                        p.SellingPrice AS 'Selling Price',
                        p.StockQuantity AS 'Stock',
                        p.MinimumStock AS 'Minimum Stock',

                        CASE
                            WHEN p.StockQuantity = 0
                                THEN 'Out of Stock'

                            WHEN p.StockQuantity <= p.MinimumStock
                                THEN 'Low Stock'

                            ELSE 'In Stock'
                        END AS 'Stock Status'

                    FROM Products p

                    LEFT JOIN Categories c
                        ON p.CategoryID = c.CategoryID

                    LEFT JOIN Suppliers s
                        ON p.SupplierID = s.SupplierID"

                If cmbStockFilter.Text = "In Stock" Then

                    query &= "
                        WHERE p.StockQuantity > p.MinimumStock"

                ElseIf cmbStockFilter.Text = "Low Stock" Then

                    query &= "
                        WHERE p.StockQuantity > 0
                        AND p.StockQuantity <= p.MinimumStock"

                ElseIf cmbStockFilter.Text = "Out of Stock" Then

                    query &= "
                        WHERE p.StockQuantity = 0"

                End If

                query &= " ORDER BY p.ProductName"


                Using cmd As New MySqlCommand(query, con)

                    Using adapter As New MySqlDataAdapter(cmd)

                        Dim dt As New DataTable()

                        adapter.Fill(dt)

                        dgvInventory.DataSource = dt

                    End Using

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "Error filtering inventory:" &
                Environment.NewLine &
                ex.Message,
                "Inventory Management",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' REFRESH BUTTON

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click

        txtSearchProducts.Clear()

        cmbStockFilter.SelectedIndex = 0

        LoadSummary()

        LoadInventory()

    End Sub

End Class