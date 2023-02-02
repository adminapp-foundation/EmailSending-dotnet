// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using Microsoft.Extensions.Options;

namespace AdminApp.Extensions.EmailSending
{
    internal sealed class StaticOptionsMonitor : IOptionsMonitor<EmailSendingOptions>
    {
        public StaticOptionsMonitor(EmailSendingOptions currentValue)
        {
            CurrentValue = currentValue;
        }

        public IDisposable? OnChange(Action<EmailSendingOptions, string> listener) => null;

        public EmailSendingOptions Get(string? name) => CurrentValue;

        public EmailSendingOptions CurrentValue { get; }
    }
}
