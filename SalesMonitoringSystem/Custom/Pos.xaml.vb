
Imports System.Collections.ObjectModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Windows.Forms
Imports HandyControl.Controls
Imports HandyControl.Data
Imports MS.Internal
Imports SalesMonitoringSystem.BaseProduct
Imports Spire.AI

Public Class Pos
    'Public Property CardModel As CardModel
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        Newcardmodel()

        Dim data As New DataTable

        ' Add columns to the DataTable
        data.Columns.Add("Name", GetType(String))
        data.Columns.Add("Quantity", GetType(Integer))
        data.Columns.Add("Price", GetType(Double))
        data.Columns.Add("TotalPrice", GetType(Double))

        ' Sample data rows (you can replace it with your actual data)
        data.Rows.Add("Item 1", 2, 105465.0) ' 0.0 for initial TotalPrice
        data.Rows.Add("Item 2", 1, 5.0, 0.0)
        data.Rows.Add("Item 3", 3, 8.0, 0.0)

        ' Calculate the total price for each row
        For Each row As DataRow In data.Rows
            Dim quantity As Integer = row.Field(Of Integer)("Quantity")
            Dim price As Double = row.Field(Of Double)("Price")

            ' Calculate the total price and update the DataTable
            row.SetField("TotalPrice", quantity * price)
        Next

        Receipt.ItemsSource = data.DefaultView

        Dim grandTotalPrice As Double = 0.0
        For Each row As DataRow In data.Rows
            grandTotalPrice += row.Field(Of Double)("TotalPrice")
        Next

        pota.Text = grandTotalPrice

        Dim discount As Double
        discount = grandTotalPrice * 0.2
        pota1.Text = discount
    End Sub

    Public Sub Newcardmodel()
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT product_name, product_description, product_image, product_price FROM tblproducts", conn)
            Dim dTable As New DataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)

            Dim cardModels As New List(Of CardModel)()

            For Each row As DataRow In dTable.Rows
                Dim cardModel As New CardModel()
                cardModel.Title = row.Field(Of String)("product_name")
                cardModel.Description = If(row.IsNull("product_description"), "None", row.Field(Of String)("product_description"))
                cardModel.Price = row.Field(Of Double)("product_price")
                If Not row.IsNull("product_image") Then
                    Try
                        Dim imageData As Byte() = DirectCast(row("product_image"), Byte())
                        Dim ms As New System.IO.MemoryStream(imageData)
                        Dim bitmap As New BitmapImage()
                        bitmap.BeginInit()
                        bitmap.StreamSource = ms
                        bitmap.EndInit()
                        cardModel.ImageSourceProperty = bitmap
                    Catch ex As Exception

                        cardModel.TextImage = "No Image"
                    End Try
                Else
                    cardModel.TextImage = "No Image"
                End If
                cardModels.Add(cardModel)
            Next
            For Each CardModel In cardModels
                Dim productCard As New ProductCard(CardModel)
                Wrappanelxd.Children.Add(productCard)
            Next
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub ProductSearch_TextChanged(sender As Object, e As TextChangedEventArgs) Handles ProductSearch.TextChanged
        Dim searchText As String = ProductSearch.Text.ToLower()

        For Each productCard As ProductCard In Wrappanelxd.Children
            Dim cardModel As CardModel = productCard.CardModel
            If cardModel.Title.ToLower().Contains(searchText) OrElse cardModel.Description.ToLower().Contains(searchText) Then
                productCard.Visibility = Visibility.Visible
            Else
                productCard.Visibility = Visibility.Collapsed
            End If
        Next
    End Sub
End Class

Public Class CardModel
    Public Property Title As String
    Public Property Description As String
    Public Property ImageSourceProperty As ImageSource
    Public Property TextImage As String
    Public Property Price As Double
End Class