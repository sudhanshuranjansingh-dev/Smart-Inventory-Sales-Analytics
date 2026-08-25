Imports MySql.Data.MySqlClient

Public Class FrmSalesHistory


    ' FORM LOAD


    Private Sub FrmSalesHistory_Load(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Load

        ConfigureDataGridView()

        LoadPaymentFilter()

        LoadStatusFilter()

        LoadSalesHistory()

        LoadSummary()

    End Sub



    ' CONFIGURE DATAGRIDVIEW


    Private Sub ConfigureDataGridView()

        dgvSalesHistory.ReadOnly = True

        dgvSalesHistory.AllowUserToAddRows = False

        dgvSalesHistory.AllowUserToDeleteRows = False

        dgvSalesHistory.MultiSelect = False

        dgvSalesHistory.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        dgvSalesHistory.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill

        dgvSalesHistory.RowHeadersVisible = False

    End Sub



    ' PAYMENT FILTER


    Private Sub LoadPaymentFilter()

        cmbPaymentFilter.Items.Clear()

        cmbPaymentFilter.Items.Add("All Payments")
        cmbPaymentFilter.Items.Add("Cash")
        cmbPaymentFilter.Items.Add("UPI")
        cmbPaymentFilter.Items.Add("Card")
        cmbPaymentFilter.Items.Add("Bank Transfer")

        cmbPaymentFilter.SelectedIndex = 0

    End Sub



    ' STATUS FILTER


    Private Sub LoadStatusFilter()

        cmbStatusFilter.Items.Clear()

        cmbStatusFilter.Items.Add("All Status")
        cmbStatusFilter.Items.Add("Completed")
        cmbStatusFilter.Items.Add("Cancelled")

        cmbStatusFilter.SelectedIndex = 0

    End Sub



    ' LOAD SALES HISTORY


    Private Sub LoadSalesHistory()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                "SELECT " &
                "SaleID, " &
                "CONCAT('INV-', LPAD(SaleID, 5, '0')) AS InvoiceNo, " &
                "SaleDate, " &
                "CustomerName, " &
                "TotalAmount, " &
                "PaymentMethod " &
                "FROM Sales " &
                "WHERE " &
                "(CONCAT('INV-', LPAD(SaleID, 5, '0')) LIKE @Search " &
                "OR CustomerName LIKE @Search) " &
                "ORDER BY SaleID DESC"

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue(
                    "@Search",
                    "%" & txtSearch.Text.Trim() & "%"
                )

                    Using adapter As New MySqlDataAdapter(cmd)

                        Dim table As New DataTable()

                        adapter.Fill(table)

                        dgvSalesHistory.DataSource = table

                    End Using

                End Using

            End Using


            ' COLUMN HEADERS

            If dgvSalesHistory.Columns.Count > 0 Then

                dgvSalesHistory.Columns("SaleID").HeaderText = "ID"

                dgvSalesHistory.Columns("InvoiceNo").HeaderText = "Invoice No"

                dgvSalesHistory.Columns("SaleDate").HeaderText = "Date"

                dgvSalesHistory.Columns("CustomerName").HeaderText = "Customer"

                dgvSalesHistory.Columns("TotalAmount").HeaderText = "Total Amount"

                dgvSalesHistory.Columns("PaymentMethod").HeaderText = "Payment"

            End If


        Catch ex As Exception

            MessageBox.Show(
            "Error loading sales history:" &
            vbCrLf & vbCrLf &
            ex.Message,
            "Database Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

        End Try

    End Sub


    ' LOAD SUMMARY


    Private Sub LoadSummary()

        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()



                ' TOTAL SALES


                Dim salesQuery As String =
                    "SELECT COALESCE(SUM(TotalAmount), 0) " &
                    "FROM Sales " &
                    "WHERE Status = 'Completed'"


                Using cmd As New MySqlCommand(
                    salesQuery,
                    con
                )

                    Dim totalSales As Decimal =
                        Convert.ToDecimal(
                            cmd.ExecuteScalar()
                        )


                    lblTotalSales.Text =
                        "₹" & totalSales.ToString("N2")

                End Using



                ' TOTAL ORDERS


                Dim ordersQuery As String =
                    "SELECT COUNT(*) " &
                    "FROM Sales " &
                    "WHERE Status = 'Completed'"


                Using cmd As New MySqlCommand(
                    ordersQuery,
                    con
                )

                    Dim totalOrders As Integer =
                        Convert.ToInt32(
                            cmd.ExecuteScalar()
                        )


                    lblTotalOrders.Text =
                        totalOrders.ToString()

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Error loading summary:" &
                vbCrLf & vbCrLf & ex.Message,
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

        LoadSalesHistory()

    End Sub



    ' PAYMENT FILTER


    Private Sub cmbPaymentFilter_SelectedIndexChanged(
        sender As Object,
        e As EventArgs
    ) Handles cmbPaymentFilter.SelectedIndexChanged

        LoadSalesHistory()

    End Sub



    ' STATUS FILTER


    Private Sub cmbStatusFilter_SelectedIndexChanged(
        sender As Object,
        e As EventArgs
    ) Handles cmbStatusFilter.SelectedIndexChanged

        LoadSalesHistory()

    End Sub



    ' REFRESH


    Private Sub btnRefresh_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnRefresh.Click

        txtSearch.Clear()

        cmbPaymentFilter.SelectedIndex = 0

        cmbStatusFilter.SelectedIndex = 0

        LoadSalesHistory()

        LoadSummary()

    End Sub



    ' VIEW DETAILS


    Private Sub btnViewDetails_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnViewDetails.Click

        If dgvSalesHistory.SelectedRows.Count = 0 Then

            MessageBox.Show(
                "Please select a sale first.",
                "Sales History",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Exit Sub

        End If


        Dim invoiceNo As String =
            dgvSalesHistory.SelectedRows(0).
            Cells("InvoiceNo").
            Value.ToString()


        MessageBox.Show(
            "Selected Invoice: " & invoiceNo,
            "Sale Details",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        )

    End Sub



    ' PRINT INVOICE


    Private Sub btnPrintInvoice_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnPrintInvoice.Click

        If dgvSalesHistory.SelectedRows.Count = 0 Then

            MessageBox.Show(
                "Please select a sale first.",
                "Sales History",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Exit Sub

        End If


        Dim invoiceNo As String =
            dgvSalesHistory.SelectedRows(0).
            Cells("InvoiceNo").
            Value.ToString()


        MessageBox.Show(
            "Print Invoice: " & invoiceNo,
            "Print",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        )

    End Sub

End Class