﻿
using amocrm.library.Configurations;
using amocrm.library.Extensions;
using amocrm.library.Interfaces;
using amocrm.library.Models;
using amocrm.library.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace amocrm.library
{
    public class AmoCrmProvider : ICrmProvider
    {
        static DateTime cookiesLiveTime;

        HttpClient client;
        HttpClientHandler handler;
        CrmEndPointConfig endPoint;
        private readonly string login;
        private readonly string pass;

        static object locker = new object();

        public AuthContent AuthInfo { get; set; }

        public TimeSpan ServerTimeDiff { get; set; }

        public AmoCrmProvider(string account, string login, string pass)
        {
            endPoint = new CrmEndPointConfig(account);
            this.login = login;
            this.pass = pass;
        }

        public async System.Threading.Tasks.Task Auth()
        {
            Debug.WriteLine($"Соединение с CRM");

            handler = new HttpClientHandler { CookieContainer = new CookieContainer(), UseCookies = true, UseDefaultCredentials = true };

            client = new HttpClient(handler);

            var requestParams = new Dictionary<string, string> { { "USER_LOGIN", this.login}, { "USER_HASH", this.pass } };

            var content = new FormUrlEncodedContent(requestParams);
            var request = await client.PostAsync(endPoint.Auth, content);
            var response = await request.Content.ReadAsStringAsync();

            if (!request.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<AuthResponse>(response);
                var exception = new AmoCrmHttpException(error);
                throw exception;
            }

            AuthInfo = JsonConvert.DeserializeObject<AuthResponse>(response).Content;
            ServerTimeDiff = (DateTime.Now - new DateTime().FromTimestamp(AuthInfo.ServerTime)) * -1;

            lock (locker)
            {
                AmoCrmProvider.cookiesLiveTime = DateTime.Now;
            }
        }

        public bool AuthCookiesLifeTime()
        {
            lock (locker)
            {
                var lifeTime = TimeSpan.FromMinutes(13);

                return DateTime.Now.Subtract(AmoCrmProvider.cookiesLiveTime) < lifeTime & handler != null;
            }
        }

        public async Task<HttpClient> GetClient()
        {
            if (!AuthCookiesLifeTime()) await Auth();

            return client;
        }

        public Uri GetEndPoint<TPoint>()
        {
            return endPoint.GetUrl<TPoint>();
        }

    }
}
