Imports HandyControl.Controls
Imports HandyControl.Tools.Extension
Imports SalesMonitoringSystem.sgsmsdbTableAdapters
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics.Eventing.Reader
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Windows.Forms
Imports System.Windows.Media
Imports SalesMonitoringSystem.Pos.DataUpdater ' Replace YourNamespace with the actual namespace containing DataUpdater class



Public Class QuantityDialog

    Public Shared Property ProductName As String
    Public Shared Property Description As String
    Public Shared Property Price As Decimal
    Public Shared Property Id As Decimal
    Private _parent As Pos
    Private _data As DataRowView

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
        'ProductNameTextBox.Text = ""
        'ProductNameTextBox.Text = ""
        'ProductNameTextBox.Text = ""
        'ProductStocks.Text = ""

    End Sub

    Public Sub UpdateTextboxes()
        ProductNameTextBox.Text = ProductName
        ProductDescriptionTextBox.Text = Description
        ProductPriceTextBox.Text = Price.ToString()
        ProductStocks.Text = BaseInventory.ScalarStocks(Id).ToString()

    End Sub
    Private Sub SaveCategoryButton_Click(ByVal sender As Object, e As RoutedEventArgs) Handles SaveCategoryButton.Click

        If InputValidation.ValidateInputString(ProductQuantityTextBox, DataInput.STRING_INTEGER)(0) Then
            'Growl.Info("Invalid Quantity!")
            If Integer.Parse(ProductStocks.Text) < Integer.Parse(ProductQuantityTextBox.Text) Then
                Growl.Info("Insufficient Stocks")
                CloseDialog(Closebtn)
            Else
                Dim isExist As Boolean = False

                Dim existingRows() As DataRow = Pos._itemSource.Select("Name = '" & ProductNameTextBox.Text & "'")

                If existingRows.Length > 0 Then
                    Growl.Info("Item already exists!")
                Else
                    Dim newRow As DataRow = Pos._itemSource.NewRow()
                    newRow("Name") = ProductNameTextBox.Text
                    newRow("Quantity") = ProductQuantityTextBox.Text
                    newRow("Price") = ProductPriceTextBox.Text

                    Dim quantity As Integer = Convert.ToInt32(ProductQuantityTextBox.Text)
                    Dim price As Double = Convert.ToDouble(ProductPriceTextBox.Text)
                    Dim totalPrice As Double = quantity * price
                    newRow("TotalPrice") = totalPrice

                    Pos._itemSource.Rows.Add(newRow)

                    isExist = True

                    Dim a As New Pos
                    a.UpdateVisualData()

                End If

            End If
        Else
            Growl.Info("Invalid Quantity!")
        End If
        'Growl.Info("Invalid Quantity!")
        CloseDialog(Closebtn)
    End Sub
End Class


