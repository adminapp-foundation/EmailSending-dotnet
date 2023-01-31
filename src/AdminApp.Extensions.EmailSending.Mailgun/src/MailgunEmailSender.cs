// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

using RestSharp.Authenticators;
using RestSharp;
using System.Text.Json;

namespace AdminApp.Extensions.EmailSending.Mailgun
{
    internal sealed class MailgunEmailSender : IEmailSender
    {
        private readonly string _name;

        internal MailgunOptions Options { get; set; }

        public MailgunEmailSender(string name, MailgunOptions options)
        {
            _name = name;
            Options = options;
        }

        public bool IsEnabled(string? fromEmail)
        {
            var apiKey = GetApiKey(fromEmail);
            if (string.IsNullOrEmpty(apiKey))
            {
                apiKey = Options.ApiKey;
            }

            return apiKey != null;
        }

        public async Task<EmailSendingResult> SendAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default)
        {
            EmailAddress? fromEmailAddress = emailMessage.FromEmailAddress ?? Options.FromEmailAddress;
            if (!IsEnabled(fromEmailAddress?.Email))
            {
                return new EmailSendingResult(false, null, $"Email sending is disabled for category '{_name}'.");
            }

            if (fromEmailAddress is null)
            {
                return new EmailSendingResult(false, null, "Please provide valid 'from' email.");
            }

            var apikey = GetApiKey(fromEmailAddress.Email);
            if (string.IsNullOrWhiteSpace(apikey))
            {
                return new EmailSendingResult(false, null, "API key is not set properly.");
            }

            var fromEmailParts = fromEmailAddress.Email.Split("@");
            var domainName = fromEmailParts.Length == 2 ? fromEmailParts[1].Trim() : null;
            if (string.IsNullOrWhiteSpace(domainName))
            {
                return new EmailSendingResult(false, null, "Please provide valid 'from' email.");
            }

            using (var client = new RestClient("https://api.mailgun.net/v3") { Authenticator = new HttpBasicAuthenticator("api", apikey) })
            {
                var request = new RestRequest();

                request.AddParameter("domain", domainName, ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";

                // from
                request.AddParameter("from", $"{fromEmailAddress.Name} <{fromEmailAddress.Email}>");

                // subject
                request.AddParameter("subject", emailMessage.Subject);

                // text
                request.AddParameter("text", emailMessage.PlainText);

                // html
                request.AddParameter("html", emailMessage.HtmlText);

                // to
                foreach (var emailAddress in emailMessage.ToEmailAddresses)
                {
                    request.AddParameter("to", MailgunEmailSender.ToEmailString(emailAddress));
                }

                // cc
                if (emailMessage.CcEmailAddresses != null && emailMessage.CcEmailAddresses.Any())
                {
                    foreach (var emailAddress in emailMessage.CcEmailAddresses)
                    {
                        request.AddParameter("cc", MailgunEmailSender.ToEmailString(emailAddress));
                    }
                }

                // bcc
                if (emailMessage.BccEmailAddresses != null && emailMessage.BccEmailAddresses.Any())
                {
                    foreach (var emailAddress in emailMessage.BccEmailAddresses)
                    {
                        request.AddParameter("bcc", MailgunEmailSender.ToEmailString(emailAddress));
                    }
                }

                // attachment
                if (emailMessage.Attachments != null && emailMessage.Attachments.Any())
                {
                    foreach (var attachment in emailMessage.Attachments)
                    {
                        request.AddFile("attachment", attachment.ContentBytes, attachment.FileName, attachment.ContentType);
                    }
                }

                // inline
                if (emailMessage.InlineImages != null && emailMessage.InlineImages.Any())
                {
                    foreach (var attachment in emailMessage.InlineImages)
                    {
                        request.AddFile("inline", attachment.ContentBytes, attachment.FileName, attachment.ContentType);
                    }
                }

                // tracking
                if (emailMessage.ClickTrackingEnabled != null)
                {
                    request.AddParameter("o:tracking", !emailMessage.ClickTrackingEnabled.Value);
                }

                // deliverytime
                if (emailMessage.SendAt != null)
                {
                    // Supply RFC 2822#section-3.3 (See https://www.rfc-editor.org/rfc/rfc2822.html#section-3.3.) or Unix epoch time to schedule your message.
                    //  "Fri, 14 Oct 2011 23:10:10 -0000"
                    // request.AddParameter("o:deliverytime", emailMessage.DeliveryTime.Value.ToUniversalTime().ToString("ddd, d MMM yyyy HH:mm:ss zzz", CultureInfo.InvariantCulture));
                    request.AddParameter("o:deliverytime", emailMessage.SendAt.Value);
                }

                // tag
                if (emailMessage.Tags != null && emailMessage.Tags.Any())
                {
                    foreach (var Tag in emailMessage.Tags)
                    {
                        request.AddParameter("o:tag", Tag);
                    }
                }

                // recipient-variables
                if (emailMessage.RecipientSubstitutions != null && emailMessage.RecipientSubstitutions.Any())
                {
                    string recipientVariablesJsonString = JsonSerializer.Serialize(emailMessage.RecipientSubstitutions, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
                    });

                    request.AddParameter("recipient-variables", recipientVariablesJsonString);
                }

                var response = await client.PostAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(response.Content))
                    {
                        var successResult = JsonSerializer.Deserialize<MailgunSuccessResult>(response.Content, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return new EmailSendingResult(true, successResult?.Id);
                    }

                    return new EmailSendingResult(true, null);

                }

                return new EmailSendingResult(false, $"StatusCode: {response.StatusCode}", response.Content);
            }
        }

        private string? GetApiKey(string? fromEmail)
        {
            if (string.IsNullOrEmpty(fromEmail))
            {
                return null;
            }

            var apiKey = Options.ApiKey;
            if (Options.FromEmailRules != null && Options.FromEmailRules.Any())
            {
                var rules = Options.FromEmailRules;
                foreach (var rule in rules)
                {
                    if (rule.FromEmailAddress.Email.ToLowerInvariant() == fromEmail.ToLowerInvariant())
                    {
                        apiKey = rule.FromEmailAddress.Email;
                        break;
                    }
                }
            }

            return apiKey;
        }

        private static string ToEmailString(EmailAddress emailAddress)
        {
            return !string.IsNullOrEmpty(emailAddress.Name) ? $"{emailAddress.Name} <{emailAddress.Email}>" : emailAddress.Email;
        }

        public class MailgunSuccessResult
        {
            public string? Id { get; set; }
            public string? Message { get; set; }
        }
    }
}
