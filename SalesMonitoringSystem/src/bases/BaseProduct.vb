Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports HandyControl.Controls
Imports SalesMonitoringSystem.Pos

Public Class BaseProduct
    Inherits SqlBaseConnection
    Implements ICommandPanel

    Private ReadOnly _data As Dictionary(Of String, String)
    Private img As System.Drawing.Image
    Public Sub New(data As Dictionary(Of String, String))
        _data = data
    End Sub

    Public Property Image() As System.Drawing.Image
        Set(value As System.Drawing.Image)
            img = value
        End Set
        Get
            Return img
        End Get
    End Property

    Public Sub Delete() Implements ICommandPanel.Delete
        Try
            _sqlCommand = New SqlCommand("EXEC DeleteProductProcedure @id, @user_id;", _sqlConnection)
            _sqlCommand.Parameters.AddWithValue("@id", _data.Item("id"))
            _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
            If _sqlCommand.ExecuteNonQuery() > 0 Then
                Growl.Success("Product has been deleted successfully!")
            Else
                Growl.Error("Failed deleting the product!")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub Update() Implements ICommandPanel.Update
        Try
            _sqlCommand = New SqlCommand("EXEC UpdateProductProcedure @id, @category_id, @product_name, @product_description, @product_price, @product_cost, @user_id;", _sqlConnection)
            _sqlCommand.Parameters.AddWithValue("@id", _data.Item("id"))
            _sqlCommand.Parameters.AddWithValue("@category_id", _data.Item("category_id"))
            _sqlCommand.Parameters.AddWithValue("@product_name", _data.Item("product_name"))
            _sqlCommand.Parameters.AddWithValue("@product_description", If(String.IsNullOrEmpty(_data.Item("product_description")), DBNull.Value, _data.Item("product_description")))
            _sqlCommand.Parameters.AddWithValue("@product_price", _data.Item("product_price"))
            _sqlCommand.Parameters.AddWithValue("@product_cost", _data.Item("product_cost"))
            _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
            If _sqlCommand.ExecuteNonQuery() > 0 Then
                Growl.Success("Product has been updated successfully!")
            Else
                Growl.Error("Failed updating the product!")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub Add() Implements ICommandPanel.Add
        Try
            _sqlCommand = New SqlCommand("EXEC InsertProductProcedure @category_id, @product_name, @product_description, @product_price, @product_cost,@product_image, @user_id;", _sqlConnection)
            _sqlCommand.Parameters.AddWithValue("@category_id", _data.Item("category_id"))
            _sqlCommand.Parameters.AddWithValue("@product_name", _data.Item("product_name"))
            _sqlCommand.Parameters.AddWithValue("@product_description", If(String.IsNullOrEmpty(_data.Item("product_description")), DBNull.Value, _data.Item("product_description")))
            _sqlCommand.Parameters.AddWithValue("@product_price", _data.Item("product_price"))
            _sqlCommand.Parameters.AddWithValue("@product_cost", _data.Item("product_cost"))
            _sqlCommand.Parameters.AddWithValue("@user_id", My.Settings.userID)
            Dim converter As New ImageConverter
            Dim byteArr As Byte() = converter.ConvertTo(img, GetType(Byte()))
            '_sqlCommand.Parameters.AddWithValue("@product_image", byteArr).SqlDbType = SqlDbType.Image

            'null image will be accepted as fuck
            If byteArr IsNot Nothing Then
                _sqlCommand.Parameters.AddWithValue("@product_image", byteArr).SqlDbType = SqlDbType.Image
            Else
                'If the image Is null, you can add a DBNull.Value parameter
                _sqlCommand.Parameters.AddWithValue("@product_image", DBNull.Value).SqlDbType = SqlDbType.Image
            End If



            'Dim imageData As Byte() = GetImageData()

            'Command.Parameters.Add("@ImageData", SqlDbType.VarBinary, -1).Value = imageData
            '_sqlCommand.Parameters.Add("@product_image", SqlDbType.VarBinary, -1).Value = "product_image"
            'If data1.ContainsKey("product_image") AndAlso TypeOf data1("product_image") Is Byte() Then
            '    Dim imageBytes As Byte() = DirectCast(data1("product_image"), Byte())

            '    ' Add the image bytes as parameter
            '    _sqlCommand.Parameters.Add("@product_image", SqlDbType.Image).Value = imageBytes
            'Else
            '    MessageBox.Show("Error: 'product_image' data is not valid.")
            'End If


            If _sqlCommand.ExecuteNonQuery() > 0 Then
                Growl.Success("Product has been added successfully!")
            Else
                Growl.Error("Failed adding the product")
            End If
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Shared Function ProductInfo(id As String) As DataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT PRICE, COST_PRICE FROM viewtblproducts WHERE id = @id", conn)
            cmd.Parameters.AddWithValue("@id", id)
            Dim dTable As New DataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)
            Return dTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New DataTable
        End Try
    End Function

    Public Shared Function ScalarProducts() As Integer
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT COUNT(*) FROM tblproducts", conn)
            Return cmd.ExecuteScalar()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return 0
        End Try
    End Function

    Public Shared Function Exists(name As String) As Integer
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT COUNT(*) FROM viewtblproducts WHERE LOWER(PRODUCT_NAME) = LOWER(@name)", conn)
            cmd.Parameters.AddWithValue("@name", name.Trim.ToLower)
            Return cmd.ExecuteScalar()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return 0
        End Try
    End Function

    Public Shared Function Search(query As String) As sgsmsdb.viewtblproductsDataTable
        Try
            Dim conn As SqlConnection = SqlConnectionSingleton.GetInstance
            Dim cmd As New SqlCommand("SELECT * FROM viewtblproducts WHERE PRODUCT_NAME LIKE CONCAT('%', @query, '%')", conn)
            cmd.Parameters.AddWithValue("@query", query)
            Dim dTable As New sgsmsdb.viewtblproductsDataTable
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dTable)
            Return dTable
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Show(ex.Message)
            Return New sgsmsdb.viewtblproductsDataTable
        End Try
    End Function
End Class