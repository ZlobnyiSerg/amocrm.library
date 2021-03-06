﻿using amocrm.library.Extensions;
using amocrm.library.Interfaces;
using amocrm.library.Models;
using amocrm.library.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace amocrm.library
{
    public partial class CrmRepositoty<T> : IQueryableRepository<T> where T : EntityCore, new()
    {
        public IQueryGenerator QueryGenerator { get; set; } = new QueryGenerator();
        public ICrmProvider Provider { get; }

        private readonly IValidationRulesFactory<T> validatingRulesFactory = new ValidateRulesManager().GetFactory<T>();
        private readonly DtoModelBuilder<T> dtoBuilder;

        public CrmRepositoty(ICrmProvider crmProvider)
        {
            this.Provider = crmProvider;
            dtoBuilder = new DtoModelBuilder<T>(Provider as ICrmData);
        }
        public CrmRepositoty(ICrmProvider crmProvider, IQueryGenerator generator)
            : this(crmProvider)
        {
            this.QueryGenerator = generator;
        }

        public IEnumerable<T> Execute()
        {
            var result = new List<T>();

            var task = System.Threading.Tasks.Task.Run<IEnumerable<T>>(
                    async () => await this.ExecuteAsync().ConfigureAwait(false)
                ).GetAwaiter();
            result = task.GetResult().ToList();

            return result;;
        }


        public async Task<IEnumerable<T>> ExecuteAsync()
        {
            var client = await Provider.GetClient();
            var endPoint = Provider.GetEndPoint<T>();

            var query = await QueryGenerator.Generate();

            var responseString = await client.GetResultAsync(endPoint + query).ConfigureAwait(false);

            if (string.IsNullOrEmpty(responseString)) return new List<T>();

            return CreateGetResult(responseString);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var array = Execute();
            return array.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        public async Task<IEnumerable<int>> UpdateAsync(T element)
        {
            return await UpdateAsync(new List<T> { element });
        }

        public async Task<IEnumerable<int>> UpdateAsync(IEnumerable<T> elements)
        {
            var client = await Provider.GetClient();
            var endPoint = Provider.GetEndPoint<T>();

            if (!isModelValid(elements, validatingRulesFactory.CreateUpdate()))
                    GenerateException(elements, validatingRulesFactory.CreateUpdate());

            SetUpdateDateTime(elements);

            var dto = await dtoBuilder.GetUpdateModel(elements);

            var responseString = await client.PostResultAsync(endPoint, dto).ConfigureAwait(false);

            if (string.IsNullOrEmpty(responseString)) return new List<int>();

            return CreatePostResult(responseString);
        }

        public async Task<IEnumerable<int>> AddAsync(T element)
        {
            return await AddAsync(new List<T> { element });
        }

        public async Task<IEnumerable<int>> AddAsync(IEnumerable<T> elements)
        {
            var client = await Provider.GetClient();
            var endPoint = Provider.GetEndPoint<T>();

            if (!isModelValid(elements, validatingRulesFactory.CreateAdd()))
                GenerateException(elements, validatingRulesFactory.CreateAdd());

            var dto = await dtoBuilder.GetAddModel(elements);

            var responseString = await client.PostResultAsync(endPoint, dto).ConfigureAwait(false);

            if (string.IsNullOrEmpty(responseString)) return new List<int>();

            return CreatePostResult(responseString);
        }

        public async Task<T> FindByIdAsync(int id)
        {
            var client = await Provider.GetClient();
            var endPoint = Provider.GetEndPoint<T>();

            var responseString = await client.GetResultAsync(endPoint + "?id="+id).ConfigureAwait(false);

            return string.IsNullOrEmpty(responseString) ? null : CreateGetResult(responseString).FirstOrDefault();
        }
    }
}