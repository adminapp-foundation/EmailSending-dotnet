// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using Microsoft.Extensions.Configuration;

namespace AdminApp.Extensions.EmailSending.Configuration
{
    internal sealed class EmailSendingConfiguration
    {
        public IConfiguration Configuration { get; }

        public EmailSendingConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
