// Copyright (c) FireGiant.  All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireGiant.Identity.Extensions;
using Microsoft.WindowsAzure.Storage.Table;

namespace FireGiant.Identity.AzureTableStorage
{
    public partial class AzureUserStore<TUser> // : IUserAccessStore<TUser>
    {
        public async Task<IEnumerable<UserAccess>> GetAccess(TUser user)
        {
            var query = new TableQuery<AzureUserAccess>()
                .Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, user.Id),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThan, "access")
                    )
                );

            TableContinuationToken token = null;

            var accesses = new List<UserAccess>();

            do
            {
                var result = await this.QueryAsync(query, token);

                accesses.AddRange(result.Results.Select(a => new UserAccess { IP = a.IP, UserAgent = a.UserAgent, Reason = a.Reason, When = a.When }));

                token = result.ContinuationToken;
            } while (token != null);

            return accesses;
        }

        public async Task AddAccess(TUser user, UserAccess access)
        {
            var a = new AzureUserAccess(user, access.IP, access.UserAgent, access.Reason, access.When);

            var op = TableOperation.Insert(a);

            var result = await this.ExecuteAsync(op);

            // TODO: should bad result error or just ignore the failure?
        }
    }
}