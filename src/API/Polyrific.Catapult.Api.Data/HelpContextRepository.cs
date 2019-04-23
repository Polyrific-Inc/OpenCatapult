// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Polyrific.Catapult.Api.Core.Entities;
using Polyrific.Catapult.Api.Core.Repositories;
using Polyrific.Catapult.Shared.Common.Interface;

namespace Polyrific.Catapult.Api.Data
{
    public class HelpContextRepository : BaseRepository<HelpContext>, IHelpContextRepository
    {
        private readonly ITextWriter _textWriter;

        public HelpContextRepository(CatapultDbContext dbContext, ITextWriter textWriter) : base(dbContext)
        {
            _textWriter = textWriter;
        }

        public override async Task<IEnumerable<HelpContext>> GetBySpec(ISpecification<HelpContext> spec, CancellationToken cancellationToken = default)
        {
            var result = await base.GetBySpec(spec, cancellationToken);

            foreach (var item in result)
            {
                var folderPath = Path.Combine(AppContext.BaseDirectory, "HelpContexts", item.Section);
                var fileName = !string.IsNullOrEmpty(item.SubSection) ? $"{item.SubSection}.txt" : "Default.txt";

                item.Text = await _textWriter.Read(folderPath, fileName);
            }

            return result;
        }
    }
}
