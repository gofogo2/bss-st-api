using bss_st_api.Helpers;
using Launcher_SE.Helpers;
using Rito;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using testMouse.Helpers;
using static Rito.HookHelper;

namespace bss_st_api
{
    public partial class MainWindow : Window
    {
        float interval = 5.0f;

        System.Drawing.Point iconPT = new System.Drawing.Point(79, 1272);

        System.Drawing.Point pt01 = new System.Drawing.Point(75, 1258);
        System.Drawing.Point pt02 = new System.Drawing.Point(440, 10);
        System.Drawing.Point pt03 = new System.Drawing.Point(124, 239);
        System.Drawing.Point pt04 = new System.Drawing.Point(25, 55);
        System.Drawing.Color c = System.Drawing.Color.FromArgb(1, 0, 120, 212);
        HookHelper hook;

        string currentValue;
        bool isSave = false;
        bool isEdit = false;
        bool isServer = false;

        DispatcherTimer timer;

        public bool UtilHeper { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Instance_PacketReceiveEventHandlerEvent(string code)
        {
            if (!isEdit)
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
            else
            {

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
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(interval);
            timer.Tick += Timer_Tick;

            hook = new HookHelper();
            hook.Begin();
            hook.OnRightButtonUp += Hook_OnRightButtonUp;

            btnSave.Click += BtnSave_Click;
            btnServer.Click += BtnServer_Click;
            UdpHelper.Instance.Start();
            UdpHelper.Instance.PacketReceiveEventHandlerEvent += Instance_PacketReceiveEventHandlerEvent;
        }

        private void BtnServer_Click(object sender, RoutedEventArgs e)
        {
            if (isServer)
            {
                isServer = false;
                timer.Stop();
                btnServer.Content = "Server Stop";
            }
            else
            {
                isServer = true;
                timer.Stop();
                timer.Start();
                btnServer.Content = "Server Start";
            }


        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            var bounds = Screen.PrimaryScreen.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(System.Drawing.Point.Empty, System.Drawing.Point.Empty, bounds.Size);
                }

                //현재 위치 좌표
                System.Drawing.Color color = bitmap.GetPixel(iconPT.X, iconPT.Y);

                if (UtilHelper.diff(color, c))
                {
                    Debug.WriteLine("실행중");
                }
                else
                {
                    isEdit = true;
                    hook.ForceSetCursor(pt01.X, pt01.Y);
                    hook.ForceLeftClick();
                    list.Items.Insert(0, "아이콘 실행");
                    await Task.Delay(5000);

                    hook.ForceSetCursor(pt02.X, pt02.Y);
                    hook.ForceLeftClick();
                    list.Items.Insert(0, "전체화면 체크");
                    await Task.Delay(1000);

                    hook.ForceSetCursor(pt03.X, pt03.Y);
                    hook.ForceLeftClick();
                    list.Items.Insert(0, "자동화 실행");
                    await Task.Delay(2000);

                    hook.ForceSetCursor(pt04.X, pt04.Y);
                    hook.ForceLeftClick();
                    list.Items.Insert(0, "탭 최소화");
                    await Task.Delay(2000);
                    isEdit = false;
                }
            }
        }
        public void Write(string text)
        {
            list.Items.Insert(0, text);
        }
    }
}
