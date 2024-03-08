Public Class fuck
    Implements IObserverPanel
    Private _subject As IObservablePanel

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Try
            _subject = Application.Current.Windows.OfType(Of Dashboard).FirstOrDefault
            _subject?.RegisterObserver(Me)
            _subject?.NotifyObserver()
        Catch ex As Exception
            HandyControl.Controls.MessageBox.Error(ex.Message, "Observer Error")
        End Try

    End Sub

    Public Sub Update() Implements IObserverPanel.Update
        'Throw New NotImplementedException()
        Pota.Text = BaseProduct.ScalarProducts()
    End Sub
End Class

