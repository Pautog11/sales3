Imports System.Data
Imports System.Windows.Forms
Imports HandyControl.Controls

Public Class fuck
    'Public _itemSource As DataTable
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        Pos._itemSource.Columns.Add("Name")
        Pos._itemSource.Columns.Add("Quantity")
        Pos._itemSource.Columns.Add("Price")
        Pos._itemSource.Columns.Add("TotalPrice")

    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)

        Dim newRow1 As DataRow = Pos._itemSource.NewRow()
        newRow1("Name") = "Jok"
        newRow1("Quantity") = 2
        newRow1("Price") = 9
        newRow1("TotalPrice") = 2 * 10.99 ' Example calculation
        Pos._itemSource.Rows.Add(newRow1)
    End Sub
End Class
