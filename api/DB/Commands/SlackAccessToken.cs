using System;
using lookaroond.Controllers;

namespace lookaroond.DB.Commands
{
    public class SlackAccessToken
    {
        public SlackAccessToken(SlackOauthResponse access)
        {
            Access = access;
            CreateOn = DateTime.Now;
        }

        public SlackOauthResponse Access { get; }
        public DateTime CreateOn { get; set; }
    }
}