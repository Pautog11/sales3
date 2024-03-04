Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports HandyControl.Controls
Imports HandyControl.Tools.Converter
Imports HandyControl.Tools.Extension

''' <summary>
''' An class that implements the IObservablePanel and IObseverPanel.
''' This is the second window of this system and it holds different Panels(UserControl) that implements the IObserverPanel.
''' @see /src/interfaces/IObserverPanel
''' @see /src/interfaces/IObservablePanel
''' </summary>
Class Dashboard
    Implements IObservablePanel, IObserverPanel

    ''' <summary>
    ''' List of observers registered to this Observale.
    ''' </summary>
    Private _observables As New List(Of IObserverPanel)

    Public Sub New()
        InitializeComponent()
        RegisterObserver(Me)
        NotifyObserver()
        'StartMonitoring()

        'Dim StackPanel As StackPanel = StackNotifContainer
        'Dim newMessage As New TextBlock()
        'newMessage.Text = ""
        'StackPanel.Children.Add(newMessage)
    End Sub

    Public Sub RegisterObserver(o As IObserverPanel) Implements IObservablePanel.RegisterObserver
        _observables.Add(o)
    End Sub

    Public Sub NotifyObserver() Implements IObservablePanel.NotifyObserver
        For Each o As IObserverPanel In _observables
            o.Update()
        Next
    End Sub

    Public Sub Update() Implements IObserverPanel.Update
        Try
            StackNotifContainer.Children.Clear()
            LabelTotalProducts.Text = BaseProduct.ScalarProducts()
            LabelTotalSales.Text = BaseTransaction.ScalarSales()
            LabelTotalTransactions.Text = BaseTransaction.ScalarTransactions()

            Dim _itemSource As DataTable = BaseTransaction.FetchLatestTransactions()
            For Each item As DataRow In _itemSource.Rows
                Dim stackpan As New StackNotificationControl
                stackpan._parent = Me
                stackpan.LabelDateAdded.Text = CDate(item.Item("date_added")).ToLongDateString
                stackpan.LabelStackHeading.Text = item.Item("invoice_number")
                If item.Item("status_id") = &H2 Then
                    stackpan.ActiveIndicator.Background = Brushes.Transparent
                Else
                    stackpan.ActiveIndicator.Background = Brushes.Red
                    stackpan.Background = New BrushConverter().ConvertFromString("#E1F1FD")
                End If
                StackNotifContainer.Children.Add(stackpan)
                stackpan.Show
            Next

            CheckInventory()
        Catch ex As Exception
            MessageBox.Info(ex.Message)
        End Try

    End Sub

#Region "SwithPanelEvents"
    ' Use for panel switching
    Public Sub SwitchPanelEvents(sender As Object, e As EventArgs) Handles BottomContainerDashboardButton.Click,
        BottomContainerProductsButton.Click, BottomContainerTransactionsButton.Click, BottomContainerLogoutButton.Click,
        BottomContainerMaintenaceButton.Click, BottomContainerInventoryButton.Click, BottomContainerLogsButton.Click, BottomContainerSalesReportButton.Click, poss.Click

        If sender.Equals(BottomContainerLogoutButton) Then
            Dim res As MessageBoxResult = MessageBox.Ask("Do you want to log out?")
            If res = MessageBoxResult.OK Then
                My.Settings.userID = -1
                My.Settings.userRole = "None"
                My.Settings.Save()
                Dim ln As New Login
                ln.Show()
                Close()
            End If
            Return
        End If

        Dim panels As Object() = {
            DashboardPanel, ProductsPanel, TransactionsPanel, MaintenancePanel, InventoryPanel,
            AuditTrailPanel, SalesReportPanel, Pos
        }
        Dim buttons As Object() = {
            BottomContainerDashboardButton, BottomContainerProductsButton,
            BottomContainerTransactionsButton, BottomContainerMaintenaceButton,
            BottomContainerInventoryButton, BottomContainerLogsButton, BottomContainerSalesReportButton, poss
        }

        ' Collapse all the panels first before opening the desired panel
        For Each panel In panels
            panel.Visibility = Visibility.Collapsed
        Next

        ' Set the background back to it's original color
        For Each button In buttons
            button.Background = Brushes.Transparent
        Next

        ' Make the panel visible and change the button's background
        For i = 0 To buttons.Count - 1
            If sender.Equals(buttons(i)) Then
                buttons(i).Background = New BrushConverter().ConvertFromString("#FF76AFD2")
                panels(i).Visibility = Visibility.Visible
            End If
        Next


    End Sub
#End Region

#Region "TitleBarClickEvents"
    ' Use for the title bar click events, minimize, maximize, and close
    Private Sub TitleBarClickEvents(sender As Object, e As RoutedEventArgs) Handles CloseButton.Click, RestoreButton.Click,
        MinimizeButton.Click
        If sender.Equals(CloseButton) Then
            Me.Close()                                                                          ' Close the window
        ElseIf sender.Equals(RestoreButton) Then
            Dim rectArea As Rect = SystemParameters.WorkArea
            ' Is the window in maximized state?
            If Me.Width = rectArea.Width AndAlso Me.Height = rectArea.Height Then
                ' If so then go back to the initial height and width
                Me.Width = Me.MinWidth
                Me.Height = Me.MinHeight
                RestoreIcon.Source = TryCast(FindResource("ic_maximizebit"), ImageSource)      ' Change the icon of restore down
            Else
                ' Position the window at the leftmost and topmost
                Me.Left = 0
                Me.Top = 0
                Me.Width = rectArea.Width
                Me.Height = rectArea.Height
                RestoreIcon.Source = TryCast(FindResource("ic_restoredownbit"), ImageSource)   ' Change the icon of the restore down

            End If
        ElseIf sender.Equals(MinimizeButton) Then
            Me.WindowState = WindowState.Minimized                                              ' Minimize the window
        End If
    End Sub
#End Region

    Private Sub SettingsButton_Click(sender As Object, e As RoutedEventArgs) Handles SettingsButton.Click
        If Not SettingsMenu.IsOpen Then
            SettingsMenu.IsOpen = True

        End If
    End Sub

    Private Sub AboutButtonItem_Click(sender As Object, e As RoutedEventArgs) Handles AboutButtonItem.Click
        Dialog.Show(New AboutDialog)
    End Sub

    Private Sub Dashboard_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If My.Settings.demoMode Then
            Dialog.Show(New WelcomeDialog())
        End If
    End Sub

#Region "CheckingStock"
    Private ReadOnly connectionString As String = SqlConnectionSingleton.GetInstance.ConnectionString

    Public Sub CheckInventory()
        'Dim query As String = "SELECT stock_in, critical_level FROM tblinventory WHERE stock_in <= critical_level"
        Dim query As String = "SELECT PRODUCT_NAME, PRODUCT_PRICE, STOCK_IN, CASE WHEN STOCK_IN < 50 THEN 'Critical' ELSE 'Not Critical' END AS critical_level FROM viewtblinventoryrecords WHERE STOCK_IN < 50;"
        'Dim query As String = "SELECT * FROM viewtblinventoryrecords"

        Using connection As New SqlConnection(connectionString)
            Dim command As New SqlCommand(query, connection)

            Try
                connection.Open()
                Dim reader As SqlDataReader = command.ExecuteReader()

                If reader.HasRows Then
                    While reader.Read()

                        'Dim p1 As String = Convert.ToInt32(reader("PRODUCT_NAME"))
                        Dim p1 As String = reader("PRODUCT_NAME").ToString()
                        Dim p2 As Integer = Convert.ToDecimal(reader("STOCK_IN"))
                        'Dim stockIn As String = Convert.ToInt32(reader("PRODUCT_NAME"))
                        'Dim p2 As String = If(reader("STOCK_IN") IsNot DBNull.Value, reader("STOCK_IN").ToString(), String.Empty)

                        ' criticalLevel As Integer = Convert.ToInt32(reader("critical_level"))

                        Dim stack As New StackProductNotification()
                        stack._parent1 = Me
                        stack.LabelStackHeading.Text = $"Your Product {p1} is running out of stock, current stock is = {p2}"
                        'stack.LabelStackHeading.Text = $"{p1}{p2}"
                        StackNotifContainer.Children.Add(stack)
                        stack.Show()
                    End While
                End If

                reader.Close()
            Catch ex As Exception
                Dim newMessage As New TextBlock()
                newMessage.Text = "Error: " & ex.Message
                StackNotifContainer.Children.Add(newMessage)
            End Try
        End Using
    End Sub

#End Region
End Class
