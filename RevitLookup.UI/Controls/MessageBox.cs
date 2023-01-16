﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using RevitLookup.UI.Common;
using RevitLookup.UI.Controls.Window;
using RevitLookup.UI.Interop;

namespace RevitLookup.UI.Controls;

/// <summary>
/// Customized window for notifications.
/// </summary>
[ToolboxItem(true)]
[ToolboxBitmap(typeof(MessageBox), "MessageBox.bmp")]
public class MessageBox : System.Windows.Window
{
    /// <summary>
    /// Property for <see cref="Footer"/>.
    /// </summary>
    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(nameof(Footer),
        typeof(object), typeof(MessageBox), new PropertyMetadata(null));

    /// <summary>
    /// Property for <see cref="ShowFooter"/>.
    /// </summary>
    public static readonly DependencyProperty ShowFooterProperty = DependencyProperty.Register(nameof(ShowFooter),
        typeof(bool), typeof(MessageBox), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="ShowTitle"/>.
    /// </summary>
    public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(nameof(ShowTitle),
        typeof(bool), typeof(MessageBox), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="MicaEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty MicaEnabledProperty = DependencyProperty.Register(nameof(MicaEnabled),
        typeof(bool), typeof(MessageBox), new PropertyMetadata(true));

    /// <summary>
    /// Property for <see cref="ButtonLeftName"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonLeftNameProperty = DependencyProperty.Register(nameof(ButtonLeftName),
        typeof(string), typeof(MessageBox), new PropertyMetadata("Action"));

    /// <summary>
    /// Property for <see cref="ButtonLeftAppearance"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonLeftAppearanceProperty = DependencyProperty.Register(nameof(ButtonLeftAppearance),
        typeof(Controls.ControlAppearance), typeof(MessageBox),
        new PropertyMetadata(Controls.ControlAppearance.Primary));

    /// <summary>
    /// Routed event for <see cref="ButtonLeftClick"/>.
    /// </summary>
    public static readonly RoutedEvent ButtonLeftClickEvent = EventManager.RegisterRoutedEvent(
        nameof(ButtonLeftClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MessageBox));

    /// <summary>
    /// Property for <see cref="ButtonRightName"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonRightNameProperty = DependencyProperty.Register(nameof(ButtonRightName),
        typeof(string), typeof(MessageBox), new PropertyMetadata("Close"));

    /// <summary>
    /// Property for <see cref="ButtonRightAppearance"/>.
    /// </summary>
    public static readonly DependencyProperty ButtonRightAppearanceProperty = DependencyProperty.Register(nameof(ButtonRightAppearance),
        typeof(Controls.ControlAppearance), typeof(MessageBox),
        new PropertyMetadata(Controls.ControlAppearance.Secondary));

    /// <summary>
    /// Routed event for <see cref="ButtonRightClick"/>.
    /// </summary>
    public static readonly RoutedEvent ButtonRightClickEvent = EventManager.RegisterRoutedEvent(
        nameof(ButtonRightClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MessageBox));

    /// <summary>
    /// Property for <see cref="TemplateButtonCommand"/>.
    /// </summary>
    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(MessageBox), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets a content of the <see cref="MessageBox"/> bottom element.
    /// </summary>
    public object Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    /// <summary>
    /// Gets or sets a value that determines whether to show the <see cref="Footer"/>.
    /// </summary>
    public bool ShowFooter
    {
        get => (bool)GetValue(ShowFooterProperty);
        set => SetValue(ShowFooterProperty, value);
    }

    /// <summary>
    /// Gets or sets a value that determines whether to show the <see cref="System.Windows.Window.Title"/> in <see cref="TitleBar"/>.
    /// </summary>
    public bool ShowTitle
    {
        get => (bool)GetValue(ShowTitleProperty);
        set => SetValue(ShowTitleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value that determines whether <see cref="MessageBox"/> should contain a <see cref="Wpf.Ui.Appearance.BackgroundType.Mica"/> effect.
    /// </summary>
    public bool MicaEnabled
    {
        get => (bool)GetValue(MicaEnabledProperty);
        set => SetValue(MicaEnabledProperty, value);
    }

    /// <summary>
    /// Name of the button on the left side of footer.
    /// </summary>
    public string ButtonLeftName
    {
        get => (string)GetValue(ButtonLeftNameProperty);
        set => SetValue(ButtonLeftNameProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/> of the button on the left, if available.
    /// </summary>
    public Controls.ControlAppearance ButtonLeftAppearance
    {
        get => (Controls.ControlAppearance)GetValue(ButtonLeftAppearanceProperty);
        set => SetValue(ButtonLeftAppearanceProperty, value);
    }

    /// <summary>
    /// Action triggered after clicking left button.
    /// </summary>
    public event RoutedEventHandler ButtonLeftClick
    {
        add => AddHandler(ButtonLeftClickEvent, value);
        remove => RemoveHandler(ButtonLeftClickEvent, value);
    }

    /// <summary>
    /// Name of the button on the right side of footer.
    /// </summary>
    public string ButtonRightName
    {
        get => (string)GetValue(ButtonRightNameProperty);
        set => SetValue(ButtonRightNameProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="ControlAppearance"/> of the button on the right, if available.
    /// </summary>
    public Controls.ControlAppearance ButtonRightAppearance
    {
        get => (Controls.ControlAppearance)GetValue(ButtonRightAppearanceProperty);
        set => SetValue(ButtonRightAppearanceProperty, value);
    }

    /// <summary>
    /// Action triggered after clicking right button.
    /// </summary>
    public event RoutedEventHandler ButtonRightClick
    {
        add => AddHandler(ButtonRightClickEvent, value);
        remove => RemoveHandler(ButtonRightClickEvent, value);
    }

    /// <summary>
    /// Command triggered after clicking the button on the Footer.
    /// </summary>
    public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

    /// <summary>
    /// Creates new instance and sets default <see cref="FrameworkElement.Loaded"/> event.
    /// </summary>
    public MessageBox()
    {
        SetWindowStartupLocation();
        Topmost = true;

        Height = 200;
        Width = 400;

        WindowStartupLocation = WindowStartupLocation.CenterOwner;

        SetValue(TemplateButtonCommandProperty, new RelayCommand<string>(o => OnTemplateButtonClick(o ?? String.Empty)));
    }

    /// Shows a <see cref="System.Windows.MessageBox"/>.
    public new void Show()
    {
        UnsafeNativeMethods.RemoveWindowTitlebarContents(this);
        WindowBackdrop.ApplyBackdrop(this, WindowBackdropType.Mica);

        base.Show();
    }

    /// <summary>
    /// Sets <see cref="System.Windows.Window.Title"/> and content of <see cref="System.Windows.Window"/>, then calls <see cref="MessageBox.Show()"/>.
    /// </summary>
    /// <param name="title"><see cref="System.Windows.Window.Title"/></param>
    /// <param name="content">Content of <see cref="System.Windows.Window"/></param>
    public void Show(string title, object content)
    {
        Title = title;
        Content = content;

        Show();
    }

    // TODO: Window height match content height.

    //protected override void OnContentChanged(object oldContent, object newContent)
    //{
    //    System.Diagnostics.Debug.WriteLine("New content");
    //    System.Diagnostics.Debug.WriteLine(newContent.GetType());

    //    if (newContent != null && newContent.GetType() == typeof(System.Windows.Controls.Grid))
    //        Height = (newContent as System.Windows.Controls.Grid).ActualHeight;

    //    base.OnContentChanged(oldContent, newContent);
    //}

    private void SetWindowStartupLocation()
    {
        if (Application.Current != null)
            Owner = Application.Current;
        else
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    private void OnTemplateButtonClick(string parameter)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(MessageBox)} button clicked with param: {parameter}", "Wpf.Ui.MessageBox");
#endif

        switch (parameter)
        {
            case "left":
                RaiseEvent(new RoutedEventArgs(ButtonLeftClickEvent, this));

                break;

            case "right":
                RaiseEvent(new RoutedEventArgs(ButtonRightClickEvent, this));

                break;
        }
    }
}
