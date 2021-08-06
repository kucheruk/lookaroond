using System.Threading.Tasks;
using lookaroond.Controllers;

namespace lookaroond.DB.Commands
{
    public class StoreAccessTokenCommand
    {
        private readonly DbClient _db;

        public StoreAccessTokenCommand(DbClient db)
        {
            _db = db;
        }

        public async Task StoreToken(SlackOauthResponse response)
        {
            await _db.Access.InsertOneAsync(new SlackAccessToken(response));
        }
    }
}