Imports MySql.Data.MySqlClient

Public Class FrmReports
    Private Sub FrmReports_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' FORM LOAD

        'Set default dates
        dtpFromDate.Value = Date.Today.AddDays(-30)
        dtpToDate.Value = Date.Today

        'Configure DataGridView
        ConfigureDataGridView()

        'Load report
        LoadSalesReport()

        'Load summary
        LoadSummary()

    End Sub



    ' DATAGRIDVIEW SETTINGS

    Private Sub ConfigureDataGridView()

            With dgvSalesReport

                .ReadOnly = True
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .MultiSelect = False
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                .RowHeadersVisible = False

            End With

        End Sub



    ' LOAD SALES REPORT

    Private Sub LoadSalesReport()

            Try

                Using con As MySqlConnection = DBConnection.GetConnection()

                    con.Open()

                    Dim query As String = "
                    SELECT
                        CONCAT('INV-', LPAD(SaleID, 5, '0')) AS 'Invoice No',
                        DATE_FORMAT(SaleDate, '%d-%m-%Y') AS 'Date',
                        CustomerName AS 'Customer',
                        PaymentMethod AS 'Payment Method',
                        TotalAmount AS 'Total Amount',
                        Status
                    FROM Sales
                    WHERE SaleDate >= @FromDate
                      AND SaleDate < DATE_ADD(@ToDate, INTERVAL 1 DAY)
                    ORDER BY SaleDate DESC
                "

                    Using cmd As New MySqlCommand(query, con)

                        cmd.Parameters.AddWithValue(
                        "@FromDate",
                        dtpFromDate.Value.Date
                    )

                        cmd.Parameters.AddWithValue(
                        "@ToDate",
                        dtpToDate.Value.Date
                    )

                        Using adapter As New MySqlDataAdapter(cmd)

                            Dim dt As New DataTable()

                            adapter.Fill(dt)

                            dgvSalesReport.DataSource = dt

                        End Using

                    End Using

                End Using


                'Format Total Amount column
                If dgvSalesReport.Columns.Contains("Total Amount") Then

                    dgvSalesReport.Columns("Total Amount").DefaultCellStyle.Format = "₹#,##0.00"

                    dgvSalesReport.Columns("Total Amount").DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight

                End If


            Catch ex As Exception

                MessageBox.Show(
                "Error loading sales report:" &
                Environment.NewLine &
                ex.Message,
                "Sales Report",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

            End Try

        End Sub



    ' LOAD SUMMARY

    Private Sub LoadSummary()

            Try

                Using con As MySqlConnection = DBConnection.GetConnection()

                    con.Open()



                ' TOTAL SALES

                Dim totalSalesQuery As String = "
                    SELECT COALESCE(SUM(TotalAmount), 0)
                    FROM Sales
                    WHERE SaleDate >= @FromDate
                      AND SaleDate < DATE_ADD(@ToDate, INTERVAL 1 DAY)
                      AND Status = 'Completed'
                "

                    Using cmd As New MySqlCommand(totalSalesQuery, con)

                        cmd.Parameters.AddWithValue(
                        "@FromDate",
                        dtpFromDate.Value.Date
                    )

                        cmd.Parameters.AddWithValue(
                        "@ToDate",
                        dtpToDate.Value.Date
                    )

                        Dim result = cmd.ExecuteScalar()

                        Dim totalSales As Decimal =
                        Convert.ToDecimal(result)

                        txtTotalSales.Text =
                        "₹" & totalSales.ToString("N2")

                    End Using



                ' TOTAL ORDERS

                Dim totalOrdersQuery As String = "
                    SELECT COUNT(*)
                    FROM Sales
                    WHERE SaleDate >= @FromDate
                      AND SaleDate < DATE_ADD(@ToDate, INTERVAL 1 DAY)
                      AND Status = 'Completed'
                "

                    Using cmd As New MySqlCommand(totalOrdersQuery, con)

                        cmd.Parameters.AddWithValue(
                        "@FromDate",
                        dtpFromDate.Value.Date
                    )

                        cmd.Parameters.AddWithValue(
                        "@ToDate",
                        dtpToDate.Value.Date
                    )

                        Dim result = cmd.ExecuteScalar()

                        txtTotalOrders.Text =
                        Convert.ToInt32(result).ToString()

                    End Using

                End Using


            Catch ex As Exception

                MessageBox.Show(
                "Error loading summary:" &
                Environment.NewLine &
                ex.Message,
                "Sales Report",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

            End Try

        End Sub



    ' SEARCH BUTTON

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

            If dtpFromDate.Value.Date > dtpToDate.Value.Date Then

                MessageBox.Show(
                "From Date cannot be greater than To Date.",
                "Invalid Date",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

                Return

            End If


            LoadSalesReport()
            LoadSummary()

        End Sub



    ' REFRESH BUTTON

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click

            dtpFromDate.Value = Date.Today.AddDays(-30)
            dtpToDate.Value = Date.Today

            LoadSalesReport()
            LoadSummary()

        End Sub

    End Class
