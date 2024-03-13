Imports HandyControl.Controls
Imports HandyControl.Tools.Extension
Imports SalesMonitoringSystem.sgsmsdbTableAdapters
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

        UpdateTextboxes()
    End Sub

    Public Sub UpdateTextboxes()
        ProductNameTextBox.Text = ProductName
        ProductDescriptionTextBox.Text = Description
        ProductPriceTextBox.Text = Price.ToString()
    End Sub
    Private Sub SaveCategoryButton_Click(ByVal sender As Object, e As RoutedEventArgs) Handles SaveCategoryButton.Click
    End Sub
End Class
