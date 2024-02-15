Imports System.Reflection.Emit
Imports System.Windows.Forms
Imports HandyControl.Controls
Public Class InsertImage
    Private Sub New()

    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        ''Dim openFileDialog1 As New OpenFileDialog()

        ''' Set the file dialog properties
        ''openFileDialog1.InitialDirectory = "C:\" ' Set initial directory
        ''openFileDialog1.Filter = "All files (*.png)|*.png|All files (*.jpg)|*.jpg" ' Set filter options

        ''' Show the file dialog and check if the user selected a file
        ''If openFileDialog1.ShowDialog() = DialogResult.OK Then
        ''    ' Get the selected file name and display it in a text box
        ''    Image.text = openFileDialog1.FileName
        ''End If

        ''openFileDialog1.ShowDialog()
        ''Label4.Text = openFileDialog1.FileName
        ''If openFileDialog1.ShowDialog = DialogResult.OK Then
        ''    ImageViewer.source(Me.OpenFileDialog1.FileName)
        ''End If



        'Dim openFileDialog1 As New OpenFileDialog()

        '' Set the file dialog properties
        'openFileDialog1.InitialDirectory = "C:\" ' Set initial directory
        'openFileDialog1.Filter = "All files (*.png;*.jpg)|*.png;*.jpg" ' Set filter options

        '' Show the file dialog and check if the user selected a file
        'If openFileDialog1.ShowDialog() = True Then
        '    ' Get the selected file name and display it
        '    Dim ImageViewer1 As New ImageViewer
        '    ImageViewer1.Source = New Uri(openFileDialog1.FileName)
        'End If

        Dim openFileDialog1 As New OpenFileDialog()

        ' Set the file dialog properties
        openFileDialog1.InitialDirectory = "C:\" ' Set initial directory
        openFileDialog1.Filter = "All files (*.png;*.jpg)|*.png;*.jpg" ' Set filter options

        ' Show the file dialog and check if the user selected a file
        If openFileDialog1.ShowDialog() = True Then
            ' Create a BitmapImage object and set its URI source
            Dim bitmapImage As New BitmapImage()
            bitmapImage.BeginInit()
            bitmapImage.UriSource = New Uri(openFileDialog1.FileName)
            bitmapImage.EndInit()

            ' Set the Image control's source to the loaded image

            'ImageViewer.source = bitmapImage

            Dim newImage As New Image()
            newImage.Source = bitmapImage
        End If
    End Sub
End Class
