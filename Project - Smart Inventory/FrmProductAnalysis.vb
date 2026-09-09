Imports MySql.Data.MySqlClient

Public Class FrmProductAnalysis


    ' FORM LOAD

    Private Sub FrmProductAnalysis_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ConfigureDataGridView()

        LoadProductAnalysis()

        LoadSummary()

    End Sub



    ' CONFIGURE DATAGRIDVIEW

    Private Sub ConfigureDataGridView()

        With dgvProductAnalysis

            .AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill

            .SelectionMode =
                DataGridViewSelectionMode.FullRowSelect

            .MultiSelect = False

            .ReadOnly = True

            .AllowUserToAddRows = False

            .AllowUserToDeleteRows = False

            .RowHeadersVisible = False

        End With

    End Sub



    ' LOAD PRODUCT ANALYSIS TABLE

    Private Sub LoadProductAnalysis()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String = "
                    SELECT
                        p.ProductName AS 'Product Name',
                        COALESCE(SUM(sd.Quantity), 0) AS 'Quantity Sold',
                        COALESCE(
                            SUM(sd.Quantity * sd.SellingPrice),
                            0
                        ) AS 'Revenue'
                    FROM SaleDetails sd

                    INNER JOIN Sales s
                        ON sd.SaleID = s.SaleID

                    INNER JOIN Products p
                        ON sd.ProductID = p.ProductID

                    WHERE s.Status = 'Completed'

                    GROUP BY
                        p.ProductID,
                        p.ProductName

                    ORDER BY
                        SUM(sd.Quantity) DESC
                "

                Using cmd As New MySqlCommand(query, con)

                    Using adapter As New MySqlDataAdapter(cmd)

                        Dim dt As New DataTable()

                        adapter.Fill(dt)

                        dgvProductAnalysis.DataSource = dt

                    End Using

                End Using

            End Using


            ' Format Revenue column
            If dgvProductAnalysis.Columns.Contains("Revenue") Then

                dgvProductAnalysis.Columns("Revenue").DefaultCellStyle.Format = "₹#,##0.00"

            End If


            ' Center quantity
            If dgvProductAnalysis.Columns.Contains("Quantity Sold") Then

                dgvProductAnalysis.Columns("Quantity Sold").DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleCenter

            End If


        Catch ex As Exception

            MessageBox.Show(
                "Error loading product analysis:" &
                Environment.NewLine &
                ex.Message,
                "Product Analysis",
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



                ' TOTAL PRODUCTS

                Dim totalProductsQuery As String = "
                    SELECT COUNT(*)
                    FROM Products
                "

                Using cmd As New MySqlCommand(totalProductsQuery, con)

                    Dim result = cmd.ExecuteScalar()

                    txtTotalProducts.Text =
                        Convert.ToInt32(result).ToString()

                End Using



                ' TOTAL SOLD ITEMS

                Dim totalSoldQuery As String = "
                    SELECT COALESCE(SUM(sd.Quantity), 0)
                    FROM SaleDetails sd

                    INNER JOIN Sales s
                        ON sd.SaleID = s.SaleID

                    WHERE s.Status = 'Completed'
                "

                Using cmd As New MySqlCommand(totalSoldQuery, con)

                    Dim result = cmd.ExecuteScalar()

                    txtTotalSoldItems.Text =
                        Convert.ToInt32(result).ToString()

                End Using



                ' TOTAL REVENUE

                Dim totalRevenueQuery As String = "
                    SELECT COALESCE(
                        SUM(sd.Quantity * sd.SellingPrice),
                        0
                    )
                    FROM SaleDetails sd

                    INNER JOIN Sales s
                        ON sd.SaleID = s.SaleID

                    WHERE s.Status = 'Completed'
                "

                Using cmd As New MySqlCommand(totalRevenueQuery, con)

                    Dim result = cmd.ExecuteScalar()

                    Dim revenue As Decimal =
                        Convert.ToDecimal(result)

                    txtTotalRevenue.Text =
                        "₹" & revenue.ToString("N2")

                End Using



                ' BEST SELLING 

                Dim bestProductQuery As String = "
                    SELECT
                        p.ProductName

                    FROM SaleDetails sd

                    INNER JOIN Sales s
                        ON sd.SaleID = s.SaleID

                    INNER JOIN Products p
                        ON sd.ProductID = p.ProductID

                    WHERE s.Status = 'Completed'

                    GROUP BY
                        p.ProductID,
                        p.ProductName

                    ORDER BY
                        SUM(sd.Quantity) DESC

                    LIMIT 1
                "

                Using cmd As New MySqlCommand(bestProductQuery, con)

                    Dim result = cmd.ExecuteScalar()

                    If result Is Nothing OrElse IsDBNull(result) Then

                        txtBestSelling.Text = "No Sales"

                    Else

                        txtBestSelling.Text =
                            result.ToString()

                    End If

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Error loading summary:" &
                Environment.NewLine &
                ex.Message,
                "Product Analysis",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' REFRESH BUTTON

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click

        LoadProductAnalysis()

        LoadSummary()

        MessageBox.Show(
            "Product analysis refreshed successfully.",
            "Product Analysis",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        )

    End Sub

End Class