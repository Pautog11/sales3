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
Imports SalesMonitoringSystem.ProductsPanel

Public Class ProductDialog

    Private _tableAdapter As New sgsmsdbTableAdapters.viewtblcategoriesTableAdapter
    Private _subject As IObservablePanel
    Private _data As viewtblproductsRow = Nothing ', _data1 As viewtblproductsRow = Nothing
    Public imagePathD As String
    'Public imageSource As BitmapImage
    'Dim a As New InsertImage
    Public Sub New(
        Optional subject As IObservablePanel = Nothing,
        Optional data As viewtblproductsRow = Nothing
        )
        'Optional data1 As viewtblproductsRow = Nothing
        ')
        InitializeComponent()

        _data = data
        _subject = subject
        DataContext = data
        If _data IsNot Nothing Then
            SaveButton.Content = "UPDATE"
        Else
            DeleteButton.Visibility = Visibility.Collapsed
        End If
    End Sub


    '===========================================


    '===========================================

    Private Sub ProductDialog_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        CategoryComboBox.ItemsSource = _tableAdapter.GetData().DefaultView
        CategoryComboBox.DisplayMemberPath = "CATEGORY_NAME"
        CategoryComboBox.SelectedValuePath = "ID"

        If _data Is Nothing Then
            CategoryComboBox.SelectedIndex = 0
        Else
            CategoryComboBox.SelectedValue = If(DBNull.Value.Equals(_data.Item("CATEGORY_ID")), -1, _data.Item("CATEGORY_ID"))
        End If

        '        If (DataSet.Tables[0].Rows.Count == 1)
        '{
        '    Byte[] data = New Byte[0];
        '    Data = (Byte[])(dataSet.Tables[0].Rows[0]["pic"]);
        '    MemoryStream mem = New MemoryStream(Data);
        '    yourPictureBox.Image = Image.FromStream(mem);
        '} 

        'Dim ms As New IO.MemoryStream(CType(plr.PlayerImage, Byte())) 'This is correct...
        'Dim returnImage As Image = Image.FromStream(ms)
        'pcbEditPlayer.Image = returnImage

        'If _data IsNot Nothing Then
        '    Dim ms As New MemoryStream(CType(_data.PRODUCT_IMAGE, Byte()))
        '    Dim b As New BitmapImage
        '    b.BeginInit()
        '    b.StreamSource = ms
        '    b.EndInit()
        '    selectedImage.Source = b
        '    'Else
        '    '    selectedImage.Source = Nothing
        'End If

        If _data IsNot Nothing Then
            If _data.PRODUCT_IMAGE IsNot Nothing Then
                Try
                    Dim ms As New MemoryStream(CType(_data.PRODUCT_IMAGE, Byte()))
                    Dim b As New BitmapImage
                    b.BeginInit()
                    b.StreamSource = ms
                    b.EndInit()
                    selectedImage.Source = b
                Catch ex As Exception
                    selectedImage.Source = Nothing ' Clear the image source
                    'selectedImage.Content = "No Image" ' Display text "No Image"
                End Try
            Else
                selectedImage.Source = Nothing ' Clear the image source
                'selectedImage.Content = "No Image" ' Display text "No Image"
            End If
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


            ' ----------------------cosio -----------------
            If ProductPriceTextBox.Text < ProductCostTextBox.Text Then
                Growl.Info("Should not lessthan from the cost price")
                Return ' ----------------cosio -----------
            End If

            Dim imageSource As BitmapImage = CType(selectedImage.Source, BitmapImage)
            'Dim imgTOIMG As Image = Image.FromFile(imagePathD)

            Dim imgTOIMG As Image = Nothing ' Initialize to null initially

            If imagePathD IsNot Nothing AndAlso File.Exists(imagePathD) Then
                imgTOIMG = Image.FromFile(imagePathD)
            End If

            '---------------------------------------------------------

            Dim data As New Dictionary(Of String, String) From {
                    {"id", _data?.Item("ID")},
                    {"category_id", CategoryComboBox.SelectedValue},
                    {"product_name", res(0)(1)},
                    {"product_description", If(String.IsNullOrEmpty(ProductDescriptionTextBox.Text), "", ProductDescriptionTextBox.Text)},
                    {"product_price", res(1)(1)},
                    {"product_cost", res(2)(1)},
                    {"product_image", If(imageSource IsNot Nothing, imageSource.ToString(), DBNull.Value.ToString())}
            }
            baseCommand = New BaseProduct(data) With {
                .Image = imgTOIMG
            }
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
            CloseDialog(Closebtn)
        Else
            Growl.Info("Please fill out the empty field(s) or input a valid data.")
        End If

    End Sub

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

    Private Sub SelectImageButton_Click(sender As Object, e As RoutedEventArgs)
        Dim openFileDialog As New Microsoft.Win32.OpenFileDialog()
        openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp|All files (*.*)|*.*"

        If openFileDialog.ShowDialog() = True Then
            Dim imagePath As String = openFileDialog.FileName
            imagePathD = imagePath
            Dim imageSource As New BitmapImage(New Uri(imagePath))

            selectedImage.Source = imageSource

        End If
        'selectedImage.Source = ImageSource
    End Sub

    'Public Sub DisplayImage(imageData As Byte())
    '    Dim bitmapImage As New BitmapImage()
    '    bitmapImage.BeginInit()
    '    bitmapImage.CacheOption = BitmapCacheOption.OnLoad
    '    bitmapImage.StreamSource = New MemoryStream(imageData)
    '    bitmapImage.EndInit()

    '    selectedImage.Source = bitmapImage
    'End Sub
End Class
Public Class Image123
    Public selectedImage As String
End Class
