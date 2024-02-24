
Imports System.Collections.ObjectModel
Imports HandyControl.Controls
Imports SalesMonitoringSystem.Pos

Public Class ProductCard
    Inherits UserControl

    'Public ReadOnly Property Image As Image
    '    Get
    '        Return Image
    '    End Get
    'End Property
    'Public Property TitleText As String
    '    Get
    '        Return Title.Text
    '    End Get
    '    Set(ByVal value As String)
    '        Title.Text = value
    '    End Set
    'End Property

    'Public Property DescriptionText As String
    '    Get
    '        Return Description.Text
    '    End Get
    '    Set(ByVal value As String)
    '        Description.Text = value
    '    End Set
    'End Property

    'Public Property ImageText As String
    '    Get
    '        Return Image.Text
    '    End Get
    '    Set(ByVal value As String)
    '        Image.Text = value
    '    End Set
    'End Property

    Public Sub New(Optional data As CardModel = Nothing)

        'This call Is required by the designer.
        InitializeComponent()


        ' Add any initialization after the InitializeComponent() call.
        DataContext = data

    End Sub


End Class
