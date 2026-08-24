Imports MySql.Data.MySqlClient

Public Class Stock


    ' FORM LOAD

    Private Sub FrmStock_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ConfigureDataGridView()

        LoadStockFilter()

        LoadStock()

        LoadStockSummary()

    End Sub



    ' CONFIGURE DATAGRIDVIEW

    Private Sub ConfigureDataGridView()

        dgvStock.ReadOnly = True

        dgvStock.AllowUserToAddRows = False

        dgvStock.AllowUserToDeleteRows = False

        dgvStock.MultiSelect = False

        dgvStock.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        dgvStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

    End Sub



    ' LOAD FILTER OPTIONS

    Private Sub LoadStockFilter()

        cmbStockFilter.Items.Clear()

        cmbStockFilter.Items.Add("All Stock")
        cmbStockFilter.Items.Add("In Stock")
        cmbStockFilter.Items.Add("Low Stock")
        cmbStockFilter.Items.Add("Out of Stock")

        cmbStockFilter.SelectedIndex = 0

    End Sub



    ' LOAD STOCK DATA

    Private Sub LoadStock()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()


                Dim query As String = "SELECT " & "p.ProductID, " & "p.ProductCode, " & "p.ProductName, " & "COALESCE(c.CategoryName, 'N/A') AS CategoryName, " &
                    "COALESCE(s.SupplierName, 'N/A') AS SupplierName, " & "p.StockQuantity, " & "p.MinimumStock, " & "CASE " &
                    "WHEN p.StockQuantity = 0 THEN 'Out of Stock' " & "WHEN p.StockQuantity <= p.MinimumStock THEN 'Low Stock' " &
                    "ELSE 'In Stock' " & "END AS StockStatus " & "FROM Products p " & "LEFT JOIN Categories c " & "ON p.CategoryID = c.CategoryID " &
                    "LEFT JOIN Suppliers s " & "ON p.SupplierID = s.SupplierID " & "WHERE " & "p.ProductCode LIKE @Search " &
                    "OR p.ProductName LIKE @Search " & "OR c.CategoryName LIKE @Search " & "OR s.SupplierName LIKE @Search "



                ' FILTER


                Select Case cmbStockFilter.Text

                    Case "In Stock"

                        query &= " AND p.StockQuantity > p.MinimumStock "

                    Case "Low Stock"

                        query &= " AND p.StockQuantity > 0 " &
                                 "AND p.StockQuantity <= p.MinimumStock "

                    Case "Out of Stock"

                        query &= " AND p.StockQuantity = 0 "

                End Select


                query &= " ORDER BY p.ProductID DESC"


                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue("@Search", "%" & txtSearch.Text.Trim() & "%")


                    Using adapter As New MySqlDataAdapter(cmd)

                        Dim table As New DataTable()

                        adapter.Fill(table)

                        dgvStock.DataSource = table

                    End Using

                End Using

            End Using



            ' CHANGE COLUMN HEADERS


            If dgvStock.Columns.Count > 0 Then

                dgvStock.Columns("ProductID").HeaderText = "ID"

                dgvStock.Columns("ProductCode").HeaderText = "Product Code"

                dgvStock.Columns("ProductName").HeaderText = "Product Name"

                dgvStock.Columns("CategoryName").HeaderText = "Category"

                dgvStock.Columns("SupplierName").HeaderText = "Supplier"

                dgvStock.Columns("StockQuantity").HeaderText = "Current Stock"

                dgvStock.Columns("MinimumStock").HeaderText = "Minimum Stock"

                dgvStock.Columns("StockStatus").HeaderText = "Status"

            End If


        Catch ex As Exception

            MessageBox.Show("Error loading stock:" & vbCrLf & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub



    ' LOAD STOCK SUMMARY

    Private Sub LoadStockSummary()

        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()



                ' TOTAL PRODUCTS


                Dim totalProductsQuery As String =
                    "SELECT COUNT(*) FROM Products"


                Using cmd As New MySqlCommand(
                    totalProductsQuery,
                    con
                )

                    lblTotalProducts.Text = "TOTAL PRODUCTS: " & Convert.ToInt32(cmd.ExecuteScalar()).ToString()

                End Using



                ' LOW STOCK


                Dim lowStockQuery As String =
                    "SELECT COUNT(*) " &
                    "FROM Products " &
                    "WHERE StockQuantity > 0 " &
                    "AND StockQuantity <= MinimumStock"


                Using cmd As New MySqlCommand(
                    lowStockQuery,
                    con
                )
                    lblLowStocks.Text = "LOW STOCKS: " & Convert.ToInt32(cmd.ExecuteScalar()).ToString()
                End Using



                ' OUT OF STOCK


                Dim outOfStockQuery As String =
                    "SELECT COUNT(*) " &
                    "FROM Products " &
                    "WHERE StockQuantity = 0"


                Using cmd As New MySqlCommand(
                    outOfStockQuery,
                    con
                )

                    lblOutofStocks.Text = "OUT OF STOCK: " & Convert.ToInt32(cmd.ExecuteScalar()).ToString()
                End Using



                ' TOTAL UNITS


                Dim totalUnitsQuery As String =
                    "SELECT COALESCE(SUM(StockQuantity), 0) " &
                    "FROM Products"


                Using cmd As New MySqlCommand(
                    totalUnitsQuery,
                    con
                )

                    lblTotalUnits.Text = "TOTAL UNITS: " & Convert.ToInt32(cmd.ExecuteScalar()).ToString()

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Error loading stock summary:" &
                vbCrLf & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' SEARCH

    Private Sub txtSearch_TextChanged(
        sender As Object,
        e As EventArgs
    ) Handles txtSearch.TextChanged

        LoadStock()

    End Sub



    ' FILTER

    Private Sub cmbStockFilter_SelectedIndexChanged(
        sender As Object,
        e As EventArgs
    ) Handles cmbStockFilter.SelectedIndexChanged

        LoadStock()

    End Sub



    ' REFRESH BUTTON

    Private Sub btnRefresh_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnRefresh.Click

        txtSearch.Clear()

        cmbStockFilter.SelectedIndex = 0

        LoadStock()

        LoadStockSummary()

    End Sub

End Class