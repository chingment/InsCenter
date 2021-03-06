﻿using System;


namespace Lumos.Session
{
    public static class SSOUtil
    {
        private static string sign = "token";
        public static void SetTokenInfo(string key, TokenInfo userInfo)
        {
            var session = new Session();
            session.Set(string.Format("{0}:{1}", sign, key), userInfo);
        }

        public static void SetTokenInfo(string key, TokenInfo userInfo, TimeSpan expireIn)
        {
            var session = new Session();
            session.Set(string.Format("{0}:{1}", sign, key), userInfo, expireIn);
        }

        public static TokenInfo GetTokenInfo(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            var session = new Session();
            return session.Get<TokenInfo>(string.Format("{0}:{1}", sign, key));
        }

        public static void Postpone(string key)
        {
            var session = new Session();

            session.Postpone(string.Format("{0}:{1}", sign, key));
        }

        public static void Quit(string key)
        {
            var session = new Session();

            session.Quit(string.Format("{0}:{1}", sign, key));
        }
    }
}
