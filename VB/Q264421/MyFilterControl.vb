Imports Microsoft.VisualBasic
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Filtering
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.Data.Filtering
Imports System.Collections.Generic
Imports System.Reflection
Imports DevExpress.Utils.Frames
Imports System.Drawing
Imports DevExpress.Utils.Drawing
Imports System
Imports DevExpress.XtraEditors.Drawing

Namespace DXSample
	Public Class MyFilterControl
		Inherits FilterControl
		Protected Overrides Function CreateNodesFactory() As INodesFactory
			Return New MyFilterControlNodesFactory()
		End Function

		Private Shared ReadOnly fCustomDrawFilterLabel As Object = New Object()
		Public Custom Event CustomDrawFilterLabel As EventHandler(Of CustomDrawFilterLabelEventArgs)
			AddHandler(ByVal value As EventHandler(Of CustomDrawFilterLabelEventArgs))
				Events.AddHandler(fCustomDrawFilterLabel, value)
			End AddHandler
			RemoveHandler(ByVal value As EventHandler(Of CustomDrawFilterLabelEventArgs))
				Events.RemoveHandler(fCustomDrawFilterLabel, value)
			End RemoveHandler
            RaiseEvent(ByVal sender As System.Object, ByVal e As CustomDrawFilterLabelEventArgs)
            End RaiseEvent
		End Event

		Friend Sub RaiseCustomDrawFilterLabel(ByVal args As CustomDrawFilterLabelEventArgs)
			Dim handler As EventHandler(Of CustomDrawFilterLabelEventArgs) = TryCast(Events(fCustomDrawFilterLabel), EventHandler(Of CustomDrawFilterLabelEventArgs))
			If handler IsNot Nothing Then
				handler(Me, args)
			End If
		End Sub
	End Class

	Public Class MyFilterControlNodesFactory
		Implements INodesFactory
		Protected Overridable Function CreateClauseNode(ByVal type As ClauseType, ByVal firstOperand As OperandProperty, ByVal operands As ICollection(Of CriteriaOperator)) As IClauseNode
			Dim result As New MyClauseNode()
			result.Operation = type
			result.FirstOperand = firstOperand
			For Each op As CriteriaOperator In operands
				result.AdditionalOperands.Add(op)
			Next op
			Return result
		End Function

		Protected Overridable Function CreateGroupNode(ByVal type As GroupType, ByVal subNodes As ICollection(Of INode)) As IGroupNode
			Dim result As New MyGroupNode()
			result.NodeType = type
			For Each subNode As INode In subNodes
				subNode.SetParentNode(result)
				result.SubNodes.Add(subNode)
			Next subNode
			Return result
		End Function

		#Region "INodesFactory Members"

		Private Function Create(ByVal type As ClauseType, ByVal firstOperand As OperandProperty, ByVal operands As ICollection(Of CriteriaOperator)) As IClauseNode Implements INodesFactory.Create
			Return CreateClauseNode(type, firstOperand, operands)
		End Function

		Private Function Create(ByVal type As GroupType, ByVal subNodes As ICollection(Of INode)) As IGroupNode Implements INodesFactory.Create
			Return CreateGroupNode(type, subNodes)
		End Function

		#End Region
	End Class

	Public Class MyClauseNode
		Inherits ClauseNode
		Public Overrides Sub SetOwner(ByVal owner As FilterControl, ByVal parentNode As GroupNode)
			MyBase.SetOwner(owner, parentNode)
			Dim li As FilterControlLabelInfo = New MyFilterControlLabelInfo(Me)
			li.CreateLabelInfoTexts(Me)
			GetType(Node).GetField("li", BindingFlags.Instance Or BindingFlags.NonPublic).SetValue(Me, li)
		End Sub
	End Class

	Public Class MyGroupNode
		Inherits GroupNode
		Public Overrides Sub SetOwner(ByVal owner As FilterControl, ByVal parentNode As GroupNode)
			MyBase.SetOwner(owner, parentNode)
			Dim li As FilterControlLabelInfo = New MyFilterControlLabelInfo(Me)
			li.CreateLabelInfoTexts(Me)
			GetType(Node).GetField("li", BindingFlags.Instance Or BindingFlags.NonPublic).SetValue(Me, li)
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
				Dim nodeElement As NodeElement = TryCast(textViewInfo.InfoText.Tag, NodeElement)
                Dim elementType As ElementType = elementType.None
                If Not nodeElement Is Nothing Then elementType = nodeElement.Type
				Dim args As New CustomDrawFilterLabelEventArgs(elementType, textViewInfo.InfoText.Text, info.Cache, textViewInfo.TextElement.Bounds)
				args.Font = info.ViewInfo.Appearance.GetFont()
				args.ForeColor = textViewInfo.InfoText.Color
				args.StringFormat = info.ViewInfo.Appearance.GetStringFormat()
				Dim filterControl As MyFilterControl = TryCast(Owner.OwnerControl, MyFilterControl)
				If filterControl IsNot Nothing Then
					filterControl.RaiseCustomDrawFilterLabel(args)
				End If
				ViewInfo(i).Draw(info.Cache, args.Font, args.ForeColor, args.StringFormat)
			Next i
		End Sub
	End Class


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

		Private fCache As GraphicsCache
		Public ReadOnly Property Cache() As GraphicsCache
			Get
				Return fCache
			End Get
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
End Namespace