
Imports System.Collections.ObjectModel

Public Class Pos

    Inherits UserControl

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'dim n As Integer = 10
        'For i = 0 To 100
        '    Wrappanelxd.Children.Add(New ProductCard())
        'Next

        'Dim ProductCard As New ProductCard
        'ProductCard.updated("Poatebjhlkjgdlkg")

        Dim cardModels As New List(Of CardModel) From {
                New CardModel With {.Title = "Product 1", .Description = "Description 1", .Image = "Imgae 1"},
                New CardModel With {.Title = "Product 2", .Description = "Description 2", .Image = "Imgae 2"},
                New CardModel With {.Title = "Product 3", .Description = "Description 3", .Image = "Imgae 3"},
                New CardModel With {.Title = "Product 4", .Description = "Description 4", .Image = "Imgae 4"}
        }

        For Each cardModel In cardModels
            ' Create an instance of ProductCard UserControl
            Dim productCard As New ProductCard(cardModel)

            ' Set the title and description using the exposed properties
            'productCard.TitleText = cardModel.Title
            'productCard.DescriptionText = cardModel.Description
            'productCard.ImageText = cardModel.Image

            ' Add the ProductCard to the WrapPanel
            Wrappanelxd.Children.Add(productCard)
        Next
    End Sub
End Class

Public Class CardModel
    Public Property Title As String
    Public Property Description As String
    Public Property Image As String
End Class


