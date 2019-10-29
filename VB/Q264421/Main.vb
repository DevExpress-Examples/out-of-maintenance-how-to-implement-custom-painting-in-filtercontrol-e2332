Imports System
Imports DXSample
Imports System.Linq
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.Data.Filtering
Imports System.Collections.Generic
Imports DevExpress.XtraEditors.Filtering
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.XtraEditors

Namespace Q264421
	Partial Public Class Main
		Inherits XtraForm

		Public Sub New()
			InitializeComponent()
			Dim columns As New FilterColumnCollection()
			columns.Add(New UnboundFilterColumn("Order ID", "OrderID", GetType(Integer), spinEdit, FilterColumnClauseClass.Generic))
			columns.Add(New UnboundFilterColumn("Product", "Product", GetType(String), textEdit, FilterColumnClauseClass.String))
			columns.Add(New UnboundFilterColumn("Discount", "Discount", GetType(Decimal), spinEdit, FilterColumnClauseClass.Generic))
			filterControl.SetFilterColumnsCollection(columns)
			filterControl.FilterCriteria = CriteriaOperator.Parse("Product between ('Alice Munton', 'Icura') && Discount > .05")
			filterControl.SetDefaultColumn(filterControl.FilterColumns("OrderID"))
		End Sub

		Private Sub OnFilterControlCustomDrawFilterLabel(ByVal sender As Object, ByVal e As CustomDrawFilterLabelEventArgs) Handles filterControl.CustomDrawFilterLabel
			Select Case e.LabelType
				Case ElementType.Group
					e.ForeColor = Color.Red
				Case ElementType.Property
					e.ForeColor = Color.Orange
				Case ElementType.Operation
					e.ForeColor = Color.Indigo
				Case ElementType.Value
					e.ForeColor = Color.Green
				Case Else
					e.ForeColor = Color.Blue
			End Select
		End Sub
	End Class
End Namespace
