﻿using ACC_Manager.Broadcast.Structs;
using ACCManager.Broadcast;
using ACCManager.Broadcast.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCManager.HUD.Overlay.Internal
{
    internal class BroadcastTracker : IDisposable
    {
        private static BroadcastTracker _instance;
        internal static BroadcastTracker Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BroadcastTracker();

                return _instance;
            }
        }

        private ACCUdpRemoteClient client;
        public bool IsConnected { get; private set; }

        private BroadcastTracker()
        {
            // fetch from json....
            this.Connect();


        }

        public event EventHandler<RealtimeUpdate> OnRealTimeUpdate;
        public event EventHandler<ConnectionState> OnConnectionStateChanged;
        public event EventHandler<TrackData> OnTrackDataUpdate;
        public event EventHandler<RealtimeCarUpdate> OnRealTimeCarUpdate;

        public void Connect()
        {
            client = new ACCUdpRemoteClient("127.0.0.1", 9000, string.Empty, string.Empty, string.Empty, 100);
            client.MessageHandler.OnRealtimeUpdate += (s, realTimeUpdate) => OnRealTimeUpdate?.Invoke(this, realTimeUpdate);
            client.MessageHandler.OnConnectionStateChanged += (int connectionId, bool connectionSuccess, bool isReadonly, string error) =>
            {
                ConnectionState state = new ConnectionState()
                {
                    ConnectionId = connectionId,
                    ConnectionSuccess = connectionSuccess,
                    IsReadonly = isReadonly,
                    Error = error
                };

                OnConnectionStateChanged?.Invoke(this, state);
            };
            client.MessageHandler.OnTrackDataUpdate += (s, trackData) => OnTrackDataUpdate?.Invoke(this, trackData);

            client.MessageHandler.OnRealtimeCarUpdate += (s, e) => OnRealTimeCarUpdate?.Invoke(this, e);

            this.IsConnected = true;
        }

        public void Disconnect()
        {
            client.Shutdown();
            client.Dispose();
            this.IsConnected = false;
        }

        public void Dispose()
        {
            this.Disconnect();
        }
    }
}
