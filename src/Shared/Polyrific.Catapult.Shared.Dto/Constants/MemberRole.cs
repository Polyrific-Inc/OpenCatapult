// Copyright (c) Polyrific, Inc 2018. All rights reserved.

namespace Polyrific.Catapult.Shared.Dto.Constants
{
    public static class MemberRole
    {
        public const string All = "All";
        public const string Owner = "Owner";
        public const string Maintainer = "Maintainer";
        public const string Contributor = "Contributor";
        public const string Member = "Member";

        public const int OwnerId = 1;
        public const int MaintainerId = 2;
        public const int ContributorId = 3;
        public const int MemberId = 4;

        public static int GetMemberRoleId(string name)
        {
            switch (System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower()))
            {
                case Owner: return OwnerId;
                case Maintainer: return MaintainerId;
                case Contributor: return ContributorId;
                case Member: return MemberId;
                default: return 0;
            }
        }
    }
}
