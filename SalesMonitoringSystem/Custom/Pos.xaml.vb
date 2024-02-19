
Public Class Pos

    'Private _dataTable As New sgsmsdb.viewtblproductsDataTable
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'dim n As Integer = 10
        For i = 0 To 100
            Wrappanelxd.Children.Add(New ProductCard())
        Next


    End Sub

End Class


