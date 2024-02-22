Imports System.Data
Imports System.Drawing.Text
Imports System.IO
Imports System.Windows.Forms
Imports HandyControl.Controls
Imports HandyControl.Tools.Extension
Imports SalesMonitoringSystem.sgsmsdb
Imports System.Drawing
Imports HandyControl.Tools
Imports System.Data.SqlClient
Imports Microsoft.Win32
Imports HandyControl
Imports System.Security.Cryptography.X509Certificates
Imports System.Text

Public Class ProductDialog
    Private _tableAdapter As New sgsmsdbTableAdapters.viewtblcategoriesTableAdapter
    Private _subject As IObservablePanel
    Private _data As viewtblproductsRow = Nothing, _data1 As viewtblproductsRow = Nothing
    Public imagePath As String
    'Public imageSource As BitmapImage
    'Dim a As New InsertImage
    Public Sub New(
        Optional subject As IObservablePanel = Nothing,
        Optional data As viewtblproductsRow = Nothing,
        Optional data1 As viewtblproductsRow = Nothing
    )
        InitializeComponent()

        _data = data
        _data1 = data1
        '_data = data1
        _subject = subject
        DataContext = data  'data1
        DataContext = data1
        If _data IsNot Nothing Then
            SaveButton.Content = "UPDATE"
        Else
            DeleteButton.Visibility = Visibility.Collapsed
        End If

    End Sub

    Private Sub ProductDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        CategoryComboBox.ItemsSource = _tableAdapter.GetData().DefaultView
        CategoryComboBox.DisplayMemberPath = "CATEGORY_NAME"
        CategoryComboBox.SelectedValuePath = "ID"

        If _data Is Nothing Then
            CategoryComboBox.SelectedIndex = 0
        Else
            CategoryComboBox.SelectedValue = If(DBNull.Value.Equals(_data.Item("CATEGORY_ID")), -1, _data.Item("CATEGORY_ID"))
        End If
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click
        Dim controls As Object() = {ProductNameTextBox, ProductPriceTextBox, ProductCostTextBox, CategoryComboBox}
        Dim types As DataInput() = {DataInput.STRING_STRING, DataInput.STRING_PRICE, DataInput.STRING_PRICE, DataInput.STRING_STRING}

        Dim res As New List(Of Object())
        For i = 0 To controls.Count - 1
            res.Add(InputValidation.ValidateInputString(controls(i), types(i)))
        Next

        Dim baseCommand As ICommandPanel = Nothing
        Dim invoker As ICommandInvoker = Nothing
        If Not res.Any(Function(item As Object()) Not item(0)) Then
            If CategoryComboBox.SelectedIndex = -1 Then
                Growl.Info("Please select a category.")
                Return
            End If


            'ddddddddddddddddddddddddddddddddddddddddddddd

            ' ----------------------cosio -----------------
            'If ProductPriceTextBox.Text < ProductCostTextBox.Text Then
            '    Growl.Info("Should not lessthan from the cost price")
            '    Return ' ----------------cosio -----------
            'End If




            'Dim selectedImage As Image = ImageSelector.SelectedImage
            'Dim selectedImage As ImageSource = ImageSelector.image123
            'Dim image As String = ConvertImageToBase64(selectedImage)

            'Dim imagesource As New BitmapImage

            'Dim imageStream As New MemoryStream()

            ' Convert the image data from the Image control to a byte array

            'Dim encoder As New JpegBitmapEncoder()
            'encoder.Frames.Add(BitmapFrame.Create(selectedImage.Source))
            'encoder.Save(imageStream)
            'Dim imageData As Byte() = imageStream.ToArray()


            'Dim ms As New MemoryStream()
            'selectedImage.Source(ms, selectedImage.Source)
            'Dim data1 As Byte() = ms.GetBuffer()
            ''Dim p As New SqlParameter("@image", SqlDbType.Image)
            'p.Value = data1
            ''Dim ms As New MemoryStream()
            ''selectedImage.Image.Save(ms, selectedImage.Image.RawFormat)

            'Dim imagePath As String = OpenFileDialog.FileName
            'Dim itemsouer
            'Growl.Info("base64String")
            'Dim imageSource As New Byte
            'Dim imagepath As String
            'Dim imageBytes As Byte() = File.ReadAllBytes(imagepath)
            'Dim imageBytes As Byte() = File.ReadAllBytes(imageSource)



            Dim imageSource As BitmapImage = CType(selectedImage.Source, BitmapImage)

            '' Convert the image source to bytes
            'Dim encoder As New JpegBitmapEncoder()
            'Dim bitmapImage As New BitmapImage()
            'bitmapImage.BeginInit()
            'bitmapImage.UriSource = imageSource.UriSource
            'bitmapImage.EndInit()

            'Dim imageStream As New MemoryStream()
            'encoder.Frames.Add(BitmapFrame.Create(bitmapImage))
            'encoder.Save(imageStream)

            'Dim imageBytes As Byte() = imageStream.ToArray()
            'Dim base64String As String = Convert.ToBase64String(imageBytes)
            ''Dim byteArray As Byte() = Encoding.GetBytes(imageBytes)

            '---------------------------------------------------------
            'Dim imageSource As Bitmap
            Dim data As New Dictionary(Of String, String) From {
                    {"id", _data?.Item("ID")},
                    {"category_id", CategoryComboBox.SelectedValue},
                    {"product_name", res(0)(1)},
                    {"product_description", If(String.IsNullOrEmpty(ProductDescriptionTextBox.Text), "", ProductDescriptionTextBox.Text)},
                    {"product_price", res(1)(1)},
                    {"product_cost", res(2)(1)},
                    {"product_image", ImageSource.ToString}
            }
            '{"product_image", selectedImage.TouchesCaptured}
            '}

            'Dim data1 As New Dictionary(Of Byte, Byte) From {
            '    {"product_image", imageBytes}
            '    }

            '/////////////////////////////////////////////////////
            baseCommand = New BaseProduct(data)
            'baseCommand = New BaseProduct(data1)
            If BaseProduct.Exists(res(0)(1)) <= 0 AndAlso _data Is Nothing Then
                invoker = New AddCommand(baseCommand)
            ElseIf _data IsNot Nothing Then
                invoker = New UpdateCommand(baseCommand)
            Else
                Growl.Info("Product exists!")
            End If
            invoker?.Execute()
            _subject?.NotifyObserver()
            'SaveImageToDatabase()
            'CloseDialog(Closebtn)
        Else
            Growl.Info("Please fill out the empty field(s) or input a valid data.")
        End If

    End Sub

    Private Function ConvertImageToBase64(image As Image) As String
        Dim ms As New MemoryStream()
        image.Save(ms, System.Drawing.Imaging.ImageFormat.Png) ' Save the image to a memory stream in PNG format (change format if needed)
        Dim imageData As Byte() = ms.ToArray()
        Return Convert.ToBase64String(imageData)
    End Function

    Private Sub DeleteButton_Click(sender As Object, e As RoutedEventArgs) Handles DeleteButton.Click
        Dim baseCommand As New BaseProduct(New Dictionary(Of String, String) From {{"id", _data.Item("ID")}})
        Dim invoker As New DeleteCommand(baseCommand)

        invoker?.Execute()
        _subject?.NotifyObserver()
        CloseDialog(Closebtn)
    End Sub

    Private Sub Closebtn_Click(sender As Object, e As RoutedEventArgs) Handles Closebtn.Click
        _subject?.NotifyObserver()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        'Dim openFileDialog1 As New OpenFileDialog()

        '' Set the file dialog properties
        'openFileDialog1.InitialDirectory = "C:\" ' Set initial directory
        'openFileDialog1.Filter = "All files (*.png)|*.png|All files (*.jpg)|*.jpg" ' Set filter options

        '' Show the file dialog and check if the user selected a file
        'If openFileDialog1.ShowDialog() = DialogResult.OK Then
        '    ' Get the selected file name and display it in a text box
        '    PictureBox.Text = openFileDialog1.FileName
        'End If

        'Me.Visibility = Visibility.Collapsed
        'Dim a As New InsertImage
        'a.Show()
        'Me.Visibility = Visibility.Collapsed


        'Dim insert As InsertPic
        'insert.Show()
        'Dialog.Show(New UserControl1())

        'Dialog.Show(New UserControl1(subject:=_subject))
        'Dim panel As New UserControl1
        'panel.Show
    End Sub

    Private Sub SelectImageButton_Click(sender As Object, e As RoutedEventArgs)
        Dim openFileDialog As New Microsoft.Win32.OpenFileDialog()
        openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp|All files (*.*)|*.*"

        If openFileDialog.ShowDialog() = True Then
            Dim imagePath As String = openFileDialog.FileName
            Dim imageSource As New BitmapImage(New Uri(imagePath))
            Growl.Info("dsjjd")
            ' Set the source of the Image control to display the selected image
            selectedImage.Source = imageSource

            'Return imageSource
        End If
        'selectedImage.Source = ImageSource
    End Sub

    'Private Sub SaveImageToDatabase()
    '    'Dim imagePath As String
    '    Try
    '        Dim imageBytes As Byte() = File.ReadAllBytes(imagePath)
    '        Dim connectionString As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Christian\OneDrive\Desktop\sales3\SalesMonitoringSystem\sgsmsdb.mdf;Integrated Security=True"
    '        Using connection As New SqlConnection(connectionString)
    '            connection.Open()
    '            Dim query As String = "INSERT INTO tblproducts (product_image) VALUES (@product_image)"
    '            Using command As New SqlCommand(query, connection)
    '                command.Parameters.AddWithValue("@im", imageBytes)
    '                command.ExecuteNonQuery()
    '                'Windows.MessageBox.Show("Image has been saved to database.", "Success", MessageBoxButton.OK, MessageBoxImage.Information)
    '                'Growl("Fuckme!")
    '            End Using
    '        End Using

    '    Catch ex As Exception
    '        'Windows.MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
    '    End Try
    'End Sub

    'Public imageSource
End Class
