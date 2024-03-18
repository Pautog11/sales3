Imports HandyControl.Controls
Imports HandyControl.Tools.Extension

Public Class WelcomeDialog
    Private Sub ContinueButton_Click(sender As Object, e As RoutedEventArgs) Handles ContinueButton.Click
        'TODO OPEN THE TUTORIAL
        Me.Hide
        Dialog.Show(New Dashboard)
    End Sub

    'Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs) Handles CloseButton.Click
    '    CloseDialog(CloseButton)
    'End Sub
End Class
