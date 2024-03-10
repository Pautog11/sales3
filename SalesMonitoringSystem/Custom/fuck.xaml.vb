Imports System.Data

Public Class fuck
    Dim dt As New DataTable
    'Implements IObserverPanel
    'Private _subject As IObservablePanel

    'Public Sub New()

    '    ' This call is required by the designer.
    '    InitializeComponent()

    '    ' Add any initialization after the InitializeComponent() call.
    '    Try
    '        _subject = Application.Current.Windows.OfType(Of Dashboard).FirstOrDefault
    '        _subject?.RegisterObserver(Me)
    '        _subject?.NotifyObserver()
    '    Catch ex As Exception
    '        HandyControl.Controls.MessageBox.Error(ex.Message, "Observer Error")
    '    End Try

    'End Sub

    'Public Sub Update() Implements IObserverPanel.Update
    '    'Throw New NotImplementedException()
    '    'Pota.Text = BaseProduct.ScalarProducts()
    'End Sub

    Public Sub New()

        InitializeComponent()
        dt.Columns.Add("Name")
        dt.Columns.Add("Quantity")
        dt.Columns.Add("Price")
        dt.Columns.Add("TotalPrice")

        dt.Rows.Add("asa", "1", "21", "1")
        Receipt.ItemsSource = dt.DefaultView
    End Sub

    Private Sub Push_Click(sender As Object, e As RoutedEventArgs) Handles Push.Click

        Dim newRow1 As DataRow = dt.NewRow()
        newRow1("Name") = "Product A"
        newRow1("Quantity") = 2
        newRow1("Price") = 10.99
        newRow1("TotalPrice") = 2 * 10.99 ' Example calculation
        dt.Rows.Add(newRow1)

        ' Update the data source of your control to reflect changes
        Receipt.ItemsSource = dt.DefaultView

    End Sub
End Class

