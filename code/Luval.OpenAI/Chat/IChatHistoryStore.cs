using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.OpenAI.Chat
{
    public interface IChatHistoryStore
    {
        Task PersistAsync(ChatHistory chatHistory);
        Task<ChatHistory> GetByIdAsync(string id);
        Task<IEnumerable<ChatHistory>> GetBySessionIdAsync(string sessionId);
        Task<IEnumerable<ChatHistory>> GetByUserIdAsync(string userId);
        Task<IEnumerable<ChatHistory>> GetByDateRangeAsync(DateTime utcStart, DateTime utcEnd);
        Task<IEnumerable<ChatHistory>> GetByUserIdDateRangeAsync(string userId, DateTime utcStart, DateTime utcEnd);

    }
}
