using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectEvent.UI.Controls
{
    public class PageContainer : Control
    {
        #region 依赖属性
        public IServiceProvider ServiceProvider
        {
            get { return (IServiceProvider)GetValue(ServiceProviderProperty); }
            set { SetValue(ServiceProviderProperty, value); }
        }
        public static readonly DependencyProperty ServiceProviderProperty =
            DependencyProperty.Register("ServiceProvider", typeof(IServiceProvider), typeof(PageContainer));

        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(PageContainer), new PropertyMetadata("Content undefined!"));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }
        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(string), typeof(PageContainer), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnUriChanged)));

        private static void OnUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PageContainer;
            if (e.NewValue != e.OldValue)
            {
                control.LoadPage();
            }
        }
        #endregion

        private readonly string ProjectName;
        public PageContainer()
        {
            DefaultStyleKey = typeof(PageContainer);

            ProjectName = App.Current.GetType().Assembly.GetName().Name;

        }

        private void LoadPage()
        {
            if (Uri != string.Empty)
            {

                Type pageType = Type.GetType(ProjectName + ".Views." + Uri);
                Type pageVMType = Type.GetType(ProjectName + ".ViewModels." + Uri + "VM");
                if (pageType != null && ServiceProvider != null)
                {
                    var page = ServiceProvider.GetService(pageType) as Page;
                    if (page != null)
                    {
                        var pageVM = ServiceProvider.GetService(pageVMType);
                        if (pageVM != null)
                        {
                            page.DataContext = pageVM;
                        }
                        Content = page;
                    }
                    else
                    {
                        Debug.WriteLine("找不到Page：" + Uri+"，请确认已被注入");
                    }

                }

            }
        }
    }
}
