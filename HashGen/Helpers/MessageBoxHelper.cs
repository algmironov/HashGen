using System.Collections.Generic;
using System.Threading.Tasks;

using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace HashGen.Helpers
{
    public static class MessageBoxHelper
    {
        private static readonly Dictionary<MessageBoxType, string> _messageBoxTitle = new()
        {
            { MessageBoxType.Error, "🚨 Error" },
            { MessageBoxType.Success, "✅ Success" }
        };

        public static async Task ShowMessage(string message, MessageBoxType messageBoxType)
        {
            var messageBox = MessageBoxManager
                        .GetMessageBoxStandard(_messageBoxTitle[messageBoxType], $"{message}", ButtonEnum.Ok);
            await messageBox.ShowAsync();
        }

    }

    public enum MessageBoxType
    {
        Error,
        Success
    }
}
