
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports HandyControl.Controls
Imports SalesMonitoringSystem.Pos

Public Class ProductCard

    Inherits UserControl

    Public Sub New(Optional data As CardModel = Nothing)

        'This call Is required by the designer.
        InitializeComponent()


        ' Add any initialization after the InitializeComponent() call.
        DataContext = data

    End Sub

    Private Sub Button_Click(sender As Object, e As EventArgs)
        ' Handle the click event here
        MessageBox.Show("Product card clicked!")
    End Sub

End Class