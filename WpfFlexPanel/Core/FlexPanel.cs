using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using WpfFlexPanel.Enums;

namespace WpfFlexPanel.Core
{
    /// <summary>
    /// A WPF Panel implementation that provides CSS Flexbox-like layout behavior.
    /// Enhanced version with improved performance and simplified logic.
    /// </summary>
    [Description("A flexible layout panel that mimics CSS Flexbox behavior")]
    [DefaultProperty(nameof(FlexDirection))]
    public sealed class FlexPanel : Panel
    {
        #region Constants

        private const double EPSILON = 1e-10;
        private const double DEFAULT_FLEX_SHRINK = 1.0;
        private const double DEFAULT_FLEX_GROW = 0.0;
        private const double DEFAULT_GAP = 0.0;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty FlexDirectionProperty =
            DependencyProperty.Register(
                nameof(FlexDirection),
                typeof(FlexDirection),
                typeof(FlexPanel),
                new FrameworkPropertyMetadata(
                    FlexDirection.Row,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnLayoutPropertyChanged));

        public static readonly DependencyProperty JustifyContentProperty =
            DependencyProperty.Register(
                nameof(JustifyContent),
                typeof(JustifyContent),
                typeof(FlexPanel),
                new FrameworkPropertyMetadata(
                    JustifyContent.FlexStart,
                    FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnLayoutPropertyChanged));

        public static readonly DependencyProperty AlignItemsProperty =
            DependencyProperty.Register(
                nameof(AlignItems),
                typeof(AlignItems),
                typeof(FlexPanel),
                new FrameworkPropertyMetadata(
                    AlignItems.Stretch,
                    FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnLayoutPropertyChanged));

        public static readonly DependencyProperty FlexWrapProperty =
            DependencyProperty.Register(
                nameof(FlexWrap),
                typeof(FlexWrap),
                typeof(FlexPanel),
                new FrameworkPropertyMetadata(
                    FlexWrap.NoWrap,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnLayoutPropertyChanged));

        public static readonly DependencyProperty GapProperty =
            DependencyProperty.Register(
                nameof(Gap),
                typeof(double),
                typeof(FlexPanel),
                new FrameworkPropertyMetadata(
                    DEFAULT_GAP,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnLayoutPropertyChanged));

        #endregion

        #region Attached Properties

        public static readonly DependencyProperty FlexGrowProperty =
            DependencyProperty.RegisterAttached(
                "FlexGrow",
                typeof(double),
                typeof(FlexPanel),
                new FrameworkPropertyMetadata(
                    DEFAULT_FLEX_GROW,
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange,
                    OnChildPropertyChanged));

        public static readonly DependencyProperty FlexShrinkProperty =
            DependencyProperty.RegisterAttached(
                "FlexShrink",
                typeof(double),
                typeof(FlexPanel),
                new FrameworkPropertyMetadata(
                    DEFAULT_FLEX_SHRINK,
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange,
                    OnChildPropertyChanged));

        public static readonly DependencyProperty FlexBasisProperty =
            DependencyProperty.RegisterAttached(
                "FlexBasis",
                typeof(double),
                typeof(FlexPanel),
                new FrameworkPropertyMetadata(
                    double.NaN,
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange,
                    OnChildPropertyChanged));

        public static readonly DependencyProperty AlignSelfProperty =
            DependencyProperty.RegisterAttached(
                "AlignSelf",
                typeof(AlignItems?),
                typeof(FlexPanel),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsParentArrange,
                    OnChildPropertyChanged));

        public static readonly DependencyProperty OrderProperty =
            DependencyProperty.RegisterAttached(
                "Order",
                typeof(int),
                typeof(FlexPanel),
                new FrameworkPropertyMetadata(
                    0,
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange,
                    OnChildPropertyChanged));

        #endregion

        #region Properties

        [Category("Layout")]
        public FlexDirection FlexDirection
        {
            get => (FlexDirection)GetValue(FlexDirectionProperty);
            set => SetValue(FlexDirectionProperty, value);
        }

        [Category("Layout")]
        public JustifyContent JustifyContent
        {
            get => (JustifyContent)GetValue(JustifyContentProperty);
            set => SetValue(JustifyContentProperty, value);
        }

        [Category("Layout")]
        public AlignItems AlignItems
        {
            get => (AlignItems)GetValue(AlignItemsProperty);
            set => SetValue(AlignItemsProperty, value);
        }

        [Category("Layout")]
        public FlexWrap FlexWrap
        {
            get => (FlexWrap)GetValue(FlexWrapProperty);
            set => SetValue(FlexWrapProperty, value);
        }

        [Category("Layout")]
        public double Gap
        {
            get => (double)GetValue(GapProperty);
            set => SetValue(GapProperty, value);
        }




        #endregion

        #region Attached Property Accessors
        public static void SetFlexGrow(DependencyObject element, double value) => element?.SetValue(FlexGrowProperty, value);
        public static double GetFlexGrow(DependencyObject element) => element != null ? (double)element.GetValue(FlexGrowProperty) : DEFAULT_FLEX_GROW;

        public static void SetFlexShrink(DependencyObject element, double value) => element?.SetValue(FlexShrinkProperty, value);
        public static double GetFlexShrink(DependencyObject element) => element != null ? (double)element.GetValue(FlexShrinkProperty) : DEFAULT_FLEX_SHRINK;

        public static void SetFlexBasis(DependencyObject element, double value) => element?.SetValue(FlexBasisProperty, value);
        public static double GetFlexBasis(DependencyObject element) => element != null ? (double)element.GetValue(FlexBasisProperty) : double.NaN;

        public static void SetAlignSelf(DependencyObject element, AlignItems? value) => element?.SetValue(AlignSelfProperty, value);
        public static AlignItems? GetAlignSelf(DependencyObject element) => element != null ? (AlignItems?)element.GetValue(AlignSelfProperty) : null;

        public static void SetOrder(DependencyObject element, int value) => element?.SetValue(OrderProperty, value);
        public static int GetOrder(DependencyObject element) => element != null ? (int)element.GetValue(OrderProperty) : 0;
        #endregion

        #region Layout Overrides
        protected override Size MeasureOverride(Size availableSize)
        {
            var children = GetVisibleOrderedChildren();
            if (!children.Any()) return new Size();

            // Measure all children with infinite constraints first
            var infiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
            foreach (var child in children)
                child.Measure(infiniteSize);

            return CanWrap
                ? MeasureWithWrapping(availableSize, children)
                : MeasureWithoutWrapping(children);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var children = GetVisibleOrderedChildren();
            if (!children.Any()) return finalSize;

            if (CanWrap)
                ArrangeWithWrapping(finalSize, children);
            else
                ArrangeWithoutWrapping(finalSize, children);

            return finalSize;
        }

        #endregion

        #region Core Layout Logic

        private List<UIElement> GetVisibleOrderedChildren()
        {
            return Children.Cast<UIElement>()
                .Where(child => child.Visibility != Visibility.Collapsed)
                .OrderBy(GetOrder)
                .ThenBy(child => Children.IndexOf(child))
                .ToList();
        }

        private Size MeasureWithoutWrapping(List<UIElement> children)
        {
            var layoutData = CreateLayoutData(children);
            return IsHorizontal
                ? new Size(layoutData.TotalMainSize, layoutData.MaxCrossSize)
                : new Size(layoutData.MaxCrossSize, layoutData.TotalMainSize);
        }

        private Size MeasureWithWrapping(Size availableSize, List<UIElement> children)
        {
            var lines = CreateFlexLines(children, GetMainSize(availableSize));
            double totalCrossSize = 0;
            double maxMainSize = 0;

            foreach (var line in lines)
            {
                var layoutData = CreateLayoutData(line);
                maxMainSize = Math.Max(maxMainSize, layoutData.TotalMainSize);
                totalCrossSize += layoutData.MaxCrossSize;
            }

            // Add gaps between lines
            if (lines.Count > 1)
                totalCrossSize += Gap * (lines.Count - 1);

            return IsHorizontal
                ? new Size(maxMainSize, totalCrossSize)
                : new Size(totalCrossSize, maxMainSize);
        }

        private void ArrangeWithoutWrapping(Size containerSize, List<UIElement> children)
        {
            var flexItems = CreateFlexItems(children);
            ResolveFlexSizes(flexItems, GetMainSize(containerSize));
            ArrangeFlexLine(flexItems, containerSize, 0);
        }

        private void ArrangeWithWrapping(Size containerSize, List<UIElement> children)
        {
            var lines = CreateFlexLines(children, GetMainSize(containerSize));
            double crossOffset = 0;

            foreach (var line in lines)
            {
                var flexItems = CreateFlexItems(line);
                ResolveFlexSizes(flexItems, GetMainSize(containerSize));

                double lineCrossSize = line.Max(child => GetCrossSize(child.DesiredSize));
                ArrangeFlexLine(flexItems, containerSize, crossOffset);

                crossOffset += lineCrossSize + Gap;
            }
        }

        #endregion

        #region  Helper Methods
        private bool IsHorizontal => FlexDirection == FlexDirection.Row || FlexDirection == FlexDirection.RowReverse;
        private bool IsReversed => FlexDirection == FlexDirection.RowReverse || FlexDirection == FlexDirection.ColumnReverse;
        private bool CanWrap => FlexWrap != FlexWrap.NoWrap;
        private LayoutData CreateLayoutData(List<UIElement> children)
        {
            double totalMainSize = 0;
            double maxCrossSize = 0;

            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];
                var mainSize = GetMainSize(child.DesiredSize);
                var crossSize = GetCrossSize(child.DesiredSize);

                totalMainSize += mainSize;
                maxCrossSize = Math.Max(maxCrossSize, crossSize);

                if (i > 0) totalMainSize += Gap;
            }

            return new LayoutData { TotalMainSize = totalMainSize, MaxCrossSize = maxCrossSize };
        }

        private List<FlexItem> CreateFlexItems(List<UIElement> children)
        {
            return children.Select(child =>
            {
                var desiredSize = child.DesiredSize;
                var mainSize = GetMainSize(desiredSize);
                var flexBasis = GetFlexBasis(child);

                return new FlexItem
                {
                    Element = child,
                    FlexBasis = double.IsNaN(flexBasis) ? mainSize : flexBasis,
                    FlexGrow = Math.Max(0, GetFlexGrow(child)), // Ensure non-negative
                    FlexShrink = Math.Max(0, GetFlexShrink(child)), // Ensure non-negative
                    CrossSize = GetCrossSize(desiredSize),
                    FinalMainSize = double.IsNaN(flexBasis) ? mainSize : flexBasis
                };
            }).ToList();
        }

        private void ResolveFlexSizes(List<FlexItem> items, double containerSize)
        {
            if (!items.Any()) return;

            double usedSpace = items.Sum(item => item.FlexBasis) + Gap * (items.Count - 1);
            double remainingSpace = containerSize - usedSpace;

            if (Math.Abs(remainingSpace) < EPSILON) return;

            if (remainingSpace > 0)
                DistributeExtraSpace(items, remainingSpace);
            else
                HandleOverflow(items, Math.Abs(remainingSpace));
        }

        private void DistributeExtraSpace(List<FlexItem> items, double extraSpace)
        {
            double totalGrow = items.Sum(item => item.FlexGrow);
            if (totalGrow < EPSILON) return;

            foreach (var item in items.Where(i => i.FlexGrow > EPSILON))
            {
                double growAmount = extraSpace * (item.FlexGrow / totalGrow);
                item.FinalMainSize = item.FlexBasis + growAmount;
            }
        }

        private void HandleOverflow(List<FlexItem> items, double overflow)
        {
            double totalWeightedShrink = items.Sum(item => item.FlexShrink * item.FlexBasis);
            if (totalWeightedShrink < EPSILON) return;

            foreach (var item in items.Where(i => i.FlexShrink > EPSILON))
            {
                double shrinkRatio = (item.FlexShrink * item.FlexBasis) / totalWeightedShrink;
                double shrinkAmount = overflow * shrinkRatio;
                item.FinalMainSize = Math.Max(0, item.FlexBasis - shrinkAmount);
            }
        }

        private void ArrangeFlexLine(List<FlexItem> items, Size containerSize, double crossOffset)
        {
            if (!items.Any()) return;

            double containerMainSize = GetMainSize(containerSize);
            double containerCrossSize = GetCrossSize(containerSize);

            var justification = CalculateJustification(items, containerMainSize);
            double currentOffset = justification.initialOffset;

            foreach (var item in items)
            {
                var alignment = GetAlignSelf(item.Element) ?? AlignItems;
                double crossPosition = CalculateCrossPosition(alignment, containerCrossSize, item.CrossSize) + crossOffset;

                var rect = CreateRect(currentOffset, crossPosition, item, containerCrossSize, alignment);

                if (IsReversed)
                    rect = ApplyReverse(rect, containerSize);

                item.Element.Arrange(rect);
                currentOffset += item.FinalMainSize + Gap + justification.spacing;
            }
        }

        private List<List<UIElement>> CreateFlexLines(List<UIElement> children, double availableMainSize)
        {
            var lines = new List<List<UIElement>>();
            var currentLine = new List<UIElement>();
            double currentLineSize = 0;

            foreach (var child in children)
            {
                double childMainSize = GetMainSize(child.DesiredSize);
                double requiredSize = currentLineSize + (currentLine.Any() ? Gap : 0) + childMainSize;

                bool shouldWrap = currentLine.Any() &&
                                 !double.IsInfinity(availableMainSize) &&
                                 requiredSize > availableMainSize;

                if (shouldWrap)
                {
                    lines.Add(currentLine);
                    currentLine = new List<UIElement> { child };
                    currentLineSize = childMainSize;
                }
                else
                {
                    currentLine.Add(child);
                    currentLineSize = requiredSize;
                }
            }

            if (currentLine.Any())
                lines.Add(currentLine);

            return FlexWrap == FlexWrap.WrapReverse
                ? lines.AsEnumerable().Reverse().ToList()
                : lines;
        }

        #endregion

        #region Utility Methods
        private double GetMainSize(Size size) => IsHorizontal ? size.Width : size.Height;
        private double GetCrossSize(Size size) => IsHorizontal ? size.Height : size.Width;

        private (double initialOffset, double spacing) CalculateJustification(List<FlexItem> items, double containerSize)
        {
            double usedSpace = items.Sum(i => i.FinalMainSize) + Gap * (items.Count - 1);
            double freeSpace = containerSize - usedSpace;

            switch (JustifyContent)
            {
                case JustifyContent.FlexEnd:
                    return (freeSpace, 0);

                case JustifyContent.Center:
                    return (freeSpace / 2, 0);

                case JustifyContent.SpaceBetween:
                    if (items.Count > 1)
                        return (0, freeSpace / (items.Count - 1));
                    break;

                case JustifyContent.SpaceAround:
                    if (items.Count > 0)
                        return (freeSpace / (2 * items.Count), freeSpace / items.Count);
                    break;

                case JustifyContent.SpaceEvenly:
                    if (items.Count > 0)
                        return (freeSpace / (items.Count + 1), freeSpace / (items.Count + 1));
                    break;
            }

            return (0, 0);
        }

        private double CalculateCrossPosition(AlignItems alignment, double containerSize, double itemSize)
        {
            switch (alignment)
            {
                case AlignItems.FlexEnd:
                    return containerSize - itemSize;

                case AlignItems.Center:
                    return (containerSize - itemSize) / 2;

                case AlignItems.Stretch:
                    return 0;

                default: // FlexStart and others
                    return 0;
            }
        }

        private Rect CreateRect(double mainOffset, double crossOffset, FlexItem item, double containerCrossSize, AlignItems alignment)
        {
            double mainSize = item.FinalMainSize;
            double crossSize = alignment == AlignItems.Stretch ? containerCrossSize : item.CrossSize;

            return IsHorizontal
                ? new Rect(mainOffset, crossOffset, mainSize, crossSize)
                : new Rect(crossOffset, mainOffset, crossSize, mainSize);
        }
        private Rect ApplyReverse(Rect rect, Size containerSize)
        {
            return IsHorizontal
                ? new Rect(containerSize.Width - rect.Right, rect.Y, rect.Width, rect.Height)
                : new Rect(rect.X, containerSize.Height - rect.Bottom, rect.Width, rect.Height);
        }

        #endregion

        #region Event Handlers

        private static void OnLayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FlexPanel)d)?.InvalidateVisual();
        }

        private static void OnChildPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (VisualTreeHelper.GetParent(d) is FlexPanel panel)
                panel.InvalidateVisual();
        }

        #endregion

        #region Supporting Types

        private struct LayoutData
        {
            public double TotalMainSize;
            public double MaxCrossSize;
        }

        private class FlexItem
        {
            public UIElement Element { get; set; }
            public double FlexBasis { get; set; }
            public double FlexGrow { get; set; }
            public double FlexShrink { get; set; }
            public double CrossSize { get; set; }
            public double FinalMainSize { get; set; }
            public bool IsFlexible => FlexGrow > EPSILON || FlexShrink > EPSILON;
            public override string ToString() =>
                $"FlexItem: Basis={FlexBasis:F1}, Grow={FlexGrow:F1}, Final={FinalMainSize:F1}";
        }

        #endregion

    }
}
