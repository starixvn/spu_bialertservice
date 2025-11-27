namespace Fanex.NAP.CTS.Core.Service
{
    using Fanex.NAP.CTS.Core.BaseClasses;
    using System.Collections.Generic;
    using Fanex.Data.Repository;

    public class CriteriaService : ICriteriaService
    {
        private readonly IDynamicRepository _repository;

        public CriteriaService()
        {
            _repository = new DynamicRepository();
        }

        public T Get<T>(ICriteria criteria)
        {
            return _repository.Get<T>(criteria);
        }

        public int Execute(INonQueryCommand command)
        {
            return _repository.Execute(command);
        }

        public IEnumerable<T> Fetch<T>(ICriteria criteria)
        {
            return _repository.Fetch<T>(criteria);
        }

        public (IEnumerable<TFirst> firstResultSet, IEnumerable<TSecond> secondResultSet) Fetch<TFirst, TSecond>(ICriteria criteria)
        {
            return _repository.FetchMultiple<TFirst, TSecond>(criteria);
        }

        public (IEnumerable<TFirst> firstResultSet, IEnumerable<TSecond> secondResultSet, IEnumerable<TThird> thirdResultSet) Fetch<TFirst, TSecond, TThird>(ICriteria criteria)
        {
            return _repository.FetchMultiple<TFirst, TSecond, TThird>(criteria);
        }

        public DBResult UpdateData(CommandBase criteria, string prefix)
        {
            _repository.Execute(criteria);

            return new DBResult
            {
                ErrorCode = criteria.ErrCode,              
                OutParam = criteria.GetOutParam()
            };
        }
    }
}
