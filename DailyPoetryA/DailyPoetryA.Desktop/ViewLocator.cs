﻿using Avalonia.Controls;
using Avalonia.Controls.Templates;
using DailyPoetryA.Library.ViewModels;
using System;
namespace DailyPoetryA.Desktop
{
    public class ViewLocator: IDataTemplate
    {
        public Control? Build(object? data)
        {
            if (data is null)
                return null;

            var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal).Replace("DailyPoetryA.Library", "DailyPoetryA.Desktop");
            var type = Type.GetType(name);

            if (type != null)
            {
                var control = (Control)Activator.CreateInstance(type)!;
                control.DataContext = data;
                return control;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}
