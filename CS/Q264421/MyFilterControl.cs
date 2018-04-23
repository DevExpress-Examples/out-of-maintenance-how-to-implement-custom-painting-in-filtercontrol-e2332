using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.Reflection;
using DevExpress.Utils.Frames;
using System.Drawing;
using DevExpress.Utils.Drawing;
using System;
using DevExpress.XtraEditors.Drawing;

namespace DXSample {
    public class MyFilterControl :FilterControl {
        protected override INodesFactory CreateNodesFactory() {
            return new MyFilterControlNodesFactory();
        }

        private static readonly object fCustomDrawFilterLabel = new object();
        public event CustomDrawFilterLabelEventHandler CustomDrawFilterLabel {
            add { Events.AddHandler(fCustomDrawFilterLabel, value); }
            remove { Events.RemoveHandler(fCustomDrawFilterLabel, value); }
        }

        internal void RaiseCustomDrawFilterLabel(CustomDrawFilterLabelEventArgs args) {
            CustomDrawFilterLabelEventHandler handler = Events[fCustomDrawFilterLabel] as CustomDrawFilterLabelEventHandler;
            if (handler != null) handler(this, args);
        }
    }

    public class MyFilterControlNodesFactory :INodesFactory {
        protected virtual IClauseNode CreateClauseNode(ClauseType type, OperandProperty firstOperand, 
            ICollection<CriteriaOperator> operands) {
            MyClauseNode result = new MyClauseNode();
            result.Operation = type;
            result.FirstOperand = firstOperand;
            foreach (CriteriaOperator op in operands) result.AdditionalOperands.Add(op);
            return result;
        }

        protected virtual IGroupNode CreateGroupNode(GroupType type, ICollection<INode> subNodes) {
            MyGroupNode result = new MyGroupNode();
            result.NodeType = type;
            foreach (INode subNode in subNodes) {
                subNode.SetParentNode(result);
                result.SubNodes.Add(subNode);
            }
            return result;
        }

        #region INodesFactory Members

        IClauseNode INodesFactory.Create(ClauseType type, OperandProperty firstOperand, ICollection<CriteriaOperator> operands) {
            return CreateClauseNode(type, firstOperand, operands);
        }

        IGroupNode INodesFactory.Create(GroupType type, ICollection<INode> subNodes) {
            return CreateGroupNode(type, subNodes);
        }

        #endregion
    }

    public class MyClauseNode :ClauseNode {
        public override void SetOwner(FilterControl owner, Node parentNode) {
            base.SetOwner(owner, parentNode);
            FilterControlLabelInfo li = new MyFilterControlLabelInfo(this);
            li.CreateLabelInfoTexts(this);
            typeof(Node).GetField("li", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, li);
        }
    }

    public class MyGroupNode :GroupNode {
        public override void SetOwner(FilterControl owner, Node parentNode) {
            base.SetOwner(owner, parentNode);
            FilterControlLabelInfo li = new MyFilterControlLabelInfo(this);
            li.CreateLabelInfoTexts(this);
            typeof(Node).GetField("li", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, li);
        }
    }

    public class MyFilterControlLabelInfo :FilterControlLabelInfo {
        public MyFilterControlLabelInfo(Node node) : base(node) { }

        public override void Paint(ControlGraphicsInfoArgs info) {
            ViewInfo.Calculate(info.Graphics);
            ViewInfo.TopLine = 0;
            for (int i = 0; i < ViewInfo.Count; ++i) {
                FilterLabelInfoTextViewInfo textViewInfo = (FilterLabelInfoTextViewInfo)ViewInfo[i];
                NodeEditableElement nodeElement = textViewInfo.InfoText.Tag as NodeEditableElement;
                ElementType elementType = null == nodeElement ? ElementType.None : nodeElement.ElementType;
                CustomDrawFilterLabelEventArgs args = 
                    new CustomDrawFilterLabelEventArgs(elementType, textViewInfo.InfoText.Text, info.Cache, textViewInfo.TextElement.Bounds);
                args.Font = info.ViewInfo.Appearance.GetFont();
                args.ForeColor = textViewInfo.InfoText.Color;
                args.StringFormat = info.ViewInfo.Appearance.GetStringFormat();
                MyFilterControl filterControl = Owner.OwnerControl as MyFilterControl;
                if (filterControl != null) filterControl.RaiseCustomDrawFilterLabel(args);
                if (!args.Handled) ViewInfo[i].Draw(info.Cache, args.Font, args.ForeColor, args.StringFormat);
            }
        }
    }


    public delegate void CustomDrawFilterLabelEventHandler(object sender, CustomDrawFilterLabelEventArgs e);
    public class CustomDrawFilterLabelEventArgs :EventArgs {
        public CustomDrawFilterLabelEventArgs(ElementType labelType, string text, GraphicsCache cache, Rectangle bounds) {
            fLabelType = labelType;
            fLabelText = text;
            fCache = cache;
            fBounds = bounds;
        }

        private ElementType fLabelType;
        public ElementType LabelType { get { return fLabelType; } }

        private Color fForeColor;
        public Color ForeColor {
            get { return fForeColor; }
            set { fForeColor = value; }
        }

        private string fLabelText;
        public string LabelText {
            get { return fLabelText; }
        }

        private GraphicsCache fCache;
        public GraphicsCache Cache { get { return fCache; } }

        private Font fFont;
        public Font Font {
            get { return fFont; }
            set { fFont = value; }
        }

        private StringFormat fStringFormat;
        public StringFormat StringFormat {
            get { return fStringFormat; }
            set { fStringFormat = value; }
        }

        private bool fHandled;
        public bool Handled {
            get { return fHandled; }
            set { fHandled = value; }
        }

        private Rectangle fBounds;
        public Rectangle Bounds {
            get { return fBounds; }
            set { fBounds = value; }
        }
    }
}