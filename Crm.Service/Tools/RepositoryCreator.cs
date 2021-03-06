﻿
using amocrm.library.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace amocrm.library.Tools
{
    internal abstract class RepositoryCreator : IRepositoryCreator
    {
        protected ICrmProvider provider;
        protected ILogger logger;

        public IQueryableRepository<T> GetRepository<T>()
        {
            return (IQueryableRepository<T>)GetObject<T>();
        }

        protected abstract Object GetObject<T>();

        protected Object CreateObject<TEntity>(Type repositoryType, params object[] @params)
        {
            var genericRepositoryType = repositoryType.MakeGenericType(typeof(TEntity));

            return (IQueryableRepository<TEntity>)Activator.CreateInstance(genericRepositoryType, @params);
        }
    }


    internal class LoggedRepositoryCreator : RepositoryCreator
    {
        public LoggedRepositoryCreator(ICrmProvider provider, ILogger logger)
        {
            this.provider = provider;
            this.logger = logger;
        }

        protected override Object GetObject<T>()
        {
            var crmRepository = CreateObject<T>(typeof(CrmRepositoty<>), this.provider);
            var loggedRepository = CreateObject<T>(typeof(LoggedRepositotyDecorator<>), new object[] { crmRepository, logger });

            return loggedRepository;
        }
    }

    internal class BasicRepositoryCreator : RepositoryCreator
    {
        public BasicRepositoryCreator(ICrmProvider provider)
        {
            this.provider = provider;
        }

        protected override object GetObject<T>()
        {
            return CreateObject<T>(typeof(CrmRepositoty<>), this.provider);
        }
    }
}
