
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports HandyControl.Controls
Imports SalesMonitoringSystem.Pos

Public Class ProductCard
    Inherits UserControl

    ' Define the CardModel property
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
        ' Handle the click event here
        ' MessageBox.Show("Product card clicked!")

        If CardModel IsNot Nothing Then
            Dim message As String = $"Product Name: {CardModel.Title}" & vbCrLf &
                                    $"Description: {CardModel.Description}" & vbCrLf &
                                    $"Price: {CardModel.Price}"
            MessageBox.Show(message, "Product Details")
        End If
    End Sub
End Class