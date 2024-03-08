Imports HandyControl.Controls
Imports HandyControl.Tools.Extension
Imports System.Data
Imports System.Diagnostics.Eventing.Reader
Imports System.Windows.Forms
Imports System.Windows.Media


Public Class QuantityDialog
    Public Shared Property ProductName As String
    Public Shared Property Description As String
    Public Shared Property Price As Decimal



    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        UpdateTextboxes()


    End Sub

    Public Sub UpdateTextboxes()
        ProductNameTextBox.Text = ProductName
        ProductDescriptionTextBox.Text = Description
        ProductPriceTextBox.Text = Price.ToString()
    End Sub

    Private Sub SaveCategoryButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveCategoryButton.Click

    End Sub
End Class
