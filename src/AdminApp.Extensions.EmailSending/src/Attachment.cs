// Use of this source code is governed by an MIT-style license that can be found in the LICENSE.txt file at the repository root.

namespace AdminApp.Extensions.EmailSending
{
    public class Attachment
    {
        public Attachment(byte[] contentBytes, string fileName, string contentType)
        {
            ContentBytes = contentBytes;
            FileName = Path.GetFileName(fileName); ;
            ContentType = contentType;
        }

        public byte[] ContentBytes { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
