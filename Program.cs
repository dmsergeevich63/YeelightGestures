using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
//using WindowsInput.Native;
using WindowsInput.Events;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using YeelightAPI;
using YeelightAPI.Models;
using YeelightAPI.Models.ColorFlow;
using YeelightAPI.Models.Cron;
using System.Text.RegularExpressions;

namespace LowLevelControls.Sample2
{
    static class Program
    {
        static KeyboardHook kbdHook = new KeyboardHook();
        static MouseHook msHook = new MouseHook();
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern UInt32 GetWindowThreadProcessId(IntPtr hwnd, ref Int32 pid);

        static void Main(string[] args)
        {
            Keys downkey = Keys.Apps, downkkey = Keys.Apps;                                 //тут должно быть присвоено чтото не нужное
            Keys upkey = Keys.Apps, upkkey = Keys.Apps;
            uint kk = 0;
            int x1 = 0, kx1 = 0;
            int y1 = 0, ky1 = 0;
            int x2 = 0;
            int y2 = 0;
            int x3 = 0;
            int y3 = 0;
            int i = 0;

            bool app = false;
            string apptitle;//заголовок окна, можно юзать как p.MainWindowTitle
            string appname = ""; //имя окна 
            bool blockup = false, dbr = false, dbl = false, complite_upkey = true, lwin = false;
            int flag_kn_lr = 0;
            int lb = 0, rb = 0;

            int l1 = 0;
            int l2 = 0;
            //bool mp1=false;

            //  InputSimulator s = new InputSimulator();


            kbdHook.KeyDownEvent += (sender, vkCode, injected) =>
            {
                Console.WriteLine((Keys)vkCode + " Down" + (injected ? " (Injected)" : ""));
                //downkkey=
                //if ((Keys)vkCode == Keys.LWin)
                {
                    //result = true; 
                    downkey = (Keys)vkCode; downkey = (Keys)vkCode;
                    downkkey = (Keys)vkCode;

                }
                return false;
            };
            kbdHook.KeyPressEvent += (sender, vkCode, injected) =>
            {
               
                Console.WriteLine((Keys)vkCode + " Press" + (injected ? " (Injected)" : ""));
                
                
                return false;
            };
            kbdHook.KeyUpEvent += (sender, vkCode, injected) =>
            {
                upkey = (Keys)vkCode;
                upkkey = (Keys)vkCode;
                /*if ((Keys)vkCode == Keys.LWin & injected==false)
                {
                    return true;
                }*/
                Console.WriteLine((Keys)vkCode + " Up" + (injected ? " (Injected)" : ""));
                return false;
            };



            msHook.MouseDownEvent += (sender, vkCode, x, y, delta, injected) =>
             {
                 if ((Keys)vkCode == Keys.RButton) { rb = 1; }
                 if ((Keys)vkCode == Keys.LButton) { lb = 1; }
                 //bool result = false;


                 ////это надо                if (app = true) { /*делаем проверку л и п кн  и блокируем их*/}
                 //injected = true;//определение макросов или используеися вместо retorn false
                 //if ((Keys)vkCode == Keys.XButton2) 
                 { //& Keys.XButton1 & MButton
                  //result = true;
                    x1 = x;
                     y1 = y;
                     downkey = (Keys)vkCode;//после передачи down жест нпчинается
                    kk = vkCode;
                 }
                 Console.WriteLine((Keys)vkCode + $" Down on ({x}, {y})" + (injected ? " (Injected)" : ""));
                 if ((Keys)vkCode == Keys.XButton1) return true;
                 if ((Keys)vkCode == Keys.XButton2) return true;
                 if ((Keys)vkCode == Keys.RButton & injected == false) { return true; }


                 if ((Keys)vkCode == Keys.LButton & rb ==1) return true;
                 return false;
             };
            msHook.MouseUpEvent += (sender, vkCode, x, y, delta, injected) =>
            {
                if ((Keys)vkCode == Keys.RButton) { rb = 2; }
                if ((Keys)vkCode == Keys.LButton) { lb = 2; }

                upkey = (Keys)vkCode;
                if ((Keys)vkCode == downkey)
                {
                    //result = true; 
                    x2 = x;
                    y2 = y;
                    
                } //после передачи upkey очищается и жест завершается
                Console.WriteLine((Keys)vkCode + $" Up on ({x}, {y})" + (injected ? " (Injected)" : ""));
                if ((Keys)vkCode == Keys.XButton1) return true;//надо их потом в элсе взять
                if ((Keys)vkCode == Keys.XButton2) return true;
                //if (blockup == true & (Keys)vkCode == Keys.LWin) return true;                
                if ((Keys)vkCode == Keys.RButton & injected == false) { return true; } //

                if ((Keys)vkCode == Keys.LButton & rb == 1 ) return true;
                return false;
            };
            msHook.MouseMoveEvent += (sender, vkCode, x, y, delta, injected) =>
            {
                //if (downkey != Keys.Apps) return true;
                //if (y > 36) { mp1 }
                //Console.WriteLine($"Mouse Move to ({x}, {y})" + (injected ? " (Injected)" : ""));
                if ((Keys)vkCode == Keys.LWin)
                {
                    //result = true; 
                    kx1 = x;
                    ky1 = y;
                }
                x3 = x;
                y3 = y;
                return false;
            };
            msHook.MouseWheelEvent += (sender, vkCode, x, y, delta, injected) =>
            {

                if (app == true)//проверка что в этом приложении такое надо
                {
                    if (36 < y & y < 67)
                    { //это облать м/д вкладками и закладками
                        Console.WriteLine("мышь в области 1! " + delta);
                        if (delta != 0)
                        {

                            if (delta == 120) { Console.WriteLine("мышь в области 1 и зн! " + delta);// s.Keyboard.KeyDown(VirtualKeyCode.LEFT); 
                            }
                            else
                            {
                               // s.Keyboard.KeyDown(VirtualKeyCode.RIGHT);//VK_RIGHT
                                Console.WriteLine("мышь в области 1 и зн! " + delta);// s.Keyboard.KeyDown(VirtualKeyCode.RIGHT);
                            }
                        }
                        //return false; //без блокировки вроде тоже норм работает
                    }
                }
                //Console.WriteLine($"Mouse Wheel with data {delta} on ({vkCode}, {y})" + (injected ? " (Injected)" : ""));
                return false;
            };
            msHook.MouseHWheelEvent += (sender, vkCode, x, y, delta, injected) =>
            {
                Console.WriteLine($"Mouse HWheel with data {delta} on ({x}, {y})" + (injected ? " (Injected)" : ""));
                return true;
            };

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            kbdHook.InstallGlobalHook();
            msHook.InstallGlobalHook();

            for (; ; )
            {
                ///определение заголовка и имени программы
                if (upkey != Keys.Apps)
                {

                    while (true)
                    {
                        //Console.Write(" " + downkey, upkey);
                        IntPtr h = GetForegroundWindow();
                        int pid = 0;
                        GetWindowThreadProcessId(h, ref pid);
                        Process p = Process.GetProcessById(pid);
                        apptitle = p.MainWindowTitle;//заголовок окна, можно юзать как p.MainWindowTitle
                        appname = p.ProcessName; //имя окна 

                        if (pid != 0) break;
                    }
                    //Console.WriteLine("APP: {0}", appname);
                }

                //string appname="";
                if (appname == "1explorer") { app = false; } //or p.MainWindowTitle == @"explorer" @"Heroes" 
                else { app = true; }
                //Console.WriteLine(p.ProcessName+"  " +app);
                //if (app == true)


                //левая-правая
                {

                    /*  
                      if (downkey == Keys.LButton) { dbl = true; }
                      if (upkey == Keys.LButton) { dbl = false; }
                      if (dbl == true & downkey == Keys.RButton) { Console.WriteLine("!!!!!!!!!!!"); }
                  */
                }
                {
                    //ПРАВАЯ-левая
                    if (downkey == Keys.RButton) //lb
                    { dbr = true; } //по нему надо блокать кн правую кн тоже
                    if (downkey == Keys.LButton) //lb
                    { dbl = true; }
                    //Console.WriteLine("++++++++++++++" + dbl + dbr+upkey + complite_upkey);
                    //if (upkey == Keys.RButton) { dbr = false; }
                    //if (lb = 2 & rb = 0) { dbl = false; dbr = false; } ///dbr = false; почему то не отжимается поэтому он тут 
                    if (lb == 1 & rb == 1) { flag_kn_lr = 1; }
                    if (lb == 2 & rb== 2 & flag_kn_lr ==1) { Console.WriteLine("                 !!!!!!!!!!!" + dbl + dbr + upkey); lb = 0; lb = 0; flag_kn_lr = 0; }
                    //if (rb == 2) { lb = 0; lb = 0; }
                    //if (lb == 2 ) { lb = 0; lb = 0; }


                    /*
                    if (upkey == Keys.RButton) { dbr = false; }
                    if (upkey == Keys.LButton) { dbl = false; dbr = false; } ///dbr = false; почему то не отжимается поэтому он тут 

                    if (dbr == true) { dbr = false; }
                    */

                    //if (dbl == true & downkey == Keys.LButton) { Console.WriteLine("!!!!!!!!!!!"); }




                    // if (upkey == Keys.LButton) { dbl = false; }
                    // if (dbl == true & downkey == Keys.LButton) { Console.WriteLine("!!!!!!!!!!!"); }

                }

                // блокает правую
                {/*
                if (dbl == true & downkey == Keys.RButton)
                {
                    if (0 != gesture(x1, x3, y1, y3))
                    {
                        blockup = true;
                        Console.WriteLine(blockup);
                    }
                }
                //снятие блокировки
                if (upkey == Keys.RButton) { blockup = false; Console.WriteLine(blockup); }
                
                */
                }

                //lwin
                {/*
                    if (downkey == Keys.LWin) { lwin = true; }
                    if (upkey == Keys.LButton) { lwin = false; }
                    if (lwin == true & downkey == Keys.RButton) { Console.WriteLine("!!!!!!!!!!!"); }
                */
                }




                //функция(upkey) идея
                //прощитываем жест заранее
                //static void gesture2(Keys upkey)
                //Console.WriteLine(upkey);
              
                if (upkey == Keys.F24)
                {
                    Console.WriteLine("Switch lamp!!");

                    // Mainn();

                    i++;
                    Console.WriteLine(i);
                    int g = gesture(x1, x2, y1, y2);
                    switch (g)
                    {

                        case 0: Console.WriteLine(g); Mainn(); break; //жест 0 при
                        case 1: Console.WriteLine(g);
                          


                            break; 
                        case 2: Console.WriteLine(g);
                            switch (l1)
                            {
                                case 0:
                                    Toggle(1);
                                    l1++;
                                    break;
                                case 1:
                                    rgb(1);
                                    l1++;
                                    break;
                                //.............
                                case 2:
                                    ct(1);
                                    l1=0;
                                    break;


                                    break;
                            }
 break;
                        case 3: Console.WriteLine(g); moonlight_daylight(2); break;
                        case 4: Console.WriteLine(g); break;
                        case 5: Console.WriteLine(g); break;
                        case 6: Console.WriteLine(g); break;

                    }
                    complite_upkey = true;
                    /////upkey = Keys.Apps;
                    ///


                }
                upkey = Keys.Apps;
                /*if (complite_upkey == true)
                {
                    upkey = Keys.Apps;
                    complite_upkey = false;
                }*/


                downkkey = Keys.Apps; //для чего это???????
                upkkey = Keys.Apps;
                {
                                                ////старый свич
                                                //switch (upkey)
                                                //{
                                                //    //сместиться сюда(двойная проверка).убрать цикл

                                                //    case Keys.LButton:
                                                //        //blockup = true;
                                                //        Console.WriteLine("!===LButton");
                                                //        break;
                                                //    case Keys.RButton:
                                                //        /*if (downkey == Keys.LButton) { Console.WriteLine("!!!!!!!!!!!"); }
                                                //        Console.WriteLine("!===RButton"+ downkey);
                                                //        break;
                                                //        /*
                                                //        case Keys.XButton2:
                                                //            { 
                                                //            Console.WriteLine("!==XButton2");
                                                //            if (appname == @"devenv") {
                                                //                    int g =gesture(x1, x2, y1, y2);
                                                //                    switch (g) 
                                                //                    {
                                                //                        case 0: Console.WriteLine(g); break; //жест 0 при
                                                //                        case 1: Console.WriteLine(g); break;
                                                //                        case 2: Console.WriteLine(g); break;
                                                //                        case 3: Console.WriteLine(g); break;
                                                //                        case 4: Console.WriteLine(g); break;
                                                //                        case 5: Console.WriteLine(g); break;
                                                //                        case 6: Console.WriteLine(g); break;

                                                //                    }
                                                //                }
                                                //            break;
                                                //            }
                                                //        case Keys.XButton1:
                                                //            //тут надо узнать активное приложение и скорректировать отклик
                                                //            s.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.MENU }, new[] { VirtualKeyCode.TAB });
                                                //            Console.WriteLine("!=XButton1");
                                                //            break;
                                                //        */
                                                //}

                                                //if (downkey != Keys.Apps)
                                                //{
                                                //    //Console.WriteLine("!=");

                                                //}
                                                //if (upkey != Keys.Apps)
                                                //{
                                                //    downkey = Keys.Apps;
                                                //    //Console.WriteLine("!==");
                                                //    /////upkey = Keys.Apps;

                                                //}*/
            }

                Thread.Sleep(1);
                Application.DoEvents();
            }
        }
        static void upbtn(Keys upkey)
        {
            switch (upkey)
            {
                //сместиться сюда(двойная проверка).убрать цикл

                case Keys.Alt:
                    Console.WriteLine("!===ATL");

                    break;
                case Keys.RButton:
                    break;

            }

        }

        private static int gesture(int x1, int x2, int y1, int y2)
        {

            int xx = x2 - x1;
            int yy = y2 - y1;

            if (y2 == y1 & x2 == x1)//0нажатие
            {
                Console.WriteLine("нажатие без жеста");
                return 0;
            }

            if (xx > Math.Abs(yy))//2жест
            {
                Console.WriteLine("ЖЕСТ2");
                return 2;
            }
            if (xx > (yy))//1жест
            {
                Console.WriteLine("ЖЕСТ1");
                return 1;
            }
            if (Math.Abs(xx) < (yy))//3жест
            {
                Console.WriteLine("ЖЕСТ3");
                return 3;
            }
            if ((xx) < (yy))//4жест
            {
                Console.WriteLine("ЖЕСТ4");
                return 4;
            }
            return 5; //по идее не должно никогда вернуться
        }


        private static void SendKeys(string v)
        {
            throw new NotImplementedException();
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            kbdHook.UninstallGlobalHook();
            msHook.UninstallGlobalHook();
        }
        
        private static async Task Mainn()
        {
            await Simulate.Events()
                //Make it so we can see special characters.
                // Console.OutputEncoding = System.Text.Encoding.Unicode;
                //var sim = await EventBuilder.Create()
               // var sim = Simulate.Events()
                //.Wait(3000)
                .Click(ButtonCode.Right)
                //.Click(KeyCode.CapsLock).
                //.Wait(1000)
                .Invoke()
                 ;



            // return Task.CompletedTask;
        }

        private static async Task Mainn1()
        {

            await Simulate.Events()
                        //Make it so we can see special characters.
                        // Console.OutputEncoding = System.Text.Encoding.Unicode;
                        //var sim = await EventBuilder.Create()
                        // var sim = Simulate.Events()
                        //.Wait(3000)
                        .Click(KeyCode.CapsLock)
                        //.Wait(1000)
                        .Invoke()
                         ;



            // return Task.CompletedTask;
        }
        public static async Task Toggle(int idlamp)
        {
            try
            {
                string hostname=null;
                switch (idlamp)
                {
                    case 1: hostname = "192.168.0.9"; break; //жест 0 при
                    case 2: hostname = "192.168.0.10"; break;
                   // case 3: hostname = "192.168.0.10";  break;//"192.168.0.85"
                }
                


                using (Device device = new Device(hostname, 55443))
                {
                    bool success = true;

                    Console.WriteLine("connecting device ...");
                    success &= await device.Connect();

                    bool globalSuccess = true; int delay = 1500;
                    await Try(async () =>
                    {
                        Console.WriteLine("Setting!!!");
                        success = await device.Toggle();
                        //success = await device.SetColorTemperature(6500, delay);
                        globalSuccess &= success;
                        WriteLineWithColor($"command success : {success}", ConsoleColor.DarkCyan);
                        await Task.Delay(delay);
                    });

                }

            }
            catch (Exception ex)
            {
                //WriteLineWithColor($"An error has occurred : {ex.Message}", ConsoleColor.Red);
            }

            Console.WriteLine("Press Enter to continue ;)");
            Console.ReadLine();
        }
        public static async Task rgb(int idlamp)
        {
            try
            {
                string hostname = null;
                switch (idlamp)
                {
                    case 1: hostname = "192.168.0.9"; break; //жест 0 при
                    case 2: hostname = "192.168.0.10"; break;
                        // case 3: hostname = "192.168.0.10";  break;//"192.168.0.85"
                }



                using (Device device = new Device(hostname, 55443))
                {
                    bool success = true;

                    Console.WriteLine("connecting device ...");
                    success &= await device.Connect();

                    bool globalSuccess = true; int delay = 1500;
                    success = await device.TurnOn();
                    ColorFlow flow = new ColorFlow(0, ColorFlowEndAction.Restore);
                    
                    flow.Add(new ColorFlowRGBExpression(255, 0, 50, 100, 10000)); // color : red / brightness : 1% / duration : 500
                        flow.Add(new ColorFlowRGBExpression(0, 255, 100, 75, 50000)); // color : green / brightness : 100% / duration : 500
                        flow.Add(new ColorFlowRGBExpression(100, 0, 255, 100, 50000)); // color : blue / brightness : 50% / duration : 500
                        //flow.Add(new ColorFlowSleepExpression(2000)); // sleeps for 2 seconds
                      

                        device.StartColorFlow(flow); // start
                    Console.WriteLine("flow");



                }

            }
            catch (Exception ex)
            {
                WriteLineWithColor($"An error has occurred : {ex.Message}", ConsoleColor.Red);
            }

           
            Console.ReadLine();
        }
        public static async Task ct(int idlamp)
        {
            try
            {
                string hostname = null;
                switch (idlamp)
                {
                    case 1: hostname = "192.168.0.9"; break; //жест 0 при
                    case 2: hostname = "192.168.0.10"; break;
                        // case 3: hostname = "192.168.0.10";  break;//"192.168.0.85"
                }



                using (Device device = new Device(hostname, 55443))
                {
                    bool success = true;

                    Console.WriteLine("connecting device ...");
                    success &= await device.Connect();

                    bool globalSuccess = true; int delay = 1500;
                    await Try(async () =>
                    {
                        Console.WriteLine("Setting!!!");
                        //success = await device.Toggle();
                        success = await device.SetColorTemperature(6500, delay);

                        globalSuccess &= success;
                        WriteLineWithColor($"command success : {success}", ConsoleColor.DarkCyan);
                        await Task.Delay(delay);
                    });

                }

            }
            catch (Exception ex)
            {
                //WriteLineWithColor($"An error has occurred : {ex.Message}", ConsoleColor.Red);
            }

            Console.WriteLine("Press Enter to continue ;)");
            Console.ReadLine();
        }
        public static async Task moonlight_daylight(int idlamp)
        {
            try
            {
                string hostname = null;
                switch (idlamp)
                {
                    case 1: hostname = "192.168.0.9"; break; //жест 0 при
                    case 2: hostname = "192.168.0.10"; break;
                        // case 3: hostname = "192.168.0.10";  break;//"192.168.0.85"
                }



                using (Device device = new Device(hostname, 55443))
                {
                    bool success = true;

                    Console.WriteLine("connecting device ...");
                    success &= await device.Connect();

                    
                    bool globalSuccess = true;
                    int delay = 1500; int smooth = 1500;
                    if (device is IDeviceReader deviceReader)
                    {
                        //while (true) 
                        {

                            await Try(async () =>
                            {
                                Console.WriteLine("get cron ...");
                                CronResult cronResult = await deviceReader.CronGet(YeelightAPI.Models.Cron.CronType.PowerOff);
                                globalSuccess &= (cronResult != null);
                                WriteLineWithColor($"command success : {success}", ConsoleColor.DarkCyan);
                                await Task.Delay(delay);

                            });
                            await Try(async () =>
                            {
                                Console.WriteLine("getting all props ...");
                               /* Dictionary<PROPERTIES, object> result = await deviceReader.GetAllProps(); //получение всех настроек лампы
                                                                                                          //Console.WriteLine($"\tprops : {JsonConvert.SerializeObject(result)}"); //вывод их в консоль

                                bool power;
                                bool active_mode;
                                power = Regex.IsMatch(JsonConvert.SerializeObject(result), @"power"":""on");
                                active_mode = Regex.IsMatch(JsonConvert.SerializeObject(result), @"active_mode"":""1");
                                Console.WriteLine(power);
                                Console.WriteLine(active_mode);
                                if (active_mode == true)
                                {
                                    bool nl_br;
                                    nl_br = Regex.IsMatch(JsonConvert.SerializeObject(result), @"nl_br"":""100");
                                    Console.WriteLine(nl_br);
                                    if (nl_br == true)
                                    {
                                        await device.SetPower(true, smooth, PowerOnMode.Night); Console.WriteLine("Night");
                                        await device.SetBrightness(1, smooth);
                                    }
                                    else
                                    {

                                        await device.SetPower(true, smooth, PowerOnMode.Ct); Console.WriteLine("Ct");
                                        await device.SetBrightness(100, smooth);
                                    }
                                }
                                else
                                {
                                    await device.SetPower(true, smooth, PowerOnMode.Night); Console.WriteLine("Night");
                                    await device.SetBrightness(100, smooth);

                                }


                                //if (r == true) 
                                /*{
                                // включить подсветку
                                Console.WriteLine("Включаем подсветку");

                                    await Try(async () =>
                                    {
                                        Console.WriteLine("powering on ...");
                                        success = await backgroundDevice.BackgroundSetPower(true, smooth);
                                        globalSuccess &= success;
                                        WriteLineWithColor($"command success : {success}", ConsoleColor.DarkCyan);
                                        await Task.Delay(delay);
                                    });

                                    await Try(async () =>
                                    {
                                        Console.WriteLine("Starting color flow ...");
                                        int repeat = 0;
                                        ColorFlow flow = new ColorFlow(repeat, ColorFlowEndAction.Restore)
                                                            {
                                                                new ColorFlowRGBExpression(0, 0, 255, 100, 10000),
                                                                new ColorFlowRGBExpression(0, 255, 0, 100, 10000),
                                                                new ColorFlowRGBExpression(255, 0, 0, 100, 10000),

                                                            };

                                        success = await backgroundDevice.BackgroundStartColorFlow(flow);
                                        globalSuccess &= success;
                                        WriteLineWithColor($"command success : {success}", ConsoleColor.DarkCyan);

                                        await Task.Delay(30 * 1000);
                                    });

                            }*/
                                //await device.SetPower(true, smooth, PowerOnMode.Night); // moonlight
                                //await device.SetBrightness(100);
                                //await device.SetPower(true, smooth, PowerOnMode.Ct); // daylight
                                //await device.SetBrightness(100);


                                await Task.Delay(10000);
                            });

                        }
                        //await device.SetPower(true, smooth, PowerOnMode.Night); // moonlight
                        //await device.SetBrightness(100);
                        //await device.SetPower(true, smooth, PowerOnMode.Ct); // daylight
                        //await device.SetBrightness(100);
                    }


                }

            }
            catch (Exception ex)
            {
                //WriteLineWithColor($"An error has occurred : {ex.Message}", ConsoleColor.Red);
            }

            Console.WriteLine("Press Enter to continue ;)");
            Console.ReadLine();
        }
        private static async Task Try(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                WriteLineWithColor(ex.Message, ConsoleColor.Magenta);
            }
        }

        private static void WriteLineWithColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
