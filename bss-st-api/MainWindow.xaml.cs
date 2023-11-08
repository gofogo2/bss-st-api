using Launcher_SE.Helpers;
using Rito;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using testMouse.Helpers;
using static Rito.HookHelper;

namespace bss_st_api
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        HookHelper hook;
        string currentValue;
        bool isSave = false;
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Instance_PacketReceiveEventHandlerEvent(string code)
        {
            string index = code.Split('|')[1];

            var coordinate = XmlHelper.Instance.Get(index);
            if (!coordinate.Equals("fail"))
            {
                int x = int.Parse(coordinate.Split(':')[0]);
                int y = int.Parse(coordinate.Split(':')[1]);

                hook.ForceSetCursor(x, y);
                hook.ForceLeftClick();
            }
        }

        private void Hook_OnRightButtonUp(MouseHookInfo mouseStruct)
        {
            if (isSave)
            {
                MousePoint point = hook.GetCursorPosition();
                currentValue = $"{point.x}:{point.y}";
                Write(currentValue);
                XmlHelper.Instance.Save(txt_index.Text, currentValue);
                txt_index.Text = (int.Parse(txt_index.Text) + 1).ToString();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (isSave)
            {
                isSave = false;
                btnSave.Content = "Save Off";
            }
            else
            {
                isSave = true;
                btnSave.Content = "Save On";
            }
        }

        public void Init()
        {
            hook = new HookHelper();
            hook.Begin();
            hook.OnRightButtonUp += Hook_OnRightButtonUp;


            btnSave.Click += BtnSave_Click;

            UdpHelper.Instance.Start();
            UdpHelper.Instance.PacketReceiveEventHandlerEvent += Instance_PacketReceiveEventHandlerEvent;

        }

        public void Write(string text)
        {
            list.Items.Insert(0, text);
        }
    }
}
