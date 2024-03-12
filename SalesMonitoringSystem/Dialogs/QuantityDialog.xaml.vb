Imports HandyControl.Controls
Imports HandyControl.Tools.Extension
Imports SalesMonitoringSystem.sgsmsdbTableAdapters
Imports System.Data
Imports System.Diagnostics.Eventing.Reader
Imports System.Windows.Forms
Imports System.Windows.Media


Public Class QuantityDialog
    Public Shared Property ProductName As String
    Public Shared Property Description As String
    Public Shared Property Price As Decimal

    Private _parent As New Pos
    Private _data As DataRowView
    Private _subject As IObservablePanel
    Public _itemSource As DataTable
    Private _tableAdapter As New viewtblproductsTableAdapter

    Public Sub New(
        Optional parent As Pos = Nothing,
        Optional data As DataRowView = Nothing
        )

        ' This call is required by the designer.
        InitializeComponent()
        _data = data
        _parent = parent
        If _data IsNot Nothing Then
            DataContext = _data
        End If
        ' Add any initialization after the InitializeComponent() call.

        UpdateTextboxes()

    End Sub

    Public Sub UpdateTextboxes()
        ProductNameTextBox.Text = ProductName
        ProductDescriptionTextBox.Text = Description
        ProductPriceTextBox.Text = Price.ToString()
    End Sub
    Private Sub SaveCategoryButton_Click(ByVal sender As Object, e As RoutedEventArgs) Handles SaveCategoryButton.Click
        'Dim pos As New Pos
        'Dim dt As New DataTable
        'dt.Columns.Add("Name")
        'dt.Columns.Add("Quantity")
        'dt.Columns.Add("Price")
        'dt.Columns.Add("TotalPrice")


        'Dim newRow As DataRow = dt.NewRow()
        'newRow("Name") = ProductNameTextBox.Text
        'newRow("Quantity") = 5
        'newRow("Price") = ProductPriceTextBox.Text
        'newRow("TotalPrice") = 75
        'pos._itemSource.Rows.Add(newRow)
        'pos.UpdateVisualData()

        '=======================================
        'Dim pos As New Pos ' Assuming Pos is your form where you want to display the DataTable
        'Dim dt As New DataTable

        '' Define columns for the DataTable
        'dt.Columns.Add("Name", GetType(String))
        'dt.Columns.Add("Quantity", GetType(Integer))
        'dt.Columns.Add("Price", GetType(Double))
        'dt.Columns.Add("TotalPrice", GetType(Double))

        '' Create a new row
        'Dim newRow As DataRow = dt.NewRow()

        '' Assign values to the columns of the new row
        'newRow("Name") = ProductNameTextBox.Text
        'newRow("Quantity") = 5 ' Assuming you want a default quantity of 5
        'newRow("Price") = Convert.ToDouble(ProductPriceTextBox.Text) ' Convert the price to Double
        'newRow("TotalPrice") = Convert.ToDouble(ProductPriceTextBox.Text) * 5 ' Assuming TotalPrice is Price * Quantity

        '' Add the new row to the DataTable
        'dt.Rows.Add(newRow)

        '' Assuming _itemSource is the DataTable in your Pos form
        'pos._itemSource = dt

        '' Update the visual data in your form
        'pos.UpdateVisualData()


        '=====================================================
        'Dim is_existing As Boolean = False
        ''If _parent IsNot Nothing Then
        'Dim _parent As Pos = New Pos()
        'For Each item As DataRow In _parent._itemSource.Rows
        '    If item.Item("Name") = ProductNameTextBox.Text Then
        '        item.Item("Quantity") = CInt(ProductDescriptionTextBox.Text)
        '        item.Item("Price") = CInt(ProductPriceTextBox.Text)
        '        is_existing = True
        '        Exit For
        '    End If
        'Next

        ''Else
        ''Growl.Info("Parent form is not set.")
        ''End If
        'If Not is_existing Then
        '    _parent._itemSource.Rows.Add({
        '                                 ProductNameTextBox.Text,
        '                                 ProductDescriptionTextBox.Text,
        '                                 ProductPriceTextBox.Text
        '                                 })
        'End If
        '_parent.UpdateVisualData()
        'CloseDialog(Closebtn)
        '=============================================================
        Dim is_existing As Boolean = False

        Try
            Dim a As New Pos()
            For Each item As DataRow In a._itemSource.Rows
                If item.Item("Name") = ProductNameTextBox.Text Then
                    item.Item("Quantity") = CInt(ProductQuantityTextBox.Text)
                    item.Item("Price") = CInt(ProductPriceTextBox.Text)
                    is_existing = True
                    Exit For
                End If
            Next

            If Not is_existing Then
                Dim newRow As DataRow = a._itemSource.NewRow()
                newRow("Name") = ProductNameTextBox.Text
                newRow("Quantity") = 11 ' CInt(ProductQuantityTextBox.Text)
                newRow("Price") = CInt(ProductPriceTextBox.Text)
                a._itemSource.Rows.Add(newRow)
            End If
            Growl.Info(ProductNameTextBox.Text)

            a.UpdateVisualData()
            CloseDialog(Closebtn)
            ' Use pos object as needed
        Catch ex As Exception
            Windows.MessageBox.Show("An error occurred while instantiating Pos: " & ex.Message)
        End Try



    End Sub
End Class
