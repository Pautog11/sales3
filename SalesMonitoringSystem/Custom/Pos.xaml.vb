Imports System.Data
Imports System.Data.SqlClient
Imports System.Reflection.Emit
Imports System.Windows.Forms
Imports System.Windows.Media.Animation
Imports HandyControl.Controls
Imports Spire.Xls.Core.Spreadsheet
Imports System.Windows.Controls
Imports HandyControl.Tools.Extension
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.IO
Imports System.Text

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

        Try
            If Receipt.SelectedItems.Count > 0 Then
                ' Assuming _subject is accessible and YourItemType is the type of the selected item
                Dim selectedObject As DataRowView = CType(Receipt.SelectedItems(0), DataRowView)
                Dialog.Show(New QuantityDialog(_subject, selectedObject))
                Receipt.SelectedIndex = -1
            End If

        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
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
    Private Sub All_Click(sender As Object, e As RoutedEventArgs) Handles All.Click
        Update()
    End Sub

    '0000000===================================================================================================00000000
    ''Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
    ''    '_itemSource.Clear()

    ''    Dim Doc As New Printing.PrintDocument()
    ''    Dim PaperSize As New Printing.PaperSize("MySize", 250, 600)
    ''    Doc.DefaultPageSettings.PaperSize = PaperSize

    ''    AddHandler Doc.PrintPage, AddressOf PrintContent

    ''    Dim PPD As New PrintPreviewDialog()
    ''    PPD.Document = Doc
    ''    PPD.WindowState = FormWindowState.Normal
    ''    PPD.ShowDialog()

    ''    'Dim a As New Form1
    ''    'a.Show()
    ''    'a.Button1.PerformClick()
    ''End Sub

    ''Private Sub PrintContent(sender As Object, e As Printing.PrintPageEventArgs)
    ''    ' Define the content to be printed
    ''    Dim printFont As New Font("Arial", 8)
    ''    Dim printText As String = "Hello, this is a sample text to be printed."

    ''    ' Draw the text on the print page
    ''    e.Graphics.DrawString(printText, printFont, Brushes.Black, 10, 10)
    ''End
    ''

    '00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000


    Dim WithEvents PD As New PrintDocument
    Dim PPD As New PrintPreviewDialog
    Dim longpaper As Integer

    Sub changelongpaper()
        Dim rowcount As Integer
        longpaper = 0
        rowcount = Receipt.Items.Count
        longpaper = rowcount * 15
        longpaper = longpaper + 240
    End Sub

    Private Sub Button_Click(sender As Object, e As EventArgs) ' Handles Button1.Click
        'Growl.Info("klkld")
        changelongpaper()
        PPD.Document = PD
        PPD.ShowDialog()
        'Growl.Info(_itemSource.ToString
        '

        'Dim dataBuilder As New StringBuilder()

        'For Each row As DataRowView In Receipt.Items
        '    ' Get values from each column in the row
        '    Dim name As String = row("Name").ToString()
        '    Dim price As String = row("Price").ToString()
        '    Dim quantity As String = row("Quantity").ToString()
        '    Dim totalPrice As String = row("TotalPrice").ToString()

        '    ' Append the values to the StringBuilder
        '    dataBuilder.AppendLine($"Name: {name}, Price: {price}, Quantity: {quantity}, Total Price: {totalPrice}")
        'Next

        '' Display the data in a message box
        'Windows.MessageBox.Show(dataBuilder.ToString(), "Data from DataGrid", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub PD_BeginPrint(sender As Object, e As PrintEventArgs) Handles PD.BeginPrint
        Dim pagesetup As New PageSettings
        pagesetup.PaperSize = New PaperSize("Custom", 250, 500) 'fixed size
        'pagesetup.PaperSize = New PaperSize("Custom", 250, longpaper)
        PD.DefaultPageSettings = pagesetup
    End Sub

    Private Sub PD_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PD.PrintPage
        Dim f8 As New Font("Calibri", 6)
        Dim f10 As New Font("Calibri", 6)
        Dim f10b As New Font("Calibri", 6)
        Dim f14 As New Font("Calibri", 6)

        Dim leftmargin As Integer = PD.DefaultPageSettings.Margins.Left
        Dim centermargin As Integer = PD.DefaultPageSettings.PaperSize.Width / 2
        Dim rightmargin As Integer = PD.DefaultPageSettings.PaperSize.Width

        'font alignment
        Dim right As New StringFormat
        Dim center As New StringFormat

        right.Alignment = StringAlignment.Far
        center.Alignment = StringAlignment.Center

        Dim line As String
        line = "****************************************************************"

        'range from top
        'logo

        'logo====================================================
        'Dim logoImage As System.Drawing.Image = My.Resources.ResourceManager.GetObject("ic_icon")
        'e.Graphics.DrawImage(logoImage, CInt((e.PageBounds.Width - 150) / 2), 5, 150, 35)

        Dim ic_products As New BitmapImage()
        ic_products.BeginInit()
        ic_products.UriSource = New Uri("C:\Users\Christian\Desktop\ic_icon.png", UriKind.Relative)
        ic_products.EndInit()

        ' Convert the BitmapImage to a System.Drawing.Image
        Dim logoImage As System.Drawing.Image

        Using ms As New MemoryStream()
            Dim encoder As New PngBitmapEncoder()
            encoder.Frames.Add(BitmapFrame.Create(ic_products))
            encoder.Save(ms)
            logoImage = System.Drawing.Image.FromStream(ms)
        End Using

        ' Draw the image on the graphics object
        e.Graphics.DrawImage(logoImage, CInt((e.PageBounds.Width - 150) / 2), 5, 150, 35)


        'e.Graphics.DrawImage(logoImage, 0, 250, 150, 50)
        'e.Graphics.DrawImage(logoImage, CInt((e.PageBounds.Width - logoImage.Width) / 2), CInt((e.PageBounds.Height - logoImage.Height) / 2), logoImage.Width, logoImage.Height)

        'e.Graphics.DrawString("Store :", f14, Brushes.Black, centermargin, 5, center)
        e.Graphics.DrawString("New York Street 15 Avenue", f10, Brushes.Black, centermargin, 40, center)
        e.Graphics.DrawString("Tel +1763545473", f10, Brushes.Black, centermargin, 55, center)

        e.Graphics.DrawString("Invoice ID", f8, Brushes.Black, 0, 75)
        e.Graphics.DrawString(":", f8, Brushes.Black, 50, 75)
        ' ReferenceNumberLabel.Text = GenInvoiceNumber(InvoiceType.Transaction)
        'e.Graphics.DrawString("DRW8555RE", f8, Brushes.Black, 70, 75)
        e.Graphics.DrawString(GenInvoiceNumber(InvoiceType.Transaction), f8, Brushes.Black, 70, 75)

        e.Graphics.DrawString("Cashier", f8, Brushes.Black, 0, 85)
        e.Graphics.DrawString(":", f8, Brushes.Black, 50, 85)
        e.Graphics.DrawString("Steve Jobs", f8, Brushes.Black, 70, 85)

        e.Graphics.DrawString("Date", f8, Brushes.Black, 0, 95)
        e.Graphics.DrawString(":", f8, Brushes.Black, 50, 95)
        e.Graphics.DrawString(Date.Now, f8, Brushes.Black, 70, 95)
        'DetailHeader
        e.Graphics.DrawString("Qty", f8, Brushes.Black, 0, 110)
        e.Graphics.DrawString("Item", f8, Brushes.Black, 25, 110)
        e.Graphics.DrawString("Price", f8, Brushes.Black, 180, 110, right)
        e.Graphics.DrawString("Total", f8, Brushes.Black, rightmargin, 110, right)
        '
        e.Graphics.DrawString(line, f8, Brushes.Black, 0, 120)


        Dim height As Integer 'DGV Position
        Dim i As Long
        'Receipt.AllowUserToAddRows = False




        'For row As Integer = 0 To Receipt.AlternationCount - 1
        '    Height += 15
        '    e.Graphics.DrawString(Receipt.Items(row).Cells(0).Value.ToString, f8, Brushes.Black, 0, 115 + height)
        '    e.Graphics.DrawString(Receipt.Items(row).Cells(0).Value.ToString, f8, Brushes.Black, 25, 115 + height)
        '    i = Receipt.Items(row).Cells(2).Value
        '    Receipt.Items(row).Cells(2).Value = Format(i, "##,##0")
        '    e.Graphics.DrawString(Receipt.Items(row).Cells(2).Value.ToString, f8, Brushes.Black, 180, 115 + height, right)

        '    'totalprice
        '    Dim totalprice As Long
        '    totalprice = Val(Receipt.Items(row).Cells(1).Value * Receipt.Items(row).Cells(2).Value)
        '    e.Graphics.DrawString(totalprice.ToString("##,##0"), f8, Brushes.Black, rightmargin, 115 + Height, right)
        'Next

        'For row As Integer = 0 To Receipt.AlternationCount - 1
        For Each rowView As DataRowView In CType(Receipt.ItemsSource, DataView)
            Dim row As DataRow = rowView.Row
            height += 15
            e.Graphics.DrawString(row(0).ToString(), f8, Brushes.Black, 0, 115 + height)
            e.Graphics.DrawString(row(1).ToString(), f8, Brushes.Black, 25, 115 + height)

            Dim s As Double = CDbl(row(2))
            row(2) = Format(s, "##,##0")

            e.Graphics.DrawString(row(2).ToString(), f8, Brushes.Black, 180, 115 + height, New StringFormat() With {.Alignment = StringAlignment.Far})

            'totalprice
            Dim totalPrice As Long = CLng(row(1)) * CLng(row(2))
            e.Graphics.DrawString(totalPrice.ToString("##,##0"), f8, Brushes.Black, rightmargin, 115 + height, New StringFormat() With {.Alignment = StringAlignment.Far})
        Next


        Dim height2 As Integer
        height2 = 145 + height
        sumprice() 'call sub
        e.Graphics.DrawString(line, f8, Brushes.Black, 0, height2)
        e.Graphics.DrawString("Total: " & Format(t_price, "##,##0"), f10b, Brushes.Black, rightmargin, 10 + height2, right)
        e.Graphics.DrawString(t_qty, f10b, Brushes.Black, 0, 10 + height2)
        e.Graphics.DrawString("~ Nosware Store ~", f10, Brushes.Black, centermargin, 85 + height2, center)

        'Dim dataBuilder As New StringBuilder()
        'For Each row As DataRowView In Receipt.Items
        '    ' Get values from each column in the row
        '    Dim name As String = row("Name").ToString()
        '    Dim price As String = row("Price").ToString()
        '    Dim quantity As String = row("Quantity").ToString()
        '    Dim totalPrice As String = row("TotalPrice").ToString()

        '    ' Append the values to the StringBuilder
        '    dataBuilder.AppendLine($"Name: {name}, Price: {price}, Quantity: {quantity}, Total Price: {totalPrice}")
        'Next


        'e.Graphics.DrawString(dataBuilder.ToString(), f8, Brushes.Black, 20, 200)

    End Sub

    Dim t_price As Long
    Dim t_qty As Long
    Sub sumprice()
        Dim countprice As Long = 0
        For rowitem As Long = 0 To Receipt.AlternationCount - 1
            countprice = countprice + Val(Receipt.Items(rowitem).Cells(2).Value * Receipt.Items(rowitem).Cells(1).Value)
        Next
        t_price = countprice
        Dim countqty As Long = 0
        For rowitem As Long = 0 To Receipt.AlternationCount - 1
            countqty = countqty + Receipt.Items(rowitem).Cells(1).Value
        Next
        t_qty = countqty
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
