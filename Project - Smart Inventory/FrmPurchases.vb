Imports MySql.Data.MySqlClient

Public Class FrmPurchases


    ' FORM LOAD

    Private Sub FrmPurchases_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ConfigureDataGridView()

        LoadSuppliers()

        LoadProducts()

        LoadPurchases()

        ClearFields()

    End Sub



    ' CONFIGURE DATAGRIDVIEW

    Private Sub ConfigureDataGridView()

        dgvPurchases.ReadOnly = True

        dgvPurchases.AllowUserToAddRows = False

        dgvPurchases.AllowUserToDeleteRows = False

        dgvPurchases.MultiSelect = False

        dgvPurchases.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        dgvPurchases.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill

    End Sub



    ' LOAD SUPPLIERS

    Private Sub LoadSuppliers()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                "SELECT SupplierID, SupplierName " &
                "FROM Suppliers " &
                "ORDER BY SupplierName"

                Using adapter As New MySqlDataAdapter(query, con)

                    Dim table As New DataTable()

                    adapter.Fill(table)

                    cmbSupplier.DataSource = table
                    cmbSupplier.DisplayMember = "SupplierName"
                    cmbSupplier.ValueMember = "SupplierID"

                    cmbSupplier.SelectedIndex = -1

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
            "Error loading suppliers:" &
            vbCrLf & ex.Message,
            "Database Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

        End Try

    End Sub



    ' LOAD PRODUCTS

    Private Sub LoadProducts()

        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                    "SELECT ProductID, ProductName, PurchasePrice " &
                    "FROM Products " &
                    "ORDER BY ProductName"

                Using cmd As New MySqlCommand(query, con)

                    Using reader As MySqlDataReader =
                        cmd.ExecuteReader()

                        cmbProduct.Items.Clear()

                        While reader.Read()

                            Dim item As New ProductComboBoxItem()

                            item.Text =
                                reader("ProductName").ToString()

                            item.Value =
                                Convert.ToInt32(
                                    reader("ProductID")
                                )

                            item.PurchasePrice =
                                Convert.ToDecimal(
                                    reader("PurchasePrice")
                                )

                            cmbProduct.Items.Add(item)

                        End While

                    End Using

                End Using

            End Using

            cmbProduct.SelectedIndex = -1

        Catch ex As Exception

            MessageBox.Show(
                "Error loading products:" &
                vbCrLf & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' PRODUCT SELECTED

    Private Sub cmbProduct_SelectedIndexChanged(
        sender As Object,
        e As EventArgs
    ) Handles cmbProduct.SelectedIndexChanged

        If cmbProduct.SelectedIndex >= 0 Then

            Dim item As ProductComboBoxItem =
                TryCast(cmbProduct.SelectedItem,
                        ProductComboBoxItem)

            If item IsNot Nothing Then

                txtPurchasePrice.Text =
                    item.PurchasePrice.ToString("0.00")

                CalculateTotal()

            End If

        End If

    End Sub



    ' QUANTITY CHANGED

    Private Sub txtQuantity_TextChanged(
        sender As Object,
        e As EventArgs
    ) Handles txtQuantity.TextChanged

        CalculateTotal()

    End Sub



    ' PURCHASE PRICE CHANGED

    Private Sub txtPurchasePrice_TextChanged(
        sender As Object,
        e As EventArgs
    ) Handles txtPurchasePrice.TextChanged

        CalculateTotal()

    End Sub



    ' CALCULATE TOTAL

    Private Sub CalculateTotal()

        Dim quantity As Decimal = 0

        Dim price As Decimal = 0

        Decimal.TryParse(
            txtQuantity.Text,
            quantity
        )

        Decimal.TryParse(
            txtPurchasePrice.Text,
            price
        )

        Dim total As Decimal =
            quantity * price

        txtTotalAmount.Text =
            total.ToString("0.00")

    End Sub



    ' ADD PURCHASE

    Private Sub btnAdd_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnAdd.Click


        ' VALIDATION


        If cmbSupplier.SelectedIndex = -1 Then

            MessageBox.Show(
                "Please select a supplier.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cmbSupplier.Focus()

            Exit Sub

        End If


        If cmbProduct.SelectedIndex = -1 Then

            MessageBox.Show(
                "Please select a product.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            cmbProduct.Focus()

            Exit Sub

        End If


        Dim quantity As Integer

        If Not Integer.TryParse(
            txtQuantity.Text.Trim(),
            quantity
        ) OrElse quantity <= 0 Then

            MessageBox.Show(
                "Please enter a valid quantity.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtQuantity.Focus()

            Exit Sub

        End If


        Dim purchasePrice As Decimal

        If Not Decimal.TryParse(
            txtPurchasePrice.Text.Trim(),
            purchasePrice
        ) OrElse purchasePrice <= 0 Then

            MessageBox.Show(
                "Please enter a valid purchase price.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtPurchasePrice.Focus()

            Exit Sub

        End If

        Dim productItem As ProductComboBoxItem =
            TryCast(
                cmbProduct.SelectedItem,
                ProductComboBoxItem
            )

        Dim ProductItems As ProductComboBoxItem =
        TryCast(
        cmbProduct.SelectedItem,
        ProductComboBoxItem
    )

        If productItem Is Nothing Then

            MessageBox.Show(
        "Invalid product selection.",
        "Error",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error
    )

            Exit Sub

        End If


        Dim TotalAmount As Decimal =
            quantity * purchasePrice



        ' DATABASE TRANSACTION


        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()

                Using transaction As MySqlTransaction =
                    con.BeginTransaction()

                    Try


                        ' 1. INSERT INTO PURCHASES


                        Dim purchaseQuery As String =
                            "INSERT INTO Purchases " &
                            "(SupplierID, PurchaseDate, TotalAmount) " &
                            "VALUES " &
                            "(@SupplierID, @PurchaseDate, @TotalAmount)"

                        Dim purchaseID As Integer


                        Using cmd As New MySqlCommand(
                            purchaseQuery,
                            con,
                            transaction
                        )

                            cmd.Parameters.AddWithValue("@SupplierID", Convert.ToInt32(cmbSupplier.SelectedValue))

                            cmd.Parameters.AddWithValue(
                                "@PurchaseDate",
                                dtpPurchaseDate.Value
                            )

                            cmd.Parameters.AddWithValue(
                                "@TotalAmount",
                                totalAmount
                            )

                            cmd.ExecuteNonQuery()

                            purchaseID =
                                Convert.ToInt32(
                                    cmd.LastInsertedId
                                )

                        End Using



                        ' 2. INSERT INTO PURCHASE DETAILS


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
                                productItem.Value
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
                                totalAmount
                            )

                            cmd.ExecuteNonQuery()

                        End Using



                        ' 3. UPDATE PRODUCT STOCK


                        Dim stockQuery As String =
                            "UPDATE Products " &
                            "SET StockQuantity = " &
                            "StockQuantity + @Quantity, " &
                            "PurchasePrice = @PurchasePrice " &
                            "WHERE ProductID = @ProductID"


                        Using cmd As New MySqlCommand(
                            stockQuery,
                            con,
                            transaction
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
                                "@ProductID",
                                productItem.Value
                            )

                            cmd.ExecuteNonQuery()

                        End Using



                        ' 4. COMMIT


                        transaction.Commit()


                        MessageBox.Show(
                            "Purchase added successfully." &
                            vbCrLf &
                            "Stock has been updated.",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        )


                        ClearFields()

                        LoadPurchases()

                        LoadProducts()


                    Catch ex As Exception

                        transaction.Rollback()

                        Throw

                    End Try

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Error adding purchase:" &
                vbCrLf & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' LOAD PURCHASE RECORDS

    Private Sub LoadPurchases()

        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                    "SELECT " &
                    "p.PurchaseID, " &
                    "p.PurchaseDate, " &
                    "s.SupplierName, " &
                    "pd.ProductID, " &
                    "pr.ProductName, " &
                    "pd.Quantity, " &
                    "pd.PurchasePrice, " &
                    "pd.TotalPrice " &
                    "FROM Purchases p " &
                    "INNER JOIN Suppliers s " &
                    "ON p.SupplierID = s.SupplierID " &
                    "INNER JOIN PurchaseDetails pd " &
                    "ON p.PurchaseID = pd.PurchaseID " &
                    "INNER JOIN Products pr " &
                    "ON pd.ProductID = pr.ProductID " &
                    "WHERE " &
                    "s.SupplierName LIKE @Search " &
                    "OR pr.ProductName LIKE @Search " &
                    "ORDER BY p.PurchaseID DESC"


                Using cmd As New MySqlCommand(
                    query,
                    con
                )

                    cmd.Parameters.AddWithValue(
                        "@Search",
                        "%" & txtSearch.Text.Trim() & "%"
                    )


                    Using adapter As New MySqlDataAdapter(cmd)

                        Dim table As New DataTable()

                        adapter.Fill(table)

                        dgvPurchases.DataSource =
                            table

                    End Using

                End Using

            End Using



            ' COLUMN HEADERS


            If dgvPurchases.Columns.Count > 0 Then

                dgvPurchases.Columns(
                    "PurchaseID"
                ).HeaderText = "Purchase ID"

                dgvPurchases.Columns(
                    "PurchaseDate"
                ).HeaderText = "Purchase Date"

                dgvPurchases.Columns(
                    "SupplierName"
                ).HeaderText = "Supplier"

                dgvPurchases.Columns(
                    "ProductID"
                ).HeaderText = "Product ID"

                dgvPurchases.Columns(
                    "ProductName"
                ).HeaderText = "Product"

                dgvPurchases.Columns(
                    "Quantity"
                ).HeaderText = "Quantity"

                dgvPurchases.Columns(
                    "PurchasePrice"
                ).HeaderText = "Purchase Price"

                dgvPurchases.Columns(
                    "TotalPrice"
                ).HeaderText = "Total Price"

            End If


        Catch ex As Exception

            MessageBox.Show(
                "Error loading purchases:" &
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

        LoadPurchases()

    End Sub



    ' CLEAR

    Private Sub btnClear_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnClear.Click

        ClearFields()

    End Sub


    Private Sub ClearFields()

        txtPurchaseNo.Clear()

        dtpPurchaseDate.Value =
            DateTime.Now

        cmbSupplier.SelectedIndex = -1

        cmbProduct.SelectedIndex = -1

        txtQuantity.Clear()

        txtPurchasePrice.Clear()

        txtTotalAmount.Clear()

        txtPurchaseNo.Focus()

    End Sub



    ' DELETE PURCHASE

    Private Sub btnDelete_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnDelete.Click

        If dgvPurchases.SelectedRows.Count = 0 Then

            MessageBox.Show(
                "Please select a purchase to delete.",
                "Delete Purchase",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Exit Sub

        End If


        Dim purchaseID As Integer =
            Convert.ToInt32(
                dgvPurchases.SelectedRows(0).
                Cells("PurchaseID").Value
            )


        Dim result As DialogResult =
            MessageBox.Show(
                "Are you sure you want to delete this purchase?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            )


        If result <> DialogResult.Yes Then

            Exit Sub

        End If


        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()

                Using transaction As MySqlTransaction =
                    con.BeginTransaction()

                    Try


                        ' Get purchase details before deleting


                        Dim quantity As Integer = 0

                        Dim productID As Integer = 0


                        Dim selectQuery As String =
                            "SELECT ProductID, Quantity " &
                            "FROM PurchaseDetails " &
                            "WHERE PurchaseID = @PurchaseID"


                        Using cmd As New MySqlCommand(
                            selectQuery,
                            con,
                            transaction
                        )

                            cmd.Parameters.AddWithValue(
                                "@PurchaseID",
                                purchaseID
                            )

                            Using reader =
                                cmd.ExecuteReader()

                                If reader.Read() Then

                                    productID =
                                        Convert.ToInt32(
                                            reader("ProductID")
                                        )

                                    quantity =
                                        Convert.ToInt32(
                                            reader("Quantity")
                                        )

                                End If

                            End Using

                        End Using



                        ' Reduce product stock


                        Dim stockQuery As String =
                            "UPDATE Products " &
                            "SET StockQuantity = " &
                            "StockQuantity - @Quantity " &
                            "WHERE ProductID = @ProductID"


                        Using cmd As New MySqlCommand(
                            stockQuery,
                            con,
                            transaction
                        )

                            cmd.Parameters.AddWithValue(
                                "@Quantity",
                                quantity
                            )

                            cmd.Parameters.AddWithValue(
                                "@ProductID",
                                productID
                            )

                            cmd.ExecuteNonQuery()

                        End Using



                        ' Delete PurchaseDetails


                        Dim detailDeleteQuery As String =
                            "DELETE FROM PurchaseDetails " &
                            "WHERE PurchaseID = @PurchaseID"


                        Using cmd As New MySqlCommand(
                            detailDeleteQuery,
                            con,
                            transaction
                        )

                            cmd.Parameters.AddWithValue(
                                "@PurchaseID",
                                purchaseID
                            )

                            cmd.ExecuteNonQuery()

                        End Using



                        ' Delete Purchase


                        Dim purchaseDeleteQuery As String =
                            "DELETE FROM Purchases " &
                            "WHERE PurchaseID = @PurchaseID"


                        Using cmd As New MySqlCommand(
                            purchaseDeleteQuery,
                            con,
                            transaction
                        )

                            cmd.Parameters.AddWithValue(
                                "@PurchaseID",
                                purchaseID
                            )

                            cmd.ExecuteNonQuery()

                        End Using


                        transaction.Commit()


                        MessageBox.Show(
                            "Purchase deleted successfully.",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        )


                        LoadPurchases()

                        LoadProducts()


                    Catch ex As Exception

                        transaction.Rollback()

                        Throw

                    End Try

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Error deleting purchase:" &
                vbCrLf & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' UPDATE PURCHASE

    Private Sub btnUpdate_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnUpdate.Click

        MessageBox.Show(
            "Purchase update should be handled carefully because changing a purchase also changes product stock." &
            vbCrLf & vbCrLf &
            "For now, delete the old purchase and add the corrected purchase.",
            "Update Purchase",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        )

    End Sub

End Class



' SUPPLIER COMBOBOX ITEM

Public Class ComboBoxItems

    Public Property Text As String

    Public Property Value As Integer


    Public Sub New(
        text As String,
        value As Integer
    )

        Me.Text = text

        Me.Value = value

    End Sub


    Public Overrides Function ToString() As String

        Return Text

    End Function

End Class



' PRODUCT COMBOBOX ITEM

Public Class ProductComboBoxItem

    Public Property Text As String

    Public Property Value As Integer

    Public Property PurchasePrice As Decimal


    Public Sub New()

    End Sub


    Public Overrides Function ToString() As String

        Return Text

    End Function

End Class