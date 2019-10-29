using System;
using System.Drawing;
using System.Reflection;
using DevExpress.XtraEditors;
using DevExpress.Utils.Drawing;
using System.Collections.Generic;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Filtering;

namespace DXSample {
    public class MyFilterControl :FilterControl {
        protected override WinFilterTreeNodeModel CreateModel() {
            return new MyWinFilterTreeNodeModel(this);
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

    public class MyFilterControlLabelInfo :FilterControlLabelInfo {
        public MyFilterControlLabelInfo(Node node) : base(node) { }

        public override void Paint(ControlGraphicsInfoArgs info) {
            ViewInfo.Calculate(info.Cache);
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
                MyFilterControl filterControl = OwnerControl as MyFilterControl;
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

    public class MyWinFilterTreeNodeModel :WinFilterTreeNodeModel {
        public MyWinFilterTreeNodeModel(FilterControl control) : base(control) { }

        public override void OnVisualChange(FilterChangedActionInternal action, Node node) {
            if (action == FilterChangedActionInternal.NodeAdded)
                ((Dictionary<Node, FilterControlLabelInfo>)typeof(WinFilterTreeNodeModel).GetField("labels", 
                    BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this))[node] =
                    new MyFilterControlLabelInfo(node);
            else if (action == FilterChangedActionInternal.RootNodeReplaced) {
                Dictionary<Node, FilterControlLabelInfo> labels =
                    (Dictionary<Node, FilterControlLabelInfo>)typeof(WinFilterTreeNodeModel).GetField("labels",
                    BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this);
                labels.Clear();
                RecursiveVisitor(RootNode, child => {
                    var info = new MyFilterControlLabelInfo(child);
                    info.Clear();
                    info.CreateLabelInfoTexts();
                    labels[child] = info;
                });
            } else base.OnVisualChange(action, node);
        }
    }
}