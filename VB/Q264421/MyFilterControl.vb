Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Reflection
Imports DevExpress.XtraEditors
Imports DevExpress.Utils.Drawing
Imports System.Collections.Generic
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.Filtering

Namespace DXSample
	Public Class MyFilterControl
		Inherits FilterControl
		Protected Overrides Function CreateModel() As WinFilterTreeNodeModel
			Return New MyWinFilterTreeNodeModel(Me)
		End Function

		Private Shared ReadOnly fCustomDrawFilterLabel As Object = New Object()
		Public Custom Event CustomDrawFilterLabel As CustomDrawFilterLabelEventHandler
			AddHandler(ByVal value As CustomDrawFilterLabelEventHandler)
				Events.AddHandler(fCustomDrawFilterLabel, value)
			End AddHandler
			RemoveHandler(ByVal value As CustomDrawFilterLabelEventHandler)
				Events.RemoveHandler(fCustomDrawFilterLabel, value)
			End RemoveHandler
			RaiseEvent(ByVal sender As Object, ByVal e As CustomDrawFilterLabelEventArgs)
			End RaiseEvent
		End Event

		Friend Sub RaiseCustomDrawFilterLabel(ByVal args As CustomDrawFilterLabelEventArgs)
			Dim handler As CustomDrawFilterLabelEventHandler = TryCast(Events(fCustomDrawFilterLabel), CustomDrawFilterLabelEventHandler)
			If handler IsNot Nothing Then
				handler(Me, args)
			End If
		End Sub
	End Class

	Public Class MyFilterControlLabelInfo
		Inherits FilterControlLabelInfo
		Public Sub New(ByVal node As Node)
			MyBase.New(node)
		End Sub

		Public Overrides Sub Paint(ByVal info As ControlGraphicsInfoArgs)
			ViewInfo.Calculate(info.Graphics)
			ViewInfo.TopLine = 0
			For i As Integer = 0 To ViewInfo.Count - 1
				Dim textViewInfo As FilterLabelInfoTextViewInfo = CType(ViewInfo(i), FilterLabelInfoTextViewInfo)
				Dim nodeElement As NodeEditableElement = TryCast(textViewInfo.InfoText.Tag, NodeEditableElement)
				Dim elementType As ElementType = If(Nothing Is nodeElement, ElementType.None, nodeElement.ElementType)
				Dim args As New CustomDrawFilterLabelEventArgs(elementType, textViewInfo.InfoText.Text, info.Cache, textViewInfo.TextElement.Bounds)
				args.Font = info.ViewInfo.Appearance.GetFont()
				args.ForeColor = textViewInfo.InfoText.Color
				args.StringFormat = info.ViewInfo.Appearance.GetStringFormat()
				Dim filterControl As MyFilterControl = TryCast(OwnerControl, MyFilterControl)
				If filterControl IsNot Nothing Then
					filterControl.RaiseCustomDrawFilterLabel(args)
				End If
				If (Not args.Handled) Then
					ViewInfo(i).Draw(info.Cache, args.Font, args.ForeColor, args.StringFormat)
				End If
			Next i
		End Sub
	End Class


	Public Delegate Sub CustomDrawFilterLabelEventHandler(ByVal sender As Object, ByVal e As CustomDrawFilterLabelEventArgs)
	Public Class CustomDrawFilterLabelEventArgs
		Inherits EventArgs
		Public Sub New(ByVal labelType As ElementType, ByVal text As String, ByVal cache As GraphicsCache, ByVal bounds As Rectangle)
			fLabelType = labelType
			fLabelText = text
			fCache = cache
			fBounds = bounds
		End Sub

		Private fLabelType As ElementType
		Public ReadOnly Property LabelType() As ElementType
			Get
				Return fLabelType
			End Get
		End Property

		Private fForeColor As Color
		Public Property ForeColor() As Color
			Get
				Return fForeColor
			End Get
			Set(ByVal value As Color)
				fForeColor = value
			End Set
		End Property

		Private fLabelText As String
		Public ReadOnly Property LabelText() As String
			Get
				Return fLabelText
			End Get
		End Property

		Private fCache As GraphicsCache
		Public ReadOnly Property Cache() As GraphicsCache
			Get
				Return fCache
			End Get
		End Property

		Private fFont As Font
		Public Property Font() As Font
			Get
				Return fFont
			End Get
			Set(ByVal value As Font)
				fFont = value
			End Set
		End Property

		Private fStringFormat As StringFormat
		Public Property StringFormat() As StringFormat
			Get
				Return fStringFormat
			End Get
			Set(ByVal value As StringFormat)
				fStringFormat = value
			End Set
		End Property

		Private fHandled As Boolean
		Public Property Handled() As Boolean
			Get
				Return fHandled
			End Get
			Set(ByVal value As Boolean)
				fHandled = value
			End Set
		End Property

		Private fBounds As Rectangle
		Public Property Bounds() As Rectangle
			Get
				Return fBounds
			End Get
			Set(ByVal value As Rectangle)
				fBounds = value
			End Set
		End Property
	End Class

	Public Class MyWinFilterTreeNodeModel
		Inherits WinFilterTreeNodeModel
		Public Sub New(ByVal control As FilterControl)
			MyBase.New(control)
		End Sub

		Public Overrides Sub OnVisualChange(ByVal action As FilterChangedActionInternal, ByVal node As Node)
			If action = FilterChangedActionInternal.NodeAdded Then
				CType(GetType(WinFilterTreeNodeModel).GetField("labels", BindingFlags.Instance Or BindingFlags.NonPublic).GetValue(Me), Dictionary(Of Node, FilterControlLabelInfo))(node) = New MyFilterControlLabelInfo(node)
			ElseIf action = FilterChangedActionInternal.RootNodeReplaced Then
				Dim labels As Dictionary(Of Node, FilterControlLabelInfo) = CType(GetType(WinFilterTreeNodeModel).GetField("labels", BindingFlags.Instance Or BindingFlags.NonPublic).GetValue(Me), Dictionary(Of Node, FilterControlLabelInfo))
				labels.Clear()
				RecursiveVisitor(RootNode, Function(child) AnonymousMethod1(child, labels))
			Else
				MyBase.OnVisualChange(action, node)
			End If
		End Sub
		
		Private Function AnonymousMethod1(ByVal child As Object, ByVal labels As Dictionary(Of Node, FilterControlLabelInfo)) As Boolean
            Dim info = New MyFilterControlLabelInfo(CType(child, DevExpress.XtraEditors.Filtering.Node))
			info.Clear()
			info.CreateLabelInfoTexts()
            labels(CType(child, DevExpress.XtraEditors.Filtering.Node)) = info
			Return True
		End Function
	End Class
End Namespace