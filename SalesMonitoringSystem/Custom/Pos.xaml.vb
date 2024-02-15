Imports System.Collections.ObjectModel
Imports System.Data
Imports HandyControl.Controls
Imports HandyControl.Tools.Extension

Public Class Pos
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        For i = 0 To 100
            Wrappanelxd.Children.Add(New ProductCard())
        Next
    End Sub

End Class


