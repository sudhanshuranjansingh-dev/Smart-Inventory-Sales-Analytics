Imports MySql.Data.MySqlClient

Public Class SalesAnalytics

    '========================================================
    ' FORM LOAD
    '========================================================

    Private Sub SalesAnalytics_Load(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Load

        ' Default date range
        dtpFromDate.Value = New DateTime(
            DateTime.Now.Year,
            DateTime.Now.Month,
            1
        )

        dtpToDate.Value = DateTime.Now

        ' Load analytics
        LoadAnalytics()

    End Sub


    '========================================================
    ' LOAD ALL ANALYTICS
    '========================================================

    Private Sub LoadAnalytics()

        If dtpFromDate.Value.Date > dtpToDate.Value.Date Then

            MessageBox.Show(
                "From Date cannot be greater than To Date.",
                "Invalid Date",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Return

        End If

        LoadTotalSales()

        LoadTotalOrders()

        LoadAverageOrder()

        LoadMonthlySales()

    End Sub


    '========================================================
    ' TOTAL SALES
    '========================================================

    Private Sub LoadTotalSales()

        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                    "SELECT COALESCE(SUM(TotalAmount), 0) " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < DATE_ADD(@ToDate, INTERVAL 1 DAY) " &
                    "AND Status = 'Completed'"

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        dtpFromDate.Value.Date
                    )

                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        dtpToDate.Value.Date
                    )

                    Dim result As Object =
                        cmd.ExecuteScalar()

                    Dim totalSales As Decimal = 0D

                    If result IsNot Nothing AndAlso
                       result IsNot DBNull.Value Then

                        totalSales = Convert.ToDecimal(result)

                    End If

                    lblTotalSales.Text =
                        "₹" & totalSales.ToString("N2")

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "Error loading total sales:" &
                vbCrLf &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '========================================================
    ' TOTAL ORDERS
    '========================================================

    Private Sub LoadTotalOrders()

        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                    "SELECT COUNT(*) " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < DATE_ADD(@ToDate, INTERVAL 1 DAY) " &
                    "AND Status = 'Completed'"

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        dtpFromDate.Value.Date
                    )

                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        dtpToDate.Value.Date
                    )

                    Dim result As Object =
                        cmd.ExecuteScalar()

                    Dim totalOrders As Integer = 0

                    If result IsNot Nothing AndAlso
                       result IsNot DBNull.Value Then

                        totalOrders =
                            Convert.ToInt32(result)

                    End If

                    lblTotalOrders.Text =
                        totalOrders.ToString()

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "Error loading total orders:" &
                vbCrLf &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '========================================================
    ' AVERAGE ORDER
    '========================================================

    Private Sub LoadAverageOrder()

        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                    "SELECT COALESCE(AVG(TotalAmount), 0) " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < DATE_ADD(@ToDate, INTERVAL 1 DAY) " &
                    "AND Status = 'Completed'"

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        dtpFromDate.Value.Date
                    )

                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        dtpToDate.Value.Date
                    )

                    Dim result As Object =
                        cmd.ExecuteScalar()

                    Dim averageOrder As Decimal = 0D

                    If result IsNot Nothing AndAlso
                       result IsNot DBNull.Value Then

                        averageOrder =
                            Convert.ToDecimal(result)

                    End If

                    pnlAverageOrder.Text =
                        "₹" & averageOrder.ToString("N2")

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "Error loading average order:" &
                vbCrLf &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' MONTHLY SALES GRAPH


    Private Sub LoadMonthlySales()

        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                    "SELECT " &
                    "MONTH(SaleDate) AS SaleMonth, " &
                    "SUM(TotalAmount) AS MonthlySales " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < DATE_ADD(@ToDate, INTERVAL 1 DAY) " &
                    "AND Status = 'Completed' " &
                    "GROUP BY MONTH(SaleDate) " &
                    "ORDER BY MONTH(SaleDate)"

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        dtpFromDate.Value.Date
                    )

                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        dtpToDate.Value.Date
                    )

                    Using reader As MySqlDataReader =
                        cmd.ExecuteReader()

                        Dim values As New List(Of Double)
                        Dim labels As New List(Of String)
                        Dim positions As New List(Of Double)

                        Dim position As Double = 0

                        While reader.Read()

                            Dim monthNumber As Integer =
                                Convert.ToInt32(
                                    reader("SaleMonth")
                                )

                            Dim salesAmount As Double =
                                Convert.ToDouble(
                                    reader("MonthlySales")
                                )

                            values.Add(salesAmount)

                            labels.Add(
                                New DateTime(
                                    2000,
                                    monthNumber,
                                    1
                                ).ToString("MMM")
                            )

                            positions.Add(position)

                            position += 1

                        End While


                        ' Clear previous graph
                        plotSales.Plot.Clear()


                        ' If no sales exist
                        If values.Count = 0 Then

                            plotSales.Plot.Title(
                                "No Sales Data Available"
                            )

                            plotSales.Refresh()

                            Return

                        End If


                        ' Convert list to array
                        Dim salesValues() As Double =
                            values.ToArray()

                        Dim tickPositions() As Double =
                            positions.ToArray()

                        Dim tickLabels() As String =
                            labels.ToArray()


                        ' Create bar graph
                        Dim bars =
                            plotSales.Plot.Add.Bars(
                                salesValues
                            )


                        ' X-axis labels
                        plotSales.Plot.Axes.Bottom.SetTicks(
                            tickPositions,
                            tickLabels
                        )


                        ' Graph title
                        plotSales.Plot.Title(
                            "Monthly Sales"
                        )


                        ' Axis titles
                        plotSales.Plot.Axes.Left.Label.Text =
                            "Sales (₹)"

                        plotSales.Plot.Axes.Bottom.Label.Text =
                            "Month"


                        ' Refresh graph
                        plotSales.Refresh()

                    End Using

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "Error loading monthly sales:" &
                vbCrLf &
                ex.Message,
                "Graph Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' REFRESH BUTTON


    Private Sub btnRefresh_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnRefresh.Click

        LoadAnalytics()

    End Sub



    ' DATE CHANGE


    Private Sub dtpFromDate_ValueChanged(
        sender As Object,
        e As EventArgs
    ) Handles dtpFromDate.ValueChanged

        If dtpFromDate.Value.Date <=
           dtpToDate.Value.Date Then

            LoadAnalytics()

        End If

    End Sub


    Private Sub dtpToDate_ValueChanged(
        sender As Object,
        e As EventArgs
    ) Handles dtpToDate.ValueChanged

        If dtpFromDate.Value.Date <=
           dtpToDate.Value.Date Then

            LoadAnalytics()

        End If

    End Sub

End Class