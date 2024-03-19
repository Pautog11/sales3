Imports System.Windows.Markup
Imports HandyControl.Controls
Imports HandyControl.Tools.Extension
Public Class AdminAcc
    Private ReadOnly _data As Dictionary(Of String, String)
    Private ReadOnly _subject As IObservablePanel

    Private ReadOnly _loginModule As New LoginModule
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub SignUp_Click(sender As Object, e As RoutedEventArgs) Handles SignUp.Click
        Dim controls As Object() = {
            FirstNameTextBox, LastNameTextBox, AddressTextBox, ContactTextBox,
            UsernameTextBox, PasswordTextBox
        }

        Dim types As DataInput() = {
            DataInput.STRING_NAME, DataInput.STRING_NAME, DataInput.STRING_STRING,
            DataInput.STRING_PHONE, DataInput.STRING_USERNAME, DataInput.STRING_PASSWORD
        }

        Dim result As New List(Of Object())
        For i = 0 To controls.Count - 1
            result.Add(InputValidation.ValidateInputString(controls(i), types(i)))
        Next

        If Not result.Any(Function(item As Object()) Not item(0)) Then
            If BaseAccount.Exists(result(5)(1)) = 0 Then
                Dim data As New Dictionary(Of String, String) From {
                    {"id", _data?.Item("id")},
                    {"role_id", 1},
                    {"first_name", result(0)(1)},
                    {"last_name", result(1)(1)},
                    {"address", result(2)(1)},
                    {"contact", result(3)(1)},
                    {"username", result(4)(1)},
                    {"password", result(5)(1)}
                }
                Dim baseCommand As New BaseAccount(data)
                Dim invoker As ICommandInvoker
                If _data Is Nothing Then
                    invoker = New AddCommand(baseCommand)
                    invoker.Execute()
                End If
                'Dim ask As MessageBoxResult = MessageBox.Ask("You will be directed to login page")
                'Dialog.Show("You will be directed to login page")
                MessageBox.Show("You will be directed to login page")
                'Dialog.Show(New WelcomeDialog) 
                'If ask = MessageBoxResult.OK Then
                Dim log As New Login
                log.Show()
                'Dialog.Show(New WelcomeDialog)
                'Dim a As New WelcomeDialog
                'a.Show
                Me.Hide()
                'End If
            End If
        End If
        Return
    End Sub
End Class
