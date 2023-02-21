using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Netcade.Networking;
using Netcade.Objects;
using Netcade.SocketObjects;
using Netcade.UI.Interfaces;
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
    public Button ConnectButton;
    public TMP_InputField Username;
    public TMP_InputField Server;
    public Button LoginButton;
    public Button CreateLobbyButton;

    void Start()
    {
        LoginButton.onClick.AddListener(LogIn);
        ConnectButton.onClick.AddListener(Connect);
        CreateLobbyButton.onClick.AddListener(CreateLobby);
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

    private bool loggedIn = false;

    void InitializeListeners()
    {
        socket.On("joinLobby", JoinLobby);
        socket.On("newLobby", JoinLobby);
        socket.On("lobbyList", AvailableLobbies);
    }

    void LogIn()
    {
        ServerData.ThisUser = new User();
        socket.On("yourId", (e) =>
        {
            ServerData.ThisUser.Id = e.GetValue<int>();
            UnityEngine.Debug.Log("Got USER ID! - " + ServerData.ThisUser.Id);
            loggedIn = true;
            InitializeListeners();
        });
        ServerData.ThisUser.Username = Username.text;
        socket.Emit("login", ServerData.ThisUser);
    }

    void CreateLobby()
    {
        socket.Emit("createLobby");
    }

    void NewLobby(SocketIOResponse response)
    {
        // TODO: show
    }

    // void JoinGame(int id)
    // {
    //     // send
    //     socket.Emit("joinGame", id);
    // }

    void JoinLobby(SocketIOResponse response)
    {
        UnityEngine.Debug.Log("Attempting to join game...");
        // recieve
        try
        {
            SOLobby lobby = response.GetValue<SOLobby>();
            UnityEngine.Debug.Log("Joining: " + lobby.name);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e);
        }
    }

    public void AvailableLobbies(SocketIOResponse response)
    {
        try
        {
            SOLobby[] lobbies = response.GetValue<SOLobby[]>();
            Lobby[] finalLobbies = new Lobby[lobbies.Length];

            for (var index = 0; index < lobbies.Length; index++)
            {
                finalLobbies[index] = new Lobby(lobbies[index]);
            }

            ServerData.Lobbies = finalLobbies;
            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                var ss = FindObjectsOfType<MonoBehaviour>().OfType<ILobbyListUpdated>();
                foreach (ILobbyListUpdated s in ss)
                {
                    s.GamesListUpdated();
                }
            });
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e);
        }
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