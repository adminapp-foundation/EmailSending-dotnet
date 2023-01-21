// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using Microsoft.Extensions.Options;

namespace AdminApp.Extensions.EmailSending
{
    internal sealed class StaticOptionsMonitor : IOptionsMonitor<EmailSenderOptions>
    {
        public StaticOptionsMonitor(EmailSenderOptions currentValue)
        {
            CurrentValue = currentValue;
        }

        public IDisposable? OnChange(Action<EmailSenderOptions, string> listener) => null;

        public EmailSenderOptions Get(string? name) => CurrentValue;

        public EmailSenderOptions CurrentValue { get; }
    }
}
