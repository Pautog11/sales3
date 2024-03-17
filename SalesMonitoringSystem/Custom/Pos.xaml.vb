Imports System.Data
Imports System.Data.SqlClient
Imports System.Reflection.Emit
Imports System.Windows.Forms
Imports System.Windows.Media.Animation
Imports HandyControl.Controls
Imports Spire.Xls.Core.Spreadsheet
Imports System.Windows.Controls
Imports HandyControl.Tools.Extension

Public Class Pos
    Implements IObserverPanel
    Private _subject As IObservablePanel
    Public Shared _itemSource As New DataTable

    Public Sub New()

        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Try
            _subject = Application.Current.Windows.OfType(Of Dashboard).FirstOrDefault
            _subject?.RegisterObserver(Me)
            _subject?.NotifyObserver()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Error(ex.Message, "Observer Error")
        End Try
        ' up()

        'Me._itemSource = New DataTable()
        '_itemSource.Columns.Add("Name", GetType(String))
        '_itemSource.Columns.Add("Quantity", GetType(Integer))
        '_itemSource.Columns.Add("Price", GetType(Double))
        '_itemSource.Columns.Add("TotalPrice", GetType(Double))

        'Dim newRow As DataRow = _itemSource.NewRow()
        'newRow("Name") = a
        'newRow("Quantity") = c
        'newRow("Price") = b
        'newRow("TotalPrice") = 75

        '_itemSource.Rows.Add()
        'Receipt.AutoGenerateColumns = False
        'UpdateVisualData()


        Dim hasNameColumn As Boolean = Pos._itemSource.Columns.Contains("Name")
        Dim hasQuantityColumn As Boolean = Pos._itemSource.Columns.Contains("Quantity")
        Dim hasPriceColumn As Boolean = Pos._itemSource.Columns.Contains("Price")
        Dim hasTotalPriceColumn As Boolean = Pos._itemSource.Columns.Contains("TotalPrice")

        ' Add columns conditionally if they don't already exist
        If Not hasNameColumn Then
            Pos._itemSource.Columns.Add("Name")
        End If
        If Not hasQuantityColumn Then
            Pos._itemSource.Columns.Add("Quantity")
        End If
        If Not hasPriceColumn Then
            Pos._itemSource.Columns.Add("Price")
        End If
        If Not hasTotalPriceColumn Then
            Pos._itemSource.Columns.Add("TotalPrice")
        End If

    End Sub

    Public Sub Update() Implements IObserverPanel.Update

        Try
            'pota.Text = GenInvoiceNumber(InvoiceType.Transaction)
            Wrappanelxd.Children.Clear()

            Dim cardModels As New List(Of CardModel)()

            Dim dTable As DataTable = BaseProduct.GetAllProduct()

            For Each row As DataRow In dTable.Rows
                Dim cardModel As New CardModel()
                cardModel.Id = row.Field(Of Integer)("id")
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

            UpdateVisualData()

        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub UpdateVisualData()

        'DataGridView1.Refresh()
        Dim total1 As Double = 0
        For i = 0 To _itemSource?.Rows.Count - 1
            total1 += _itemSource.Rows(i).Item("TotalPrice")
        Next
        Subtotal.Text = total1
        Total.Text = total1

        Receipt.ItemsSource = _itemSource?.DefaultView

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
        newRow1("Name") = "Jok"
        newRow1("Quantity") = 2
        newRow1("Price") = 9
        newRow1("TotalPrice") = 100 ' Example calculation
        _itemSource.Rows.Add(newRow1)
        UpdateVisualData()

    End Sub

    Private Sub Receipt_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles Receipt.SelectionChanged
        'If e.VerticalChange <> 0 AndAlso TypeOf e.OriginalSource Is ScrollBar Then
        '    StackedHeaderPanel.Margin = New Thickness(0, -e.VerticalOffset, 0, 0)
        'End If
        'If Receipt.SelectedItems.Count > 0 Then
        '    Dialog.Show(New QuantityDialog(_subject, Receipt.SelectedItems(0)))
        '    Receipt.SelectedIndex = -1
        'End If

        If Receipt.SelectedItems.Count > 0 Then
            ' Assuming _subject is accessible and YourItemType is the type of the selected item
            Dim selectedObject As DataRowView = CType(Receipt.SelectedItems(0), DataRowView)
            Dialog.Show(New QuantityDialog(_subject, selectedObject))
            Receipt.SelectedIndex = -1
        End If

        'UpdateVisualData()
        'If Receipt.SelectedItems.Count > 0 Then
        '    '    Dim dialog As New QuantityDialog(_subject, Receipt.SelectedItems(0))
        '    '    dialog.Show()
        '    '    Receipt.SelectedIndex = -1
        '    Dim dialog As New QuantityDialog
        '    dialog.Show()
        'End If
    End Sub

    Private Sub Discount_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles Discount.SelectionChanged

        If Discount.SelectedIndex <> -1 Then
            Dim subtotalValue As Decimal
            If Decimal.TryParse(Subtotal.Text, subtotalValue) Then
                Dim dis As Decimal
                Select Case Discount.SelectedItem.ToString()
                    Case "0%"
                        dis = subtotalValue - (subtotalValue * 0.0D)
                    Case "10%"
                        dis = subtotalValue - (subtotalValue * 0.1D)
                    Case "20%"
                        dis = subtotalValue - (subtotalValue * 0.2D)
                    Case "30%"
                        dis = subtotalValue - (subtotalValue * 0.3D)
                    Case "50%"
                        dis = subtotalValue - (subtotalValue * 0.5D)
                End Select

                Total.Text = dis.ToString("F2")
            End If
        End If
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        ''Receipt.ItemsSource = Nothing
        'Dim a As New DataGridView
        'a.DataSource = Nothing
        'a.datas.Clear()
        'Receipt.ItemsSource = New List(Of CardModel)()
        'Receipt.Columns.Clear()
        ''Receipt.Refresh()
        'Receipt.ItemsSource = Nothing
        'Receipt.Refresh()

        'Receipt.Rows.Clear()

        _itemSource.Clear()

        'Call CType(Receipt.ItemsSource, DataTable).Rows.Clear()
    End Sub

    Private Sub Frames_Click(sender As Object, e As RoutedEventArgs) Handles Frames.Click
        'Growl.Info(BaseProduct.FramesQuery)
        Wrappanelxd.Children.Clear()

        Dim cardModels As New List(Of CardModel)()

        Dim dTable As DataTable = BaseProduct.FramesQuery()

        For Each row As DataRow In dTable.Rows
            Dim cardModel As New CardModel()
            cardModel.Id = row.Field(Of Integer)("id")
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
    End Sub

    Private Sub All_Click(sender As Object, e As RoutedEventArgs) Handles All.Click
        Wrappanelxd.Children.Clear()
        Update()
    End Sub

    Private Sub Lens_Click(sender As Object, e As RoutedEventArgs) Handles Lens.Click
        Wrappanelxd.Children.Clear()

        Dim cardModels As New List(Of CardModel)()

        Dim dTable As DataTable = BaseProduct.LensQuery()

        For Each row As DataRow In dTable.Rows
            Dim cardModel As New CardModel()
            cardModel.Id = row.Field(Of Integer)("id")
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
    End Sub
End Class

Public Class CardModel
    Public Property Id As String
    Public Property Title As String
    Public Property Description As String
    Public Property ImageSourceProperty As ImageSource
    Public Property TextImage As String
    Public Property Price As Decimal
End Class
