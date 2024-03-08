Imports System.Data
Imports System.Data.SqlClient
Imports HandyControl.Controls

Public Class Pos
    'Implements IObservablePanel, IObserverPanel

    'Private _observables As New List(Of IObserverPanel)
    'Public Sub New()

    '    'This call is required by the designer.
    '    InitializeComponent()
    '    RegisterObserver(Me)
    '    NotifyObserver()

    'End Sub
    'Public Sub RegisterObserver(o As IObserverPanel) Implements IObservablePanel.RegisterObserver
    '    _observables.Add(o)
    'End Sub

    'Public Sub NotifyObserver() Implements IObservablePanel.NotifyObserver
    '    For Each o As IObserverPanel In _observables
    '        o.Update()
    '    Next
    'End Sub

    'Public Sub Newcardmodel()
    '    Try
    '        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
    '        Dim cmd As New SqlCommand("SELECT product_name, product_description, product_image, product_price FROM tblproducts", conn)
    '        Dim dTable As New DataTable
    '        Dim adapter As New SqlDataAdapter(cmd)
    '        adapter.Fill(dTable)

    '        Dim cardModels As New List(Of CardModel)()

    '        For Each row As DataRow In dTable.Rows
    '            Dim cardModel As New CardModel()
    '            cardModel.Title = row.Field(Of String)("product_name")
    '            cardModel.Description = If(row.IsNull("product_description"), "None", row.Field(Of String)("product_description"))
    '            cardModel.Price = row.Field(Of Double)("product_price")
    '            If Not row.IsNull("product_image") Then
    '                Try
    '                    Dim imageData As Byte() = DirectCast(row("product_image"), Byte())
    '                    Dim ms As New System.IO.MemoryStream(imageData)
    '                    Dim bitmap As New BitmapImage()
    '                    bitmap.BeginInit()
    '                    bitmap.StreamSource = ms
    '                    bitmap.EndInit()
    '                    cardModel.ImageSourceProperty = bitmap
    '                Catch ex As Exception

    '                    cardModel.TextImage = "No Image"
    '                End Try
    '            Else
    '                cardModel.TextImage = "No Image"
    '            End If
    '            cardModels.Add(cardModel)
    '        Next
    '        For Each CardModel In cardModels
    '            Dim productCard As New ProductCard(CardModel)
    '            Wrappanelxd.Children.Add(productCard)
    '        Next
    '    Catch ex As Exception
    '        HandyControl.Controls.MessageBox.Show(ex.Message)
    '    End Try
    'End Sub

    'Private Sub ProductSearch_TextChanged(sender As Object, e As TextChangedEventArgs) Handles ProductSearch.TextChanged
    '    Dim searchText As String = ProductSearch.Text.ToLower()

    '    For Each productCard As ProductCard In Wrappanelxd.Children
    '        Dim cardModel As CardModel = productCard.CardModel
    '        If cardModel.Title.ToLower().Contains(searchText) OrElse cardModel.Description.ToLower().Contains(searchText) Then
    '            productCard.Visibility = Visibility.Visible
    '        Else
    '            productCard.Visibility = Visibility.Collapsed
    '        End If
    '    Next
    'End Sub

    'Private Sub AsdDsll_Click(sender As Object, e As RoutedEventArgs) Handles AsdDsll.Click
    '    If Tite.Visibility = Visibility.Visible Then
    '        Tite.Visibility = Visibility.Collapsed
    '        ' Optionally, you can also hide the row containing the collapsed grid
    '        ' Grid.SetRowSpan(Grid1, 0)
    '    Else
    '        Tite.Visibility = Visibility.Visible
    '        ' Optionally, if you hid the row containing the collapsed grid, set it back to 1
    '        ' Grid.SetRowSpan(Grid1, 1)
    '    End If

    '    Windows.MessageBox.Show("jkjlkjlk")
    'End Sub

    'Private Sub Asdsll_Click(sender As Object, e As RoutedEventArgs) Handles Asdsll.Click
    '    Dialog.Show(New QuantityDialog)
    'End Sub
    'Public Sub lickmedaddy()
    '    Dim data As New DataTable
    '    'Add columns to the DataTable
    '    data.Columns.Add("Name", GetType(String))
    '    data.Columns.Add("Quantity", GetType(Integer))
    '    data.Columns.Add("Price", GetType(Double))
    '    data.Columns.Add("TotalPrice", GetType(Double))

    '    'Sample data rows (you can replace it with your actual data)
    '    data.Rows.Add("Item 1", 2, 500) ' 0.0 for initial TotalPrice
    '    data.Rows.Add("Item 2", 1, 250, 0.0)
    '    data.Rows.Add("Item 3", 3, 300, 0.0)

    '    '' Calculate the total price for each row
    '    For Each row As DataRow In data.Rows
    '        Dim quantity As Integer = row.Field(Of Integer)("Quantity")
    '        Dim price As Double = row.Field(Of Double)("Price")

    '        ' Calculate the total price and update the DataTable
    '        row.SetField("TotalPrice", quantity * price)
    '    Next

    '    Receipt.ItemsSource = data.DefaultView

    '    Dim grandTotalPrice As Double = 0.0
    '    For Each row As DataRow In data.Rows
    '        grandTotalPrice += row.Field(Of Double)("TotalPrice")
    '    Next

    '    pota.Text = grandTotalPrice 'Total Price


    '    Dim discount As Double
    '    discount = grandTotalPrice * 0.2
    '    pota1.Text = discount ' Amount Discounted

    '    pota2.Text = grandTotalPrice - discount ' deducted the discount amount

    '    Dim change As Double
    '    change = 3000 - pota2.Text
    '    pota3.Text = change
    'End Sub

    'Public Sub Update() Implements IObserverPanel.Update

    '    Try
    '        Wrappanelxd.Children.Clear()
    '        'Throw New NotImplementedException()
    '        pota1.Text = BaseProduct.ScalarProducts()
    '        Wrappanelxd.Children.Clear()

    '        Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
    '        Dim cmd As New SqlCommand("SELECT product_name, product_description, product_image, product_price FROM tblproducts", conn)
    '        Dim dTable As New DataTable
    '        Dim adapter As New SqlDataAdapter(cmd)
    '        adapter.Fill(dTable)

    '        Dim cardModels As New List(Of CardModel)()

    '        For Each row As DataRow In dTable.Rows
    '            Dim cardModel As New CardModel()
    '            cardModel.Title = row.Field(Of String)("product_name")
    '            cardModel.Description = If(row.IsNull("product_description"), "None", row.Field(Of String)("product_description"))
    '            cardModel.Price = row.Field(Of Double)("product_price")
    '            If Not row.IsNull("product_image") Then
    '                Try
    '                    Dim imageData As Byte() = DirectCast(row("product_image"), Byte())
    '                    Dim ms As New System.IO.MemoryStream(imageData)
    '                    Dim bitmap As New BitmapImage()
    '                    bitmap.BeginInit()
    '                    bitmap.StreamSource = ms
    '                    bitmap.EndInit()
    '                    cardModel.ImageSourceProperty = bitmap
    '                Catch ex As Exception

    '                    cardModel.TextImage = "No Image"
    '                End Try
    '            Else
    '                cardModel.TextImage = "No Image"
    '            End If
    '            cardModels.Add(cardModel)
    '        Next
    '        For Each CardModel In cardModels
    '            Dim productCard As New ProductCard(CardModel)
    '            Wrappanelxd.Children.Add(productCard)
    '        Next
    '    Catch ex As Exception
    '        HandyControl.Controls.MessageBox.Show(ex.Message)
    '    End Try
    'End Sub



    Implements IObserverPanel
    Private _subject As IObservablePanel

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Try
            _subject = Application.Current.Windows.OfType(Of Dashboard).FirstOrDefault
            _subject?.RegisterObserver(Me)
            _subject?.NotifyObserver()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Error(ex.Message, "Observer Error")
        End Try

    End Sub

    Public Sub Update() Implements IObserverPanel.Update
        'Throw New NotImplementedException()
        Try
            pota.Text = BaseProduct.ScalarProducts()

            Wrappanelxd.Children.Clear()

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
End Class

Public Class CardModel
    Public Property Title As String
    Public Property Description As String
    Public Property ImageSourceProperty As ImageSource
    Public Property TextImage As String
    Public Property Price As Double
End Class