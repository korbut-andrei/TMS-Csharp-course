using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerChoiceBackend.Tests.MockObjects
{
    internal class FormFileMock : IFormFile
    {
        private readonly byte[] _fileContents;

        public FormFileMock(byte[] fileContents, string fileName)
        {
            _fileContents = fileContents;
            FileName = fileName;
            Name = fileName;
        }

        public string ContentDisposition => throw new NotImplementedException();
        public string ContentType => "application/octet-stream";
        public IHeaderDictionary Headers => new HeaderDictionary();
        public long Length => _fileContents.Length;
        public string FileName { get; }
        public string Name { get; }
        public IFormCollection FormCollection => throw new NotImplementedException();
        public Stream OpenReadStream() => new MemoryStream(_fileContents);
        public void CopyTo(Stream target) => target.Write(_fileContents, 0, _fileContents.Length);
        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            CopyTo(target); 
            return Task.CompletedTask;
        }
    }
}
