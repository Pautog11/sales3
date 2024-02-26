Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Windows.Forms
Imports HandyControl.Controls
Imports HandyControl.Data
Imports SalesMonitoringSystem.ProductDialog
Imports System.Windows.Media.Imaging


Public Class ProductsPanel
    Implements IObserverPanel
    Private _tableAdapter As New sgsmsdbTableAdapters.viewtblproductsTableAdapter
    Private _dataTable As New sgsmsdb.viewtblproductsDataTable
    Private _subject As IObservablePanel
    Private Const MAX_PAGE_COUNT = 30

    Public Sub New()
        InitializeComponent()
        Try
            _subject = Application.Current.Windows.OfType(Of Dashboard).FirstOrDefault
            _subject?.RegisterObserver(Me)
            _subject?.NotifyObserver()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Error(ex.Message, "Observer Error")
        End Try
    End Sub

    Public Sub Update() Implements IObserverPanel.Update
        _tableAdapter.Fill(_dataTable)
        ProductDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)

        PaginationConfig()
    End Sub

    Private Sub AddButton_Click(sender As Object, e As RoutedEventArgs) Handles AddButton.Click
        Dialog.Show(New ProductDialog(subject:=_subject))
    End Sub

    Private Sub ProductDataGridView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ProductDataGridView.SelectionChanged
        'Private Sub ProductDataGridView_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ProductDataGridView.SelectionChanged


        If ProductDataGridView.SelectedItems.Count > 0 Then
            Dialog.Show(New ProductDialog(_subject, ProductDataGridView.SelectedItems(0)))
            ProductDataGridView.SelectedIndex = -1

            '' Fetch the selected product details including the image data
            'Dim selectedProduct As Product = ProductDataGridView.SelectedItems(0)

            '' Create a new instance of the ProductDialog form
            'Dim productDialogForm As New ProductDialog(_subject, selectedProduct)

            '' Set the image to the PictureBox control in the ProductDialog form
            'productDialogForm.PictureBox1.Image = ConvertByteArrayToImage(selectedProduct.ImageData)

            '' Show the ProductDialog form
            'Dialog.Show(productDialogForm)

            '' Deselect the selected row in the DataGridView
            'ProductDataGridView.SelectedIndex = -1
        End If
    End Sub

    Private Sub ProductSearch_SearchStarted(sender As Object, e As FunctionEventArgs(Of String)) Handles ProductSearch.SearchStarted
        _dataTable = BaseProduct.Search(ProductSearch.Text)
        ProductDataGridView.ItemsSource = _dataTable.Take(MAX_PAGE_COUNT)
        PaginationConfig()
    End Sub

    ''' <summary>
    ''' To configure the paginations pages
    ''' </summary>
    Private Sub PaginationConfig()
        If _dataTable.Count <= MAX_PAGE_COUNT Then
            Pagination.Visibility = Visibility.Collapsed
            Return
        Else
            Pagination.Visibility = Visibility.Visible
        End If

        If MAX_PAGE_COUNT / _dataTable.Count > 0 Then
            Pagination.MaxPageCount = _dataTable.Count / MAX_PAGE_COUNT + 1
        Else
            Pagination.MaxPageCount = _dataTable.Count / MAX_PAGE_COUNT
        End If
    End Sub


    Private Function GetImageDataFromDatabase(id As Integer) As Byte()
        Dim imageData As Byte() = Nothing

        Using connection As New SqlConnection(My.Settings.sgsmsdbConnectionString)
            Dim query As String = "SELECT product_image FROM Products WHERE id = @id"
            Dim command As New SqlCommand(query, connection)
            command.Parameters.AddWithValue("id", id)

            connection.Open()
            Dim reader As SqlDataReader = command.ExecuteReader()

            If reader.Read() Then
                If Not reader.IsDBNull(0) Then
                    imageData = DirectCast(reader("product_image"), Byte())
                End If
            End If

            reader.Close()
        End Using

        Return imageData
    End Function
End Class