﻿#if !DISABLESTEAMWORKS && HE_SYSCORE && STEAMWORKSNET
using Steamworks;
using System;
using UnityEngine;

namespace HeathenEngineering.SteamworksIntegration
{
    /// <summary>
    /// Represents a ticekt such as is generated by a user and sent to start an authentication session.
    /// </summary>
    [Serializable]
    public class AuthenticationTicket
    {
        /// <summary>
        /// Indicates that this session is being managed by a client or server
        /// </summary>
        public bool IsClientTicket { get; private set; } = true;
        /// <summary>
        /// The authentication handle assoceated with this ticket
        /// </summary>
        public HAuthTicket Handle { get; private set; }
        /// <summary>
        /// The ticket data of this ticket ... this is what should be sent to servers for processing
        /// </summary>
        public byte[] Data { get; private set; }
        /// <summary>
        /// Has this ticket been verified, this gets set to true when the Get Authentication Session responce comes back from the Steamworks backend.
        /// </summary>
        public bool Verified { get; private set; }
        /// <summary>
        /// The Steamworks date time this ticket was created
        /// </summary>
        public uint CreatedOn { get; private set; }

        public EResult Result { get; private set; }
        public Action<AuthenticationTicket, bool> Callback { get; private set; }

        public AuthenticationTicket(SteamNetworkingIdentity forIDentity, Action<AuthenticationTicket, bool> callback, bool isClient = true)
        {
            Callback = callback;
            IsClientTicket = isClient;
            var array = new byte[1024];
            uint m_pcbTicket;
            if(isClient)
                Handle = SteamUser.GetAuthSessionTicket(array, 1024, out m_pcbTicket, ref forIDentity);
            else
                Handle = SteamGameServer.GetAuthSessionTicket(array, 1024, out m_pcbTicket, ref forIDentity);
            CreatedOn = SteamUtils.GetServerRealTime();
            Array.Resize(ref array, (int)m_pcbTicket);
            Data = array;
        }

        public void Authenticate(GetAuthSessionTicketResponse_t responce)
        {
            if (Handle != default(HAuthTicket) && Handle != HAuthTicket.Invalid
                    && responce.m_eResult == EResult.k_EResultOK)
            {
                Result = responce.m_eResult;
                Verified = true;
                Callback?.Invoke(this, false);
            }
            else
            {
                Result = responce.m_eResult;
                Callback?.Invoke(this, true);
            }
        }

        /// <summary>
        /// The age of this ticket from the current server realtime
        /// </summary>
        public TimeSpan Age
        {
            get { return new TimeSpan(0, 0, (int)(SteamUtils.GetServerRealTime() - CreatedOn)); }
        }

        /// <summary>
        /// Cancels the ticekt
        /// </summary>
        public void Cancel()
        {
            if (IsClientTicket)
                SteamUser.CancelAuthTicket(Handle);
            else
                SteamGameServer.CancelAuthTicket(Handle);
        }
    }
    }
#endif