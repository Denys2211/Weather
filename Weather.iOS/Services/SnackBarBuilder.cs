﻿using CoreAnimation;
using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace Weather.iOS.Services
{
    public enum SnackbarAnimationType
    {
        FadeInFadeOut,
        SlideFromBottomToTop,
        SlideFromBottomBackToBottom,
        SlideFromLeftToRight,
        SlideFromRightToLeft,
    }

    public enum SnackbarLocation
    {
        Bottom,
        Top,
    }

    public class SnackBarBuilder : UIView
    {

        /// Snackbar action button max width.
        private const float snackbarActionButtonMaxWidth = 64;

        // Snackbar action button min width.
        private const float snackbarActionButtonMinWidth = 44;

        // Snackbar icon imageView default width
        private const float snackbarIconImageViewWidth = 32;

        private NSLayoutConstraint[] hConstraints;

        // Action callback.
        public Action<SnackBarBuilder> ActionBlock { get; set; } = null;

        // Second action block
        public Action<SnackBarBuilder> SecondActionBlock { get; set; } = null;

        // Dismiss block
        public Action<SnackBarBuilder> DismissBlock { get; set; }

        // Snackbar display duration. Default is Short - 1 second.
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(3);

        // Snackbar animation type. Default is SlideFromBottomBackToBottom.
        public SnackbarAnimationType AnimationType = SnackbarAnimationType.FadeInFadeOut;

        // Snackbar location
        public SnackbarLocation LocationType = SnackbarLocation.Bottom;

        // Show and hide animation duration. Default is 0.3
        private float AnimationDuration = 0.3f;

        private float _cornerRadius = 4;
        public float CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                _cornerRadius = value;
                if (_cornerRadius > Height)
                {
                    _cornerRadius = Height / 2;
                }
                if (_cornerRadius < 0)
                    _cornerRadius = 0;

                this.Layer.CornerRadius = _cornerRadius;
                this.Layer.MasksToBounds = true;
            }
        }

        /// Top margin. Default is 4
        private float _topMargin = 20;
        public float TopMargin
        {
            get { return _topMargin; }
            set
            {
                _topMargin = value; if (topMarginConstraint != null) { topMarginConstraint.Constant = _topMargin; this.LayoutIfNeeded(); }
            }
        }

        /// Left margin. Default is 4
        private float _leftMargin = 10;
        public float LeftMargin
        {
            get { return _leftMargin; }
            set { _leftMargin = value; if (leftMarginConstraint != null) { leftMarginConstraint.Constant = _leftMargin; this.LayoutIfNeeded(); } }
        }

        private float _rightMargin = 10;
        public float RightMargin
        {
            get { return _rightMargin; }
            set { _rightMargin = value; if (rightMarginConstraint != null) { rightMarginConstraint.Constant = _leftMargin; this.LayoutIfNeeded(); } }
        }

        /// Bottom margin. Default is 4
        private float _bottomMargin = 10;
        public float BottomMargin
        {
            get { return _bottomMargin; }
            set { _bottomMargin = value; if (bottomMarginConstraint != null) { bottomMarginConstraint.Constant = _bottomMargin; this.LayoutIfNeeded(); } }
        }

        private float _height = 44;
        public float Height
        {
            get { return _height; }
            set { _height = value; if (heightConstraint != null) { heightConstraint.Constant = _height; this.LayoutIfNeeded(); } }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; if (this.MessageLabel != null) { this.MessageLabel.Text = _message; } }
        }

        private UIColor _messageTextColor = UIColor.White;
        public UIColor MessageTextColor
        {
            get { return _messageTextColor; }
            set { _messageTextColor = value; this.MessageLabel.TextColor = _messageTextColor; }
        }

        private UIFont _messageTextFont = UIFont.BoldSystemFontOfSize(14);
        public UIFont MessageTextFont
        {
            get { return _messageTextFont; }
            set { _messageTextFont = value; this.MessageLabel.Font = _messageTextFont; }
        }

        private UITextAlignment _messageTextAlign;
        public UITextAlignment MessageTextAlign
        {
            get { return _messageTextAlign; }
            set { _messageTextAlign = value; this.MessageLabel.TextAlignment = _messageTextAlign; }
        }

        private nfloat _messageMarginLeft = 4;
        public nfloat MessageMarginLeft
        {
            get { return _messageMarginLeft; }
            set { _messageMarginLeft = value; InvalidateHorizontalConstraints(); }
        }

        private nfloat _messageMarginRight = 2;
        public nfloat MessageMarginRight
        {
            get { return _messageMarginRight; }
            set { _messageMarginRight = value; InvalidateHorizontalConstraints(); }
        }

        private string _actionText;
        public string ActionText
        {
            get { return _actionText; }
            set
            {
                _actionText = value;
                if (this.ActionButton != null)
                {
                    this.ActionButton.SetTitle(_actionText, UIControlState.Normal);
                    this.ActionButton.Hidden = string.IsNullOrEmpty(value);
                }
            }
        }

        private string _secondActionText;
        public string SecondActionText
        {
            get { return _secondActionText; }
            set { _secondActionText = value; if (this.SecondActionButton != null) { this.SecondActionButton.SetTitle(_secondActionText, UIControlState.Normal); } }
        }

        // Action button title color. Default is white.
        private UIColor _actionTextColor = UIColor.White;
        public UIColor ActionTextColor
        {
            get { return _actionTextColor; }
            set { _actionTextColor = value; this.ActionButton.SetTitleColor(_actionTextColor, UIControlState.Normal); }
        }

        // Second action button title color. Default is white.
        private UIColor _secondActionTextColor = UIColor.White;
        public UIColor SecondActionTextColor
        {
            get { return _secondActionTextColor; }
            set { _secondActionTextColor = value; this.SecondActionButton.SetTitleColor(_secondActionTextColor, UIControlState.Normal); }
        }

        // First action text font. Default is Bold system font (14).
        private UIFont _actionTextFont = UIFont.BoldSystemFontOfSize(14);
        public UIFont ActionTextFont
        {
            get { return _actionTextFont; }
            set { _actionTextFont = value; this.ActionButton.TitleLabel.Font = _actionTextFont; }
        }

        // First action text font. Default is Bold system font (14).
        private UIFont _secondActionTextFont = UIFont.BoldSystemFontOfSize(14);
        public UIFont SecondActionTextFont
        {
            get { return _secondActionTextFont; }
            set { _secondActionTextFont = value; this.SecondActionButton.TitleLabel.Font = _secondActionTextFont; }
        }

        private UIImage _icon;
        public UIImage Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                IconImageView.Image = _icon;
            }
        }

        private UIViewContentMode _iconContentMode = UIViewContentMode.Center;
        public UIViewContentMode IconContentMode
        {
            get { return _iconContentMode; }
            set
            {
                _iconContentMode = value;
                IconImageView.ContentMode = _iconContentMode;
            }
        }

        public UIImageView IconImageView;
        public UILabel MessageLabel;
        public UIView SeperateView;
        public UIButton ActionButton;
        public UIButton SecondActionButton;
        public UIActivityIndicatorView ActivityIndicatorView;

        // Timer to dismiss the snackbar.
        private NSTimer dismissTimer;

        // Constraints.
        private NSLayoutConstraint heightConstraint;
        private NSLayoutConstraint leftMarginConstraint;
        private NSLayoutConstraint rightMarginConstraint;
        private NSLayoutConstraint bottomMarginConstraint;
        private NSLayoutConstraint topMarginConstraint;
        private NSLayoutConstraint actionButtonWidthConstraint;
        private NSLayoutConstraint secondActionButtonWidthConstraint;
        private NSLayoutConstraint iconImageViewWidthConstraint;

        public SnackBarBuilder() : base(CoreGraphics.CGRect.FromLTRB(0, 0, 320, 64))
        {
            Configure();
        }

        public void Show()
        {
            // Only show once
            if (this.Superview != null)
                return;

            // Create dismiss timer
            dismissTimer = NSTimer.CreateScheduledTimer(Duration, (t) => Dismiss());

            // Show or hide Icon
            IconImageView.Hidden = (Icon == null);

            // Show or hide action button
            ActionButton.Hidden = string.IsNullOrEmpty(ActionText) || ActionBlock == null;
            SecondActionButton.Hidden = string.IsNullOrEmpty(SecondActionText) || SecondActionBlock == null;

            SeperateView.Hidden = ActionButton.Hidden;

            iconImageViewWidthConstraint.Constant = IconImageView.Hidden ? 0 : SnackBarBuilder.snackbarIconImageViewWidth;
            actionButtonWidthConstraint.Constant = ActionButton.Hidden ? 0 : (SecondActionButton.Hidden ? SnackBarBuilder.snackbarActionButtonMaxWidth : SnackBarBuilder.snackbarActionButtonMinWidth);
            secondActionButtonWidthConstraint.Constant = SecondActionButton.Hidden ? 0 : (ActionButton.Hidden ? SnackBarBuilder.snackbarActionButtonMaxWidth : SnackBarBuilder.snackbarActionButtonMinWidth);

            this.LayoutIfNeeded();

            var localSuperView = UIApplication.SharedApplication.KeyWindow;
            if (localSuperView != null)
            {
                NSObject layoutGuide = localSuperView;

                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    layoutGuide = localSuperView.SafeAreaLayoutGuide;
                }

                localSuperView.AddSubview(this);

                topMarginConstraint = NSLayoutConstraint.Create(
                    this,
                    NSLayoutAttribute.Top,
                    NSLayoutRelation.Equal,
                    layoutGuide,
                    NSLayoutAttribute.Top,
                    1,
                    TopMargin);

                heightConstraint = NSLayoutConstraint.Create(
                    this,
                    NSLayoutAttribute.Height,
                    NSLayoutRelation.GreaterThanOrEqual,
                    null,
                    NSLayoutAttribute.NoAttribute,
                    1,
                    Height);

                leftMarginConstraint = NSLayoutConstraint.Create(
                    this,
                    NSLayoutAttribute.Left,
                    NSLayoutRelation.Equal,
                    layoutGuide,
                    NSLayoutAttribute.Left,
                    1,
                    LeftMargin);

                rightMarginConstraint = NSLayoutConstraint.Create(
                    this,
                    NSLayoutAttribute.Right,
                    NSLayoutRelation.Equal,
                    layoutGuide,
                    NSLayoutAttribute.Right,
                    1,
                    -RightMargin);

                bottomMarginConstraint = NSLayoutConstraint.Create(
                    this,
                    NSLayoutAttribute.Bottom,
                    NSLayoutRelation.Equal,
                    layoutGuide,
                    NSLayoutAttribute.Bottom,
                    1,
                    -BottomMargin);

                leftMarginConstraint.Priority = 999;
                rightMarginConstraint.Priority = 999;

                this.AddConstraint(heightConstraint);
                localSuperView.AddConstraint(leftMarginConstraint);
                localSuperView.AddConstraint(rightMarginConstraint);

                switch (LocationType)
                {
                    case SnackbarLocation.Top:
                        localSuperView.AddConstraint(topMarginConstraint);
                        break;
                    default:
                        localSuperView.AddConstraint(bottomMarginConstraint);
                        break;
                }


                // Show
                ShowWithAnimation();
            }
            else
            {
                Console.WriteLine("TTGSnackbar needs a keyWindows to display.");
            }
        }

        /// <summary>
        /// Dismiss the snackbar manually..
        /// </summary>
        public void Dismiss()
        {
            this.DismissAnimated(true);
        }

        /// <summary>
        /// Configure this instance.
        /// </summary>
        private void Configure()
        {
            this.TranslatesAutoresizingMaskIntoConstraints = false;
            this.BackgroundColor = UIColor.LightGray;
            this.Layer.CornerRadius = CornerRadius;
            this.Layer.MasksToBounds = true;
            //this.Layer.ShadowColor = new UIColor(red: 0.25f, green: 0.64f, blue: 0.83f, alpha: 0.01f).CGColor;
            //this.Layer.ShadowOpacity = 0.6f;
            //this.Layer.ShadowOffset = new CGSize(360, 44);

            CGColor[] colors = new CGColor[] { new UIColor(red: 0.25f, green:0.64f, blue:0.83f, alpha:0.01f).CGColor,
                new UIColor(red: 75/255, green: 128 / 255, blue: 153 / 255, alpha: 1.0f ).CGColor};
            CAGradientLayer gradientLayer = new CAGradientLayer();
            gradientLayer.Frame = new CGRect(0,0,360,64);
            gradientLayer.Colors = colors;
            this.Layer.InsertSublayer(gradientLayer, 0);

            IconImageView = new UIImageView();
            IconImageView.TranslatesAutoresizingMaskIntoConstraints = false;
            IconImageView.BackgroundColor = UIColor.Clear;
            IconImageView.ContentMode = IconContentMode;

            this.AddSubview(IconImageView);

            MessageLabel = new UILabel();
            MessageLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            MessageLabel.TextColor = UIColor.White;
            MessageLabel.Font = MessageTextFont;
            MessageLabel.BackgroundColor = UIColor.Clear;
            MessageLabel.LineBreakMode = UILineBreakMode.CharacterWrap;
            MessageLabel.Lines = 2;
            MessageLabel.TextAlignment = UITextAlignment.Natural;
            MessageLabel.Text = Message;
            this.AddSubview(MessageLabel);

            ActionButton = new UIButton();
            ActionButton.TranslatesAutoresizingMaskIntoConstraints = false;
            ActionButton.Hidden = string.IsNullOrEmpty(ActionText);
            ActionButton.BackgroundColor = UIColor.Clear;
            ActionButton.TitleLabel.Font = ActionTextFont;
            ActionButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
            ActionButton.SetTitle(ActionText, UIControlState.Normal);
            ActionButton.SetTitleColor(ActionTextColor, UIControlState.Normal);
            ActionButton.TouchUpInside += (s, e) =>
            {
                if (!ActionButton.Hidden && ActionButton.Title(UIControlState.Normal) != String.Empty && ActionButton != null)
                {
                    ActionBlock(this);
                    DismissAnimated(true);
                }
            };

            this.AddSubview(ActionButton);

            SecondActionButton = new UIButton();
            SecondActionButton.TranslatesAutoresizingMaskIntoConstraints = false;
            SecondActionButton.BackgroundColor = UIColor.Clear;
            SecondActionButton.TitleLabel.Font = SecondActionTextFont;
            SecondActionButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
            SecondActionButton.SetTitle(SecondActionText, UIControlState.Normal);
            SecondActionButton.SetTitleColor(SecondActionTextColor, UIControlState.Normal);
            SecondActionButton.TouchUpInside += (s, e) =>
            {
                if (!SecondActionButton.Hidden && SecondActionButton.Title(UIControlState.Normal) != String.Empty && SecondActionBlock != null)
                {
                    SecondActionBlock(this);
                    DismissAnimated(true);
                }
            };

            this.AddSubview(SecondActionButton);

            SeperateView = new UIView();
            SeperateView.TranslatesAutoresizingMaskIntoConstraints = false;
            SeperateView.BackgroundColor = UIColor.Gray;

            this.AddSubview(SeperateView);

            ActivityIndicatorView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White);
            ActivityIndicatorView.TranslatesAutoresizingMaskIntoConstraints = false;
            ActivityIndicatorView.StopAnimating();

            this.AddSubview(ActivityIndicatorView);

            // Add constraints        
            InvalidateHorizontalConstraints();

            var vConstraintsForIconImageView = NSLayoutConstraint.FromVisualFormat(
                "V:|-2-[iconImageView]-2-|", 0, new NSDictionary(), NSDictionary.FromObjectsAndKeys(new NSObject[] { IconImageView }, new NSObject[] { new NSString("iconImageView") })
            );

            var vConstraintsForMessageLabel = NSLayoutConstraint.FromVisualFormat(
                "V:|-0-[messageLabel]-0-|", 0, new NSDictionary(), NSDictionary.FromObjectsAndKeys(new NSObject[] { MessageLabel }, new NSObject[] { new NSString("messageLabel") })
            );

            var vConstraintsForSeperateView = NSLayoutConstraint.FromVisualFormat(
                "V:|-4-[seperateView]-4-|", 0, new NSDictionary(), NSDictionary.FromObjectsAndKeys(new NSObject[] { SeperateView }, new NSObject[] { new NSString("seperateView") })
            );

            var vConstraintsForActionButton = NSLayoutConstraint.FromVisualFormat(
                "V:|-0-[actionButton]-0-|", 0, new NSDictionary(), NSDictionary.FromObjectsAndKeys(new NSObject[] { ActionButton }, new NSObject[] { new NSString("actionButton") })
            );

            var vConstraintsForSecondActionButton = NSLayoutConstraint.FromVisualFormat(
                "V:|-0-[secondActionButton]-0-|", 0, new NSDictionary(), NSDictionary.FromObjectsAndKeys(new NSObject[] { SecondActionButton }, new NSObject[] { new NSString("secondActionButton") })
            );

            iconImageViewWidthConstraint = NSLayoutConstraint.Create(IconImageView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, SnackBarBuilder.snackbarIconImageViewWidth);

            actionButtonWidthConstraint = NSLayoutConstraint.Create(ActionButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, SnackBarBuilder.snackbarActionButtonMinWidth);

            secondActionButtonWidthConstraint = NSLayoutConstraint.Create(SecondActionButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, SnackBarBuilder.snackbarActionButtonMinWidth);

            var vConstraintsForActivityIndicatorView = NSLayoutConstraint.FromVisualFormat(
                "V:|-2-[activityIndicatorView]-2-|", 0, new NSDictionary(), NSDictionary.FromObjectsAndKeys(new NSObject[] { ActivityIndicatorView }, new NSObject[] { new NSString("activityIndicatorView") })
            );

            var hConstraintsForActivityIndicatorView = NSLayoutConstraint.FromVisualFormat(
                "H:[activityIndicatorView]-2-|",
                0,
                new NSDictionary(),
                NSDictionary.FromObjectsAndKeys(
                    new NSObject[] { ActivityIndicatorView },
                    new NSObject[] { new NSString("activityIndicatorView") })
            );

            IconImageView.AddConstraint(iconImageViewWidthConstraint);
            ActionButton.AddConstraint(actionButtonWidthConstraint);
            SecondActionButton.AddConstraint(secondActionButtonWidthConstraint);

            this.AddConstraints(hConstraints);
            this.AddConstraints(vConstraintsForIconImageView);
            this.AddConstraints(vConstraintsForMessageLabel);
            this.AddConstraints(vConstraintsForSeperateView);
            this.AddConstraints(vConstraintsForActionButton);
            this.AddConstraints(vConstraintsForSecondActionButton);
            this.AddConstraints(vConstraintsForActivityIndicatorView);
            this.AddConstraints(hConstraintsForActivityIndicatorView);

        }

        /// <summary>
        /// Invalid the dismiss timer.
        /// </summary>
        private void InvalidDismissTimer()
        {
            if (dismissTimer != null)
            {
                dismissTimer.Invalidate();
                dismissTimer = null;
            }
        }


        private void InvalidateHorizontalConstraints()
        {
            if (hConstraints != null && hConstraints.Length > 0)
                this.RemoveConstraints(hConstraints);


            hConstraints = NSLayoutConstraint.FromVisualFormat(
                $"H:|-20-[iconImageView]-{MessageMarginLeft}-[messageLabel]-{MessageMarginRight}-[seperateView(0.5)]-2-[actionButton(>=44@999)]-0-[secondActionButton(>=44@999)]-0-|",
                0, new NSDictionary(),
                NSDictionary.FromObjectsAndKeys(
                    new NSObject[] {
                        IconImageView,
                        MessageLabel,
                        SeperateView,
                        ActionButton,
                        SecondActionButton
                }, new NSObject[] {
                    new NSString("iconImageView"),
                    new NSString("messageLabel"),
                    new NSString("seperateView"),
                    new NSString("actionButton"),
                    new NSString("secondActionButton")
                })
            );
            this.AddConstraints(hConstraints);
        }

        /// <summary>
        /// If dismiss with animation.
        /// </summary>
        private void DismissAnimated(bool animated)
        {
            InvalidDismissTimer();

            ActivityIndicatorView.StopAnimating();

            nfloat superViewWidth = 0;

            if (Superview != null)
                superViewWidth = Superview.Frame.Width;

            if (!animated)
            {
                DismissAndPerformAction();
                return;
            }

            Action animationBlock = () => { };

            switch (AnimationType)
            {
                case SnackbarAnimationType.FadeInFadeOut:
                    animationBlock = () => { this.Alpha = 0; };
                    break;
                case SnackbarAnimationType.SlideFromBottomBackToBottom:
                    animationBlock = () => { bottomMarginConstraint.Constant = Height; };
                    break;
                case SnackbarAnimationType.SlideFromBottomToTop:
                    animationBlock = () => { this.Alpha = 0; bottomMarginConstraint.Constant = -Height - BottomMargin; };
                    break;
                case SnackbarAnimationType.SlideFromLeftToRight:
                    animationBlock = () => { leftMarginConstraint.Constant = LeftMargin + superViewWidth; rightMarginConstraint.Constant = -RightMargin + superViewWidth; };
                    break;
                case SnackbarAnimationType.SlideFromRightToLeft:
                    animationBlock = () =>
                    {
                        leftMarginConstraint.Constant = LeftMargin - superViewWidth;
                        rightMarginConstraint.Constant = -RightMargin - superViewWidth;
                    };
                    break;
            };

            this.SetNeedsLayout();

            UIView.Animate(AnimationDuration, 0, UIViewAnimationOptions.CurveEaseIn, animationBlock, DismissAndPerformAction);
        }

        void DismissAndPerformAction()
        {
            if (DismissBlock != null)
            {
                DismissBlock(this);
            }

            this.RemoveFromSuperview();
        }

        /// <summary>
        /// Shows with animation.
        /// </summary>
        private void ShowWithAnimation()
        {
            Action animationBlock = () => { this.LayoutIfNeeded(); };
            var superViewWidth = Superview.Frame.Width;

            switch (AnimationType)
            {
                case SnackbarAnimationType.FadeInFadeOut:
                    this.Alpha = 0;
                    this.SetNeedsLayout();

                    animationBlock = () => { this.Alpha = 1; };
                    break;
                case SnackbarAnimationType.SlideFromBottomBackToBottom:
                case SnackbarAnimationType.SlideFromBottomToTop:
                    bottomMarginConstraint.Constant = -BottomMargin;
                    this.LayoutIfNeeded();
                    break;
                case SnackbarAnimationType.SlideFromLeftToRight:
                    leftMarginConstraint.Constant = LeftMargin - superViewWidth;
                    rightMarginConstraint.Constant = -RightMargin - superViewWidth;
                    bottomMarginConstraint.Constant = -BottomMargin;
                    this.LayoutIfNeeded();
                    break;
                case SnackbarAnimationType.SlideFromRightToLeft:
                    leftMarginConstraint.Constant = LeftMargin + superViewWidth;
                    rightMarginConstraint.Constant = -RightMargin + superViewWidth;
                    bottomMarginConstraint.Constant = -BottomMargin;
                    this.LayoutIfNeeded();
                    break;
            };

            // Final state
            bottomMarginConstraint.Constant = -BottomMargin;
            leftMarginConstraint.Constant = LeftMargin;
            rightMarginConstraint.Constant = -RightMargin;
            topMarginConstraint.Constant = TopMargin;

            UIView.Animate(AnimationDuration, 0, UIViewAnimationOptions.CurveEaseIn, animationBlock, null);
        }

        private void DoAction(UIButton button)
        {
            // Call action block first
            if (button == ActionButton)
            {
                ActionBlock(this);
            }
            else if (button == SecondActionButton)
            {
                SecondActionBlock(this);
            }

            DismissAnimated(true);
        }
    }
}