
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports HandyControl.Controls
Imports SalesMonitoringSystem.Pos

Public Class ProductCard
    Inherits UserControl
    Public Property CardModel As CardModel

    Public Sub New(Optional data As CardModel = Nothing)
        ' This call is required by the designer.
        InitializeComponent()

        ' Set the DataContext to the provided data
        DataContext = data
        ' Set the CardModel property
        CardModel = data
    End Sub

    Private Sub Button_Click(sender As Object, e As EventArgs)

        If CardModel IsNot Nothing Then
            ' Assuming MyUserControl1 is the instance of your user control
            QuantityDialog.ProductName = CardModel.Title
            QuantityDialog.Description = CardModel.Description
            QuantityDialog.Price = CardModel.Price
            QuantityDialog.Id = CardModel.Id
        End If

        Dialog.Show(New QuantityDialog)

    End Sub
End Class