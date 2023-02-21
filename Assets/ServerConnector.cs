using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using TMPro;
using Debug = System.Diagnostics.Debug;

public class ServerConnector : MonoBehaviour
{
    public SocketIOUnity socket;
    public TMP_Text ReceivedText;

    // Start is called before the first frame update
    public Button ConnectButton;
    public TMP_InputField Username;
    public TMP_InputField Server;
    public Button LoginButton;
    
    public User thisUser = new User();
    void Start()
    {
        LoginButton.onClick.AddListener(LogIn);
        LoginButton.enabled = false;
        ConnectButton.onClick.AddListener(Connect);
    }

    void Connect()
    {
        //TODO: check the Uri if Valid.
        var uri = new Uri(Server.text);
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
            {
                { "token", "UNITY" }
            },
            EIO = 4,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        socket.JsonSerializer = new NewtonsoftJsonSerializer();

        ///// reserved socketio events
        socket.OnConnected += async (sender, e) =>
        {
            Debug.Print("socket.OnConnected");
            LoginButton.enabled = true;
            await EmitConnectionTest();
        };
        socket.OnPing += (sender, e) => { Debug.Print("Ping"); };
        socket.OnPong += (sender, e) => { Debug.Print("Pong: " + e.TotalMilliseconds); };
        socket.OnDisconnected += (sender, e) => { Debug.Print("disconnect: " + e); };
        socket.OnReconnectAttempt += (sender, e) => { Debug.Print($"{DateTime.Now} Reconnecting: attempt = {e}"); };
        ////

        Debug.Print("Connecting...");
        socket.Connect();
        

        //socket.OnUnityThread("spin", (data) => { rotateAngle = 0; });

        ReceivedText.text = "";
        socket.OnAnyInUnityThread((name, response) =>
        {
            ReceivedText.text += "Received On " + name + " : " + response.ToString() + "\n";
        });
        //socket.Emit("test");
    }

    public class User
    {
        public int Id = -1;
        public string Username = "";
    }
    void LogIn()
    {
        thisUser.Username = Username.text;
        socket.Emit("login", thisUser);
    }


    public static bool IsJSON(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return false;
        }

        str = str.Trim();
        if ((str.StartsWith("{") && str.EndsWith("}")) || //For object
            (str.StartsWith("[") && str.EndsWith("]"))) //For array
        {
            try
            {
                var obj = JToken.Parse(str);
                return true;
            }
            catch (Exception ex) //some other exception
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public async Task EmitConnectionTest()
    {
        UnityEngine.Debug.Log("attempting to...");
        await socket.EmitAsync("checkConnectivity", "owo");
    }
}