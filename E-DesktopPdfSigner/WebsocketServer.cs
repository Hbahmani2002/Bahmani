using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesktopPdfSigner;
using Fleck2.Interfaces;
using System.Windows.Forms;
namespace DesktopPdfSigner
{

    public class WebsocketServer
    {
        public static List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();
        public static Fleck2.WebSocketServer server = new Fleck2.WebSocketServer("ws://localhost:2339");
        public static void Aaa()
        {
            try
            {
                Fleck2.FleckLog.Level = Fleck2.LogLevel.Debug;

                server.Start(socket =>
                        {
                            socket.OnOpen = () =>
                            {
                                //Console.WriteLine("Open!");
                                allSockets.Add(socket);
                            
                                
                            };
                            socket.OnClose = () =>
                      {
                          //Console.WriteLine("Close!");
                          allSockets.Remove(socket);
                      };
                            socket.OnMessage = message =>
               {
                   

                   allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
               };
                        });

                //  Process.Start("client.html");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        public static void Sender(string text)
        {

            Console.WriteLine($"SOCKET ({allSockets.Count}): {text}");
            foreach (var socket in allSockets.ToList())
            {
                socket.Send(text);
                
            }


        }
    }
}
