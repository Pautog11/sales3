
Imports System.Collections.ObjectModel
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Windows.Forms
Imports MS.Internal

Public Class Pos

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
        Newcardmodel()
        'GetProductModels()

    End Sub

    Public Sub Newcardmodel()
        Dim connectionString As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Christian\OneDrive\Desktop\sales3\SalesMonitoringSystem\sgsmsdb.mdf;Integrated Security=True"
        Dim query As String = "SELECT product_name, product_description, product_image, product_price FROM tblproducts"
        Dim cardModels As New List(Of CardModel)()

        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                connection.Open()
                Using reader As SqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim cardModel As New CardModel()
                        cardModel.Title = reader.GetString(0)
                        cardModel.Description = If(Not reader.IsDBNull(1), reader.GetString(1), Nothing)
                        cardModel.Price = reader.GetDouble(3)

                        If Not reader.IsDBNull(reader.GetOrdinal("product_image")) Then
                            Try
                                ' Assuming the image data is in the "product_image" column of the reader
                                Dim imageData As Byte() = DirectCast(reader("product_image"), Byte())

                                ' Create a memory stream from the byte array
                                Dim ms As New System.IO.MemoryStream(imageData)

                                ' Create a BitmapImage
                                Dim bitmap As New BitmapImage()
                                bitmap.BeginInit()
                                bitmap.StreamSource = ms
                                bitmap.EndInit()

                                ' Assign the BitmapImage to the ImageSourceProperty
                                cardModel.ImageSourceProperty = bitmap
                            Catch ex As Exception
                                ' Handle the exception or log it
                                ' Dim a As String = "No Image"
                                cardModel.TextImage = "No Image"
                            End Try
                        Else
                            'cardModel.Image = "image_not_available
                            cardModel.TextImage = "No Image"
                        End If

                        cardModels.Add(cardModel)
                    End While
                End Using
            End Using
        End Using

        For Each cardModel In cardModels
            Dim productCard As New ProductCard(cardModel)
            Wrappanelxd.Children.Add(productCard)
        Next
    End Sub


End Class

Public Class CardModel
    Public Property Title As String
    Public Property Description As String
    Public Property ImageSourceProperty As ImageSource
    Public Property TextImage As String
    Public Property Price As Double
End Class
