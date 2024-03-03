Imports HandyControl.Controls
Imports HandyControl.Tools.Extension
Imports System.Windows.Input

Public Class StackProductNotification
    Public Property _parent1 As IObservablePanel

    Private Sub StackProductNotification_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles Me.MouseLeftButtonDown
        'Dim inventory_dialog As New InventoryPanel
        'Dialog.Show(inventory_dialog)
        Try
            Dim dash As New Dashboard

            'AddHandler dash.BottomContainerInventoryButton.Click, AddressOf BottomContainerInventoryButtonClick
            'dash.BottomContainerInventoryButton.RaiseEvent(New RoutedEventArgs(Button.ClickEvent))
            ''dash.BottomContainerInventoryButton(Me, New MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left))
            dash.Collapse
            dash.BottomContainerInventoryButton.Visibility = True


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub
End Class
