// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending
{
    public class EmailSendingResult
    {
        public EmailSendingResult(bool isSuccess, string? referenceId = null, string? message = null)
        {
            IsSuccess = isSuccess;
            ReferenceId = referenceId;
            Message = message;
        }

        public bool IsSuccess { get; }
        public string? ReferenceId { get; } = null;
        public string? Message { get; } = null;
    }
}
