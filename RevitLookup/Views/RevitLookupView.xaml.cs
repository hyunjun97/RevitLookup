﻿// Copyright 2003-2022 by Autodesk, Inc.
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
// 
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Services.Contracts;
using RevitLookup.UI.Contracts;
using RevitLookup.UI.Controls.Navigation;

namespace RevitLookup.Views;

public sealed partial class RevitLookupView : IWindow
{
    private readonly IServiceScope _serviceScope;

    public RevitLookupView(IServiceScopeFactory scopeFactory, ISettingsService settingsService)
    {
        UI.Application.Current = this;
        InitializeComponent();

        _serviceScope = scopeFactory.CreateScope();
        var navigationService = _serviceScope.ServiceProvider.GetService<INavigationService>()!;
        var pageService = _serviceScope.ServiceProvider.GetService<IPageService>()!;
        var windowController = _serviceScope.ServiceProvider.GetService<IWindowController>()!;
        var dialogService = _serviceScope.ServiceProvider.GetService<IDialogService>()!;
        var snackbarService = _serviceScope.ServiceProvider.GetService<ISnackbarService>()!;

        windowController.SetControlledWindow(this);
        navigationService.SetPageService(pageService);
        navigationService.SetNavigationControl(RootNavigation);

        dialogService.SetDialogControl(RootDialog);
        snackbarService.SetSnackbarControl(RootSnackbar);
        snackbarService.Timeout = 3000;

        RootNavigation.TransitionDuration = settingsService.TransitionDuration;
        WindowBackdropType = settingsService.Background;

        Unloaded += UnloadServices;
        GotFocus += (sender, _) => { UI.Application.Current = (Window) sender; };
    }

    public IServiceProvider Context => _serviceScope.ServiceProvider;

    public Frame GetFrame()
    {
        return RootFrame;
    }
    
    public INavigation GetNavigation()
    {
        return RootNavigation;
    }
    
    public bool Navigate(Type pageType)
    {
        return RootNavigation.Navigate(pageType);
    }
    
    public void SetPageService(IPageService pageService)
    {
        RootNavigation.PageService = pageService;
    }

    public void Show(IntPtr handle)
    {
        ApplicationExtensions.Show(this, handle);
    }

    private void UnloadServices(object sender, RoutedEventArgs e)
    {
        _serviceScope.Dispose();
    }
}