// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto.ManagedFile
{
    public class ManagedFileDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}
