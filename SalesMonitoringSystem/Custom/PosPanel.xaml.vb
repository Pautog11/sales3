Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions

Public Class PosPanel
    Public Sub New()
        InitializeComponent()

        Dim connectionString As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Christian\OneDrive\Desktop\sales3\SalesMonitoringSystem\sgsmsdb.mdf;Integrated Security=True"
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            'Dim query As String = "SELECT image, name, price FROM item"
            Dim query As String = "SELECT product_image, product_name, product_price FROM tblproducts"
            Using command As New SqlCommand(query, connection)
                Using reader As SqlDataReader = command.ExecuteReader()
                    While reader.Read()
                        Dim button As New Button()

                        ' Apply custom styling to the button
                        button.Background = Brushes.LightBlue
                        button.Foreground = Brushes.DarkBlue
                        button.BorderBrush = Brushes.Black
                        button.Margin = New Thickness(10, 5, 10, 5)
                        button.BorderThickness = New Thickness(2)

                        Dim stackPanel As New StackPanel()

                        ' Assuming there's a column named "ImageDataColumn" in your table

                        ' Check if the image data is not null
                        If Not IsDBNull(reader("product_image")) Then
                            Dim imageData As Byte() = DirectCast(reader("product_image"), Byte())

                            ' Convert byte array to BitmapImage
                            Dim bitmapImage As New BitmapImage()
                            Using stream As New MemoryStream(imageData)
                                bitmapImage.BeginInit()
                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad
                                bitmapImage.StreamSource = stream
                                bitmapImage.EndInit()
                            End Using

                            ''Load the image using the binary data
                            Dim image As New Image()
                            image.Source = bitmapImage
                            image.Width = 1000 ' Set the desired width
                            image.Height = 1000 ' Set the desired height
                            stackPanel.Children.Add(image)
                        Else
                            ' Load a default image if no image data is found
                            Dim imagePath As String = "C:\Users\Christian\Downloads\img.png"

                            Dim bitmapImage As New BitmapImage()
                            bitmapImage.BeginInit()
                            bitmapImage.UriSource = New Uri(imagePath)
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad
                            bitmapImage.EndInit()

                            'Load the image using the file path
                            Dim image As New Image()
                            image.Source = bitmapImage
                            image.Width = 100 ' Set the desired width
                            image.Height = 100 ' Set the desired height
                            stackPanel.Children.Add(image)
                        End If

                        ' Assuming there's a column named "ImageNameColumn" in your table
                        Dim imageName As String = reader("product_name").ToString()

                        ' Assuming there's a column named "PriceColumn" in your table
                        Dim price As Decimal = Convert.ToDecimal(reader("product_price"))

                        ' Create text blocks for image name and price
                        Dim nameTextBlock As New TextBlock()
                        nameTextBlock.Text = $"product_name: {imageName}"

                        Dim priceTextBlock As New TextBlock()
                        priceTextBlock.Text = $"product_price: {price:C}"

                        stackPanel.Children.Add(nameTextBlock)
                        stackPanel.Children.Add(priceTextBlock)

                        ' Add the stack panel to the button's content
                        button.Content = stackPanel

                        AddHandler button.Click, AddressOf Button_Click
                        flowLayoutPanel.Children.Add(button)
                    End While
                End Using
            End Using
        End Using
    End Sub


    'Private clickCounts As New Dictionary(Of Button, Integer)()
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)

        'Dim data As New SqlConnection
        'MessageBox.Show("Button clicked: " & (CType(sender, Button)).Content)

        Dim clickedButton As Button = TryCast(sender, Button)

        'clickedButton.Margin = New Padding(10, 5, 10, 5)
        'clickedButton.Padding = New Padding(10, 5, 10, 5)

        If clickedButton IsNot Nothing Then

            ' Access the details stored in the button's content
            Dim stackPanel As StackPanel = TryCast(clickedButton.Content, StackPanel)
            If stackPanel IsNot Nothing Then
                ' Access the text blocks inside the stack panel

                Dim nameTextBlock As TextBlock = TryCast(stackPanel.Children(1), TextBlock)
                Dim priceTextBlock As TextBlock = TryCast(stackPanel.Children(stackPanel.Children.Count - 1), TextBlock)
                'Dim Quantextblock As New Integer

                '''''''''''''''''''''''''''
                'Dim nameTextBlock As TextBlock = TryCast(stackPanel.Children(1), TextBlock)
                'log.Children.Add(nameTextBlock)

                If nameTextBlock IsNot Nothing AndAlso priceTextBlock IsNot Nothing Then
                    ' Display details in a MessageBox
                    'MessageBox.Show($"Name: {nameTextBlock.Text}{Environment.NewLine}Price: {priceTextBlock.Text}", "Item Details")
                    'label.Text = $"Name: {nameTextBlock.Text}{Environment.NewLine}Price: {priceTextBlock.Text}"

                    ''
                    Dim l1, l2 As String
                    Dim l3 As Integer = 1

                    l1 = $"{nameTextBlock.Text}"
                    l2 = $"{priceTextBlock.Text}"
                    'l3 = Quantextblock


                    Dim panelKo As New StackPanel With {
                      .Orientation = Orientation.Vertical,
                      .Background = Brushes.Red
                    }

                    Dim fuck As New TextBlock With {
                        .Text = l1
                    }
                    Dim fuck1 As New TextBlock With {
                        .Text = l2
                    }
                    Dim fuck2 As New TextBlock With {
                        .Text = l3
                    }
                    Dim isexist = False
                    'For idx As Integer = 0 To log.Children.Count - 1
                    '    Dim stack As StackPanel = log.Children(idx)
                    '    Dim nameText = TryCast(stack.Children(0), TextBlock)
                    '    If nameText.Text = l1 Then
                    '        MsgBox("Meron Na!")
                    '        isexist = True
                    '        Exit For
                    '    End If
                    'Next
                    For idx As Integer = 0 To log.Children.Count - 1
                        Dim stack As StackPanel = log.Children(idx)
                        Dim nameText = TryCast(stack.Children(0), TextBlock)
                        If nameText.Text = l1 Then
                            isexist = True
                            l3 += 1
                            fuck2.Text = l3.ToString()
                            MsgBox("Meron Na!")
                            Return
                            Exit For
                        End If
                    Next

                    If Not isexist Then
                        panelKo.Children.Add(fuck)
                        panelKo.Children.Add(fuck1)
                        panelKo.Children.Add(fuck2)

                        log.Children.Add(panelKo)
                    End If
                    ''''''''''''''''''count the button if it is same
                    'clickedButton = DirectCast(sender, Button)
                    'clickCounts(clickedButton) += 1
                    'MessageBox.Show($"Clicked {label4.Text} {clickCounts(clickedButton)} times.")

                    '''''''''''''''''''''''''
                    'label1.Text = $"{nameTextBlock.Text}"
                    'label2.Text = $"{priceTextBlock.Text}"









                    '''''
                    '' seperating the integer from string
                    'Dim randomString As String = priceTextBlock.Text

                    '' Use a regular expression to extract the number
                    'Dim match As Match = Regex.Match(randomString, "[\d,\.]+")

                    '' Check if a match was found
                    'If match.Success Then
                    '    ' Remove commas and convert the matched value to a double
                    '    Dim numberString As String = match.Value.Replace(",", "")
                    '    Dim extractedNumber As Double

                    '    ' Try to parse the number
                    '    If Double.TryParse(numberString, extractedNumber) Then

                    '        'label3.Text = extractedNumber

                    '    End If

                    '    'label3.Text = priceTextBlock.Text



                    'End If
                End If

            End If
        End If
    End Sub
End Class
