
Imports System.Diagnostics.Eventing.Reader
Imports System.Security.Policy
Imports HandyControl.Controls
Public Class Login
    Private _loginModule As New LoginModule

    Private Sub LoginButton_Click(sender As Object, e As RoutedEventArgs) Handles LoginButton.Click
        Dim res As Object() = Nothing
        Dim controls As Object() = {UsernameTextBox, PasswordTextBox}
        Dim types As DataInput() = {DataInput.STRING_USERNAME, DataInput.STRING_PASSWORD}

        If BaseAccount.CountUser() = 0 Then
            Dim createadmin As New AdminAcc
            Dim res1 As MessageBoxResult = MessageBox.Ask("You will be directed to create account Panel.")
            If res1 = MessageBoxResult.OK Then
                createadmin.Show()
            End If
            UsernameTextBox.Text = ""
            PasswordTextBox.Password = ""
            Return
        End If

        Dim vres As New List(Of Object())
        For i = 0 To controls.Count - 1
            vres.Add(InputValidation.ValidateInputString(controls(i), types(i)))
        Next

        If Not vres.Any(Function(item As Object()) Not item(0)) Then
            res = _loginModule.LoginAccount(UsernameTextBox.Text, PasswordTextBox.Password)

            If res?(0) Then
                Dim dash As New Dashboard
                'If My.Settings.userRole <> 1 Then
                '    dash.BottomContainerProductsButton.Visibility = Visibility.Collapsed
                '    dash.BottomContainerLogsButton.Visibility = Visibility.Collapsed
                '    dash.poss.Visibility = Visibility.Collapsed                             '''  cosio
                '    'dash.SalesToday.Visibility = Visibility.Collapsed
                '    Dim tabs As ItemCollection = dash.MaintainanceContainer.TabControlContainer.Items()
                '    tabs.Remove(dash.MaintainanceContainer.AccountTab)
                '    tabs.Remove(dash.MaintainanceContainer.CategoryTab)
                '    tabs.Remove(dash.MaintainanceContainer.SupplierTab)
                'ElseIf My.Settings.userRole = 2 Then
                '    dash.BottomContainerProductsButton.Visibility = Visibility.Collapsed
                '    dash.BottomContainerLogsButton.Visibility = Visibility.Collapsed
                '    dash.BottomContainerTransactionsButton.Visibility = Visibility.Collapsed
                '    dash.BottomContainerMaintenaceButton.Visibility = Visibility.Collapsed
                '    dash.BottomContainerInventoryButton.Visibility = Visibility.Collapsed
                '    dash.BottomContainerSalesReportButton.Visibility = Visibility.Collapsed
                '    dash.BottomContainerLogsButton.Visibility = Visibility.Collapsed
                'End If

                Select Case My.Settings.userRole
                    Case 1 ' Super Admin
                        ' Handle Super Admin
                        'dash.BottomContainerProductsButton.Visibility = Visibility.Collapsed
                        'dash.BottomContainerLogsButton.Visibility = Visibility.Collapsed
                        'dash.poss.Visibility = Visibility.Collapsed ' Not sure what this is
                        'dash.SalesToday.Visibility = Visibility.Collapsed
                        'Dim tabs As ItemCollection = dash.MaintainanceContainer.TabControlContainer.Items()
                        'tabs.Remove(dash.MaintainanceContainer.AccountTab)
                        'tabs.Remove(dash.MaintainanceContainer.CategoryTab)
                        'tabs.Remove(dash.MaintainanceContainer.SupplierTab)
                    Case 2 ' Stock Holder
                        ' Handle Stock Holder
                        dash.BottomContainerProductsButton.Visibility = Visibility.Collapsed
                        dash.BottomContainerLogsButton.Visibility = Visibility.Collapsed
                        dash.BottomContainerTransactionsButton.Visibility = Visibility.Collapsed
                        dash.BottomContainerMaintenaceButton.Visibility = Visibility.Collapsed
                        dash.BottomContainerInventoryButton.Visibility = Visibility.Collapsed
                        dash.BottomContainerSalesReportButton.Visibility = Visibility.Collapsed
                        dash.BottomContainerLogsButton.Visibility = Visibility.Collapsed
                        dash.poss.Visibility = Visibility.Collapsed
                    Case 3 ' Admin
                        dash.BottomContainerProductsButton.Visibility = Visibility.Collapsed
                        dash.BottomContainerLogsButton.Visibility = Visibility.Collapsed
                        dash.poss.Visibility = Visibility.Collapsed ' Not sure what this is
                        'dash.SalesToday.Visibility = Visibility.Collapsed
                        Dim tabs As ItemCollection = dash.MaintainanceContainer.TabControlContainer.Items()
                        tabs.Remove(dash.MaintainanceContainer.AccountTab)
                        tabs.Remove(dash.MaintainanceContainer.CategoryTab)
                        '    tabs.Remove(dash.MaintainanceContainer.SupplierTab)
                        'Case Else
                        '    ' Handle other roles or invalid roles (optional)
                        '    MessageBox.Info("Invalid user role!", "Login Failed!")
                        '    Return ' or any other appropriate action
                End Select
                dash.Show()
                Close()
            Else
                MessageBox.Info(res(1), "Login Failed!")
            End If
        End If
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs) Handles CloseButton.Click
        Close()
    End Sub
End Class
