
Imports System.Collections.ObjectModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Windows.Forms
Imports MS.Internal
Imports SalesMonitoringSystem.BaseProduct

Public Class Pos
    'Private _subject As IObservablePanel
    'Inherits UserControl
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

        '======================================================================================================================

        'Dim cardModels As New List(Of CardModel) From {
        '        New CardModel With {.Title = "Product 1", .Description = "Description 1", .Image = "Imgae 1"},
        '        New CardModel With {.Title = "Product 2", .Description = "Description 2", .Image = "Imgae 2"},
        '        New CardModel With {.Title = "Product 3", .Description = "Description 3", .Image = "Imgae 3"},
        '        New CardModel With {.Title = "Product 4", .Description = "Description 4", .Image = "Imgae 4"},
        '        New CardModel With {.Title = "Product 4", .Description = "Description 4", .Image = "Imgae 4"}
        '}

        'For Each cardModel In cardModels
        '    ' Create an instance of ProductCard UserControl
        '    Dim productCard As New ProductCard(cardModel)

        '    ' Set the title and description using the exposed properties
        '    'productCard.TitleText = cardModel.Title
        '    'productCard.DescriptionText = cardModel.Description
        '    'productCard.ImageText = cardModel.Image

        '    ' Add the ProductCard to the WrapPanel
        '    Wrappanelxd.Children.Add(productCard)
        'Next
        'Newcardmodel()


    End Sub

    Public Sub Newcardmodel()
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT product_name, product_description, product_image, product_price FROM tblproducts", conn)
            Dim dTable As New DataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)

            Dim cardModels As New List(Of CardModel)()

            For Each row As DataRow In dTable.Rows
                Dim cardModel As New CardModel()
                cardModel.Title = row.Field(Of String)("product_name")
                cardModel.Description = If(row.IsNull("product_description"), Nothing, row.Field(Of String)("product_description"))
                cardModel.Price = row.Field(Of Double)("product_price")
                If Not row.IsNull("product_image") Then
                    Try
                        Dim imageData As Byte() = DirectCast(row("product_image"), Byte())
                        Dim ms As New System.IO.MemoryStream(imageData)
                        Dim bitmap As New BitmapImage()
                        bitmap.BeginInit()
                        bitmap.StreamSource = ms
                        bitmap.EndInit()
                        cardModel.ImageSourceProperty = bitmap
                    Catch ex As Exception

                        cardModel.TextImage = "No Image"
                    End Try
                Else
                    cardModel.TextImage = "No Image"
                End If
                cardModels.Add(cardModel)
            Next

            For Each cardModel In cardModels
                Dim productCard As New ProductCard(cardModel)
                Wrappanelxd.Children.Add(productCard)
            Next
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try

    End Sub

End Class

Public Class CardModel
    Public Property Title As String
    Public Property Description As String
    Public Property ImageSourceProperty As ImageSource
    Public Property TextImage As String
    Public Property Price As Double
End Class
