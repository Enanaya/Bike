﻿using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using 客户端.Models;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace 客户端.View.Privacy
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NickName : Page
    {
        public NickName()
        {
            this.InitializeComponent();
        }

        string this_account = "";

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                this_account = e.Parameter.ToString();
            }
        }

        private void reNameBox_Loaded(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection conn = UserDatabase.GetDbConnection())
            {
                TableQuery<UserAccount> t = conn.Table<UserAccount>();
                var q = from s in t.AsParallel<UserAccount>()
                        orderby s.user_id
                        where s.user_id == this_account
                        select s;

                ///如何将用户登陆后的账户通信到这些页面呢？？这个分支采用导航传参数

                foreach (var item in q)
                {
                    reNameBox.Text = item.nickname.ToString();
                }

            }
        }

        private void BackB_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void SaveB_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection conn = UserDatabase.GetDbConnection())
            {
                TableQuery<UserAccount> t = conn.Table<UserAccount>();
                var q = (from s in t
                        orderby s.user_id
                        where s.user_id == this_account
                        select s).SingleOrDefault();

                q.nickname = reNameBox.Text;
                conn.Update(q);
                //q.First().nickname = reNameBox.Text;
                
                //conn.InsertOrReplace(q);
            }
            if (this.Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
