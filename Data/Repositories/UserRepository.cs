using System.Collections.Generic;
using System.Linq;
using BusinessEntities;
using Common;
using Data.Indexes;
using Raven.Abstractions.Data;
using Raven.Client;

namespace Data.Repositories
{
    [AutoRegister]
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IDocumentSession _documentSession;

        public UserRepository(IDocumentSession documentSession) : base(documentSession)
        {
            _documentSession = documentSession;
        }

        public IEnumerable<User> Get(UserTypes? userType = null, string name = null, string email = null)
        {
            var query = _documentSession.Advanced.DocumentQuery<User, UsersListIndex>();

            var hasFirstParameter = false;
            if (userType != null)
            {
                query = query.WhereEquals("Type", (int)userType);
                hasFirstParameter = true;
            }

            if (name != null)
            {
                if (hasFirstParameter)
                {
                    query = query.AndAlso();
                }
                else
                {
                    hasFirstParameter = true;
                }
                query = query.Where($"Name:*{name}*");
            }

            if (email != null)
            {
                if (hasFirstParameter)
                {
                    query = query.AndAlso();
                }
                query = query.WhereEquals("Email", email);
            }
            return query.ToList();
        }

        public void DeleteAll()
        {
            base.DeleteAll<UsersListIndex>();
        }

        public IEnumerable<User> GetUsersByTag(string tag)
        {

            if (string.IsNullOrEmpty(tag))
            {
                // Query for users whose tags are either null or an empty array
                var query = _documentSession.Query<User, UsersListIndex>()
                                            .Where(x => x.Tags == null || !x.Tags.Any());  // Handle both null and empty arrays

                return query.ToList();  // Return the list of users with no tags
            }

            // If a specific tag is provided, return users with that tag.
            var tagQuery = _documentSession.Advanced.DocumentQuery<User, UsersListIndex>()
                                           .Where($"Tags:{tag}");  // Query for users with the specific tag
            return tagQuery.ToList();  // Return users with the specified tag

        }
    }
}