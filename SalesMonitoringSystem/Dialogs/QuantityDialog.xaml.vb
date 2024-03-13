Imports HandyControl.Controls
Imports HandyControl.Tools.Extension
Imports SalesMonitoringSystem.sgsmsdbTableAdapters
Imports System.Collections.Specialized
Imports System.Data
Imports System.Diagnostics.Eventing.Reader
Imports System.Security.Cryptography
Imports System.Windows.Forms
Imports System.Windows.Media


Public Class QuantityDialog
    Public Shared Property ProductName As String
    Public Shared Property Description As String
    Public Shared Property Price As Decimal

    Private _parent As Pos
    Private _data As DataRowView

    'Private _subject As IObservablePanel
    'Public _itemSource As DataTable
    'Private _tableAdapter As New viewtblproductsTableAdapter
    ' c As String

    Public Sub New(
        Optional parent As Pos = Nothing,
        Optional data As DataRowView = Nothing)
        'ByRef De As Pos
        ')

        'This call Is required by the designer.
        InitializeComponent()
        _data = data
        _parent = parent
        '_parent = Pos
        If _data IsNot Nothing Then
            DataContext = _data
        End If
        ' Add any initialization after the InitializeComponent() call.

        'If Pos._itemSource Is Nothing Then
        '    Pos._itemSource = New DataTable()
        '    Pos._itemSource.Columns.Add("Name")
        '    Pos._itemSource.Columns.Add("Quantity")
        '    Pos._itemSource.Columns.Add("Price")
        '    Pos._itemSource.Columns.Add("TotalPrice")
        '    Pos._itemSource.Clear()
        '    'Else
        '    '    'Pos._itemSource.Columns.Add("Name")
        '    '    'Pos._itemSource.Columns.Add("Quantity")
        '    '    'Pos._itemSource.Columns.Add("Price")
        '    '    'Pos._itemSource.Columns.Add("TotalPrice")
        '    '    Pos._itemSource.Clear()
        'End If

        'Pos._itemSource.Clear()
        'Pos._itemSource.Columns.Add("Name")
        'Pos._itemSource.Columns.Add("Quantity")
        'Pos._itemSource.Columns.Add("Price")
        'Pos._itemSource.Columns.Add("TotalPrice")

        ' Check if the DataTable already has the columns
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


        UpdateTextboxes()
    End Sub

    Public Sub UpdateTextboxes()
        ProductNameTextBox.Text = ProductName
        ProductDescriptionTextBox.Text = Description
        ProductPriceTextBox.Text = Price.ToString()

        'If Not String.IsNullOrEmpty(ProductNameTextBox.Text) Then
        '    Dim info As DataTable = BaseProduct.ProductInfo(ProductNameTextBox.Text)
        '    ' Assuming BaseInventory.ScalarStocks(ProductNameTextBox.Text) returns an integer
        '    ProductStocks.Text = BaseInventory.ScalarStocks(ProductNameTextBox.Text).ToString()
        'End If
        'Dim info As String = BaseProduct.ProductStocks(ProductNameTextBox.Text)
        'ProductStocks.Text = 

    End Sub
    Private Sub SaveCategoryButton_Click(ByVal sender As Object, e As RoutedEventArgs) Handles SaveCategoryButton.Click

        Dim newRow1 As DataRow = Pos._itemSource.NewRow()
        newRow1("Name") = ProductNameTextBox.Text
        newRow1("Quantity") = ProductQuantityTextBox.Text
        newRow1("Price") = ProductPriceTextBox.Text

        Dim quantity As Integer = Convert.ToInt32(ProductQuantityTextBox.Text)
        Dim price As Double = Convert.ToDouble(ProductPriceTextBox.Text)

        ' Calculate the total price for the new row
        Dim totalPrice As Double = quantity * price
        newRow1("TotalPrice") = totalPrice

        'For Each row As DataRow In Pos._itemSource.Rows
        '    ' Calculate the total price for existing rows
        '    Dim existingQuantity As Integer = row.Field(Of Integer)("Quantity")
        '    Dim existingPrice As Double = row.Field(Of Double)("Price")
        '    row.SetField("TotalPrice", existingQuantity * existingPrice)
        'Next




        Pos._itemSource.Rows.Add(newRow1)
        CloseDialog(Closebtn)
    End Sub
    'Private Sub ProductNameComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ProductNameComboBox.SelectionChanged
    '    If ProductNameComboBox.SelectedIndex <> -1 Then
    '        Dim info As DataTable = BaseProduct.ProductInfo(ProductNameComboBox.SelectedValue)
    '        SellingPriceTextBox.Text = info.Rows(0).Item("PRICE").ToString
    '        QuantityAvailable.Text = BaseInventory.ScalarStocks(ProductNameComboBox.SelectedValue).ToString
    '    Else
    '        SellingPriceTextBox.Text = Nothing
    '        QuantityAvailable.Text = Nothing
    '    End If
    'End Sub
End Class
