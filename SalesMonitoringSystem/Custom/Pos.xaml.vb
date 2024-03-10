Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports System.Windows.Media.Animation
Imports HandyControl.Controls
Imports Spire.Xls.Core.Spreadsheet

Public Class Pos
    Implements IObserverPanel
    Private _subject As IObservablePanel
    Private _data As DataRowView
    Public _itemSource As DataTable


    'Implements IObservablePanel, IObserverPanel

    'Private _observables As New List(Of IObserverPanel)
    'Public Sub New()

    '    'This call is required by the designer.
    '    InitializeComponent()
    '    RegisterObserver(Me)
    '    NotifyObserver()

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



    Public Sub New()
        'Optional data1 As DataRowView = Nothing,
        'Optional subject1 As IObservablePanel = Nothing
        ')

        ' This call is required by the designer.
        InitializeComponent()
        '_data = data1
        '_subject = subject1
        'DataContext = _data
        'up()

        _itemSource = New DataTable
        _itemSource.Columns.Add("Name")
        _itemSource.Columns.Add("Quantity")
        _itemSource.Columns.Add("Price")
        _itemSource.Columns.Add("TotalPrice")

        '_itemSource = New DataTable
        '_itemSource.Columns.Add("Name")
        '_itemSource.Columns.Add("Quantity")
        '_itemSource.Columns.Add("Price")
        '_itemSource.Columns.Add("TotalPrice")




        ' Add any initialization after the InitializeComponent() call.
        Try
            _subject = Application.Current.Windows.OfType(Of Dashboard).FirstOrDefault
            _subject?.RegisterObserver(Me)
            _subject?.NotifyObserver()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Error(ex.Message, "Observer Error")
        End Try
        up()

    End Sub

    Public Sub up(
        Optional data As DataRowView = Nothing,
        Optional subject As IObservablePanel = Nothing
        )
        ' This call is required by the designer.
        InitializeComponent()
        _data = data
        _subject = subject
        DataContext = _data

        ' Add any initialization after the InitializeComponent() call.
        If _data Is Nothing Then
            _itemSource = New DataTable
            _itemSource.Columns.Add("Name")
            _itemSource.Columns.Add("Quantity")
            _itemSource.Columns.Add("Price")
            _itemSource.Columns.Add("TotalPrice")


            '_itemSource.Rows.Add("as", "as", "As", "AS")

            'Receipt.ItemsSource = _itemSource.DefaultView
        Else
            Growl.Info("kj")
        End If

    End Sub


    Public Sub Update() Implements IObserverPanel.Update

        Try
            'pota.Text = ClearTypeHint

            pota.Text = GenInvoiceNumber(InvoiceType.Transaction)

            Wrappanelxd.Children.Clear()

            Dim cardModels As New List(Of CardModel)()

            Dim dTable As DataTable = BaseProduct.GetAllProduct()

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



        'Dim dt As New DataTable

        ''If dt.Columns.Count = 0 Then
        'dt.Columns.Add("Name", GetType(String))
        'dt.Columns.Add("Quantity", GetType(Integer))
        'dt.Columns.Add("Price", GetType(Double))
        'dt.Columns.Add("TotalPrice", GetType(Double))
        ''End If

        ''Dim newRow As DataRow = dt.NewRow()
        ''newRow("Name") = a
        '''newRow("Quantity") = c
        '''newRow("Price") = b
        ''newRow("TotalPrice") = 75
        ''
        'dt.Rows.Add(a)

        '' Add the new DataRow to the DataTable
        'dt.Rows.Add(newRow)
        '' Bind the DataTable to your control
        'Receipt.ItemsSource = dt.DefaultView

        '''''''''''''''''''''''''''''
        '''

        ' Pass the value of "a" to the method in UserControlB to add a row to the DataTable

        ''''''''' loop---------------------------------------------
        'Dim dt As New DataTable

        '' Check if the DataTable already has columns, if not, add them
        'If dt.Columns.Count = 0 Then
        '    dt.Columns.Add("Name", GetType(String))
        '    dt.Columns.Add("Quantity", GetType(Integer))
        '    dt.Columns.Add("Price", GetType(Double))
        '    dt.Columns.Add("TotalPrice", GetType(Double))
        'End If

        '' Loop to add multiple rows
        'For i As Integer = 1 To 100 ' You can adjust the loop count as per your requirement
        '    Dim newRow As DataRow = dt.NewRow()

        '    ' Example values for demonstration purposes
        '    newRow("Name") = "Item " & i
        '    newRow("Quantity") = i * 2
        '    newRow("Price") = i * 500
        '    newRow("TotalPrice") = (i * 2) * (i * 500) ' TotalPrice should be Quantity * Price

        '    ' Add the new row to the DataTable
        '    dt.Rows.Add(newRow)
        'Next

        '' Bind the DataTable to your control
        'Receipt.ItemsSource = dt.DefaultView

        ''''''''''''''''''''''''''''''==========================================================
        ''''


    End Sub

    Public Sub UpdateVisualData()
        Receipt.ItemsSource = _itemSource?.DefaultView
        'Dim total As Integer = 0
        'For i = 0 To _itemSource?.Rows.Count - 1
        '    total += _itemSource.Rows(i).Item("TOTAL")
        'Next
        'TotalPrice.Text = total
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

    Private Sub AsdDsll_Click(sender As Object, e As RoutedEventArgs) Handles AsdDsll.Click

        Dim newRow1 As DataRow = _itemSource.NewRow()
        newRow1("Name") = "Product A"
        newRow1("Quantity") = 2
        newRow1("Price") = 10.99
        newRow1("TotalPrice") = 2 * 10.99 ' Example calculation
        _itemSource.Rows.Add(newRow1)
        UpdateVisualData()
    End Sub
End Class

Public Class CardModel
    Public Property Title As String
    Public Property Description As String
    Public Property ImageSourceProperty As ImageSource
    Public Property TextImage As String
    Public Property Price As Double
End Class