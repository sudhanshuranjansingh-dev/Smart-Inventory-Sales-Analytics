Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Text.Json
Imports Microsoft.Web.WebView2.Core
Imports System.Collections.Generic


Public Class FrmDashboard

    '=========================================================
    ' DASHBOARD DATA
    '=========================================================

    Private totalProducts As Integer = 0

    Private totalStock As Integer = 0

    Private lowStockItems As Integer = 0

    Private totalSales As Decimal = 0D

    Private inStockItems As Integer = 0

    Private outOfStockItems As Integer = 0


    '=========================================================
    ' MONTHLY SALES DATA
    '=========================================================

    Private salesMonths As New List(Of String)

    Private monthlySales As New List(Of Decimal)


    '=========================================================
    ' FORM LOAD
    '=========================================================

    Private Async Sub FrmDashboard_Load(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Load

        Try

            '-------------------------------------------------
            ' Initialize WebView2
            '-------------------------------------------------

            Await WebViewDashboard.EnsureCoreWebView2Async()


            '-------------------------------------------------
            ' Receive messages from JavaScript
            '-------------------------------------------------

            AddHandler WebViewDashboard.CoreWebView2.WebMessageReceived,
                AddressOf WebView_MessageReceived


            '-------------------------------------------------
            ' HTML Dashboard Path
            '-------------------------------------------------

            Dim htmlPath As String =
                Path.Combine(
                    Application.StartupPath,
                    "WebUI",
                    "dashboard.html"
                )


            '-------------------------------------------------
            ' Check HTML File
            '-------------------------------------------------

            If Not File.Exists(htmlPath) Then

                MessageBox.Show(
                    "Dashboard HTML was not found:" &
                    Environment.NewLine &
                    Environment.NewLine &
                    htmlPath,
                    "Dashboard Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                )

                Return

            End If


            '-------------------------------------------------
            ' Navigation Completed Event
            '-------------------------------------------------

            AddHandler WebViewDashboard.NavigationCompleted,
                AddressOf DashboardNavigationCompleted


            '-------------------------------------------------
            ' Load HTML
            '-------------------------------------------------

            WebViewDashboard.Source =
                New Uri(htmlPath)


        Catch ex As Exception

            MessageBox.Show(
                "Unable to initialize the HTML Dashboard:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "WebView2 Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '=========================================================
    ' HTML PAGE LOADED
    '=========================================================

    Private Sub DashboardNavigationCompleted(
        sender As Object,
        e As CoreWebView2NavigationCompletedEventArgs
    )

        If e.IsSuccess Then

            LoadDashboardData()

        Else

            MessageBox.Show(
                "The HTML Dashboard could not be loaded.",
                "Dashboard Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End If

    End Sub


    '=========================================================
    ' LOAD DASHBOARD DATA FROM MYSQL
    '=========================================================

    Private Sub LoadDashboardData()

        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()


                '=================================================
                ' TOTAL PRODUCTS
                '=================================================

                Dim queryProducts As String =
                    "SELECT COUNT(*) FROM Products"


                Using cmd As New MySqlCommand(
                    queryProducts,
                    con
                )

                    Dim result As Object =
                        cmd.ExecuteScalar()


                    If result Is Nothing OrElse
                       IsDBNull(result) Then

                        totalProducts = 0

                    Else

                        totalProducts =
                            Convert.ToInt32(result)

                    End If

                End Using


                '=================================================
                ' TOTAL STOCK / TOTAL UNITS
                '=================================================

                Dim queryStock As String =
                    "SELECT COALESCE(SUM(StockQuantity), 0) " &
                    "FROM Products"


                Using cmd As New MySqlCommand(
                    queryStock,
                    con
                )

                    Dim result As Object =
                        cmd.ExecuteScalar()


                    If result Is Nothing OrElse
                       IsDBNull(result) Then

                        totalStock = 0

                    Else

                        totalStock =
                            Convert.ToInt32(result)

                    End If

                End Using


                '=================================================
                ' LOW STOCK
                '=================================================

                Dim queryLowStock As String =
                    "SELECT COUNT(*) " &
                    "FROM Products " &
                    "WHERE StockQuantity > 0 " &
                    "AND StockQuantity <= MinimumStock"


                Using cmd As New MySqlCommand(
                    queryLowStock,
                    con
                )

                    Dim result As Object =
                        cmd.ExecuteScalar()


                    If result Is Nothing OrElse
                       IsDBNull(result) Then

                        lowStockItems = 0

                    Else

                        lowStockItems =
                            Convert.ToInt32(result)

                    End If

                End Using


                '=================================================
                ' OUT OF STOCK
                '=================================================

                Dim queryOutOfStock As String =
                    "SELECT COUNT(*) " &
                    "FROM Products " &
                    "WHERE StockQuantity = 0"


                Using cmd As New MySqlCommand(
                    queryOutOfStock,
                    con
                )

                    Dim result As Object =
                        cmd.ExecuteScalar()


                    If result Is Nothing OrElse
                       IsDBNull(result) Then

                        outOfStockItems = 0

                    Else

                        outOfStockItems =
                            Convert.ToInt32(result)

                    End If

                End Using


                '=================================================
                ' IN STOCK
                '=================================================

                inStockItems =
                    totalProducts -
                    lowStockItems -
                    outOfStockItems


                If inStockItems < 0 Then

                    inStockItems = 0

                End If


                '=================================================
                ' TOTAL SALES
                '=================================================

                Dim querySales As String =
                    "SELECT COALESCE(SUM(TotalAmount), 0) " &
                    "FROM Sales"


                Using cmd As New MySqlCommand(
                    querySales,
                    con
                )

                    Dim result As Object =
                        cmd.ExecuteScalar()


                    If result Is Nothing OrElse
                       IsDBNull(result) Then

                        totalSales = 0D

                    Else

                        totalSales =
                            Convert.ToDecimal(result)

                    End If

                End Using


                '=================================================
                ' MONTHLY SALES
                '=================================================

                salesMonths.Clear()

                monthlySales.Clear()


                Dim queryMonthlySales As String =
                    "SELECT MONTH(SaleDate) AS SaleMonth, " &
                    "COALESCE(SUM(TotalAmount), 0) AS TotalSales " &
                    "FROM Sales " &
                    "WHERE YEAR(SaleDate) = YEAR(CURDATE()) " &
                    "GROUP BY MONTH(SaleDate) " &
                    "ORDER BY MONTH(SaleDate)"


                Using cmd As New MySqlCommand(
                    queryMonthlySales,
                    con
                )

                    Using reader As MySqlDataReader =
                        cmd.ExecuteReader()


                        While reader.Read()

                            Dim monthNumber As Integer =
                                Convert.ToInt32(
                                    reader("SaleMonth")
                                )


                            Dim monthName As String =
                                New DateTime(
                                    DateTime.Now.Year,
                                    monthNumber,
                                    1
                                ).ToString("MMM")


                            Dim salesAmount As Decimal =
                                Convert.ToDecimal(
                                    reader("TotalSales")
                                )


                            salesMonths.Add(
                                monthName
                            )


                            monthlySales.Add(
                                salesAmount
                            )

                        End While

                    End Using

                End Using


            End Using


            '=================================================
            ' SEND DATA TO HTML
            '=================================================

            SendDashboardData()


        Catch ex As Exception

            MessageBox.Show(
                "Error loading dashboard data:" &
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
    ' SEND DATA TO JAVASCRIPT
    '=========================================================

    Private Sub SendDashboardData()

        Try

            '-------------------------------------------------
            ' Check WebView2
            '-------------------------------------------------

            If WebViewDashboard.CoreWebView2 Is Nothing Then

                Return

            End If


            '-------------------------------------------------
            ' Create Dashboard Dictionary
            '-------------------------------------------------

            Dim dashboardData As New Dictionary(Of String, Object)


            dashboardData("totalProducts") =
                totalProducts


            dashboardData("totalSales") =
                "₹" & totalSales.ToString("N2")


            dashboardData("lowStock") =
                lowStockItems


            dashboardData("totalUnits") =
                totalStock


            dashboardData("inStock") =
                inStockItems


            dashboardData("lowStockItems") =
                lowStockItems


            dashboardData("outOfStock") =
                outOfStockItems


            dashboardData("months") =
                salesMonths


            dashboardData("sales") =
                monthlySales


            '-------------------------------------------------
            ' Convert to JSON
            '-------------------------------------------------

            Dim jsonData As String =
                JsonSerializer.Serialize(
                    dashboardData
                )


            '-------------------------------------------------
            ' Send JSON to JavaScript
            '-------------------------------------------------

            WebViewDashboard.CoreWebView2.
                PostWebMessageAsJson(
                    jsonData
                )


        Catch ex As Exception

            MessageBox.Show(
                "Error sending dashboard data:" &
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
    ' RECEIVE MESSAGE FROM JAVASCRIPT
    '=========================================================

    Private Sub WebView_MessageReceived(
        sender As Object,
        e As CoreWebView2WebMessageReceivedEventArgs
    )

        Try

            '-------------------------------------------------
            ' Read JavaScript Message
            '-------------------------------------------------

            Dim message As String =
                e.WebMessageAsJson


            '-------------------------------------------------
            ' Parse JSON
            '-------------------------------------------------

            Using jsonDoc As JsonDocument =
                JsonDocument.Parse(message)


                Dim root As JsonElement =
                    jsonDoc.RootElement


                '-------------------------------------------------
                ' Get Action
                '-------------------------------------------------

                Dim actionElement As JsonElement


                If root.TryGetProperty(
                    "action",
                    actionElement
                ) Then


                    Dim action As String =
                        actionElement.GetString()


                    '---------------------------------------------
                    ' Refresh Dashboard
                    '---------------------------------------------

                    If action = "refreshDashboard" Then

                        LoadDashboardData()

                    End If

                End If

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Error receiving dashboard message:" &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "JavaScript Communication Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '=========================================================
    ' PUBLIC REFRESH METHOD
    '=========================================================

    Public Sub RefreshDashboard()

        LoadDashboardData()

    End Sub


End Class